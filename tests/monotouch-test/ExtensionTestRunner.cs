// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Foundation;
using MonoTouch.NUnit.UI;

#if !__MACOS__
using UIKit;
#endif

#nullable enable

public static class ExtensionTestRunner {
	public static BaseTouchRunner CreateHeadlessRunner (string? testName = null, Action<string>? log = null)
	{
		var options = new TouchOptions ([]) {
			AutoStart = true,
			TerminateAfterExecution = false,
		};
		if (!string.IsNullOrEmpty (testName))
			options.TestName = testName;
		TouchOptions.Current = options;

		if (log is null)
			log = Console.WriteLine;

		var runner = new HeadlessTouchRunner ();
		runner.RunOnMainThread = true;
		var asms = TestLoader.GetTestAssemblies ();
		log ($"Loading {asms.Count ()} assemblies");
		foreach (var assembly in asms) {
			log ($"Loaded assembly: {assembly}");
			runner.Load (assembly);
		}
		return runner;
	}

	public static Task RunAsync (BaseTouchRunner runner)
	{
		ArgumentNullException.ThrowIfNull (runner);

		if (runner is HeadlessTouchRunner headless)
			return headless.RunOnMainThreadAsync ();

		return runner.RunAsync ();
	}

	sealed class HeadlessTouchRunner : BaseTouchRunner {
		public Task RunOnMainThreadAsync ()
		{
			var tcs = new TaskCompletionSource<object?> ();
			ExecuteOnMainThread (() => {
				try {
					Run ();
					tcs.SetResult (null);
				} catch (Exception ex) {
					tcs.SetException (ex);
				}
			});
			return tcs.Task;
		}

		protected override void WriteDeviceInformation (TextWriter writer)
		{
#if __MACOS__
			var processInfo = NSProcessInfo.ProcessInfo;
			writer.WriteLine ("[macOS: {0}]", processInfo.OperatingSystemVersionString);
#else
			var device = UIDevice.CurrentDevice;
			writer.WriteLine ("[{0}:\t{1} v{2}]", device.Model, device.SystemName, device.SystemVersion);
			writer.WriteLine ("[Device Name:\t{0}]", device.Name);
#endif
		}
	}
}
