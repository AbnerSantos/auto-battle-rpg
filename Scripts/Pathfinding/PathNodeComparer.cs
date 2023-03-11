namespace AutoBattleRPG.Scripts.Pathfinding;

public class PathNodeComparer : IComparer<PathNode>
{
    public int Compare(PathNode? x, PathNode? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (ReferenceEquals(null, y)) return 1;
        if (ReferenceEquals(null, x)) return -1;
        return x.FCost.CompareTo(y.FCost);
    }
}