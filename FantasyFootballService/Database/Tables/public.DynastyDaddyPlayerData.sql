create table "DynastyDaddyPlayerData"
(
    "NameId"    varchar(50) not null
        primary key,
    "SleeperId" varchar(50) not null,
    "FirstName" varchar(50),
    "LastName"  varchar(50),
    "Position"  varchar(10),
    "Resource"  json,
    "UpdatedAt" timestamp
);

create index "DynastyDaddyPlayerData_IND1"
    on "DynastyDaddyPlayerData" ("SleeperId");