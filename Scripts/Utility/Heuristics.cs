namespace AutoBattleRPG.Scripts.Utility;

public static class Heuristics
{
    /// <summary>
    ///     Manhattan Distance is the sum of the absolute differences of point coordinates
    /// </summary>
    public static int ManhattanDistance((int x, int y) a, (int x, int y) b)
    {
        return Math.Abs(b.x - a.x) + Math.Abs(b.y - a.y);
    }

    /// <summary>
    ///     Chebyshev Distance is the greatest of the differences between points along any dimension
    /// </summary>
    public static int ChebyshevDistance((int x, int y) a, (int x, int y) b)
    {
        return Math.Max(Math.Abs(b.x - a.x), Math.Abs(b.y - a.y));
    }
}