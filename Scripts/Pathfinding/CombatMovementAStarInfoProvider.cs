using AutoBattleRPG.Scripts.Character.Classes;
using AutoBattleRPG.Scripts.Stage;

namespace AutoBattleRPG.Scripts.Pathfinding;

public abstract class CombatMovementAStarInfoProvider : IAStarInfoProviderDelegate
{
    protected readonly GameMap GameMap;

    protected CombatMovementAStarInfoProvider(GameMap gameMap)
    {
        GameMap = gameMap;
    }

    public abstract List<PathNode> GetNeighborhood(PathNode currentNode, PathNode targetNode);
    public abstract int Distance(PathNode currentNode, PathNode targetNode);
    protected abstract bool IsPathFree(PathNode node, PathNode targetNode);
}