using CheckersDemo.Client.Data;

namespace CheckersDemo.Client;

public record Cell(int Row, int Col);

public static class ListExtensions
{
    public static void AddBoardValues(this List<int> list, int[] values)
    {
        foreach (int value in values)
        {
            AddBoardValue(list, value);
        }
    }
    public static void AddBoardValue(this List<int> list, int value)
    {
        if (value >= 0 && value <= 7)
        {
            list.Add(value);
        }
    }

    public static Checker? GetChecker(this List<Checker> list, int row, int col)
    {
        return list.FirstOrDefault(x => x.Row == row && x.Column == col);
    }
}