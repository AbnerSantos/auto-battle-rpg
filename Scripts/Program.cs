using AutoBattleRPG.Scripts.GameLoop;
using AutoBattleRPG.Scripts.Stage;
using AutoBattleRPG.Scripts.Utility;

namespace AutoBattleRPG.Scripts;

internal static class Program
{
    private static void Main(string[] args)
    {
        MatchController match = new MatchController();
        match.StartGame();
    }
}