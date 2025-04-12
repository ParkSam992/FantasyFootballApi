create function get_average_rankings(market character varying, OUT rankings json) returns json
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
    AND "Market" ILIKE '%' || market
GROUP BY "SleeperId"
    ),
    sleeper_rankings AS (
SELECT
    "Id",
    CASE
    WHEN market ILIKE 'STD' THEN "ADPFullPPR"
    ELSE "ADPDynastyPPR"
    END AS "OneQBRanking",
    CASE
    WHEN market ILIKE 'STD' THEN "ADP2QB"
    ELSE "ADPDynasty2QB"
    END AS "TwoQBRanking"
FROM "SleeperRankings"
WHERE "Position" IN ('QB', 'RB', 'WR', 'TE')
    ),
    fantasy_calc_rankings AS (
SELECT
    a."SleeperId",
    a."OverallRank" AS "OneQbRanking",
    b."OverallRank" AS "TwoQbRanking"
FROM "FantasyCalcMarketRankings" a
    JOIN "FantasyCalcMarketRankings" b ON a."SleeperId" = b."SleeperId" AND b."IsOneQb" = false
WHERE a."IsDynasty" = (market = 'DYN')
  AND b."IsDynasty" = (market = 'DYN')
  AND a."IsOneQb" = true
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
                       'Market', CASE WHEN market ILIKE 'STD' THEN 'STD_AVERAGE' ELSE 'DYN_AVERAGE' END,
                       'OneQbRanking', "OneQbRankIndex"::varchar,
                       'TwoQbRanking', "TwoQbRankIndex"::varchar
               )
       ) INTO rankings
FROM ranked_players;
END
$$;

alter function get_average_rankings(varchar, out json) owner to sampark99;

