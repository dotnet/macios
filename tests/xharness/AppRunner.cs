using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Xharness.Hardware;
using Xharness.Execution;
using Xharness.Jenkins.TestTasks;
using Xharness.Listeners;
using Xharness.Logging;
using Xharness.Utilities;

namespace Xharness {

	public enum RunMode {
		Sim32,
		Sim64,
		Classic,
		iOS,
		TvOS,
		WatchOS,
	}

	public enum Extension
	{
		WatchKit2,
		TodayExtension,
	}

	public class AppInformation {
		public string AppName { get; }
		public string BundleIdentifier { get; }
		public string AppPath { get; }
		public string LaunchAppPath { get; }
		public Extension? Extension { get; }

		public AppInformation (string appName, string bundleIdentifier, string appPath, string launchAppPath, Extension? extension)
		{
			AppName = appName;
			BundleIdentifier = bundleIdentifier;
			AppPath = appPath;
			LaunchAppPath = launchAppPath;
			Extension = extension;
		}
	}

	class AppRunner
	{
		readonly IProcessManager processManager;
		readonly ISimulatorsLoaderFactory simulatorsLoaderFactory;
		readonly ISimpleListenerFactory listenerFactory;
		readonly IDeviceLoaderFactory devicesLoaderFactory;
		
		readonly RunMode mode;
		readonly bool isSimulator;
		readonly AppRunnerTarget target;
		readonly string projectFilePath;
		readonly IHarness harness;
		readonly string configuration;
		readonly string variation;
		readonly double timeoutMultiplier;
		readonly BuildToolTask buildTask;
		readonly string logDirectory;

		string deviceName;
		string companionDeviceName;
		ISimulatorDevice [] simulators;
		ISimulatorDevice simulator => simulators [0];

		bool ensureCleanSimulatorState = true;
		bool EnsureCleanSimulatorState {
			get => ensureCleanSimulatorState && string.IsNullOrEmpty (Environment.GetEnvironmentVariable ("SKIP_SIMULATOR_SETUP"));
			set => ensureCleanSimulatorState = value;
		}

		bool IsExtension => AppInformation.Extension.HasValue;
		
		public AppInformation AppInformation { get; }

		public TestExecutingResult Result { get; private set; }

		public string FailureMessage { get; private set; }

		public ILog MainLog { get; set; }	

		public ILogs Logs { get; }
		

		public AppRunner (IProcessManager processManager,
						  ISimulatorsLoaderFactory simulatorsFactory,
						  ISimpleListenerFactory simpleListenerFactory,
						  IDeviceLoaderFactory devicesFactory,
						  AppRunnerTarget target,
						  IHarness harness,
						  ILog mainLog,
						  string projectFilePath,
						  string configuration,
						  string logDirectory = null,
						  ISimulatorDevice [] simulators = null,
						  string deviceName = null,
						  string companionDeviceName = null,
						  bool ensureCleanSimulatorState = false,
						  double timeoutMultiplier = 1,
						  string variation = null,
						  BuildToolTask buildTask = null)
		{
			this.processManager = processManager ?? throw new ArgumentNullException (nameof (processManager));
			this.simulatorsLoaderFactory = simulatorsFactory ?? throw new ArgumentNullException (nameof (simulatorsFactory));
			this.listenerFactory = simpleListenerFactory ?? throw new ArgumentNullException (nameof (simpleListenerFactory));
			this.devicesLoaderFactory = devicesFactory ?? throw new ArgumentNullException (nameof (devicesFactory));
			this.harness = harness ?? throw new ArgumentNullException (nameof (harness));
			this.MainLog = mainLog ?? throw new ArgumentNullException (nameof (mainLog));
			this.projectFilePath = projectFilePath ?? throw new ArgumentNullException (nameof (projectFilePath));
			this.logDirectory = logDirectory ?? harness.LogDirectory;
			this.Logs = new Logs (this.logDirectory);
			this.configuration = configuration;
			this.timeoutMultiplier = timeoutMultiplier;
			this.deviceName = deviceName;
			this.companionDeviceName = companionDeviceName;
			this.ensureCleanSimulatorState = ensureCleanSimulatorState;
			this.simulators = simulators;
			this.variation = variation;
			this.buildTask = buildTask;
			this.target = target;

			mode = target.ToRunMode ();
			isSimulator = target.IsSimulator ();
			AppInformation = Initialize ();
		}

		AppInformation Initialize ()
		{
			var csproj = new XmlDocument ();
			csproj.LoadWithoutNetworkAccess (projectFilePath);
			string appName = csproj.GetAssemblyName ();
			string info_plist_path = csproj.GetInfoPListInclude ();

			var info_plist = new XmlDocument ();
			string plistPath = Path.Combine (Path.GetDirectoryName (projectFilePath), info_plist_path.Replace ('\\', Path.DirectorySeparatorChar));
			info_plist.LoadWithoutNetworkAccess (plistPath);

			string bundleIdentifier = info_plist.GetCFBundleIdentifier ();

			Extension? extension = null;
			string extensionPointIdentifier = info_plist.GetNSExtensionPointIdentifier ();
			if (!string.IsNullOrEmpty (extensionPointIdentifier))
				extension = extensionPointIdentifier.ParseFromNSExtensionPointIdentifier ();

			string appPath = Path.Combine (Path.GetDirectoryName (projectFilePath),
				csproj.GetOutputPath (isSimulator ? "iPhoneSimulator" : "iPhone", configuration).Replace ('\\', Path.DirectorySeparatorChar),
				appName + (extension != null ? ".appex" : ".app"));

			if (!Directory.Exists (appPath))
				throw new Exception (string.Format ("The app directory {0} does not exist. This is probably a bug in the test harness.", appPath));

			string launchAppPath = mode == RunMode.WatchOS
				? Directory.GetDirectories (Path.Combine (appPath, "Watch"), "*.app") [0]
				: appPath;

			return new AppInformation (appName, bundleIdentifier, appPath, launchAppPath, extension);
		}

		async Task<bool> FindSimulatorAsync ()
		{
			if (simulators != null)
				return true;
			
			var sims = simulatorsLoaderFactory.CreateLoader();
			await sims.LoadAsync (Logs.Create ($"simulator-list-{Helpers.Timestamp}.log", "Simulator list"), false, false);
			simulators = await sims.FindAsync (target, MainLog);

			return simulators != null;
		}

		void FindDevice ()
		{
			if (deviceName != null)
				return;
			
			deviceName = Environment.GetEnvironmentVariable ("DEVICE_NAME");
			if (!string.IsNullOrEmpty (deviceName))
				return;
			
			var devs = devicesLoaderFactory.CreateLoader ();
			Task.Run (async () =>
			{
				await devs.LoadAsync (MainLog, false, false);
			}).Wait ();

			DeviceClass [] deviceClasses;
			switch (mode) {
			case RunMode.iOS:
				deviceClasses = new [] { DeviceClass.iPhone, DeviceClass.iPad, DeviceClass.iPod };
				break;
			case RunMode.WatchOS:
				deviceClasses = new [] { DeviceClass.Watch };
				break;
			case RunMode.TvOS:
				deviceClasses = new [] { DeviceClass.AppleTV }; // Untested
				break;
			default:
				throw new ArgumentException (nameof(mode));
			}

			var selected = devs.ConnectedDevices.Where ((v) => deviceClasses.Contains (v.DeviceClass) && v.IsUsableForDebugging != false);
			IHardwareDevice selected_data;
			if (selected.Count () == 0) {
				throw new NoDeviceFoundException ($"Could not find any applicable devices with device class(es): {string.Join (", ", deviceClasses)}");
			} else if (selected.Count () > 1) {
				selected_data = selected
					.OrderBy ((dev) =>
					{
						Version v;
						if (Version.TryParse (dev.ProductVersion, out v))
							return v;
						return new Version ();
					})
					.First ();
				MainLog.WriteLine ("Found {0} devices for device class(es) '{1}': '{2}'. Selected: '{3}' (because it has the lowest version).", selected.Count (), string.Join ("', '", deviceClasses), string.Join ("', '", selected.Select ((v) => v.Name).ToArray ()), selected_data.Name);
			} else {
				selected_data = selected.First ();
			}

			deviceName = selected_data.Name;

			if (mode == RunMode.WatchOS)
				companionDeviceName = devs.FindCompanionDevice (MainLog, selected_data).Name;
		}

		public async Task<ProcessExecutionResult> InstallAsync (CancellationToken cancellation_token)
		{
			if (isSimulator) {
				// We reset the simulator when running, so a separate install step does not make much sense.
				throw new InvalidOperationException ("Installing to a simulator is not supported.");
			}

			FindDevice ();

			var args = new List<string> ();
			if (!string.IsNullOrEmpty (harness.XcodeRoot)) {
				args.Add ("--sdkroot");
				args.Add (harness.XcodeRoot);
			}
			for (int i = -1; i < harness.Verbosity; i++)
				args.Add ("-v");
			
			args.Add ("--installdev");
			args.Add (AppInformation.AppPath);
			AddDeviceName (args, companionDeviceName ?? deviceName);

			if (mode == RunMode.WatchOS) {
				args.Add ("--device");
				args.Add ("ios,watchos");
			}

			var totalSize = Directory.GetFiles (AppInformation.AppPath, "*", SearchOption.AllDirectories).Select ((v) => new FileInfo (v).Length).Sum ();
			MainLog.WriteLine ($"Installing '{AppInformation.AppPath}' to '{companionDeviceName ?? deviceName}'. Size: {totalSize} bytes = {totalSize / 1024.0 / 1024.0:N2} MB");

			return await processManager.ExecuteCommandAsync (harness.MlaunchPath, args, MainLog, TimeSpan.FromHours (1), cancellation_token: cancellation_token);
		}

		public async Task<ProcessExecutionResult> UninstallAsync ()
		{
			if (isSimulator)
				throw new InvalidOperationException ("Uninstalling from a simulator is not supported.");

			FindDevice ();

			var args = new List<string> ();
			if (!string.IsNullOrEmpty (harness.XcodeRoot)) {
				args.Add ("--sdkroot");
				args.Add (harness.XcodeRoot);
			}
			for (int i = -1; i < harness.Verbosity; i++)
				args.Add ("-v");

			args.Add ("--uninstalldevbundleid");
			args.Add (AppInformation.BundleIdentifier);
			AddDeviceName (args, companionDeviceName ?? deviceName);

			return await processManager.ExecuteCommandAsync (harness.MlaunchPath, args, MainLog, TimeSpan.FromMinutes (1));
		}

		(string resultLine, bool failed, bool crashed) ParseResult (AppInformation appInfo, string test_log_path, bool timed_out, out bool crashed)
		{
			crashed = false;
			if (!File.Exists (test_log_path)) {
				crashed = true;
				return (null, false, true); // if we do not have a log file, the test crashes
			}
			// parsing the result is different if we are in jenkins or not.
			// When in Jenkins, Touch.Unit produces an xml file instead of a console log (so that we can get better test reporting).
			// However, for our own reporting, we still want the console-based log. This log is embedded inside the xml produced
			// by Touch.Unit, so we need to extract it and write it to disk. We also need to re-save the xml output, since Touch.Unit
			// wraps the NUnit xml output with additional information, which we need to unwrap so that Jenkins understands it.
			// 
			// On the other hand, the nunit and xunit do not have that data and have to be parsed.
			// 
			// This if statement has a small trick, we found out that internet sharing in some of the bots (VSTS) does not work, in
			// that case, we cannot do a TCP connection to xharness to get the log, this is a problem since if we did not get the xml
			// from the TCP connection, we are going to fail when trying to read it and not parse it. Therefore, we are not only
			// going to check if we are in CI, but also if the listener_log is valid.
			var path = Path.ChangeExtension (test_log_path, "xml");
			XmlResultParser.CleanXml (test_log_path, path);

			if (harness.InCI && XmlResultParser.IsValidXml (path, out var xmlType)) {
				(string resultLine, bool failed, bool crashed) parseResult = (null, false, false);
				crashed = false;
				try {
					var newFilename = XmlResultParser.GetXmlFilePath (path, xmlType);

					// at this point, we have the test results, but we want to be able to have attachments in vsts, so if the format is
					// the right one (NUnitV3) add the nodes. ATM only TouchUnit uses V3.
					var testRunName = $"{appInfo.AppName} {variation}";
					if (xmlType == XmlResultJargon.NUnitV3) {
						var logFiles = new List<string> ();
						// add our logs AND the logs of the previous task, which is the build task
						logFiles.AddRange (Directory.GetFiles (Logs.Directory));
						if (buildTask != null) // when using the run command, we do not have a build task, ergo, there are no logs to add.
							logFiles.AddRange (Directory.GetFiles (buildTask.LogDirectory));
						// add the attachments and write in the new filename
						// add a final prefix to the file name to make sure that the VSTS test uploaded just pick
						// the final version, else we will upload tests more than once
						newFilename = XmlResultParser.GetVSTSFilename (newFilename);
						XmlResultParser.UpdateMissingData (path, newFilename, testRunName, logFiles);
					} else {
						// rename the path to the correct value
						File.Move (path, newFilename);
					}
					path = newFilename;

					// write the human readable results in a tmp file, which we later use to step on the logs
					var tmpFile = Path.Combine (Path.GetTempPath (), Guid.NewGuid ().ToString ());
					(parseResult.resultLine, parseResult.failed) = XmlResultParser.GenerateHumanReadableResults (path, tmpFile, xmlType);
					File.Copy (tmpFile, test_log_path, true);
					File.Delete (tmpFile);

					// we do not longer need the tmp file
					Logs.AddFile (path, LogType.XmlLog.ToString ());
					return parseResult;

				} catch (Exception e) {
					MainLog.WriteLine ("Could not parse xml result file: {0}", e);
					// print file for better debugging
					MainLog.WriteLine ("File data is:");
					MainLog.WriteLine (new string ('#', 10));
					using (var stream = new StreamReader (path)) {
						string line;
						while ((line = stream.ReadLine ()) != null) {
							MainLog.WriteLine (line);
						}
					}
					MainLog.WriteLine (new string ('#', 10));
					MainLog.WriteLine ("End of xml results.");
					if (timed_out) {
						WrenchLog.WriteLine ($"AddSummary: <b><i>{mode} timed out</i></b><br/>");
						return parseResult;
					} else {
						WrenchLog.WriteLine ($"AddSummary: <b><i>{mode} crashed</i></b><br/>");
						MainLog.WriteLine ("Test run crashed");
						crashed = true;
						parseResult.crashed = true;
						return parseResult;
					}
				}

			}               // delete not needed copy
			File.Delete (path);
			// not the most efficient way but this just happens when we run
			// the tests locally and we usually do not run all tests, we are
			// more interested to be efficent on the bots
			string resultLine = null;
			using (var reader = new StreamReader (test_log_path)) {
				string line = null;
				bool failed = false;
				while ((line = reader.ReadLine ()) != null) {
					if (line.Contains ("Tests run:")) {
						Console.WriteLine (line);
						resultLine = line;
						break;
					} else if (line.Contains ("[FAIL]")) {
						Console.WriteLine (line);
						failed = true;
					}
				}
				return (resultLine, failed, false);
			}
		}

		public bool TestsSucceeded (AppInformation appInfo, string test_log_path, bool timed_out, out bool crashed)
		{
			var (resultLine, failed, crashed_out) = ParseResult (appInfo, test_log_path, timed_out, out crashed);
			// read the parsed logs in a human readable way
			if (resultLine != null) {
				var tests_run = resultLine.Replace ("Tests run: ", "");
				if (failed) {
					WrenchLog.WriteLine ("AddSummary: <b>{0} failed: {1}</b><br/>", mode, tests_run);
					MainLog.WriteLine ("Test run failed");
					return false;
				} else {
					WrenchLog.WriteLine ("AddSummary: {0} succeeded: {1}<br/>", mode, tests_run);
					MainLog.WriteLine ("Test run succeeded");
					return true;
				}
			} else if (timed_out) {
				WrenchLog.WriteLine ("AddSummary: <b><i>{0} timed out</i></b><br/>", mode);
				return false;
			} else {
				WrenchLog.WriteLine ("AddSummary: <b><i>{0} crashed</i></b><br/>", mode);
				MainLog.WriteLine ("Test run crashed");
				crashed = true;
				return false;
			}
		}

		public async Task<int> RunAsync ()
		{
			CrashReportSnapshot crash_reports;
			ILog device_system_log = null;
			ILog listener_log = null;
			ILog run_log = MainLog;

			if (!isSimulator)
				FindDevice ();

			crash_reports = new CrashReportSnapshot (harness, MainLog, Logs, isDevice: !isSimulator, deviceName);

			var args = new List<string> ();
			if (!string.IsNullOrEmpty (harness.XcodeRoot)) {
				args.Add ("--sdkroot");
				args.Add (harness.XcodeRoot);
			}
			for (int i = -1; i < harness.Verbosity; i++)
				args.Add ("-v");
			args.Add ("-argument=-connection-mode");
			args.Add ("-argument=none"); // This will prevent the app from trying to connect to any IDEs
			args.Add ("-argument=-app-arg:-autostart");
			args.Add ("-setenv=NUNIT_AUTOSTART=true");
			args.Add ("-argument=-app-arg:-autoexit");
			args.Add ("-setenv=NUNIT_AUTOEXIT=true");
			args.Add ("-argument=-app-arg:-enablenetwork");
			args.Add ("-setenv=NUNIT_ENABLE_NETWORK=true");
			// detect if we are using a jenkins bot.
			var useXmlOutput = harness.InCI;
			if (useXmlOutput) {
				args.Add ("-setenv=NUNIT_ENABLE_XML_OUTPUT=true");
				args.Add ("-setenv=NUNIT_ENABLE_XML_MODE=wrapped");
				args.Add ("-setenv=NUNIT_XML_VERSION=nunitv3");
			}

			if (harness.InCI) {
				// We use the 'BUILD_REVISION' variable to detect whether we're running CI or not.
				args.Add ($"-setenv=BUILD_REVISION=${Environment.GetEnvironmentVariable ("BUILD_REVISION")}");
			}

			if (!harness.GetIncludeSystemPermissionTests (TestPlatform.iOS, !isSimulator))
				args.Add ("-setenv=DISABLE_SYSTEM_PERMISSION_TESTS=1");

			if (isSimulator) {
				args.Add ("-argument=-app-arg:-hostname:127.0.0.1");
				args.Add ("-setenv=NUNIT_HOSTNAME=127.0.0.1");
			} else {
				var ips = new StringBuilder ();
				var ipAddresses = System.Net.Dns.GetHostEntry (System.Net.Dns.GetHostName ()).AddressList;
				for (int i = 0; i < ipAddresses.Length; i++) {
					if (i > 0)
						ips.Append (',');
					ips.Append (ipAddresses [i].ToString ());
				}

				args.Add ($"-argument=-app-arg:-hostname:{ips}");
				args.Add ($"-setenv=NUNIT_HOSTNAME={ips}");
			}

			listener_log = Logs.Create ($"test-{mode.ToString().ToLower()}-{Helpers.Timestamp}.log", LogType.TestLog.ToString (), timestamp: !useXmlOutput);
			var (transport, listener, listenerTmpFile) = listenerFactory.Create (mode, MainLog, listener_log, isSimulator, true, useXmlOutput);
			
			args.Add ($"-argument=-app-arg:-transport:{transport}");
			args.Add ($"-setenv=NUNIT_TRANSPORT={transport.ToString ().ToUpper ()}");

			if (transport == ListenerTransport.File)
				args.Add ($"-setenv=NUNIT_LOG_FILE={listenerTmpFile}");

			listener.Initialize ();

			args.Add ($"-argument=-app-arg:-hostport:{listener.Port}");
			args.Add ($"-setenv=NUNIT_HOSTPORT={listener.Port}");

			listener.StartAsync ();

			var cancellation_source = new CancellationTokenSource ();
			var timed_out = false;

			listener.ConnectedTask
				.TimeoutAfter (TimeSpan.FromMinutes (harness.LaunchTimeout))
				.ContinueWith ((v) => {
					if (v.IsFaulted) {
						MainLog.WriteLine ("Test launch failed: {0}", v.Exception);
					} else if (v.IsCanceled) {
						MainLog.WriteLine ("Test launch was cancelled.");
					} else if (v.Result) {
						MainLog.WriteLine ("Test run started");
					} else {
						cancellation_source.Cancel ();
						MainLog.WriteLine ("Test launch timed out after {0} minute(s).", harness.LaunchTimeout);
						timed_out = true;
					}
				}).DoNotAwait ();

			foreach (var kvp in harness.EnvironmentVariables)
				args.Add ($"-setenv={kvp.Key}={kvp.Value}");

			bool? success = null;
			bool launch_failure = false;

			if (IsExtension) {
				switch (AppInformation.Extension) {
				case Extension.TodayExtension:
					args.Add (isSimulator ? "--launchsimbundleid" : "--launchdevbundleid");
					args.Add ("todayviewforextensions:" + AppInformation.BundleIdentifier);
					args.Add ("--observe-extension");
					args.Add (AppInformation.LaunchAppPath);
					break;
				case Extension.WatchKit2:
				default:
					throw new NotImplementedException ();
				}
			} else {
				args.Add (isSimulator ? "--launchsim" : "--launchdev");
				args.Add (AppInformation.LaunchAppPath);
			}
			if (!isSimulator)
				args.Add ("--disable-memory-limits");

			var timeout = TimeSpan.FromMinutes (harness.Timeout * timeoutMultiplier);
			if (isSimulator) {
				if (!await FindSimulatorAsync ())
					return 1;

				if (mode != RunMode.WatchOS) {
					var stderr_tty = harness.GetStandardErrorTty();
					if (!string.IsNullOrEmpty (stderr_tty)) {
						args.Add ($"--stdout={stderr_tty}");
						args.Add ($"--stderr={stderr_tty}");
					} else {
						var stdout_log = Logs.CreateFile ($"stdout-{Helpers.Timestamp}.log", "Standard output");
						var stderr_log = Logs.CreateFile ($"stderr-{Helpers.Timestamp}.log", "Standard error");
						args.Add ($"--stdout={stdout_log}");
						args.Add ($"--stderr={stderr_log}");
					}
				}

				var systemLogs = new List<CaptureLog> ();
				foreach (var sim in simulators) {
					// Upload the system log
					MainLog.WriteLine ("System log for the '{1}' simulator is: {0}", sim.SystemLog, sim.Name);
					bool isCompanion = sim != simulator;

					var log = new CaptureLog (Logs, Path.Combine (logDirectory, sim.Name + ".log"), sim.SystemLog, entire_file: harness.Action != HarnessAction.Jenkins)
					{
						Description = isCompanion ? LogType.CompanionSystemLog.ToString () : LogType.SystemLog.ToString (),
					};
					log.StartCapture ();
					Logs.Add (log);
					systemLogs.Add (log);
					WrenchLog.WriteLine ("AddFile: {0}", log.Path);
				}

				MainLog.WriteLine ("*** Executing {0}/{1} in the simulator ***", AppInformation.AppName, mode);

				if (EnsureCleanSimulatorState) {
					foreach (var sim in simulators)
						await sim.PrepareSimulatorAsync (MainLog, AppInformation.BundleIdentifier);
				}

				args.Add ($"--device=:v2:udid={simulator.UDID}");

				await crash_reports.StartCaptureAsync ();

				MainLog.WriteLine ("Starting test run");

				var result = await processManager.ExecuteCommandAsync (harness.MlaunchPath, args, run_log, timeout, cancellation_token: cancellation_source.Token);
				if (result.TimedOut) {
					timed_out = true;
					success = false;
					MainLog.WriteLine ("Test run timed out after {0} minute(s).", timeout);
				} else if (result.Succeeded) {
					MainLog.WriteLine ("Test run completed");
					success = true;
				} else {
					MainLog.WriteLine ("Test run failed");
					success = false;
				}

				if (!success.Value) {
					// find pid
					var pid = -1;
					using (var reader = run_log.GetReader ()) {
						while (!reader.EndOfStream) {
							var line = reader.ReadLine ();
							if (line.StartsWith ("Application launched. PID = ", StringComparison.Ordinal)) {
								var pidstr = line.Substring ("Application launched. PID = ".Length);
								if (!int.TryParse (pidstr, out pid))
									MainLog.WriteLine ("Could not parse pid: {0}", pidstr);
							} else if (line.Contains ("Xamarin.Hosting: Launched ") && line.Contains (" with pid ")) {
								var pidstr = line.Substring (line.LastIndexOf (' '));
								if (!int.TryParse (pidstr, out pid))
									MainLog.WriteLine ("Could not parse pid: {0}", pidstr);
							} else if (line.Contains ("error MT1008")) {
								launch_failure = true;
							}
						}
					}
					if (pid > 0) {
						var launchTimedout = cancellation_source.IsCancellationRequested;
						var timeoutType = launchTimedout ? "Launch" : "Completion";
						var timeoutValue = launchTimedout ? harness.LaunchTimeout : timeout.TotalSeconds;
						MainLog.WriteLine ($"{timeoutType} timed out after {timeoutValue} seconds");
						await processManager.KillTreeAsync (pid, MainLog, true);
					} else {
						MainLog.WriteLine ("Could not find pid in mtouch output.");
					}
				}


				// cleanup after us
				if (EnsureCleanSimulatorState)
					await SimDevice.KillEverythingAsync (MainLog);

				foreach (var log in systemLogs)
					log.StopCapture ();
				
			} else {
				MainLog.WriteLine ("*** Executing {0}/{1} on device '{2}' ***", AppInformation.AppName, mode, deviceName);

				if (mode == RunMode.WatchOS) {
					args.Add ("--attach-native-debugger"); // this prevents the watch from backgrounding the app.
				} else {
					args.Add ("--wait-for-exit");
				}
				
				AddDeviceName (args);

				device_system_log = Logs.Create ($"device-{deviceName}-{Helpers.Timestamp}.log", "Device log");
				var logdev = new DeviceLogCapturer () {
					Harness =  harness,
					Log = device_system_log,
					DeviceName = deviceName,
				};
				logdev.StartCapture ();

				try {
					await crash_reports.StartCaptureAsync ();

					MainLog.WriteLine ("Starting test run");

					bool waitedForExit = true;
					// We need to check for MT1111 (which means that mlaunch won't wait for the app to exit).
					var callbackLog = new CallbackLog ((line) => {
						// MT1111: Application launched successfully, but it's not possible to wait for the app to exit as requested because it's not possible to detect app termination when launching using gdbserver
						waitedForExit &= line?.Contains ("MT1111: ") != true;
						if (line?.Contains ("error MT1007") == true)
							launch_failure = true;
					});
					var runLog = Log.CreateAggregatedLog (callbackLog, MainLog);
					var timeoutWatch = Stopwatch.StartNew ();
					var result = await processManager.ExecuteCommandAsync (harness.MlaunchPath, args, runLog, timeout, cancellation_token: cancellation_source.Token);

					if (!waitedForExit && !result.TimedOut) {
						// mlaunch couldn't wait for exit for some reason. Let's assume the app exits when the test listener completes.
						MainLog.WriteLine ("Waiting for listener to complete, since mlaunch won't tell.");
						if (!await listener.CompletionTask.TimeoutAfter (timeout - timeoutWatch.Elapsed)) {
							result.TimedOut = true;
						}
					}

					if (result.TimedOut) {
						timed_out = true;
						success = false;
						MainLog.WriteLine ("Test run timed out after {0} minute(s).", timeout.TotalMinutes);
					} else if (result.Succeeded) {
						MainLog.WriteLine ("Test run completed");
						success = true;
					} else {
						MainLog.WriteLine ("Test run failed");
						success = false;
					}
				} finally {
					logdev.StopCapture ();
					device_system_log.Dispose ();
				}

				// Upload the system log
				if (File.Exists (device_system_log.FullPath)) {
					MainLog.WriteLine ("A capture of the device log is: {0}", device_system_log.FullPath);
					WrenchLog.WriteLine ("AddFile: {0}", device_system_log.FullPath);
				}
			}

			listener.Cancel ();
			listener.Dispose ();

			// check the final status
			var crashed = false;
			if (File.Exists (listener_log.FullPath)) {
				WrenchLog.WriteLine ("AddFile: {0}", listener_log.FullPath);
				success = TestsSucceeded (this.AppInformation, listener_log.FullPath, timed_out, out crashed);
			} else if (timed_out) {
				WrenchLog.WriteLine ("AddSummary: <b><i>{0} never launched</i></b><br/>", mode);
				MainLog.WriteLine ("Test run never launched");
				success = false;
			} else if (launch_failure) {
 				WrenchLog.WriteLine ("AddSummary: <b><i>{0} failed to launch</i></b><br/>", mode);
 				MainLog.WriteLine ("Test run failed to launch");
 				success = false;
			} else {
				WrenchLog.WriteLine ("AddSummary: <b><i>{0} crashed at startup (no log)</i></b><br/>", mode);
				MainLog.WriteLine ("Test run crashed before it started (no log file produced)");
				crashed = true;
				success = false;
			}
				
			if (!success.HasValue)
				success = false;

			await crash_reports.EndCaptureAsync (TimeSpan.FromSeconds (success.Value ? 0 : 5));

			if (timed_out) {
				Result = TestExecutingResult.TimedOut;
			} else if (crashed) {
				Result = TestExecutingResult.Crashed;
			} else if (success.Value) {
				Result = TestExecutingResult.Succeeded;
			} else {
				Result = TestExecutingResult.Failed;
			}

			// Check crash reports to see if any of them explains why the test run crashed.
			if (!success.Value) {
				int pid = 0;
				string crash_reason = null;
				foreach (var crash in crash_reports.Logs) {
					try {
						if (pid == 0) {
							// Find the pid
							using (var log_reader = MainLog.GetReader ()) {
								string line;
								while ((line = log_reader.ReadLine ()) != null) {
									const string str = "was launched with pid '";
									var idx = line.IndexOf (str, StringComparison.Ordinal);
									if (idx > 0) {
										idx += str.Length;
										var next_idx = line.IndexOf ('\'', idx);
										if (next_idx > idx)
											int.TryParse (line.Substring (idx, next_idx - idx), out pid);
									}
									if (pid != 0)
										break;
								}
							}
						}

						using (var crash_reader = crash.GetReader ()) {
							var text = crash_reader.ReadToEnd ();

							var reader = System.Runtime.Serialization.Json.JsonReaderWriterFactory.CreateJsonReader (Encoding.UTF8.GetBytes (text), new XmlDictionaryReaderQuotas ());
							var doc = new XmlDocument ();
							doc.Load (reader);
							foreach (XmlNode node in doc.SelectNodes ($"/root/processes/item[pid = '" + pid + "']")) {
								Console.WriteLine (node?.InnerXml);
								Console.WriteLine (node?.SelectSingleNode ("reason")?.InnerText);
								crash_reason = node?.SelectSingleNode ("reason")?.InnerText;
							}
						}
						if (crash_reason != null) {
							// if in CI, do write an xml error that will be picked as a failure by VSTS
							if (harness.InCI) {
								XmlResultParser.GenerateFailure (Logs,
									"crash",
									AppInformation.AppName,
									variation,
									$"App Crash {AppInformation.AppName} {variation}",
									$"App crashed {crash_reason}.",
									crash_reports.Log.FullPath,
									harness.XmlJargon);
							}

							break;
						}
					} catch (Exception e) {
						harness.Log (2, "Failed to process crash report '{1}': {0}", e.Message, crash.Description);
					}
				}
				if (!string.IsNullOrEmpty (crash_reason)) {
					if (crash_reason == "per-process-limit") {
						FailureMessage = "Killed due to using too much memory (per-process-limit).";
					} else {
						FailureMessage = $"Killed by the OS ({crash_reason})";
					}
					if (harness.InCI) {
						XmlResultParser.GenerateFailure (
							Logs,
							"crash",
							AppInformation.AppName,
							variation,
							$"App Crash {AppInformation.AppName} {variation}",
							$"App crashed: {FailureMessage}",
							crash_reports.Log.FullPath,
							harness.XmlJargon);
					}
				} else if (launch_failure) {
					// same as with a crash
					FailureMessage = $"Launch failure";
					if (harness.InCI) {
						XmlResultParser.GenerateFailure (
							Logs,
							"launch",
							AppInformation.AppName,
							variation,
							$"App Launch {AppInformation.AppName} {variation} on {deviceName}",
							$"{FailureMessage} on {deviceName}",
							MainLog.FullPath,
							XmlResultJargon.NUnitV3);
					}
				} else if (!isSimulator && crashed && string.IsNullOrEmpty (crash_reason) && harness.InCI) {
					// this happens more that what we would like on devices, the main reason most of the time is that we have had netwoking problems and the
					// tcp connection could not be stablished. We are going to report it as an error since we have not parsed the logs, evne when the app might have
					// not crashed. We need to check the main_log to see if we do have an tcp issue or not
					var isTcp = false;
					using (var reader = new StreamReader (MainLog.FullPath)) {
						string line;
						while ((line = reader.ReadLine ()) != null) {
							if (line.Contains ("Couldn't establish a TCP connection with any of the hostnames")) {
								isTcp = true;
								break;
							}
						}
					}

					if (isTcp) {
						XmlResultParser.GenerateFailure (Logs,
							"tcp-connection",
							AppInformation.AppName,
							variation,
							$"TcpConnection on {deviceName}",
							$"Device {deviceName} could not reach the host over tcp.",
							MainLog.FullPath,
							harness.XmlJargon);
					}
				} else if (timed_out && harness.InCI) {
					XmlResultParser.GenerateFailure (Logs,
						"timeout",
						AppInformation.AppName,
						variation,
						$"App Timeout {AppInformation.AppName} {variation} on bot {deviceName}",
						$"{AppInformation.AppName} {variation} Test run timed out after {timeout.TotalMinutes} minute(s) on bot {deviceName}.",
						MainLog.FullPath,
						harness.XmlJargon);
				}
			}

			return success.Value ? 0 : 1;
		}

		public void AddDeviceName (IList<string> args)
		{
			AddDeviceName (args, deviceName);
		}

		public static void AddDeviceName (IList<string> args, string device_name)
		{
			if (!string.IsNullOrEmpty (device_name)) {
				args.Add ("--devname");
				args.Add (device_name);
			}
		}
	}
}
