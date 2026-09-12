#!/usr/bin/env python3
"""
Freeze the public part of https://ai.bgcoder.com (the AiPortal MVC app) into docs/ as a
plain static site, so it can be served by GitHub Pages or Cloudflare without the app,
its database or the IIS server.

What is exported (everything an anonymous visitor can see):
    /                        -> docs/index.html
    /Battles/All             -> docs/Battles/All.html
    /Uploads/All             -> docs/Uploads/All.html
    /Battles/Info/<id>       -> docs/Battles/Info/<id>.html
    /Teams/Info/<id>         -> docs/Teams/Info/<id>.html
    /Competitions/Info/<id>  -> docs/Competitions/Info/<id>.html
Extensionless links stay as they are in the HTML: GitHub Pages and Cloudflare Workers
static assets both serve /Battles/Info/123 from Battles/Info/123.html.

Rewrites applied to every page:
    - the versioned ASP.NET bundles become plain files (/Content/site.css, /Scripts/*.js)
    - the "Log in with Telerik Academy" links are removed (login is gone with the app)
    - avatars hot-linked from telerikacademy.com are downloaded when they still exist,
      otherwise replaced by /Content/avatar-placeholder.svg

Usage:  python tools/export-static-site.py            (from the repo root, stdlib only)
"""
import concurrent.futures as cf
import html
import re
import sys
import time
import urllib.error
import urllib.request
from collections import deque
from pathlib import Path

BASE = "https://ai.bgcoder.com"
ROOT = Path(__file__).resolve().parent.parent
OUT = ROOT / "docs"
PUBLIC = re.compile(r"^/(|Battles/All|Uploads/All|Battles/Info/\d+|Teams/Info/\d+|Competitions/Info/\d+)$")
AVATAR = re.compile(r'src="(https?://(?:www\.)?telerikacademy\.com/Content/(?:Avatars|Images/DefaultAvatar)/[^"]+)"')
PLACEHOLDER = "/Content/avatar-placeholder.svg"
PLACEHOLDER_SVG = """<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 32 32" width="32" height="32">
  <rect width="32" height="32" rx="4" fill="#3e444c"/>
  <circle cx="16" cy="12" r="5.5" fill="#9aa3ad"/>
  <path d="M6 28c0-6 4.5-9 10-9s10 3 10 9z" fill="#9aa3ad"/>
</svg>
"""
UA = {"User-Agent": "ai.bgcoder.com static export (github.com/NikolayIT/GameAITesterOnline)"}


def fetch(url, tries=4):
    last = None
    for attempt in range(tries):
        try:
            with urllib.request.urlopen(urllib.request.Request(url, headers=UA), timeout=60) as r:
                return r.status, r.headers.get("Content-Type", ""), r.read()
        except urllib.error.HTTPError as e:
            if e.code in (404, 410):
                return e.code, "", b""
            last = e
        except Exception as e:  # noqa: BLE001 - network hiccup, retry
            last = e
        time.sleep(1.5 * (attempt + 1))
    raise RuntimeError(f"{url}: {last}")


def save(rel, data):
    p = OUT / rel.lstrip("/")
    p.parent.mkdir(parents=True, exist_ok=True)
    p.write_bytes(data)


def page_path(path):
    return "index.html" if path == "/" else path.lstrip("/") + ".html"


def main():
    OUT.mkdir(exist_ok=True)
    (OUT / ".nojekyll").write_bytes(b"")
    save("Content/avatar-placeholder.svg", PLACEHOLDER_SVG.encode())

    # --- static assets -------------------------------------------------------------
    assets = {}
    status, _, home = fetch(BASE + "/")
    assert status == 200, status
    home = home.decode("utf-8")
    css = re.search(r'href="(/Content/css\?v=[^"]+)"', home).group(1)
    boot = re.search(r'src="(/bundles/bootstrap\?v=[^"]+)"', home).group(1)
    jq = re.search(r'src="(/bundles/jquery\?v=[^"]+)"', home).group(1)
    assets[css] = "/Content/site.css"
    assets[boot] = "/Scripts/bootstrap.bundle.js"
    assets[jq] = "/Scripts/jquery.bundle.js"
    for src, dst in assets.items():
        st, _, data = fetch(BASE + src)
        assert st == 200, (src, st)
        save(dst, data)
        print(f"asset {src} -> {dst} ({len(data)} B)")
    st, _, fav = fetch(BASE + "/favicon.ico")
    if st == 200:
        save("favicon.ico", fav)
    css_text = (OUT / "Content/site.css").read_text("utf-8", errors="replace")
    for ref in sorted(set(re.findall(r"url\(['\"]?(\.\./fonts/[^'\")?#]+)", css_text))):
        rel = ref.replace("../", "/")
        st, _, data = fetch(BASE + rel)
        if st == 200:
            save(rel, data)
            print(f"font  {rel} ({len(data)} B)")
        else:
            print(f"font  {rel} MISSING ({st})")

    # --- crawl ------------------------------------------------------------------------
    seen, queue, pages, missing, skipped = set(), deque(["/", "/Battles/All", "/Uploads/All"]), {}, [], set()
    seen.update(queue)

    def get_page(path):
        st, ctype, data = fetch(BASE + path)
        return path, st, data.decode("utf-8") if st == 200 else ""

    with cf.ThreadPoolExecutor(max_workers=8) as pool:
        while queue:
            batch = [queue.popleft() for _ in range(min(len(queue), 32))]
            for path, st, text in pool.map(get_page, batch):
                if st != 200:
                    missing.append((path, st))
                    continue
                pages[path] = text
                for href in re.findall(r'href="(/[^"]*)"', text):
                    href = html.unescape(href)
                    if PUBLIC.match(href):
                        if href not in seen:
                            seen.add(href)
                            queue.append(href)
                    elif not href.startswith(("/Content/", "/bundles/")):
                        skipped.add(href)
            print(f"crawled {len(pages)} pages, {len(queue)} queued", flush=True)

    # --- avatars ------------------------------------------------------------------------
    avatar_urls = sorted({u for t in pages.values() for u in AVATAR.findall(t)})
    avatar_map, alive = {}, 0

    def get_avatar(url):
        try:
            st, ctype, data = fetch(url, tries=2)
        except RuntimeError:
            st, ctype, data = 0, "", b""
        return url, st, ctype, data

    with cf.ThreadPoolExecutor(max_workers=8) as pool:
        for url, st, ctype, data in pool.map(get_avatar, avatar_urls):
            if st == 200 and ctype.startswith("image/") and data:
                rel = "/Content/" + url.rsplit("/Content/", 1)[1]
                save(rel, data)
                avatar_map[url] = rel
                alive += 1
            else:
                avatar_map[url] = PLACEHOLDER
    print(f"avatars: {len(avatar_urls)} unique, {alive} still online, {len(avatar_urls) - alive} replaced by the placeholder")

    # --- rewrite + write ----------------------------------------------------------------
    login_li = re.compile(r'[ \t]*<li><a href="/Account/LoginWithTelerikAcademy"[^>]*>[^<]*</a></li>\r?\n?')
    login_btn = re.compile(r'[ \t]*<a class="btn btn-info" href="/Account/LoginWithTelerikAcademy">[^<]*</a>\r?\n?')
    stamp = f"<!-- Static archive of ai.bgcoder.com, exported {time.strftime('%Y-%m-%d')} by tools/export-static-site.py -->\n"
    leftovers = set()
    for path, text in pages.items():
        for src, dst in assets.items():
            text = text.replace(f'"{src}"', f'"{dst}"').replace(f'"{html.escape(src)}"', f'"{dst}"')
        text = login_li.sub("", text)
        text = login_btn.sub("", text)
        text = AVATAR.sub(lambda m: f'src="{avatar_map[m.group(1)]}"', text)
        for ref in re.findall(r'(?:href|src)="(/Account/[^"]*|/bundles/[^"]*|/Content/css[^"]*)"', text):
            leftovers.add((path, ref))
        text = text.replace("<!DOCTYPE html>", "<!DOCTYPE html>\n" + stamp, 1)
        save(page_path(path), text.encode("utf-8"))

    print(f"pages written: {len(pages)}")
    if missing:
        print(f"missing pages ({len(missing)}): " + ", ".join(f"{p} ({s})" for p, s in sorted(missing)))
    if skipped:
        print("internal links not exported: " + ", ".join(sorted(skipped)))
    if leftovers:
        print("WARNING - dynamic references left in pages: " + ", ".join(f"{p}: {r}" for p, r in sorted(leftovers)))
    return 0


if __name__ == "__main__":
    sys.exit(main())
