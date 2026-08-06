# Project Status

Last updated: 2026-08-05

This is a living doc — update it as work resumes/pauses so future sessions (human or Claude) know
where things stand.

## Recent work (most recent first)

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
- **Android SDK platform 35 is not installed** on this dev machine
  (`C:\Program Files (x86)\Android\android-sdk\platforms\android-35` is missing `android.jar`).
  This blocks building for `net9.0-android` — the primary target — even though the `android`
  MAUI workload itself is installed. Fix with:
  `dotnet build -t:InstallAndroidDependencies -f net9.0-android "-p:AndroidSdkDirectory=C:\Program Files (x86)\Android\android-sdk"`
  (or install API 35 via Android Studio's SDK Manager). Windows/iOS/MacCatalyst targets build fine.
- `Bingo.UI.Shared.Tests.csproj` exists on disk but **is not referenced in `BeerAndBingo.sln`** —
  it was skipped by solution-wide build/test/package commands until this was noticed. Worth
  adding it to the `.sln`.
- **NuGet packages updated 2026-08-05** (see git history) to the latest versions that work with
  the currently-installed **MAUI workload 9.0.120** (`net9.0-*` TFMs): `Microsoft.Maui.Controls`
  → 9.0.120, `CommunityToolkit.Mvvm` → 8.4.2, `SkiaSharp`/`SkiaSharp.Views.Maui.Controls` →
  3.119.4, `Microsoft.Extensions.*`/`System.Text.Json` → 10.0.10, `Microsoft.ML` → 5.0.0,
  `TesseractOcrMaui` → 1.5.2, `Microsoft.NET.Test.Sdk` → 18.8.1, `coverlet.collector` → 10.0.1,
  `xunit.runner.visualstudio` → 3.1.5. All lib/test builds pass; app builds pass on
  Windows/iOS/MacCatalyst (Android blocked by the SDK gap above, unrelated to these bumps).
  - **Not bumped**: `Microsoft.Maui.Controls` has a true-latest of 10.0.90, but that requires
    MAUI workload 10.x (confirmed via a failed test build: error `MA003`). Moving to it means
    running `dotnet workload update` to the 10.x band and likely retargeting `net9.0-*` →
    `net10.0-*` — a deliberate upgrade, not a routine package bump. `SkiaSharp.Extended.UI.Maui`
    was left at 2.0.0 (latest 2.x; true latest 3.0.0 is untested against the 9.x MAUI workload).
  - No `global.json`/central package management — worth considering if version drift across
    projects becomes a recurring papercut.
- Player app scope/roadmap not yet defined here — fill in once decided.

## Next steps

- (Fill in as work resumes.)
