using AutoBattleRPG.Scripts.Stage;

namespace AutoBattleRPG.Scripts.Pathfinding;

public class RangedCombatMovementAStarInfoProvider : CombatMovementAStarInfoProvider
{
    private readonly int _range;
    
    public RangedCombatMovementAStarInfoProvider(GameMap gameMap, int range) : base(gameMap)
    {
        _range = range;
    }
    
    public override List<PathNode> GetNeighborhood(PathNode currentNode, PathNode targetNode)
    {
        if (PathNode.ChebyshevDistance(currentNode, targetNode) > _range)
            return currentNode.FourNeighbors(node => IsPathFree(node, targetNode));
        
        // Shoots diagonally, over obstacles and other characters
        return currentNode.EightNeighbors(node => IsPathFree(node, targetNode));
    }
    
    public override int Distance(PathNode currentNode, PathNode targetNode)
    {
        int manhattanDistance = PathNode.ManhattanDistance(currentNode, targetNode);
        int chebyshevDistance = PathNode.ChebyshevDistance(currentNode, targetNode);

        // Shoots diagonally, over obstacles and other characters
        return chebyshevDistance <= _range ? chebyshevDistance : manhattanDistance;
    }

    protected override bool IsPathFree(PathNode node, PathNode targetNode)
    {
        return PathNode.ChebyshevDistance(node, targetNode) < _range ||
        !GameMap[node.X, node.Y].IsOccupied;   
    }
}