using AutoBattleRPG.Scripts.Character.Classes;
using AutoBattleRPG.Scripts.Stage;

namespace AutoBattleRPG.Scripts.Character;

public class EnemyCharacter : ACharacter
{
    public override ACharacter Target => GameMap.Player;

    public EnemyCharacter(GameMap gameMap, string name, ICharacterClassDelegate characterClass) : base(gameMap, name, characterClass)
    {
    }
}