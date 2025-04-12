using Npgsql;
using NpgsqlTypes;

namespace FantasyFootballService.Helpers;

public static class PostgresCommandHelper
{
    public static NpgsqlCommand GetAverageRankings(NpgsqlConnection conn, bool isDynasty)
    {
        var cmd = new NpgsqlCommand(SqlStrings.GET_AVERAGE_RANKINGS, conn);
        cmd.Parameters.AddWithValue("market", NpgsqlDbType.Varchar, isDynasty ? "DYN" : "STD");
        cmd.Prepare();
        return cmd;
    }
    
    public static NpgsqlCommand GetSleeperRankings(NpgsqlConnection conn, bool isDynasty)
    {
        var cmd = new NpgsqlCommand(SqlStrings.GET_SLEEPER_RANKINGS, conn);
        cmd.Parameters.AddWithValue("market", NpgsqlDbType.Varchar, isDynasty ? "DYN" : "STD");
        cmd.Prepare();
        return cmd;
    }
    
    public static NpgsqlCommand GetDynastyDaddyAverageRankings(NpgsqlConnection conn, bool isDynasty)
    {
        var cmd = new NpgsqlCommand(SqlStrings.GET_DYNASTY_DADDY_AVERAGE_RANKINGS, conn);
        cmd.Parameters.AddWithValue("market", NpgsqlDbType.Varchar, isDynasty ? "DYN" : "STD");
        cmd.Prepare();
        return cmd;
    }
    
    public static NpgsqlCommand GetKeepTradeCutRankings(NpgsqlConnection conn, bool isDynasty)
    {
        var cmd = new NpgsqlCommand(SqlStrings.GET_KEEP_TRADE_CUT_RANKINGS, conn);
        cmd.Parameters.AddWithValue("market", NpgsqlDbType.Varchar, isDynasty ? "DYN" : "STD");
        cmd.Prepare();
        return cmd;
    }
    
    public static NpgsqlCommand GetFantasyCalcRankings(NpgsqlConnection conn, bool isDynasty)
    {
        var cmd = new NpgsqlCommand(SqlStrings.GET_FANTASY_CALC_RANKINGS, conn);
        cmd.Parameters.AddWithValue("isDynasty", NpgsqlDbType.Boolean, isDynasty);
        cmd.Prepare();
        return cmd;
    }
    
    public static NpgsqlCommand GetPlayerTradeValue(NpgsqlConnection conn)
    {
        var cmd = new NpgsqlCommand(SqlStrings.GET_PLAYER_TRADE_VALUE, conn);
        cmd.Prepare();
        return cmd;
    }

    public static NpgsqlCommand PlayerSearch(string name, NpgsqlConnection conn)
    {
        var cmd = new NpgsqlCommand(SqlStrings.PLAYER_SEARCH, conn);
        cmd.Parameters.AddWithValue("name", NpgsqlDbType.Varchar, name);
        cmd.Prepare();
        return cmd;
    }
}