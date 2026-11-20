using System;
using System.Runtime.InteropServices;

using Foundation;
using ObjCRuntime;

namespace MySimpleApp {
	public class Program {
		static int Main (string [] args)
		{
			GC.KeepAlive (typeof (NSObject)); // prevent linking away the platform assembly

			var testCaseString = Environment.GetEnvironmentVariable ("EXCEPTIONAL_TEST_CASE");
			if (string.IsNullOrEmpty (testCaseString)) {
				Console.WriteLine ($"The environment variable EXCEPTIONAL_TEST_CASE wasn't set.");
				return 2;
			}
			var testCase = int.Parse (testCaseString);
			switch (testCase) {
			case 1:
				AppDomain.CurrentDomain.UnhandledException += (object sender, UnhandledExceptionEventArgs e) => {
					if (e.ExceptionObject is TestCaseException) {
						Console.WriteLine (Environment.GetEnvironmentVariable ("MAGIC_WORD"));
					} else {
						Console.WriteLine ($"Unexpected exception type: {e.ExceptionObject?.GetType ()}");
					}
					Environment.Exit (0);
				};
				throw new TestCaseException ();
			case 2:
				AppDomain.CurrentDomain.UnhandledException += (object sender, UnhandledExceptionEventArgs e) => {
					if (e.ExceptionObject is TestCaseException) {
						Console.WriteLine (Environment.GetEnvironmentVariable ("MAGIC_WORD"));
					} else {
						Console.WriteLine ($"Unexpected exception type: {e.ExceptionObject?.GetType ()}");
					}
					Environment.Exit (0);
				};

				var obj = new ThrowExceptionClass ();
				var thread = new NSThread (obj, new Selector ("throwException:"), null);
				thread.Start ();
				System.Threading.Thread.Sleep (10000);
				break;
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
				return RunUncaughtExceptionTest (testCase);
			default:
				Console.WriteLine ($"Unknown test case: {testCase}");
				return 3;
			}

			return 1;
		}

		[DllImport ("__Internal")]
		static extern void throw_uncaught_objc_exception (IntPtr exception);

		static int RunUncaughtExceptionTest (int testCase)
		{
			// 3: Native-origin exception notifies the handler before termination.
			// 4: A throwing handler is logged without replacing the native-origin exception.
			// 5: An unsubscribed handler is not notified of the native-origin exception.
			// 6: A native-origin exception is handled normally without any subscribers.
			// 7: A managed-origin exception notifies the handler before the original managed exception is reported.
			// 8: A throwing handler is logged without replacing the managed-origin exception.
			var managedOrigin = testCase >= 7;
			var throwsInHandler = testCase == 4 || testCase == 8;
			using var exception = new NSException ("Uncaught test exception", "Uncaught test reason", null);
			using var obj = new ThrowExceptionClass ();

			UncaughtObjectiveCExceptionHandler handler = (sender, args) => {
				if (sender is not null)
					Console.WriteLine ("Unexpected sender");
				if (!managedOrigin && !ReferenceEquals (args.Exception, exception))
					Console.WriteLine ("Unexpected native exception");
				Console.WriteLine ($"Uncaught event: {args.Exception?.Name}: {args.Exception?.Reason}");
				if (throwsInHandler)
					throw new InvalidOperationException ("Uncaught handler failure");
			};

			if (testCase != 6)
				Runtime.UncaughtObjectiveCException += handler;
			if (testCase == 5)
				Runtime.UncaughtObjectiveCException -= handler;

			AppDomain.CurrentDomain.UnhandledException += (sender, args) => {
				if (ReferenceEquals (args.ExceptionObject, obj.Exception))
					Console.WriteLine ("Original managed exception");
			};

			Console.WriteLine (Environment.GetEnvironmentVariable ("MAGIC_WORD"));
			if (managedOrigin) {
				Runtime.MarshalManagedException += (sender, args) => {
					args.ExceptionMode = MarshalManagedExceptionMode.ThrowObjectiveCException;
				};
				using var thread = new NSThread (obj, new Selector ("throwException:"), null);
				thread.Start ();
				System.Threading.Thread.Sleep (10000);
			} else {
				throw_uncaught_objc_exception (exception.Handle);
				System.Threading.Thread.Sleep (10000);
			}

			Console.WriteLine ("Uncaught exception did not terminate the process");
			return 1;
		}
	}
}

class ThrowExceptionClass : NSObject {
	public TestCaseException Exception { get; } = new TestCaseException ();

	[Export ("throwException:")]
	public void ThrowException (NSObject obj)
	{
		throw Exception;
	}
}

class TestCaseException : Exception {
	public TestCaseException ()
		: base ("Testing, testing")
	{
	}
}
