# Documentation Source of Truth

## Purpose

KeysharpDocs must distinguish three different questions:

1. **Does a script-visible API exist?**
2. **What does that API do?**
3. **On which platforms and configurations has that behavior been verified?**

No single source answers all three. In particular, an inherited AutoHotkey page
describes the compatibility target, while a public C# declaration proves
neither that the member is script-visible nor that its native operation works
on every platform.

## Evidence rules

Use evidence in this order for a documentation claim:

1. A reproducible script run with an identified Keysharp build on the named
   platform.
2. Keysharp tests and source.
3. Keysharp's `docs/reference.md`.
4. A capability entry which has been independently rechecked against items
   1-3.
5. AutoHotkey documentation or behavior, as the intended compatibility
   contract rather than proof of current Keysharp behavior.

Record the narrowest claim the evidence supports. A Windows result does not
establish Linux or macOS behavior. Successful parsing does not establish that
a native operation succeeds. A public member does not establish that its
behavior is complete.

When evidence conflicts, current reproducible behavior wins. Report stale
source comments, tests, reference notes, or capability data separately so they
can be corrected.

## Source-derived API inventory

The generated files in `audits/` answer only the first question: which public
types, global functions, and global properties the current runtime exposes as
candidate script API.

The inventory mirrors the runtime rule in
`Keysharp.Core/Internals/Invoke/Reflections.cs`:

- exported classes under `Keysharp.Builtins` are script-visible unless marked
  `[PublicHiddenFromUser]`;
- public static methods and properties on exported static built-in classes are
  globally discoverable unless marked `[PublicHiddenFromUser]`;
- `[UserDeclaredName]` supplies the script-visible name; and
- `Keysharp.Runtime.Ahk` is included as the runtime's explicit exception.

Each audit records the source commit, whether the source tree was dirty, the
host platform, the compiled assembly hash, and the KeysharpDocs commit. This
identity is part of the evidence. A report generated from a dirty source tree
is provisional until it is regenerated from a reviewed commit.

The documentation status in this inventory is intentionally conservative:

- **Locator found** means the name has a structural documentation locator such as
  an exact index entry, page name, heading, or anchor.
- **Needs review** means no such locator was found.

`Locator found` does not prove that the located content documents the runtime
member's contract. Likewise, `Needs review` is not automatically `Unsupported`
or even `Undocumented`. The
member might be intentionally internal but missing `[PublicHiddenFromUser]`,
documented under another public name, or genuinely missing documentation.
Every item must be triaged.

The inventory does not yet cover directives, language syntax, command-line
options, or the complete member surface of built-in classes. Those are
separate audit scopes so that their different exposure rules remain explicit.

## Regenerating the audit

From the KeysharpDocs repository:

```powershell
pwsh ./scripts/Update-SourceAudit.ps1 `
  -KeysharpRepo ../Keysharp_clone
```

By default, the script builds only `Keysharp.Core`; it does not run the test
suite. Use `-SkipKeysharpBuild` only when the existing assembly is known to
match the source tree.

The current host produces:

- `audits/global-api.windows.json`, the machine-readable inventory; and
- `audits/global-api.windows.md`, the human review queue.

Run the same process on Linux and macOS before interpreting platform-specific
surface differences. A later CI job can publish all three audit artifacts,
but platform behavior still requires tests or reproducible probes.

## Review workflow

For each `Needs review` entry:

1. Confirm that a script can resolve the name.
2. Decide whether it is intended public API.
3. If it is unintended, fix the runtime visibility rather than hiding the
   audit result in the docs.
4. If it is intended, locate or add its canonical documentation.
5. Verify behavior with an existing test or a minimal script; add a focused
   test when the contract is otherwise ambiguous.
6. Record platform qualifications only for platforms actually checked.
7. Regenerate the inventory and run `scripts/Test-Docs.ps1`.

Do not add manual exceptions merely to improve the coverage count. Any
exception must state the alternate public name or the source rule which makes
the candidate non-public.

## Planned audit scopes

Add further scopes independently:

1. built-in class methods and properties;
2. directives and command-line options;
3. built-in variables which are created outside static properties;
4. lexer/parser syntax and reserved words;
5. runnable documentation examples; and
6. Windows, Linux/X11, Linux/Wayland, and macOS behavior probes.

This separation keeps the generated inventory useful without turning it into
another manually maintained capability matrix.
