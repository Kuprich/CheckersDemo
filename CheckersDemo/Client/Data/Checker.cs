namespace CheckersDemo.Client.Data;

public class Checker
{
    public Checker(int row, int column, CheckerDirection direction, bool isWhite)
    {
        Row = row;
        Column = column;
        Direction = direction;
        IsWhite = isWhite;
    }

    public int Row { get; set; }
    public int Column { get; set; }
    public CheckerDirection Direction { get; set; }
    public bool IsWhite { get; set; }

}

public enum CheckerDirection
{
    Down, Up, Both
}
