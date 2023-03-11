using AutoBattleRPG.Scripts.Stage;

namespace AutoBattleRPG.Scripts.Pathfinding;

public class PathMap : AMap<PathNode>
{
    public sealed override PathNode[,] Grid { get; protected set; }

    public PathMap(int width, int height)
    {
        Grid = new PathNode[width, height];
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                PathNode newNode = new PathNode(this, i, j);
                Grid[i, j] = newNode;
            }
        }
    }
}