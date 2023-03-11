namespace AutoBattleRPG.Scripts.Pathfinding;

public interface IAStarInfoProviderDelegate
{
    public List<PathNode> GetNeighborhood(PathNode currentNode, PathNode targetNode);
    public int Distance(PathNode currentNode, PathNode targetNode);
}