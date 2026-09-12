# GameAITesterOnline
Web portal for determining the best AI players for games

## Build status

[![Build status](https://ci.appveyor.com/api/projects/status/pa7ox3tdq064u84y?svg=true)](https://ci.appveyor.com/project/NikolayIT/gameaitesteronline)

## Static archive of ai.bgcoder.com

The portal is no longer maintained. Everything an anonymous visitor could see on
https://ai.bgcoder.com (home, battles, teams, competitions, uploads: about 700 pages)
was frozen into `docs/` on 2026-09-12 by `tools/export-static-site.py`, so the site can be
served as plain static files (GitHub Pages or Cloudflare Workers) without the ASP.NET app,
its database or the IIS server.

- URLs are unchanged (`/Battles/Info/123` is `docs/Battles/Info/123.html`; both hosts serve
  extensionless requests from the `.html` file).
- The ASP.NET bundles were saved as plain files (`Content/site.css`, `Scripts/*.js`, `fonts/`).
- The Telerik Academy login is gone. The user avatars that were hot-linked from
  telerikacademy.com no longer exist there, so they show `Content/avatar-placeholder.svg`.

Re-running the script re-crawls the live site (while it is still up) and overwrites `docs/`.
