using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FantasyFootballService.Interfaces;
using FantasyFootballService.Models.Sleeper;

namespace FantasyFootballService.Implementations;

public class SleeperService : ISleeperService
{
    private static string _sleeperBaseUrl = "https://api.sleeper.app";
    static HttpClient _client = new HttpClient();

    public async Task<Dictionary<string, SleeperPlayer>> GetAllPlayers()
    {
        var route = $"/v1/players/nfl";
        var response = await _client.GetAsync(_sleeperBaseUrl + route);
        var strResponse = await response.Content.ReadAsStringAsync();
        return string.IsNullOrWhiteSpace(strResponse)
            ? null
            : JsonSerializer.Deserialize<Dictionary<string, SleeperPlayer>>(strResponse);
    }

    public async Task<List<SleeperLeagueUser>> GetLeagueUsers(string leagueId)
    {
        var route = $"/v1/league/{leagueId}/users";
        var response = await _client.GetAsync(_sleeperBaseUrl + route);
        var strResponse = await response.Content.ReadAsStringAsync();
        return string.IsNullOrWhiteSpace(strResponse)
            ? null
            : JsonSerializer.Deserialize<List<SleeperLeagueUser>>(strResponse);
    }
    
    public async Task<List<SleeperMemberRoster>> GetLeagueRosters(string leagueId)
    {
        var route = $"/v1/league/{leagueId}/rosters";
        var response = await _client.GetAsync(_sleeperBaseUrl + route);
        var strResponse = await response.Content.ReadAsStringAsync();
        return string.IsNullOrWhiteSpace(strResponse)
            ? null
            : JsonSerializer.Deserialize<List<SleeperMemberRoster>>(strResponse);
    }

    public async Task<SleeperLeagueUser> GetUserByUsername(string username)
    {
        var route = $"/v1/user/{username}";
        var response = await _client.GetAsync(_sleeperBaseUrl + route);
        var strResponse = await response.Content.ReadAsStringAsync();
        return string.IsNullOrWhiteSpace(strResponse)
            ? null
            : JsonSerializer.Deserialize<SleeperLeagueUser>(strResponse);
    }
    
    public async Task<List<SleeperLeague>> GetUserLeagues(string userId, string sport, string year)
    {
        var route = $"/v1/user/{userId}/leagues/{sport}/{year}";
        var response = await _client.GetAsync(_sleeperBaseUrl + route);
        var strResponse = await response.Content.ReadAsStringAsync();
        return string.IsNullOrWhiteSpace(strResponse)
            ? null
            : JsonSerializer.Deserialize<List<SleeperLeague>>(strResponse);
    }
}