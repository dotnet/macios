#nullable enable

using System.Collections.Generic;

namespace MonoTouchFixtures.CoreFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ArrayTest {
		static string [] TestArray = new string [] { "a", "b", "??" };

		void VerifyArray (CFArray? a)
		{
			Assert.That (a, Is.Not.Null, "NotNull");
			Assert.That (a.Count, Is.EqualTo ((nint) 3), "Count");
			for (var i = 0; i < a.Count; i++)
				Assert.That ((string) CFString.FromHandle (a.GetValue (i), false), Is.EqualTo (TestArray [i]), i.ToString ());
		}

		void VerifyArray (NSString []? a)
		{
			Assert.That (a, Is.Not.Null, "NotNull");
			Assert.That (a.Length, Is.EqualTo (3), "Count");
			for (var i = 0; i < a.Length; i++)
				Assert.That ((string) a [i], Is.EqualTo (TestArray [i]), i.ToString ());
		}

		void VerifyArray (string []? a)
		{
			Assert.That (a, Is.Not.Null, "NotNull");
			Assert.That (a.Length, Is.EqualTo (3), "Count");
			for (var i = 0; i < a.Length; i++)
				Assert.That ((string) a [i], Is.EqualTo (TestArray [i]), i.ToString ());
		}

		[Test]
		public void CreateTest ()
		{
			var handle = CFArray.Create (TestArray);
			using var a = Runtime.GetINativeObject<CFArray> (handle, true);
			VerifyArray (a);
			Assert.That (CFGetRetainCount (handle), Is.EqualTo ((nint) 1), "RC");
		}

		[Test]
		public void FromStringsTest ()
		{
			using var a = CFArray.FromStrings (TestArray);
			VerifyArray (a);
			Assert.That (CFGetRetainCount (a.Handle), Is.EqualTo ((nint) 1), "RC");
		}

		[Test]
		public void CreateWithNullItemsTest ()
		{
			var handle = CFArray.Create (new string? [] { "a", null, "b" });
			using var a = Runtime.GetINativeObject<CFArray> (handle, true);
			Assert.That (a, Is.Not.Null, "NotNull");
			Assert.That (a!.Count, Is.EqualTo ((nint) 3), "Count");
			Assert.That (CFString.FromHandle (a.GetValue (0), false), Is.EqualTo ("a"), "0");
			Assert.That (a.GetValue (1), Is.EqualTo (NSNull.Null.Handle), "1 - null item is CFNull");
			Assert.That (CFString.FromHandle (a.GetValue (2), false), Is.EqualTo ("b"), "2");
			Assert.That (CFGetRetainCount (handle), Is.EqualTo ((nint) 1), "RC");
		}

		[Test]
		public void CreateWithNativeObjectsTest ()
		{
			using var first = new NSString ("first");
			using var last = new NSString ("last");
			var handle = CFArray.Create (new INativeObject? [] { first, null, last });
			try {
				var values = CFArray.ArrayFromHandle<NSString> (handle);
				Assert.That (values, Is.EqualTo (new NSString? [] { first, null, last }), "Null elements round-trip as NSNull");
				Assert.That (CFGetRetainCount (handle), Is.EqualTo ((nint) 1), "Array is owned by the caller");
				CFRetain (handle);
				CFArray.ReleaseNative (handle);
				Assert.That (CFGetRetainCount (handle), Is.EqualTo ((nint) 1), "ReleaseNative releases one reference");
			} finally {
				CFArray.ReleaseNative (handle);
			}
			CFArray.ReleaseNative (NativeHandle.Zero);
		}

		[Test]
		public void CreateNestedNativeObjectsTest ()
		{
			using var first = new NSString ("first");
			using var last = new NSString ("last");
			var handle = CFArray.Create (new INativeObject [] [] { [first, last], [] });
			try {
				using var array = Runtime.GetINativeObject<CFArray> (handle, false);
				Assert.That (array, Is.Not.Null, "Array");
				ArgumentNullException.ThrowIfNull (array);
				Assert.That (array.Count, Is.EqualTo ((nint) 2), "Rows");
				Assert.That (CFArray.ArrayFromHandle<NSString> (array.GetValue (0)), Is.EqualTo (new [] { first, last }), "First row");
				Assert.That (CFArray.ArrayFromHandle<NSString> (array.GetValue (1)), Is.Empty, "Empty row");
				Assert.That (CFGetRetainCount (array.GetValue (0)), Is.EqualTo ((nint) 1), "Only the outer array retains the row");
			} finally {
				CFArray.ReleaseNative (handle);
			}
		}

		[Test]
		public void CreateNativeObjectMatrixTest ()
		{
			using var first = new NSString ("first");
			using var last = new NSString ("last");
			var handle = CFArray.Create (new INativeObject [,] { { first, last, first }, { last, first, last } });
			try {
				using var array = Runtime.GetINativeObject<CFArray> (handle, false);
				Assert.That (array, Is.Not.Null, "Array");
				ArgumentNullException.ThrowIfNull (array);
				Assert.That (array.Count, Is.EqualTo ((nint) 3), "Rows use the second dimension");
				Assert.That (CFArray.ArrayFromHandle<NSString> (array.GetValue (0)), Is.EqualTo (new [] { first, last }), "Row 0");
				Assert.That (CFArray.ArrayFromHandle<NSString> (array.GetValue (1)), Is.EqualTo (new [] { last, first }), "Row 1");
				Assert.That (CFArray.ArrayFromHandle<NSString> (array.GetValue (2)), Is.EqualTo (new [] { first, last }), "Row 2");
			} finally {
				CFArray.ReleaseNative (handle);
			}
		}

		[Test]
		public void CreateNestedNativeObjectsRejectsNullTest ()
		{
			using var value = new NSObject ();
			var retainCount = CFGetRetainCount (value.Handle);
			var rows = new INativeObject [2] [];
			rows [0] = [value];
			Assert.Throws<ArgumentNullException> (() => CFArray.Create (rows), "Null row");
			Assert.That (CFGetRetainCount (value.Handle), Is.EqualTo (retainCount), "Release rows created before the null row");
			rows [1] = new INativeObject [1];
			Assert.Throws<ArgumentNullException> (() => CFArray.Create (rows), "Null element");
			Assert.That (CFGetRetainCount (value.Handle), Is.EqualTo (retainCount), "Release rows created before the null element");
		}

		[Test]
		public void FromStringsWithNullItemsTest ()
		{
			using var a = CFArray.FromStrings (new string? [] { "x", null, "y" });
			Assert.That (a, Is.Not.Null, "NotNull");
			Assert.That (a.Count, Is.EqualTo ((nint) 3), "Count");
			Assert.That (CFString.FromHandle (a.GetValue (0), false), Is.EqualTo ("x"), "0");
			Assert.That (a.GetValue (1), Is.EqualTo (NSNull.Null.Handle), "1 - null item is CFNull");
			Assert.That (CFString.FromHandle (a.GetValue (2), false), Is.EqualTo ("y"), "2");
		}

		[Test]
		public void CreateWithIReadOnlyListTest ()
		{
			IReadOnlyList<string?> list = new List<string?> { "p", null, "q" };
			var handle = CFArray.Create (list);
			using var a = Runtime.GetINativeObject<CFArray> (handle, true);
			Assert.That (a, Is.Not.Null, "NotNull");
			Assert.That (a!.Count, Is.EqualTo ((nint) 3), "Count");
			Assert.That (CFString.FromHandle (a.GetValue (0), false), Is.EqualTo ("p"), "0");
			Assert.That (a.GetValue (1), Is.EqualTo (NSNull.Null.Handle), "1 - null item is CFNull");
			Assert.That (CFString.FromHandle (a.GetValue (2), false), Is.EqualTo ("q"), "2");
		}

		[Test]
		public void ArrayFromHandleTest ()
		{
			var handle = CFArray.Create (TestArray);
			var a = CFArray.ArrayFromHandle<NSString> (handle);
			VerifyArray (a);
			Assert.That (CFGetRetainCount (handle), Is.EqualTo ((nint) 1), "RC");
			CFRelease (handle);
		}

		[Test]
		public void ArrayFromHandleTest_bool_true ()
		{
			var handle = CFArray.Create (TestArray);
			CFRetain (handle);
			var a = CFArray.ArrayFromHandle<NSString> (handle, true);
			VerifyArray (a);
			Assert.That (CFGetRetainCount (handle), Is.EqualTo ((nint) 1), "RC");
			CFRelease (handle);
		}

		[Test]
		public void ArrayFromHandleTest_bool_false ()
		{
			var handle = CFArray.Create (TestArray);
			var a = CFArray.ArrayFromHandle<NSString> (handle, false);
			VerifyArray (a);
			Assert.That (CFGetRetainCount (handle), Is.EqualTo ((nint) 1), "RC");
			CFRelease (handle);
		}

		[Test]
		public void ArrayFromHandleFuncTest ()
		{
			var handle = CFArray.Create (TestArray);
			var a = CFArray.ArrayFromHandleFunc<string> (handle, (v) => CFString.FromHandle (v));
			VerifyArray (a);
			Assert.That (CFGetRetainCount (handle), Is.EqualTo ((nint) 1), "RC");
			CFRelease (handle);
		}

		[Test]
		public void ArrayFromHandleFuncTest_bool_true ()
		{
			var handle = CFArray.Create (TestArray);
			CFRetain (handle);
			var a = CFArray.ArrayFromHandleFunc<string> (handle, (v) => CFString.FromHandle (v), true);
			VerifyArray (a);
			Assert.That (CFGetRetainCount (handle), Is.EqualTo ((nint) 1), "RC");
			CFRelease (handle);
		}

		[Test]
		public void ArrayFromHandleFuncTest_bool_false ()
		{
			var handle = CFArray.Create (TestArray);
			var a = CFArray.ArrayFromHandleFunc<string> (handle, (v) => CFString.FromHandle (v), false);
			VerifyArray (a);
			Assert.That (CFGetRetainCount (handle), Is.EqualTo ((nint) 1), "RC");
			CFRelease (handle);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		internal extern static nint CFGetRetainCount (IntPtr handle);

		[DllImport (Constants.CoreFoundationLibrary)]
		internal extern static void CFRetain (IntPtr obj);

		[DllImport (Constants.CoreFoundationLibrary)]
		internal extern static void CFRelease (IntPtr obj);
	}
}
