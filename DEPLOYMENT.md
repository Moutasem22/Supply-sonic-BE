# Deploying AppAPI to Azure App Service

## 1. Create the App Service settings

In the Azure portal, open the App Service, then **Settings** > **Environment
variables**. Add the following application settings and save. Restart the app
after saving.

| Setting | Description |
| --- | --- |
| `ASPNETCORE_ENVIRONMENT` | Set this to `Production`. |
| `ConnectionStrings__DefaultConnection` | Azure SQL connection string used by EF Core and Hangfire. |
| `ConnectionStrings__ExcelConString` | Optional override for the non-secret Excel provider format string. |
| `AppSettings__Secret` | Production JWT signing secret. |
| `AppSettings__auth` | Application authentication setting. |
| `AppSettings__applicationID` | Firebase application ID. |
| `AppSettings__firebaseURL` | Firebase project URL. |
| `AppSettings__SENDER_ID` | Firebase sender ID. |
| `EmailSettings__Password` | SMTP account password. |
| `AllowedOrigins` | Semicolon-separated frontend origins, for example `https://app.example.com;https://www.example.com`. |
| `LogFilePath__Uri` | Optional writable log directory. If unset, logs go to `Logs/` below the app content root. |
| `ElasticConfiguration__Uri` | Optional absolute Elasticsearch URL. Leave it unset when Elasticsearch is unavailable. |

For Azure SQL SQL authentication, set `ConnectionStrings__DefaultConnection`
to a value such as:

```text
Server=tcp:<server>.database.windows.net,1433;Initial Catalog=<database>;User ID=<sql-admin>;******;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;MultipleActiveResultSets=True;
```

Replace the redacted segment with the SQL authentication credential property
and your SQL admin password before saving it in Azure.

Alternatively, under **Connection strings**, add a connection string named
`DefaultConnection` with type **SQL Server**. Azure exposes that as
`ConnectionStrings:DefaultConnection` to the application. Use exactly one of
these forms; never put this value in a committed settings file.

## 2. Apply migrations

Install the EF Core command-line tool if it is not already available:

```bash
dotnet tool install --global dotnet-ef --version 6.*
```

From the repository root, apply migrations with the Azure SQL connection
string:

```bash
dotnet ef database update --project src/Hosts/SupplySonic/DB/DB.csproj --startup-project src/Hosts/SupplySonic/API/AppAPI.csproj --connection "<Azure SQL connection string>"
```

You can instead temporarily set
`ConnectionStrings__DefaultConnection` in your local shell, then run the same
command without `--connection`. Do not save that shell value in the repository.

## 3. CORS and logging

Set `AllowedOrigins` to every React site that calls this API, separated with
semicolons and without trailing paths. The setting is read from standard
ASP.NET Core configuration, so Azure application settings override JSON files.

Elasticsearch logging is enabled only when `ElasticConfiguration__Uri` is a
valid absolute URI. Leave it unset if no Elasticsearch service is deployed.
Console logging remains enabled. File logs use `LogFilePath__Uri` when it is a
writable path; otherwise the application falls back to a cross-platform
`Logs/` directory.

## 4. Container deployment

The root `Dockerfile` builds the .NET 6 API and exposes port `8080`. It sets
`ASPNETCORE_URLS=http://+:8080`; configure an Azure Linux custom container to
use port `8080` (or set its `WEBSITES_PORT` setting to `8080`).

For local secret overrides, copy
`src/Hosts/SupplySonic/API/appsettings.Local.json.example` to
`appsettings.Local.json`, fill in the values, and keep it untracked. The
`.gitignore` rules exclude local override files.
