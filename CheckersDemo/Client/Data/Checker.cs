namespace CheckersDemo.Client.Data;

public class Checker
{
    public Checker(int row, int column, CheckerDirection direction, bool isWhite)
    {
        Cell = new Cell(row, column);
        Direction = direction;
        IsWhite = isWhite;
    }
    public Checker() { }

    public Cell Cell;
    public CheckerDirection Direction { get; set; }
    public bool IsWhite { get; set; }

}
