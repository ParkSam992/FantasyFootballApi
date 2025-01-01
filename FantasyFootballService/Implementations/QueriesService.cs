using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using FantasyFootballService.Helpers;
using FantasyFootballService.Interfaces;
using FantasyFootballService.Models;
using FantasyFootballService.Models.Enums;
using Npgsql;

namespace FantasyFootballService.Implementations;

public class QueriesService : IQueriesService
{
    public async Task<List<Player>> GetPlayerRankingByMarket(NpgsqlConnection conn, MarketEnum market)
    {
        switch (market)
        {
            case MarketEnum.STD_AVERAGE:
                return GetAverageRankings(conn);
            case MarketEnum.STD_SLEEPER:
                return GetSleeperRankings(conn);
            case MarketEnum.STD_DYNASTY_DADDY:
                return GetDynastyDaddyRankings(conn);
            case MarketEnum.DYN_AVERAGE:
                return GetAverageRankings(conn, true);
            case MarketEnum.DYN_SLEEPER:
                return GetSleeperRankings(conn, true);
            case MarketEnum.DYN_DYNASTY_DADDY:
                return GetDynastyDaddyRankings(conn, true);
        }

        return new List<Player>();
    }

    public List<Player> GetAverageRankings(NpgsqlConnection conn, bool isDynasty = false)
    {
        var cmd = PostgresCommandHelper.GetAverageRankings(conn, isDynasty);
        var strResponse = Convert.ToString(cmd.ExecuteScalar());
        return string.IsNullOrWhiteSpace(strResponse) 
            ? new List<Player>() 
            : JsonSerializer.Deserialize<List<Player>>(strResponse);
    }
    
    public List<Player> GetSleeperRankings(NpgsqlConnection conn, bool isDynasty = false)
    {
        var cmd = PostgresCommandHelper.GetSleeperRankings(conn, isDynasty);
        var strResponse = Convert.ToString(cmd.ExecuteScalar());
        return string.IsNullOrWhiteSpace(strResponse) 
            ? new List<Player>() 
            : JsonSerializer.Deserialize<List<Player>>(strResponse);
    }
    
    public List<Player> GetDynastyDaddyRankings(NpgsqlConnection conn, bool isDynasty = false)
    {
        var cmd = PostgresCommandHelper.GetDynastyDaddyRankings(conn, isDynasty);
        var strResponse = Convert.ToString(cmd.ExecuteScalar());
        return string.IsNullOrWhiteSpace(strResponse) 
            ? new List<Player>() 
            : JsonSerializer.Deserialize<List<Player>>(strResponse);
    }
}