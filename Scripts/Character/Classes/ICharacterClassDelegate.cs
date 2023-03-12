using AutoBattleRPG.Scripts.BehaviorTree;
using AutoBattleRPG.Scripts.Dice;
using AutoBattleRPG.Scripts.Pathfinding;
using AutoBattleRPG.Scripts.Stage;

namespace AutoBattleRPG.Scripts.Character.Classes;

public interface ICharacterClassDelegate
{
    public string Name { get; }
    public string Description { get; }
    public char Symbol { get; }
    public DiceRoll? Def { get; }
    public int MaxHp { get; }
    public int MaxMana { get; }
    public int Range { get; }
    public int Movement { get; }

    public BehaviorTree<RpgBtData> SetupBehaviorTree(RpgBtData rpgBtData);
    public List<APickUpSkill> SetupStartingPickUpSkills(ACharacter character);
    public List<APassiveSkill> SetupPassiveSkills(ACharacter character);
    public AStarPathfinder GeneratePathfinder(GameMap gameMap, ACharacter character);
    public int AttackDistance(Tile t1, Tile t2);
    public void DefenseQuote(ACharacter defendant, DiceResult defense);
    public void PerfectDefenseQuote(ACharacter defendant);
    public void MovementQuote(ACharacter character, Tile prevTile, Tile newTile);
}