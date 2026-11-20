// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using CoreMedia;

namespace MonoTouchFixtures.CoreMedia {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SimpleQueueTest {
		[Test]
		public void RoundTrip ()
		{
			Assert.That (CMSimpleQueue.Create (2, out var queue), Is.Zero, "Create");
			Assert.That (queue, Is.Not.Null, "Queue");
			if (queue is null)
				return;
			using (queue) {
				Assert.That (queue.Capacity, Is.EqualTo (2), "Capacity");
				Assert.That (queue.Count, Is.Zero, "Initial count");
				Assert.That (queue.Head, Is.EqualTo (IntPtr.Zero), "Empty head");
				Assert.That (queue.Dequeue (), Is.EqualTo (IntPtr.Zero), "Empty dequeue");
				Assert.That (queue.Enqueue ((IntPtr) 1), Is.Zero, "Enqueue first");
				Assert.That (queue.Enqueue ((IntPtr) 2), Is.Zero, "Enqueue second");
				Assert.That (queue.Enqueue ((IntPtr) 3), Is.EqualTo (-12773), "Full");
				Assert.That (queue.Count, Is.EqualTo (2), "Count");
				Assert.That (queue.Head, Is.EqualTo ((IntPtr) 1), "Head");
				Assert.That (queue.Dequeue (), Is.EqualTo ((IntPtr) 1), "First");
				Assert.That (queue.Dequeue (), Is.EqualTo ((IntPtr) 2), "Second");
				Assert.That (queue.Enqueue ((IntPtr) 3), Is.Zero, "Reuse");
				Assert.That (queue.Reset (), Is.Zero, "Reset");
				Assert.That (queue.Count, Is.Zero, "Reset count");
				Assert.That (queue.Dequeue (), Is.EqualTo (IntPtr.Zero), "Reset dequeue");
				Assert.Throws<ArgumentException> (() => queue.Enqueue (IntPtr.Zero), "Null element");
			}
			Assert.Throws<ObjectDisposedException> (() => queue.Dequeue (), "Disposed");
		}

		[Test]
		public void InvalidCapacity ()
		{
			Assert.That (CMSimpleQueue.Create (-1, out var queue), Is.EqualTo (-12772), "Status");
			Assert.That (queue, Is.Null, "Queue");
		}
	}
}
