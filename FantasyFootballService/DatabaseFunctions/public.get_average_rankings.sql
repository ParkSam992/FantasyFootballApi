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
FROM "SleeperRankings" a
WHERE "Position" IN ('QB', 'RB', 'WR', 'TE')
    ),
    fantasy_calc_rankings AS (
SELECT
    "SleeperId",
    "OverallRank" AS "OneQbRanking",
    "SFOverallRank" AS "TwoQbRanking"
FROM "FantasyCalcMarketRankings" a
WHERE a."IsDynasty" = (market = 'DYN')
    )
SELECT JSON_AGG(
               JSON_BUILD_OBJECT(
                       'SleeperId', a."Id",
                       'FirstName', pd."FirstName",
                       'LastName', pd."LastName",
                       'Position', pd."Position",
                       'Market', CASE
                                     WHEN market ILIKE 'STD' THEN 'STD_AVERAGE'
                                     ELSE 'DYN_AVERAGE'
                           END,
                       'OneQbRanking', ((a."OneQBRanking") + (b."OverallRankAvg" * 3) + c."OneQbRanking" / 5.0)::varchar,
                       'TwoQbRanking', ((a."TwoQBRanking") + (b."SFOverallRankAvg" * 3) + c."TwoQbRanking" / 5.0)::varchar
               )
       ) into rankings
FROM sleeper_rankings a
         JOIN dynasty_daddy_rankings b ON a."Id" = b."SleeperId"
         JOIN fantasy_calc_rankings c ON a."Id" = c."SleeperId"
         JOIN "DynastyDaddyPlayerData" pd ON a."Id" = pd."SleeperId";
END
$$;