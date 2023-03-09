namespace AutoBattleRPG.Scripts;

[Serializable]
public class Settings
{
    public (int x, int y) GridSize;

    public static (int x, int y) GridMinimum = (5, 5);
        
    public static Settings DefaultSettings = new Settings
    {
        GridSize = (7, 7),
    };
}