using CheckersDemo.Shared;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Json;

namespace CheckersDemo.Client.Managers;

public class HubManager
{
    private readonly HttpClient _httpClient;
    public HubConnection HubConnection { get; set; }

    public HubManager(HttpClient httpClient)
    {
        if (httpClient.BaseAddress == null) throw new ApplicationException($"{nameof(httpClient.BaseAddress)} must not be null");

        _httpClient = httpClient;

        HubConnection = new HubConnectionBuilder()
           .WithUrl(new Uri(_httpClient.BaseAddress, "connect"))
           .Build();
        _httpClient = httpClient;
    }

    public async Task<List<string>> RefreshTables()
    {
        if (_httpClient.BaseAddress == null) return new List<string>();
        return await _httpClient.GetFromJsonAsync<List<string>>(new Uri(_httpClient.BaseAddress, "checkers/getTables")) ?? new();
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
