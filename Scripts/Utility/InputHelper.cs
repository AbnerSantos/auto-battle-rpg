namespace AutoBattleRPG.Scripts.Utility;

public static class InputHelper
{
    public static ConsoleKeyInfo GetKey(char lowerBound, char upperBound)
    {
        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(true);
        } while (key.KeyChar < lowerBound || key.KeyChar > upperBound);

        return key;
    }
    
    public static ConsoleKeyInfo GetKey(HashSet<char> acceptedCharacters)
    {
        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(true);
        } while (!acceptedCharacters.Contains(key.KeyChar));

        return key;
    }

    public static int GetNumber(int lowerBound, int upperBound)
    {
        int? number = null;

        do
        {
            string? line = Console.ReadLine();
            if (line == null) continue;
            
            if (int.TryParse(line, out int num)) number = num;
            else
            {
                Console.Write("Not a number. Try again: ");
                continue;
            }

            if (number >= lowerBound && number <= upperBound) continue;
            
            number = null;
            Console.Write("Number out of bounds. Try again: ");

        } while (number == null);

        return (int) number;
    }
}