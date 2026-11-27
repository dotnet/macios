using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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

		[DllImport (Constants.ObjectiveCLibrary)]
		static extern IntPtr objc_retain (IntPtr handle);

		[DllImport (Constants.ObjectiveCLibrary)]
		static extern void objc_release (IntPtr handle);

		[DllImport (Constants.ObjectiveCLibrary)]
		static extern unsafe IntPtr objc_initWeak (IntPtr* location, IntPtr handle);

		[DllImport (Constants.ObjectiveCLibrary)]
		static extern unsafe IntPtr objc_loadWeakRetained (IntPtr* location);

		[DllImport (Constants.ObjectiveCLibrary)]
		static extern unsafe void objc_destroyWeak (IntPtr* location);

		[Test]
		public void NativeOwnershipKeepsCustomPeerAlive ()
		{
#if !NATIVEAOT
			if (!TestRuntime.IsCoreCLR)
				Assert.Ignore ("Implicit toggle refs are only supported on CoreCLR and NativeAOT.");
#endif

			var weakPeer = CheckNativeOwnership (() => new CustomPeer ());

			CollectAndDrain ();
			Assert.That (weakPeer.TryGetTarget (out _), Is.False, "peer collected after native release");
		}

		[Test]
		public void ReplacedInitHandleDoesNotRootSecondPeer ()
		{
			using var owner = new CustomPeer ();
			var weakPeer = CreatePeerWithReplacedHandle (owner);

			CollectAndDrain ();
			Assert.That (weakPeer.TryGetTarget (out _), Is.False, "second peer collected");
			Assert.That (Runtime.GetNSObject (owner.Handle), Is.SameAs (owner), "original peer");
			GC.KeepAlive (owner);
		}

		[MethodImpl (MethodImplOptions.NoInlining)]
		static WeakReference<CustomPeer> CreatePeerWithReplacedHandle (CustomPeer owner)
		{
			var peer = new CustomPeer (objc_retain ((IntPtr) owner.Handle));
			Assert.That (peer.Handle, Is.EqualTo (owner.Handle), "replacement handle");
			return new WeakReference<CustomPeer> (peer, trackResurrection: true);
		}

		[Test]
		public void NativeOwnershipKeepsPeerWithNewInitHandleAlive ()
		{
#if !NATIVEAOT
			if (!TestRuntime.IsCoreCLR)
				Assert.Ignore ("Implicit toggle refs are only supported on CoreCLR and NativeAOT.");
#endif

			var weakPeer = CheckNativeOwnership (CreatePeerWithNewHandle);

			CollectAndDrain ();
			Assert.That (weakPeer.TryGetTarget (out _), Is.False, "peer collected after native release");
		}

		// The replacement handle is a new instance, so its GCHandle ivar is still empty, and
		// the peer must claim it (and its implicit toggle ref) after the handle changes.
		[MethodImpl (MethodImplOptions.NoInlining)]
		static CustomPeer CreatePeerWithNewHandle ()
		{
			var replacement = Messaging.IntPtr_objc_msgSend (Class.GetHandle (typeof (CustomPeer)), Selector.GetHandle ("alloc"));
			var peer = new CustomPeer (replacement);
			Assert.That ((IntPtr) peer.Handle, Is.EqualTo (replacement), "replacement handle");
			return peer;
		}

		// Keep any managed locals from the identity check out of the collection after release.
		[MethodImpl (MethodImplOptions.NoInlining)]
		static WeakReference<CustomPeer> CheckNativeOwnership (Func<CustomPeer> createPeer)
		{
			var (handle, retained, weakPeer) = CreateRetainedPeer (createPeer);
			try {
				Assert.That (retained, Is.EqualTo (handle), "native retain");
				CollectAndDrain ();

				weakPeer.TryGetTarget (out var original);
				var resolved = Runtime.GetNSObject (handle) as CustomPeer;
				Assert.That (original, Is.Not.Null, "original peer survived collection");
				Assert.That (resolved, Is.SameAs (original), "peer identity");
				Assert.That (resolved?.State, Is.EqualTo (42), "peer state");
			} finally {
				objc_release (retained);
			}
			return weakPeer;
		}

		[MethodImpl (MethodImplOptions.NoInlining)]
		static (IntPtr Handle, IntPtr Retained, WeakReference<CustomPeer> Peer) CreateRetainedPeer (Func<CustomPeer> createPeer)
		{
			var peer = createPeer ();
			peer.State = 42;
			var handle = (IntPtr) peer.Handle;
			var retained = PromoteWeakReference (handle);
			return (handle, retained, new WeakReference<CustomPeer> (peer, trackResurrection: true));
		}

		static unsafe IntPtr PromoteWeakReference (IntPtr handle)
		{
			var slot = IntPtr.Zero;
			objc_initWeak (&slot, handle);
			try {
				return objc_loadWeakRetained (&slot);
			} finally {
				objc_destroyWeak (&slot);
			}
		}

		static void CollectAndDrain ()
		{
			for (var i = 0; i < 3; i++) {
				GC.Collect ();
				GC.WaitForPendingFinalizers ();
				// Finalized NSObject peers release their native reference on the main thread.
				using var deadline = NSDate.FromTimeIntervalSinceNow (0.05);
				NSRunLoop.Current.RunUntil (deadline);
			}
		}

		[Preserve (AllMembers = true)]
		class CustomPeer : NSObject {
			public int State;

			public CustomPeer ()
			{
			}

			public CustomPeer (NativeHandle handle) : base (handle)
			{
			}

			// Simulate an initializer that returns a different object than the one it was sent to.
			// The caller passes ownership of 'replacement'.
			public CustomPeer (IntPtr replacement) : base (NSObjectFlag.Empty)
			{
				objc_release ((IntPtr) Handle);
				InitializeHandle (replacement, "initForTest", throwOnInitFailure: false);
			}
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
