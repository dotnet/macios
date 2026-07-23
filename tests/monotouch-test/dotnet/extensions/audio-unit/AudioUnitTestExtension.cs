// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using AudioToolbox;
using AudioUnit;
using AVFoundation;
using Foundation;
using MonoTouch.NUnit.UI;
using ObjCRuntime;

namespace MonotouchTest.AudioUnitExtensionHost {
	static class ExtensionTestHost {
		const string logPrefix = "[monotouch-test-audio-unit-extension]";
		static bool hasRun;
		static bool debugHooksInstalled;
		static readonly object guard = new object ();

		// xamarin_log is a non-variadic native function in libxamarin
		// that calls NSLog internally. We use this instead of P/Invoking
		// NSLog directly, because NSLog is variadic, and P/Invoke doesn't
		// handle variadic functions correctly on ARM64.
		[DllImport ("__Internal")]
		static extern void xamarin_log (IntPtr unicodeMessage);

		[DllImport ("/usr/lib/libSystem.B.dylib")]
		static extern unsafe int atexit (delegate* unmanaged<void> callback);

		static void Log (string message)
		{
			Console.WriteLine (message);
			var logMessage = $"ZZZZ {message}";
			unsafe {
				fixed (char* ptr = logMessage)
					xamarin_log ((IntPtr) ptr);
			}
		}

		static void SafeLog (string message)
		{
			try {
				Log (message);
			} catch (Exception ex) {
				Console.WriteLine ($"{message}{Environment.NewLine}{ex}");
			}
		}

		public static unsafe void InstallDebugHooks ()
		{
			lock (guard) {
				if (debugHooksInstalled)
					return;
				debugHooksInstalled = true;
			}

			Runtime.MarshalObjectiveCException += ObjectiveCExceptionMarshaled;
			Runtime.MarshalManagedException += ManagedExceptionMarshaled;

			var rv = atexit (&AtExitCallback);
			SafeLog ($"{logPrefix} Installed debug hooks (atexit registration result: {rv}).");
		}

		static void ObjectiveCExceptionMarshaled (object? sender, MarshalObjectiveCExceptionEventArgs args)
		{
			var stackTrace = args.Exception.CallStackSymbols is null
				? new StackTrace (1, true).ToString ()
				: string.Join (Environment.NewLine, args.Exception.CallStackSymbols);
			SafeLog ($"{logPrefix} Objective-C exception marshaled. Mode: {args.ExceptionMode}. Name: {args.Exception.Name}. Reason: {args.Exception.Reason}{Environment.NewLine}{stackTrace}");
		}

		static void ManagedExceptionMarshaled (object? sender, MarshalManagedExceptionEventArgs args)
		{
			SafeLog ($"{logPrefix} Managed exception marshaled. Mode: {args.ExceptionMode}.{Environment.NewLine}{args.Exception}");
		}

		[UnmanagedCallersOnly]
		static void AtExitCallback ()
		{
			SafeLog ($"{logPrefix} Process is exiting.{Environment.NewLine}{new StackTrace (1, true)}");
		}

		static string? GetTestName ()
		{
			var testName = Environment.GetEnvironmentVariable ("NUNIT_TEST_NAME");
			if (!string.IsNullOrWhiteSpace (testName)) {
				SafeLog ($"{logPrefix} Using NUNIT_TEST_NAME test filter: {testName}");
				return testName;
			}

			var mainBundlePath = NSBundle.MainBundle.BundlePath ?? "";
			var resourcePath = NSBundle.MainBundle.ResourcePath ?? "";
			var extensionBundlePath = mainBundlePath.EndsWith (".app", StringComparison.OrdinalIgnoreCase)
				? Path.Combine (mainBundlePath, "Contents", "PlugIns", "AppExtension.appex")
				: mainBundlePath;
			var hostAppPath = mainBundlePath.EndsWith (".appex", StringComparison.OrdinalIgnoreCase)
				? Path.GetFullPath (Path.Combine (mainBundlePath, "..", "..", ".."))
				: mainBundlePath;
			SafeLog ($"{logPrefix} Main bundle path: {mainBundlePath}");
			SafeLog ($"{logPrefix} Resource path: {resourcePath}");
			foreach (var testFilterFile in new [] {
				Path.Combine (resourcePath, "monotouch-extension-test-filter.txt"),
				Path.Combine (extensionBundlePath, "Contents", "Resources", "monotouch-extension-test-filter.txt"),
				Path.Combine (hostAppPath, "Contents", "Resources", "monotouch-extension-test-filter.txt"),
				Path.Combine (Path.GetTempPath (), "monotouch-test", "extensions", "audio-unit", "test-filter.txt"),
			}) {
				SafeLog ($"{logPrefix} Checking for test filter file: {testFilterFile}");
				if (File.Exists (testFilterFile)) {
					testName = File.ReadAllText (testFilterFile).Trim ();
					if (!string.IsNullOrWhiteSpace (testName)) {
						SafeLog ($"{logPrefix} Using file-based test filter: {testName}");
						return testName;
					}
				}
			}

			testName = NSBundle.MainBundle.ObjectForInfoDictionary ("MonotouchExtensionTestName")?.ToString ();
			if (!string.IsNullOrWhiteSpace (testName) && testName.Contains ("$("))
				return null;
			return string.IsNullOrWhiteSpace (testName) ? null : testName;
		}

		public static async Task RunOnce ()
		{
			lock (guard) {
				if (hasRun)
					return;
				hasRun = true;
			}

			var testName = GetTestName ();

			var runner = ExtensionTestRunner.CreateHeadlessRunner (TestLoader.GetTestAssemblies (), testName, Log);
			runner.LogCallback = Log;

			var runDescription = string.IsNullOrEmpty (testName) ? "all monotouch-test tests" : testName;
			var startMessage = $"{logPrefix} Starting monotouch-test audio unit extension test run ({runDescription})";
			Log (startMessage);
			try {
				await ExtensionTestRunner.RunAsync (runner);
				var summary = $"{logPrefix} Finished monotouch-test audio unit extension test run. Passed: {runner.PassedCount} Failed: {runner.FailedCount} Ignored: {runner.IgnoredCount} Inconclusive: {runner.InconclusiveCount}";
				Log (summary);
			} catch (Exception ex) {
				var failure = $"{logPrefix} Extension test run failed: {ex}";
				Log (failure);
			}
		}
	}


	[Register ("MonotouchTestAudioUnitFactory")]
	public class MonotouchTestAudioUnitFactory : NSObject, IAUAudioUnitFactory {
		public MonotouchTestAudioUnitFactory (NativeHandle handle) : base (handle)
		{
		}

		public AUAudioUnit CreateAudioUnit (AudioComponentDescription desc, out NSError error)
		{
			ExtensionTestHost.InstallDebugHooks ();
			error = null;
			var audioUnit = new MonotouchTestAudioUnit (desc, out error);
			if (error is null)
				Task.Run (async () => {
					await Task.Delay (1000);
					await ExtensionTestHost.RunOnce ();
				});
			return audioUnit;
		}

		[Export ("beginRequestWithExtensionContext:")]
		public void BeginRequestWithExtensionContext (NSExtensionContext context)
		{
			ExtensionTestHost.InstallDebugHooks ();
			Task.Run (async () => await ExtensionTestHost.RunOnce ());
		}
	}

	[Register ("MonotouchTestAudioUnit")]
	public class MonotouchTestAudioUnit : AUAudioUnit {
		AUAudioUnitBusArray inputBusArray;
		AUAudioUnitBusArray outputBusArray;

		public MonotouchTestAudioUnit (AudioComponentDescription componentDescription, out NSError error)
			: base (componentDescription, AudioComponentInstantiationOptions.OutOfProcess, out error)
		{
			var format = new AVAudioFormat (44100, 2);
			var inputBus = new AUAudioUnitBus (format, out error);
			var outputBus = new AUAudioUnitBus (format, out error);
			inputBusArray = new AUAudioUnitBusArray (this, AUAudioUnitBusType.Input, new [] { inputBus });
			outputBusArray = new AUAudioUnitBusArray (this, AUAudioUnitBusType.Output, new [] { outputBus });
		}

		public MonotouchTestAudioUnit (NativeHandle handle) : base (handle)
		{
		}

		public override AUAudioUnitBusArray InputBusses => inputBusArray;

		public override AUAudioUnitBusArray OutputBusses => outputBusArray;

		public override AUInternalRenderBlock InternalRenderBlock {
			get {
				return (ref AudioUnitRenderActionFlags actionFlags, ref AudioTimeStamp timestamp,
						uint frameCount, nint outputBusNumber, AudioBuffers outputData,
						AURenderEventEnumerator realtimeEventListHead, AURenderPullInputBlock pullInputBlock) => {
					if (pullInputBlock is null)
						return AudioUnitStatus.NoError;
					pullInputBlock (ref actionFlags, ref timestamp, frameCount, 0, outputData);
					return AudioUnitStatus.NoError;
				};
			}
		}
	}
}
