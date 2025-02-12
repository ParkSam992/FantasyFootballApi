using System;
using System.Collections.Generic;
using System.Text.Json;
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
        // TODO: I want any average rankings to use rank not ADP. I might want rank on everything not adp
        return market switch
        {
            MarketEnum.STD_AVERAGE => GetAverageRankings(conn),
            MarketEnum.STD_SLEEPER => GetSleeperRankings(conn),
            MarketEnum.STD_DYNASTY_DADDY_AVG => GetDynastyDaddyAverageRankings(conn),
            MarketEnum.STD_KEEP_TRADE_CUT => GetKeepTradeCutRankings(conn),
            MarketEnum.STD_FANTASY_CALC => GetFantasyCalcRankings(conn),
            MarketEnum.DYN_AVERAGE => GetAverageRankings(conn, true),
            MarketEnum.DYN_SLEEPER => GetSleeperRankings(conn, true),
            MarketEnum.DYN_DYNASTY_DADDY_AVG => GetDynastyDaddyAverageRankings(conn, true),
            MarketEnum.DYN_KEEP_TRADE_CUT => GetKeepTradeCutRankings(conn, true),
            MarketEnum.DYN_FANTASY_CALC => GetFantasyCalcRankings(conn, true),
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

    private List<Player> GetAverageRankings(NpgsqlConnection conn, bool isDynasty = false)
    {
        var cmd = PostgresCommandHelper.GetAverageRankings(conn, isDynasty);
        var strResponse = Convert.ToString(cmd.ExecuteScalar());
        return string.IsNullOrWhiteSpace(strResponse) 
            ? new List<Player>() 
            : JsonSerializer.Deserialize<List<Player>>(strResponse);
    }
    
    private List<Player> GetSleeperRankings(NpgsqlConnection conn, bool isDynasty = false)
    {
        var cmd = PostgresCommandHelper.GetSleeperRankings(conn, isDynasty);
        var strResponse = Convert.ToString(cmd.ExecuteScalar());
        return string.IsNullOrWhiteSpace(strResponse) 
            ? new List<Player>() 
            : JsonSerializer.Deserialize<List<Player>>(strResponse);
    }
    
    private List<Player> GetKeepTradeCutRankings(NpgsqlConnection conn, bool isDynasty = false)
    {
        var cmd = PostgresCommandHelper.GetKeepTradeCutRankings(conn, isDynasty);
        var strResponse = Convert.ToString(cmd.ExecuteScalar());
        return string.IsNullOrWhiteSpace(strResponse) 
            ? new List<Player>() 
            : JsonSerializer.Deserialize<List<Player>>(strResponse);
    }
    
    private List<Player> GetDynastyDaddyAverageRankings(NpgsqlConnection conn, bool isDynasty = false)
    {
        var cmd = PostgresCommandHelper.GetDynastyDaddyAverageRankings(conn, isDynasty);
        var strResponse = Convert.ToString(cmd.ExecuteScalar());
        return string.IsNullOrWhiteSpace(strResponse) 
            ? new List<Player>() 
            : JsonSerializer.Deserialize<List<Player>>(strResponse);
    }
    
    private List<Player> GetFantasyCalcRankings(NpgsqlConnection conn, bool isDynasty = false)
    {
        var cmd = PostgresCommandHelper.GetFantasyCalcRankings(conn, isDynasty);
        var strResponse = Convert.ToString(cmd.ExecuteScalar());
        return string.IsNullOrWhiteSpace(strResponse) 
            ? new List<Player>() 
            : JsonSerializer.Deserialize<List<Player>>(strResponse);
    }
}