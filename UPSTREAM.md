# Porting changes from AutoHotkeyDocs

KeysharpDocs intentionally follows AutoHotkeyDocs for compatible language
behavior, but upstream changes are ported manually. A direct GitHub **Sync
fork** can reintroduce AutoHotkey branding, version selectors, product links,
or behavior which Keysharp has not implemented.

## Configure the upstream remote

```powershell
git remote add upstream https://github.com/AutoHotkey/AutoHotkeyDocs.git
git fetch upstream alpha
```

If the remote already exists, only the fetch command is required.

## Recommended sync workflow

```powershell
git switch alpha
git pull --ff-only origin alpha
git switch -c sync/autohotkey-alpha-YYYY-MM-DD
git log --oneline alpha..upstream/alpha
git diff --stat alpha...upstream/alpha
```

Port individual commits with `git cherry-pick -n`, or reproduce the relevant
content changes manually. Before committing:

1. Confirm that the documented behavior exists in Keysharp.
2. Retain Keysharp page-title suffixes, logo, theme, notices, navigation, and
   repository links.
3. Review platform differences.
4. Regenerate the search data when page content or names change.
5. Run `scripts/Test-Docs.ps1` and preview the site.

Avoid merging `upstream/alpha` wholesale. If a large merge is genuinely useful,
perform it on a temporary branch and review every conflict and branding
invariant before opening a pull request.

## References that should normally remain upstream

These are not branding defects when used in context:

- `#Requires AutoHotkey`
- AutoHotkey version-history pages
- upstream issue, pull-request, changelog, and forum links
- acknowledgements
- comparisons with AutoHotkey v1/v2

The distinction is simple: upstream references explain compatibility or
provenance; Keysharp links identify the product, downloads, support, and
maintained documentation.
