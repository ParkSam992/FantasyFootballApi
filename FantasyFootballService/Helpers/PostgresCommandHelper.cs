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
    
    public static NpgsqlCommand GetDynastyDaddyRankings(NpgsqlConnection conn, bool isDynasty)
    {
        var cmd = new NpgsqlCommand(SqlStrings.GET_DYNASTY_DADDY_RANKINGS, conn);
        cmd.Parameters.AddWithValue("market", NpgsqlDbType.Varchar, isDynasty ? "DYN" : "STD");
        cmd.Prepare();
        return cmd;
    }
}