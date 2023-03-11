using AutoBattleRPG.Scripts.Utility;

namespace AutoBattleRPG.Scripts.Dice;

public class Die
{
    public readonly int Sides;
    
    public Die(int sides)
    {
        Sides = sides;
    }

    public int Roll()
    {
        return RandomHelper.Rand.Next(1, Sides + 1);
    }
}