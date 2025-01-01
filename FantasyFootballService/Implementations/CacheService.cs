using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFootballService.Interfaces;
using FantasyFootballService.Models.Sleeper;

namespace FantasyFootballService.Implementations;

public class CacheService : ICacheService
{
    private readonly ISleeperService _sleeperService;
    private Dictionary<string, SleeperPlayer> _playerData = new ();
    private Dictionary<string, List<SleeperLeagueUser>> _sleeperLeagueUsers = new();
    private DateTime _lastCacheRefresh;
    
    public CacheService(ISleeperService sleeperService)
    {
        _sleeperService = sleeperService;
    }


    public async Task<Dictionary<string, SleeperPlayer>> GetAllPlayersAsync()
    {
        if (_playerData.Any() || _lastCacheRefresh.AddDays(1) >= DateTime.UtcNow)
        {
            return _playerData;
        }
        
        _playerData = await _sleeperService.GetAllPlayers();
        _lastCacheRefresh = DateTime.UtcNow;
        return _playerData;
    }

    public async Task<List<SleeperLeagueUser>> GetSleeperLeagueUsersAsync(string leagueId)
    {
        if (_sleeperLeagueUsers.ContainsKey(leagueId) && _lastCacheRefresh.AddDays(1) >= DateTime.UtcNow)
        {
            return _sleeperLeagueUsers[leagueId];
        }

        var leagueUsers = await _sleeperService.GetLeagueUsers(leagueId);
        _sleeperLeagueUsers.Add(leagueId, leagueUsers);
        _lastCacheRefresh = DateTime.UtcNow;
        return leagueUsers;
    }
}