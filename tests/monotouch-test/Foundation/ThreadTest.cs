//
// Unit tests for NSThread
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

using System.Reflection;
using System.Threading;

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ThreadTest {

		[Test]
		public void MainThread ()
		{
			ClassicAssert.True (NSThread.IsMain, "IsMain");
			ClassicAssert.True (NSThread.MainThread.IsMainThread, "IsMainThread");
		}

		[Test]
		public void GetEntryAssemblyReturnsOk ()
		{
			ClassicAssert.IsNotNull (Assembly.GetEntryAssembly ());
			ClassicAssert.IsTrue (NSThread.IsMain);
			int rv = -1;
			var t = new Thread (() => {
				if (NSThread.IsMain)
					rv = 1;
				else if (Assembly.GetEntryAssembly () is null)
					rv = 2;
				else
					rv = 0;
			}) {
				IsBackground = true,
			};
			t.Start ();
			t.Join ();
			ClassicAssert.AreEqual (0, rv);
		}

		[Test]
		public void InitWithDataTest ()
		{
			var obj = new InitWithDataObject ();
			var thread = new NSThread (obj, new Selector ("start:"), null);
			thread.Start ();
			ClassicAssert.IsTrue (obj.StartedEvent.WaitOne (TimeSpan.FromSeconds (5)), "thread start");
			GC.Collect ();
		}

		class InitWithDataObject : NSObject {
			public ManualResetEvent StartedEvent = new ManualResetEvent (false);

			[Export ("start:")]
			public void Start (NSObject obj)
			{
				StartedEvent.Set ();
			}
		}
	}
}
