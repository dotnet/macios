// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MonoTouchFixtures.Foundation {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSFastEnumeratorTest {
		[TestCase (0)]
		[TestCase (1)]
		[TestCase (16)]
		[TestCase (17)]
		[TestCase (65)]
		public void Enumerate (int count)
		{
			var expected = Enumerable.Range (0, count).Select (v => v.ToString ()).ToArray ();
			using var array = NSArray.FromStrings (expected);
			using IEnumerator<NSString> enumerator = new NSFastEnumerator<NSString> (array);
			Assert.Throws<InvalidOperationException> (() => _ = enumerator.Current, "Before starting");
			foreach (var value in expected) {
				Assert.That (enumerator.MoveNext (), Is.True, value);
				Assert.That (enumerator.Current.ToString (), Is.EqualTo (value), "Current");
				Assert.That (((IEnumerator) enumerator).Current, Is.SameAs (enumerator.Current), "Non-generic Current");
			}
			Assert.That (enumerator.MoveNext (), Is.False, "Finished");
			Assert.That (enumerator.MoveNext (), Is.False, "Still finished");
			Assert.Throws<InvalidOperationException> (() => _ = enumerator.Current, "After finishing");
			enumerator.Reset ();
			Assert.Throws<InvalidOperationException> (() => _ = enumerator.Current, "After resetting");
			Assert.That (enumerator.MoveNext (), Is.EqualTo (count > 0), "Restart");
			if (count > 0)
				Assert.That (enumerator.Current.ToString (), Is.EqualTo ("0"), "Restarted value");
		}

		[TestCase (1)]
		[TestCase (15)]
		[TestCase (16)]
		[TestCase (17)]
		public void Mutation (int count)
		{
			using var array = new NSMutableArray ();
			using var first = new NSString ("first");
			using var second = new NSString ("second");
			for (var i = 0; i < count; i++)
				array.Add (first);
			using IEnumerator<NSString> enumerator = new NSFastEnumerator<NSString> (array);
			for (var i = 0; i < count; i++)
				Assert.That (enumerator.MoveNext (), Is.True, $"Item {i}");
			array.Add (second);
			Assert.Throws<InvalidOperationException> (() => _ = enumerator.Current, "Generic Current");
			Assert.Throws<InvalidOperationException> (() => _ = ((IEnumerator) enumerator).Current, "Non-generic Current");
			Assert.Throws<InvalidOperationException> (() => enumerator.MoveNext (), "Next batch");
		}

		[Test]
		public void Dispose ()
		{
			using var array = new NSArray ();
			IEnumerator<NSObject> enumerator = new NSFastEnumerator<NSObject> (array);
			enumerator.Dispose ();
			enumerator.Dispose ();
			Assert.Throws<ObjectDisposedException> (() => enumerator.MoveNext (), "MoveNext");
			Assert.Throws<ObjectDisposedException> (() => enumerator.Reset (), "Reset");
			Assert.Throws<ObjectDisposedException> (() => _ = enumerator.Current, "Current");
		}

		[Test]
		public void DisposedCollection ()
		{
			var array = new NSArray ();
			using var enumerator = new NSFastEnumerator<NSObject> (array);
			array.Dispose ();
			Assert.Throws<ObjectDisposedException> (() => enumerator.MoveNext (), "MoveNext");
			Assert.Throws<ObjectDisposedException> (() => enumerator.Reset (), "Reset");
			Assert.Throws<ObjectDisposedException> (() => _ = enumerator.Current, "Current");
			Assert.Throws<ObjectDisposedException> (() => new NSFastEnumerator<NSObject> (array), "Constructor");
		}

		[Test]
		public void NullCollection ()
		{
#pragma warning disable CS8625
			Assert.Throws<ArgumentNullException> (() => new NSFastEnumerator<NSObject> (null));
#pragma warning restore CS8625
		}
	}
}
