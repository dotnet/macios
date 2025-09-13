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
		public void TestWeakReferences ()
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

			const int objectCount = 100;

			var creatorThread = new Thread (() => {
				using var holder = new WeakReferenceHolder ();
				for (var i = 0; i < objectCount; i++) {
					holder.AddObject (new MyWeakReferencedObject ());
				}
				GC.Collect ();
				GC.WaitForPendingFinalizers ();

				holder.CallDoSomething (ref nilObjectCount, ref nonNilObjectCount, ref gotExpectedResponse, ref gotUnexpectedResponse, ref gotFinalizedResponse);
			}) {
				IsBackground = true,
			};
			creatorThread.Start ();

			Assert.That (creatorThread.Join (TimeSpan.FromSeconds (15)), "Join CreatorThread");

			var msg = $"Nil object count: {nilObjectCount} Non-nil object count: {nonNilObjectCount} Expected response: {gotExpectedResponse} Unexpected responses: {gotUnexpectedResponse} Finalized response: {gotFinalizedResponse}";
			Assert.Multiple (() => {
				Assert.That (nonNilObjectCount, Is.EqualTo (objectCount - nilObjectCount), $"Non-nil object count: {msg}");
				Assert.That (gotExpectedResponse, Is.EqualTo (objectCount - nilObjectCount), $"Expected response count: {msg}");
				Assert.That (gotUnexpectedResponse, Is.EqualTo (0), $"Unexpected response count: {msg}");
				Assert.That (gotFinalizedResponse, Is.EqualTo (0), $"Responses after finalization: {msg}");
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
