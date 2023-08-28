CREATE TABLE IF NOT EXISTS "Urls" (
    "Id" UUID NOT NULL PRIMARY KEY, 
    "LongUrl" TEXT NOT NULL UNIQUE,
    "ShortPath" TEXT UNIQUE,
    "CreatedDate" TIMESTAMPTZ,
    "LastRequestedDate" TIMESTAMPTZ,
    "RequestCounter" INT,
    "ExpireDate" TIMESTAMPTZ,
    "IsPublic" BOOLEAN
);

CREATE TABLE IF NOT EXISTS "OutboxMessages" (
    "Id" UUID NOT NULL PRIMARY KEY, 
    "EventType" TEXT NOT NULL,
    "EventPayload" TEXT NOT NULL,
    "EventDate" TIMESTAMPTZ NOT NULL,
    "IsSent" BOOLEAN,
    "SentDate" TIMESTAMPTZ
);


-- COPY "Urls"("Id", "LongUrl", "ShortPath", "CreatedDate", "LastRequestedDate", "RequestCounter", "ExpireDate", "IsPublic")
-- FROM '/docker-entrypoint-initdb.d/MOCK_DATA.csv' DELIMITER ',' CSV HEADER;

-- INSERT INTO public."Urls" ("Id", "LongUrl", "ShortPath", "CreatedDate", "LastRequestedDate", "RequestCounter", "ExpireDate", "IsPublic")
-- VALUES('8ac64b1f-f258-4c84-9222-74f842376d1e', 'https://recepgunes.blog', NULL, '0001-01-01', '0001-01-01', 0, '0001-01-01', true);

