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
       ) into rankings
FROM (
         SELECT
             b."SleeperId",
             "FirstName",
             "LastName",
             "Position",
             "OverallRank" AS "OneQbRanking",
             "SFOverallRank" AS "TwoQbRanking"
         FROM "FantasyCalcMarketRankings" a
                  JOIN "DynastyDaddyPlayerData" b ON a."SleeperId" = b."SleeperId"
         WHERE a."IsDynasty" = isDynasty
     ) a;
END
$$;

