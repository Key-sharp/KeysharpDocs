# KeysharpDocs Agent Guide

## Purpose

KeysharpDocs is the reference documentation for
[Keysharp](https://github.com/Descolada/keysharp), an independent,
cross-platform C# implementation of the AutoHotkey v2 language. Keysharp
parses `.ahk` and `.ks` source, lowers it to C#, compiles it with Roslyn, and
runs the resulting .NET assembly.

This repository began as the AutoHotkeyDocs `alpha` branch with permission
from the AutoHotkey team. Its current compatibility target is AutoHotkey
v2.1-alpha.30. That target defines intended language semantics; it does not
prove that every feature is implemented on every Keysharp platform.

Windows currently has the broadest coverage. Linux support varies between X11
and Wayland compositors and can depend on installed helpers. macOS support can
depend on Accessibility, Input Monitoring, and Screen Recording permissions.
Treat platform support as a property to verify, not infer.

Keysharp and KeysharpDocs are not affiliated with or endorsed by AutoHotkey.
Retain the independence and provenance statements in the site.

## Repository map

- `docs/` contains the source documentation.
- `docs/lib/` contains most function, directive, object, and statement pages.
- `docs/howto/` and `docs/misc/` contain tutorials and conceptual material.
- `docs/static/` contains the shared page shell, theme, search, and assets.
- `docs/static/source/data_toc.js` is the manually maintained table of
  contents.
- `docs/static/source/data_index.js` is the manually maintained keyword index.
- `docs/static/source/data_search.js` is generated; do not hand-edit it.
- `docs/index.htm` is the production homepage.
- Root `index.html` is a source-tree convenience redirect and is not included
  in the Pages artifact. Do not edit it to change the production homepage.
- `.github/workflows/docs.yml` copies the contents of `docs/` to the root of
  the Pages artifact. Production URLs therefore do not contain `/docs/`.
- `Project.hhp` and `compile_chm.ahk` define the optional Windows CHM build.
- `scripts/Test-Docs.ps1` checks required files, local links, titles, and
  branding invariants.
- `AutoHotkeyChangeLog.htm` and `AutoHotkeyLicense.htm` are archived upstream
  pages. Do not present them as Keysharp release or license information.

Also read `CONTRIBUTING.md` before semantic edits and `UPSTREAM.md` before
porting an AutoHotkeyDocs change.

## Source of truth

Documentation must describe Keysharp behavior, not merely repeat the
compatibility target.

Use evidence in this order:

1. A reproducible script run with the current Keysharp build on the named
   platform.
2. Keysharp source and tests.
3. `docs/capabilities.json` in the Keysharp repository. The rendered
   `docs/capabilities.md` matrix is generated from this file.
4. `docs/reference.md` in the Keysharp repository.
5. AutoHotkeyDocs and AutoHotkey behavior, but only as the intended
   compatibility specification or a historical reference.

Do not turn `Unknown` into `Unsupported`, or `Partial` into a vague statement
that a feature "works." State the verified boundary:

- **Full**: implemented and generally usable on the named platform.
- **Partial**: implemented, with the exact missing behavior or prerequisite
  stated.
- **Planned/unsupported**: unavailable; say whether it is intentionally
  unsupported or simply not implemented yet when that is known.
- **Unknown/unverified**: no reliable result is available. Do not guess.

If the evidence conflicts, prefer the tested current runtime and update or
report the stale capability data separately.

## Keysharp and AutoHotkey wording

Use **Keysharp** for the implementation, executable, installer, runtime
behavior, downloads, issue tracker, and project community.

Do not globally replace `AutoHotkey` or `AHK`. Preserve names and references
which are part of compatibility or attribution, including:

- `#Requires AutoHotkey`;
- `.ahk` file names;
- `A_AhkVersion`, `A_AhkPath`, `ahk_class`, and similar compatibility names;
- comparisons between AutoHotkey v1, v2.0, and v2.1;
- upstream issues, pull requests, changelogs, forums, and specifications; and
- copyright, acknowledgements, and provenance.

When behavior differs, identify the subject explicitly:

> In AutoHotkey v2.1, ... . In Keysharp, ... .

Do not silently rewrite an inherited AutoHotkey rule into a Keysharp rule if
the difference matters to compatible scripts.

## Adding platform-specific material

Platform notes should answer a user-visible question: Is the feature
available? Does it behave differently? Does it require permission, a helper,
or a particular desktop backend? Avoid implementation trivia which does not
change setup, observable behavior, portability, security, or performance.

Write the platform-independent contract first. Add the exception as close as
possible to the affected statement without interrupting every paragraph.

Use this decision table:

| Situation | Presentation | Placement |
| --- | --- | --- |
| The whole feature is unavailable or materially restricted on a platform | One `warning` paragraph | Immediately after the introductory description or syntax |
| One platform has a short behavioral difference or prerequisite | One `note` paragraph | In `Remarks`, next to the affected behavior |
| Two or more platforms differ in meaningful ways | `Platform Notes` subsection with an `info` table | Near the start of `Remarks` |
| A long setup procedure is platform-specific | A dedicated platform subsection | On an install/how-to page; link to it from the API page |
| Only one example is platform-specific | Label that example | In the example description; keep a portable example first when possible |

### Whole-feature availability

Use an early warning when a reader otherwise could build around an API which
is unavailable:

```html
<p class="warning"><strong>Platform:</strong> This function is supported only
on Windows and is unavailable on Linux and macOS.</p>
```

Name the actual result: compile-time error, thrown exception, empty value,
no-op, or missing integration. Do not write only "Windows-specific" if the
runtime outcome is known.

### One compact divergence

Use a note for one bounded caveat:

```html
<p class="note"><strong>macOS:</strong> Reading the screen requires Screen
Recording permission. Keysharp requests it when this feature is first used.</p>
```

Do not stack three consecutive platform note boxes. Once multiple platforms
need separate explanations, use a table.

### Multiple platform differences

Use this structure when comparison materially helps:

```html
<h3 id="Platform_Notes">Platform Notes</h3>
<table class="info">
  <tr><th>Platform</th><th>Behavior</th><th>Requirements</th></tr>
  <tr>
    <td>Windows</td>
    <td>Describe the observable behavior.</td>
    <td>None.</td>
  </tr>
  <tr>
    <td>Linux (X11)</td>
    <td>Describe the observable behavior or exact limitation.</td>
    <td>Describe any helper or permission.</td>
  </tr>
  <tr>
    <td>Linux (Wayland)</td>
    <td>Describe protocol- or compositor-dependent behavior.</td>
    <td>Name the helper, portal, extension, or compositor requirement.</td>
  </tr>
  <tr>
    <td>macOS</td>
    <td>Describe the observable behavior or exact limitation.</td>
    <td>Name the required Privacy &amp; Security permission.</td>
  </tr>
</table>
```

Use the platform order **Windows, Linux, macOS**. Within Linux, use **X11,
Wayland**. Split Wayland into GNOME, KDE/KWin, Cinnamon, or another compositor
only when the distinction changes the documented result.

Do not use "Unix" as a synonym for Linux and macOS unless the statement was
verified for both. Do not describe all Wayland environments from a result on
one compositor.

### Availability is not the same as prerequisites

Keep these ideas separate:

- "Supported on macOS; requires Accessibility permission."
- "Supported on Linux when `keysharp-inputd` is installed."
- "Partially supported on Wayland; activation depends on the compositor
  protocol."
- "Unverified on macOS."

A required permission or privileged helper does not automatically make a
feature partial. Conversely, successful parsing or compilation does not prove
that the native operation works.

### Keep notes concise

- Lead with the difference, then the reason only if it helps the user.
- Prefer one exact limitation over a list of internal backends.
- Link to the Keysharp platform reference for lengthy installation steps.
- Avoid repeating the full capability matrix on individual pages.
- Include versions only when behavior actually changed at that boundary.
- State tested compositor or permission assumptions in the pull request when
  they would clutter the reference page.

## Page editing conventions

- Documentation pages are HTML, not Markdown.
- Page titles end with `| Keysharp`, except the explicitly archived
  AutoHotkey changelog and license pages.
- Preserve the existing heading hierarchy and stable anchor IDs.
- Put general semantics before `Remarks`, platform differences in or near
  `Remarks`, and runnable material under `Examples`.
- Use `<code>` for identifiers and inline code, `<pre>` for Keysharp script
  examples, and `<pre class="no-highlight">` for shell commands or plain text.
- Use relative links between documentation pages.
- Use current Keysharp/KeysharpDocs links for product and support actions.
- Retain upstream links when they are citations, specifications, history, or
  attribution, and make their context clear.
- Prefer the existing `note`, `warning`, `info`, `Syntax`, and `NoIndent`
  styles. Do not introduce a one-off visual language for platform notes.
- Keep HTML and CSS compatible with the static site and CHM viewer. Do not add
  a framework, build-time dependency, web font, remote script, or feature
  which is required for the content to remain readable.
- Keep the light and dark themes usable. Do not encode platform status by
  color alone.
- Use plain platform names instead of emoji or icon-only badges; they survive
  search, translation, accessibility tools, printing, and CHM rendering.

When an inherited paragraph is correct for Keysharp, leave it intact apart
from necessary Keysharp subject wording. Small, reviewable addenda are easier
to audit and port than broad rewrites.

Do not add a platform label merely because an inherited page mentions
Windows. Determine whether it means the Windows operating system, a GUI
window, or historical AutoHotkey behavior, and whether Keysharp actually
differs. If a whole family of pages shares one limitation, keep each page's
warning short and link to one maintained explanation instead of copying a
large platform essay everywhere.

## Examples

- Prefer a portable example first.
- Label an OS-specific example in its prose and comments.
- Use `.ks` for explicitly Keysharp-oriented file examples; retain `.ahk`
  where compatibility or editor tooling is the point.
- Do not claim that an example is cross-platform unless it was verified on
  each named platform.
- If an example requires elevation, Accessibility, Input Monitoring, Screen
  Recording, a Linux helper, or a desktop extension, state that before the
  code.
- Keep examples safe to run. Do not make destructive filesystem, registry, or
  process changes without an explicit warning and narrowly scoped target.

## Adding or moving pages

After adding, removing, or renaming a page:

1. Update `docs/static/source/data_toc.js`.
2. Add useful terms to `docs/static/source/data_index.js`.
3. Add the file or asset to `Project.hhp` when the CHM build needs an explicit
   entry.
4. Regenerate `docs/static/source/data_search.js` on Windows:

   ```powershell
   & "C:\Program Files\AutoHotkey\v2\AutoHotkey32.exe" `
     .\docs\static\source\build_search.ahk
   ```

The inherited search builder requires the 32-bit AutoHotkey v2 executable
because it uses the Windows HTML document COM component. If that tool is not
available, do not fabricate or hand-edit generated search data; disclose that
the regeneration is pending.

## Validation

Run the repository validator from the root:

```powershell
pwsh ./scripts/Test-Docs.ps1
```

Windows PowerShell 5.1 is also supported:

```powershell
powershell -ExecutionPolicy Bypass -File ./scripts/Test-Docs.ps1
```

Preview through a local server:

```powershell
python -m http.server 8000
```

Then check the changed page through `http://localhost:8000/`. For layout or
shell changes, also check:

- sidebar navigation, index, and search;
- direct page loading and back/forward navigation;
- light and dark schemes;
- a narrow/mobile viewport;
- code highlighting and example download controls;
- local and changed external links; and
- the Keysharp independence/compatibility banner and footer.

The Pages workflow serves `docs/*` at the production root. If deployment
packaging changes, build the same `_site` layout locally and verify that `/`,
`/static/keysharp_logo.png`, and at least one nested reference page return
without a redirect.

## Upstream and scope discipline

- Port AutoHotkeyDocs changes manually; do not use GitHub's **Sync fork**
  button or merge the upstream branch wholesale.
- Verify new upstream behavior exists in Keysharp before documenting it as
  implemented.
- Preserve Keysharp branding, navigation, title suffixes, repository links,
  compatibility notices, and Pages workflow.
- Keep an upstream semantic port separate from unrelated Keysharp-specific
  documentation when practical.
- Do not modify Keysharp runtime code from this repository. Report or fix
  runtime gaps in the Keysharp repository, then document the resulting
  behavior here.
- Do not invent a documentation license. Preserve `NOTICE.md`,
  `docs/license.htm`, and the archived upstream attribution.

## Git and deployment

- `alpha` is the maintained default branch.
- Pull requests to `alpha` run documentation validation.
- Pushes to `alpha` validate and deploy the production Pages site.
- Treat a push to `alpha` as a production deployment; do not push merely to
  preview work or unless the task authorizes deployment.
- Keep the worktree's unrelated changes intact.
- Prefer a focused documentation commit over mixing generated churn,
  upstream ports, branding changes, and semantic edits.
- After changing the AutoHotkey compatibility target, update the homepage,
  shared compatibility banner, README, change page, and this guide together.

## Completion checklist

Before finishing a documentation change, confirm:

- the claim is supported by current Keysharp evidence;
- `Keysharp` and `AutoHotkey` are used in the correct roles;
- availability, prerequisites, and unverified status are not conflated;
- platform material uses the smallest readable pattern from this guide;
- headings, anchors, titles, local links, TOC, index, and search data are
  consistent;
- the page remains readable without remote assets or modern-only browser
  features;
- `scripts/Test-Docs.ps1` passes; and
- the site was previewed in proportion to the visual risk of the change.
