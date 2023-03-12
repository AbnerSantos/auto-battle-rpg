using AutoBattleRPG.Scripts.Dice;
using AutoBattleRPG.Scripts.Pathfinding;
using AutoBattleRPG.Scripts.Stage;

namespace AutoBattleRPG.Scripts.Character.Classes;

public class Warrior : ICharacterClassDelegate
{
    public string Name => "Warrior";
    public char Symbol => 'w';
    public DiceRoll Atk => new DiceRoll(new List<Die>{ new Die(10) }, 2);
    public DiceRoll Def => new DiceRoll(new List<Die>{ new Die(4) });
    public int MaxHp => 20;
    public int Range => 1;
    public int Movement => 2;

    public AStarPathfinder GeneratePathfinder(GameMap gameMap)
    {
        return new AStarPathfinder(gameMap.Width, gameMap.Height,
            new MeleeCombatMovementAStarInfoProvider(gameMap, this));
    }
    
    public int AttackDistance(Tile t1, Tile t2)
    {
        // Melee weapons can attack only attack adjacent tiles
        return Tile.ManhattanDistance(t1, t2);
    }
    
    public int GetMovementCost(Tile tile)
    {
        return Terrain.MovementCostPerTerrain[tile.Terrain];
    }
}