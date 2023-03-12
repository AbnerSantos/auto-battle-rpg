using AutoBattleRPG.Scripts.Character;

namespace AutoBattleRPG.Scripts.BehaviorTree;

public abstract class APickUpSkill : INamedSkill
{
    public string Name { get; set; }
    public string Description { get; set; }

    protected APickUpSkill(string name, string description, ACharacter character)
    {
        Name = name;
        Description = description;
    }
}