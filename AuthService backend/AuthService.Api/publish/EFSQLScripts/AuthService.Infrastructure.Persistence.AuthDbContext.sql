CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260219110801_init') THEN
    CREATE TABLE addresses (
        id uuid NOT NULL DEFAULT (gen_random_uuid()),
        address_line_one character varying(255),
        address_line_two character varying(255),
        city character varying(255),
        state character varying(255),
        zip character varying(255),
        country character varying(255),
        created_at timestamp with time zone NOT NULL DEFAULT (now()),
        updated_at timestamp with time zone NOT NULL DEFAULT (now()),
        CONSTRAINT "PK_addresses" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260219110801_init') THEN
    CREATE TABLE applications (
        id uuid NOT NULL DEFAULT (gen_random_uuid()),
        name character varying(255),
        organization_id uuid NOT NULL,
        client_id uuid NOT NULL,
        client_secret character varying(255),
        is_enabled boolean NOT NULL DEFAULT TRUE,
        soft_deleted_at timestamp with time zone DEFAULT (now()),
        created_at timestamp with time zone NOT NULL DEFAULT (now()),
        updated_at timestamp with time zone NOT NULL DEFAULT (now()),
        CONSTRAINT "PK_applications" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260219110801_init') THEN
    CREATE TABLE users (
        id uuid NOT NULL DEFAULT (gen_random_uuid()),
        email character varying(255) NOT NULL,
        first_name character varying(255),
        last_name character varying(255),
        display_name character varying(255),
        avatar character varying(255),
        password character varying(255) NOT NULL,
        is_verified boolean NOT NULL DEFAULT FALSE,
        is_enabled boolean NOT NULL DEFAULT FALSE,
        is_active boolean NOT NULL DEFAULT FALSE,
        last_login_at timestamp with time zone,
        soft_deleted_at timestamp with time zone,
        deleted_by uuid,
        created_by uuid,
        created_at timestamp with time zone NOT NULL DEFAULT (now()),
        updated_at timestamp with time zone NOT NULL DEFAULT (now()),
        address_id uuid,
        CONSTRAINT "PK_users" PRIMARY KEY (id),
        CONSTRAINT "FK_users_addresses_address_id" FOREIGN KEY (address_id) REFERENCES addresses (id) ON DELETE SET NULL
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260219110801_init') THEN
    CREATE TABLE applications_redirect_uris (
        id uuid NOT NULL DEFAULT (gen_random_uuid()),
        application_id uuid NOT NULL,
        redirect_uris Text,
        created_at timestamp with time zone NOT NULL DEFAULT (now()),
        updated_at timestamp with time zone NOT NULL DEFAULT (now()),
        CONSTRAINT "PK_applications_redirect_uris" PRIMARY KEY (id),
        CONSTRAINT "FK_applications_redirect_uris_applications_application_id" FOREIGN KEY (application_id) REFERENCES applications (id) ON DELETE SET NULL
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260219110801_init') THEN
    CREATE TABLE grants (
        id uuid NOT NULL DEFAULT (gen_random_uuid()),
        code character varying(255),
        grant_type smallint NOT NULL,
        redirect_uri Text,
        scopes Text,
        code_challenge character varying(255) NOT NULL,
        code_challenge_method character varying(10) NOT NULL,
        revoked_at timestamp with time zone,
        expires_at timestamp with time zone NOT NULL,
        consumed_at timestamp with time zone,
        application_id uuid NOT NULL,
        user_id uuid NOT NULL,
        created_at timestamp with time zone NOT NULL,
        updated_at timestamp with time zone NOT NULL,
        CONSTRAINT "PK_grants" PRIMARY KEY (id),
        CONSTRAINT "FK_grants_applications_application_id" FOREIGN KEY (application_id) REFERENCES applications (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260219110801_init') THEN
    CREATE TABLE organizations (
        id uuid NOT NULL DEFAULT (gen_random_uuid()),
        name character varying(255),
        description character varying(255),
        slug character varying(255) NOT NULL,
        owner_id uuid,
        address_id uuid,
        soft_deleted_at timestamp with time zone,
        deleted_by uuid,
        created_by uuid,
        created_at timestamp with time zone NOT NULL,
        updated_at timestamp with time zone NOT NULL,
        CONSTRAINT "PK_organizations" PRIMARY KEY (id),
        CONSTRAINT "FK_organizations_addresses_address_id" FOREIGN KEY (address_id) REFERENCES addresses (id) ON DELETE SET NULL,
        CONSTRAINT "FK_organizations_users_created_by" FOREIGN KEY (created_by) REFERENCES users (id) ON DELETE SET NULL,
        CONSTRAINT "FK_organizations_users_deleted_by" FOREIGN KEY (deleted_by) REFERENCES users (id) ON DELETE SET NULL,
        CONSTRAINT "FK_organizations_users_owner_id" FOREIGN KEY (owner_id) REFERENCES users (id) ON DELETE SET NULL
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260219110801_init') THEN
    CREATE TABLE access_tokens (
        id uuid NOT NULL DEFAULT (gen_random_uuid()),
        token character varying(255) NOT NULL,
        scopes text,
        is_revoked boolean NOT NULL DEFAULT TRUE,
        expires_at timestamp with time zone NOT NULL,
        grant_id uuid NOT NULL,
        created_at timestamp with time zone NOT NULL,
        updated_at timestamp with time zone NOT NULL,
        CONSTRAINT "PK_access_tokens" PRIMARY KEY (id),
        CONSTRAINT "FK_access_tokens_grants_grant_id" FOREIGN KEY (grant_id) REFERENCES grants (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260219110801_init') THEN
    CREATE TABLE refresh_tokens (
        id uuid NOT NULL DEFAULT (gen_random_uuid()),
        token character varying(255) NOT NULL,
        grant_id uuid NOT NULL,
        "TokenFamilyId" uuid NOT NULL,
        replaced_by_token_id uuid,
        revoked_at timestamp with time zone,
        expires_at timestamp with time zone NOT NULL,
        created_at timestamp with time zone NOT NULL,
        updated_at timestamp with time zone NOT NULL,
        CONSTRAINT "PK_refresh_tokens" PRIMARY KEY (id),
        CONSTRAINT "FK_refresh_tokens_grants_grant_id" FOREIGN KEY (grant_id) REFERENCES grants (id) ON DELETE CASCADE,
        CONSTRAINT "FK_refresh_tokens_refresh_tokens_replaced_by_token_id" FOREIGN KEY (replaced_by_token_id) REFERENCES refresh_tokens (id) ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260219110801_init') THEN
    CREATE TABLE organization_users (
        id uuid NOT NULL DEFAULT (gen_random_uuid()),
        organization_id uuid,
        user_id uuid,
        status character varying(255),
        created_at timestamp with time zone NOT NULL,
        updated_at timestamp with time zone NOT NULL,
        CONSTRAINT "PK_organization_users" PRIMARY KEY (id),
        CONSTRAINT "FK_organization_users_organizations_organization_id" FOREIGN KEY (organization_id) REFERENCES organizations (id) ON DELETE SET NULL,
        CONSTRAINT "FK_organization_users_users_user_id" FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE SET NULL
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260219110801_init') THEN
    CREATE INDEX "IX_access_tokens_grant_id" ON access_tokens (grant_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260219110801_init') THEN
    CREATE INDEX "IX_applications_redirect_uris_application_id" ON applications_redirect_uris (application_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260219110801_init') THEN
    CREATE INDEX "IX_grants_application_id" ON grants (application_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260219110801_init') THEN
    CREATE INDEX "IX_organization_users_organization_id" ON organization_users (organization_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260219110801_init') THEN
    CREATE INDEX "IX_organization_users_user_id" ON organization_users (user_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260219110801_init') THEN
    CREATE INDEX "IX_organizations_address_id" ON organizations (address_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260219110801_init') THEN
    CREATE INDEX "IX_organizations_created_by" ON organizations (created_by);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260219110801_init') THEN
    CREATE INDEX "IX_organizations_deleted_by" ON organizations (deleted_by);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260219110801_init') THEN
    CREATE UNIQUE INDEX "IX_organizations_id" ON organizations (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260219110801_init') THEN
    CREATE INDEX "IX_organizations_owner_id" ON organizations (owner_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260219110801_init') THEN
    CREATE INDEX "IX_refresh_tokens_grant_id" ON refresh_tokens (grant_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260219110801_init') THEN
    CREATE UNIQUE INDEX "IX_refresh_tokens_replaced_by_token_id" ON refresh_tokens (replaced_by_token_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260219110801_init') THEN
    CREATE INDEX "IX_users_address_id" ON users (address_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260219110801_init') THEN
    CREATE UNIQUE INDEX "IX_users_email" ON users (email);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260219110801_init') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260219110801_init', '10.0.2');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260220093410_RowVersionGrant') THEN
    ALTER TABLE grants ADD "RowVersion" bytea NOT NULL DEFAULT BYTEA E'\\x';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260220093410_RowVersionGrant') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260220093410_RowVersionGrant', '10.0.2');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260220115103_ChangeTypeOnUserOrg') THEN
    UPDATE organization_users SET user_id = '00000000-0000-0000-0000-000000000000' WHERE user_id IS NULL;
    ALTER TABLE organization_users ALTER COLUMN user_id SET NOT NULL;
    ALTER TABLE organization_users ALTER COLUMN user_id SET DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260220115103_ChangeTypeOnUserOrg') THEN
    ALTER TABLE organization_users ALTER COLUMN status TYPE smallint;
    UPDATE organization_users SET status = 0 WHERE status IS NULL;
    ALTER TABLE organization_users ALTER COLUMN status SET NOT NULL;
    ALTER TABLE organization_users ALTER COLUMN status SET DEFAULT 0;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260220115103_ChangeTypeOnUserOrg') THEN
    UPDATE organization_users SET organization_id = '00000000-0000-0000-0000-000000000000' WHERE organization_id IS NULL;
    ALTER TABLE organization_users ALTER COLUMN organization_id SET NOT NULL;
    ALTER TABLE organization_users ALTER COLUMN organization_id SET DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260220115103_ChangeTypeOnUserOrg') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260220115103_ChangeTypeOnUserOrg', '10.0.2');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223141400_Session') THEN
    ALTER TABLE access_tokens DROP CONSTRAINT "FK_access_tokens_grants_grant_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223141400_Session') THEN
    ALTER TABLE refresh_tokens DROP CONSTRAINT "FK_refresh_tokens_grants_grant_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223141400_Session') THEN
    ALTER TABLE refresh_tokens RENAME COLUMN grant_id TO "GrantId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223141400_Session') THEN
    ALTER TABLE refresh_tokens RENAME COLUMN "TokenFamilyId" TO "SessionId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223141400_Session') THEN
    ALTER INDEX "IX_refresh_tokens_grant_id" RENAME TO "IX_refresh_tokens_GrantId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223141400_Session') THEN
    ALTER TABLE access_tokens RENAME COLUMN grant_id TO "GrantId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223141400_Session') THEN
    ALTER INDEX "IX_access_tokens_grant_id" RENAME TO "IX_access_tokens_GrantId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223141400_Session') THEN
    ALTER TABLE refresh_tokens ALTER COLUMN "GrantId" DROP NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223141400_Session') THEN
    ALTER TABLE refresh_tokens ADD "RowVersion" bytea NOT NULL DEFAULT BYTEA E'\\x';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223141400_Session') THEN
    ALTER TABLE access_tokens ALTER COLUMN "GrantId" DROP NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223141400_Session') THEN
    ALTER TABLE access_tokens ADD "SessionId" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223141400_Session') THEN
    CREATE TABLE "Session" (
        "Id" uuid NOT NULL,
        "UserId" uuid NOT NULL,
        "LastActivity" timestamp with time zone NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        "IsRevoked" boolean NOT NULL,
        CONSTRAINT "PK_Session" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223141400_Session') THEN
    CREATE TABLE "TokenFingerprint" (
        "Id" uuid NOT NULL,
        "SessionId" uuid NOT NULL,
        "DeviceFingerprint" text NOT NULL,
        "IpRangeHash" text NOT NULL,
        "UserAgentHash" text NOT NULL,
        CONSTRAINT "PK_TokenFingerprint" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_TokenFingerprint_Session_SessionId" FOREIGN KEY ("SessionId") REFERENCES "Session" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223141400_Session') THEN
    CREATE INDEX "IX_refresh_tokens_SessionId" ON refresh_tokens ("SessionId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223141400_Session') THEN
    CREATE INDEX "IX_access_tokens_SessionId" ON access_tokens ("SessionId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223141400_Session') THEN
    CREATE INDEX "IX_TokenFingerprint_SessionId" ON "TokenFingerprint" ("SessionId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223141400_Session') THEN
    ALTER TABLE access_tokens ADD CONSTRAINT "FK_access_tokens_Session_SessionId" FOREIGN KEY ("SessionId") REFERENCES "Session" ("Id") ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223141400_Session') THEN
    ALTER TABLE access_tokens ADD CONSTRAINT "FK_access_tokens_grants_GrantId" FOREIGN KEY ("GrantId") REFERENCES grants (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223141400_Session') THEN
    ALTER TABLE refresh_tokens ADD CONSTRAINT "FK_refresh_tokens_Session_SessionId" FOREIGN KEY ("SessionId") REFERENCES "Session" ("Id") ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223141400_Session') THEN
    ALTER TABLE refresh_tokens ADD CONSTRAINT "FK_refresh_tokens_grants_GrantId" FOREIGN KEY ("GrantId") REFERENCES grants (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223141400_Session') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260223141400_Session', '10.0.2');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE access_tokens DROP CONSTRAINT "FK_access_tokens_Session_SessionId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE refresh_tokens DROP CONSTRAINT "FK_refresh_tokens_Session_SessionId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE "TokenFingerprint" DROP CONSTRAINT "FK_TokenFingerprint_Session_SessionId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE "TokenFingerprint" DROP CONSTRAINT "PK_TokenFingerprint";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE "Session" DROP CONSTRAINT "PK_Session";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE "TokenFingerprint" RENAME TO token_fingerprints;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE "Session" RENAME TO sessions;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE refresh_tokens RENAME COLUMN "SessionId" TO session_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER INDEX "IX_refresh_tokens_SessionId" RENAME TO "IX_refresh_tokens_session_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE access_tokens RENAME COLUMN "SessionId" TO session_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER INDEX "IX_access_tokens_SessionId" RENAME TO "IX_access_tokens_session_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE token_fingerprints RENAME COLUMN "Id" TO id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE token_fingerprints RENAME COLUMN "UserAgentHash" TO user_agent_hash;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE token_fingerprints RENAME COLUMN "SessionId" TO session_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE token_fingerprints RENAME COLUMN "IpRangeHash" TO ip_range_hash;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE token_fingerprints RENAME COLUMN "DeviceFingerprint" TO device_fingerprint;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER INDEX "IX_TokenFingerprint_SessionId" RENAME TO "IX_token_fingerprints_session_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE sessions RENAME COLUMN "Id" TO id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE sessions RENAME COLUMN "UserId" TO user_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE sessions RENAME COLUMN "UpdatedAt" TO updated_at;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE sessions RENAME COLUMN "LastActivity" TO last_activity;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE sessions RENAME COLUMN "IsRevoked" TO is_revoked;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE sessions RENAME COLUMN "CreatedAt" TO created_at;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE refresh_tokens ALTER COLUMN token TYPE character varying(512);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE access_tokens ALTER COLUMN token TYPE character varying(512);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE token_fingerprints ALTER COLUMN id SET DEFAULT (gen_random_uuid());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE token_fingerprints ALTER COLUMN user_agent_hash TYPE character varying(255);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE token_fingerprints ALTER COLUMN ip_range_hash TYPE character varying(255);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE token_fingerprints ALTER COLUMN device_fingerprint TYPE character varying(255);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE sessions ALTER COLUMN id SET DEFAULT (gen_random_uuid());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE sessions ALTER COLUMN is_revoked SET DEFAULT FALSE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE token_fingerprints ADD CONSTRAINT "PK_token_fingerprints" PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE sessions ADD CONSTRAINT "PK_sessions" PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    CREATE UNIQUE INDEX "IX_refresh_tokens_token" ON refresh_tokens (token);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    CREATE UNIQUE INDEX "IX_access_tokens_token" ON access_tokens (token);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    CREATE INDEX "IX_sessions_user_id" ON sessions (user_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE access_tokens ADD CONSTRAINT "FK_access_tokens_sessions_session_id" FOREIGN KEY (session_id) REFERENCES sessions (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE refresh_tokens ADD CONSTRAINT "FK_refresh_tokens_sessions_session_id" FOREIGN KEY (session_id) REFERENCES sessions (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    ALTER TABLE token_fingerprints ADD CONSTRAINT "FK_token_fingerprints_sessions_session_id" FOREIGN KEY (session_id) REFERENCES sessions (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260223142534_SessionAndMore') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260223142534_SessionAndMore', '10.0.2');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260224074447_addrowversionsession') THEN
    ALTER TABLE sessions ADD "RowVersion" bytea NOT NULL DEFAULT BYTEA E'\\x';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260224074447_addrowversionsession') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260224074447_addrowversionsession', '10.0.2');
    END IF;
END $EF$;
COMMIT;

