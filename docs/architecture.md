# Architecture

Beer & Bingo is a .NET MAUI solution (`src/BeerAndBingo.sln`) split into two apps and a set of
shared libraries. The apps and `Bingo.UI.Shared` target `net10.0-android`, `net10.0-ios`,
`net10.0-maccatalyst`, and `net10.0-windows10.0.19041.0` (MAUI 10, upgraded 2026-08-05 — see
`docs/status.md`); the other class libraries have no MAUI dependency and stay on plain `net9.0`.

## Apps (`src/apps`)

- **Bingo.Caller.App** — the host/caller device. Runs pattern selection, the flash board (number
  calling), and OCR-based card recognition. Composition root: `MauiProgram.cs` →
  `Startup/ConfigureAppServices.cs` (DI registration), `ConfigureFonts.cs`, `ConfigureLogging.cs`.
- **Bingo.Player.App** — the player-facing app. Currently a much thinner shell (default MAUI
  `MainPage`/`AppShell`) than the Caller app.

Both apps are `SingleProject` MAUI heads; platform-specific code lives under each app's
`Platforms/` folder, plus a shared `src/Platforms/Android` at the solution root.

## Libraries (`src/libs`)

Dependency direction flows roughly Core → Services → AppServices → ViewModel → UI.Shared, with
Infrastructure/Integrations/ImageProcessing as supporting libs consumed where needed:

- **Bingo.Core** — domain types with no external dependencies: `Patterns` (BingoPattern,
  PatternCell, grid settings, JSON conversion), `FlashBoard` (the number-calling board model),
  `Domain` (GameSessionState/Snapshot, SyncSnapshot), `Device` (form factor / persona
  abstractions), `Messaging` (in-app messages), `Extensions`.
- **Bingo.Services** — application logic over Core domain types (`Patterns`, `Recognition`,
  `Feedback`). Depends on `CommunityToolkit.Mvvm` + `Bingo.Core`.
- **Bingo.AppServices** — orchestration/config layer above Services (`Configuration`,
  `Feedback`, `Patterns`, `Recognition`). Depends on `Bingo.Core` + `Bingo.Services`.
- **Bingo.Infrastructure** — cross-cutting infrastructure concerns (logging/JSON plumbing),
  depends only on `Bingo.Core`.
- **Bingo.Integrations** — external integration points; currently has no third-party package
  dependencies.
- **Bingo.ImageProcessing** — card/number recognition pipeline: `Microsoft.ML`, `SkiaSharp`,
  `TesseractOcrMaui`. Depends on `Bingo.Core`.
- **Bingo.ViewModel** — MVVM view models (`FlashBoard`, `MainPage`, `NextRoundClock`,
  `Patterns`, `Helpers`, `Messages`) built on `CommunityToolkit.Mvvm`. Depends on
  `Bingo.AppServices` + `Bingo.Services`.
- **Bingo.UI.Shared** — shared MAUI UI: `Views`, `Controls`, `Drawables`, `Styles`,
  `Converters`, `Services`, plus platform-specific UI bits under `Platforms/`. Depends on
  `Bingo.ViewModel`, `Microsoft.Maui.Controls`, `SkiaSharp` (+ `SkiaSharp.Views.Maui.Controls`,
  `SkiaSharp.Extended.UI.Maui`).

`Bingo.Caller.App` references all of the above directly and wires them up via DI in
`ConfigureAppServices.AddBeerAndBingoServices`. `Bingo.Player.App` only references
`Microsoft.Maui.Controls` today and hasn't been wired into the shared library stack yet.

## Tests (`src/tests`)

One test project per library (`Bingo.Core.Tests`, `Bingo.Services.Tests`,
`Bingo.AppServices.Tests`, `Bingo.ViewModel.Tests`, `Bingo.UI.Shared.Tests`,
`Bingo.Infrastructure.Tests`, `Bingo.Integrations.Tests`, `Bingo.ImageProcessing.Tests`), using
xUnit + Moq.

## Key domain concepts

- **Pattern** (`Bingo.Core.Patterns`) — a bingo card pattern (e.g. four corners) defined as a
  grid of cells, with column-based calling restrictions (e.g. a four-corners pattern only calls
  the B and O columns).
- **FlashBoard** (`Bingo.Core.FlashBoard`) — the model behind the number-calling display shown
  on the Caller app.
- **GameSessionState/Snapshot** (`Bingo.Core.Domain`) — session state used for sync between
  devices.
