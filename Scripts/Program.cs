using AutoBattleRPG.Scripts.GameLoop;

namespace AutoBattleRPG.Scripts;

internal static class Program
{
    private static void Main(string[] args)
    {
        MatchController match = new MatchController();
        match.StartGame();
    }
}