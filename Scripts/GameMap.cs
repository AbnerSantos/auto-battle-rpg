using System.Text;

namespace AutoBattleRPG.Scripts;

public class GameMap
{
    public readonly Tile[,] Grid;

    public GameMap(Settings settings)
    {
        Grid = new Tile[settings.GridSize.x, settings.GridSize.y];
        for (int i = 0; i < Grid.GetLength(0); i++)
        {
            for (int j = 0; j < Grid.GetLength(1); j++)
                Grid[i, j] = new Tile(i, j);
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
                strBuilder.Append(' ');
                strBuilder.Append(Grid[i, j]);
                strBuilder.Append(' ');
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