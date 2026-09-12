# GameAITesterOnline

[![Build](https://github.com/NikolayIT/GameAITesterOnline/actions/workflows/build.yml/badge.svg)](https://github.com/NikolayIT/GameAITesterOnline/actions/workflows/build.yml)

Web portal for finding the best AI players for card games. Students signed in with their
Telerik Academy account, formed teams and uploaded their bot as a .NET `.dll`; a background
worker then played it against the other teams' bots and the portal ranked the teams by the
results. It ran at https://ai.bgcoder.com from 2016 with two competitions:
**Santase (66)** and **Texas Hold'em**.

> **Archived.** The application was taken offline on 2026-09-12. Its public pages are kept
> as a static archive at https://ai.bgcoder.com (GitHub Pages, served from `docs/`), and the
> code stays here for reference only. It is not maintained.

## How it worked

1. A team uploads a `.dll` with a player for one of the game engines,
   [Santase.Logic](https://github.com/NikolayIT/SantaseGameEngine) or
   [TexasHoldem.Logic](https://github.com/NikolayIT/TexasHoldemGameEngine). The upload is
   loaded in a sandbox and validated against the engine's player contract.
2. The portal generates battles between pairs of teams in the competition.
3. `OnlineGames.Workers.BattlesSimulator`, a Windows service, picks up pending battles,
   plays the games with the engine and records every game's result.
4. Won battles turn into points; the competition page shows the ranking and each battle
   page lists all of its games.

## Repository layout

| Path | What it is |
|---|---|
| `src/OnlineGames.sln` | .NET Framework 4.6 solution |
| `src/Web/OnlineGames.Web.AiPortal` | ASP.NET MVC 5 site (OWIN + ASP.NET Identity, Telerik Academy OAuth login, Ninject, AutoMapper) |
| `src/Services/OnlineGames.Services.AiPortal` | upload validation, sandbox, battle generation |
| `src/Workers/OnlineGames.Workers.BattlesSimulator` | Windows service that runs the battles |
| `src/Data/*` | Entity Framework 6 models, context and repositories (SQL Server) |
| `src/OnlineGames.Common` | shared helpers |
| `docs/` | static archive of the public site, published by GitHub Pages at ai.bgcoder.com |
| `tools/export-static-site.py` | the crawler that produced `docs/` |

## Building

Visual Studio 2022 opens `src/OnlineGames.sln`. From the command line:

```powershell
nuget restore src/OnlineGames.sln
msbuild src/OnlineGames.sln /p:Configuration=Release
```

The projects target .NET Framework 4.6, whose targeting pack no longer ships with Visual
Studio. Either install it separately or point MSBuild at the reference assemblies from
NuGet, as the GitHub Actions workflow in `.github/workflows/build.yml` does. Running the
site needs a SQL Server database (`DefaultConnection` in `Web.config`) and Telerik Academy
OAuth credentials, neither of which exists any more.

## Static archive of ai.bgcoder.com

Everything an anonymous visitor could see (home, battles, teams, competitions, uploads:
693 pages) was frozen into `docs/` on 2026-09-12 by `tools/export-static-site.py`.

- URLs are unchanged: `/Battles/Info/123` is `docs/Battles/Info/123.html`, and GitHub Pages
  serves the extensionless request from the `.html` file.
- The ASP.NET bundles were saved as plain files (`Content/site.css`, `Scripts/*.js`, `fonts/`).
- The Telerik Academy login is gone. The user avatars that were hot-linked from
  telerikacademy.com no longer exist there, so they show `Content/avatar-placeholder.svg`.

## License

[MIT](LICENSE)
