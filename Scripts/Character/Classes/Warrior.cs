using AutoBattleRPG.Scripts.Dice;

namespace AutoBattleRPG.Scripts.Character.Classes;

public class Warrior : ICharacterClassDelegate
{
    public string Name => "Warrior";
    public DiceRoll Atk => new DiceRoll(new List<Die>{ new Die(10) }, 2);
    public DiceRoll Def => new DiceRoll(new List<Die>{ new Die(4) });
    public int MaxHp => 20;
    public int Range => 1;
}