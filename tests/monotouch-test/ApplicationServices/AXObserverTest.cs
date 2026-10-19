// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if __MACOS__
using System.Runtime.CompilerServices;
using ApplicationServices;

namespace MonoTouchFixtures.ApplicationServices {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AXObserverTest {
		[TestCase (false)]
		[TestCase (true)]
		public void Create (bool withInfo)
		{
			AXObserver? observer;
			var error = withInfo
				? AXObserver.CreateWithInfoCallback (Environment.ProcessId, (o, e, n, i, r) => Assert.Fail ("Unexpected notification"), out observer)
				: AXObserver.Create (Environment.ProcessId, (o, e, n, r) => Assert.Fail ("Unexpected notification"), out observer);
			using (observer) {
				Assert.That (error, Is.EqualTo (AXError.Success), "Create result");
				Assert.That (observer, Is.Not.Null, "Observer");
				Assert.That ((nuint) CFType.GetTypeID (observer.Handle), Is.EqualTo (AXObserver.GetTypeID ()), "Type ID");
				using var source = observer.GetRunLoopSource ();
				Assert.That (source.Handle, Is.Not.EqualTo (NativeHandle.Zero), "Source");
				CFRunLoop.Current.AddSource (source, CFRunLoop.ModeDefault);
				CFRunLoop.Current.RemoveSource (source, CFRunLoop.ModeDefault);
			}
		}

		[Test]
		public void InvalidArguments ()
		{
			Assert.Throws<ArgumentNullException> (() => AXObserver.Create (Environment.ProcessId, null, out var observer), "Null callback");
			Assert.Throws<ArgumentNullException> (() => AXObserver.CreateWithInfoCallback (Environment.ProcessId, null, out var observer), "Null info callback");
			Assert.That (AXObserver.Create (Environment.ProcessId, (o, e, n, r) => { }, out var result), Is.EqualTo (AXError.Success), "Create");
			using var observer = result;
			using var element = AXUIElement.CreateSystemWide ();
			Assert.Throws<ArgumentNullException> (() => observer.AddNotification (null, "AXValueChanged"), "Null element");
			Assert.Throws<ArgumentNullException> (() => observer.AddNotification (element, null), "Null notification");
			Assert.Throws<ArgumentNullException> (() => observer.RemoveNotification (null, "AXValueChanged"), "Remove null element");
			Assert.Throws<ArgumentNullException> (() => observer.RemoveNotification (element, null), "Remove null notification");
		}

		[Test]
		public void SystemWideNotifications ()
		{
			Assert.That (AXObserver.Create (Environment.ProcessId, (o, e, n, r) => { }, out var result), Is.EqualTo (AXError.Success), "Create");
			using var observer = result;
			using var element = AXUIElement.CreateSystemWide ();
			Assert.That (observer.AddNotification (element, "AXValueChanged"), Is.EqualTo (AXError.NotificationUnsupported), "Add");
			Assert.That (observer.RemoveNotification (element, "AXValueChanged"), Is.EqualTo (AXError.NotificationUnsupported), "Remove");
		}

		[Test]
		public void Disposed ()
		{
			Assert.That (AXObserver.Create (Environment.ProcessId, (o, e, n, r) => { }, out var observer), Is.EqualTo (AXError.Success), "Create");
			observer.Dispose ();
			observer.Dispose ();
			Assert.Throws<ObjectDisposedException> (() => observer.GetRunLoopSource (), "Source");
			using var element = AXUIElement.CreateSystemWide ();
			Assert.Throws<ObjectDisposedException> (() => observer.AddNotification (element, "AXValueChanged"), "Add");
			Assert.Throws<ObjectDisposedException> (() => observer.RemoveNotification (element, "AXValueChanged"), "Remove");
		}

		[MethodImpl (MethodImplOptions.NoInlining)]
		static WeakReference CreateUnrootedObserver ()
		{
			Assert.That (AXObserver.Create (Environment.ProcessId, (o, e, n, r) => { }, out var observer), Is.EqualTo (AXError.Success), "Create");
			return new WeakReference (observer);
		}

		[Test]
		public void DoesNotRootObserver ()
		{
			var reference = CreateUnrootedObserver ();
			GC.Collect ();
			GC.WaitForPendingFinalizers ();
			GC.Collect ();
			Assert.That (reference.IsAlive, Is.False, "Observer is collectible");
		}
	}
}
#endif
