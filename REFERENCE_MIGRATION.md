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
| SETUP-01 | 17-18 | .NET 10 prerequisite | `howto/Install.htm#Prerequisites` | Pending |
| SETUP-02 | 20-46 | Windows support, install, portable use and build/package steps | `Platforms.htm#Windows`, `howto/Install.htm#Windows` | Pending |
| SETUP-03 | 48-57 | Linux X11 and Wayland/compositor support | `Platforms.htm#Linux` | Pending |
| SETUP-04 | 59-77 | Linux install, helpers, extensions and editor shim | `howto/Install.htm#Linux` | Pending |
| SETUP-05 | 79-86 | Linux source and package build | `howto/Install.htm#Build_Linux` | Pending |
| SETUP-06 | 88-104 | macOS feature status and permissions | `Platforms.htm#macOS` | Pending |
| SETUP-07 | 106-153 | macOS requirements, DMG/PKG install, Gatekeeper, commands and editor shim | `howto/Install.htm#macOS` | Pending |
| SETUP-08 | 154-168 | macOS permission setup and capability request | `Platforms.htm#macOS_Permissions`, `howto/Install.htm#macOS_Permissions` | Pending |
| SETUP-09 | 169-189 | macOS uninstall and TCC reset | `howto/Install.htm#Uninstall_macOS` | Pending |
| SETUP-10 | 191-210 | macOS source/package build and signing | `howto/Install.htm#Build_macOS` | Pending |
| GUI-01 | 212-224 | macOS App/Edit menus and `AppMenu` option | `lib/Gui.htm#AppMenu` | Pending |
| MATRIX-01 | 226-255 | Unreviewed capability matrix | Explicitly excluded; do not migrate | Excluded |
| PROJECT-01 | 257-265 | History, platforms and alpha status | `Keysharp.htm#Status` | Pending |
| PROGRAM-01 | 267-282 | compilation pipeline, file types and Keyview | `Program.htm#execution-model`, `Program.htm#create` | Pending |
| ACK-01 | 835-849 | Code acknowledgements | `misc/Acknowledgements.htm#Keysharp` | Pending |
| SUPPORT-01 | 851-853 | Issue tracker and support scope | `Keysharp.htm#Links`, `howto/Install.htm#Problems` | Pending |

## Behavioral and syntax differences

| ID | Source lines | Content | Canonical destination | Status |
| --- | ---: | --- | --- | --- |
| DIFF-01 | 288-293 | Linux controls, AtSpi, GUI and Registry limits | `Platforms.htm#Linux`, control pages, Registry pages | Pending |
| DIFF-02 | 294-302 | .NET lifetimes, garbage collection and `__Delete` | `Objects.htm#Lifetime` | Pending |
| DIFF-03 | 303-305 | Increment/decrement of unset variables | `Variables.htm#IncDec` | Pending |
| DIFF-04 | 306-309 | `FuncObj`, `Func()` and built-in function objects | `lib/Func.htm` | Pending |
| DIFF-05 | 310 | Error stack trace origin | `lib/Error.htm` | Pending |
| DIFF-06 | 311-314, 349-351 | `StrPtr`, `StringBuffer` and `ObjFree` behavior | `lib/StrPtr.htm`, `lib/StringBuffer.htm`, `lib/ObjFree.htm` | Pending |
| DIFF-07 | 315-317 | `CallbackCreate` calling convention, pointers and cost | `lib/CallbackCreate.htm` | Pending |
| DIFF-08 | 318 | Deleting tab controls | `lib/GuiControl.htm#Delete` | Pending |
| DIFF-09 | 319-321 | GUI sizing, `xc`/`yc` and `UseGroup` | `lib/Gui.htm#Position`, `lib/Gui.htm#UseGroup` | Pending |
| DIFF-10 | 322-330 | WinForms `ClassNN`, `NetClassNN` and GUI lookup | `lib/GuiControl.htm#NetClassNN`, `misc/WinTitle.htm` | Pending |
| DIFF-11 | 331-334 | `TrayTip` option differences | `lib/TrayTip.htm` | Pending |
| DIFF-12 | 335-336, 352 | `Sleep`, timers and shutdown | `lib/Sleep.htm` | Pending |
| DIFF-13 | 337 | `#HotIf` optimization does not apply | `lib/_HotIf.htm` | Pending |
| DIFF-14 | 338-340 | `#ErrorStdOut` requires redirected output | `lib/_ErrorStdOut.htm` | Pending |
| DIFF-15 | 341-342 | Menus and threading; `AddStandard` lookup | `lib/Menu.htm` | Pending |
| DIFF-16 | 343 | Nested-control coordinates | `lib/ControlMove.htm`, `lib/ControlSetPos.htm` | Pending |
| DIFF-17 | 344 | Function-object reflection cost | `lib/Func.htm#Performance` | Pending |
| DIFF-18 | 345 | Internal `KeysharpFile` name | `lib/File.htm` | Pending |
| DIFF-19 | 346 | `SetTimer` priority range | `lib/SetTimer.htm` | Pending |
| DIFF-20 | 347 | Null `VT_DISPATCH` assignment | `lib/ComObject.htm` | Pending |
| DIFF-21 | 348 | `A_LineNumber` reliability | `Variables.htm#LineNumber` | Pending |
| DIFF-22 | 349-350 | `ObjPtr` and `ObjPtrAddRef` return types | `lib/ObjPtr.htm` | Pending |
| DIFF-23 | 353 | Concatenation assignment performance | `Variables.htm#AssignOp` | Pending |
| DIFF-24 | 354-355 | Debug switch and compiled-script arguments | `Program.htm#cmd` | Pending |
| SYNTAX-01 | 357-359 | `DllCall` double-pointer guidance | `lib/DllCall.htm` | Pending |
| SYNTAX-02 | 360 | `ImageSearch` options parameter | `lib/ImageSearch.htm#Syntax` | Pending |
| SYNTAX-03 | 361 | Leading plus on numeric literals | `Concepts.htm#numbers` | Pending |
| SYNTAX-04 | 362 | `unset` representation and `IsSet` | `Concepts.htm#unset` | Pending |
| SYNTAX-05 | 363 | Dynamic dereference performance | `Variables.htm#deref` | Pending |
| SYNTAX-06 | 364-365 | Compile-time-only `Goto` targets | `lib/Goto.htm` | Pending |
| SYNTAX-07 | 366-380 | `#Requires Keysharp` and capability form | `lib/_Requires.htm` | Pending |
| SYNTAX-08 | 381 | `__Enum` parameter count | `Objects.htm#__Enum` | Pending |
| SYNTAX-09 | 382-391 | PCRE2 options and callout differences | `misc/RegEx-QuickRef.htm`, `misc/RegExCallout.htm` | Pending |

## Keysharp-specific APIs and extensions

| ID | Source lines | Content | Canonical destination | Status |
| --- | ---: | --- | --- | --- |
| EXT-01 | 393-396 | KS module and import requirement | `Modules.htm#KS` | Pending |
| EXT-02 | 397-408 | `A_ThreadId` and targeted `Exit` | `lib/KeysharpVariables.htm#A_ThreadId`, `lib/Exit.htm` | Pending |
| EXT-03 | 409-432 | `StringBuffer` class | `lib/StringBuffer.htm` | Pending |
| EXT-04 | 434-437 | `Sinh`, `Cosh`, `Tanh` | `lib/Math.htm` | Pending |
| EXT-05 | 438 | `HashMap` | `lib/HashMap.htm` | Pending |
| EXT-06 | 439 | `RandomSeed` | `lib/Random.htm#RandomSeed` | Pending |
| EXT-07 | 440-442 | `FileDirName`, `FileFullPath` | `lib/FilePath.htm` | Pending |
| EXT-08 | 443-445 | `WinFromPoint`, `WinMaximizeAll` | dedicated function pages | Pending |
| EXT-09 | 446-447 | `ShowDebug`, `OutputDebugLine` | `lib/OutputDebug.htm` | Pending |
| EXT-10 | 448-455 | Base64, EOL, `Join`, string prefix/suffix APIs | `lib/StringFunctions.htm`, `lib/String.htm` | Pending |
| EXT-11 | 456-485 | `RegExMatchCs`, `RegExReplaceCs` | `lib/RegExCs.htm` | Pending |
| EXT-12 | 486-487 | `FormatCs` | `lib/FormatCs.htm` | Pending |
| EXT-13 | 488-502 | `RequestCapabilities` | `lib/RequestCapabilities.htm` | Pending |
| EXT-14 | 503 | `Image` class | `lib/Image.htm` | Pending |
| EXT-15 | 504 | `Overlay` class | `lib/Overlay.htm` | Pending |
| EXT-16 | 505 | GUI `AutoScroll` option | `lib/Gui.htm#AutoScroll` | Pending |
| EXT-17 | 506-510 | Clipboard image copy and empty test | `lib/ClipboardFunctions.htm` | Pending |
| EXT-18 | 511-514 | `Collect` | `lib/Collect.htm` | Pending |
| EXT-19 | 515-517 | `RunScript` and `ProcessInfo` | `lib/RunScript.htm` | Pending |
| EXT-20 | 518-550 | Keysharp built-in accessors | `lib/KeysharpVariables.htm` | Pending |
| EXT-21 | 551-555 | AES, hashes, CRC32 and secure random | `lib/Crypto.htm` | Pending |
| EXT-22 | 556-566 | `RealThread` | `lib/RealThread.htm` | Pending |
| EXT-23 | 567-598 | `WinEvent` | `lib/WinEvent.htm` | Pending |
| EXT-24 | 599-600 | `LockRun` | `lib/LockRun.htm` | Pending |
| EXT-25 | 601-612 | `Mail` | `lib/Mail.htm` | Pending |
| EXT-26 | 613-620 | Experimental `Clr` interop | `lib/Clr.htm` | Pending |
| EXT-27 | 621 | Map implementation and enumeration | `lib/Map.htm` | Pending |
| EXT-28 | 622 | Multiple spread arguments | `Functions.htm#Variadic` | Pending |
| EXT-29 | 623-624 | Buffer index and conversion methods | `lib/Buffer.htm` | Pending |
| EXT-30 | 625-640 | Array methods | `lib/Array.htm` | Pending |
| EXT-31 | 641-642 | Separate `Run`/`RunWait` arguments | `lib/Run.htm` | Pending |
| EXT-32 | 643 | GUI known colors | `lib/Gui.htm#Color` | Pending |
| EXT-33 | 644 | `ListView.DeleteCol` | `lib/ListView.htm` | Pending |
| EXT-34 | 645-649 | Menu visibility, color and metadata | `lib/Menu.htm` | Pending |
| EXT-35 | 650 | Clear a Picture control | `lib/GuiControl.htm#Value` | Pending |
| EXT-36 | 651-655 | UpDown `Increment` and `Hex` | `lib/GuiControls.htm#UpDown` | Pending |
| EXT-37 | 656 | `TabControl.SetTabIcon` | `lib/GuiControl.htm` | Pending |
| EXT-38 | 657 | `TreeView.GetNode` | `lib/TreeView.htm` | Pending |
| EXT-39 | 658 | Control autosizing argument | `lib/Gui.htm#Add` | Pending |
| EXT-40 | 659 | `Gui.Visible` | `lib/Gui.htm#Visible` | Pending |
| EXT-41 | 660 | Cross-platform `EnvUpdate` | `lib/EnvUpdate.htm` | Pending |
| EXT-42 | 661 | Unlimited hotstring abbreviation length | `Hotstrings.htm` | Pending |
| EXT-43 | 662 | `FileGetSize` G/T units | `lib/FileGetSize.htm` | Pending |
| EXT-44 | 663-664 | Millisecond date units | `lib/DateAdd.htm`, `lib/DateDiff.htm` | Pending |
| EXT-45 | 665 | Default `SubStr` start | `lib/SubStr.htm` | Pending |
| EXT-46 | 666 | `MaxIndex` and `MinIndex` | `lib/Array.htm`, `lib/Map.htm` | Pending |
| EXT-47 | 667-673 | RichEdit control | `lib/GuiControls.htm#RichEdit` | Pending |
| EXT-48 | 674-678 | Named icon resources in .NET DLLs | `lib/LoadPicture.htm`, `lib/TraySetIcon.htm`, `lib/Menu.htm` | Pending |
| EXT-49 | 679-681 | `WM_COPYDATA` string handling | `lib/SendMessage.htm` | Pending |
| EXT-50 | 682 | `A_ClipboardTimeout` | `lib/KeysharpVariables.htm#A_ClipboardTimeout` | Pending |
| EXT-51 | 683-684 | Reloading compiled scripts | `lib/Reload.htm` | Pending |
| EXT-52 | 685-686 | Signed wheel `A_EventInfo` | `Variables.htm#EventInfo` | Pending |
| EXT-53 | 687 | Custom-base `Log` | `lib/Math.htm#Log` | Pending |
| EXT-54 | 688-691 | Timer `A_EventInfo` and menu behavior | `lib/SetTimer.htm` | Pending |
| EXT-55 | 692-716 | Expanded reference-parameter targets | `Functions.htm#ByRef` | Pending |
| EXT-56 | 717 | `OwnPropCount` method | `lib/Object.htm` | Pending |
| EXT-57 | 718 | `ComObjConnect` debug parameter | `lib/ComObjConnect.htm` | Pending |
| EXT-58 | 719-757 | Conditional compilation syntax | `lib/Preprocessor.htm` | Pending |
| EXT-59 | 758-768 | Hook mutex and assembly directives | `lib/KeysharpDirectives.htm` | Pending |
| EXT-60 | 769-792 | Complete command-line switch reference | `Program.htm#cmd` | Pending |

## Removed or unsupported inherited behavior

| ID | Source lines | Content | Canonical destination | Status |
| --- | ---: | --- | --- | --- |
| REMOVE-01 | 794-795 | Non-functional `ListLines` | `lib/ListLines.htm` | Pending |
| REMOVE-02 | 796-798 | Unsupported `FormatTime` date flags | `lib/FormatTime.htm` | Pending |
| REMOVE-03 | 799 | Static-control `WM_CTLCOLORSTATIC` | `lib/GuiControls.htm#Text` | Pending |
| REMOVE-04 | 800 | Button double-click handlers | `lib/GuiOnEvent.htm` | Pending |
| REMOVE-05 | 801-803 | UpDown buddy and option limitations | `lib/GuiControls.htm#UpDown` | Pending |
| REMOVE-06 | 804 | Simplified `IL_Create` | `lib/IL_Create.htm` | Pending |
| REMOVE-07 | 805 | No `GDI+` option in `LoadPicture` | `lib/LoadPicture.htm` | Pending |
| REMOVE-08 | 806 | Slider event-info limitation | `lib/GuiOnEvent.htm` | Pending |
| REMOVE-09 | 807 | Ignored `PixelGetColor` mode | `lib/PixelGetColor.htm` | Pending |
| REMOVE-10 | 808-811 | `DirSelect` option and modality limits | `lib/DirSelect.htm` | Pending |
| REMOVE-11 | 812-816 | `MsgBox` modality/help limits | `lib/MsgBox.htm` | Pending |
| REMOVE-12 | 817 | Only Tab3 behavior | `lib/GuiControls.htm#Tab` | Pending |
| REMOVE-13 | 818 | Ignored ListView `Count` | `lib/GuiControls.htm#ListView` | Pending |
| REMOVE-14 | 819-820 | Reference operator returns `VarRef` | `Variables.htm#ref` | Pending |
| REMOVE-15 | 821-822 | `OnMessage` model and GUI requirement | `lib/OnMessage.htm` | Pending |
| REMOVE-16 | 823-824 | Pause unsupported | `lib/Pause.htm` | Pending |
| REMOVE-17 | 825-827 | Non-COM reference-count functions | `lib/ObjAddRef.htm` | Pending |
| REMOVE-18 | 828 | `#Warn` unsupported | `lib/_Warn.htm` | Pending |
| REMOVE-19 | 829 | Compiled-script `/script` unsupported | `Program.htm#cmd` | Pending |
| REMOVE-20 | 830 | Help and Window Spy menu items absent | `Program.htm#tray-icon` | Pending |
| REMOVE-21 | 831 | `Download` option limits | `lib/Download.htm` | Pending |
| REMOVE-22 | 832 | `Thread("Interrupt")` line-count limit | `lib/Thread.htm` | Pending |
| REMOVE-23 | 833 | Tooltips remain after clicking | `lib/ToolTip.htm` | Pending |

## Completion audit

- [ ] Every non-excluded row is marked Complete.
- [ ] `docs/KeysharpDifferences.htm` links to every difference/removal family.
- [ ] No KeysharpDocs page presents `capabilities.json` or
  `capabilities.md` as authoritative.
- [ ] New pages appear in the TOC and keyword index.
- [ ] New pages are included in `Project.hhp`.
- [ ] Search data is regenerated.
- [ ] `scripts/Test-Docs.ps1` passes.
- [ ] The rendered site is reviewed in light, dark and narrow layouts.
