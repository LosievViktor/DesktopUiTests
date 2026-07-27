# DesktopUiTests

C# code examples for Desktop UI automation tests made with FlaUI library.

Stack: C#, FlaUI, NUnit

Target application: [MauiUkraine](https://github.com/LosievViktor/MauiUkraine) — a MAUI sample app with a Ukrainian language UI.

## Setup

1. Clone and build [MauiUkraine](https://github.com/LosievViktor/MauiUkraine).
2. Open `.runsettings` and set:
   - `AppPath` — full path to `MauiControlsShowcase.exe`
   - `ProcessName` — process name without `.exe` (default: `MauiControlsShowcase`)
   - `DefaultTimeout` — UI wait timeout in **seconds** (default: `30`)
   - `PollInterval` — poll interval in **milliseconds** (default: `250`)
3. Build this solution.

## Launch tests

### Visual Studio (Test Explorer)

1. Select the runsettings file: **Test → Configure Run Settings → Select Solution Wide runsettings File** and choose `.runsettings`.
2. Or enable auto-detect: **Tools → Options → Test → Auto Detect runsettings Files**.
3. Run tests from Test Explorer.

### Command line

```powershell
dotnet test --settings .runsettings
```

To run a single test class:

```powershell
dotnet test --settings .runsettings --filter FullyQualifiedName~HomeNavigationTests
```

`AppConfig` reads `AppPath`, `ProcessName`, `DefaultTimeout`, and `PollInterval` from `.runsettings` via NUnit `TestContext.Parameters`. All four parameters are required.

