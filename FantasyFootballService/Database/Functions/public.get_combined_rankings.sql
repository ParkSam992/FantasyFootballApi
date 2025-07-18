
create function get_combined_rankings(OUT rankings json) returns json
    language plpgsql
as
$$
BEGIN
    -- TODO: The weight of DynastyDaddy and Sleeper averages is 3/1 right now, might want to adjust at some point
    WITH dynasty_daddy_rankings AS (
        SELECT
            "SleeperId",
            AVG("OverallRank") AS "OverallRankAvg",
            AVG("SFOverallRank") AS "SFOverallRankAvg"
        FROM "DynastyDaddyMarketRankings" a
                 JOIN "DynastyDaddyPlayerData" b ON a."NameId" = b."NameId"
        WHERE b."Position" != 'PI'
    GROUP BY "SleeperId"
        ),
        sleeper_rankings AS (
    SELECT
        "Id",
        "ADPFullPPR" + "ADPDynastyPPR" / 2 as "OneQBRanking",
        "ADP2QB" + "ADPDynasty2QB" / 2 as "TwoQBRanking"
    FROM "SleeperRankings"
    WHERE "Position" IN ('QB', 'RB', 'WR', 'TE')
        ),
        fantasy_calc_rankings AS (
    SELECT a."SleeperId",
        AVG("OverallRank") FILTER (WHERE a."IsOneQb" = true)  AS "OneQbRanking",
        AVG("OverallRank") FILTER (WHERE a."IsOneQb" = false) AS "TwoQbRanking"
    FROM "FantasyCalcMarketRankings" a
    group by "SleeperId"
        ),
        combined_rankings AS (
    SELECT
        a."Id" AS "SleeperId",
        pd."FirstName",
        pd."LastName",
        pd."Position",
        (a."OneQBRanking") + (b."OverallRankAvg" * 3) + c."OneQbRanking" / 5.0 AS "OneQbRanking",
        (a."TwoQBRanking") + (b."SFOverallRankAvg" * 3) + c."TwoQbRanking" / 5.0 AS "TwoQbRanking"
    FROM sleeper_rankings a
        JOIN dynasty_daddy_rankings b ON a."Id" = b."SleeperId"
        JOIN fantasy_calc_rankings c ON a."Id" = c."SleeperId"
        JOIN "DynastyDaddyPlayerData" pd ON a."Id" = pd."SleeperId"
        ),
        ranked_players AS (
    SELECT *,
        RANK() OVER (ORDER BY "OneQbRanking") AS "OneQbRankIndex",
        RANK() OVER (ORDER BY "TwoQbRanking") AS "TwoQbRankIndex"
    FROM combined_rankings
        )
    SELECT JSON_AGG(
                   JSON_BUILD_OBJECT(
                           'SleeperId', "SleeperId",
                           'FirstName', "FirstName",
                           'LastName', "LastName",
                           'Position', "Position",
                           'OneQbRanking', "OneQbRankIndex"::varchar,
                           'TwoQbRanking', "TwoQbRankIndex"::varchar
                   )
           )
    INTO rankings
    FROM ranked_players;
END
$$;