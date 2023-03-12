namespace AutoBattleRPG.Scripts.Utility;

[Serializable]
public class Settings
{
    public (int x, int y) GridSize = (16, 10);
    public int PartySize = 2;
    public float ForestDensity = 0.45f;
    public string Seed = RandomHelper.Seed;

    public static (int x, int y) GridMinimum = (2, 2);
    public static (float min, float max) ForestConstraints = (0f, 1f);
        
    public static Settings DefaultSettings = new()
    {
        GridSize = (16, 10),
        PartySize = 2,
        ForestDensity = 0.45f
    };
    
    public static int MaxPartySize(int width, int height) => width * height / 2;
}