using AutoBattleRPG.Scripts.Utility;

namespace AutoBattleRPG.Scripts.BehaviorTree;

public class AttackTargetInRangeNode : ABtNode<RpgBtData>
{
    public override void Execute()
    {
        if (BtData == null) throw new Exceptions.BtDataIsNull();
        if (BtData.CurrentTarget == null) throw new Exceptions.NoValidTargets();
        
        BtData.CurrentTarget.TryDamage(BtData.Character.Atk, BtData.Character);
    }
}