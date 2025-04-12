create table "SleeperRankings"
(
    "Id"                 varchar(50) not null
        primary key,
    "FirstName"          varchar(50),
    "LastName"           varchar(50),
    "Position"           varchar(10),
    "ADP2QB"             numeric,
    "ADPDynasty"         numeric,
    "ADPDynasty2QB"      numeric,
    "ADPDynastyHalfPPR"  numeric,
    "ADPDynastyPPR"      numeric,
    "ADPDynastyStandard" numeric,
    "ADPHalfPPR"         numeric,
    "ADPFullPPR"         numeric,
    "ADPRookie"          numeric,
    "ADPStandard"        numeric,
    "Resource"           json,
    "UpdatedAt"          timestamp
);

create index "SleeperRanking_IND1"
    on "SleeperRankings" ("FirstName", "LastName");
