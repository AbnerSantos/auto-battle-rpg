namespace AutoBattleRPG.Scripts.Character.Classes;

public static class CharacterClasses
{
    public enum PlayerClass
    {
        Warrior,
        Ranger
    }

    public static readonly Dictionary<PlayerClass, ICharacterClassDelegate> PlayerClasses = new()
    {
        { PlayerClass.Warrior, new Warrior() },
        { PlayerClass.Ranger, new Ranger() },
    };
}