using System.Text;

namespace AutoBattleRPG.Scripts.Utility;

public static class RandomHelper
{
    private const int SeedLength = 6;
    
    public static string Seed { get; private set; } = RandomSeed();
    public static Random Rand = new(Seed.GetHashCode());

    public static void SetupSeed(string seed)
    {
        Rand = new Random(seed.GetHashCode());
    }

    static string RandomSeed()
    {
        Random r = new Random();
        StringBuilder strBuilder = new StringBuilder();
        
        for (int i = 0; i < SeedLength; i++)
        {
            int offset = r.Next(0, 'Z' - 'A');
            strBuilder.Append(Convert.ToChar('A' + offset));
        }

        Seed = strBuilder.ToString();
        return Seed;
    }
}