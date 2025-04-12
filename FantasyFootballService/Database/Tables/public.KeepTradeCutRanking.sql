create table "KeepTradeCutRanking"
(
    "Id"           varchar(10) not null
        primary key,
    "FirstName"    varchar(50),
    "LastName"     varchar(50),
    "Position"     varchar(10),
    "RankingOneQB" integer,
    "RankingTwoQB" integer,
    "Resource"     json,
    "UpdatedAt"    timestamp
);

create index "KeepTradeCutRanking_IND1"
    on "KeepTradeCutRanking" ("FirstName", "LastName");

create index "KeepTradeCutRanking_IND2"
    on "KeepTradeCutRanking" ("RankingOneQB", "RankingTwoQB");
