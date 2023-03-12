using AutoBattleRPG.Scripts.BehaviorTree;
using AutoBattleRPG.Scripts.Dice;
using AutoBattleRPG.Scripts.Pathfinding;
using AutoBattleRPG.Scripts.Stage;

namespace AutoBattleRPG.Scripts.Character.Classes;

public class Mage : ICharacterClassDelegate
{
    public string Name => "Mage";
    public string Description => "Frail but casts very powerful spells at range - if you have the mana, that is.";
    public char Symbol => 'm';
    public DiceRoll? Def => null;
    public int MaxHp => 20;
    public int MaxMana => 20;
    public int Range => 2;
    public int Movement => 2;

    public BehaviorTree<RpgBtData> SetupBehaviorTree(RpgBtData rpgBtData)
    {
        WeightedSelectorNode chargeOrMove = new();
        MoveTowardsTargetNode movement = new();
        
        DiceRoll fireballRoll = new DiceRoll(new List<Die>{ new Die(8), new Die(8) }, 2);
        AttackTargetInRangeSkill fireball = new
        (
            name: "Fireball",
            description: $"Shoots a ball of magic fire at the enemy for {fireballRoll} damage!",
            fireballRoll,
            manaCost: 10,
            attackQuote: (target, result) => AttackQuote(rpgBtData.Character, target, fireballRoll, result),
            defaultWeight: 19
        );

        IsWithinTargetRangeSelectorNode checkTarget = new
        (
            ifFalse: movement,
            ifTrue: fireball,
            defaultWeight: 2,
            new ManaProportionalWeightedSkillDelegate
            (
                manaCost: fireball.ManaCost,
                positiveIfNoMana: false,
                missingManaToWeightMultiplier: 0,
                defaultWeight: 2
            )
        );

        DiceRoll manaCharge = new DiceRoll(new List<Die> { new Die(8) }, 3);
        ChargeManaSkill chargeManaSkill = new
        (
            name: "Concentrate",
            description: $"Concentrate and charge your mana by {manaCharge}",
            manaRecovery: manaCharge,
            manaCost: 0,
            defaultWeight: 1,
            new ManaProportionalWeightedSkillDelegate
            (
                manaCost: fireball.ManaCost,
                positiveIfNoMana: true,
                missingManaToWeightMultiplier: 2f,
                defaultWeight: 2
            )
        );
        
        chargeOrMove.Add(chargeManaSkill);
        chargeOrMove.Add(checkTarget);

        return new BehaviorTree<RpgBtData>(chargeOrMove, rpgBtData);
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
            new RangedCombatMovementAStarInfoProvider(gameMap, character, Range));
    }

    public int AttackDistance(Tile t1, Tile t2)
    {
        // Ranged weapons can attack diagonally as easily
        return Tile.ChebyshevDistance(t1, t2);
    }

    public void AttackQuote(ACharacter attacker, ACharacter target, DiceRoll roll, DiceResult rawDmg)
    {
        Console.WriteLine($"{attacker.Name} throws a fireball at {target.Name} for {roll} = {rawDmg} damage!");
    }

    public void DefenseQuote(ACharacter defendant, DiceResult defense) { }

    public void PerfectDefenseQuote(ACharacter defendant) { }

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