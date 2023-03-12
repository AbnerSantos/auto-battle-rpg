using AutoBattleRPG.Scripts.Character;
using AutoBattleRPG.Scripts.Dice;
using AutoBattleRPG.Scripts.Stage;
using AutoBattleRPG.Scripts.Utility;

namespace AutoBattleRPG.Scripts.BehaviorTree;

public class HealManaInForests : APassiveSkill
{
    private readonly DiceRoll _manaRecovery;
    private readonly ACharacter _character;
    
    public HealManaInForests(string name, string description, ACharacter character, DiceRoll manaRecovery) : base(name, description)
    {
        _manaRecovery = manaRecovery;
        _character = character;
    }

    public override void Execute()
    {
        if (_character.CurrentTile == null) throw new Exceptions.CharacterNotInGameMap();

        if (_character.CurrentTile.Terrain != Terrain.TerrainType.Forest) return;
        
        Console.WriteLine($"{_character.Name} meditates in the woods and recovers mana!");
        _character.HealMana(_manaRecovery);
    }
}