using AutoBattleRPG.Scripts.Character;
using AutoBattleRPG.Scripts.Utility;

namespace AutoBattleRPG.Scripts.BehaviorTree;

public class IsWithinTargetRangeSelectorNode : ABtConditionalSelectorNode<RpgBtData>
{
    public IsWithinTargetRangeSelectorNode(ABtNode<RpgBtData> ifFalse, ABtNode<RpgBtData> ifTrue) : base(ifFalse, ifTrue)
    {
    }

    protected override bool CheckCondition()
    {
        if (BtData == null) throw new Exceptions.BtDataIsNull();
        
        if (BtData.Character.CurrentTile == null) throw new Exceptions.CharacterNotInGameMap();

        List<ACharacter> targetsByAscendingDistance = BtData.Character.AvailableTargetsAscendingDistance();

        if (targetsByAscendingDistance.Count == 0) throw new Exceptions.NoValidTargets();
        
        ACharacter nearestTarget = targetsByAscendingDistance[0];
        BtData.CurrentTarget = nearestTarget;
        BtData.TargetsByAscendingDistance = targetsByAscendingDistance;
        
        return BtData.Character.IsWithinRange(nearestTarget);
    }
}