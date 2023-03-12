using AutoBattleRPG.Scripts.BehaviorTree;
using AutoBattleRPG.Scripts.Character.Classes;
using AutoBattleRPG.Scripts.Dice;
using AutoBattleRPG.Scripts.Pathfinding;
using AutoBattleRPG.Scripts.Stage;
using AutoBattleRPG.Scripts.Utility;

namespace AutoBattleRPG.Scripts.Character;

public abstract class ACharacter
{
    public readonly string Name;
    public readonly AStarPathfinder Pathfinder;
    public readonly BehaviorTree<RpgBtData> BehaviorTree;
    public readonly Dictionary<Terrain.TerrainType, int> TerrainMovModifiers = new();
    public readonly List<INamedSkill> AllSkills;
    
    protected readonly GameMap GameMap;
    
    private readonly ICharacterClassDelegate _characterClass;
    private readonly List<APassiveSkill> _passiveSkills;

    private int _hp;
    private int _mana;
    private int _movLeft;

    public event Action<List<ACharacter>>? Died;

    public DiceRoll? Def => _characterClass.Def;
    public int MaxHp => _characterClass.MaxHp;
    public int MaxMana => _characterClass.MaxMana;
    public int Range => _characterClass.Range;
    public int MaxMovement => _characterClass.Movement;
    public char Symbol => _characterClass.Symbol;
    public Tile? CurrentTile { get; private set; }
    public abstract List<ACharacter> AvailableTargets { get; }
    public abstract List<ACharacter> Team { get; }
    public bool IsAlive => Hp > 0;

    private int Hp
    {
        get => _hp;
        set => _hp = Math.Clamp(value, 0, MaxHp);
    }
    public int Mana
    {
        get => _mana;
        private set => _mana = Math.Clamp(value, 0, MaxMana);
    }

    protected ACharacter(GameMap gameMap, string name, ICharacterClassDelegate characterClass)
    {
        GameMap = gameMap;
        _characterClass = characterClass;
        Hp = _characterClass.MaxHp;
        Mana = _characterClass.MaxMana;
        BehaviorTree = _characterClass.SetupBehaviorTree(new RpgBtData(GameMap, this));
        _passiveSkills = _characterClass.SetupPassiveSkills(this);
        var pickUpSkills = _characterClass.SetupStartingPickUpSkills(this);
        var activeSkills = FetchActiveSkills();
        AllSkills = new List<INamedSkill>(activeSkills);
        AllSkills.AddRange(pickUpSkills);
        AllSkills.AddRange(_passiveSkills);
        Name = name;
        Pathfinder = characterClass.GeneratePathfinder(gameMap, this);
    }

    public bool TryProcessTurn()
    {
        if (!IsAlive || CurrentTile == null) return false;

        foreach (APassiveSkill skill in _passiveSkills) skill.Execute();

        return BehaviorTree.Execute();
    }

    public void PlaceOnMap()
    {
        int randIndex = RandomHelper.Rand.Next(0, GameMap.AvailableTiles.Count);

        MoveTo(GameMap.AvailableTiles[randIndex]);
    }

    public void ResetMovement()
    {
        _movLeft = MaxMovement;
    }

    public void TryDamage(int rawDmg, ACharacter attacker)
    {
        int defenseTotal = 0;
        if (Def != null)
        {
            DiceResult defense = Def.Roll();
            _characterClass.DefenseQuote(this, defense);
            defenseTotal = defense.Total;
        }
        
        int lostHp = rawDmg - defenseTotal;
        if (lostHp > 0)
        {
            Console.WriteLine($"{Name} loses {lostHp} Hp!");

            Hp -= lostHp;
            if (!IsAlive)
            {
                OnDeath(attacker);
                return;
            }
        }
        else
        {
            _characterClass.PerfectDefenseQuote(this);
        }
        
        Console.WriteLine($"{Name} has {Hp}/{MaxHp} HP left!");
    }

    private void OnDeath(ACharacter attacker)
    {
        Console.WriteLine($"{Name} has been slain by {attacker.Name}!");
        CurrentTile?.Free();
        CurrentTile = null;
        Died?.Invoke(Team);
    }

    private void MoveTo(int x, int y)
    {
        MoveTo(GameMap[x, y]);
    }

    private void MoveTo(Tile tile)
    {
        if (tile.IsOccupied) return;

        if (CurrentTile != null) _characterClass.MovementQuote(this, CurrentTile, tile);
        CurrentTile?.Free();
        CurrentTile = tile;
        CurrentTile.Occupy(this);
    }

    public void MoveTowardsPath(List<(int x, int y)> path, ACharacter target)
    {
        foreach ((int x, int y) in path)
        {
            int movCost = GetMovementCost(GameMap[x, y]);
            
            if (_movLeft < movCost) continue;
            
            _movLeft -= movCost;
            MoveTo(x, y);

            if (!IsWithinRange(target)) continue;
            
            GameMap.DisplayMap();
            return;
        }
        GameMap.DisplayMap();
    }

    public int GetMovementCost(Tile tile)
    {
        TerrainMovModifiers.TryGetValue(tile.Terrain, out int modifier);
        return Terrain.MovementCostPerTerrain[tile.Terrain] + modifier;
    }

    public bool IsWithinRange(ACharacter character)
    {
        if (CurrentTile == null || character.CurrentTile == null) return false;
        
        int distance = _characterClass.AttackDistance(CurrentTile, character.CurrentTile);
        return distance <= Range;
    }

    public List<ACharacter> AvailableTargetsAscendingDistance()
    {
        List<ACharacter> targets = new ();
        foreach (ACharacter target in AvailableTargets)
        {
            if (target.IsAlive || target.CurrentTile != null) targets.Add(target);
        }
        
        targets.Sort((character1, character2) => _characterClass.AttackDistance(CurrentTile!, character1.CurrentTile!)
            .CompareTo(_characterClass.AttackDistance(CurrentTile!, character2.CurrentTile!)));
        
        return targets;
    }

    public void HealMana(DiceRoll manaRecovery)
    {
        DiceResult mana = manaRecovery.Roll();
        Console.WriteLine($"{Name} concentrates and recovers {manaRecovery} = {mana} mana!");
        Console.WriteLine($"{Name} has {Mana}/{MaxMana} Mana!");
    }

    public void SpendMana(int total)
    {
        if (total <= 0) return;
        
        Mana -= total;
        Console.WriteLine($"{Name} has {Mana}/{MaxMana} Mana left!");
    }

    private List<AActiveSkill> FetchActiveSkills()
    {
        return ChildActiveSkills(BehaviorTree.RootNode!);
    }

    private List<AActiveSkill> ChildActiveSkills(ABtNode<RpgBtData?> node)
    {
        List<AActiveSkill> activeSkills = new();
        if (node is AActiveSkill active) activeSkills.Add(active);
        
        foreach (ABtNode<RpgBtData?> childNode in node.ChildNodes)
        {
           activeSkills.AddRange(ChildActiveSkills(childNode));
        }

        return activeSkills;
    }
}