using AutoBattleRPG.Scripts.Dice;

namespace AutoBattleRPG.Scripts.BehaviorTree;

public class ChargeManaSkill : AActiveSkill
{
    private readonly DiceRoll _manaRecovery;
    
    public ChargeManaSkill(string name, string description, DiceRoll manaRecovery, int manaCost, int defaultWeight = 1, IWeightedSkillDelegate? skillWeightDelegate = null) 
        : base(name, description, manaCost, defaultWeight, skillWeightDelegate)
    {
        _manaRecovery = manaRecovery;
    }

    public override void Execute()
    {
        base.Execute();
        BtData!.Character.HealMana(_manaRecovery);
    }
}