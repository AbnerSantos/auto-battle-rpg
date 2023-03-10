using AutoBattleRPG.Scripts.Stage;

namespace AutoBattleRPG.Scripts.Character;

public class PlayerCharacter : ACharacter
{
    public override int Atk => 5;
    public override int MaxHp => 20;
    public override ACharacter Target => GameMap.Enemy;

    public PlayerCharacter(GameMap gameMap, string name) : base(gameMap, name)
    {
    }
}