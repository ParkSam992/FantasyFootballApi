using System.Collections.Generic;
using System.Threading.Tasks;
using FantasyFootballService.Models.Sleeper;

namespace FantasyFootballService.Interfaces;

public interface ICacheService
{
    Task<Dictionary<string, SleeperPlayer>> GetAllPlayersAsync();
    Task<List<SleeperLeagueUser>> GetSleeperLeagueUsersAsync(string leagueId);
}