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
    public DiceRoll Def => new DiceRoll(new List<Die>{ new Die(2) });
    public int MaxHp => 20;
    public int MaxMana => 20;
    public int Range => 3;
    public int Movement => 2;

    public BehaviorTree<RpgBtData> SetupBehaviorTree(RpgBtData rpgBtData)
    {
        MoveTowardsTargetNode movement = new();

        DiceRoll normalAttackRoll = new DiceRoll(new List<Die>{ new Die(6) }, 2);
        AttackTargetInRangeSkill normalAttack = new
        (
            name: "Shoot",
            description: $"Shoot arrow at enemy for {normalAttackRoll} damage.",
            normalAttackRoll,
            manaCost: 0,
            attackQuote: (target, result) => AttackQuote(rpgBtData.Character, target, normalAttackRoll, result)
        );
        
        IsWithinTargetRangeSelectorNode checkTarget = new
        (
            ifFalse: movement,
            ifTrue: normalAttack
        );

        return new BehaviorTree<RpgBtData>(checkTarget, rpgBtData);
    }

    public List<APickUpSkill> SetupStartingPickUpSkills(ACharacter character)
    {
        Dictionary<Terrain.TerrainType, int> modifiers = new()
        {
            { Terrain.TerrainType.Forest, -1 }
        };

        return new List<APickUpSkill>
        {
            new TerrainMovementBonus
            (
                name: "Hunter",
                description: "The Ranger is used to the forest and can walk through it without any movement penalties.",
                character: character,
                terrainModifiers: modifiers
            )
        };
    }

    public List<APassiveSkill> SetupPassiveSkills(ACharacter character)
    {
        return new List<APassiveSkill>
        {
            new HealManaInForests
            (
                name: "Boyscout",
                description: "The Ranger finds peace in starting his turn in the forest and heals some Mana.",
                character: character,
                manaRecovery: new DiceRoll(new List<Die> { new Die(4) }, 1)
            )
        };
    }

    public AStarPathfinder GeneratePathfinder(GameMap gameMap, ACharacter character)
    {
        return new AStarPathfinder(gameMap.Width, gameMap.Height,
            new RangedCombatMovementAStarInfoProvider(gameMap, character, Range));
    }

    public int AttackDistance(Tile t1, Tile t2)
    {
        // Ranged weapons can attack diagonally as easily
        return Tile.ChebyshevDistance(t1, t2);
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