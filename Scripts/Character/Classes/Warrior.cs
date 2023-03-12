using AutoBattleRPG.Scripts.BehaviorTree;
using AutoBattleRPG.Scripts.Dice;
using AutoBattleRPG.Scripts.Pathfinding;
using AutoBattleRPG.Scripts.Stage;

namespace AutoBattleRPG.Scripts.Character.Classes;

public class Warrior : ICharacterClassDelegate
{
    public string Name => "Warrior";
    public string Description => "High damaging melee fighter that uses his shield to defend himself.";
    public char Symbol => 'w';
    public DiceRoll Atk => new DiceRoll(new List<Die>{ new Die(10) }, 2);
    public DiceRoll Def => new DiceRoll(new List<Die>{ new Die(4) });
    public int MaxHp => 20;
    public int Range => 1;
    public int Movement => 2;
    
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
    
    public void AttackQuote(ACharacter attacker, ACharacter target, DiceRoll roll, DiceResult rawDmg)
    {
        Console.WriteLine($"{attacker.Name} slashes {target.Name} for {roll} = {rawDmg} damage!");
    }

    public void DefenseQuote(ACharacter defendant, DiceResult defense)
    {
        Console.WriteLine($"{defendant.Name} raises their shield and blocks {defendant.Def} = {defense} damage!");
    }

    public void PerfectDefenseQuote(ACharacter defendant)
    {
        Console.WriteLine($"{defendant.Name} blocks all incoming damage!");
    }

    public void MovementQuote(ACharacter character, Tile prevTile, Tile newTile)
    {
        switch (newTile.Terrain)
        {
            case Terrain.TerrainType.Plains:
                Console.WriteLine($"{character.Name} moved {Tile.GetCardinalDirection(prevTile, newTile)} in the plains.");
                break;
            case Terrain.TerrainType.Forest:
                Console.WriteLine($"{character.Name} was slowed down by moving {Tile.GetCardinalDirection(prevTile, newTile)} into the thick forest.");
                break;
        }
    }
}