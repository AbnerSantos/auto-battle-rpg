using AutoBattleRPG.Scripts.Dice;
using AutoBattleRPG.Scripts.Stage;
using AutoBattleRPG.Scripts.Utility;

namespace AutoBattleRPG.Scripts.Character;

public abstract class ACharacter
{
    public readonly string Name;
    protected readonly GameMap GameMap;
    private int _hp;

    public abstract DiceRoll Atk { get; }
    public abstract int MaxHp { get; }
    public abstract int Range { get; }
    public abstract ACharacter Target { get; }
    
    public Tile? CurrentTile { get; private set; }
    public int? X => CurrentTile?.X;
    public int? Y => CurrentTile?.Y;
    public bool IsAlive => Hp > 0;
    public int Hp
    {
        get => _hp;
        private set => _hp = Math.Clamp(value, 0, MaxHp);
    }
    
    protected ACharacter(GameMap gameMap, string name)
    {
        GameMap = gameMap;
        Hp = MaxHp;
        Name = name;
    }

    public void ComputeTurn()
    {
        if (!IsAlive || !Target.IsAlive) return;
        
        if (IsWithinRange(Target))
        {
            Target.TryDamage(Atk, this);
            return;
        }
        
        if (Target.X > X)
        {
            MoveTo((int)X! + 1, (int)Y!);
        }
        else if (Target.X < X)
        {
            MoveTo((int)X! - 1, (int)Y!);
        }
        else if (Target.Y > Y)
        {
            MoveTo((int)X!, (int)Y! + 1);
        }
        else if (Target.Y < Y)
        {
            MoveTo((int)X!, (int)Y! - 1);
        }
    }

    public void PlaceOnMap()
    {
        int randIndex = RandomHelper.Rand.Next(0, GameMap.AvailableTiles.Count);

        MoveTo(GameMap.AvailableTiles[randIndex]);
    }

    private void TryDamage(DiceRoll roll, ACharacter attacker)
    {
        DiceResult result = roll.Roll();
        Console.WriteLine($"{attacker.Name} attacks {Name} for {roll} = {result} damage!");
        Hp -= result.Total;
        Console.WriteLine($"{Name} has {Hp}/{MaxHp} HP left!");
        if (!IsAlive) OnDeath(attacker);
    }

    private void OnDeath(ACharacter attacker)
    {
        Console.WriteLine($"{Name} has been slain!");
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
}