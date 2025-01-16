create function get_player_trade_value(OUT rankings json) returns json
    language plpgsql
as
$$
BEGIN
    SELECT JSON_AGG(
        JSON_BUILD_OBJECT(
            'SfTradeValue', a."SFTradeValue"::varchar,
            'TradeValue', a."TradeValue"::varchar,
            'SleeperId', a."SleeperId",
            'FirstName', a."FirstName",
            'LastName', a."LastName",
            'Position', a."Position"
        )
    ) into rankings
    FROM (
         SELECT
             AVG((a."Resource" ->> 'sf_trade_value')::integer) AS "SFTradeValue",
             AVG((a."Resource" ->> 'trade_value')::integer) AS "TradeValue",
             b."SleeperId",
             b."FirstName",
             b."LastName",
             b."Position"
         FROM "DynastyDaddyMarketRankings" a
         JOIN "DynastyDaddyPlayerData" b ON a."NameId" = b."NameId"
         GROUP BY
             b."SleeperId",
             b."FirstName",
             b."LastName",
             b."Position"
     ) a;
END
$$;
