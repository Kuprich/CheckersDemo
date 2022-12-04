namespace CheckersDemo.Client;


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
}