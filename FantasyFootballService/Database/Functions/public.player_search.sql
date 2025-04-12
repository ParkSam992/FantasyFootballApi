create function player_search(player_name character varying, OUT players json) returns json
    language plpgsql
as
$$
BEGIN
    SELECT json_agg(json_build_object('label', "FirstName" || ' ' || "LastName", 'value', "SleeperId"))
    INTO players
    FROM "DynastyDaddyPlayerData" a
    WHERE "FirstName" || "LastName" ILIKE replace('%'|| player_name || '%', ' ', '');
END
$$;

alter function player_search(varchar, out json) owner to sampark99;
