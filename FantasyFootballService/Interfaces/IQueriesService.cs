using System.Collections.Generic;
using System.Threading.Tasks;
using FantasyFootballService.Models;
using FantasyFootballService.Models.Enums;
using Npgsql;

namespace FantasyFootballService.Interfaces;

public interface IQueriesService
{
    Task<List<Player>> GetPlayerRankingByMarket(NpgsqlConnection conn, MarketEnum market);
}