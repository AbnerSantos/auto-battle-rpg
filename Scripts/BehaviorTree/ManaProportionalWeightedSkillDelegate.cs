namespace AutoBattleRPG.Scripts.BehaviorTree;

public class ManaProportionalWeightedSkillDelegate : IWeightedSkillDelegate
{
    private readonly int _manaCost;
    private readonly bool _positiveIfNoMana;
    private readonly float _missingManaToWeightMultiplier;
    private readonly int _defaultWeight;
        
    public ManaProportionalWeightedSkillDelegate(int manaCost, bool positiveIfNoMana, float missingManaToWeightMultiplier, int defaultWeight)
    {
        _manaCost = manaCost;
        _positiveIfNoMana = positiveIfNoMana;
        _missingManaToWeightMultiplier = missingManaToWeightMultiplier;
        _defaultWeight = defaultWeight;
    }

    public int GetWeight(RpgBtData btData)
    {
        bool isWithinRange = btData.CurrentTarget != null && btData.Character.IsWithinRange(btData.CurrentTarget);
        bool notEnoughMana = btData.Character.Mana < _manaCost;
        
        if (_positiveIfNoMana)
        {
            if (notEnoughMana) return 100;
            if (isWithinRange) return 0;
            return (int)((btData.Character.MaxMana - btData.Character.Mana) * _missingManaToWeightMultiplier);
        }

        if (notEnoughMana) return 0;
        if (isWithinRange) return 100;
        return _defaultWeight;
    }
}