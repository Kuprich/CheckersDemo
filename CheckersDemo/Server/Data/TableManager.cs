namespace CheckersDemo.Server.Data;

public class TableManager
{
    public Dictionary<Guid, Table> Tables = new();

    public IEnumerable<string> GetTables()
    {
        return Tables.Where(x => x.Value.State != TableState.Full).Select(x => x.Value.Name);
    }
}

public class Table
{
    public Table(string name, TableState state = TableState.Empty)
    {
        Name = name;
        State = state;
    }

    public string Name { get; set; }
    public TableState State { get; set; }
}


public enum TableState
{
    Empty, WaitingPlayer, Full
}