using AutoBattleRPG.Scripts.Dice;

namespace AutoBattleRPG.Scripts.Character.Classes;

public interface ICharacterClassDelegate
{
    public string Name { get; }
    public DiceRoll Atk { get; }
    public DiceRoll Def { get; }
    public int MaxHp { get; }
    public int Range { get; }
}