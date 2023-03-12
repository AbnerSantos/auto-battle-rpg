using AutoBattleRPG.Scripts.Utility;

namespace AutoBattleRPG.Scripts.BehaviorTree;

public class WeightedSelectorNode : ABtNode<RpgBtData>
{
    public override void Execute()
    {
        int currentThreshold = 0;
        List<(int, ABtNode<RpgBtData>)> thresholds = new();
        foreach (ABtNode<RpgBtData?> childNode in ChildNodes)
        {
            if (childNode is IWeightedSkill skill) currentThreshold += skill.GetWeight();
            thresholds.Add((currentThreshold, childNode)!);
        }

        if (currentThreshold == 0) throw new Exceptions.NoValidAttack();

        int roll = RandomHelper.Rand.Next(1, currentThreshold + 1);
        foreach ((int weightThreshold, ABtNode<RpgBtData> node) in thresholds)
        {
            if (roll > weightThreshold) continue;
            
            node.Execute();
            return;
        }
    }
}