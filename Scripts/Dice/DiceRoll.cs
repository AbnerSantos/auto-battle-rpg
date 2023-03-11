using System.Text;

namespace AutoBattleRPG.Scripts.Dice;

public class DiceRoll
{
    public int Modifier { get; private set; }
    public List<Die> Dice { get; private set; }

    public DiceRoll(List<Die> dice, int modifier = 0)
    {
        Modifier = modifier;
        Dice = dice;
    }

    public DiceResult Roll()
    {
        return new DiceResult(this);
    }

    public override string ToString()
    {
        Dictionary<int, int> dicePerSides = new Dictionary<int, int>();
        foreach (Die die in Dice)
        {
            dicePerSides[die.Sides] = dicePerSides.GetValueOrDefault(die.Sides) + 1;
        }

        StringBuilder strBuilder = new StringBuilder();
        using Dictionary<int, int>.Enumerator enumerator = dicePerSides.GetEnumerator();

        bool last = !enumerator.MoveNext();
        while (!last)
        {
            KeyValuePair<int, int> current = enumerator.Current;
            strBuilder.Append($"{current.Value}d{current.Key}");
            
            last = !enumerator.MoveNext();
            if (!last || Modifier != 0) strBuilder.Append(" + ");
        }

        if (Modifier != 0) strBuilder.Append(Modifier);
        
        return strBuilder.ToString();
    }
}