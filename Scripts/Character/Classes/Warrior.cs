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
    public DiceRoll Def => new DiceRoll(new List<Die>{ new Die(4) });
    public int MaxHp => 20;
    public int MaxMana => 0;
    public int Range => 1;
    public int Movement => 2;
    
    public BehaviorTree<RpgBtData> SetupBehaviorTree(RpgBtData rpgBtData)
    {
        MoveTowardsTargetNode movement = new();
        WeightedSelectorNode attacks = new();
        
        DiceRoll normalAttackRoll = new DiceRoll(new List<Die>{ new Die(10) }, 2);
        AttackTargetInRangeSkill normalAttack = new
        (
            name: "Slash",
            description: $"Swings sword at enemy for {normalAttackRoll} damage.",
            normalAttackRoll,
            manaCost: 0,
            attackQuote: (target, result) => AttackQuote(rpgBtData.Character, target, normalAttackRoll, result),
            defaultWeight: 19
        );
        
        DiceRoll critAttackRoll = new DiceRoll(new List<Die>{ new Die(10) }, 2);
        AttackTargetInRangeSkill critAttack = new
        (
            name: "Impale",
            description: $"Low chance of critically impaling enemy for double ({critAttackRoll}) damage!.",
            critAttackRoll,
            manaCost: 0,
            attackQuote: (target, result) => CritQuote(rpgBtData.Character, target, critAttackRoll, result),
            defaultWeight: 1
        );
        
        attacks.Add(normalAttack);
        attacks.Add(critAttack);
        
        IsWithinTargetRangeSelectorNode checkTarget = new
        (
            ifFalse: movement,
            ifTrue: attacks
        );

        return new BehaviorTree<RpgBtData>(checkTarget, rpgBtData);
    }

    public List<APickUpSkill> SetupStartingPickUpSkills(ACharacter character)
    {
        return new List<APickUpSkill>();
    }

    public List<APassiveSkill> SetupPassiveSkills(ACharacter character)
    {
        return new List<APassiveSkill>();
    }

    public AStarPathfinder GeneratePathfinder(GameMap gameMap, ACharacter character)
    {
        return new AStarPathfinder(gameMap.Width, gameMap.Height,
            new MeleeCombatMovementAStarInfoProvider(gameMap, character));
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
    
    public void CritQuote(ACharacter attacker, ACharacter target, DiceRoll roll, DiceResult rawDmg)
    {
        Console.WriteLine($"{attacker.Name} critically impales {target.Name} for {roll} = {rawDmg} damage!");
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