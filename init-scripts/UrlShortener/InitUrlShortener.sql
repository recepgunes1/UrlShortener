CREATE TABLE IF NOT EXISTS "Urls" (
    "Id" UUID NOT NULL PRIMARY KEY, 
    "LongUrl" TEXT NOT NULL UNIQUE,
    "ShortPath" TEXT UNIQUE,
    "CreatedDate" DATE,
    "LastRequestedDate" DATE,
    "RequestCounter" INT,
    "ExpireDate" DATE,
    "IsPublic" BOOLEAN
);

COPY "Urls"("Id", "LongUrl", "ShortPath", "CreatedDate", "LastRequestedDate", "RequestCounter", "ExpireDate", "IsPublic")
FROM '/docker-entrypoint-initdb.d/MOCK_DATA.csv' DELIMITER ',' CSV HEADER;
INSERT INTO public."Urls" ("Id", "LongUrl", "ShortPath", "CreatedDate", "LastRequestedDate", "RequestCounter", "ExpireDate", "IsPublic")
VALUES('8ac64b1f-f258-4c84-9222-74f842376d1e', 'https://recepgunes.blog', NULL, '1-1-1', '1-1-1', 0, '1-1-1', true);
