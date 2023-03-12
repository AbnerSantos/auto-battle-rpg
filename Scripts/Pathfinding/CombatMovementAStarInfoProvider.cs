using AutoBattleRPG.Scripts.Character.Classes;
using AutoBattleRPG.Scripts.Stage;

namespace AutoBattleRPG.Scripts.Pathfinding;

public abstract class CombatMovementAStarInfoProvider : IAStarInfoProviderDelegate
{
    protected readonly GameMap GameMap;
    private readonly ICharacterClassDelegate _characterClass;

    protected CombatMovementAStarInfoProvider(GameMap gameMap, ICharacterClassDelegate characterClass)
    {
        GameMap = gameMap;
        _characterClass = characterClass;
    }

    public int GetMovementCost(int x, int y)
    {
        return _characterClass.GetMovementCost(GameMap[x, y]);
    }

    public abstract List<PathNode> GetNeighborhood(PathNode currentNode, PathNode targetNode);
    public abstract int Distance(PathNode currentNode, PathNode targetNode);
    protected abstract bool IsPathFree(PathNode node, PathNode targetNode);
}