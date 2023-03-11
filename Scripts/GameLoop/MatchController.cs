﻿using AutoBattleRPG.Scripts.Character;
using AutoBattleRPG.Scripts.Character.Classes;
using AutoBattleRPG.Scripts.Stage;
using AutoBattleRPG.Scripts.Utility;

namespace AutoBattleRPG.Scripts.GameLoop;

public class MatchController
{
    public void StartGame()
    {
        bool isRunning = true;
        while (isRunning)
        {
            Console.WriteLine("\nWelcome to Trust in Your Party - The RPG!\n");
            
            Console.WriteLine($"What's the width of your battlefield? (Min: {Settings.GridMinimum.x}, Recommended: {Settings.DefaultSettings.GridSize.x})");
            int width = InputHelper.GetNumber(Settings.GridMinimum.x, int.MaxValue);
            
            Console.WriteLine($"What's the height of your battlefield? (Min: {Settings.GridMinimum.y}, Recommended: {Settings.DefaultSettings.GridSize.y})");
            int height = InputHelper.GetNumber(Settings.GridMinimum.y, int.MaxValue);

            Settings settings = new Settings
            {
                GridSize = (width, height)
            };
            
            StartMatch(settings);
            
            Console.WriteLine("\nDo you want to play again?[y/n]");
            ConsoleKeyInfo key = InputHelper.GetKey(new HashSet<char> { 'y', 'n' });
            if (key.KeyChar == 'n') isRunning = false;
        }
    }
    
    private void StartMatch(Settings settings)
    {
        GameMap gameMap = new GameMap(settings);
        
        Console.WriteLine("Choose your class:");
        ICharacterClassDelegate playerClass = ChooseClass();
        PlayerCharacter player = new PlayerCharacter(gameMap, "Player", playerClass);
        
        Console.WriteLine("Choose your enemy's class:");
        ICharacterClassDelegate enemyClass = ChooseClass();
        EnemyCharacter enemy = new EnemyCharacter(gameMap, "Enemy", enemyClass);
        
        player.PlaceOnMap();
        enemy.PlaceOnMap();
        
        gameMap.DisplayMap();

        ACharacter first = RandomHelper.Rand.Next(2) == 1 ? player : enemy;
        ACharacter second = first == player ? enemy : player;
        
        Console.WriteLine($"Characters placed! {first.Name} starts!");

        ACharacter? winner = null;
        while (winner == null)
        {
            first.ComputeTurn();
            if (!second.IsAlive)
            {
                winner = first;
                continue;
            }

            PromptKeyPress();
            
            second.ComputeTurn();
            if (!first.IsAlive)
            {
                winner = second;
                continue;
            }

            PromptKeyPress();
        }
        
        Console.WriteLine($"{winner.Name} wins!");
    }

    private void PromptKeyPress()
    {
        Console.WriteLine("\nPress any key to continue...\n");
        Console.ReadKey(true);
    }

    private ICharacterClassDelegate ChooseClass()
    {
        char option = '1';
        foreach ((_, ICharacterClassDelegate characterClass) in CharacterClasses.PlayerClasses)
        {
            Console.WriteLine($"[{option}] - {characterClass.Name}");
            option = Convert.ToChar(option + 1);
        }

        ConsoleKeyInfo key = InputHelper.GetKey('1', Convert.ToChar(option - 1));
        return CharacterClasses.PlayerClasses[(CharacterClasses.PlayerClass) key.KeyChar - '1'];
    }
}