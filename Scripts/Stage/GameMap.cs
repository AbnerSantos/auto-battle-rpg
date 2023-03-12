using System.Text;
using AutoBattleRPG.Scripts.Character;
using AutoBattleRPG.Scripts.Utility;

namespace AutoBattleRPG.Scripts.Stage;

public class GameMap : AMap<Tile>
{
    public sealed override Tile[,] Grid { get; protected set; }
    public List<ACharacter> PlayerTeam { get; } = new();
    public List<ACharacter> EnemyTeam { get; } = new();
    public List<ACharacter> Characters { get; } = new();
    public List<Tile> AvailableTiles { get; } = new();

    public GameMap(Settings settings)
    {
        Grid = new Tile[settings.GridSize.x, settings.GridSize.y];
        float[,] noise = PerlinNoise.GenerateNoiseMap(Width, Height);
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                Terrain.TerrainType terrain = noise[i, j] >= settings.ForestDensity ? Terrain.TerrainType.Forest : Terrain.TerrainType.Plains;
                Tile newTile = new Tile(i, j, terrain, this);
                Grid[i, j] = newTile;
                AvailableTiles.Add(newTile);
            }
        }
    }

    public void DisplayMap()
    {
        StringBuilder strBuilder = new StringBuilder();
        strBuilder.Append('╔');
        strBuilder.Append('═', Width * 3);
        strBuilder.Append('╗');
        Console.WriteLine(strBuilder.ToString());

        for (int i = 0; i < Height; i++)
        {
            Console.Write('║');

            for (int j = 0; j < Width; j++)
            {
                Grid[j, i].DisplayTile();
            }
            
            Console.WriteLine('║');
        }

        strBuilder.Clear();
        
        strBuilder.Append('╚');
        strBuilder.Append('═', Width * 3);
        strBuilder.Append('╝');
        Console.WriteLine(strBuilder.ToString());
    }
}