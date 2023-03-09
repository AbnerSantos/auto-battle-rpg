namespace AutoBattleRPG.Scripts;

class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        GameMap gameMap = new GameMap(Settings.DefaultSettings);
        Console.WriteLine(gameMap.ToString());
    }
}