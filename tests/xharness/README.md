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

# Relaunching after a SpringBoard crash

Sometimes SpringBoard crashes in the simulator right after xharness launches
the test app. The app then stops making progress, and the test run eventually
times out. The crash is a `SIGABRT` from `NSAssertionHandler` in
`-[RBSAssertion dealloc]`, called while PosterUIFoundation (or PaperBoardUI)
takes a wallpaper snapshot.

While a simulator test app runs, xharness checks
`~/Library/Logs/DiagnosticReports` every five seconds for new SpringBoard
crash reports. A report triggers a relaunch only if all of these are true:

- Its `coalitionName` is `com.apple.CoreSimulator.SimDevice.<UDID>` for the
  simulator running the test.
- Its `captureTime` is no earlier than the launch (with two seconds of
  tolerance), and no later than two minutes after it. The header's
  `timestamp` isn't used, because it can be much later than the crash.
- The crashed thread has the frames listed above, in that order.
- The test run hasn't finished, and the test log has no `[FAIL]` or
  `Tests run:` lines yet. A relaunch never hides a test result.

xharness then collects the simulator lifecycle log, stops the app, and
launches it once more on the same (still booted) simulator, with a new
listener and new logs. The first attempt's logs and crash report stay in the
test's report, with "(attempt 1, ended after a SpringBoard crash)" added to
their descriptions, and the relaunch is noted in the execution log and in the
test summary. The second attempt's result is reported as usual.

If the same crash happens in the second attempt, the test fails right away as
crashed, with a `springboard-crash` failure in the test results. Any other
failure, timeout, or crash isn't retried. A matching crash outside the
two-minute window is only logged.
