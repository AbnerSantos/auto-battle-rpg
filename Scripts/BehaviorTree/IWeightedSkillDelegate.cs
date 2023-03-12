namespace AutoBattleRPG.Scripts.BehaviorTree;

public interface IWeightedSkillDelegate
{
    public int GetWeight(RpgBtData btData);
}