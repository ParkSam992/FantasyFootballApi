using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FantasyFootballService.Interfaces;
using FantasyFootballService.Models;

namespace FantasyFootballService.Implementations;

public class SleeperApi : ISleeperApi
{
    private static string _sleeperBaseUrl = "https://api.sleeper.app";
    static HttpClient _client = new HttpClient();
    
    public async Task<List<LeagueRoster>> GetLeagueRosters(string leagueId)
    {
        var route = $"/v1/league/{leagueId}/rosters";
        var response = await _client.GetAsync(_sleeperBaseUrl + route);
        var strResponse = await response.Content.ReadAsStringAsync();
        return string.IsNullOrWhiteSpace(strResponse)
            ? null
            : JsonSerializer.Deserialize<List<LeagueRoster>>(strResponse);
    }
}