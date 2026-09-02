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
