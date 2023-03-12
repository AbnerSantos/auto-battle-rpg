using AutoBattleRPG.Scripts.Dice;
using AutoBattleRPG.Scripts.Pathfinding;
using AutoBattleRPG.Scripts.Stage;

namespace AutoBattleRPG.Scripts.Character.Classes;

public class Ranger : ICharacterClassDelegate
{
    public string Name => "Ranger";
    public char Symbol => 'r';
    public DiceRoll Atk => new DiceRoll(new List<Die>{ new Die(6) }, 3);
    public DiceRoll Def => new DiceRoll(new List<Die>{ new Die(2) });
    public int MaxHp => 20;
    public int Range => 3;
    public int Movement => 2;
    
    public Dictionary<Terrain.TerrainType, int> TerrainMovModifiers { get; } = new()
    {
        { Terrain.TerrainType.Forest, -1 }
    };

    public AStarPathfinder GeneratePathfinder(GameMap gameMap)
    {
        return new AStarPathfinder(gameMap.Width, gameMap.Height,
            new RangedCombatMovementAStarInfoProvider(gameMap, this, Range));
    }

    public int AttackDistance(Tile t1, Tile t2)
    {
        // Ranged weapons can attack diagonally as easily
        return Tile.ChebyshevDistance(t1, t2);
    }

    public int GetMovementCost(Tile tile)
    {
        TerrainMovModifiers.TryGetValue(tile.Terrain, out int modifier);
        return Terrain.MovementCostPerTerrain[tile.Terrain] + modifier;
    }
}