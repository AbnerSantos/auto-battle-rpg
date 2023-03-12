namespace AutoBattleRPG.Scripts.BehaviorTree;

public interface IWeightedSkill
{
    public IWeightedSkillDelegate? SkillWeightDelegate { get; set; }
    public int GetWeight();
}