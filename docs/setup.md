# Setup / Build / Run

## Prerequisites

- **.NET SDK 10.0** (`dotnet --version` should report `10.0.x`). The MAUI apps and `Bingo.UI.Shared`
  target `net10.0-*` TFMs; the plain class libraries target `net9.0` (no MAUI dependency, so no
  need to bump them).
- **MAUI workloads on the 10.x band**: `android`, `ios`, `maccatalyst`, `maui-windows`. Install/update
  with:
  ```
  dotnet workload install android ios maccatalyst maui-windows
  dotnet workload update
  ```
  `dotnet workload list` should show manifest versions in the `10.0.x` range (e.g. `maui-windows
  10.0.20/10.0.100`). MAUI 10.x packages (`Microsoft.Maui.Controls` 10.0.90+) require this — MAUI
  9.x workloads will fail with `error MA003`.
- **Android SDK** (via Visual Studio or `dotnet workload`) if deploying to an Android
  device/emulator — this project's primary target.
- Visual Studio 2022 (17.8+) with the .NET MAUI workload is the easiest way to run/debug on an
  Android emulator or device; `dotnet build`/`dotnet run` from the CLI also works per-project.

## Restore & build the whole solution

```
dotnet restore src/BeerAndBingo.sln
dotnet build src/BeerAndBingo.sln
```

## Build/run a specific app

Caller app (Android):
```
dotnet build src/apps/Bingo.Caller.App/Bingo.Caller.App.csproj -f net10.0-android
```

Player app (Android):
```
dotnet build src/apps/Bingo.Player.App/Bingo.Player.App.csproj -f net10.0-android
```

To deploy to a connected device/emulator, use `dotnet build -t:Run -f net10.0-android` from the
app's project directory, or open `src/BeerAndBingo.sln` in Visual Studio and select the Android
target + device.

## Run tests

```
dotnet test src/BeerAndBingo.sln
```

Or an individual test project, e.g.:
```
dotnet test src/tests/Bingo.Core.Tests/Bingo.Core.Tests.csproj
```

## Notes

- There's no `global.json` pinning the SDK version and no central package management
  (`Directory.Packages.props`) — each `.csproj` pins its own `PackageReference` versions.
- No CI workflow exists yet beyond Dependabot (`.github/dependabot.yml`), which opens PRs for
  outdated NuGet packages.
