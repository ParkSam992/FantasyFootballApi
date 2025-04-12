using System.Collections.Generic;
using FantasyFootballService.Models;
using FantasyFootballService.Models.Enums;
using Npgsql;

namespace FantasyFootballService.Interfaces;

public interface IQueriesService
{
    List<Player> GetPlayerRankingByMarket(NpgsqlConnection conn, MarketEnum market);

    List<PlayerTradeValue> GetPlayerTradeValue(NpgsqlConnection conn);

    List<SelectOption> PlayerSearch(string name, NpgsqlConnection conn);
}