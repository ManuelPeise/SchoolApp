# Web.Api - Agent Description

Overview
- Project: `Web.Api`
- Purpose: Backend Web API (.NET 9) providing business logic, data access, authentication and integrations.

Context & Architecture
- Target Framework: .NET 9
- Hosting: Kestrel (supports IIS, containers)
- API docs: Swagger / OpenAPI (default `/swagger`)
- Patterns: Dependency Injection, Middleware, HealthChecks

Code Conventions
- Private fields
  - Start with a single underscore (`_`) followed by camelCase.
  - Examples: `_repository`, `_logger`, `_currentUser`
- Properties
  - Use CamelCase for property identifiers (in C# this is commonly referred to as PascalCase — start with an uppercase letter).
  - Example: `public string UserName { get; set; }`
- Formatting / Indentation
  - Use 4 spaces for indentation (no tabs).
  - Example:
    ```csharp
    public class Sample
    {
        private readonly ILogger _logger;

        public Sample(ILogger logger)
        {
            _logger = logger;
        }
    }
    ```
- Curly braces
  - Always use braces for blocks and place opening braces on a new line.
  - Example:
    ```csharp
    public void DoWork()
    {
        if (_logger == null)
        {
            throw new InvalidOperationException();
        }

        // work...
    }
    ```

Key Endpoints (examples)
- Authentication: `POST /api/auth/login`, `POST /api/auth/refresh`
- Users: `GET /api/users`, `GET /api/users/{id}`, `POST /api/users`
- Health: `GET /health`
- Metrics (optional): `/metrics`

Authentication & Authorization
- Recommended: JWT Bearer tokens (`Authorization: Bearer <token>`)
- Config keys: `Jwt:Issuer`, `Jwt:Key`, `Jwt:ExpiryMinutes`
- Roles / Policies: e.g. `Admin`, `User` — define centrally in `Program.cs`
- Secrets: use Secret Manager locally or Azure Key Vault in production

Configuration (common appsettings keys)
- `ConnectionStrings:DefaultConnection`
- `Jwt:Issuer`, `Jwt:Key`, `Jwt:ExpiryMinutes`
- `Logging:LogLevel`
- `AllowedHosts`

Database
- Typical ORM: EF Core (if used)
- Local migrations: `dotnet ef migrations add <Name> --project Web.Api` and `dotnet ef database update --project Web.Api`
- Keep connection strings out of source control

Run / Debug locally
- Visual Studio: select `Web.Api` → __F5__ or __Ctrl+F5__
- CLI: `dotnet run --project Web.Api`
- Check `Properties/launchSettings.json` for profiles and ports

Container / Deployment
- Provide a multi-stage `Dockerfile` at project root
- Build: `docker build -t company/web.api:tag -f Web.Api/Dockerfile .`
- Ensure health/readiness probes and environment-based config for containers

Observability & Monitoring
- Logging: `ILogger` (Serilog recommended for structured logs)
- Telemetry: Application Insights / OpenTelemetry
- HealthChecks endpoint: `/health`
- Optional: Prometheus metrics endpoint

Testing
- Unit tests: project like `tests/Web.Api.UnitTests`
- Integration tests: use `WebApplicationFactory<TEntryPoint>` or TestServer
- Run tests: `dotnet test`

CI / CD
- Build: `dotnet build` / `dotnet publish -c Release`
- Security: scan dependencies, do not check secrets into repo
- DB migrations in CD: run with caution, prefer controlled migrations

Security Best Practices
- No secrets in repo
- Restrictive CORS policy
- Validate inputs and check `ModelState`
- Consider rate-limiting and abuse protection

Ownership & Docs
- Owner / contact: @ApiOwner
- Related docs: `Web.Api/README.md`, `docs/` (if present)

Quick CLI
- Start local: `dotnet run --project Web.Api`
- Swagger: `http://localhost:<port>/swagger`
- DB migration: `dotnet ef database update --project Web.Api`