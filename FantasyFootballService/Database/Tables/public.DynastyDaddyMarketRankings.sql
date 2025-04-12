create table "DynastyDaddyMarketRankings"
(
    "NameId"        varchar(50) not null,
    "Market"        varchar(50) not null,
    "OverallRank"   integer,
    "SFOverallRank" integer,
    "Resource"      json,
    "UpdatedAt"     timestamp,
    constraint "DynastyDaddyMarketRankings_PK"
        primary key ("NameId", "Market")
);
