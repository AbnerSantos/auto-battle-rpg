using AutoBattleRPG.Scripts.Character;
using AutoBattleRPG.Scripts.Character.Classes;
using AutoBattleRPG.Scripts.Stage;

namespace AutoBattleRPG.Scripts.Pathfinding;

public abstract class CombatMovementAStarInfoProvider : IAStarInfoProviderDelegate
{
    protected readonly GameMap GameMap;
    private readonly ACharacter _character;

    protected CombatMovementAStarInfoProvider(GameMap gameMap, ACharacter character)
    {
        GameMap = gameMap;
        _character = character;
    }

    public int GetMovementCost(int x, int y)
    {
        return _character.GetMovementCost(GameMap[x, y]);
    }

    public abstract List<PathNode> GetNeighborhood(PathNode currentNode, PathNode targetNode);
    public abstract int Distance(PathNode currentNode, PathNode targetNode);
    protected abstract bool IsPathFree(PathNode node, PathNode targetNode);
}