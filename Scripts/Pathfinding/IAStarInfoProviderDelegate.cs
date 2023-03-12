namespace AutoBattleRPG.Scripts.Pathfinding;

public interface IAStarInfoProviderDelegate
{
    public int GetMovementCost(int x, int y);
    public List<PathNode> GetNeighborhood(PathNode currentNode, PathNode targetNode);
    public int Distance(PathNode currentNode, PathNode targetNode);
}