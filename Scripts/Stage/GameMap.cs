using System.Text;
using AutoBattleRPG.Scripts.Character;
using AutoBattleRPG.Scripts.Utility;

namespace AutoBattleRPG.Scripts.Stage;

public class GameMap
{
    public readonly Tile[,] Grid;

    public List<ACharacter> PlayerTeam { get; } = new();
    public List<ACharacter> EnemyTeam { get; } = new();
    public List<ACharacter> Characters { get; } = new();
    public List<Tile> AvailableTiles { get; } = new();

    public GameMap(Settings settings)
    {
        Grid = new Tile[settings.GridSize.x, settings.GridSize.y];
        for (int i = 0; i < Grid.GetLength(0); i++)
        {
            for (int j = 0; j < Grid.GetLength(1); j++)
            {
                Tile newTile = new Tile(i, j, this);
                Grid[i, j] = newTile;
                AvailableTiles.Add(newTile);
            }
        }
    }

    public void DisplayMap()
    {
        StringBuilder strBuilder = new StringBuilder();
        strBuilder.Append('╔');
        strBuilder.Append('═', Grid.GetLength(0) * 3);
        strBuilder.Append('╗');
        Console.WriteLine(strBuilder.ToString());

        for (int i = 0; i < Grid.GetLength(0); i++)
        {
            Console.Write('║');

            for (int j = 0; j < Grid.GetLength(1); j++)
            {
                Grid[i, j].DisplayTile();
            }
            
            Console.WriteLine('║');
        }

        strBuilder.Clear();
        
        strBuilder.Append('╚');
        strBuilder.Append('═', Grid.GetLength(0) * 3);
        strBuilder.Append('╝');
        Console.WriteLine(strBuilder.ToString());
    }
}