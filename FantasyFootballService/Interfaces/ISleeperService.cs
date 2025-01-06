using System.Collections.Generic;
using System.Threading.Tasks;
using FantasyFootballService.Models.Sleeper;

namespace FantasyFootballService.Interfaces;

public interface ISleeperService
{
    Task<Dictionary<string, SleeperPlayer>> GetAllPlayers();
    Task<List<SleeperLeagueUser>> GetLeagueUsers(string leagueId);
    Task<List<SleeperMemberRoster>> GetLeagueRosters(string leagueId);
    Task<SleeperLeagueUser> GetUserByUsername(string username);
    Task<List<SleeperLeague>> GetUserLeagues(string userId, string sport, string year);
    Task<SleeperGraphqlResponse> GetAlreadyDraftedPlayers(string draftId);
}