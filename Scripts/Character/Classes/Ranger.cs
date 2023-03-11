using AutoBattleRPG.Scripts.Dice;
using AutoBattleRPG.Scripts.Pathfinding;
using AutoBattleRPG.Scripts.Stage;
using AutoBattleRPG.Scripts.Utility;

namespace AutoBattleRPG.Scripts.Character.Classes;

public class Ranger : ICharacterClassDelegate
{
    public string Name => "Ranger";
    public char Symbol => 'r';
    public DiceRoll Atk => new DiceRoll(new List<Die>{ new Die(6) }, 3);
    public DiceRoll Def => new DiceRoll(new List<Die>{ new Die(2) });
    public int MaxHp => 20;
    public int Range => 3;
    
    public AStarPathfinder GeneratePathfinder(GameMap gameMap)
    {
        return new AStarPathfinder(gameMap.Width, gameMap.Height,
            new RangedCombatMovementAStarInfoProvider(gameMap, Range));
    }

    public int AttackDistance(Tile t1, Tile t2)
    {
        // Ranged weapons can attack diagonally as easily
        return Tile.ChebyshevDistance(t1, t2);
    }
}