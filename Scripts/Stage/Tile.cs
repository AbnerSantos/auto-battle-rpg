using AutoBattleRPG.Scripts.Character;

namespace AutoBattleRPG.Scripts.Stage;

public class Tile
{
    public readonly int X;
    public readonly int Y;
    
    private readonly GameMap _gameMap;
        
    public ACharacter? Character { get; set; }
    public bool IsOccupied => Character != null;
    public int MovCost => 1;

    
    public Tile(int x, int y, GameMap gameMap)
    {
        X = x;
        Y = y;
        _gameMap = gameMap;
    }

    public void Occupy(ACharacter character)
    {
        Character = character;
        _gameMap.AvailableTiles.Remove(this);
        switch (character)
        {
            case PlayerCharacter player:
                _gameMap.Player = player;
                break;
            case EnemyCharacter enemy:
                _gameMap.Enemy = enemy;
                break;
        }
    }

    public void Free()
    {
        Character = null;
        _gameMap.AvailableTiles.Add(this);
    }

    public override string ToString()
    {
        return " · ";
    }

    public static int Distance(Tile t1, Tile t2)
    {
        // Manhattan Distance
        return Math.Abs(t1.X - t2.X) - Math.Abs(t1.Y - t2.Y);
    }
}