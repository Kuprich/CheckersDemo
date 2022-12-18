namespace CheckersDemo.Server.Data;

public class TableManager
{
    public Dictionary<string, int> Tables = new();

    public IEnumerable<string> GetTables()
    {
        return Tables.Where(x => x.Value < 2).Select(x => x.Key);
    }
}