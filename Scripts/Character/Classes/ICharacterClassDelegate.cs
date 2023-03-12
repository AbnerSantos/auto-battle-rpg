using AutoBattleRPG.Scripts.Dice;
using AutoBattleRPG.Scripts.Pathfinding;
using AutoBattleRPG.Scripts.Stage;

namespace AutoBattleRPG.Scripts.Character.Classes;

public interface ICharacterClassDelegate
{
    public string Name { get; }
    public char Symbol { get; }
    public DiceRoll Atk { get; }
    public DiceRoll Def { get; }
    public int MaxHp { get; }
    public int Range { get; }
    public int Movement { get; }

    public AStarPathfinder GeneratePathfinder(GameMap gameMap);
    public int AttackDistance(Tile t1, Tile t2);
    public int GetMovementCost(Tile tile);
}