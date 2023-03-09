namespace AutoBattleRPG.Scripts;

public class Tile
{
    public readonly int X;
    public readonly int Y;
        
    public Character? Character { get; set; }
    public bool IsOccupied => Character != null;
        
    public int MovCost => 1;

    public Tile(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void Occupy(Character? character)
    {
        Character = character;
    }

    public void Free()
    {
        Character = null;
    }

    public override string ToString()
    {
        return "·";
    }
}