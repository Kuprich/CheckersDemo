namespace CheckersDemo.Client;

public class CheckersList : List<int>
{
    public async Task AddAsync(int value)
    {
        if (value >= 0 && value <= 7 && !Contains(value))
        {
            await Task.Run(() => { Add(value); });
        }

    }
    public async Task AddRange(int[] values)
    {
        foreach (int value in values)
        {
            await AddAsync(value);
        }
        return;
    }
}

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