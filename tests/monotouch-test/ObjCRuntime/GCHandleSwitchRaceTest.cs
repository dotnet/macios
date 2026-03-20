using System.Diagnostics;
using System.Threading;

namespace MonoTouchFixtures.ObjCRuntime {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class GCHandleSwitchRaceTest {

		class CustomObject : NSObject {
			[Export ("getCounter")]
			public int GetCounter ()
			{
				return 42;
			}
		}

		[Test]
		public void RunTest ()
		{
			// This test verifies a race condition in xamarin_switch_gchandle:
			// Thread A switches between weak/strong gchandles (via retain/release),
			// while Thread B fetches the gchandle (by calling an exported ObjC method).
			// The race is that Thread B can read the old gchandle after Thread A has
			// freed it but before Thread A has set the new one.
			// Ref: https://github.com/dotnet/macios/issues/24702
			var done = new ManualResetEvent (false);
			Exception? switchException = null;
			Exception? fetchException = null;

			var obj = new CustomObject ();

			// Thread that switches gchandle between weak and strong repeatedly.
			var switchThread = new Thread (() => {
				try {
					while (!done.WaitOne (0)) {
						obj.DangerousRetain ();
						obj.DangerousRelease ();
					}
				} catch (Exception ex) {
					switchException = ex;
					done.Set ();
				}
			}) {
				IsBackground = true,
				Name = "GCHandle Switch Thread",
			};
			switchThread.Start ();

			// Thread that fetches the managed object (reads the gchandle) via an ObjC call.
			var fetchThread = new Thread (() => {
				try {
					while (!done.WaitOne (0)) {
						Messaging.int_objc_msgSend (obj.Handle, Selector.GetHandle ("getCounter"));
					}
				} catch (Exception ex) {
					fetchException = ex;
					done.Set ();
				}
			}) {
				IsBackground = true,
				Name = "GCHandle Fetch Thread",
			};
			fetchThread.Start ();

			// Let the threads race for a few seconds.
			Thread.Sleep (TimeSpan.FromSeconds (3));

			done.Set ();

			// If there's a hang, the bug is there.
			if (!switchThread.Join (TimeSpan.FromSeconds (30)))
				Assert.Fail ("Switch thread is hung.");
			if (!fetchThread.Join (TimeSpan.FromSeconds (30)))
				Assert.Fail ("Fetch thread is hung.");

			Assert.IsNull (switchException, $"Switch thread failed: {switchException}");
			Assert.IsNull (fetchException, $"Fetch thread failed: {fetchException}");
		}
	}
}
