namespace AutoBattleRPG.Scripts.Utility;

[Serializable]
public class Settings
{
    public (int x, int y) GridSize;
    public int PartySize;
    public string Seed = RandomHelper.Seed;

    public static (int x, int y) GridMinimum = (2, 2);
        
    public static Settings DefaultSettings = new Settings
    {
        GridSize = (9, 9),
        PartySize = 2
    };
    
    public static int MaxPartySize(int width, int height) => width * height / 2;
}