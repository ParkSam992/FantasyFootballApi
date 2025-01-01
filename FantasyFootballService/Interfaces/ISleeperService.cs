using System.Collections.Generic;
using System.Threading.Tasks;
using FantasyFootballService.Models.Sleeper;

namespace FantasyFootballService.Interfaces;

public interface ISleeperService
{
    Task<Dictionary<string, SleeperPlayer>> GetAllPlayers();
    Task<List<SleeperLeagueUser>> GetLeagueUsers(string leagueId);
    Task<List<SleeperMemberRoster>> GetLeagueRosters(string leagueId);
}