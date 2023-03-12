using AutoBattleRPG.Scripts.Character;
using AutoBattleRPG.Scripts.Utility;

namespace AutoBattleRPG.Scripts.Stage;

public class Tile
{
    public readonly int X;
    public readonly int Y;
    public readonly Terrain.TerrainType Terrain;

    private readonly GameMap _gameMap;

    public ACharacter? Character { get; set; }
    public bool IsOccupied => Character != null;

    public Tile(int x, int y, Terrain.TerrainType terrain, GameMap gameMap)
    {
        X = x;
        Y = y;
        _gameMap = gameMap;
        Terrain = terrain;
    }

    public void Occupy(ACharacter character)
    {
        Character = character;
        _gameMap.AvailableTiles.Remove(this);

        if (_gameMap.Characters.Contains(character)) return;
        
        switch (character)
        {
            case PlayerCharacter player:
                _gameMap.PlayerTeam.Add(player);
                break;
            case EnemyCharacter enemy:
                _gameMap.EnemyTeam.Add(enemy);
                break;
        }
        _gameMap.Characters.Add(character);
    }

    public void Free()
    {
        Character = null;
        _gameMap.AvailableTiles.Add(this);
    }

    public void DisplayTile()
    {
        if (IsOccupied)
        {
            ConsoleColor color = Character is PlayerCharacter ? ConsoleColor.Blue : ConsoleColor.Red;
            Terrain.Draw(Character!.Symbol, color);
        }
        else
        {
            Terrain.Draw();
        }
        Console.ResetColor();
    }

    public static int ManhattanDistance(Tile t1, Tile t2)
    {
        return Heuristics.ManhattanDistance((t1.X, t1.Y), (t2.X, t2.Y));
    }

    public static int ChebyshevDistance(Tile t1, Tile t2)
    {
        return Heuristics.ChebyshevDistance((t1.X, t1.Y), (t2.X, t2.Y));
    }
}