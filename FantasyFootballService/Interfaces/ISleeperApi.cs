using System.Collections.Generic;
using System.Threading.Tasks;
using FantasyFootballService.Models;

namespace FantasyFootballService.Interfaces;

public interface ISleeperApi
{
    Task<List<LeagueRoster>> GetLeagueRosters(string leagueId);
}