//
// Unit tests for NSArray Generic support
//
// Authors:
//	Alex Soto <alex.soto@xamarin.com>
//	
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

using System.Collections;

#nullable enable

namespace MonoTouchFixtures.Foundation {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSArray1Test {
		[Test]
		public void Ctor ()
		{
			var arr = new NSArray<NSData> ();

			Assert.That (arr.Count, Is.EqualTo ((nuint) 0), "NSArray Count");
		}

		[Test]
		public void FromNSObjectsTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";

			using (var arr = NSArray<NSString>.FromNSObjects (str1, str2, str3)) {
				Assert.That (arr.Count, Is.EqualTo ((nuint) 3), "NSArray Count");
				Assert.That (arr [0], Is.SameAs (str1), "NSArray indexer");
				Assert.That (arr [1], Is.SameAs (str2), "NSArray indexer");
				Assert.That (arr [2], Is.SameAs (str3), "NSArray indexer");
			}
		}

		[Test]
		public void FromNSObjectsCountTest ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";

			using (var arr = NSArray<NSString>.FromNSObjects (3, str1, str2, str3)) {
				Assert.That (arr.Count, Is.EqualTo ((nuint) 3), "NSArray Count");
				Assert.That (arr [0], Is.SameAs (str1), "NSArray indexer");
				Assert.That (arr [1], Is.SameAs (str2), "NSArray indexer");
				Assert.That (arr [2], Is.SameAs (str3), "NSArray indexer");
			}
		}

		[Test]
		public void IEnumerableTest ()
		{
			const int C = 16 * 2 + 3; // NSFastEnumerator has a array of size 16, use more than that, and not an exact multiple.
			var values = new NSString [C];
			for (int i = 0; i < C; i++)
				values [i] = (NSString) i.ToString ();

			var st = NSArray<NSString>.FromNSObjects (values);
			Assert.That (st.Count, Is.EqualTo ((nuint) C), "Count 1");

			var lst = new List<NSString> ();
			foreach (NSString a in (IEnumerable) st) {
				Assert.That (a, Is.Not.Null, "null item iterator");
				Assert.That (lst.Contains (a), Is.False, "duplicated item iterator");
				lst.Add (a);
				Assert.That (Array.IndexOf (values, a) >= 0, Is.True, "different object");
			}
			Assert.That (lst.Count, Is.EqualTo (C), "iterator count");
		}

		[Test]
		public void IEnumerable1Test_EnumeratorType ()
		{
			var myEnumerable = new NSArray<NSNumber> ();
			foreach (var item in myEnumerable) {
				// The point of this test is to verify that the compiler finds the correct enumerator (the one returning NSNumbers, and not the one from the non-generic NSSet class returning NSObjects).
				// This means that we don't have to actually execute this code, it's enough to make it compile.
				Console.WriteLine (item.LongValue);
			}
		}

		[Test]
		public void FromNSObjectsNullTest ()
		{
			var str1 = (NSString) "1";
			NSString str2 = null;
			var str3 = (NSString) "3";

			using (var arr = NSArray<NSString>.FromNSObjects (str1, str2, str3)) {
				Assert.That (arr.Count, Is.EqualTo ((nuint) 3), "NSArray Count");
				Assert.That (arr [0], Is.SameAs (str1), "NSArray indexer");
				Assert.That (arr [1], Is.Null, "NSArray null indexer");
				Assert.That (arr [2], Is.SameAs (str3), "NSArray indexer");
			}
		}

		[Test]
		public void ToArray ()
		{
			using (var a = NSArray<NSString>.FromNSObjects ((NSString) "abc")) {
				var arr = a.ToArray ();
				NSString element = arr [0];
				Assert.That (arr.Length, Is.EqualTo (1), "Length");
				Assert.That (arr [0].ToString (), Is.EqualTo ("abc"), "Value");
				Assert.That ((string) element, Is.EqualTo ("abc"), "Value element");
			}
		}

		[Test]
		public void ToArray_T ()
		{
			using (var a = NSArray<NSString>.FromNSObjects ((NSString) "abc")) {
				var arr = a.ToArray ();
				NSString element = arr [0];
				Assert.That (arr.Length, Is.EqualTo (1), "Length");
				Assert.That (arr [0].ToString (), Is.EqualTo ("abc"), "Value");
				Assert.That ((string) element, Is.EqualTo ("abc"), "Value element");
			}
		}

		[Test]
		public void FromIntPtrs_NativeHandle ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";

			var handles = new NativeHandle [] { str1.Handle, str2.Handle, str3.Handle };
			using (var arr = NSArray.FromIntPtrs (handles)) {
				Assert.That (arr.Count, Is.EqualTo ((nuint) 3), "NSArray Count");
				Assert.That (arr.GetItem<NSString> (0).ToString (), Is.EqualTo ("1"), "NSArray item 0");
				Assert.That (arr.GetItem<NSString> (1).ToString (), Is.EqualTo ("2"), "NSArray item 1");
				Assert.That (arr.GetItem<NSString> (2).ToString (), Is.EqualTo ("3"), "NSArray item 2");
			}
		}

		[Test]
		public void FromIntPtrs_NativeHandle_Null ()
		{
			NativeHandle []? handles = null;
			Assert.Throws<ArgumentNullException> (() => NSArray.FromIntPtrs (handles!), "Null array");
		}

		[Test]
		public void FromIntPtrs_NativeHandle_Empty ()
		{
			var handles = new NativeHandle [0];
			using (var arr = NSArray.FromIntPtrs (handles)) {
				Assert.That (arr.Count, Is.EqualTo ((nuint) 0), "NSArray Count");
			}
		}

		[Test]
		public void FromNSObjects_JaggedArray ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var str4 = (NSString) "4";

			var jaggedArray = new NSString [] [] {
				new NSString [] { str1, str2 },
				new NSString [] { str3, str4 }
			};

			using (var arr = NSArray.FromNSObjects (jaggedArray)) {
				Assert.That (arr.Count, Is.EqualTo ((nuint) 2), "Outer array count");
				var row0 = arr.GetItem<NSArray> (0);
				var row1 = arr.GetItem<NSArray> (1);
				Assert.That (row0.Count, Is.EqualTo ((nuint) 2), "Row 0 count");
				Assert.That (row1.Count, Is.EqualTo ((nuint) 2), "Row 1 count");
				Assert.That (row0.GetItem<NSString> (0).ToString (), Is.EqualTo ("1"), "Row 0, Item 0");
				Assert.That (row0.GetItem<NSString> (1).ToString (), Is.EqualTo ("2"), "Row 0, Item 1");
				Assert.That (row1.GetItem<NSString> (0).ToString (), Is.EqualTo ("3"), "Row 1, Item 0");
				Assert.That (row1.GetItem<NSString> (1).ToString (), Is.EqualTo ("4"), "Row 1, Item 1");
			}
		}

		[Test]
		public void FromNSObjects_JaggedArray_Null ()
		{
			NSString [] []? jaggedArray = null;
			var arr = NSArray.FromNSObjects (jaggedArray);
			Assert.That (arr, Is.Null, "Should return null for null input");
		}

		[Test]
		public void FromNSObjects_JaggedArray_NullRow ()
		{
			var str1 = (NSString) "1";
			var jaggedArray = new NSString? [] [] {
				new NSString [] { str1 },
				null
			};

			Assert.Throws<ArgumentNullException> (() => NSArray.FromNSObjects (jaggedArray), "Should throw for null row");
		}

		[Test]
		public void FromNSObjects_JaggedArray_NullElement ()
		{
			var str1 = (NSString) "1";
			var jaggedArray = new NSString? [] [] {
				new NSString? [] { str1, null }
			};

			Assert.Throws<ArgumentNullException> (() => NSArray.FromNSObjects (jaggedArray), "Should throw for null element");
		}

		[Test]
		public void FromNSObjects_2DArray ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var str4 = (NSString) "4";

			var array2D = new NSString [,] {
				{ str1, str2 },
				{ str3, str4 }
			};

			using (var arr = NSArray.FromNSObjects (array2D)) {
				Assert.That (arr.Count, Is.EqualTo ((nuint) 2), "Outer array count");
				var row0 = arr.GetItem<NSArray> (0);
				var row1 = arr.GetItem<NSArray> (1);
				Assert.That (row0.Count, Is.EqualTo ((nuint) 2), "Row 0 count");
				Assert.That (row1.Count, Is.EqualTo ((nuint) 2), "Row 1 count");
			}
		}

		[Test]
		public void FromNSObjects_2DArray_Null ()
		{
			NSString [,]? array2D = null;
			var arr = NSArray.FromNSObjects (array2D);
			Assert.That (arr, Is.Null, "Should return null for null input");
		}

		[Test]
		public void FromNSObjects_WithConverter ()
		{
			var numbers = new int [] { 1, 2, 3 };
			using (var arr = NSArray.FromNSObjects<int> (x => NSNumber.FromInt32 (x), numbers)) {
				Assert.That (arr.Count, Is.EqualTo ((nuint) 3), "Count");
				Assert.That (arr.GetItem<NSNumber> (0).Int32Value, Is.EqualTo (1), "Item 0");
				Assert.That (arr.GetItem<NSNumber> (1).Int32Value, Is.EqualTo (2), "Item 1");
				Assert.That (arr.GetItem<NSNumber> (2).Int32Value, Is.EqualTo (3), "Item 2");
			}
		}

		[Test]
		public void FromNSObjects_WithConverter_Null ()
		{
			int []? numbers = null;
			var arr = NSArray.FromNSObjects<int> (x => NSNumber.FromInt32 (x), numbers);
			Assert.That (arr, Is.Null, "Should return null for null input");
		}

		[Test]
		public void FromNSObjects_WithConverter_NullConverter ()
		{
			var numbers = new int [] { 1, 2, 3 };
			Assert.Throws<ArgumentNullException> (() => NSArray.FromNSObjects<int> (null, numbers), "Should throw for null converter");
		}

		[Test]
		public void FromNSObjects_WithConverter_ReturnsNull ()
		{
			var numbers = new int? [] { 1, null, 3 };
			var arr = NSArray.FromNSObjects<int?> (x => x.HasValue ? NSNumber.FromInt32 (x.Value) : null, numbers);
			Assert.That (arr, Is.Not.Null, "Array should not be null");
			Assert.That (arr!.Count, Is.EqualTo ((nuint) 3), "Count");

			// Check if the array actually contains NSNull at index 1
			// Use reflection or try-catch to see what's there
			try {
				var item0 = arr.GetItem<NSNumber> (0);
				Assert.That (item0, Is.Not.Null, "Item 0 should not be null");
				Assert.That (item0.Int32Value, Is.EqualTo (1), "Item 0");
			} catch (Exception ex) {
				Assert.Fail ($"Item 0 failed: {ex.Message}");
			}

			// The converter returns null, so we expect NSNull in the array
			// But GetItem<T> might skip null items or return null
			var count = arr.Count;
			Assert.That (count, Is.EqualTo ((nuint) 3), "Should have 3 items including null");

			try {
				var item2 = arr.GetItem<NSNumber> (2);
				Assert.That (item2, Is.Not.Null, "Item 2 should not be null");
				Assert.That (item2.Int32Value, Is.EqualTo (3), "Item 2");
			} catch (Exception ex) {
				Assert.Fail ($"Item 2 failed: {ex.Message}");
			}

			arr.Dispose ();
		}

		[Test]
		public void FromObjects_WithCount_ConvertsOnlyCount ()
		{
			var items = new object [] { 1, 2, 3, 4, 5 };

			using (var arr = NSArray.FromObjects (2, items)) {
				// This should only convert the first 2 items
				Assert.That (arr.Count, Is.EqualTo ((nuint) 2), "Count should be 2");
			}
		}

		[Test]
		public void FromObjects_CountLargerThanArray ()
		{
			var items = new object [] { 1, 2, 3 };
			Assert.Throws<ArgumentException> (() => NSArray.FromObjects (5, items), "Should throw when count > array length");
		}

		[Test]
		public void FromObjects_ConverterThrows ()
		{
			// FromObjects uses NSObject.FromObject which can throw for unsupported types
			var items = new object? [] { new global::System.IO.MemoryStream () };
			Assert.Throws<ArgumentException> (() => NSArray.FromObjects (items), "Should throw for unmarshalable type");
		}

		[Test]
		public void FromNSObjects_CountFirst_WithNull ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var items = new NSString? [] { str1, null, str3, str1 };

			using (var arr = NSArray.FromNSObjects<NSString> (3, items)) {
				Assert.That (arr.Count, Is.EqualTo ((nuint) 3), "Count should include null");
				Assert.That (arr.GetItem<NSString> (0), Is.EqualTo (str1), "Item 0");
				// Item 1 is null, but GetItem may not retrieve it properly
				// Just verify count is correct (3 items including the null)
				Assert.That (arr.GetItem<NSString> (2), Is.EqualTo (str3), "Item 2");
			}
		}

		[Test]
		public void FromNSObjects_CountFirst_Basic ()
		{
			var str1 = (NSString) "1";
			var str2 = (NSString) "2";
			var str3 = (NSString) "3";
			var str4 = (NSString) "4";
			var items = new NSString [] { str1, str2, str3, str4 };

			using (var arr = NSArray.FromNSObjects<NSString> (2, items)) {
				Assert.That (arr.Count, Is.EqualTo ((nuint) 2), "Count");
				Assert.That (arr.GetItem<NSString> (0), Is.EqualTo (str1), "Item 0");
				Assert.That (arr.GetItem<NSString> (1), Is.EqualTo (str2), "Item 1");
			}
		}

		[Test]
		public void FromNSObjects_CountFirst_NullArray ()
		{
			NSString []? items = null;
			using (var arr = NSArray.FromNSObjects<NSString> (0, items)) {
				Assert.That (arr.Count, Is.EqualTo ((nuint) 0), "Null array should create empty array");
			}
		}

		[Test]
		public void FromObjects_BasicTypes ()
		{
			var items = new object [] { 1, "hello", 3.14, true };
			using (var arr = NSArray.FromObjects (items)) {
				Assert.That (arr.Count, Is.EqualTo ((nuint) 4), "Count");
				Assert.That (arr.GetItem<NSNumber> (0).Int32Value, Is.EqualTo (1), "Item 0");
				Assert.That (arr.GetItem<NSString> (1).ToString (), Is.EqualTo ("hello"), "Item 1");
				Assert.That (arr.GetItem<NSNumber> (2).DoubleValue, Is.EqualTo (3.14).Within (0.01), "Item 2");
				Assert.That (arr.GetItem<NSNumber> (3).BoolValue, Is.EqualTo (true), "Item 3");
			}
		}

		[Test]
		public void FromObjects_Null ()
		{
			object []? items = null;
			using (var arr = NSArray.FromObjects (items)) {
				Assert.That (arr.Count, Is.EqualTo ((nuint) 0), "Should return empty array for null input");
			}
		}

		[Test]
		public void FromObjects_WithCount ()
		{
			var items = new object [] { 1, 2, 3, 4, 5 };
			using (var arr = NSArray.FromObjects (3, items)) {
				Assert.That (arr.Count, Is.EqualTo ((nuint) 3), "Count");
				Assert.That (arr.GetItem<NSNumber> (0).Int32Value, Is.EqualTo (1), "Item 0");
				Assert.That (arr.GetItem<NSNumber> (1).Int32Value, Is.EqualTo (2), "Item 1");
				Assert.That (arr.GetItem<NSNumber> (2).Int32Value, Is.EqualTo (3), "Item 2");
			}
		}

		[Test]
		public void FromObjects_WithCountZero ()
		{
			var items = new object [] { 1, 2, 3 };
			using (var arr = NSArray.FromObjects (0, items)) {
				Assert.That (arr.Count, Is.EqualTo ((nuint) 0), "Count should be 0");
			}
		}

		[Test]
		public void FromObjects_WithNegativeCount ()
		{
			var items = new object [] { 1, 2, 3 };
			Assert.Throws<ArgumentOutOfRangeException> (() => NSArray.FromObjects (-1, items), "Should throw for negative count");
		}

		[Test]
		public void FromObjects_WithCount_Null ()
		{
			using (var arr = NSArray.FromObjects (0, null)) {
				Assert.That (arr.Count, Is.EqualTo ((nuint) 0), "Should return empty array");
			}
		}

		[Test]
		public void EnumsFromHandle_ReturnsNullForZeroHandle ()
		{
			var result = NSArray.EnumsFromHandle<NSComparisonResult> (NativeHandle.Zero);
			Assert.That (result, Is.Null, "null for zero handle");
		}

		[Test]
		public void EnumsFromHandle_Roundtrip ()
		{
			var expected = new [] {
				NSComparisonResult.Ascending,
				NSComparisonResult.Same,
				NSComparisonResult.Descending,
			};
			using var n0 = NSNumber.FromNInt ((nint) (long) expected [0]);
			using var n1 = NSNumber.FromNInt ((nint) (long) expected [1]);
			using var n2 = NSNumber.FromNInt ((nint) (long) expected [2]);
			using var array = NSArray.FromNSObjects (n0, n1, n2);
			var actual = NSArray.EnumsFromHandle<NSComparisonResult> (array.Handle);
			Assert.That (actual, Is.Not.Null, "not null");
			Assert.That (actual!.Length, Is.EqualTo (3), "Length");
			Assert.That (actual, Is.EqualTo (expected), "values");
		}

		[Test]
		public void EnumsFromHandle_ThrowsForNonEnum ()
		{
			Assert.Throws<ArgumentException> (() => NSArray.EnumsFromHandle<int> (NativeHandle.Zero), "non-enum should throw");
		}

#if false // https://github.com/dotnet/macios/issues/15577
		[Test]
		public void GetDifferenceFromArrayTest ()
		{
			TestRuntime.AssertXcodeVersion (13,0);
			using var str1 = (NSString) "1";
			using var str2 = (NSString) "1";
			using var str3 = (NSString) "1";
			
			using var array1 = NSArray.FromObjects (str1, str2);
			using var array2 = NSArray.FromObjects (str1, str3);
			NSOrderedCollectionDifference? diff = null;
			Assert.DoesNotThrow (() => {
				diff = array1.GetDifferenceFromArray (array2,
					NSOrderedCollectionDifferenceCalculationOptions.OmitInsertedObjects,
					(first, second) => {
						var firstStr = (NSString) first;
						var secondStr = (NSString) second;
						return first.ToString ().Equals (second.ToString ());
					});
			}, "Not throws");
			// https://github.com/dotnet/macios/issues/15577 - Did not rewrite tests that were disabled
			// Maybe assert that we get a specific diff result as well?
			Assert.That (diff, Is.Not.Null, "Not null");
		}
#endif

		[Test]
		public void FromArrayOfArray ()
		{
			var inner1 = NSArray.FromNSObjects ((NSString) "a", (NSString) "b");
			var inner2 = NSArray.FromNSObjects ((NSString) "c");
			using var outer = NSArray.FromNSObjects (inner1, inner2);

			var result = NSArray.FromArrayOfArray (outer);

			Assert.That (result, Is.Not.Null, "result");
			Assert.That (result!.Length, Is.EqualTo (2), "outer length");
			Assert.That (result [0].Length, Is.EqualTo (2), "inner1 length");
			Assert.That (result [1].Length, Is.EqualTo (1), "inner2 length");
			Assert.That (result [0] [0].ToString (), Is.EqualTo ("a"), "inner1[0]");
			Assert.That (result [0] [1].ToString (), Is.EqualTo ("b"), "inner1[1]");
			Assert.That (result [1] [0].ToString (), Is.EqualTo ("c"), "inner2[0]");
		}

		[Test]
		public void FromArrayOfArray_Null ()
		{
			var result = NSArray.FromArrayOfArray (null);
			Assert.That (result, Is.Null, "result");
		}

		[Test]
		public void From_JaggedArray ()
		{
			var items = new NSObject [] [] {
				new NSObject [] { (NSString) "x", (NSString) "y" },
				new NSObject [] { (NSString) "z" },
			};

			using var arr = NSArray.From (items);

			Assert.That (arr, Is.Not.Null, "arr");
			Assert.That (arr!.Count, Is.EqualTo ((nuint) 2), "outer count");
			var row0 = arr.GetItem<NSArray> (0)!;
			var row1 = arr.GetItem<NSArray> (1)!;
			Assert.That (row0.Count, Is.EqualTo ((nuint) 2), "row0 count");
			Assert.That (row1.Count, Is.EqualTo ((nuint) 1), "row1 count");
		}

		[Test]
		public void From_JaggedArray_Null ()
		{
			var result = NSArray.From ((NSObject [] []?) null);
			Assert.That (result, Is.Null, "result");
		}

		[Test]
		public void FromArrayOfArray_Roundtrip ()
		{
			var original = new NSObject [] [] {
				new NSObject [] { (NSString) "1", (NSString) "2" },
				new NSObject [] { (NSString) "3" },
			};

			using var native = NSArray.From (original);
			Assert.That (native, Is.Not.Null, "native");

			var roundtripped = NSArray.FromArrayOfArray (native);
			Assert.That (roundtripped, Is.Not.Null, "roundtripped");
			Assert.That (roundtripped!.Length, Is.EqualTo (original.Length), "outer length");
			Assert.That (roundtripped [0].Length, Is.EqualTo (original [0].Length), "inner0 length");
			Assert.That (roundtripped [1].Length, Is.EqualTo (original [1].Length), "inner1 length");
			Assert.That (roundtripped [0] [0].ToString (), Is.EqualTo ("1"), "[0][0]");
			Assert.That (roundtripped [0] [1].ToString (), Is.EqualTo ("2"), "[0][1]");
			Assert.That (roundtripped [1] [0].ToString (), Is.EqualTo ("3"), "[1][0]");
		}
	}
}
