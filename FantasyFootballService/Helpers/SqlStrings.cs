namespace FantasyFootballService.Helpers;

public class SqlStrings
{
    // TODO: Currently weighting the combo of Dynasty Daddy, and sleeper the same, may want to weight differently
    public const string GET_AVERAGE_RANKINGS =
        @"WITH ""dynasty_daddy_rankings"" AS (
            SELECT
                ""SleeperId"",
                AVG(""OverallRank"") AS ""OverallRankAvg"",
                AVG(""SFOverallRank"") AS ""SFOverallRankAvg""
            FROM ""DynastyDaddyMarketRankings"" a
            JOIN ""DynastyDaddyPlayerData"" b ON a.""NameId"" = b.""NameId""
            WHERE b.""Position"" != 'PI'
              AND ""Market"" ILIKE '%' || @market
            GROUP BY ""SleeperId""
        ), 
        ""sleeper_rankings"" AS (
            SELECT
                ""Id"",
                CASE
                    WHEN @market ILIKE 'STD' THEN ""ADPFullPPR""
                    ELSE ""ADPDynastyPPR""
                END AS ""OneQBRanking"",
                CASE
                    WHEN @market ILIKE 'STD' THEN ""ADP2QB""
                    ELSE ""ADPDynasty2QB""
                END AS ""TwoQBRanking""
            FROM ""SleeperRankings"" a
            WHERE ""Position"" IN ('QB', 'RB', 'WR', 'TE')
        )
        SELECT JSON_AGG(
            JSON_BUILD_OBJECT(
                'SleeperId', a.""Id"",
                'FirstName', c.""FirstName"",
                'LastName', c.""LastName"",
                'Position', c.""Position"",
                'Market', CASE
                             WHEN @market ILIKE 'STD' THEN 'STD_AVERAGE'
                             ELSE 'DYN_AVERAGE'
                          END,
                'OneQbRanking', ((b.""OverallRankAvg"" + a.""OneQBRanking"") / 2.0)::varchar,
                'TwoQbRanking', ((b.""SFOverallRankAvg"" + a.""TwoQBRanking"") / 2.0)::varchar
            )
        )
        FROM ""sleeper_rankings"" a
        JOIN ""dynasty_daddy_rankings"" b ON a.""Id"" = b.""SleeperId""
        JOIN ""DynastyDaddyPlayerData"" c ON a.""Id"" = c.""SleeperId"";";

    public const string GET_SLEEPER_RANKINGS =
        @"SELECT JSON_AGG(
            JSON_BUILD_OBJECT(
                'Market', 'STD_SLEEPER',
                'SleeperId', ""Id"",
                'OneQbRanking', CASE
                    WHEN @market ILIKE 'STD' THEN ""ADPFullPPR""::varchar
                    ELSE ""ADPDynastyPPR""::varchar
                END,
                'TwoQbRanking', CASE
                    WHEN @market ILIKE 'STD' THEN ""ADP2QB""::varchar
                    ELSE ""ADPDynasty2QB""::varchar
                END,
                'FirstName', a.""FirstName"",
                'LastName', a.""LastName"",
                'Position', a.""Position""
            )
        )
        FROM ""SleeperRankings"" a
        JOIN ""DynastyDaddyPlayerData"" b ON a.""Id"" = b.""SleeperId"";";

    public const string GET_DYNASTY_DADDY_RANKINGS =
        @"SELECT JSON_AGG(
            JSON_BUILD_OBJECT(
                'SleeperId', a.""SleeperId"",
                'FirstName', a.""FirstName"",
                'LastName', a.""LastName"",
                'Position', a.""Position"",
                'Market', CASE
                             WHEN 'std' ILIKE 'STD' THEN 'STD_DYNASTY_DADDY'
                             ELSE 'DYN_DYNASTY_DADDY'
                          END,
                'OneQbRanking', a.""OneQbRanking""::varchar,
                'TwoQbRanking', a.""TwoQbRanking""::varchar
            )
        )
        FROM (
            SELECT 
                ""SleeperId"",
                ""FirstName"",
                ""LastName"",
                ""Position"",
                AVG(""OverallRank"") AS ""OneQbRanking"",
                AVG(""SFOverallRank"") AS ""TwoQbRanking""
            FROM ""DynastyDaddyMarketRankings"" a
            LEFT JOIN ""DynastyDaddyPlayerData"" b ON a.""NameId"" = b.""NameId""
            WHERE b.""Position"" != 'PI'
              AND ""Market"" ILIKE '%' || @market
            GROUP BY ""SleeperId"", ""FirstName"", ""LastName"", ""Position""
        ) a;";

    public const string GET_PLAYER_TRADE_VALUE =
        @"SELECT JSON_AGG(
            JSON_BUILD_OBJECT(
                'SfTradeValue', a.""SFTradeValue""::varchar,
                'TradeValue', a.""TradeValue""::varchar,
                'SleeperId', a.""SleeperId"",
                'FirstName', a.""FirstName"",
                'LastName', a.""LastName"",
                'Position', a.""Position""
            )
        )
        FROM (
            SELECT
                AVG((a.""Resource"" ->> 'sf_trade_value')::integer) AS ""SFTradeValue"",
                AVG((a.""Resource"" ->> 'trade_value')::integer) AS ""TradeValue"",
                b.""SleeperId"",
                b.""FirstName"",
                b.""LastName"",
                b.""Position""
            FROM ""DynastyDaddyMarketRankings"" a
            JOIN ""DynastyDaddyPlayerData"" b ON a.""NameId"" = b.""NameId""
            GROUP BY
                b.""SleeperId"",
                b.""FirstName"",
                b.""LastName"",
                b.""Position""
        ) a;";
}