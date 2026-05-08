//
// Unit tests for NSArray
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

using System.Collections.Generic;
using System.Linq;
using Security;

#nullable enable

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSArrayTest {

		[Test]
		public void FromStrings_Null ()
		{
			Assert.Throws<ArgumentNullException> (() => NSArray.FromStrings (null), "null");

			using (var a = NSArray.FromStrings (new string [1])) {
				Assert.That (a.Count, Is.EqualTo ((nuint) 1), "null item");
				Assert.That (a.GetItem<NSString> (0), Is.Null, "0");
			}
		}

		[Test]
		public void FromStrings_WithNullItems ()
		{
			using (var a = NSArray.FromStrings (new string? [] { "a", null, "b" })) {
				Assert.That (a.Count, Is.EqualTo ((nuint) 3), "Count");
				Assert.That (a.GetItem<NSString> (0)?.ToString (), Is.EqualTo ("a"), "0");
				Assert.That (a.GetItem<NSString> (1), Is.Null, "1 - null item");
				Assert.That (a.GetItem<NSString> (2)?.ToString (), Is.EqualTo ("b"), "2");
			}
		}

		[Test]
		public void FromStrings_IReadOnlyList ()
		{
			IReadOnlyList<string?> list = new List<string?> { "x", null, "y" };
			using (var a = NSArray.FromStrings (list)) {
				Assert.That (a.Count, Is.EqualTo ((nuint) 3), "Count");
				Assert.That (a.GetItem<NSString> (0)?.ToString (), Is.EqualTo ("x"), "0");
				Assert.That (a.GetItem<NSString> (1), Is.Null, "1 - null item");
				Assert.That (a.GetItem<NSString> (2)?.ToString (), Is.EqualTo ("y"), "2");
			}
		}

		[Test]
		public void FromNullableStrings_Null ()
		{
			Assert.That (NSArray.FromNullableStrings (null), Is.Null, "null returns null");
		}

		[Test]
		public void FromNullableStrings_WithValues ()
		{
			using (var a = NSArray.FromNullableStrings (new string? [] { "hello", null, "world" })) {
				Assert.That (a, Is.Not.Null, "not null");
				Assert.That (a!.Count, Is.EqualTo ((nuint) 3), "Count");
				Assert.That (a.GetItem<NSString> (0)?.ToString (), Is.EqualTo ("hello"), "0");
				Assert.That (a.GetItem<NSString> (1), Is.Null, "1 - null item");
				Assert.That (a.GetItem<NSString> (2)?.ToString (), Is.EqualTo ("world"), "2");
			}
		}

		[Test]
		public void FromNullableStrings_Empty ()
		{
			using (var a = NSArray.FromNullableStrings (Array.Empty<string?> ())) {
				Assert.That (a, Is.Not.Null, "not null");
				Assert.That (a!.Count, Is.EqualTo ((nuint) 0), "Count");
			}
		}

		[Test]
		public void Null ()
		{
			using (var a = NSArray.FromNSObjects (NSNull.Null)) {
				Assert.That (a.Count, Is.EqualTo ((nuint) 1), "Count");
				Assert.That (a.GetItem<NSNull> (0), Is.Null, "0");
			}
		}

		int comparator_count;

		// the new NSObject are often, but not always, in ascending order 
		// (because of how we allocate them) so we sort the other way
		NSComparisonResult Comparator (NSObject obj1, NSObject obj2)
		{
			comparator_count++;
			return (NSComparisonResult) (((long) (IntPtr) obj2.Handle - (long) (IntPtr) obj1.Handle));
		}

		[Test]
		public void Sort ()
		{
			comparator_count = 0;
			using (var obj1 = new NSObject ())
			using (var obj2 = new NSObject ())
			using (var a = new NSMutableArray ()) {
				a.Add (obj1);
				a.Add (a);
				a.Add (obj2);
				using (var s = a.Sort (Comparator)) {
					Assert.That ((long) (IntPtr) s.ValueAt (0), Is.GreaterThan ((long) (IntPtr) s.ValueAt (1)), "0");
					Assert.That ((long) (IntPtr) s.ValueAt (1), Is.GreaterThan ((long) (IntPtr) s.ValueAt (2)), "1");
				}
			}
			Assert.That (comparator_count, Is.GreaterThanOrEqualTo (2), "2+");
		}

		int evaluator_count;

		bool Evaluator (NSObject evaluatedObject, NSDictionary bindings)
		{
			evaluator_count++;
			return (evaluatedObject is NSMutableArray);
		}

		[Test]
		public void Filter ()
		{
			TestRuntime.AssertNotInterpreter ("This test does not work in the interpreter: https://github.com/dotnet/runtime/issues/110649");

			evaluator_count = 0;
			using (var obj1 = new NSObject ())
			using (var obj2 = new NSObject ())
			using (var a = new NSMutableArray ()) {
				a.Add (obj1);
				a.Add (a);
				a.Add (obj2);
				using (NSPredicate p = NSPredicate.FromExpression (Evaluator))
				using (var f = a.Filter (p)) {
					Assert.That (f.ValueAt (0), Is.EqualTo (a.Handle), "0");
					Assert.That (f.Count, Is.EqualTo ((nuint) 1), "Count");
				}
			}
			Assert.That (evaluator_count, Is.EqualTo (3), "3");
		}

		[Test]
		public void INativeObjects ()
		{
			using (var policy = SecPolicy.CreateSslPolicy (true, "mail.xamarin.com")) {
				using (var a = NSArray.FromObjects (policy)) {
					var b = NSArray.ArrayFromHandle<SecPolicy> (a.Handle);
					Assert.That (b, Is.Not.SameAs (a));
				}
			}
		}

		[Test]
		public void FromNSObjects ()
		{
			using (var a = NSArray.FromNSObjects (null)) {
				// on the managed side we have an empty array
				Assert.That (a.Count, Is.EqualTo ((nuint) 0), "Count");
				// and a valid native instance (or some other API might fail)
				Assert.That (a.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");
			}
		}

		[Test]
		public void ToArray ()
		{
			using (var a = NSArray.FromStrings (new string [1] { "abc" })) {
				var arr = a.ToArray ();
				Assert.That (arr.Length, Is.EqualTo (1), "Length");
				Assert.That (arr [0].ToString (), Is.EqualTo ("abc"), "Value");
			}
		}

		[Test]
		public void ToArray_T ()
		{
			using (var a = NSArray.FromStrings (new string [1] { "abc" })) {
				var arr = a.ToArray<NSString> ();
				Assert.That (arr.Length, Is.EqualTo (1), "Length");
				Assert.That (arr [0].ToString (), Is.EqualTo ("abc"), "Value");
			}
		}

		[Test]
		public void Enumerator ()
		{
			using (var a = NSArray.FromStrings (new string [1] { "abc" })) {
				foreach (var item in a)
					Assert.That (item.ToString (), Is.EqualTo ("abc"), "Value");
				var list = a.ToList ();
				Assert.That (list.Count (), Is.EqualTo (1), "Length");
				Assert.That (list [0].ToString (), Is.EqualTo ("abc"), "Value");
			}
		}
	}
}
