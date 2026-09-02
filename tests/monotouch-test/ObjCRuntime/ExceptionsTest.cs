using System.Diagnostics;
using System.Threading;

#if !MONOMAC
using UIKit;
#endif
using Bindings.Test;

namespace MonoTouchFixtures.ObjCRuntime {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ExceptionsTest {

		MarshalObjectiveCExceptionMode defaultObjectiveCExceptionMode = MarshalObjectiveCExceptionMode.ThrowManagedException;
		MarshalManagedExceptionMode defaultManagedExceptionMode = MarshalManagedExceptionMode.Default;

		static List<MarshalObjectiveCExceptionEventArgs> objcEventArgs;
		static List<MarshalManagedExceptionEventArgs> managedEventArgs;
		static MarshalManagedExceptionMode? managedTargetMode;
		static MarshalObjectiveCExceptionMode? objcTargetMode;

		static void ObjExceptionHandler (object sender, MarshalObjectiveCExceptionEventArgs args)
		{
			objcEventArgs.Add (new MarshalObjectiveCExceptionEventArgs () {
				Exception = args.Exception,
				ExceptionMode = args.ExceptionMode,
			});
			if (objcTargetMode.HasValue)
				args.ExceptionMode = objcTargetMode.Value;
		}

		static void ManagedExceptionHandler (object sender, MarshalManagedExceptionEventArgs args)
		{
			managedEventArgs.Add (new MarshalManagedExceptionEventArgs () {
				Exception = args.Exception,
				ExceptionMode = args.ExceptionMode,
			});
			if (managedTargetMode.HasValue)
				args.ExceptionMode = managedTargetMode.Value;
		}

		static void ClearExceptionData ()
		{
			objcEventArgs = new List<MarshalObjectiveCExceptionEventArgs> ();
			managedEventArgs = new List<MarshalManagedExceptionEventArgs> ();
			objcTargetMode = null;
			managedTargetMode = null;
		}

		static void InstallHandlers ()
		{
			ClearExceptionData ();
			Runtime.MarshalManagedException += ManagedExceptionHandler;
			Runtime.MarshalObjectiveCException += ObjExceptionHandler;
		}

		static void UninstallHandlers ()
		{
			Runtime.MarshalManagedException -= ManagedExceptionHandler;
			Runtime.MarshalObjectiveCException -= ObjExceptionHandler;
		}

		// Simulator/desktop only
		[Test]
		public void ObjCException ()
		{
#if !__MACOS__ && !__MACCATALYST__
			if (Runtime.Arch == Arch.DEVICE)
				Assert.Ignore ("This test requires wrapper functions, which are not enabled for monotouch-test on device.");
#endif

#if !DEBUG
			Assert.Ignore ("This test only works in debug mode in the simulator.");
#endif

			InstallHandlers ();

			try {
				using (var e = new ObjCExceptionTest ()) {
					ObjCException thrownException = null;
					try {
						objcTargetMode = MarshalObjectiveCExceptionMode.ThrowManagedException;
						e.ThrowObjCException ();
						Assert.Fail ("managed exception not thrown");
					} catch (ObjCException ex) {
						thrownException = ex;
					}
					Assert.That (thrownException.Reason, Is.EqualTo ("exception was thrown"), "objc reason");
					Assert.That (thrownException.Name, Is.EqualTo ("Some exception"), "objc name");
					Assert.That (objcEventArgs.Count, Is.EqualTo (1), "objc exception");
					Assert.That (objcEventArgs [0].Exception.Handle, Is.EqualTo (thrownException.NSException.Handle), "objc exception");
					Assert.That (objcEventArgs [0].ExceptionMode, Is.EqualTo (defaultObjectiveCExceptionMode), "objc mode");
					Assert.That (managedEventArgs.Count, Is.EqualTo (0), "managed exception");
				}
			} finally {
				UninstallHandlers ();
			}
		}

		class ManagedExceptionTest : ObjCExceptionTest {
			public Exception Exception;
			public override void ThrowManagedException ()
			{
				Exception = new ApplicationException ("3,14");
				throw Exception;
			}
		}

		// Simulator/desktop only test
		[Test]
		public void ManagedExceptionPassthrough ()
		{
			Exception thrownException = null;

#if !__MACOS__
			TestRuntime.AssertNotDevice ("This test requires wrapper functions, which are not enabled for monotouch-test on device.");
#endif

#if !DEBUG
			Assert.Ignore ("This test only works in debug mode in the simulator.");
#endif

			var hasDebugger = global::System.Diagnostics.Debugger.IsAttached;

			InstallHandlers ();
			try {
				using (var e = new ManagedExceptionTest ()) {
					try {
						objcTargetMode = MarshalObjectiveCExceptionMode.ThrowManagedException;
						managedTargetMode = MarshalManagedExceptionMode.ThrowObjectiveCException;
						e.InvokeManagedExceptionThrower ();
						Assert.Fail ("no exception thrown 1");
					} catch (Exception ex) {
						thrownException = ex;
					}
					Assert.That (thrownException, Is.SameAs (e.Exception), "exception");
					Assert.That (thrownException.Message, Does.StartWith ("3,14"), "1 thrown message");
					Assert.That (thrownException.GetType (), Is.SameAs (typeof (ApplicationException)), "1 thrown type");
					if (hasDebugger) {
						Assert.That (objcEventArgs.Count, Is.EqualTo (0), "1 objc exception");
					} else {
						Assert.That (objcEventArgs.Count, Is.EqualTo (1), "1 objc exception");
						Assert.That (objcEventArgs [0].ExceptionMode, Is.EqualTo (defaultObjectiveCExceptionMode), "1 objc mode");
						Assert.That (objcEventArgs [0].Exception.Name, Is.EqualTo ("System.ApplicationException"), "1 objc reason");
						Assert.That (objcEventArgs [0].Exception.Reason, Does.StartWith ("3,14"), "1 objc message");
					}
					if (hasDebugger) {
						Assert.That (managedEventArgs.Count, Is.EqualTo (0), "1 managed count");
					} else {
						Assert.That (managedEventArgs.Count, Is.EqualTo (1), "1 managed count");
						Assert.That (managedEventArgs [0].ExceptionMode, Is.EqualTo (defaultManagedExceptionMode), "1 managed mode");
						Assert.That (managedEventArgs [0].Exception, Is.SameAs (thrownException), "1 managed exception");
					}

					ClearExceptionData ();
					try {
						objcTargetMode = MarshalObjectiveCExceptionMode.ThrowManagedException;
						managedTargetMode = MarshalManagedExceptionMode.ThrowObjectiveCException;
						e.InvokeManagedExceptionThrowerAndRethrow ();
						Assert.Fail ("no exception thrown 2");
					} catch (Exception ex) {
						thrownException = ex;
					}
					if (hasDebugger) {
						Assert.That (thrownException, Is.SameAs (e.Exception), "exception");
					} else {
						Assert.That (thrownException, Is.Not.SameAs (e.Exception), "exception");
						Assert.That (thrownException.GetType (), Is.SameAs (typeof (ObjCException)), "2 thrown type");
						Assert.That (((ObjCException) thrownException).Name, Is.EqualTo ("Caught exception"), "2 thrown name");
						Assert.That (((ObjCException) thrownException).Reason, Does.StartWith ("exception was rethrown"), "2 thrown reason");
					}
					if (hasDebugger) {
						Assert.That (objcEventArgs.Count, Is.EqualTo (0), "2 objc exception");
					} else {
						Assert.That (objcEventArgs.Count, Is.EqualTo (1), "2 objc exception");
						Assert.That (objcEventArgs [0].ExceptionMode, Is.EqualTo (defaultObjectiveCExceptionMode), "2 objc mode");
						Assert.That (objcEventArgs [0].Exception.Name, Is.EqualTo ("Caught exception"), "2 objc reason");
						Assert.That (objcEventArgs [0].Exception.Reason, Does.StartWith ("exception was rethrown"), "2 objc message");
					}
					if (hasDebugger) {
						Assert.That (managedEventArgs.Count, Is.EqualTo (0), "2 managed count");
					} else {
						Assert.That (managedEventArgs.Count, Is.EqualTo (1), "2 managed count");
						Assert.That (managedEventArgs [0].ExceptionMode, Is.EqualTo (defaultManagedExceptionMode), "2 managed mode");
						Assert.That (managedEventArgs [0].Exception, Is.SameAs (e.Exception), "2 managed exception");
					}

					ClearExceptionData ();
					if (!hasDebugger) {
						objcTargetMode = MarshalObjectiveCExceptionMode.ThrowManagedException;
						managedTargetMode = MarshalManagedExceptionMode.ThrowObjectiveCException;
						e.InvokeManagedExceptionThrowerAndCatch (); // no exception.
						Assert.That (objcEventArgs.Count, Is.EqualTo (0), "3 objc exception");
						Assert.That (managedEventArgs.Count, Is.EqualTo (1), "3 managed count");
						Assert.That (managedEventArgs [0].ExceptionMode, Is.EqualTo (defaultManagedExceptionMode), "3 managed mode");
						Assert.That (managedEventArgs [0].Exception, Is.SameAs (e.Exception), "3 managed exception");
					}
				}
			} finally {
				UninstallHandlers ();
			}
		}
	}
}
