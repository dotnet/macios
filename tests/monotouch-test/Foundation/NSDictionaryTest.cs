
using Xamarin.Utils;

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSDictionaryTest {

		//
		// Tests for the new NSDictionary from parameters constructors
		//
		[Test]
		public void DictionaryCtorKeyValues ()
		{
			var key = new NSString ("key");
			var value = new NSString ("value");
			var j = new NSDictionary (key, value);

			Assert.That (j.Count, Is.EqualTo ((nuint) 1), "count");
			Assert.That (value, Is.EqualTo (j [key]), "key lookup");

			j = new NSDictionary (new NSString ("first"), new NSString ("first-k"),
								  new NSString ("second"), new NSString ("second-k"));
			Assert.That (j.Count, Is.EqualTo ((nuint) 2), "count");
			Assert.That ((string) (NSString) (j ["first"]), Is.EqualTo ("first-k"), "lookup1");
			Assert.That ((string) (NSString) (j ["second"]), Is.EqualTo ("second-k"), "lookup2");
		}

		[Test]
		public void DictionaryCtorKeyValuesObjects ()
		{
			var j = new NSDictionary ("key", "value");

			Assert.That (j.Count, Is.EqualTo ((nuint) 1), "count");
			Assert.That ((string) (NSString) (j ["key"]), Is.EqualTo ("value"), "key lookup");

			j = new NSDictionary (1, 2, 3, 4);

			Assert.That (j.Count, Is.EqualTo ((nuint) 2), "count");
			Assert.That (((NSNumber) j [new NSNumber (1)]).Int32Value, Is.EqualTo (2), "lookup1");
			Assert.That (((NSNumber) j [new NSNumber (3)]).Int32Value, Is.EqualTo (4), "lookup2");
		}

		[Test]
		public void InbalancedCtor ()
		{
			try {
				var j = new NSDictionary (new NSString ("key"), new NSString ("value"), new NSString ("other"));
			} catch (ArgumentException) {
				return;
			}
			Assert.Fail ("Should have thrown an exception");
		}

		[Test]
		public void InbalancedCtor2 ()
		{
			try {
				var j = new NSDictionary (1, 2, 3);
			} catch (ArgumentException) {
				return;
			}
			Assert.Fail ("Should have thrown an exception");
		}

		[Test]
		public void KeyValue_Autorelease ()
		{
			using (var k = new NSString ("keyz"))
			using (var v = new NSString ("valuez")) {
				var k1 = k.RetainCount;
				if (k1 >= int.MaxValue)
					Assert.Ignore ("RetainCount unusable for testing");
				var k2 = k1;
				Assert.That (k.RetainCount, Is.EqualTo ((nuint) 1), "Key.RetainCount-a");
				var v1 = v.RetainCount;
				var v2 = v1;
				Assert.That (v.RetainCount, Is.EqualTo ((nuint) 1), "Value.RetainCount-a");
				using (var d = new NSDictionary (k, v)) {
					k2 = k.RetainCount;
					Assert.That (k2, Is.GreaterThan (k1), "Key.RetainCount-b");
					v2 = v.RetainCount;
					Assert.That (v2, Is.GreaterThan (v1), "Value.RetainCount-b");

					Assert.That (d.Keys, Is.Not.Null, "Keys");
					// accessing `allKeys` should *NOT* change the retainCount
					// that would happen without an [Autorelease] and can lead to memory exhaustion
					// https://bugzilla.xamarin.com/show_bug.cgi?id=7723
					Assert.That (k.RetainCount, Is.EqualTo (k2), "Key.RetainCount-c");

					Assert.That (d.Values, Is.Not.Null, "Values");
					Assert.That (v.RetainCount, Is.EqualTo (v2), "Value.RetainCount-c");
				}
				Assert.That (k.RetainCount, Is.LessThan (k2), "Key.RetainCount-d");
				Assert.That (v.RetainCount, Is.LessThan (v2), "Value.RetainCount-d");
			}
		}

		[Test]
		public void XForY_Autorelease ()
		{
			using (var k = new NSString ("keyz"))
			using (var v = new NSString ("valuez")) {
				var k1 = k.RetainCount;
				if (k1 >= int.MaxValue)
					Assert.Ignore ("RetainCount unusable for testing");
				var k2 = k1;
				Assert.That (k.RetainCount, Is.EqualTo ((nuint) 1), "Key.RetainCount-a");
				var v1 = v.RetainCount;
				var v2 = v1;
				Assert.That (v.RetainCount, Is.EqualTo ((nuint) 1), "Value.RetainCount-a");
				using (var d = new NSDictionary (k, v)) {
					k2 = k.RetainCount;
					Assert.That (k2, Is.GreaterThan (k1), "Key.RetainCount-b");
					v2 = v.RetainCount;
					Assert.That (v2, Is.GreaterThan (v1), "Value.RetainCount-b");

					var x = d.KeysForObject (v);
					Assert.That (x [0], Is.SameAs (k), "KeysForObject");

					var y = d.ObjectForKey (k);
					Assert.That (y, Is.Not.Null, "ObjectForKey");

					using (var a = new NSMutableArray ()) {
						a.Add (k);
						var z = d.ObjectsForKeys (a, k);
						Assert.That (z [0], Is.SameAs (v), "ObjectsForKeys");
					}

					Assert.That (k.RetainCount, Is.EqualTo (k2), "Key.RetainCount-c");
					Assert.That (v.RetainCount, Is.EqualTo (v2), "Value.RetainCount-c");
				}
				Assert.That (k.RetainCount, Is.LessThan (k2), "Key.RetainCount-d");
				Assert.That (v.RetainCount, Is.LessThan (v2), "Value.RetainCount-d");
			}
		}

		[Test]
		public void FromObjectsAndKeysTest ()
		{
			{
				var keys = new NSObject [] { new NSNumber (1), new NSNumber (2) };
				var objs = new NSObject [] { new NSNumber (1), new NSNumber (4) };
				NSDictionary ns = NSDictionary.FromObjectsAndKeys (objs, keys, 1);
				Console.WriteLine (ns.Count);
				Assert.That (ns.Count, Is.EqualTo ((nuint) 1), "#1");
			}
			{
				var keys = new object [] { 1, 2 };
				var objs = new object [] { 3, 4 };
				NSDictionary ns = NSDictionary.FromObjectsAndKeys (objs, keys, 1);
				Assert.That (ns.Count, Is.EqualTo ((nuint) 1), "#2");
			}
		}

		[Test]
		public void FromObjectsAndKeysTest_NullValue ()
		{
			var keys = new NSObject [] { new NSNumber (1), new NSNumber (2), new NSNumber (3) };
			var objs = new NSObject? [] { new NSNumber (1), null, new NSNumber (4) };
			NSDictionary ns = NSDictionary.FromObjectsAndKeys (objs, keys, 3);
			Assert.That (ns.Count, Is.EqualTo ((nuint) 3), "Count");
			Assert.That (((NSNumber) ns [new NSNumber (1)]).Int32Value, Is.EqualTo (1), "Value 1");
			Assert.That (ns [new NSNumber (2)], Is.InstanceOf<NSNull> (), "Null value");
			Assert.That (((NSNumber) ns [new NSNumber (3)]).Int32Value, Is.EqualTo (4), "Value 3");
		}

		[Test]
		public void FromObjectsAndKeysTest_NullValue_NoCount ()
		{
			var keys = new NSObject [] { new NSNumber (1), new NSNumber (2), new NSNumber (3) };
			var objs = new NSObject? [] { new NSNumber (1), null, new NSNumber (4) };
			NSDictionary ns = NSDictionary.FromObjectsAndKeys (objs, keys);
			Assert.That (ns.Count, Is.EqualTo ((nuint) 3), "Count");
			Assert.That (((NSNumber) ns [new NSNumber (1)]).Int32Value, Is.EqualTo (1), "Value 1");
			Assert.That (ns [new NSNumber (2)], Is.InstanceOf<NSNull> (), "Null value");
			Assert.That (((NSNumber) ns [new NSNumber (3)]).Int32Value, Is.EqualTo (4), "Value 3");
		}

		[Test]
		public void DictionaryCtorKeyValues_WithNull ()
		{
			var key1 = new NSString ("key1");
			var key2 = new NSString ("key2");
			var value = new NSString ("value");

			// Test null value
			var dict = new NSDictionary (key1, null);
			Assert.That (dict.Count, Is.EqualTo ((nuint) 1), "count with null value");
			var rawValue = dict.ObjectForKey (key1);
			Assert.That (rawValue, Is.InstanceOf<NSNull> (), "Null value should be NSNull");

			// Test null in variadic args (value position)
			dict = new NSDictionary (key1, value, key2, null);
			Assert.That (dict.Count, Is.EqualTo ((nuint) 2), "count with null in args");
			rawValue = dict.ObjectForKey (key2);
			Assert.That (rawValue, Is.InstanceOf<NSNull> (), "Null value in args should be NSNull");
		}

		[Test]
		public void Copy ()
		{
			using (var k = new NSString ("key"))
			using (var v = new NSString ("value"))
			using (var d = new NSDictionary (k, v)) {
				// NSObject.Copy works because NSDictionary conforms to NSCopying
				// note: we do not Dispose the "copies" because it's the same instance being returned
				var copy1 = (NSDictionary) d.Copy ();
				Assert.That (copy1, Is.SameAs (d), "1");
				Assert.That (copy1, Is.Not.TypeOf<NSMutableDictionary> (), "NSDictionary-1");
				Assert.That (copy1.Count, Is.EqualTo ((nuint) 1), "Count-1");

				var copy2 = (NSDictionary) d.Copy (null);
				Assert.That (copy2, Is.SameAs (d), "2");
				Assert.That (copy2, Is.Not.TypeOf<NSMutableDictionary> (), "NSDictionary-2");
				Assert.That (copy2.Count, Is.EqualTo ((nuint) 1), "Count-2");

				var copy3 = (NSDictionary) d.Copy (NSZone.Default);
				Assert.That (copy3, Is.SameAs (d), "3");
				Assert.That (copy3, Is.Not.TypeOf<NSMutableDictionary> (), "NSDictionary-3");
				Assert.That (copy3.Count, Is.EqualTo ((nuint) 1), "Count-3");
			}
		}

		[Test]
		public void MutableCopy ()
		{
			using (var k = new NSString ("key"))
			using (var v = new NSString ("value"))
			using (var d = new NSDictionary (k, v)) {
				// NSObject.Copy works because NSDictionary conforms to NSMutableCopying
				using (var copy = (NSDictionary) d.MutableCopy ()) {
					Assert.That (copy, Is.TypeOf<NSMutableDictionary> (), "NSMutableDictionary");
					Assert.That (copy.Count, Is.EqualTo ((nuint) 1), "Count");
				}

				using (var copy = (NSDictionary) d.MutableCopy (null)) {
					Assert.That (copy, Is.TypeOf<NSMutableDictionary> (), "NSMutableDictionary-2");
					Assert.That (copy.Count, Is.EqualTo ((nuint) 1), "Count-2");
				}

				using (var copy = (NSDictionary) d.MutableCopy (NSZone.Default)) {
					Assert.That (copy, Is.TypeOf<NSMutableDictionary> (), "NSMutableDictionary-3");
					Assert.That (copy.Count, Is.EqualTo ((nuint) 1), "Count-3");
				}
			}
		}

		[Test]
		public void IndexerTest ()
		{
			// This test doesn't work on Lion, because Lion returns mutable dictionaries in some places this test asserts that those dictionaries are non-mutable.
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 8, throwIfOtherPlatform: false);
			IntPtr strkeyptr = IntPtr.Zero;
			IntPtr strobjptr = IntPtr.Zero;
			IntPtr objptr;
			IntPtr keyptr;

			NSString v;

			try {
				strkeyptr = Marshal.StringToHGlobalAuto ("key");
				strobjptr = Marshal.StringToHGlobalAuto ("obj");

				// this[string]
				keyptr = Messaging.IntPtr_objc_msgSend_IntPtr (Class.GetHandle (typeof (NSString)), Selector.GetHandle ("stringWithUTF8String:"), strkeyptr);
				objptr = Messaging.IntPtr_objc_msgSend_IntPtr (Class.GetHandle (typeof (NSString)), Selector.GetHandle ("stringWithUTF8String:"), strobjptr);
				using (var dict = Runtime.GetNSObject<NSDictionary> (Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr (Class.GetHandle (typeof (NSDictionary)), Selector.GetHandle ("dictionaryWithObject:forKey:"), objptr, keyptr))) {
					v = (NSString) dict ["key"];
					Assert.That ((string) v, Is.EqualTo ("obj"), "a");

					Assert.Throws<NotSupportedException> (() => dict ["key"] = (NSString) "value", "a ex");
				}

				// this[NSObject]
				keyptr = Messaging.IntPtr_objc_msgSend_IntPtr (Class.GetHandle (typeof (NSString)), Selector.GetHandle ("stringWithUTF8String:"), strkeyptr);
				objptr = Messaging.IntPtr_objc_msgSend_IntPtr (Class.GetHandle (typeof (NSString)), Selector.GetHandle ("stringWithUTF8String:"), strobjptr);
				using (var dict = Runtime.GetNSObject<NSDictionary> (Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr (Class.GetHandle (typeof (NSDictionary)), Selector.GetHandle ("dictionaryWithObject:forKey:"), objptr, keyptr))) {
					v = (NSString) dict [(NSObject) (NSString) "key"];
					Assert.That ((string) v, Is.EqualTo ("obj"), "b");

					Assert.Throws<NotSupportedException> (() => dict [(NSObject) (NSString) "key"] = (NSString) "value", "a ex");
				}

				// this[NSString]
				keyptr = Messaging.IntPtr_objc_msgSend_IntPtr (Class.GetHandle (typeof (NSString)), Selector.GetHandle ("stringWithUTF8String:"), strkeyptr);
				objptr = Messaging.IntPtr_objc_msgSend_IntPtr (Class.GetHandle (typeof (NSString)), Selector.GetHandle ("stringWithUTF8String:"), strobjptr);
				using (var dict = Runtime.GetNSObject<NSDictionary> (Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr (Class.GetHandle (typeof (NSDictionary)), Selector.GetHandle ("dictionaryWithObject:forKey:"), objptr, keyptr))) {
					v = (NSString) dict [(NSString) "key"];
					Assert.That ((string) v, Is.EqualTo ("obj"), "c");

					Assert.Throws<NotSupportedException> (() => dict [(NSString) "key"] = (NSString) "value", "a ex");
				}

			} finally {
				Marshal.FreeHGlobal (strkeyptr);
				Marshal.FreeHGlobal (strobjptr);
			}
		}
	}
}
