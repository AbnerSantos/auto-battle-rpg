using AutoBattleRPG.Scripts.BehaviorTree;
using AutoBattleRPG.Scripts.Dice;
using AutoBattleRPG.Scripts.Pathfinding;
using AutoBattleRPG.Scripts.Stage;

namespace AutoBattleRPG.Scripts.Character.Classes;

public class Ranger : ICharacterClassDelegate
{
    public string Name => "Ranger";
    public string Description => "Can easily traverse thick forests and uses a bow for ranged attacks that go over obstacles.";
    public char Symbol => 'r';
    public DiceRoll Atk => new DiceRoll(new List<Die>{ new Die(6) }, 3);
    public DiceRoll Def => new DiceRoll(new List<Die>{ new Die(2) });
    public int MaxHp => 20;
    public int Range => 2;
    public int Movement => 2;
    
    public Dictionary<Terrain.TerrainType, int> TerrainMovModifiers { get; } = new()
    {
        { Terrain.TerrainType.Forest, -1 }
    };

    public BehaviorTree<RpgBtData> SetupBehaviorTree(RpgBtData rpgBtData)
    {
        IsWithinTargetRangeSelectorNode checkTarget = new IsWithinTargetRangeSelectorNode
        (
            ifFalse: new MoveTowardsTargetNode(),
            ifTrue: new AttackTargetInRangeNode()
        );

        return new BehaviorTree<RpgBtData>(checkTarget, rpgBtData);
    }
    
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
    
    public void AttackQuote(ACharacter attacker, ACharacter target, DiceRoll roll, DiceResult rawDmg)
    {
        Console.WriteLine($"{attacker.Name} shoots {target.Name} for {roll} = {rawDmg} damage!");
    }

    public void DefenseQuote(ACharacter defendant, DiceResult defense)
    {
        Console.WriteLine($"{defendant.Name} tries to dodge and avoids {defendant.Def} = {defense} damage!");
    }

    public void PerfectDefenseQuote(ACharacter defendant)
    {
        Console.WriteLine($"{defendant.Name} fully dodges the attack!");
    }

    public void MovementQuote(ACharacter character, Tile prevTile, Tile newTile)
    {
        switch (newTile.Terrain)
        {
            case Terrain.TerrainType.Plains:
                Console.WriteLine($"{character.Name} moved {Tile.GetCardinalDirection(prevTile, newTile)} in the plains.");
                break;
            case Terrain.TerrainType.Forest:
                Console.WriteLine($"{character.Name} swiftly moved {Tile.GetCardinalDirection(prevTile, newTile)} into the forest.");
                break;
        }
    }
}