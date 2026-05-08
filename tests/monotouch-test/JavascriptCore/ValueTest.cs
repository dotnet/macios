//
// JSValue Unit Tests
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013 Xamarin Inc.
//

using JavaScriptCore;

namespace MonoTouchFixtures.JavascriptCore {

	[TestFixture]
	// we want the test to be availble if we use the linker
	[Preserve (AllMembers = true)]
	[TestFixture]
	public class ValueTest {

		[Test]
		public void From ()
		{
			TestRuntime.AssertXcodeVersion (5, 0, 1);

			using (var c = new JSContext ()) {
				using (var d = JSValue.From (1.0, c)) {
					Assert.That (d.ToDouble (), Is.EqualTo (1.0d), "double");
					Assert.That (c, Is.SameAs (d.Context), "double.Context");
					Assert.That (d.IsNumber, Is.True, "double.IsNumber");
				}
			}
		}

		[Test]
		public void Invoke ()
		{
			TestRuntime.AssertXcodeVersion (5, 0, 1);

			using (var c = new JSContext ()) {
				using (var d = JSValue.From (1.0, c)) {
					Assert.That (d.Invoke ("toString").ToString (), Is.EqualTo ("1"), "toString");
				}

				using (var s1 = JSValue.From ("Hello Xamarin!", c))
				using (var s2 = JSValue.From ("Hello", c))
				using (var s3 = JSValue.From ("Bonjour", c)) {
					Assert.That (s1.Invoke ("replace", s2, s3).ToString (), Is.EqualTo ("Bonjour Xamarin!"), "replace");

					Assert.That (s1.Invoke ("replace", s2, JSValue.Null (c)).ToString (), Is.EqualTo ("null Xamarin!"), "replace-2");
				}
			}
		}

		[Test]
		public void IsEqual ()
		{
			TestRuntime.AssertXcodeVersion (5, 0, 1);

			using (var c = new JSContext ())
			using (var d = JSValue.From (1.0d, c))
			using (var f = JSValue.From (1.0f, c)) {
				Assert.That (d.IsEqualTo (d), Is.True, "=== self");
				Assert.That (d.IsEqualTo (f), Is.True, "=== double/float"); // it's a number now
				Assert.That (d.IsEqualTo ((NSNumber) 1.0d), Is.True, "=== NSNumber");
				Assert.That (d.IsEqualTo ((NSNumber) 2.0d), Is.False, "=== NSNumber-2");

				Assert.That (d.IsEqualWithTypeCoercionTo (d), Is.True, "== self");
				Assert.That (d.IsEqualWithTypeCoercionTo (f), Is.True, "== double/float");
				Assert.That (d.IsEqualWithTypeCoercionTo ((NSNumber) 1.0d), Is.True, "== NSNumber");
				Assert.That (d.IsEqualWithTypeCoercionTo ((NSNumber) 2.0d), Is.False, "== NSNumber-2");
			}
		}

		[Test]
		public void CreatePromise ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);

			using (var c = new JSContext ()) {
				bool called = false;
				var p = JSValue.CreatePromise (c, (resolve, reject) => {
					Assert.That (resolve, Is.Not.Null, "resolve");
					Assert.That (reject, Is.Not.Null, "reject");
					called = true;
				});
				Assert.That (called, Is.True, "called");
			}

		}

		[Test]
		public void ToArray ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);

			using var context = new JSContext ();
			using var array = NSArray.FromStrings ("a", "b");
			using var value = JSValue.From (array, context);
			using var arr2 = value.ToArray ();
			Assert.That (arr2.GetItem<NSString> (0).ToString (), Is.EqualTo ("a"), "a");
			Assert.That (arr2.GetItem<NSString> (1).ToString (), Is.EqualTo ("b"), "a");
		}
	}
}
