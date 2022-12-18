using CheckersDemo.Client.Data;
using CheckersDemo.Server.Data;
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
    public async Task JoinTable(string tableId)
    {
        if (_tableManager.Tables.ContainsKey(tableId))
        {
            if (_tableManager.Tables[tableId] < 2)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, tableId);
                await Clients.Groups(tableId).SendAsync("JoinTableInvoked");
                //await Clients.GroupExcept(tableId, Context.ConnectionId).SendAsync("JoinTableInvoked");
                _tableManager.Tables[tableId]++;
            }
        }

        else
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, tableId);
            _tableManager.Tables.Add(tableId, 1);
        }
    }

    public async Task Move(string tableId, Checker checker, Cell cell)
    {
        await Clients.GroupExcept(tableId, Context.ConnectionId)
            .SendAsync("MoveInvoked", checker, cell);
    }
}
