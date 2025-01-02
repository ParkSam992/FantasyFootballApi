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
    public List<Player> GetPlayerRankingByMarket(NpgsqlConnection conn, MarketEnum market)
    {
        return market switch
        {
            MarketEnum.STD_AVERAGE => GetAverageRankings(conn),
            MarketEnum.STD_SLEEPER => GetSleeperRankings(conn),
            MarketEnum.STD_DYNASTY_DADDY => GetDynastyDaddyRankings(conn),
            MarketEnum.DYN_AVERAGE => GetAverageRankings(conn, true),
            MarketEnum.DYN_SLEEPER => GetSleeperRankings(conn, true),
            MarketEnum.DYN_DYNASTY_DADDY => GetDynastyDaddyRankings(conn, true),
            _ => new List<Player>()
        };
    }

    public List<PlayerTradeValue> GetPlayerTradeValue(NpgsqlConnection conn)
    {
        var cmd = PostgresCommandHelper.GetPlayerTradeValue(conn);
        var strResponse = Convert.ToString(cmd.ExecuteScalar());
        return string.IsNullOrWhiteSpace(strResponse) 
            ? new List<PlayerTradeValue>() 
            : JsonSerializer.Deserialize<List<PlayerTradeValue>>(strResponse);
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