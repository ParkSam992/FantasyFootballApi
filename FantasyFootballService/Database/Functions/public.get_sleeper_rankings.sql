create function get_sleeper_rankings(market character varying, OUT rankings json) returns json
    language plpgsql
as
$$
BEGIN
SELECT JSON_AGG(
               JSON_BUILD_OBJECT(
                       'Market', market || '_SLEEPER',
                       'SleeperId', "Id",
                       'OneQbRanking', CASE
                                           WHEN market ILIKE 'STD' THEN "ADPFullPPR"::varchar
                ELSE "ADPDynastyPPR"::varchar
            END,
                       'TwoQbRanking', CASE
                                           WHEN market ILIKE 'STD' THEN "ADP2QB"::varchar
                ELSE "ADPDynasty2QB"::varchar
            END,
                       'FirstName', a."FirstName",
                       'LastName', a."LastName",
                       'Position', a."Position"
               )
       ) into rankings
FROM "SleeperRankings" a
         JOIN "DynastyDaddyPlayerData" b ON a."Id" = b."SleeperId";
END
$$;

alter function get_sleeper_rankings(varchar, out json) owner to sampark99;

