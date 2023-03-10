using System.Text;

namespace AutoBattleRPG.Scripts.Dice;

public class DiceResult
{
    public readonly int Modifier;
    public readonly List<(Die Die, int Result)> DiceResults = new List<(Die Die, int Result)>();
    public readonly int Total = 0;

    public DiceResult(DiceRoll roll)
    {
        Modifier = roll.Modifier;
        int total = 0;
        DiceResults.AddRange(roll.Dice.Select(die =>
        {
            int result = die.Roll();
            total += result;
            return (die, result);
        }));
        Total = total;
    }

    public override string ToString()
    {
        StringBuilder strBuilder = new StringBuilder();
        for (int i = 0; i < DiceResults.Count; i++)
        {
            strBuilder.Append($"({DiceResults[i].Result})");
            if (i < DiceResults.Count - 1 || Modifier != 0)
            {
                strBuilder.Append(" + ");
            }
        }

        if (Modifier != 0) strBuilder.Append(Modifier);

        strBuilder.Append($" = {Total}");
        return strBuilder.ToString();
    }
}