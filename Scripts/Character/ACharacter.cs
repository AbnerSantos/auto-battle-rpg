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
    protected readonly GameMap GameMap;
    private readonly ICharacterClassDelegate _characterClass;
    
    private int _hp;
    private int _movLeft;

    public event Action<List<ACharacter>>? Died;

    public DiceRoll Atk => _characterClass.Atk;
    public DiceRoll Def => _characterClass.Def;
    public int MaxHp => _characterClass.MaxHp;
    public int Range => _characterClass.Range;
    public int MaxMovement => _characterClass.Movement;
    public char Symbol => _characterClass.Symbol;
    public Tile? CurrentTile { get; private set; }
    public abstract List<ACharacter> AvailableTargets { get; }
    public abstract List<ACharacter> Team { get; }
    public int? X => CurrentTile?.X;
    public int? Y => CurrentTile?.Y;
    public bool IsAlive => Hp > 0;
    public int Hp
    {
        get => _hp;
        private set => _hp = Math.Clamp(value, 0, MaxHp);
    }

    protected ACharacter(GameMap gameMap, string name, ICharacterClassDelegate characterClass)
    {
        GameMap = gameMap;
        _characterClass = characterClass;
        Hp = _characterClass.MaxHp;
        BehaviorTree = _characterClass.SetupBehaviorTree(new RpgBtData(GameMap, this));
        Name = name;
        Pathfinder = characterClass.GeneratePathfinder(gameMap);
    }

    public bool TryProcessTurn()
    {
        if (!IsAlive || CurrentTile == null) return false;

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

    public void TryDamage(DiceRoll roll, ACharacter attacker)
    {
        DiceResult rawDmg = roll.Roll();
        _characterClass.AttackQuote(attacker, this, roll, rawDmg);
        
        DiceResult defense = Def.Roll();
        _characterClass.DefenseQuote(this, defense);
        
        int lostHp = rawDmg.Total - defense.Total;
        if (lostHp > 0)
        {
            Console.WriteLine($"{Name} loses {rawDmg.Total - defense.Total} Hp!");

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
            int movCost = _characterClass.GetMovementCost(GameMap[x, y]);
            
            if (_movLeft < movCost) continue;
            
            _movLeft -= movCost;
            MoveTo(x, y);

            if (!IsWithinRange(target)) continue;
            
            GameMap.DisplayMap();
            return;
        }
        GameMap.DisplayMap();
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
}