using System.Text;
using AutoBattleRPG.Scripts.Character;
using AutoBattleRPG.Scripts.Utility;

namespace AutoBattleRPG.Scripts.Stage;

public class GameMap
{
    public readonly Tile[,] Grid;

    public ACharacter Player { get; set; } = null!;
    public ACharacter Enemy { get; set; } = null!;
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

    public override string ToString()
    {
        StringBuilder strBuilder = new StringBuilder();
        strBuilder.Append('╔');
        strBuilder.Append('═', Grid.GetLength(0) * 3);
        strBuilder.Append('╗');
        strBuilder.AppendLine();

        for (int i = 0; i < Grid.GetLength(0); i++)
        {
            strBuilder.Append('║');

            for (int j = 0; j < Grid.GetLength(1); j++)
            {
                strBuilder.Append(Grid[i, j]);
            }
            
            strBuilder.Append('║');
            strBuilder.AppendLine();
        }

        strBuilder.Append('╚');
        strBuilder.Append('═', Grid.GetLength(0) * 3);
        strBuilder.Append('╝');

        return strBuilder.ToString();
    }
}