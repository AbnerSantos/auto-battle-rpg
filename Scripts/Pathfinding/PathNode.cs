using AutoBattleRPG.Scripts.Utility;

namespace AutoBattleRPG.Scripts.Pathfinding;

public class PathNode
{
    public readonly int X;
    public readonly int Y;
    
    public PathNode? PrevNode = null;
    private readonly PathMap _pathMap;
    
    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost => GCost + HCost;
    
    public PathNode(PathMap pathMap, int x, int y)
    {
        X = x;
        Y = y;
        _pathMap = pathMap;
    }

    public override string ToString()
    {
        return ("(" + X + "," + Y + ")" + " hCost = " + HCost + " gCost = " + GCost + " fCost = " + FCost);
    }
    
    public List<PathNode> FourNeighbors(Func<PathNode, bool> isValidPathDelegate)
    {
        List<PathNode> neighbors = new ();
        if(X + 1 < _pathMap.Width && isValidPathDelegate(_pathMap[X + 1, Y])) // Right
            neighbors.Add(_pathMap[X + 1, Y]);
        if(Y + 1 < _pathMap.Height && isValidPathDelegate(_pathMap[X, Y + 1])) // Up
            neighbors.Add(_pathMap[X, Y + 1]);
        if(Y - 1 >= 0 && isValidPathDelegate(_pathMap[X, Y - 1])) // Down
            neighbors.Add(_pathMap[X, Y - 1]);
        if(X - 1 >= 0 && isValidPathDelegate(_pathMap[X - 1, Y])) // Left
            neighbors.Add(_pathMap[X - 1, Y]);
        return neighbors;
    }

    public List<PathNode> EightNeighbors(Func<PathNode, bool> isValidPathDelegate)
    {
        List<PathNode> neighbors = FourNeighbors(isValidPathDelegate);
        if(X + 1 < _pathMap.Width && Y + 1 < _pathMap.Height && isValidPathDelegate(_pathMap[X + 1, Y + 1])) // Right-Up
            neighbors.Add(_pathMap[X + 1, Y + 1]);
        if(X - 1 >= 0 && Y + 1 < _pathMap.Height && isValidPathDelegate(_pathMap[X - 1, Y + 1])) // Left-Up
            neighbors.Add(_pathMap[X - 1, Y + 1]);
        if(X + 1 < _pathMap.Width && Y - 1 >= 0 && isValidPathDelegate(_pathMap[X + 1, Y - 1])) // Right-Down
            neighbors.Add(_pathMap[X + 1, Y - 1]);
        if(X - 1 >= 0 && Y - 1 >= 0 && isValidPathDelegate(_pathMap[X - 1, Y - 1])) // Left-Down
            neighbors.Add(_pathMap[X - 1, Y - 1]);
        return neighbors;
    }

    public static int ManhattanDistance(PathNode node1, PathNode node2)
    {
        return Heuristics.ManhattanDistance((node1.X, node1.Y), (node2.X, node2.Y));
    }
    
    public static int ChebyshevDistance(PathNode node1, PathNode node2)
    {
        return Heuristics.ChebyshevDistance((node1.X, node1.Y), (node2.X, node2.Y));
    }
}