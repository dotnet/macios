# Hung test diagnostics

The timeout covers the whole test run. It is not an inactivity timeout.
When a test app times out (iOS or tvOS simulator, macOS, or Mac Catalyst),
xharness collects diagnostics before stopping the app. The run still fails
as a timeout. Simulator launch timeouts also trigger collection before mlaunch
is cancelled.

The test's report includes:

- `hang-diagnostics-<pid>.log`: the app's PID, parent PID, process state,
  elapsed time, and diagnostic command results.
- `native-stacks-<pid>.txt`: a three-second macOS `sample` call graph.
- `managed-stacks-<pid>.txt`: `dotnet-stack report` output when the tool is on
  `PATH`. This requires a CoreCLR app with diagnostics enabled. Host apps
  (macOS and Mac Catalyst) use host diagnostic sockets. Simulator containers
  may use separate sockets that the host tool cannot find. NativeAOT cannot
  use this tool.
- `simulator-lifecycle.log`: SpringBoard messages from that simulator, and
  RunningBoard messages that mention the app's bundle identifier, for the
  length of the test timeout.
- New crash reports, including SpringBoard crash reports. The execution log
  lists any SpringBoard crash reports it found.

Each diagnostic command can run for up to 15 seconds. After the timeout,
xharness allows three more minutes for diagnostics and cleanup; the test itself
does not get more time. Missing tools, permission failures, and failed commands
are logged.

For simulator runs, xharness records the app's PID and start time when its
launch output arrives. For macOS and Mac Catalyst, it uses the process it
started. It checks the executable name and exact start time before collection,
then checks the same process again before sending signals. It does not search
for or kill processes by name. After collection,
it sends `SIGABRT`, then `SIGCONT` so a stopped app can handle the abort.
If the app does not exit within five seconds, it sends `SIGKILL`.

Crash reports depend on the OS and the app's signal handling. If the app does
not exit after `SIGABRT` (CoreCLR apps may not), xharness sends `SIGKILL`, which
does not produce a crash report. If the app exits, xharness polls for up to
30 seconds for a readable report with that PID before collecting crash reports.
This prevents an earlier, unrelated crash from ending the wait too soon.
Native samples do not guarantee managed method names. On physical devices,
xharness collects crash reports, but these tools cannot inspect the app process.

Run the focused parser and native-process tests with:

```sh
dotnet test tests/xharness.Tests/xharness.Tests.csproj
```

The native-process tests require macOS. They exercise a sleeping process,
a stopped process, and rejection of a mismatched executable or start time.
Their native stack reports are attached to the test results. This project is
currently run manually, not by the CI test pipeline.
