SPAWNER
=======

This is a very simple tool, which executes another process, disclaiming any
responsibility for it.

This is important when executing tests apps, because when macOS sees that an
app uses API that needs specific entries in the Info.plist (such as the
`NSAppleMusicUsageDescription`), the responsible process is where macOS looks
for said key.

Example crash report:

```
Process:               introspection [85822]
Path:                  /Users/USER/*/introspection.app/Contents/MacOS/introspection
Identifier:            com.xamarin.introspection
Version:               1.0 (1.0)
Code Type:             ARM-64 (Native)
Parent Process:        dotnet [81129]
Responsible:           Electron [68966]
User ID:               501

Date/Time:             2025-11-13 17:33:10.8123 +0100
OS Version:            macOS 15.7.2 (24G325)
Report Version:        12
Anonymous UUID:        F22C0F06-0F16-E475-C0CB-264A0FF4F6A3


Time Awake Since Boot: 27000 seconds

System Integrity Protection: enabled

Crashed Thread:        16  Dispatch queue: com.apple.root.default-qos

Exception Type:        EXC_CRASH (SIGKILL)
Exception Codes:       0x0000000000000000, 0x0000000000000000

Termination Reason:    Namespace TCC, Code 0 
This app has crashed because it attempted to access privacy-sensitive data without a usage description. The app's Info.plist must contain an NSAppleMusicUsageDescription key with a string value explaining to the user how the app uses this data.
```

The app crashed because macOS says it needs the `NSAppleMusicUsageDescription` entry in its `Info.plist` file.

This is confusing, because introspection _has_ an `NSAppleMusicUsageDescription` entry in its `Info.plist` file.

Here's what happens:

Note that there's a "Responsible [Process]" (Electron 68966) line, which is not the same as "Process" (introspection 85822), and this is the crux of the matter.

In this particular case:

* I opened the xharness project in VSCode.
* I launched the xharness project in the debugger, and then ran introspection for Mac Catalyst.
* The responsible process ended up being VS Code (aka Electron, with pid 85822), and that's where macOS ended up looking for the `NSAppleMusicUsageDescription` key.

The fix is to launch `introspection` (and any other test app on macOS) using
this `spawner` tool, which disclaims reponsibility for anything it launches,
thus letting `introspection` be a grown up process and fully responsible for
itself.

Usage is simple: just pass the executable + any arguments to `spawner`.

References:

* https://gitlab.com/gnachman/iterm2/-/issues/10360
* https://github.com/llvm/llvm-project/commit/041c7b84a4b925476d1e21ed302786033bb6035f#diff-a38ae411ccf0c85f3d7c0c45d8e1ad035030d5171d59e478b86a094941d3209dR16-R17
* https://lldb.llvm.org/cpp_reference/PosixSpawnResponsible_8h_source.html
* https://steipete.me/posts/2025/applescript-cli-macos-complete-guide
