create table "FantasyCalcMarketRankings"
(
    "SleeperId"   varchar(50) not null,
    "IsDynasty"   boolean     not null,
    "IsOneQb"     boolean     not null,
    "OverallRank" integer,
    "OverallADP"  integer,
    "Resource"    json,
    "UpdatedAt"   timestamp,
    constraint "FantasyCalcMarketRankings_PK"
        primary key ("SleeperId", "IsDynasty", "IsOneQb")
);

create index "FantasyCalcMarketRankings_IND1"
    on "FantasyCalcMarketRankings" ("IsDynasty");

create index "FantasyCalcMarketRankings_IND2"
    on "FantasyCalcMarketRankings" ("IsOneQb");