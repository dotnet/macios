// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#if __MACOS__

using AppKit;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSArrayControllerTests {
		[TestCase (false)]
		[TestCase (true)]
		public void RemoveEmptyArrayDoesNotDisposeUnrelatedArray (bool generic)
		{
			using var controller = new NSArrayController ();
			using NSArray array = generic ? NSArray<NSObject>.FromNSObjects ([]) : new NSArray ();
			var handle = array.Handle;
			Assert.That (handle, Is.Not.EqualTo (NativeHandle.Zero), "Initial handle");
			Assert.That (Runtime.TryGetNSObject (handle), Is.SameAs (array), "Cached wrapper");

			controller.Remove (controller.ArrangedObjects ());

			Assert.That (array.Handle, Is.EqualTo (handle), "Unrelated array must not be disposed");
			Assert.That (array.Count, Is.EqualTo ((nuint) 0), "Count");
			Assert.That (array, Is.Empty, "Enumeration");
			Assert.That (Runtime.TryGetNSObject (handle), Is.SameAs (array), "Cached wrapper after removal");
		}
	}
}

#endif // __MACOS__
