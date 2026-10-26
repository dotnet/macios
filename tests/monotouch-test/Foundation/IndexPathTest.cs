//
// Unit tests for NSIndexPath
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class IndexPathTest {
		[Test]
		public void FromIndex ()
		{
			using (var ip = NSIndexPath.FromIndex (314159)) {
				Assert.That (ip.Length, Is.EqualTo ((nint) 1), "Length");
				var rv = ip.GetIndexes ();
				Assert.That (rv.Length, Is.EqualTo (1), "GetIndexes ().Length");
				Assert.That (rv [0], Is.EqualTo ((nuint) 314159), "GetIndexes ()[0]");
			}
		}

		[Test]
		public void IndexPathByAddingIndexTest ()
		{
			using (var ip1 = new NSIndexPath ()) {
				using (var ip2 = ip1.IndexPathByAddingIndex (3141592)) {
					Assert.That (ip2.Length, Is.EqualTo ((nint) 1), "Length");
					var rv = ip2.GetIndexes ();
					Assert.That (rv.Length, Is.EqualTo (1), "GetIndexes ().Length");
					Assert.That (rv [0], Is.EqualTo ((nuint) 3141592), "GetIndexes ()[0]");
				}
			}
		}

		[Test]
		public void IndexPathByRemovingLastIndexTest ()
		{
			using (var ip1 = NSIndexPath.FromIndex (3)) {
				using (var ip2 = ip1.IndexPathByRemovingLastIndex ()) {
					Assert.That (ip2.Length, Is.EqualTo ((nint) 0), "Length");
					var rv = ip2.GetIndexes ();
					Assert.That (rv.Length, Is.EqualTo (0), "GetIndexes ().Length");
				}
			}
		}

		[Test]
		public void IndexAtPositionTest ()
		{
			using (var ip = NSIndexPath.Create (3, 14, 15)) {
				Assert.That (ip.Length, Is.EqualTo ((nint) 3), "Length");
				Assert.That (ip.IndexAtPosition (0), Is.EqualTo ((nuint) 3), "[0]");
				Assert.That (ip.IndexAtPosition (1), Is.EqualTo ((nuint) 14), "[0]");
				Assert.That (ip.IndexAtPosition (2), Is.EqualTo ((nuint) 15), "[0]");
			}
		}

		[Test]
		public void CompareTest ()
		{
			using (var ip1 = NSIndexPath.Create (3, 14, 15)) {
				using (var ip2 = NSIndexPath.Create (3, 14, 15)) {
					using (var ip3 = NSIndexPath.Create (3, 14)) {
						Assert.That (ip1.Compare (ip2), Is.EqualTo ((nint) 0), "ip1.Compare (ip2)");
						Assert.That (ip1.Equals (ip2), Is.True, "ip1.Equals (ip2)");
						// "Two objects that are equal return hash codes that are equal."
						Assert.That (ip1.GetHashCode (), Is.EqualTo (ip2.GetHashCode ()), "GetHashCode");
						Assert.That (ip1.Compare (ip3), Is.Not.EqualTo ((nint) 0), "ip1.Compare (ip3)");
						Assert.That (ip1.Equals (ip3), Is.False, "ip1.Equals (ip3)");
					}
				}
			}
		}

		[Test]
		public void CreateTest ()
		{
			Assert.Throws<ArgumentNullException> (() => NSIndexPath.Create ((int []) null), "ANE 1");
			Assert.Throws<ArgumentNullException> (() => NSIndexPath.Create ((uint []) null), "ANE 2");
			Assert.Throws<ArgumentNullException> (() => NSIndexPath.Create ((nint []) null), "ANE 3");
			Assert.Throws<ArgumentNullException> (() => NSIndexPath.Create ((nuint []) null), "ANE 4");

			using (var ip = NSIndexPath.Create (1, 2, 3, 4)) {
				Assert.That (ip.Length, Is.EqualTo ((nint) 4), "Length");
				var rv = ip.GetIndexes ();
				Assert.That (rv.Length, Is.EqualTo (4), "GetIndexes ().Length");
				Assert.That (rv [0], Is.EqualTo ((nuint) 1), "GetIndexes ()[0]");
				Assert.That (rv [1], Is.EqualTo ((nuint) 2), "GetIndexes ()[1]");
				Assert.That (rv [2], Is.EqualTo ((nuint) 3), "GetIndexes ()[2]");
				Assert.That (rv [3], Is.EqualTo ((nuint) 4), "GetIndexes ()[3]");
			}

			using (var ip = NSIndexPath.Create ((uint) 1, (uint) 2)) {
				Assert.That (ip.Length, Is.EqualTo ((nint) 2), "Length");
				var rv = ip.GetIndexes ();
				Assert.That (rv.Length, Is.EqualTo (2), "GetIndexes ().Length");
				Assert.That (rv [0], Is.EqualTo ((nuint) 1), "GetIndexes ()[0]");
				Assert.That (rv [1], Is.EqualTo ((nuint) 2), "GetIndexes ()[1]");
			}
		}

		[Test]
		public void CreateNativeArrays ()
		{
			nint [] signed = { 0, -1, nint.MinValue, nint.MaxValue };
			nuint [] unsigned = { 0, nuint.MaxValue, (nuint) nint.MaxValue + 1 };
			using var signedPath = NSIndexPath.Create (signed);
			using var unsignedPath = NSIndexPath.Create (unsigned);
			signed [0] = 42;
			unsigned [0] = 42;
			Assert.That (signedPath.GetIndexes (), Is.EqualTo (new nuint [] { 0, nuint.MaxValue, unchecked ((nuint) nint.MinValue), (nuint) nint.MaxValue }), "Signed");
			Assert.That (unsignedPath.GetIndexes (), Is.EqualTo (new nuint [] { 0, nuint.MaxValue, (nuint) nint.MaxValue + 1 }), "Unsigned");

			using var signedParams = NSIndexPath.Create ((nint) 1, (nint) 2);
			using var unsignedParams = NSIndexPath.Create ((nuint) 1, (nuint) 2);
			Assert.That (signedParams.GetIndexes (), Is.EqualTo (new nuint [] { 1, 2 }), "Signed params");
			Assert.That (unsignedParams.GetIndexes (), Is.EqualTo (new nuint [] { 1, 2 }), "Unsigned params");
		}

		[Test]
		public void CreateNativeSignedSpan ()
		{
			nint [] source = { 42, 0, -1, nint.MinValue, nint.MaxValue, 42 };
			using var path = NSIndexPath.Create ((ReadOnlySpan<nint>) source.AsSpan (1, 4));
			source [1] = 42;
			Assert.That (path.GetIndexes (), Is.EqualTo (new nuint [] { 0, nuint.MaxValue, unchecked ((nuint) nint.MinValue), (nuint) nint.MaxValue }), "Slice copy");

			using var stackPath = NSIndexPath.Create ((ReadOnlySpan<nint>) stackalloc nint [] { -1, 123 });
			Assert.That (stackPath.GetIndexes (), Is.EqualTo (new nuint [] { nuint.MaxValue, 123 }), "Stack");
			using var empty = NSIndexPath.Create (ReadOnlySpan<nint>.Empty);
			using var defaultPath = NSIndexPath.Create (default (ReadOnlySpan<nint>));
			Assert.That (empty.Length, Is.EqualTo ((nint) 0), "Empty");
			Assert.That (defaultPath.Length, Is.EqualTo ((nint) 0), "Default");
		}

		[Test]
		public void CreateNativeUnsignedSpan ()
		{
			nuint [] source = { 42, 0, nuint.MaxValue, (nuint) nint.MaxValue + 1, 42 };
			using var path = NSIndexPath.Create ((ReadOnlySpan<nuint>) source.AsSpan (1, 3));
			source [1] = 42;
			Assert.That (path.GetIndexes (), Is.EqualTo (new nuint [] { 0, nuint.MaxValue, (nuint) nint.MaxValue + 1 }), "Slice copy");

			using var stackPath = NSIndexPath.Create ((ReadOnlySpan<nuint>) stackalloc nuint [] { nuint.MaxValue, 123 });
			Assert.That (stackPath.GetIndexes (), Is.EqualTo (new nuint [] { nuint.MaxValue, 123 }), "Stack");
			using var empty = NSIndexPath.Create (ReadOnlySpan<nuint>.Empty);
			using var defaultPath = NSIndexPath.Create (default (ReadOnlySpan<nuint>));
			Assert.That (empty.Length, Is.EqualTo ((nint) 0), "Empty");
			Assert.That (defaultPath.Length, Is.EqualTo ((nint) 0), "Default");
		}

		[Test]
		public void CreateSignedSpan ()
		{
			int [] source = { 42, 0, -1, int.MinValue, int.MaxValue, 42 };
			nuint [] expected = { 0, nuint.MaxValue, unchecked ((nuint) (nint) int.MinValue), int.MaxValue };
			using var path = NSIndexPath.Create ((ReadOnlySpan<int>) source.AsSpan (1, 4));
			using var arrayPath = NSIndexPath.Create (new int [] { 0, -1, int.MinValue, int.MaxValue });
			source [1] = 42;
			Assert.That (path.GetIndexes (), Is.EqualTo (expected), "Slice copy");
			Assert.That (arrayPath.GetIndexes (), Is.EqualTo (expected), "Array conversion");

			using var stackPath = NSIndexPath.Create ((ReadOnlySpan<int>) stackalloc int [] { -1, 123 });
			Assert.That (stackPath.GetIndexes (), Is.EqualTo (new nuint [] { nuint.MaxValue, 123 }), "Stack");
			using var empty = NSIndexPath.Create (ReadOnlySpan<int>.Empty);
			using var defaultPath = NSIndexPath.Create (default (ReadOnlySpan<int>));
			Assert.That (empty.Length, Is.EqualTo ((nint) 0), "Empty");
			Assert.That (defaultPath.Length, Is.EqualTo ((nint) 0), "Default");
		}

		[Test]
		public void CreateUnsignedSpan ()
		{
			uint [] source = { 42, 0, uint.MaxValue, (uint) int.MaxValue + 1, 42 };
			nuint [] expected = { 0, uint.MaxValue, (uint) int.MaxValue + 1 };
			using var path = NSIndexPath.Create ((ReadOnlySpan<uint>) source.AsSpan (1, 3));
			using var arrayPath = NSIndexPath.Create (new uint [] { 0, uint.MaxValue, (uint) int.MaxValue + 1 });
			source [1] = 42;
			Assert.That (path.GetIndexes (), Is.EqualTo (expected), "Slice copy");
			Assert.That (arrayPath.GetIndexes (), Is.EqualTo (expected), "Array conversion");

			using var stackPath = NSIndexPath.Create ((ReadOnlySpan<uint>) stackalloc uint [] { uint.MaxValue, 123 });
			Assert.That (stackPath.GetIndexes (), Is.EqualTo (new nuint [] { uint.MaxValue, 123 }), "Stack");
			using var empty = NSIndexPath.Create (ReadOnlySpan<uint>.Empty);
			using var defaultPath = NSIndexPath.Create (default (ReadOnlySpan<uint>));
			Assert.That (empty.Length, Is.EqualTo ((nint) 0), "Empty");
			Assert.That (defaultPath.Length, Is.EqualTo ((nint) 0), "Default");
		}

		[Test]
		public void CreateEmptyArrays ()
		{
			using var signed = NSIndexPath.Create (new int [0]);
			using var unsigned = NSIndexPath.Create (new uint [0]);
			using var nativeSigned = NSIndexPath.Create (new nint [0]);
			using var nativeUnsigned = NSIndexPath.Create (new nuint [0]);
			Assert.That (signed.Length, Is.EqualTo ((nint) 0), "Signed");
			Assert.That (unsigned.Length, Is.EqualTo ((nint) 0), "Unsigned");
			Assert.That (nativeSigned.Length, Is.EqualTo ((nint) 0), "Native signed");
			Assert.That (nativeUnsigned.Length, Is.EqualTo ((nint) 0), "Native unsigned");
		}

		[Test]
		public void GetIndexesCopies ()
		{
			using var path = NSIndexPath.Create (1, 2, 3, 4);
			var all = path.GetIndexes ();
			var range = path.GetIndexes (new NSRange (1, 2));
			Assert.That (all, Is.EqualTo (new nuint [] { 1, 2, 3, 4 }), "All");
			Assert.That (range, Is.EqualTo (new nuint [] { 2, 3 }), "Range");
			all [1] = 42;
			range [0] = 42;
			Assert.That (path.GetIndexes (), Is.EqualTo (new nuint [] { 1, 2, 3, 4 }), "Independent copies");
			Assert.That (path.GetIndexes (new NSRange (4, 0)), Is.Empty, "End");
			Assert.That (path.GetIndexes (new NSRange (0, 0)), Is.Empty, "Start");
			using var empty = new NSIndexPath ();
			Assert.That (empty.GetIndexes (), Is.Empty, "Empty path");
			Assert.That (empty.GetIndexes (new NSRange (0, 0)), Is.Empty, "Empty path range");
		}

		[Test]
		public void GetIndexesSpan ()
		{
			using var path = NSIndexPath.Create (1, 2, 3);
			nuint [] destination = { 42, 42, 42, 42, 42, 42 };
			path.GetIndexes (destination.AsSpan (1, 4));
			Assert.That (destination, Is.EqualTo (new nuint [] { 42, 1, 2, 3, 42, 42 }), "Slice");
			Span<nuint> stack = stackalloc nuint [3];
			path.GetIndexes (stack);
			Assert.That (stack.ToArray (), Is.EqualTo (new nuint [] { 1, 2, 3 }), "Stack");
			destination [1] = 42;
			Assert.That (path.GetIndexes (), Is.EqualTo (new nuint [] { 1, 2, 3 }), "Independent copy");

			using var empty = new NSIndexPath ();
			empty.GetIndexes (Span<nuint>.Empty);
			empty.GetIndexes (default (Span<nuint>));
			empty.GetIndexes (destination.AsSpan ());
			Assert.That (destination, Is.EqualTo (new nuint [] { 42, 42, 2, 3, 42, 42 }), "Empty path leaves destination unchanged");
		}

		[Test]
		public void GetIndexesRangeSpan ()
		{
			using var path = NSIndexPath.Create (1, 2, 3, 4);
			nuint [] destination = { 42, 42, 42, 42, 42 };
			path.GetIndexes (destination.AsSpan (1, 3), new NSRange (1, 2));
			Assert.That (destination, Is.EqualTo (new nuint [] { 42, 2, 3, 42, 42 }), "Slice");
			Span<nuint> stack = stackalloc nuint [2];
			path.GetIndexes (stack, new NSRange (2, 2));
			Assert.That (stack.ToArray (), Is.EqualTo (new nuint [] { 3, 4 }), "Stack end");
			path.GetIndexes (Span<nuint>.Empty, new NSRange (4, 0));
			path.GetIndexes (default (Span<nuint>), new NSRange (0, 0));
			path.GetIndexes (destination.AsSpan (), new NSRange (4, 0));
			Assert.That (destination, Is.EqualTo (new nuint [] { 42, 2, 3, 42, 42 }), "Empty range leaves destination unchanged");
			destination [1] = 42;
			Assert.That (path.GetIndexes (), Is.EqualTo (new nuint [] { 1, 2, 3, 4 }), "Independent copy");
			using var empty = new NSIndexPath ();
			empty.GetIndexes (Span<nuint>.Empty, new NSRange (0, 0));
		}

		[Test]
		public void GetIndexesSmallDestination ()
		{
			using var path = NSIndexPath.Create (1, 2, 3);
			nuint [] destination = { 42, 42, 42 };
			Assert.Throws<ArgumentException> (() => path.GetIndexes (destination.AsSpan (1, 2)), "All");
			Assert.Throws<ArgumentException> (() => path.GetIndexes (Span<nuint>.Empty), "Empty");
			Assert.Throws<ArgumentException> (() => path.GetIndexes (destination.AsSpan (1, 1), new NSRange (1, 2)), "Range");
			Assert.Throws<ArgumentException> (() => path.GetIndexes (default (Span<nuint>), new NSRange (0, 1)), "Default range");
			Assert.That (destination, Is.EqualTo (new nuint [] { 42, 42, 42 }), "Unchanged");
		}

		[Test]
		public void GetIndexesInvalidRanges ()
		{
			using var path = NSIndexPath.Create (1, 2, 3);
			nuint [] destination = { 42, 42, 42 };
			NSRange [] ranges = {
				new NSRange (-1, 1),
				new NSRange (0, -1),
				new NSRange (4, 0),
				new NSRange (3, 1),
				new NSRange (2, 2),
				new NSRange (1, nint.MaxValue),
				new NSRange (nint.MaxValue, nint.MaxValue),
			};
			foreach (var range in ranges) {
				Assert.Throws<ArgumentOutOfRangeException> (() => path.GetIndexes (range), $"Array {range}");
				Assert.Throws<ArgumentOutOfRangeException> (() => path.GetIndexes (destination.AsSpan (), range), $"Span {range}");
			}
			Assert.That (destination, Is.EqualTo (new nuint [] { 42, 42, 42 }), "Unchanged");
		}
	}
}
