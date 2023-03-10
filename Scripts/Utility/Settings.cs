namespace AutoBattleRPG.Scripts.Utility;

[Serializable]
public class Settings
{
    public (int x, int y) GridSize;
    public string Seed = RandomHelper.Seed;

    public static (int x, int y) GridMinimum = (5, 5);
        
    public static Settings DefaultSettings = new Settings
    {
        GridSize = (7, 7)
    };
}