# Web.App - Agent Description

Overview
- Project: `Web.App`
- Purpose: Frontend / UI project (Blazor, SPA or .NET MAUI hybrid) serving client-side UI, assets and user interaction.

Context & Architecture
- Target Framework: .NET 9
- UI technologies: Blazor / Razor / static SPA / .xaml for MAUI views if present
- API consumption: `Web.Api` (check `appsettings` or environment variable `API_BASE_URL`)
- Patterns: Dependency Injection, MVVM or component-based state management, client-side routing

Code Conventions
- Private fields
  - Start with a single underscore (`_`) followed by camelCase.
  - Examples: `_repository`, `_logger`, `_currentUser`
- Properties
  - Use PascalCase for property identifiers.
  - Example: `public string UserName { get; set; }`
- Formatting / Indentation (C# / Razor / code)
  - Use 4 spaces for indentation (no tabs).
  - Curly braces: always use braces for blocks; place opening brace on a new line.
  - Example C#:
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
- Razor / .cshtml
  - Keep server code blocks and markup separated clearly.
  - Indent C# inside `@code` blocks with 4 spaces and follow the same braces rule.

XAML Formatting (additional rules)
- Indentation
  - Use 4 spaces (no tabs) for XAML indentation to match code formatting.
- Element layout and attributes
  - For simple elements with few attributes keep them on a single line.
  - For elements with multiple attributes or long attribute values, place each attribute on its own line, aligned below the element name; close the start tag on a new line.
  - Use self-closing tags when no inner content is present.
- Naming
  - Use `x:Name` in PascalCase when assigning names required by code-behind.
  - Binding paths should match viewmodel property names (PascalCase).
- Resources & Keys
  - Use PascalCase for resource keys (e.g., `PrimaryButtonStyle`).
- Tooling
  - Use a XAML formatter (e.g., XAML Styler) and configure it to 4 spaces and the attribute-per-line policy.
  - In Visual Studio configure XAML formatting via __Tools > Options > XAML Styler__ (or the extension settings).
- Example XAML (follow these rules):
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui" 
					xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" 
					x:Class="Web.App.Views.LoginPage"> 
						<VerticalStackLayout 
							Padding="20"> 
							<Entry 
								x:Name="UserNameEntry" 
								Placeholder="Username" 
								Text="{Binding UserName}" /> 
							<Button 
								Text="Login" 
								Command="{Binding LoginCommand}" 
								Style="{StaticResource PrimaryButtonStyle}" />
						</VerticalStackLayout> 
</ContentPage>

UI / Assets
- Static assets: `wwwroot/`, `wwwroot/css`, `wwwroot/js`, `Assets/` (MAUI)
- Images: use optimized formats and responsive sizes
- Localization: use `Resources` and follow naming conventions in this file

Run / Debug locally
- Visual Studio: select `Web.App` → __F5__ or __Ctrl+F5__
- CLI: `dotnet run --project Web.App`
- Check `Properties/launchSettings.json` for profiles and dev ports

Testing
- Unit tests: component tests / viewmodel tests in a `tests/Web.App.UnitTests` project
- E2E: Playwright / Cypress for browser flows, run against local `Web.Api` when needed

CI / CD / Linting
- Add `.editorconfig` for C#/Razor and enable XAML styler rules
- Add Roslyn analyzers for naming/formatting enforcement
- Enforce conventions in PR checklist: reference this `Agent.md`

Ownership & Docs
- Owner / contact: @AppOwner
- Related docs: `Web.App/README.md`, `docs/` (if present)

Quick CLI
- Start local: `dotnet run --project Web.App`
- Build for production: `dotnet publish -c Release --project Web.App`