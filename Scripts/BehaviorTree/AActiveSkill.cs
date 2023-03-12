using AutoBattleRPG.Scripts.Utility;

namespace AutoBattleRPG.Scripts.BehaviorTree;

public abstract class AActiveSkill : ABtNode<RpgBtData>, IWeightedSkill, INamedSkill
{
    public IWeightedSkillDelegate? SkillWeightDelegate { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public readonly int ManaCost;
    private readonly int _defaultWeight; 

    protected AActiveSkill(string name, string description, int manaCost, int defaultWeight = 1, IWeightedSkillDelegate? skillWeightDelegate = null)
    {
        Name = name;
        Description = description;
        ManaCost = manaCost;
        _defaultWeight = defaultWeight;
        SkillWeightDelegate = skillWeightDelegate;
    }

    public override void Execute()
    {
        if (BtData == null) throw new Exceptions.BtDataIsNull();
        if (BtData.Character.Mana < ManaCost) throw new Exceptions.AttemptedSkillWithoutMana();
    }

    public virtual int GetWeight()
    {
        if (BtData == null) throw new Exceptions.BtDataIsNull();
        
        return SkillWeightDelegate?.GetWeight(BtData) ?? _defaultWeight;
    }
}
    