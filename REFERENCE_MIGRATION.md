# Keysharp `reference.md` Migration Plan

## Goal

Migrate every current, user-facing claim from
`Keysharp/docs/reference.md` into the canonical KeysharpDocs page where a
reader would expect to find it. The Markdown file is a migration source, not
the intended long-term information architecture.

The embedded capability matrix at source lines 226-255 is intentionally
excluded because its data has not yet been reviewed. It must not be used as
evidence or copied into KeysharpDocs. Once independently verified, capability
data can be rebuilt as a separate summary of the canonical documentation.

## Migration rules

- Put installation and shared platform setup in `docs/howto/Install.htm`.
- Put reusable platform behavior and prerequisites in `docs/Platforms.htm`.
- Put runtime architecture and command-line behavior in `docs/Program.htm`.
- Put each API contract or limitation in its existing reference page when one
  exists.
- Create a reference page for every Keysharp-specific API which has no
  inherited page. Closely related small APIs may share a page with stable
  anchors.
- Put a short compatibility index in `docs/KeysharpDifferences.htm`; link to
  canonical pages instead of duplicating their full contracts.
- Update the TOC, keyword index, CHM manifest and generated search data for
  every new page.
- A row is complete only after its content, navigation and links validate.

## Shared guides and project material

| ID | Source lines | Content | Canonical destination | Status |
| --- | ---: | --- | --- | --- |
| SETUP-01 | 17-18 | .NET 10 prerequisite | `howto/Install.htm#Prerequisites` | Complete |
| SETUP-02 | 20-46 | Windows support, install, portable use and build/package steps | `Platforms.htm#Windows`, `howto/Install.htm#Windows` | Complete |
| SETUP-03 | 48-57 | Linux X11 and Wayland/compositor support | `Platforms.htm#Linux` | Complete |
| SETUP-04 | 59-77 | Linux install, helpers, extensions and editor shim | `howto/Install.htm#Linux` | Complete |
| SETUP-05 | 79-86 | Linux source and package build | `howto/Install.htm#Build_Linux` | Complete |
| SETUP-06 | 88-104 | macOS feature status and permissions | `Platforms.htm#macOS` | Complete |
| SETUP-07 | 106-153 | macOS requirements, DMG/PKG install, Gatekeeper, commands and editor shim | `howto/Install.htm#macOS` | Complete |
| SETUP-08 | 154-168 | macOS permission setup and capability request | `Platforms.htm#macOS_Permissions`, `howto/Install.htm#macOS_Permissions` | Complete |
| SETUP-09 | 169-189 | macOS uninstall and TCC reset | `howto/Install.htm#Uninstall_macOS` | Complete |
| SETUP-10 | 191-210 | macOS source/package build and signing | `howto/Install.htm#Build_macOS` | Complete |
| GUI-01 | 212-224 | macOS App/Edit menus and `AppMenu` option | `lib/Gui.htm#AppMenu` | Complete |
| MATRIX-01 | 226-255 | Unreviewed capability matrix | Explicitly excluded; do not migrate | Excluded |
| PROJECT-01 | 257-265 | History, platforms and alpha status | `Keysharp.htm#Status` | Complete |
| PROGRAM-01 | 267-282 | compilation pipeline, file types and Keyview | `Program.htm#execution-model`, `Program.htm#create` | Complete |
| ACK-01 | 835-849 | Code acknowledgements | `misc/Acknowledgements.htm#Keysharp` | Complete |
| SUPPORT-01 | 851-853 | Issue tracker and support scope | `Keysharp.htm#Links`, `howto/Install.htm#Problems` | Complete |

## Behavioral and syntax differences

| ID | Source lines | Content | Canonical destination | Status |
| --- | ---: | --- | --- | --- |
| DIFF-01 | 288-293 | Linux controls, AtSpi, GUI and Registry limits | `Platforms.htm#Linux`, control pages, Registry pages | Complete |
| DIFF-02 | 294-302 | .NET lifetimes, garbage collection and `__Delete` | `Objects.htm#Lifetime` | Complete |
| DIFF-03 | 303-305 | Increment/decrement of unset variables | `Variables.htm#IncDec` | Complete |
| DIFF-04 | 306-309 | `FuncObj`, `Func()` and built-in function objects | `lib/Func.htm` | Complete |
| DIFF-05 | 310 | Error stack trace origin | `lib/Error.htm` | Complete |
| DIFF-06 | 311-314, 349-351 | `StrPtr`, `StringBuffer` and `ObjFree` behavior | `lib/StrPtr.htm`, `lib/StringBuffer.htm`, `lib/ObjFree.htm` | Complete |
| DIFF-07 | 315-317 | `CallbackCreate` calling convention, pointers and cost | `lib/CallbackCreate.htm` | Complete |
| DIFF-08 | 318 | Deleting tab controls | `lib/GuiControl.htm#Delete` | Complete |
| DIFF-09 | 319-321 | GUI sizing, `xc`/`yc` and `UseGroup` | `lib/Gui.htm#Position`, `lib/Gui.htm#UseGroup` | Complete |
| DIFF-10 | 322-330 | WinForms `ClassNN`, `NetClassNN` and GUI lookup | `lib/GuiControl.htm#NetClassNN`, `misc/WinTitle.htm` | Complete |
| DIFF-11 | 331-334 | `TrayTip` option differences | `lib/TrayTip.htm` | Complete |
| DIFF-12 | 335-336, 352 | `Sleep`, timers and shutdown | `lib/Sleep.htm` | Complete |
| DIFF-13 | 337 | `#HotIf` optimization does not apply | `lib/_HotIf.htm` | Complete |
| DIFF-14 | 338-340 | `#ErrorStdOut` requires redirected output | `lib/_ErrorStdOut.htm` | Complete |
| DIFF-15 | 341-342 | Menus and threading; `AddStandard` lookup | `lib/Menu.htm` | Complete |
| DIFF-16 | 343 | Nested-control coordinates | `lib/ControlMove.htm`; `ControlSetPos` is no longer a current API name | Resolved |
| DIFF-17 | 344 | Function-object reflection cost | `lib/Func.htm#Performance` | Complete |
| DIFF-18 | 345 | Internal `KeysharpFile` name | `lib/File.htm` | Complete |
| DIFF-19 | 346 | `SetTimer` priority range | `lib/SetTimer.htm` | Complete |
| DIFF-20 | 347 | Null `VT_DISPATCH` assignment | `lib/ComObject.htm` | Complete |
| DIFF-21 | 348 | `A_LineNumber` reliability | `Variables.htm#LineNumber` | Complete |
| DIFF-22 | 349-350 | `ObjPtr` and `ObjPtrAddRef` return types | `lib/ObjPtr.htm` | Complete |
| DIFF-23 | 353 | Concatenation assignment performance | `Variables.htm#AssignOp` | Complete |
| DIFF-24 | 354-355 | Debug switch and compiled-script arguments | `Program.htm#cmd` | Complete |
| SYNTAX-01 | 357-359 | `DllCall` double-pointer guidance | `lib/DllCall.htm` | Complete |
| SYNTAX-02 | 360 | `ImageSearch` options parameter | Current `Screen.ImageSearch` retains options in `ImageFile` | Resolved |
| SYNTAX-03 | 361 | Leading plus on numeric literals | `Concepts.htm#numbers` | Complete |
| SYNTAX-04 | 362 | `unset` representation and `IsSet` | `Concepts.htm#unset` | Complete |
| SYNTAX-05 | 363 | Dynamic dereference performance | `Variables.htm#deref` | Complete |
| SYNTAX-06 | 364-365 | Compile-time-only `Goto` targets | `lib/Goto.htm` | Complete |
| SYNTAX-07 | 366-380 | `#Requires Keysharp` and capability form | `lib/_Requires.htm` | Complete |
| SYNTAX-08 | 381 | `__Enum` parameter count | `Objects.htm#__Enum` | Complete |
| SYNTAX-09 | 382-391 | PCRE2 options and callout differences | `misc/RegEx-QuickRef.htm`, `misc/RegExCallout.htm` | Complete |

## Keysharp-specific APIs and extensions

| ID | Source lines | Content | Canonical destination | Status |
| --- | ---: | --- | --- | --- |
| EXT-01 | 393-396 | KS module and import requirement | `Modules.htm#KS` | Complete |
| EXT-02 | 397-408 | `A_ThreadId` and targeted `Exit` | `lib/KeysharpVariables.htm#A_ThreadId`, `lib/Exit.htm` | Complete |
| EXT-03 | 409-432 | `StringBuffer` class | `lib/StringBuffer.htm` | Complete |
| EXT-04 | 434-437 | `Sinh`, `Cosh`, `Tanh` | `lib/Math.htm` | Complete |
| EXT-05 | 438 | `HashMap` | `lib/HashMap.htm` | Complete |
| EXT-06 | 439 | `RandomSeed` | `lib/Random.htm#RandomSeed` | Complete |
| EXT-07 | 440-442 | `FileDirName`, `FileFullPath` | `lib/FilePath.htm` | Complete |
| EXT-08 | 443-445 | `WinFromPoint`, `WinMaximizeAll` | dedicated function pages | Complete |
| EXT-09 | 446-447 | `ShowDebug`, `OutputDebugLine` | `lib/OutputDebug.htm` | Complete |
| EXT-10 | 448-455 | Base64, EOL, `Join`, string prefix/suffix APIs | `lib/StringFunctions.htm`, `lib/String.htm` | Complete |
| EXT-11 | 456-485 | `RegExMatchCs`, `RegExReplaceCs` | `lib/RegExCs.htm` | Complete |
| EXT-12 | 486-487 | `FormatCs` | `lib/FormatCs.htm` | Complete |
| EXT-13 | 488-502 | `RequestCapabilities` | `lib/RequestCapabilities.htm` | Complete |
| EXT-14 | 503 | `Image` class | `lib/Image.htm` | Complete |
| EXT-15 | 504 | `Overlay` class | `lib/Overlay.htm` | Complete |
| EXT-16 | 505 | GUI `AutoScroll` option | `lib/Gui.htm#AutoScroll` | Complete |
| EXT-17 | 506-510 | Clipboard image copy and empty test | `lib/ClipboardFunctions.htm` | Complete |
| EXT-18 | 511-514 | `Collect` | `lib/Collect.htm` | Complete |
| EXT-19 | 515-517 | `RunScript` and `ProcessInfo` | `lib/RunScript.htm` | Complete |
| EXT-20 | 518-550 | Keysharp built-in accessors | `lib/KeysharpVariables.htm` | Complete |
| EXT-21 | 551-555 | AES, hashes, CRC32 and secure random | `lib/Crypto.htm` | Complete |
| EXT-22 | 556-566 | `RealThread` | `lib/RealThread.htm` | Complete |
| EXT-23 | 567-598 | `WinEvent` | `lib/WinEvent.htm` | Complete |
| EXT-24 | 599-600 | `LockRun` | `lib/LockRun.htm` | Complete |
| EXT-25 | 601-612 | `Mail` | `lib/Mail.htm` | Complete |
| EXT-26 | 613-620 | Experimental `Clr` interop | `lib/Clr.htm` | Complete |
| EXT-27 | 621 | Map implementation and enumeration | `lib/Map.htm` | Complete |
| EXT-28 | 622 | Multiple spread arguments | `Functions.htm#Variadic` | Complete |
| EXT-29 | 623-624 | Buffer index and conversion methods | `lib/Buffer.htm` | Complete |
| EXT-30 | 625-640 | Array methods | `lib/Array.htm` | Complete |
| EXT-31 | 641-642 | Separate `Run`/`RunWait` arguments | `lib/Run.htm` | Complete |
| EXT-32 | 643 | GUI known colors | `lib/Gui.htm#Color` | Complete |
| EXT-33 | 644 | `ListView.DeleteCol` | `lib/ListView.htm` | Complete |
| EXT-34 | 645-649 | Menu visibility, color and metadata | `lib/Menu.htm` | Complete |
| EXT-35 | 650 | Clear a Picture control | `lib/GuiControl.htm#Value` | Complete |
| EXT-36 | 651-655 | UpDown `Increment` and `Hex` | `lib/GuiControls.htm#UpDown` | Complete |
| EXT-37 | 656 | `TabControl.SetTabIcon` | `lib/GuiControl.htm` | Complete |
| EXT-38 | 657 | `TreeView.GetNode` | `lib/TreeView.htm` | Complete |
| EXT-39 | 658 | Control autosizing argument | `lib/Gui.htm#Add` | Complete |
| EXT-40 | 659 | `Gui.Visible` | `lib/Gui.htm#Visible` | Complete |
| EXT-41 | 660 | Cross-platform `EnvUpdate` | `lib/EnvUpdate.htm` | Complete |
| EXT-42 | 661 | Unlimited hotstring abbreviation length | `Hotstrings.htm` | Complete |
| EXT-43 | 662 | `FileGetSize` G/T units | `lib/FileGetSize.htm` | Complete |
| EXT-44 | 663-664 | Millisecond date units | `lib/DateAdd.htm`, `lib/DateDiff.htm` | Complete |
| EXT-45 | 665 | Default `SubStr` start | `lib/SubStr.htm` | Complete |
| EXT-46 | 666 | `MaxIndex` and `MinIndex` | `lib/Array.htm`, `lib/Map.htm` | Complete |
| EXT-47 | 667-673 | RichEdit control | `lib/GuiControls.htm#RichEdit` | Complete |
| EXT-48 | 674-678 | Named icon resources in .NET DLLs | `lib/LoadPicture.htm`, `lib/TraySetIcon.htm`, `lib/Menu.htm` | Complete |
| EXT-49 | 679-681 | `WM_COPYDATA` string handling | `lib/SendMessage.htm` | Complete |
| EXT-50 | 682 | `A_ClipboardTimeout` | `lib/KeysharpVariables.htm#A_ClipboardTimeout` | Complete |
| EXT-51 | 683-684 | Reloading compiled scripts | `lib/Reload.htm` | Complete |
| EXT-52 | 685-686 | Signed wheel `A_EventInfo` | `Variables.htm#EventInfo` | Complete |
| EXT-53 | 687 | Custom-base `Log` | `lib/Math.htm#Log` | Complete |
| EXT-54 | 688-691 | Timer `A_EventInfo` and menu behavior | `lib/SetTimer.htm` | Complete |
| EXT-55 | 692-716 | Expanded reference-parameter targets | `Functions.htm#ByRef` | Complete |
| EXT-56 | 717 | `OwnPropCount` method | `lib/Object.htm` | Complete |
| EXT-57 | 718 | `ComObjConnect` debug parameter | `lib/ComObjConnect.htm` | Complete |
| EXT-58 | 719-757 | Conditional compilation syntax | `lib/Preprocessor.htm` | Complete |
| EXT-59 | 758-768 | Hook mutex and assembly directives | `lib/KeysharpDirectives.htm` | Complete |
| EXT-60 | 769-792 | Complete command-line switch reference | `Program.htm#cmd` | Complete |

## Removed or unsupported inherited behavior

| ID | Source lines | Content | Canonical destination | Status |
| --- | ---: | --- | --- | --- |
| REMOVE-01 | 794-795 | Non-functional `ListLines` | `lib/ListLines.htm` | Complete |
| REMOVE-02 | 796-798 | Unsupported `FormatTime` date flags | `lib/FormatTime.htm` | Complete |
| REMOVE-03 | 799 | Static-control `WM_CTLCOLORSTATIC` | `lib/GuiControls.htm#Text` | Complete |
| REMOVE-04 | 800 | Button double-click handlers | `lib/GuiOnEvent.htm` | Complete |
| REMOVE-05 | 801-803 | UpDown buddy and option limitations | `lib/GuiControls.htm#UpDown` | Complete |
| REMOVE-06 | 804 | Simplified `IL_Create` | `lib/IL_Create.htm` | Complete |
| REMOVE-07 | 805 | No `GDI+` option in `LoadPicture` | `lib/LoadPicture.htm` | Complete |
| REMOVE-08 | 806 | Slider event-info limitation | `lib/GuiOnEvent.htm` | Complete |
| REMOVE-09 | 807 | Ignored `PixelGetColor` mode | `lib/PixelGetColor.htm` | Complete |
| REMOVE-10 | 808-811 | `DirSelect` option and modality limits | `lib/DirSelect.htm` | Complete |
| REMOVE-11 | 812-816 | `MsgBox` modality/help limits | `lib/MsgBox.htm` | Complete |
| REMOVE-12 | 817 | Only Tab3 behavior | `lib/GuiControls.htm#Tab` | Complete |
| REMOVE-13 | 818 | Ignored ListView `Count` | `lib/GuiControls.htm#ListView` | Complete |
| REMOVE-14 | 819-820 | Reference operator returns `VarRef` | `Variables.htm#ref` | Complete |
| REMOVE-15 | 821-822 | `OnMessage` model and GUI requirement | `lib/OnMessage.htm` | Complete |
| REMOVE-16 | 823-824 | Pause unsupported | Current `Flow.Pause` implements pausing | Resolved |
| REMOVE-17 | 825-827 | Non-COM reference-count functions | `lib/ObjAddRef.htm` | Complete |
| REMOVE-18 | 828 | `#Warn` unsupported | Current lowerer implements `#Warn` analysis | Resolved |
| REMOVE-19 | 829 | Compiled-script `/script` unsupported | Current runner implements `--script` | Resolved |
| REMOVE-20 | 830 | Help and Window Spy menu items absent | Current standard menu implements both items; see `Program.htm#tray-icon` and `Program.htm#Window_Spy` | Resolved |
| REMOVE-21 | 831 | `Download` option limits | `lib/Download.htm` | Complete |
| REMOVE-22 | 832 | `Thread("Interrupt")` line-count limit | `lib/Thread.htm` | Complete |
| REMOVE-23 | 833 | Tooltips remain after clicking | `lib/ToolTip.htm` | Complete |

## Source resolutions

The migration source contains a few statements which were overtaken by runtime
changes or contradicted another section of the same file. These were checked
against the current source rather than copied into user-facing documentation:

- `ImageSearch` still parses `*` options from the start of `ImageFile`;
  there is no separate fifth options parameter in the current public method.
- Version requirements use the current semantic-version parser, including
  pre-release forms. `#Requires` therefore retains the inherited comparison
  syntax and adds the `Keysharp` and `capability` forms.
- `Pause`, `#Warn` and compiled-script `--script` are implemented.
- The current assembly-title directive is `#AssemblyTitle`; the older
  `#AssemblyName` note is retained only as a compatibility correction.
- Conditional compilation defines `OSX` on macOS in addition to `WINDOWS`,
  `LINUX` and `KEYSHARP`.
- The current API is named `ControlMove`; there is no separate
  `ControlSetPos` function to document.
- The standard tray menu now implements Help and Window Spy. Linux and macOS
  additionally expose their bundled accessibility inspectors.

## Completion audit

- [x] Every non-excluded row is marked Complete.
- [x] `docs/KeysharpDifferences.htm` links to every difference/removal family.
- [x] No KeysharpDocs page presents `capabilities.json` or
  `capabilities.md` as authoritative.
- [x] New pages appear in the TOC and keyword index.
- [x] New pages are included in `Project.hhp`.
- [x] Search data is regenerated.
- [x] `scripts/Test-Docs.ps1` passes.
- [x] The rendered site is reviewed in light, dark and narrow layouts.
