namespace AutoBattleRPG.Scripts.BehaviorTree;

public abstract class APassiveSkill : INamedSkill
{
    public string Name { get; set; }
    public string Description { get; set; }

    protected APassiveSkill(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public abstract void Execute();
}