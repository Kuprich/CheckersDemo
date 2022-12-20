using CheckersDemo.Shared;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Json;
using System.Xml.Linq;

namespace CheckersDemo.Client.Managers;

public class HubManager
{
    public HubConnection HubConnection { get; set; }

    private HttpClient _httpClient = new();
    public HubManager()
	{
        HubConnection = new HubConnectionBuilder()
           .WithUrl("https://localhost:44314/connect")
           .Build();
    }

    public async Task<List<string>> RefreshTables()
    {
        return await _httpClient.GetFromJsonAsync<List<string>>("https://localhost:44314/Checkers/GetTables") ?? new();
    }
    public async Task CreateGame(string tableId)
    {
        await HubConnection.StartAsync();
        await HubConnection.SendAsync(HubMethodNames.CreateTable, tableId);
    }

    public async Task JoinGame(string tableId)
    {
        await HubConnection.StartAsync();
        await HubConnection.SendAsync(HubMethodNames.JoinTable, tableId);
    }
}
