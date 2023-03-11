using AutoBattleRPG.Scripts.Character.Classes;
using AutoBattleRPG.Scripts.Dice;
using AutoBattleRPG.Scripts.Stage;
using AutoBattleRPG.Scripts.Utility;

namespace AutoBattleRPG.Scripts.Character;

public abstract class ACharacter
{
    public readonly string Name;
    protected readonly GameMap GameMap;
    private readonly ICharacterClassDelegate _characterClass;
    
    private int _hp;

    public event Action<List<ACharacter>>? Died;

    public DiceRoll Atk => _characterClass.Atk;
    public DiceRoll Def => _characterClass.Def;
    public int MaxHp => _characterClass.MaxHp;
    public int Range => _characterClass.Range;
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
        Name = name;
    }

    public bool TryProcessTurn()
    {
        if (!IsAlive || CurrentTile == null || !TryGetNearestTarget(out ACharacter? target)) return false;

        ACharacter nearestTarget = target!;
        
        if (IsWithinRange(nearestTarget))
        {
            nearestTarget.TryDamage(Atk, this);
            return true;
        }
        
        if (nearestTarget.X > X)
        {
            MoveTo((int)X! + 1, (int)Y!);
        }
        else if (nearestTarget.X < X)
        {
            MoveTo((int)X! - 1, (int)Y!);
        }
        else if (nearestTarget.Y > Y)
        {
            MoveTo((int)X!, (int)Y! + 1);
        }
        else if (nearestTarget.Y < Y)
        {
            MoveTo((int)X!, (int)Y! - 1);
        }

        return true;
    }

    public void PlaceOnMap()
    {
        int randIndex = RandomHelper.Rand.Next(0, GameMap.AvailableTiles.Count);

        MoveTo(GameMap.AvailableTiles[randIndex]);
    }

    private void TryDamage(DiceRoll roll, ACharacter attacker)
    {
        DiceResult rawDmg = roll.Roll();
        Console.WriteLine($"{attacker.Name} attacks {Name} for {roll} = {rawDmg} damage!");
        
        DiceResult defense = Def.Roll();
        Console.WriteLine($"{Name} blocks {Def} = {defense} damage!");
        
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
            Console.WriteLine($"{Name} blocks all incoming damage!");
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
        MoveTo(GameMap.Grid[x, y]);
    }

    private void MoveTo(Tile tile)
    {
        if (tile.IsOccupied) return;

        CurrentTile?.Free();
        CurrentTile = tile;
        CurrentTile.Occupy(this);
    }

    private bool IsWithinRange(ACharacter character)
    {
        if (CurrentTile == null || character.CurrentTile == null) return false;
        int distance = Tile.Distance(CurrentTile, character.CurrentTile);
        return distance <= Range;
    }

    private bool TryGetNearestTarget(out ACharacter? finalTarget)
    {
        ACharacter? nearestTarget = null;
        int minDistance = int.MaxValue;
        foreach (ACharacter target in AvailableTargets)
        {
            if (!target.IsAlive || CurrentTile == null || target.CurrentTile == null) continue;
            int distance = Tile.Distance(CurrentTile, target.CurrentTile);
            
            if (distance >= minDistance) continue;
            
            minDistance = distance;
            nearestTarget = target;
        }
        
        finalTarget = nearestTarget;
        return nearestTarget != null;
    }
}