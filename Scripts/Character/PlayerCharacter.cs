using AutoBattleRPG.Scripts.Character.Classes;
using AutoBattleRPG.Scripts.Stage;

namespace AutoBattleRPG.Scripts.Character;

public class PlayerCharacter : ACharacter
{
    public override List<ACharacter> AvailableTargets => GameMap.EnemyTeam;
    public override List<ACharacter> Team => GameMap.PlayerTeam;

    public PlayerCharacter(GameMap gameMap, string name, ICharacterClassDelegate characterClass) : base(gameMap, name, characterClass)
    {
    }
}