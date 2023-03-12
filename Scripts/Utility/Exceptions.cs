namespace AutoBattleRPG.Scripts.Utility;

public static class Exceptions
{
    [Serializable] public class BtDataIsNull : Exception { }
    [Serializable] public class CharacterNotInGameMap : Exception { }
    [Serializable] public class NoValidTargets : Exception { }
}