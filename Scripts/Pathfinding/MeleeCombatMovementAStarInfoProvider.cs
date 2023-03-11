using AutoBattleRPG.Scripts.Stage;

namespace AutoBattleRPG.Scripts.Pathfinding;

public class MeleeCombatMovementAStarInfoProvider : CombatMovementAStarInfoProvider
{ 
    public MeleeCombatMovementAStarInfoProvider(GameMap gameMap) : base(gameMap)
    {
    }
    
    public override List<PathNode> GetNeighborhood(PathNode currentNode, PathNode targetNode)
    {
        // Different from ranged weapon fighters, even if the melee weapon has a higher reach, it doesn't go through obstacles and other characters
        return currentNode.FourNeighbors( node => IsPathFree(node, targetNode));
    }
    protected override bool IsPathFree(PathNode node, PathNode targetNode) => !GameMap[node.X, node.Y].IsOccupied || node == targetNode;

    public override int Distance(PathNode currentNode, PathNode targetNode)
    {
        return PathNode.ManhattanDistance(currentNode, targetNode);
    }
}