namespace CheckersDemo.Client.Data;

public class Checker
{
    public Checker(int row, int column, CheckerDirection direction, string color)
    {
        Row = row;
        Column = column;
        Direction = direction;
        Color = color;
    }

    public int Row { get; set; }
    public int Column { get; set; }
    public CheckerDirection Direction { get; set; }
    public string Color { get; set; }

}

public enum CheckerDirection
{
    Down, Up, Both
}
