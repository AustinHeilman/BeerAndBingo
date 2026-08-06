# CLAUDE.md

Guidance for Claude Code sessions working in this repo. See `docs/` for details:
`docs/architecture.md` (layers/dependencies), `docs/setup.md` (build/run/test), `docs/status.md`
(what's currently in flight — update it as work progresses).

## What this is

Beer & Bingo — a .NET MAUI solution with two apps (`Bingo.Caller.App`, `Bingo.Player.App`) and a
set of shared class libraries under `src/libs`. Solution file: `src/BeerAndBingo.sln`.

## Conventions

- Layering is Core → Services → AppServices → ViewModel → UI.Shared; don't add references that
  point the wrong direction (e.g. `Bingo.Core` must stay dependency-free of everything above it).
- DI registration for the Caller app lives in
  `src/apps/Bingo.Caller.App/Startup/ConfigureAppServices.cs`. New services/views/view models get
  registered there.
- MVVM via `CommunityToolkit.Mvvm`.
- No central package management (`Directory.Packages.props`) — package versions are pinned
  per-`.csproj`. Keep versions consistent across sibling projects (e.g. all test projects) when
  bumping.

## Build/test

```
dotnet restore src/BeerAndBingo.sln
dotnet build src/BeerAndBingo.sln
dotnet test src/BeerAndBingo.sln
```

Full details, including per-app Android build/run commands and workload prerequisites, are in
`docs/setup.md`.
