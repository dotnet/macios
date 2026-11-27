# Hung test diagnostics

xharness uses the existing total-run timeout, not an inactivity timeout.
When an iOS/tvOS simulator or macOS/Mac Catalyst app times out, it collects
diagnostics before terminating the test app. The run still fails as a timeout.

The test's report includes:

- `hang-diagnostics-<pid>.log`: the app's PID, parent PID, process state,
  elapsed time, and diagnostic command results.
- `native-stacks-<pid>.txt`: a three-second macOS `sample` call graph.
- `managed-stacks-<pid>.txt`: `dotnet-stack report` output when the tool is on
  `PATH`. This requires an app with CoreCLR diagnostics enabled. NativeAOT does
  not support that protocol.
- `simulator-lifecycle.log`: the simulator's SpringBoard log and RunningBoard
  messages mentioning the test's bundle identifier, covering the run timeout.
- Crash reports collected by the existing crash reporter, including new
  SpringBoard reports. The execution log also lists collected SpringBoard reports.

Each diagnostic command has a 15-second limit. The launcher has two extra
minutes for diagnostic collection and termination; this does not extend the
test's allowed running time. Missing tools, permission failures, and failed
commands are logged.

The app PID comes from its launch output. xharness checks its executable name
and start time before collection, then checks its identity again before sending
signals. It does not search for or kill processes by name. After collection,
it sends `SIGABRT`, then `SIGCONT` so a stopped app can handle the abort.
If the app does not exit within five seconds, it sends `SIGKILL`.

Crash reports depend on the OS and the app's signal handling. An app that ignores
the abort may require `SIGKILL`, which does not produce a crash report. Native
samples do not guarantee managed method names. Physical-device runs retain
their existing crash-report collection; these host-side stack tools cannot
inspect a process running on a physical device.

Run the focused parser and native-process tests with:

```sh
dotnet test tests/xharness.Tests/xharness.Tests.csproj
```

The native-process tests require macOS. They exercise a sleeping process,
a stopped process, and rejection of a mismatched executable. Their native
stack reports are attached to the test results.
