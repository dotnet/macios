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
  length of the test timeout. For a relaunched app (see below), this is
  `simulator-lifecycle-attempt2.log`.
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
- Its `captureTime` is no earlier than the start of the launch (with two
  seconds of tolerance), and no later than 60 seconds after it. On CI,
  this crash happened 17-24 seconds after mlaunch started. The header's
  `timestamp` isn't used, because it can be much later than the crash.
- The crashed thread has the frames listed above, in that order.
- The test run hasn't finished or timed out, and neither the test log nor
  the app's standard output has a `[FAIL]`, `Tests run:`, or XML result yet.
  This is checked again after collecting diagnostics and stopping the app.

xharness stops looking for crash reports two minutes after the launch (the
60-second window, plus up to 60 seconds for the report to be written; on CI,
reports were written up to 39 seconds after the crash). Diagnostic collection
and stopping the app can take longer.

In the wrapped XML mode used on CI, the app sends its results to the test
log only at the end of the run. The app's standard output shows each failed
test as it finishes, so a failure that was already printed stops the
relaunch. The first attempt may still have run (and passed) some tests, and a
test that was running when SpringBoard crashed may have been about to fail.
Those tests run again in the second attempt. A failure that happens every
time is reported by the second attempt, but an intermittent failure in the
first attempt can be hidden.

xharness then collects the simulator lifecycle log, stops the app, and
launches it once more on the same (still booted) simulator, with a new
listener and new logs. The first attempt's test log, standard output and
standard error stay in the test's report, with "(attempt 1, ended after a
SpringBoard crash)" added to their descriptions. Its lifecycle log and crash
reports are also kept. The relaunch is noted in the execution log and test
summary. The second attempt's result is reported as usual.

If the same crash happens in the second attempt, the test fails right away as
crashed, with a `springboard-crash` failure in the test results. Any other
failure, timeout, or crash isn't retried. A matching crash outside the
60-second window is only logged.
