using AutoBattleRPG.Scripts.Stage;
using AutoBattleRPG.Scripts.Utility;

namespace AutoBattleRPG.Scripts;

internal static class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        GameMap gameMap = new GameMap(Settings.DefaultSettings);
        Console.WriteLine(gameMap.ToString());
    }
}