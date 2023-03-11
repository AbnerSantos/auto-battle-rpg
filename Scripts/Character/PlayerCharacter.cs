using AutoBattleRPG.Scripts.Character.Classes;
using AutoBattleRPG.Scripts.Stage;

namespace AutoBattleRPG.Scripts.Character;

public class PlayerCharacter : ACharacter
{
    public override ACharacter Target => GameMap.Enemy;

    public PlayerCharacter(GameMap gameMap, string name, ICharacterClassDelegate characterClass) : base(gameMap, name, characterClass)
    {
    }
}