using AutoBattleRPG.Scripts.Character;
using AutoBattleRPG.Scripts.Dice;
using AutoBattleRPG.Scripts.Utility;

namespace AutoBattleRPG.Scripts.BehaviorTree;

public class AttackTargetInRangeSkill : AActiveSkill
{
    private readonly DiceRoll _atk;
    private readonly Action<ACharacter, DiceResult> _attackQuote;
    
    public AttackTargetInRangeSkill(string name, string description, DiceRoll atk, int manaCost, Action<ACharacter, DiceResult> attackQuote, int defaultWeight = 1, IWeightedSkillDelegate? skillWeightDelegate = null)
        : base(name, description, manaCost, defaultWeight, skillWeightDelegate)
    {
        _atk = atk;
        _attackQuote = attackQuote;
    }
    
    public override void Execute()
    {
        base.Execute();
        if (BtData!.CurrentTarget == null) throw new Exceptions.NoValidTargets();
        if (BtData!.Character.Mana < ManaCost) throw new Exceptions.AttemptedSkillWithoutMana();

        DiceResult dmg = _atk.Roll();
        _attackQuote(BtData.CurrentTarget, dmg);
        BtData.Character.SpendMana(ManaCost);
        BtData.CurrentTarget.TryDamage(dmg.Total, BtData.Character);
    }

    public override int GetWeight()
    {
        if (BtData == null) throw new Exceptions.BtDataIsNull();

        if (BtData!.Character.Mana < ManaCost) return 0; // Impossible to use move if no mana to use it
        return base.GetWeight();
    }
}