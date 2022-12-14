using CheckersDemo.Client.Data;
using CheckersDemo.Server.Data;
using CheckersDemo.Shared;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace CheckersDemo.Server.Hubs;

public class MultiplayerHub : Hub
{
    private readonly TableManager _tableManager;

    public MultiplayerHub(TableManager tableManager)
    {
        _tableManager = tableManager;
    }

    public async Task CreateTable(Guid tableId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, tableId.ToString());
        _tableManager.Tables.Add(tableId, new Table(tableId.ToString()));
    }
    public async Task JoinTable(Guid tableId)
    {
        if (!_tableManager.Tables.ContainsKey(tableId) || _tableManager.Tables[tableId].State == TableState.Full) return;

        await Groups.AddToGroupAsync(Context.ConnectionId, tableId.ToString());
        await Clients.Groups(tableId.ToString()).SendAsync(HubMethodNames.JoinTable.On());
        _tableManager.Tables[tableId].State = TableState.Full;
    }

    public async Task Move(string tableId, Cell cellFrom, Cell cellTo)
    {
        await Clients.Groups(tableId).SendAsync(HubMethodNames.Move.On(), cellFrom, cellTo);
    }
}
