create function get_keep_trade_cut_rankings(market character varying, OUT rankings json) returns json
    language plpgsql
as
$$
BEGIN
    SELECT JSON_AGG(
        JSON_BUILD_OBJECT(
            'SleeperId', a."SleeperId",
            'FirstName', a."FirstName",
            'LastName', a."LastName",
            'Position', a."Position",
            'Market', CASE
                         WHEN market ILIKE 'STD' THEN 'KEEP_TRADE_CUT_STD'
                         ELSE 'KEEP_TRADE_CUT_DYN'
                      END,
            'OneQbRanking', a."OneQbRanking"::varchar,
            'TwoQbRanking', a."TwoQbRanking"::varchar
        )
    ) into rankings
    FROM (
         SELECT
             "SleeperId",
             "FirstName",
             "LastName",
             "Position",
             AVG("OverallRank") AS "OneQbRanking",
             AVG("SFOverallRank") AS "TwoQbRanking"
         FROM "DynastyDaddyMarketRankings" a
                  LEFT JOIN "DynastyDaddyPlayerData" b ON a."NameId" = b."NameId"
         WHERE b."Position" != 'PI'
          AND ("Market" = 'KEEP_TRADE_CUT_DYN' OR "Market" = 'KEEP_TRADE_CUT_STD')
          AND "Market" ILIKE '%' || market
         GROUP BY "SleeperId", "FirstName", "LastName", "Position"
     ) a;
END
$$;

alter function get_keep_trade_cut_rankings(varchar, out json) owner to sampark99;

