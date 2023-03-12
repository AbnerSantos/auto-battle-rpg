using AutoBattleRPG.Scripts.Character;
using AutoBattleRPG.Scripts.Dice;
using AutoBattleRPG.Scripts.Stage;
using AutoBattleRPG.Scripts.Utility;

namespace AutoBattleRPG.Scripts.BehaviorTree;

public class HealInForests : APassiveSkill
{
    private readonly DiceRoll _hpRecovery;
    private readonly ACharacter _character;
    
    public HealInForests(string name, string description, ACharacter character, DiceRoll hpRecovery) : base(name, description)
    {
        _hpRecovery = hpRecovery;
        _character = character;
    }

    public override void Execute()
    {
        if (_character.CurrentTile == null) throw new Exceptions.CharacterNotInGameMap();

        if (_character.CurrentTile.Terrain != Terrain.TerrainType.Forest) return;
        
        Console.WriteLine($"{_character.Name} meditates in the woods and recovers health!");
        _character.Heal(_hpRecovery);
    }
}