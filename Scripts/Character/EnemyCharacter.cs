using AutoBattleRPG.Scripts.Character.Classes;
using AutoBattleRPG.Scripts.Stage;

namespace AutoBattleRPG.Scripts.Character;

public class EnemyCharacter : ACharacter
{
    public override List<ACharacter> AvailableTargets => GameMap.PlayerTeam;
    public override List<ACharacter> Team => GameMap.EnemyTeam;

    public EnemyCharacter(GameMap gameMap, string name, ICharacterClassDelegate characterClass) : base(gameMap, name, characterClass)
    {
    }
}