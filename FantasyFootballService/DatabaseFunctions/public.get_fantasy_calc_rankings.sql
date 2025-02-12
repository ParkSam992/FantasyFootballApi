create function get_fantasy_calc_rankings(isdynasty boolean, OUT rankings json) returns json
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
                                     WHEN isDynasty THEN 'FANTASY_CALC_DYN'
                                     ELSE 'FANTASY_CALC_STD'
                           END,
                       'OneQbRanking', a."OneQbRanking"::varchar,
                       'TwoQbRanking', a."TwoQbRanking"::varchar
               )
       ) INTO rankings
FROM (
         SELECT
             b."SleeperId",
             "FirstName",
             "LastName",
             "Position",
             b."OverallRank" AS "OneQbRanking",
             a."OverallRank" AS "TwoQbRanking"
         FROM "FantasyCalcMarketRankings" a
                  JOIN "FantasyCalcMarketRankings" b ON a."SleeperId" = b."SleeperId" AND b."IsOneQb"
                  JOIN "DynastyDaddyPlayerData" c ON a."SleeperId" = c."SleeperId"
         WHERE a."IsDynasty" = isDynasty
           AND b."IsDynasty" = isDynasty
           AND NOT a."IsOneQb"
     ) a;
END
$$;

alter function get_fantasy_calc_rankings(boolean, out json) owner to sampark99;

