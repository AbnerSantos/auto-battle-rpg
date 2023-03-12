using System.Drawing;
using AutoBattleRPG.Scripts.Pathfinding;

namespace AutoBattleRPG.Scripts.Stage;

public static class Terrain
{
    public enum TerrainType
    {
        Plains,
        Forest
    }

    private struct DrawInfo
    {
        public readonly string Str;
        public readonly ConsoleColor ForegroundColor;
        public readonly ConsoleColor BackgroundColor;
        
        public DrawInfo(string str, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            Str = str;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }
    }

    public static readonly Dictionary<TerrainType, int> MovementCostPerTerrain = new()
    {
        { TerrainType.Plains, 1 },
        { TerrainType.Forest, 2 }
    };
    
    private static readonly Dictionary<TerrainType, DrawInfo> SymbolsPerTerrain = new()
    {
        { TerrainType.Plains, new DrawInfo(" · ", ConsoleColor.White, ConsoleColor.Black ) },
        { TerrainType.Forest, new DrawInfo("¤¤¤", ConsoleColor.DarkGreen, ConsoleColor.Black) }
    };

    public static void Draw(this TerrainType terrainType, char? characterSymbol = null, ConsoleColor? characterColor = null)
    {
        DrawInfo drawInfo = SymbolsPerTerrain[terrainType];
        Console.ForegroundColor = drawInfo.ForegroundColor;
        Console.BackgroundColor = drawInfo.BackgroundColor;
        Console.Write(drawInfo.Str[0]);

        if (characterSymbol != null)
        {
            ConsoleColor color = characterColor ?? ConsoleColor.White;
            Console.ForegroundColor = color;
            Console.Write(characterSymbol);
        }
        else
        {
            Console.ForegroundColor = drawInfo.ForegroundColor;
            Console.BackgroundColor = drawInfo.BackgroundColor;
            Console.Write(drawInfo.Str[1]);
            Console.Write(drawInfo.Str[2]);
            return;
        }
        
        Console.ForegroundColor = drawInfo.ForegroundColor;
        Console.BackgroundColor = drawInfo.BackgroundColor;
        Console.Write(drawInfo.Str[2]);
    }
}