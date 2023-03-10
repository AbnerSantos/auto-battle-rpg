using AutoBattleRPG.Scripts.Stage;
using AutoBattleRPG.Scripts.Utility;

namespace AutoBattleRPG.Scripts.Character;

public abstract class ACharacter
{
    protected readonly GameMap GameMap;
    private int _hp;

    public abstract int Atk { get; }
    public abstract int MaxHp { get; }
    public abstract ACharacter Target { get; }

    public int Hp
    {
        get => _hp;
        private set => _hp = Math.Clamp(value, 0, MaxHp);
    }
    
    public Tile? CurrentTile { get; private set; }

    public int? X => CurrentTile?.X;
    public int? Y => CurrentTile?.Y;
    public bool IsAlive => Hp > 0;
    
    protected ACharacter(GameMap gameMap)
    {
        GameMap = gameMap;
        Hp = MaxHp;
    }

    public void ComputeTurn()
    {
        if (!Target.IsAlive) return;
        
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

    private void TryDamage(int dmg, ACharacter attacker)
    {
        Hp -= dmg;
        if (!IsAlive) OnDeath(attacker);
    }
    
    protected abstract void OnDeath(ACharacter attacker);

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

        return Tile.Distance(CurrentTile, character.CurrentTile) == 1;
    }
}