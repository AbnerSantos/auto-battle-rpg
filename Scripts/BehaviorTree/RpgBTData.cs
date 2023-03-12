using AutoBattleRPG.Scripts.Character;
using AutoBattleRPG.Scripts.Stage;

namespace AutoBattleRPG.Scripts.BehaviorTree;

public class RpgBtData
{
    public readonly ACharacter Character;
    public readonly GameMap GameMap;
    public ACharacter? CurrentTarget { get; set; } = null;
    public List<ACharacter> TargetsByAscendingDistance { get; set; } = new();

    public RpgBtData(GameMap gameMap, ACharacter character)
    {
        GameMap = gameMap;
        Character = character;
    }
}