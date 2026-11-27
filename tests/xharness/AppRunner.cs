using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.DotNet.XHarness.Common;
using Microsoft.DotNet.XHarness.Common.CLI;
using Microsoft.DotNet.XHarness.Common.Execution;
using Microsoft.DotNet.XHarness.Common.Logging;
using Microsoft.DotNet.XHarness.Common.Utilities;
using Microsoft.DotNet.XHarness.iOS.Shared;
using Microsoft.DotNet.XHarness.iOS.Shared.Execution;
using Microsoft.DotNet.XHarness.iOS.Shared.Hardware;
using Microsoft.DotNet.XHarness.iOS.Shared.Listeners;
using Microsoft.DotNet.XHarness.iOS.Shared.Logging;
using Microsoft.DotNet.XHarness.iOS.Shared.Utilities;
using Microsoft.DotNet.XHarness.iOS.Shared.XmlResults;
using Xharness.Jenkins.TestTasks;

namespace Xharness {

	public class AppRunner {
		readonly IMlaunchProcessManager processManager;
		readonly ISimulatorLoaderFactory simulatorsLoaderFactory;
		readonly ISimpleListenerFactory listenerFactory;
		readonly IDeviceLoaderFactory devicesLoaderFactory;
		readonly CrashSnapshotReporterFactory snapshotReporterFactory;
		readonly ICaptureLogFactory captureLogFactory;
		readonly DeviceLogCapturerFactory deviceLogCapturerFactory;
		readonly ITestReporterFactory testReporterFactory;
		readonly IAppBundleInformationParser appBundleInformationParser;

		readonly RunMode runMode;
		readonly bool isSimulator;
		readonly TestTarget target;
		readonly Harness harness;
		readonly double timeoutMultiplier;
		readonly BuildToolTask? buildTask;
		readonly string? variation;
		readonly string projectFilePath;
		readonly string? buildConfiguration;

		string? deviceName;
		ISimulatorDevice? simulator;

		bool ensureCleanSimulatorState = true;
		bool EnsureCleanSimulatorState {
			get => ensureCleanSimulatorState && string.IsNullOrEmpty (Environment.GetEnvironmentVariable ("SKIP_SIMULATOR_SETUP"));
			set => ensureCleanSimulatorState = value;
		}

		AppBundleInformation? appInformation;
		public AppBundleInformation AppInformation {
			get => appInformation!;
		}

		bool IsExtension => AppInformation?.Extension.HasValue ?? false;

		public TestExecutingResult Result { get; private set; }

		public string? FailureMessage { get; private set; }

		public IFileBackedLog MainLog { get; set; }

		public ILogs Logs { get; }

		public AppRunner (IMlaunchProcessManager processManager,
						  IAppBundleInformationParser appBundleInformationParser,
						  ISimulatorLoaderFactory simulatorsFactory,
						  ISimpleListenerFactory simpleListenerFactory,
						  IDeviceLoaderFactory devicesFactory,
						  CrashSnapshotReporterFactory snapshotReporterFactory,
						  ICaptureLogFactory captureLogFactory,
						  DeviceLogCapturerFactory deviceLogCapturerFactory,
						  ITestReporterFactory reporterFactory,
						  TestTarget target,
						  Harness harness,
						  IFileBackedLog mainLog,
						  ILogs logs,
						  string projectFilePath,
						  string? buildConfiguration,
						  ISimulatorDevice? simulator = null,
						  string? deviceName = null,
						  bool ensureCleanSimulatorState = false,
						  double timeoutMultiplier = 1,
						  string? variation = null,
						  BuildToolTask? buildTask = null)
		{
			this.processManager = processManager ?? throw new ArgumentNullException (nameof (processManager));
			this.simulatorsLoaderFactory = simulatorsFactory ?? throw new ArgumentNullException (nameof (simulatorsFactory));
			this.listenerFactory = simpleListenerFactory ?? throw new ArgumentNullException (nameof (simpleListenerFactory));
			this.devicesLoaderFactory = devicesFactory ?? throw new ArgumentNullException (nameof (devicesFactory));
			this.snapshotReporterFactory = snapshotReporterFactory ?? throw new ArgumentNullException (nameof (snapshotReporterFactory));
			this.captureLogFactory = captureLogFactory ?? throw new ArgumentNullException (nameof (captureLogFactory));
			this.deviceLogCapturerFactory = deviceLogCapturerFactory ?? throw new ArgumentNullException (nameof (deviceLogCapturerFactory));
			this.testReporterFactory = reporterFactory ?? throw new ArgumentNullException (nameof (testReporterFactory));
			this.appBundleInformationParser = appBundleInformationParser ?? throw new ArgumentNullException (nameof (appBundleInformationParser));
			this.harness = harness ?? throw new ArgumentNullException (nameof (harness));
			this.MainLog = mainLog ?? throw new ArgumentNullException (nameof (mainLog));
			this.Logs = logs ?? throw new ArgumentNullException (nameof (logs));
			this.timeoutMultiplier = timeoutMultiplier;
			this.deviceName = deviceName;
			this.ensureCleanSimulatorState = ensureCleanSimulatorState;
			this.simulator = simulator;
			this.buildTask = buildTask;
			this.target = target;
			this.variation = variation;
			this.projectFilePath = projectFilePath;
			this.buildConfiguration = buildConfiguration;

			runMode = target.ToRunMode ();
			isSimulator = target.IsSimulator ();
		}

		public async Task InitializeAsync ()
		{
			appInformation = await appBundleInformationParser.ParseFromProject2 (harness.AppBundleLocator, projectFilePath, target, buildConfiguration!);
			AppInformation.Variation = variation;
		}

		async Task<bool> FindSimulatorAsync ()
		{
			if (simulator is not null)
				return true;

			var sims = simulatorsLoaderFactory.CreateLoader ();
			await sims.LoadDevices (Logs.Create ($"simulator-list-{Harness.Helpers.Timestamp}.log", "Simulator list"), false, false);
			(simulator, _) = await sims.FindSimulators (target.GetTargetOs (false), MainLog);

			return simulator is not null;
		}

		async Task FindDevice ()
		{
			if (deviceName is not null)
				return;

			deviceName = Environment.GetEnvironmentVariable ("DEVICE_NAME");
			if (!string.IsNullOrEmpty (deviceName))
				return;

			var devs = devicesLoaderFactory.CreateLoader ();
			await devs.LoadDevices (MainLog, false, false);

			var device = await devs.FindDevice (runMode, MainLog, false, false);

			deviceName = device?.Name;
		}

		public async Task<ProcessExecutionResult> InstallAsync (CancellationToken cancellation_token)
		{
			if (isSimulator) {
				// We reset the simulator when running, so a separate install step does not make much sense.
				throw new InvalidOperationException ("Installing to a simulator is not supported.");
			}

			await FindDevice ();

			if (string.IsNullOrEmpty (deviceName))
				throw new NoDeviceFoundException ();

			var args = new MlaunchArguments ();

			for (int i = -1; i < harness.Verbosity; i++)
				args.Add (new VerbosityArgument ());

			args.Add (new InstallAppOnDeviceArgument (AppInformation.AppPath));
			args.Add (new DeviceNameArgument (deviceName));

			var totalSize = Directory.GetFiles (AppInformation.AppPath, "*", SearchOption.AllDirectories).Select ((v) => new FileInfo (v).Length).Sum ();
			MainLog.WriteLine ($"Installing '{AppInformation.AppPath}' to '{deviceName}'. Size: {totalSize} bytes = {totalSize / 1024.0 / 1024.0:N2} MB");

			return await processManager.ExecuteCommandAsync (args, MainLog, TimeSpan.FromHours (1), cancellationToken: cancellation_token);
		}

		public async Task<ProcessExecutionResult> UninstallAsync ()
		{
			if (isSimulator)
				throw new InvalidOperationException ("Uninstalling from a simulator is not supported.");

			await FindDevice ();

			var args = new MlaunchArguments ();

			for (int i = -1; i < harness.Verbosity; i++)
				args.Add (new VerbosityArgument ());

			args.Add (new UninstallAppFromDeviceArgument (AppInformation.BundleIdentifier));
			args.Add (new DeviceNameArgument (deviceName));

			return await processManager.ExecuteCommandAsync (args, MainLog, TimeSpan.FromMinutes (1));
		}

		public async Task<int> RunAsync ()
		{
			if (!isSimulator) {
				await FindDevice ();

				if (deviceName is null)
					throw new NoDeviceFoundException ();
			}

			var args = new MlaunchArguments ();

			for (int i = -1; i < harness.Verbosity; i++)
				args.Add (new VerbosityArgument ());

			args.Add (new SetAppArgumentArgument ("-connection-mode"));
			args.Add (new SetAppArgumentArgument ("none")); // This will prevent the app from trying to connect to any IDEs
			args.Add (new SetAppArgumentArgument ("-autostart", true));
			args.Add (new SetEnvVariableArgument ("NUNIT_AUTOSTART", true));
			args.Add (new SetAppArgumentArgument ("-autoexit", true));
			args.Add (new SetEnvVariableArgument ("NUNIT_AUTOEXIT", true));
			args.Add (new SetAppArgumentArgument ("-enablenetwork", true));
			args.Add (new SetEnvVariableArgument ("NUNIT_ENABLE_NETWORK", true));
			// detect if we are using a jenkins bot.
			var useXmlOutput = harness.InCI;
			if (useXmlOutput) {
				args.Add (new SetEnvVariableArgument ("NUNIT_ENABLE_XML_OUTPUT", true));
				args.Add (new SetEnvVariableArgument ("NUNIT_ENABLE_XML_MODE", "wrapped"));
				args.Add (new SetEnvVariableArgument ("NUNIT_XML_VERSION", "nunitv3"));
			}

			if (harness.InCI) {
				// We use the 'BUILD_REVISION' variable to detect whether we're running CI or not.
				args.Add (new SetEnvVariableArgument ("BUILD_REVISION", Environment.GetEnvironmentVariable ("BUILD_REVISION")));

				// Forward VM_VENDOR (set by the pipeline when running on a VM-backed
				// pool such as ACES) so TestRuntime.AssertNotVirtualMachine works
				// inside the iOS/tvOS simulator process.
				var vmVendor = Environment.GetEnvironmentVariable ("VM_VENDOR");
				if (!string.IsNullOrEmpty (vmVendor))
					args.Add (new SetEnvVariableArgument ("VM_VENDOR", vmVendor));
			}

			if (!harness.GetIncludeSystemPermissionTests (TestPlatform.iOS, !isSimulator))
				args.Add (new SetEnvVariableArgument ("DISABLE_SYSTEM_PERMISSION_TESTS", 1));

			if (isSimulator) {
				args.Add (new SetAppArgumentArgument ("-hostname:127.0.0.1", true));
				args.Add (new SetEnvVariableArgument ("NUNIT_HOSTNAME", "127.0.0.1"));
			} else if (!listenerFactory.UseTunnel) {  // if is not simulator AND we are not using the tunnel, if we use the tunnel this is not needed
				var ips = new StringBuilder ();
				var ipAddresses = System.Net.Dns.GetHostEntry (System.Net.Dns.GetHostName ()).AddressList;
				for (int i = 0; i < ipAddresses.Length; i++) {
					if (i > 0)
						ips.Append (',');
					ips.Append (ipAddresses [i].ToString ());
				}

				var ipArg = ips.ToString ();
				args.Add (new SetAppArgumentArgument ($"-hostname:{ipArg}", true));
				args.Add (new SetEnvVariableArgument ("NUNIT_HOSTNAME", ipArg));
			}

			if (isSimulator && !await FindSimulatorAsync ())
				return 1;

			var testReporterTimeout = TimeSpan.FromMinutes (harness.Timeout * timeoutMultiplier);
			var attempt = CreateLaunchAttempt (args, 1, useXmlOutput, testReporterTimeout);
			var transport = attempt.Transport;
			var listener = attempt.Listener;
			var crashReporter = attempt.CrashReporter;
			var testReporter = attempt.TestReporter;
			string? springBoardFailure = null;

			if (isSimulator) {
				var simulators = new [] { simulator! };
				var systemLogs = new List<ICaptureLog> ();
				foreach (var sim in simulators) {
					// Upload the system log
					MainLog.WriteLine ("System log for the '{1}' simulator is: {0}", sim.SystemLog, sim.Name);
					var logDescription = LogType.SystemLog.ToString ();
					var log = captureLogFactory.Create (
						Path.Combine (Logs.Directory, sim.Name + ".log"),
						sim.SystemLog,
						harness.Action != HarnessAction.Jenkins,
						logDescription);

					log.StartCapture ();
					Logs.Add (log);
					systemLogs.Add (log);
					WrenchLog.WriteLine ("AddFile: {0}", log.FullPath);
				}

				MainLog.WriteLine ("*** Executing {0}/{1} in the simulator ***", AppInformation.AppName, runMode);

				if (EnsureCleanSimulatorState) {
					foreach (var sim in simulators) {
						using var tcclog = Logs.Create ($"prepare-simulator-{Harness.Helpers.Timestamp}.log", "Simulator preparation");
						var rv = await sim.PrepareSimulator (tcclog, AppInformation.BundleIdentifier);
						tcclog.Description += rv ? " ✅ " : " (failed) ⚠️";
					}
				}

				MainLog.WriteLine ("Enabling verbose logging");
				foreach (var sim in simulators) {
					var udid = sim.UDID;
					await sim.Boot (MainLog, new CancellationToken ());
					await processManager.ExecuteXcodeCommandAsync ("simctl", new string [] { "logverbose", udid, "enable" }, MainLog, TimeSpan.FromMinutes (5));
					await sim.Shutdown (MainLog);
				}
				MainLog.WriteLine ("Enabled verbose logging");

				MainLog.WriteLine ("Starting test run");

				// If SpringBoard crashes right after the launch, launch the app again (once) on the same simulator,
				// with a new listener, crash reporter and test reporter.
				var (finalAttempt, decision, attempts) = await SimulatorAttemptController.RunAsync (
					async (attemptNumber) => {
						var current = attemptNumber == 1 ? attempt : CreateLaunchAttempt (args, attemptNumber, useXmlOutput, testReporterTimeout);
						return (current, await RunSimulatorAttemptAsync (current, testReporterTimeout));
					},
					DiscardSimulatorAttemptAsync,
					MainLog);
				attempt = finalAttempt;
				listener = attempt.Listener;
				crashReporter = attempt.CrashReporter;
				testReporter = attempt.TestReporter;
				if (decision == SimulatorAttemptDecision.RepeatedSpringBoardCrash)
					springBoardFailure = $"SpringBoard crashed (wallpaper snapshot) in the simulator {simulator!.UDID} right after launching the test app, in {attempts} consecutive attempts.";

				// cleanup after us
				if (EnsureCleanSimulatorState)
					await simulator!.KillEverything (MainLog);

				foreach (var log in systemLogs)
					log.StopCapture ();

				MainLog.WriteLine ("Disabling verbose logging");
				foreach (var sim in simulators) {
					var udid = sim.UDID;
					await processManager.ExecuteXcodeCommandAsync ("simctl", new string [] { "logverbose", udid, "disable" }, MainLog, TimeSpan.FromMinutes (5));
				}
				MainLog.WriteLine ("Disabledverbose logging");
			} else {
				MainLog.WriteLine ("*** Executing {0}/{1} on device '{2}' ***", AppInformation.AppName, runMode, deviceName);

				args = attempt.Arguments;
				args.Add (new WaitForExitArgument ());
				args.Add (new DeviceNameArgument (deviceName));

				var deviceSystemLog = Logs.Create ($"device-{deviceName}-{Harness.Helpers.Timestamp}.log", "Device log");
				var deviceLogCapturer = deviceLogCapturerFactory.Create (harness.HarnessLog!, deviceSystemLog, deviceName);
				deviceLogCapturer.StartCapture ();

				try {
					await crashReporter.StartCaptureAsync ();

					MainLog.WriteLine ("Starting test run");

					if (transport == ListenerTransport.Tcp && listenerFactory.UseTunnel && listener is SimpleTcpListener tcpListener) {
						// create a new tunnel using the listener
						var tunnel = listenerFactory.TunnelBore.Create (deviceName, MainLog);
						tunnel.Open (deviceName, tcpListener, testReporterTimeout, MainLog);
						// wait until we started the tunnel
						await tunnel.Started;
					}

					// We need to check for MT1111 (which means that mlaunch won't wait for the app to exit).
					var aggregatedLog = Log.CreateAggregatedLog (testReporter.CallbackLog, MainLog);
					ProcessExecutionResult runTestResults = await processManager.ExecuteCommandAsync (
						args,
						aggregatedLog,
						testReporterTimeout,
						cancellationToken: testReporter.CancellationToken);

					await testReporter.CollectDeviceResult (runTestResults);
				} finally {
					deviceLogCapturer.StopCapture ();
					deviceSystemLog.Dispose ();
				}

				// Upload the system log
				if (File.Exists (deviceSystemLog.FullPath)) {
					MainLog.WriteLine ("A capture of the device log is: {0}", deviceSystemLog.FullPath);
					WrenchLog.WriteLine ("AddFile: {0}", deviceSystemLog.FullPath);
				}
			}

			if (!attempt.ListenerStopped)
				StopListener (listener);

			// close a tunnel if it was created
			if (!isSimulator && listenerFactory.UseTunnel)
				await listenerFactory.TunnelBore.Close (deviceName);

			if ((attempt.StopFailure ?? springBoardFailure) is string failureMessage) {
				await crashReporter.EndCaptureAsync (TimeSpan.FromSeconds (5));
				Result = attempt.StopFailure is not null ? TestExecutingResult.HarnessException : TestExecutingResult.Crashed;
				FailureMessage = failureMessage;
				MainLog.WriteLine (failureMessage);
				WrenchLog.WriteLine ($"AddSummary: <b><i>{runMode}: {System.Net.WebUtility.HtmlEncode (failureMessage)}</i></b><br/>");
				if (harness.XmlJargon != XmlResultJargon.Missing) {
					new XmlResultParser ().GenerateFailure (
						Logs,
						attempt.StopFailure is not null ? "simulator-stop-failure" : "springboard-crash",
						AppInformation.AppName,
						AppInformation.Variation,
						$"{(attempt.StopFailure is not null ? "Simulator stop failure" : "SpringBoard crash")} {AppInformation.AppName} {AppInformation.Variation} on {simulator!.UDID}",
						failureMessage,
						MainLog.FullPath,
						harness.XmlJargon);
				}
				return 1;
			}

			// check the final status, copy all the required data
			(Result, FailureMessage) = await testReporter.ParseResult ();

			if (attempt.FailureWhileStopping is string stoppedFailure && Result == TestExecutingResult.Crashed) {
				Result = TestExecutingResult.Failed;
				FailureMessage = $"A test failed before the app was stopped: {stoppedFailure}";
				MainLog.WriteLine (FailureMessage);
				WrenchLog.WriteLine ($"AddSummary: <b><i>{runMode}: A test failed before the app was stopped; the test run did not finish.</i></b><br/>");
				if (harness.XmlJargon != XmlResultJargon.Missing) {
					new XmlResultParser ().GenerateFailure (
						Logs,
						"interrupted-test-failure",
						AppInformation.AppName,
						AppInformation.Variation,
						$"Interrupted test failure {AppInformation.AppName} {AppInformation.Variation}",
						FailureMessage,
						MainLog.FullPath,
						harness.XmlJargon);
				}
				return 1;
			}

			// If the result is "Crashed" but the log shows a launch failure, report it as such
			if (Result == TestExecutingResult.Crashed && IsLaunchFailure (MainLog)) {
				Result = TestExecutingResult.LaunchFailure;
				FailureMessage = "Test app failed to launch.";
			}

			return testReporter?.Success == true ? 0 : 1;
		}

		class LaunchAttempt {
			public required int Number { get; init; }
			public required string Suffix { get; init; }
			public required MlaunchArguments Arguments { get; init; }
			public required ListenerTransport Transport { get; init; }
			public required ISimpleListener Listener { get; init; }
			public required IFileBackedLog TestLog { get; init; }
			public required string? [] ProgressLogs { get; init; }
			public required ICrashSnapshotReporter CrashReporter { get; init; }
			public required ITestReporter TestReporter { get; init; }
			public required CancellationTokenSource LaunchCallbackCancellation { get; init; }
			public required List<string> AttemptLogs { get; init; }
			public bool ListenerStopped { get; set; }
			public string? FailureWhileStopping { get; set; }
			public string? StopFailure { get; set; }
		}

		LaunchAttempt CreateLaunchAttempt (MlaunchArguments baseArgs, int attemptNumber, bool useXmlOutput, TimeSpan testReporterTimeout)
		{
			var suffix = attemptNumber == 1 ? "" : $"-attempt{attemptNumber}";
			var args = new MlaunchArguments ();
			args.AddRange (baseArgs);
			var attemptLogs = new List<string> ();

			var listener_log = Logs.Create ($"test-{runMode.ToString ().ToLowerInvariant ()}-{Harness.Helpers.Timestamp}{suffix}.log", LogType.TestLog.ToString (), timestamp: !useXmlOutput);
			attemptLogs.Add (listener_log.FullPath);
			var (transport, listener, listenerTmpFile) = listenerFactory.Create (runMode, MainLog, listener_log, isSimulator, true, useXmlOutput);

			var listenerPort = listener.InitializeAndGetPort ();

			args.Add (new SetAppArgumentArgument ($"-transport:{transport}", true));
			args.Add (new SetEnvVariableArgument ("NUNIT_TRANSPORT", transport.ToString ().ToUpper ()));

			if (transport == ListenerTransport.File)
				args.Add (new SetEnvVariableArgument ("NUNIT_LOG_FILE", listenerTmpFile));

			args.Add (new SetAppArgumentArgument ($"-hostport:{listenerPort}", true));
			args.Add (new SetEnvVariableArgument ("NUNIT_HOSTPORT", listenerPort));

			if (listenerFactory.UseTunnel)
				args.Add (new SetEnvVariableArgument ("USE_TCP_TUNNEL", true));

			listener.StartAsync ();

			// object that will take care of capturing and parsing the results
			ICrashSnapshotReporter crashReporter = snapshotReporterFactory.Create (MainLog, Logs, isDevice: !isSimulator, deviceName, AppInformation);

			var testReporter = testReporterFactory.Create (MainLog,
				MainLog,
				Logs,
				crashReporter,
				listener,
				new XmlResultParser (),
				AppInformation,
				runMode,
				harness.XmlJargon,
				isSimulator ? simulator!.UDID : deviceName,
				testReporterTimeout,
				buildTask?.Logs?.Directory);

			// Cancelled if the attempt is discarded, so that a discarded attempt can't report a launch failure.
			var launchCallbackCancellation = new CancellationTokenSource ();
			listener.ConnectedTask
				.TimeoutAfter (TimeSpan.FromMinutes (harness.LaunchTimeout))
				.ContinueWith (testReporter.LaunchCallback, launchCallbackCancellation.Token)
				.DoNotAwait ();

			args.AddRange (harness.EnvironmentVariables.Select (kvp => new SetEnvVariableArgument (kvp.Key, kvp.Value)));

			if (IsExtension) {
				throw new NotImplementedException ("Extensions aren't supported in xharness at the moment (no .NET extension tests yet)");
			} else {
				args.Add (isSimulator
					? (MlaunchArgument) new LaunchSimulatorAppArgument (AppInformation.LaunchAppPath)
					: new LaunchDeviceArgument (AppInformation.LaunchAppPath));
			}
			if (!isSimulator)
				args.Add (new DisableMemoryLimitsArgument ());

			string? stdout_log = null;
			string? stderr_log = null;
			if (isSimulator) {
				stdout_log = Logs.CreateFile ($"stdout-{Harness.Helpers.Timestamp}{suffix}.log", "Standard output");
				stderr_log = Logs.CreateFile ($"stderr-{Harness.Helpers.Timestamp}{suffix}.log", "Standard error");
				attemptLogs.Add (stdout_log);
				attemptLogs.Add (stderr_log);
				args.Add (new SetStdoutArgument (stdout_log));
				args.Add (new SetStderrArgument (stderr_log));
				args.Add (new SimulatorUDIDArgument (simulator!.UDID));
			}

			return new LaunchAttempt {
				Number = attemptNumber,
				Suffix = suffix,
				Arguments = args,
				Transport = transport,
				Listener = listener,
				TestLog = listener_log,
				// The test runner's console output goes through NSLog, which writes to stderr.
				ProgressLogs = new [] { listener_log.FullPath, stdout_log, stderr_log },
				CrashReporter = crashReporter,
				TestReporter = testReporter,
				LaunchCallbackCancellation = launchCallbackCancellation,
				AttemptLogs = attemptLogs,
			};
		}

		// Returns true if the attempt was ended early because SpringBoard crashed right after the launch.
		async Task<bool> RunSimulatorAttemptAsync (LaunchAttempt attempt, TimeSpan testReporterTimeout)
		{
			var testReporter = attempt.TestReporter;
			var udid = simulator!.UDID;

			await attempt.CrashReporter.StartCaptureAsync ();

			using var launchProcess = new Process ();
			using var runCancellation = new CancellationTokenSource ();
			using var timeoutCancellation = CancellationTokenSource.CreateLinkedTokenSource (testReporter.CancellationToken);
			using var detectorCancellation = new CancellationTokenSource ();
			var attemptStart = DateTimeOffset.Now;
			var detector = new SpringBoardCrashDetector (SpringBoardCrashDetector.DefaultDirectory, udid, attemptStart, MainLog);
			var launchedPid = 0;
			var pidLog = new CallbackLog (line => {
				if (ProcessDiagnostics.TryParsePid (line, AppInformation.BundleIdentifier, out var pid))
					Interlocked.Exchange (ref launchedPid, pid);
			});
			var launchLog = Log.CreateAggregatedLog (testReporter.CallbackLog, MainLog, pidLog);
			var execution = processManager.RunAsync (launchProcess, attempt.Arguments, launchLog, testReporterTimeout + TimeSpan.FromMinutes (3), cancellationToken: runCancellation.Token, diagnostics: false);
			var timeoutTask = Task.Delay (testReporterTimeout, timeoutCancellation.Token);
			var crashTask = detector.WaitForCrashAsync (detectorCancellation.Token);

			// Never end an attempt that has finished, or that has shown a failed test or the final summary.
			string? GetTestProgress ()
			{
				if (attempt.Listener.CompletionTask.IsCompleted)
					return "completed";
				if (execution.IsCompleted || timeoutTask.IsCompleted)
					return "finished or timed out";
				if (SimulatorAttemptController.HasTestResults (MainLog, out _, attempt.ProgressLogs))
					return "reported results";
				return null;
			}

			var completed = await Task.WhenAny (execution, timeoutTask, crashTask);
			if (completed == crashTask) {
				if (crashTask.Result is string crashReport) {
					var progress = await SimulatorAttemptController.GetProgressAfterDiagnosticsAsync (GetTestProgress, async () => {
						MainLog.WriteLine ($"SpringBoard crashed in the simulator {udid} right after launching the test app (crash report: {crashReport}).");
						try {
							await ProcessDiagnostics.CollectSimulatorLifecycleAsync (processManager, Logs, MainLog, udid, AppInformation.BundleIdentifier, DateTimeOffset.Now - attemptStart + TimeSpan.FromMinutes (1),
								$"simulator-lifecycle-springboard{attempt.Suffix}.log", $"Simulator SpringBoard and test app lifecycle (attempt {attempt.Number}, SpringBoard crash)");
						} catch (Exception e) {
							MainLog.WriteLine ($"Failed to collect the simulator lifecycle log: {e.Message}");
						}
					});
					if (progress is null) {
						MainLog.WriteLine ("Ending this attempt.");
						timeoutCancellation.Cancel ();
						runCancellation.Cancel ();
						ProcessExecutionResult stoppedResult;
						try {
							stoppedResult = await execution;
						} catch (Exception e) {
							MainLog.WriteLine ($"Failed to stop the test app: {e}");
							attempt.StopFailure = $"The simulator launch process failed while stopping the test app: {e.Message}";
							stoppedResult = new ProcessExecutionResult { ExitCode = 1 };
						}
						// Make sure the app isn't still running when it's launched again.
						try {
							await TerminateSimulatorAppAsync (Volatile.Read (ref launchedPid) > 0);
						} catch (Exception e) {
							MainLog.WriteLine ($"Failed to terminate the simulator test app: {e}");
							attempt.StopFailure ??= $"Could not stop the simulator test app after a SpringBoard crash: {e.Message}";
						}
						StopListener (attempt.Listener);
						attempt.ListenerStopped = true;
						var hasResults = SimulatorAttemptController.HasTestResults (MainLog, out var failure, attempt.ProgressLogs);
						if (attempt.StopFailure is not null) {
							if (failure is not null)
								attempt.StopFailure += $"; a test failure was also reported: {failure}";
							attempt.LaunchCallbackCancellation.Cancel ();
							return false;
						}
						if (hasResults) {
							attempt.FailureWhileStopping = failure;
							MainLog.WriteLine ("Test results arrived while stopping the app; reporting this attempt instead of relaunching.");
							await testReporter.CollectSimulatorResult (stoppedResult);
							return false;
						}
						return true;
					}
					MainLog.WriteLine ($"SpringBoard crashed in the simulator {udid} right after launching the test app (crash report: {crashReport}), but the test run has already {progress}. Waiting for the test run to finish.");
				}
				await Task.WhenAny (execution, timeoutTask);
			}
			detectorCancellation.Cancel ();
			await crashTask;

			var timedOut = !execution.IsCompleted;
			timeoutCancellation.Cancel ();
			if (timedOut) {
				try {
					await testReporter.CollectSimulatorResult (new ProcessExecutionResult { TimedOut = true });
				} finally {
					// Collect the app's stacks before cancelling mlaunch, which may also terminate the app.
					runCancellation.Cancel ();
				}
				await execution;
			} else {
				await testReporter.CollectSimulatorResult (await execution);
			}
			return false;
		}

		async Task TerminateSimulatorAppAsync (bool launchReported)
		{
			using var output = new MemoryLog () { Timestamp = false };
			var log = Log.CreateAggregatedLog (MainLog, output);
			var result = await processManager.ExecuteXcodeCommandAsync ("simctl", new string [] { "terminate", simulator!.UDID, AppInformation.BundleIdentifier }, log, TimeSpan.FromMinutes (1));
			var text = output.ToString ();
			// simctl reports ESRCH when the app has already exited.
			var alreadyStopped = result.ExitCode == 3 && (text.Contains ("found nothing to terminate", StringComparison.Ordinal) || text.Contains ("the app is not currently running", StringComparison.Ordinal));
			if (!result.TimedOut && (result.Succeeded || (alreadyStopped && launchReported)))
				return;
			throw new InvalidOperationException ($"simctl terminate failed or the app launch was not confirmed (exit code {result.ExitCode}, timed out: {result.TimedOut}): {text}");
		}

		async Task DiscardSimulatorAttemptAsync (int attemptNumber, LaunchAttempt attempt)
		{
			attempt.LaunchCallbackCancellation.Cancel ();
			if (!attempt.ListenerStopped)
				StopListener (attempt.Listener);
			await attempt.CrashReporter.EndCaptureAsync (TimeSpan.FromSeconds (5));

			// Keep the logs, but relabel them so that they're not parsed for test results.
			foreach (var log in Logs.Where (v => attempt.AttemptLogs.Contains (v.FullPath)))
				log.Description += $" (attempt {attemptNumber}, ended after a SpringBoard crash)";

			WrenchLog.WriteLine ($"AddSummary: <b><i>{runMode}: SpringBoard crashed in the simulator {simulator!.UDID} right after launching the test app; launched the app again.</i></b><br/>");
		}

		void StopListener (ISimpleListener listener)
		{
			if (!listener.StopAsync ().Wait (TimeSpan.FromSeconds (5))) {
				MainLog.WriteLine ("Failed to stop listener within 5 seconds. Will cancel it.");
				listener.Cancel ();
			}
			listener.Dispose ();
		}

		static bool IsLaunchFailure (IFileBackedLog log)
		{
			try {
				using var reader = log.GetReader ();
				string? line;
				while ((line = reader.ReadLine ()) is not null) {
					if (line.Contains ("Could not launch the app", StringComparison.Ordinal))
						return true;
					if (line.Contains ("error HE0042", StringComparison.Ordinal))
						return true;
				}
			} catch {
				// Ignore any errors reading the log
			}
			return false;
		}
	}
}
