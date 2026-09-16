# Project Status

Last updated: 2026-09-16

This is a living doc — update it as work resumes/pauses so future sessions (human or Claude) know
where things stand.

## Recent work (most recent first)

- **Retargeted remaining libs/tests to net10.0 + full NuGet update** (2026-09-16): the class
  libraries under `src/libs` (`Bingo.Core`, `Bingo.AppServices`, `Bingo.ImageProcessing`,
  `Bingo.Infrastructure`, `Bingo.Integrations`, `Bingo.Services`, `Bingo.ViewModel`) and all 8 test
  projects were still on `net9.0` after the MAUI 10 upgrade below — bumped them all to `net10.0`.
  Updated every outdated top-level package to latest: `Microsoft.Extensions.*`/`System.Text.Json`
  10.0.10 → 10.0.12, `Microsoft.Maui.Controls`/`.Compatibility` 10.0.90 → 10.0.101, `SkiaSharp`/
  `SkiaSharp.Views.Maui.Controls` 4.151.1 → 4.152.0, `SkiaSharp` in `Bingo.ImageProcessing` 3.119.4
  → 4.152.0 (now matches the `SkiaSharp` major version used in `Bingo.UI.Shared`),
  `Microsoft.NET.Test.Sdk` 18.8.1 → 18.10.1, `xunit.runner.visualstudio` 3.1.5 → 4.0.0. Also
  dropped the explicit `System.Text.Json` `PackageReference` from `Bingo.AppServices` and
  `Bingo.Infrastructure` — NuGet's new net10.0 pruning warning (NU1510) flagged it as redundant
  since it now ships in the shared framework. Full solution restores/builds clean (0 errors,
  pre-existing warnings only) and all tests pass. `Microsoft.ML`, `TesseractOcrMaui`,
  `CommunityToolkit.Mvvm`, `Moq`, `xunit`, `coverlet.collector` were already at latest and
  untouched.
- **Upgraded to MAUI 10 / net10.0** (2026-08-05): updated the MAUI workload from 9.0.120 to the
  10.x band (`dotnet workload update`), retargeted `Bingo.UI.Shared`, `Bingo.Caller.App`, and
  `Bingo.Player.App` from `net9.0-*` to `net10.0-*`, and bumped `Microsoft.Maui.Controls` →
  10.0.90, `SkiaSharp`/`SkiaSharp.Views.Maui.Controls` → 4.151.1, `SkiaSharp.Extended.UI.Maui` →
  3.0.0. Had to add an explicit `Microsoft.Maui.Controls.Compatibility` 10.0.90 package reference
  to `Bingo.UI.Shared` — without it, the Windows TFM transitively resolved
  `Microsoft.Maui.Controls.Compatibility`/`Core`/`Xaml` to a stale `9.0.82` and failed to copy
  `WebView2Loader.dll` (missing from that stale package on disk). Full solution now builds clean
  (0 errors) across Android/iOS/MacCatalyst/Windows and all 54 unit tests pass. As a side effect,
  the workload update also resolved the Android SDK gap below (it now targets API 36, which *is*
  installed, instead of API 35, which wasn't).
- Added a winner sound effect (currently placeholder: Windows 3.1 startup sound) for when a
  player calls bingo.
- Added column-calling restrictions for patterns — e.g. a four-corners pattern now only calls
  numbers from the B and O columns instead of the full board.
- Earlier: got pattern loading (`LoadPatternView`/`LoadPatternViewModel` +
  `FilePatternRepository`) working end-to-end after several iterations.

## Current state

- **Bingo.Caller.App** is the actively-developed app: pattern creation/loading, flash board
  (number calling with column restrictions), and image-recognition scaffolding
  (`Bingo.ImageProcessing`, Tesseract OCR) are wired up via DI.
- **Bingo.Player.App** is still essentially the default MAUI template — not yet wired into the
  shared `Bingo.ViewModel`/`Bingo.UI.Shared` stack. This is likely the next big chunk of work if
  the player-facing experience is in scope.
- Patterns persist to `FileSystem.AppDataDirectory` via `FilePatternRepository` (see
  `docs/architecture.md`).
- No CI pipeline beyond Dependabot; no automated build/test gate on PRs yet.

## Known gaps / open questions

- Winner sound is a placeholder — needs a real sound asset.
- `Bingo.UI.Shared.Tests.csproj` exists on disk but **is not referenced in `BeerAndBingo.sln`** —
  it was skipped by solution-wide build/test/package commands until this was noticed. Worth
  adding it to the `.sln`.
- All NuGet packages are now at latest-stable as of 2026-09-16 (see git history: the initial
  9.x-workload-safe bump, the MAUI 10 upgrade, then the net10.0 retarget + full package update
  above). No known version-drift gaps remain.
  - No `global.json`/central package management — worth considering if version drift across
    projects becomes a recurring papercut.
- Player app scope/roadmap not yet defined here — fill in once decided.
- Now that the toolchain is on MAUI 10, keep an eye on the `CS0618` obsolete-API warnings that
  showed up in the upgrade build (`ViewExtensions.TranslateTo/ScaleTo/FadeTo` →
  `*Async` variants, `Page.DisplayAlert` → `DisplayAlertAsync`) — non-blocking today, but worth
  migrating next time those call sites are touched.

## Next steps

- (Fill in as work resumes.)
