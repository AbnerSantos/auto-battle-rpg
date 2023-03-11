using AutoBattleRPG.Scripts.Dice;
using AutoBattleRPG.Scripts.Stage;

namespace AutoBattleRPG.Scripts.Character;

public class PlayerCharacter : ACharacter
{
    public override DiceRoll Atk => new DiceRoll(new List<Die>{ new Die(8) }, 2);
    public override DiceRoll Def => new DiceRoll(new List<Die>{ new Die(4) });
    public override int MaxHp => 20;
    public override int Range => 2;
    public override ACharacter Target => GameMap.Enemy;

    public PlayerCharacter(GameMap gameMap, string name) : base(gameMap, name)
    {
    }
}