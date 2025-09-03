using System;
using System.Diagnostics;
using System.Threading;

using Foundation;
using ObjCRuntime;

using Bindings.Test;

using NUnit.Framework;

using Xamarin.Utils;

namespace MonoTouchFixtures.ObjCRuntime {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NativeWeakTest {
		[Test]
		public void DoIt ()
		{
			var start = Stopwatch.StartNew ();

			var gcThread = new Thread (() => {
				while (start.Elapsed.TotalSeconds < 5) {
					GC.Collect ();
					Thread.Sleep (100);
				}
			}) {
				IsBackground = true,
			};
			gcThread.Start ();

			int nilObjectCount = 0;
			int nonNilObjectCount = 0;
			int gotExpectedResponse = 0;
			int gotUnexpectedResponse = 0;
			int gotFinalizedResponse = 0;

			var creatorThread = new Thread (() => {
				using var holder = new WeakReferenceHolder ();
				const int objectCount = 100;
				for (var i = 0; i < objectCount; i++) {
					holder.AddObject (new MyWeakReferencedObject ());
				}
				GC.Collect ();
				GC.WaitForPendingFinalizers ();
				GC.Collect ();
				GC.WaitForPendingFinalizers ();

				holder.CallDoSomething (ref nilObjectCount, ref nonNilObjectCount, ref gotExpectedResponse, ref gotUnexpectedResponse, ref gotFinalizedResponse);
				// TestRuntime.NSLog ($"Nil object count: {nilObjectCount} Non-nil object count: {nonNilObjectCount} Expected response: {gotExpectedResponse} Unexpected responses: {gotUnexpectedResponse} Finalized response: {gotFinalizedResponse}");
			}) {
				IsBackground = true,
			};
			creatorThread.Start ();

			Assert.That (creatorThread.Join (TimeSpan.FromSeconds (15)), "Join CreatorThread");

			Assert.Multiple (() => {
				Assert.That (nilObjectCount, Is.Not.EqualTo (0), "Nil object count");
				Assert.That (nonNilObjectCount, Is.Not.EqualTo (0), "Non-nil object count");
				Assert.That (gotExpectedResponse, Is.Not.EqualTo (0), "Expected response count");
				Assert.That (gotUnexpectedResponse, Is.EqualTo (0), "Unexpected response count");
				Assert.That (gotFinalizedResponse, Is.EqualTo (0), "Responses after finalization");
			});
		}
	}

	class MyWeakReferencedObject : WeakReferencedObject {
		bool finalized;

		public override int DoSomething ()
		{
			return finalized ? 314 : 42;
		}

		~MyWeakReferencedObject ()
		{
			finalized = true;
		}
	}
}
