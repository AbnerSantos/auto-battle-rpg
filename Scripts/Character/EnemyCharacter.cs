﻿using AutoBattleRPG.Scripts.Dice;
using AutoBattleRPG.Scripts.Stage;

namespace AutoBattleRPG.Scripts.Character;

public class EnemyCharacter : ACharacter
{
    public override DiceRoll Atk => new DiceRoll(new List<Die>{ new Die(8) }, 2);
    public override int MaxHp => 20;
    public override ACharacter Target => GameMap.Player;

    public EnemyCharacter(GameMap gameMap, string name) : base(gameMap, name)
    {
    }
}