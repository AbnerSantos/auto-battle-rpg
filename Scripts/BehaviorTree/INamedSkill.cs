using AutoBattleRPG.Scripts.Utility;

namespace AutoBattleRPG.Scripts.BehaviorTree;

public interface INamedSkill
{
    public string Name { get; set; }
    public string Description { get; set; }
}
    