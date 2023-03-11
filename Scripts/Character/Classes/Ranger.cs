using AutoBattleRPG.Scripts.Dice;

namespace AutoBattleRPG.Scripts.Character.Classes;

public class Ranger : ICharacterClassDelegate
{
    public string Name => "Ranger";
    public DiceRoll Atk => new DiceRoll(new List<Die>{ new Die(6) }, 3);
    public DiceRoll Def => new DiceRoll(new List<Die>{ new Die(2) });
    public int MaxHp => 20;
    public int Range => 3;
}