namespace AutoBattleRPG.Scripts.Stage;

public abstract class AMap<T>
{
    public abstract T[,] Grid { get; protected set; }
    public int Width => Grid.GetLength(0);
    public int Height => Grid.GetLength(1);
    
    public T this[int x, int y]
    {
        get => Grid[x, y];
        set => Grid[x, y] = value;
    }
}