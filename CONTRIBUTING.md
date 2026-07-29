# Contributing to KeysharpDocs

Thank you for helping document Keysharp.

## Source of truth

Documentation should describe observed Keysharp behavior. AutoHotkey
v2.1-alpha.30 is the compatibility target and a valuable language reference,
but it is not evidence that a feature is already implemented on every Keysharp
platform.

For current implementation status, consult:

- the [Keysharp capability matrix](https://github.com/Descolada/keysharp/blob/master/docs/capabilities.md);
- the [Keysharp platform reference](https://github.com/Descolada/keysharp/blob/master/docs/reference.md);
- the Keysharp source and tests; and
- a reproducible script run against the current Keysharp build.

If support is partial or unverified, say so explicitly. Prefer a short note near
the relevant behavior over a broad claim that the whole page is cross-platform.

## Preserve compatibility names

Do not globally replace `AutoHotkey` or `AHK`. These can be intentional:

- `#Requires AutoHotkey`
- `.ahk` file names
- `A_AhkVersion`, `A_AhkPath`, and other compatibility variables
- `ahk_class` and related window matching syntax
- historical comparisons with AutoHotkey v1 or v2
- links to upstream specifications, issues, pull requests, and forum material
- acknowledgements and copyright notices

Use **Keysharp** when describing this implementation, executable, installer,
runtime behavior, downloads, issue tracker, or project community.

## Page titles and links

- Page titles must end with `| Keysharp`.
- Product and support links should use the current Keysharp or KeysharpDocs
  repository.
- Upstream citations should remain pointed at their original AutoHotkey source
  and should be described as upstream references.
- Do not remove the independence or compatibility notices.

## Generated data

After adding, removing, or renaming a page:

1. Update `docs/static/source/data_toc.js`.
2. Update `docs/static/source/data_index.js` when an index entry is useful.
3. Rebuild `docs/static/source/data_search.js`:

   ```powershell
   & "C:\Program Files\AutoHotkey\v2\AutoHotkey32.exe" `
     .\docs\static\source\build_search.ahk
   ```

The upstream search builder currently depends on the Windows HTML document COM
component and therefore requires the 32-bit AutoHotkey v2 executable.

## Validation

Run:

```powershell
pwsh ./scripts/Test-Docs.ps1
```

Also preview the site through a local HTTP server and check:

- sidebar navigation, index, and search;
- light and dark modes;
- narrow/mobile layouts;
- the page edit link;
- code highlighting and downloaded examples; and
- any changed external links.

## Pull requests

Keep branding/shell changes separate from large semantic ports when practical.
State which platforms and Keysharp build were used to verify behavioral
changes. Do not combine an unreviewed upstream sync with unrelated
Keysharp-specific documentation.
