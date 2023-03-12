using AutoBattleRPG.Scripts.Character;
using AutoBattleRPG.Scripts.Stage;

namespace AutoBattleRPG.Scripts.BehaviorTree;

public class TerrainMovementBonus : APickUpSkill
{
    public TerrainMovementBonus(string name, string description, ACharacter character, Dictionary<Terrain.TerrainType, int> terrainModifiers) : base(name, description, character)
    {
        foreach ((Terrain.TerrainType terrain, int modifier) in terrainModifiers)
        {
            character.TerrainMovModifiers[terrain] =
                character.TerrainMovModifiers.GetValueOrDefault(terrain) + modifier;
        }
    }
}