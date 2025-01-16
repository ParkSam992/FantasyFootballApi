CREATE OR REPLACE FUNCTION get_average_rankings(market character varying, out rankings json)
RETURNS JSON
language plpgsql
AS
$$
BEGIN
    -- TODO: The weight of DynastyDaddy and Sleeper averages is 2/1 right now, might want to adjust at some point
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
        )
    SELECT JSON_AGG(
        JSON_BUILD_OBJECT(
            'SleeperId', a."Id",
            'FirstName', c."FirstName",
            'LastName', c."LastName",
            'Position', c."Position",
            'Market', CASE
                        WHEN market ILIKE 'STD' THEN 'STD_AVERAGE'
                        ELSE 'DYN_AVERAGE'
                      END,
            'OneQbRanking', (((b."OverallRankAvg" * 2) + a."OneQBRanking") / 3.0)::varchar,
            'TwoQbRanking', (((b."SFOverallRankAvg" * 2) + a."TwoQBRanking") / 3.0)::varchar
        )
    ) into rankings
    FROM sleeper_rankings a
    JOIN dynasty_daddy_rankings b ON a."Id" = b."SleeperId"
    JOIN "DynastyDaddyPlayerData" c ON a."Id" = c."SleeperId";
END
$$;