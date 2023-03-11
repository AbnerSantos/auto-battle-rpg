using AutoBattleRPG.Scripts.Character;
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
            // Fetching settings
            Console.WriteLine("\nWelcome to Trust in Your Party - The RPG!\n");
            
            Console.WriteLine($"What's the width of your battlefield? (Min: {Settings.GridMinimum.x}, Recommended: {Settings.DefaultSettings.GridSize.x})");
            int width = InputHelper.GetNumber(Settings.GridMinimum.x, int.MaxValue);
            
            Console.WriteLine($"What's the height of your battlefield? (Min: {Settings.GridMinimum.y}, Recommended: {Settings.DefaultSettings.GridSize.y})");
            int height = InputHelper.GetNumber(Settings.GridMinimum.y, int.MaxValue);

            int maxPartySize = Settings.MaxPartySize(width, height);
            Console.WriteLine($"What will be the party size? (Min: 1, Max: {maxPartySize}, Recommended: {Settings.DefaultSettings.PartySize})");
            int partySize = InputHelper.GetNumber(1, maxPartySize);

            Settings settings = new Settings
            {
                GridSize = (width, height),
                PartySize = partySize
            };
            
            StartMatch(settings);
            
            Console.WriteLine("\nDo you want to play again?[y/n]");
            ConsoleKeyInfo key = InputHelper.GetKey(new HashSet<char> { 'y', 'n' });
            if (key.KeyChar == 'n') isRunning = false;
        }
    }
    
    private void StartMatch(Settings settings)
    {
        string? winner = null;
        GameMap gameMap = new GameMap(settings);
        
        void OnCharacterDeath(List<ACharacter> team)
        {
            if (!team.TrueForAll(character => !character.IsAlive)) return;

            winner = team[0] is EnemyCharacter ? "Player" : "Enemy";
        }

        Console.WriteLine("\nCreate your party!");
        for (int i = 0; i < settings.PartySize; i++)
        {
            Console.WriteLine($"\nPlayer Character [{i+1}]");
            PlayerCharacter player = CreatePlayerCharacter(gameMap);
            player.Died += OnCharacterDeath;
        }
        
        Console.WriteLine("\nCreate the enemy party!");
        for (int i = 0; i < settings.PartySize; i++)
        {
            Console.WriteLine($"\nEnemy Character [{i+1}]");
            EnemyCharacter enemy = CreateEnemyCharacter(gameMap);
            enemy.Died += OnCharacterDeath;
        }

        gameMap.DisplayMap();

        // Shuffles order of character turns
        gameMap.Characters.Sort((_, _) => RandomHelper.Rand.Next());
        
        Console.WriteLine($"Characters placed! {gameMap.Characters[0].Name} starts!");
        
        while (winner == null)
        {
            foreach (ACharacter character in gameMap.Characters)
            {
                if (character.TryProcessTurn()) PromptKeyPress();
                if (winner != null) break; // Assigned OnCharacterDeath
            }
        }
        
        Console.WriteLine($"{winner} wins!");
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

    private PlayerCharacter CreatePlayerCharacter(GameMap gameMap)
    {
        Console.WriteLine("Choose your class:");
        ICharacterClassDelegate playerClass = ChooseClass();
        Console.WriteLine("What's the name of your character? (Enter for default)");
        string? rawName = Console.ReadLine();
        string name = ProcessCharacterName(rawName, playerClass, gameMap.PlayerTeam);
        PlayerCharacter player = new PlayerCharacter(gameMap, name, playerClass);
        player.PlaceOnMap();
        return player;
    }
    
    private EnemyCharacter CreateEnemyCharacter(GameMap gameMap)
    {
        Console.WriteLine("Choose their class:");
        ICharacterClassDelegate enemyClass = ChooseClass();
        Console.WriteLine("What's the name of their character? (Enter for default)");
        string? rawName = Console.ReadLine();
        string name = ProcessCharacterName(rawName, enemyClass, gameMap.EnemyTeam, "Enemy ");
        EnemyCharacter enemy = new EnemyCharacter(gameMap, name, enemyClass);
        enemy.PlaceOnMap();
        return enemy;
    }

    private string ProcessCharacterName(string? rawName, ICharacterClassDelegate characterClass, List<ACharacter> team, string prefix = "")
    {
        string? name = rawName;
        // Class name if no name has been input
        if (string.IsNullOrWhiteSpace(rawName)) name = $"{prefix}{characterClass.Name}";
        
        bool IsUnique() => !team.Exists(character => character.Name.Equals(name));
        
        int instances = 1;
        while (!IsUnique())
        {
            // "Ranger 1", "Ranger 2" and so on
            name = $"{prefix}{characterClass.Name} {instances++}";
        }

        return name!;
    }
}