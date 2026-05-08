using System.Collections;
using System.Linq;

using Xamarin.Utils;

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSMutableDictionary2Test {

		[Test]
		public void Ctor ()
		{
			var dict = new NSMutableDictionary<NSDate, NSSet> ();
			ClassicAssert.AreEqual ((nuint) 0, dict.Count, "Count");
		}

		[Test]
		public void Ctor_NSDictionary ()
		{
			var other = new NSDictionary<NSString, NSString> ((NSString) "key", (NSString) "value");
			var j = new NSMutableDictionary<NSString, NSString> (other);

			ClassicAssert.AreEqual (j.Count, (nuint) 1, "count");
			ClassicAssert.AreEqual ((string) (NSString) (j [(NSString) "key"]), "value", "key lookup");
		}

		[Test]
		public void Ctor_NSMutableDictionary ()
		{
			var other = new NSMutableDictionary<NSString, NSString> ();
			other.Add ((NSString) "key", (NSString) "value");
			var j = new NSMutableDictionary<NSString, NSString> (other);

			ClassicAssert.AreEqual (j.Count, (nuint) 1, "count");
			ClassicAssert.AreEqual ((string) (NSString) (j [(NSString) "key"]), "value", "key lookup");
		}

		[Test]
		public void FromObjectsAndKeysGenericTest ()
		{
			var keys = new [] {
				new NSString ("Key1"),
				new NSString ("Key2"),
				new NSString ("Key3"),
				new NSString ("Key4"),
				new NSString ("Key5"),
			};
			var values = new [] {
				NSNumber.FromByte (0x1),
				NSNumber.FromFloat (8.5f),
				NSNumber.FromDouble (10.5),
				NSNumber.FromInt32 (42),
				NSNumber.FromBoolean (true),
			};

			var dict = NSMutableDictionary<NSString, NSNumber>.FromObjectsAndKeys (values, keys, values.Length);
			ClassicAssert.AreEqual (dict.Count, (nuint) 5, "count");
			for (int i = 0; i < values.Length; i++)
				ClassicAssert.AreEqual (dict [keys [i]], values [i], $"key lookup, Iteration: {i}");
		}

		[Test]
		public void Ctor_WithNullValue ()
		{
			var key = (NSString) "key";
			using (var dict = new NSMutableDictionary<NSString, NSString> (key, null)) {
				ClassicAssert.AreEqual ((nuint) 1, dict.Count, "count");
				var baseDict = (NSDictionary) dict;
				var rawValue = baseDict.ObjectForKey (key);
				ClassicAssert.IsInstanceOf<NSNull> (rawValue, "Null value should be NSNull");
			}
		}

		[Test]
		public void FromObjectsAndKeys_Generic_WithNull ()
		{
			var keys = new NSString [] { (NSString) "key1", (NSString) "key2" };
			var values = new NSString? [] { (NSString) "value1", null };

			using (var dict = NSMutableDictionary<NSString, NSString>.FromObjectsAndKeys (values, keys)) {
				ClassicAssert.IsNotNull (dict, "Dictionary should not be null");
				ClassicAssert.AreEqual ((nuint) 2, dict!.Count, "Count");
				ClassicAssert.AreEqual ("value1", dict [keys [0]].ToString (), "First value");
				var baseDict = (NSDictionary) dict;
				var rawValue = baseDict.ObjectForKey (keys [1]);
				ClassicAssert.IsInstanceOf<NSNull> (rawValue, "Null value should be NSNull");
			}
		}

		[Test]
		public void FromObjectsAndKeys_Generic_WithCount_WithNull ()
		{
			var keys = new NSString [] { (NSString) "key1", (NSString) "key2", (NSString) "key3" };
			var values = new NSString? [] { (NSString) "value1", null, (NSString) "value3" };

			using (var dict = NSMutableDictionary<NSString, NSString>.FromObjectsAndKeys (values, keys, 2)) {
				ClassicAssert.IsNotNull (dict, "Dictionary should not be null");
				ClassicAssert.AreEqual ((nuint) 2, dict!.Count, "Count");
				ClassicAssert.AreEqual ("value1", dict [keys [0]].ToString (), "First value");
				var baseDict = (NSDictionary) dict;
				var rawValue = baseDict.ObjectForKey (keys [1]);
				ClassicAssert.IsInstanceOf<NSNull> (rawValue, "Null value should be NSNull");
			}
		}

		[Test]
		public void FromObjectsAndKeys_Object_WithCount ()
		{
			var keys = new object [] { "key1", "key2", "key3" };
			var objs = new object [] { "value1", "value2", "value3" };

			using (var dict = NSMutableDictionary<NSString, NSString>.FromObjectsAndKeys (objs, keys, 2)) {
				ClassicAssert.IsNotNull (dict, "Dictionary should not be null");
				ClassicAssert.AreEqual ((nuint) 2, dict!.Count, "Count");
				ClassicAssert.AreEqual ("value1", dict [(NSString) "key1"].ToString (), "First value");
				ClassicAssert.AreEqual ("value2", dict [(NSString) "key2"].ToString (), "Second value");
			}
		}

		[Test]
		public void FromObjectsAndKeys_NSObject_WithCount_WithNull ()
		{
			var keys = new NSObject [] { new NSString ("key1"), new NSString ("key2"), new NSString ("key3") };
			var objs = new NSObject? [] { new NSString ("value1"), null, new NSString ("value3") };

			using (var dict = NSMutableDictionary<NSString, NSString>.FromObjectsAndKeys (objs, keys, 2)) {
				ClassicAssert.IsNotNull (dict, "Dictionary should not be null");
				ClassicAssert.AreEqual ((nuint) 2, dict!.Count, "Count");
				ClassicAssert.AreEqual ("value1", dict [(NSString) keys [0]].ToString (), "First value");
				var baseDict = (NSDictionary) dict;
				var rawValue = baseDict.ObjectForKey (keys [1]);
				ClassicAssert.IsInstanceOf<NSNull> (rawValue, "Null value should be NSNull");
			}
		}

		[Test]
		public void FromObjectsAndKeys_NSObject_WithCount ()
		{
			var keys = new NSObject [] { new NSString ("key1"), new NSString ("key2"), new NSString ("key3") };
			var objs = new NSObject [] { new NSString ("value1"), new NSString ("value2"), new NSString ("value3") };

			using (var dict = NSMutableDictionary<NSString, NSString>.FromObjectsAndKeys (objs, keys, 2)) {
				ClassicAssert.IsNotNull (dict, "Dictionary should not be null");
				ClassicAssert.AreEqual ((nuint) 2, dict!.Count, "Count");
				ClassicAssert.AreEqual ("value1", dict [(NSString) keys [0]].ToString (), "First value");
				ClassicAssert.AreEqual ("value2", dict [(NSString) keys [1]].ToString (), "Second value");
			}
		}

		[Test]
		public void FromObjectsAndKeys_Generic_WithCountZero ()
		{
			var keys = new NSString [] { (NSString) "key1", (NSString) "key2" };
			var values = new NSString [] { (NSString) "value1", (NSString) "value2" };

			using (var dict = NSMutableDictionary<NSString, NSString>.FromObjectsAndKeys (values, keys, 0)) {
				ClassicAssert.IsNotNull (dict, "Dictionary should not be null");
				ClassicAssert.AreEqual ((nuint) 0, dict!.Count, "Count should be 0");
			}
		}

		[Test]
		public void FromObjectsAndKeys_DifferentArrayLengths_WithCount ()
		{
			var keys = new NSString [] { (NSString) "key1", (NSString) "key2" };
			var values = new NSString [] { (NSString) "value1", (NSString) "value2", (NSString) "value3", (NSString) "value4" };

			// Should work fine since we only use first 2 items from each array
			using (var dict = NSMutableDictionary<NSString, NSString>.FromObjectsAndKeys (values, keys, 2)) {
				ClassicAssert.IsNotNull (dict, "Dictionary should not be null");
				ClassicAssert.AreEqual ((nuint) 2, dict!.Count, "Count");
				ClassicAssert.AreEqual ("value1", dict [keys [0]].ToString (), "First value");
				ClassicAssert.AreEqual ("value2", dict [keys [1]].ToString (), "Second value");
			}
		}

		[Test]
		public void FromObjectsAndKeys_CountLargerThanKeys ()
		{
			var keys = new NSString [] { (NSString) "key1", (NSString) "key2" };
			var values = new NSString [] { (NSString) "value1", (NSString) "value2", (NSString) "value3" };

			// Should throw because count > keys.Length
			Assert.Throws<ArgumentException> (() => NSMutableDictionary<NSString, NSString>.FromObjectsAndKeys (values, keys, 3), "Should throw when count > keys.Length");
		}

		[Test]
		public void FromObjectsAndKeys_CountLargerThanValues ()
		{
			var keys = new NSString [] { (NSString) "key1", (NSString) "key2", (NSString) "key3" };
			var values = new NSString [] { (NSString) "value1", (NSString) "value2" };

			// Should throw because count > values.Length
			Assert.Throws<ArgumentException> (() => NSMutableDictionary<NSString, NSString>.FromObjectsAndKeys (values, keys, 3), "Should throw when count > values.Length");
		}

		[Test]
		public void FromObjectsAndKeys_NegativeCount ()
		{
			var keys = new NSString [] { (NSString) "key1", (NSString) "key2" };
			var values = new NSString [] { (NSString) "value1", (NSString) "value2" };

			// Should throw for negative count
			Assert.Throws<ArgumentOutOfRangeException> (() => NSMutableDictionary<NSString, NSString>.FromObjectsAndKeys (values, keys, -1), "Should throw for negative count");
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
				using (var d = new NSMutableDictionary<NSString, NSString> (k, v)) {
					k2 = k.RetainCount;
					Assert.That (k2, Is.GreaterThan (k1), "Key.RetainCount-b");
					v2 = v.RetainCount;
					Assert.That (v2, Is.GreaterThan (v1), "Value.RetainCount-b");

					ClassicAssert.NotNull (d.Keys, "Keys");
					// accessing `allKeys` should *NOT* change the retainCount
					// that would happen without an [Autorelease] and can lead to memory exhaustion
					// https://bugzilla.xamarin.com/show_bug.cgi?id=7723
					Assert.That (k.RetainCount, Is.EqualTo (k2), "Key.RetainCount-c");

					ClassicAssert.NotNull (d.Values, "Values");
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
				using (var d = new NSMutableDictionary<NSString, NSString> (k, v)) {
					k2 = k.RetainCount;
					Assert.That (k2, Is.GreaterThan (k1), "Key.RetainCount-b");
					v2 = v.RetainCount;
					Assert.That (v2, Is.GreaterThan (v1), "Value.RetainCount-b");

					var x = d.KeysForObject (v);
					Assert.That (x [0], Is.SameAs (k), "KeysForObject");

					var y = d.ObjectForKey (k);
					ClassicAssert.NotNull (y, "ObjectForKey");

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
		public void Copy ()
		{
			using (var k = new NSString ("key"))
			using (var v = new NSString ("value"))
			using (var d = new NSMutableDictionary<NSString, NSString> (k, v)) {
				// NSObject.Copy works because NSDictionary conforms to NSCopying
				using (var copy1 = (NSDictionary) d.Copy ()) {
					ClassicAssert.AreNotSame (d, copy1, "1");
					Assert.That (copy1, Is.Not.TypeOf<NSMutableDictionary> (), "NSDictionary-1");
					Assert.That (copy1.Count, Is.EqualTo ((nuint) 1), "Count-1");
				}

				using (var copy2 = (NSDictionary) d.Copy (null)) {
					ClassicAssert.AreNotSame (d, copy2, "2");
					Assert.That (copy2, Is.Not.TypeOf<NSMutableDictionary> (), "NSDictionary-2");
					Assert.That (copy2.Count, Is.EqualTo ((nuint) 1), "Count-2");
				}

				using (var copy3 = (NSDictionary) d.Copy (NSZone.Default)) {
					ClassicAssert.AreNotSame (d, copy3, "3");
					Assert.That (copy3, Is.Not.TypeOf<NSMutableDictionary> (), "NSDictionary-3");
					Assert.That (copy3.Count, Is.EqualTo ((nuint) 1), "Count-3");
				}
			}
		}

		[Test]
		public void MutableCopy ()
		{
			using (var k = new NSString ("key"))
			using (var v = new NSString ("value"))
			using (var d = new NSMutableDictionary<NSString, NSString> (k, v)) {
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
		public void ObjectForKeyTest ()
		{
			var value = NSDate.FromTimeIntervalSinceNow (23);
			var key = new NSString ("right key");
			var dict = new NSMutableDictionary<NSString, NSDate> (key, value);

			Assert.Throws<ArgumentNullException> (() => dict.ObjectForKey ((NSString) null), "ANE");
			ClassicAssert.AreSame (value, dict.ObjectForKey (key), "right");
			ClassicAssert.IsNull (dict.ObjectForKey ((NSString) "wrong key"), "wrong");
		}

		[Test]
		public void KeysTest ()
		{
			var value = NSDate.FromTimeIntervalSinceNow (23);
			var key = new NSString ("right key");
			var dict = new NSMutableDictionary<NSString, NSDate> (key, value);

			var keys = dict.Keys;
			ClassicAssert.AreEqual (1, keys.Length, "Length");
			ClassicAssert.AreSame (key, keys [0], "1");
		}

		[Test]
		public void KeysForObjectTest ()
		{
			var value1 = NSDate.FromTimeIntervalSinceNow (1);
			var value2 = NSDate.FromTimeIntervalSinceNow (2);
			var value3 = NSDate.FromTimeIntervalSinceNow (3);
			var key1 = new NSString ("key1");
			var key2 = new NSString ("key2");
			var key3 = new NSString ("key3");

			var dict = new NSMutableDictionary<NSString, NSDate> (
				new NSString [] { key1, key2, key3 },
				new NSDate [] { value1, value1, value2 }
			);

			var rv = dict.KeysForObject (value1);
			ClassicAssert.AreEqual (2, rv.Length, "v1");

			rv = dict.KeysForObject (value2);
			ClassicAssert.AreEqual (1, rv.Length, "v2");
			ClassicAssert.AreSame (key3, rv [0], "v2 key");

			rv = dict.KeysForObject (value3);
			ClassicAssert.AreEqual (0, rv.Length, "v3");

			Assert.Throws<ArgumentNullException> (() => dict.KeysForObject (null), "ANE");
		}

		[Test]
		public void ValuesTest ()
		{
			var value = NSDate.FromTimeIntervalSinceNow (23);
			var key = new NSString ("right key");
			var dict = new NSMutableDictionary<NSString, NSDate> (key, value);

			var keys = dict.Values;
			ClassicAssert.AreEqual (1, dict.Values.Length, "Length");
			ClassicAssert.AreSame (value, dict [key], "1");
		}

		[Test]
		public void ObjectsForKeysTest ()
		{
			var value1 = NSDate.FromTimeIntervalSinceNow (1);
			var value2 = NSDate.FromTimeIntervalSinceNow (2);
			var value3 = NSDate.FromTimeIntervalSinceNow (3);
			var key1 = new NSString ("key1");
			var key2 = new NSString ("key2");
			var key3 = new NSString ("key3");
			var key4 = new NSString ("key4");

			var dict = new NSMutableDictionary<NSString, NSDate> (
				new NSString [] { key1, key2, key3 },
				new NSDate [] { value1, value1, value2 }
			);

			var rv = dict.ObjectsForKeys (new NSString [] { key1, key4 }, value3);
			ClassicAssert.AreEqual (2, rv.Length, "a");
			ClassicAssert.AreSame (value1, rv [0], "a 0");
			ClassicAssert.AreSame (value3, rv [1], "a 1");

			rv = dict.ObjectsForKeys (new NSString [] { }, value3);
			ClassicAssert.AreEqual (0, rv.Length, "b length");

			Assert.Throws<ArgumentNullException> (() => dict.ObjectsForKeys ((NSString []) null, value3), "c");
			Assert.Throws<ArgumentNullException> (() => dict.ObjectsForKeys (new NSString [] { }, null), "d");
		}

		[Test]
		public void ContainsKeyTest ()
		{
			var value1 = NSDate.FromTimeIntervalSinceNow (1);
			var value2 = NSDate.FromTimeIntervalSinceNow (2);
			var value3 = NSDate.FromTimeIntervalSinceNow (3);
			var key1 = new NSString ("key1");
			var key2 = new NSString ("key2");
			var key3 = new NSString ("key3");

			var dict = new NSMutableDictionary<NSString, NSDate> (
				new NSString [] { key1, key2 },
				new NSDate [] { value1, value1 }
			);

			ClassicAssert.True (dict.ContainsKey (key1), "a");
			ClassicAssert.False (dict.ContainsKey (key3), "b");
			Assert.Throws<ArgumentNullException> (() => dict.ContainsKey ((NSString) null), "ANE");
		}

		[Test]
		public void TryGetValueTest ()
		{
			var value1 = NSDate.FromTimeIntervalSinceNow (1);
			var value2 = NSDate.FromTimeIntervalSinceNow (2);
			var value3 = NSDate.FromTimeIntervalSinceNow (3);
			var key1 = new NSString ("key1");
			var key2 = new NSString ("key2");
			var key3 = new NSString ("key3");

			var dict = new NSMutableDictionary<NSString, NSDate> (
				new NSString [] { key1, key2 },
				new NSDate [] { value1, value1 }
			);

			NSDate value;

			ClassicAssert.True (dict.TryGetValue (key1, out value), "a");
			ClassicAssert.AreSame (value1, value, "a same");

			ClassicAssert.False (dict.TryGetValue (key3, out value), "b");
			ClassicAssert.IsNull (value, "b null");
		}

		[Test]
		public void IndexerTest ()
		{
			var value1 = NSDate.FromTimeIntervalSinceNow (1);
			var value2 = NSDate.FromTimeIntervalSinceNow (2);
			var value3 = NSDate.FromTimeIntervalSinceNow (3);
			var key1 = new NSString ("key1");
			var key2 = new NSString ("key2");
			var key3 = new NSString ("key3");

			var dict = new NSMutableDictionary<NSString, NSDate> (
				new NSString [] { key1, key2 },
				new NSDate [] { value1, value1 }
			);

			ClassicAssert.AreSame (value1, dict [key1], "a");
			ClassicAssert.IsNull (dict [key3], "b");
			Assert.Throws<ArgumentNullException> (() => GC.KeepAlive (dict [(NSString) null]), "c");
		}

		[Test]
		public void IndexerGetterKeyNotFoundBehaviorTest ()
		{
			var value1 = NSDate.FromTimeIntervalSinceNow (1);
			var key1 = new NSString ("key1");
			var keyMissing = new NSString ("missing");

			var dict = new NSMutableDictionary<NSString, NSDate> (key1, value1);

			// Accessing via the indexer property should return null
			ClassicAssert.IsNull (dict [keyMissing], "missing key");

			// Accessing via IDictionary interface should return null too
			IDictionary<NSString, NSDate> idict = dict;
			ClassicAssert.IsNull (idict [keyMissing], "missing key via interface");
		}

		[Test]
		public void MissingKeyAccessTest ()
		{
			var value1 = NSDate.FromTimeIntervalSinceNow (1);
			var value2 = NSDate.FromTimeIntervalSinceNow (2);
			var key1 = new NSString ("key1");
			var key2 = new NSString ("key2");
			var keyMissing = new NSString ("missing");

			var dict = new NSMutableDictionary<NSString, NSDate> (
				new NSString [] { key1, key2 },
				new NSDate [] { value1, value2 }
			);

			// ObjectForKey should return null for missing keys
			ClassicAssert.IsNull (dict.ObjectForKey (keyMissing), "ObjectForKey missing");

			// TryGetValue should return false for missing keys
			NSDate value;
			ClassicAssert.IsFalse (dict.TryGetValue (keyMissing, out value), "TryGetValue missing");
			ClassicAssert.IsNull (value, "TryGetValue out value");

			// ContainsKey should return false for missing keys
			ClassicAssert.IsFalse (dict.ContainsKey (keyMissing), "ContainsKey missing");

			// Indexer getter should return null
			ClassicAssert.IsNull (dict [keyMissing], "Indexer missing");

			// IDictionary indexer should also return null
			IDictionary<NSString, NSDate> idict = dict;
			ClassicAssert.IsNull (idict [keyMissing], "IDictionary indexer missing");
		}

		[Test]
		public void EmptyDictionaryMissingKeyTest ()
		{
			var dict = new NSMutableDictionary<NSString, NSDate> ();
			var keyMissing = new NSString ("missing");

			// All access methods should handle missing keys in empty dictionary
			ClassicAssert.IsNull (dict.ObjectForKey (keyMissing), "ObjectForKey");
			ClassicAssert.IsFalse (dict.ContainsKey (keyMissing), "ContainsKey");

			NSDate value;
			ClassicAssert.IsFalse (dict.TryGetValue (keyMissing, out value), "TryGetValue");
			ClassicAssert.IsNull (value, "TryGetValue out");

			ClassicAssert.IsNull (dict [keyMissing], "Indexer");

			IDictionary<NSString, NSDate> idict = dict;
			ClassicAssert.IsNull (idict [keyMissing], "IDictionary indexer");
		}

		[Test]
		public void ObjectsForKeysMissingKeysTest ()
		{
			var value1 = NSDate.FromTimeIntervalSinceNow (1);
			var value2 = NSDate.FromTimeIntervalSinceNow (2);
			var marker = NSDate.FromTimeIntervalSinceNow (999);
			var key1 = new NSString ("key1");
			var key2 = new NSString ("key2");
			var keyMissing1 = new NSString ("missing1");
			var keyMissing2 = new NSString ("missing2");

			var dict = new NSMutableDictionary<NSString, NSDate> (
				new NSString [] { key1, key2 },
				new NSDate [] { value1, value2 }
			);

			// Request mix of existing and missing keys - marker should replace missing values
			var result = dict.ObjectsForKeys (new NSString [] { key1, keyMissing1, key2, keyMissing2 }, marker);
			ClassicAssert.AreEqual (4, result.Length, "Length");
			ClassicAssert.AreSame (value1, result [0], "0 - existing");
			ClassicAssert.AreSame (marker, result [1], "1 - missing");
			ClassicAssert.AreSame (value2, result [2], "2 - existing");
			ClassicAssert.AreSame (marker, result [3], "3 - missing");

			// Request all missing keys
			result = dict.ObjectsForKeys (new NSString [] { keyMissing1, keyMissing2 }, marker);
			ClassicAssert.AreEqual (2, result.Length, "All missing length");
			ClassicAssert.AreSame (marker, result [0], "All missing 0");
			ClassicAssert.AreSame (marker, result [1], "All missing 1");
		}

		[Test]
		public void IDictionary2Test ()
		{
			var value1 = NSDate.FromTimeIntervalSinceNow (1);
			var value2 = NSDate.FromTimeIntervalSinceNow (2);
			var value3 = NSDate.FromTimeIntervalSinceNow (3);
			var key1 = new NSString ("key1");
			var key2 = new NSString ("key2");
			var key3 = new NSString ("key3");

			var dictobj = new NSMutableDictionary<NSString, NSDate> (
				new NSString [] { key1, key2 },
				new NSDate [] { value1, value1 }
			);

			var dict = (IDictionary<NSString, NSDate>) dictobj;

			// Add
			Assert.Throws<ArgumentNullException> (() => dict.Add (new KeyValuePair<NSString, NSDate> (null, value1)), "Add ANE 1");
			Assert.Throws<ArgumentNullException> (() => dict.Add (new KeyValuePair<NSString, NSDate> (key1, null)), "Add ANE 2");
			dict.Add (new KeyValuePair<NSString, NSDate> (key3, value3));
			ClassicAssert.AreSame (value3, dictobj [key3], "Add 1");
			ClassicAssert.AreEqual (3, dict.Count, "Add Count");
			dictobj.Remove (key3); // restore state.

			// Clear
			dict.Clear ();
			ClassicAssert.AreEqual (0, dict.Count, "Clear Count");
			dictobj.Add (key1, value1); // restore state
			dictobj.Add (key2, value1); // restore state

			// Contains
			ClassicAssert.IsTrue (dict.Contains (new KeyValuePair<NSString, NSDate> (key1, value1)), "Contains 1"); // both key and value matches
			ClassicAssert.IsFalse (dict.Contains (new KeyValuePair<NSString, NSDate> (key1, value2)), "Contains 2"); // found key, wrong value
			ClassicAssert.IsFalse (dict.Contains (new KeyValuePair<NSString, NSDate> (key3, value2)), "Contains 3"); // wrong key

			// ContainsKey
			ClassicAssert.IsTrue (dict.ContainsKey (key1), "ContainsKey 1");
			ClassicAssert.IsFalse (dict.ContainsKey (key3), "ContainsKey 2");

			// CopyTo
			var kvp_array = new KeyValuePair<NSString, NSDate> [1];
			Assert.Throws<ArgumentNullException> (() => dict.CopyTo (null, 0), "CopyTo ANE");
			Assert.Throws<ArgumentOutOfRangeException> (() => dict.CopyTo (kvp_array, -1), "CopyTo AOORE");
			Assert.Throws<ArgumentException> (() => dict.CopyTo (kvp_array, kvp_array.Length), "CopyTo AE 2");
			Assert.Throws<ArgumentException> (() => dict.CopyTo (kvp_array, 0), "CopyTo AE 3");

			kvp_array = new KeyValuePair<NSString, NSDate> [dictobj.Count];

			Assert.Throws<ArgumentException> (() => dict.CopyTo (kvp_array, 1), "CopyTo AE 4");
			dict.CopyTo (kvp_array, 0);
			Assert.That (key1, Is.SameAs (kvp_array [0].Key).Or.SameAs (kvp_array [1].Key), "CopyTo K1");
			ClassicAssert.AreSame (value1, kvp_array [0].Value, "CopyTo V1");
			Assert.That (key2, Is.SameAs (kvp_array [0].Key).Or.SameAs (kvp_array [1].Key), "CopyTo K2");
			ClassicAssert.AreSame (value1, kvp_array [1].Value, "CopyTo V2");

			// Count
			ClassicAssert.AreEqual (2, dict.Count, "Count");

			// GetEnumerator
			var enumerated = Enumerable.ToArray (dict);
			ClassicAssert.AreEqual (2, enumerated.Length, "Enumerator Count");

			// IsReadOnly
			ClassicAssert.IsFalse (dict.IsReadOnly, "IsReadOnly");

			// Keys
			ClassicAssert.AreEqual (2, dict.Keys.Count, "Keys Count");

			// Remove
			Assert.Throws<ArgumentNullException> (() => dict.Remove (new KeyValuePair<NSString, NSDate> (null, value3)), "Remove ANE 1");
			Assert.Throws<ArgumentNullException> (() => dict.Remove (new KeyValuePair<NSString, NSDate> (key3, null)), "Remove ANE 2");
			ClassicAssert.IsFalse (dict.Remove (new KeyValuePair<NSString, NSDate> (key3, value3)), "Remove 1"); // inexistent key
			ClassicAssert.AreEqual (2, dict.Count, "Remove 1 Count");

			ClassicAssert.IsFalse (dict.Remove (new KeyValuePair<NSString, NSDate> (key1, value2)), "Remove 2"); // existing key, wrong value
			ClassicAssert.AreEqual (2, dict.Count, "Remove 2 Count");

			ClassicAssert.IsTrue (dict.Remove (new KeyValuePair<NSString, NSDate> (key1, value1)), "Remove 3"); // existing key,value pair
			ClassicAssert.AreEqual (1, dict.Count, "Remove 3 Count");
			dictobj.Add (key1, value1); // restore state

			// TryGetValue
			NSDate value;
			Assert.Throws<ArgumentNullException> (() => dict.TryGetValue (null, out value), "TryGetValue ANE");
			ClassicAssert.IsTrue (dict.TryGetValue (key1, out value), "TryGetValue K1");
			ClassicAssert.AreSame (value1, value, "TryGetValue V1");
			ClassicAssert.IsFalse (dict.TryGetValue (key3, out value), "TryGetValue K2");

			// Values
			ClassicAssert.AreEqual (2, dict.Values.Count, "Values Count");

			// Indexer
			ClassicAssert.AreSame (value1, dict [key1], "this [1]");
			ClassicAssert.IsNull (dict [key3], "this [2]");
			Assert.Throws<ArgumentNullException> (() => GC.KeepAlive (dict [null]), "this [null]");

			dict [key3] = value3;
			ClassicAssert.AreEqual (3, dict.Count, "this [3] Count");
			ClassicAssert.AreSame (value3, dict [key3], "this [3] = 3");
			dictobj.Remove (key3); // restore state

			Assert.Throws<ArgumentNullException> (() => dict [key3] = null, "this [4] = null");
		}

		[Test]
		public void ICollection2Test ()
		{
			var value1 = NSDate.FromTimeIntervalSinceNow (1);
			var value2 = NSDate.FromTimeIntervalSinceNow (2);
			var value3 = NSDate.FromTimeIntervalSinceNow (3);
			var key1 = new NSString ("key1");
			var key2 = new NSString ("key2");
			var key3 = new NSString ("key3");

			var dictobj = new NSMutableDictionary<NSString, NSDate> (
				new NSString [] { key1, key2 },
				new NSDate [] { value1, value1 }
			);

			var dict = (ICollection<KeyValuePair<NSString, NSDate>>) dictobj;

			// Add
			Assert.Throws<ArgumentNullException> (() => dict.Add (new KeyValuePair<NSString, NSDate> (null, value1)), "Add ANE 1");
			Assert.Throws<ArgumentNullException> (() => dict.Add (new KeyValuePair<NSString, NSDate> (key1, null)), "Add ANE 2");
			dict.Add (new KeyValuePair<NSString, NSDate> (key3, value3));
			ClassicAssert.AreSame (value3, dictobj [key3], "Add 1");
			ClassicAssert.AreEqual (3, dict.Count, "Add Count");
			dictobj.Remove (key3); // restore state.

			// Clear
			dict.Clear ();
			ClassicAssert.AreEqual (0, dict.Count, "Clear Count");
			dictobj.Add (key1, value1); // restore state
			dictobj.Add (key2, value1); // restore state

			// Contains
			ClassicAssert.IsTrue (dict.Contains (new KeyValuePair<NSString, NSDate> (key1, value1)), "Contains 1"); // both key and value matches
			ClassicAssert.IsFalse (dict.Contains (new KeyValuePair<NSString, NSDate> (key1, value2)), "Contains 2"); // found key, wrong value
			ClassicAssert.IsFalse (dict.Contains (new KeyValuePair<NSString, NSDate> (key3, value2)), "Contains 3"); // wrong key


			// CopyTo
			var kvp_array = new KeyValuePair<NSString, NSDate> [1];
			Assert.Throws<ArgumentNullException> (() => dict.CopyTo (null, 0), "CopyTo ANE");
			Assert.Throws<ArgumentOutOfRangeException> (() => dict.CopyTo (kvp_array, -1), "CopyTo AOORE");
			Assert.Throws<ArgumentException> (() => dict.CopyTo (kvp_array, kvp_array.Length), "CopyTo AE 2");
			Assert.Throws<ArgumentException> (() => dict.CopyTo (kvp_array, 0), "CopyTo AE 3");

			kvp_array = new KeyValuePair<NSString, NSDate> [dictobj.Count];

			Assert.Throws<ArgumentException> (() => dict.CopyTo (kvp_array, 1), "CopyTo AE 4");
			dict.CopyTo (kvp_array, 0);
			Assert.That (key1, Is.SameAs (kvp_array [0].Key).Or.SameAs (kvp_array [1].Key), "CopyTo K1");
			ClassicAssert.AreSame (value1, kvp_array [0].Value, "CopyTo V1");
			Assert.That (key2, Is.SameAs (kvp_array [0].Key).Or.SameAs (kvp_array [1].Key), "CopyTo K2");
			ClassicAssert.AreSame (value1, kvp_array [1].Value, "CopyTo V2");

			// Count
			ClassicAssert.AreEqual (2, dict.Count, "Count");

			// GetEnumerator
			var enumerated = Enumerable.ToArray (dict);
			ClassicAssert.AreEqual (2, enumerated.Length, "Enumerator Count");

			// IsReadOnly
			ClassicAssert.IsFalse (dict.IsReadOnly, "IsReadOnly");

			// Remove
			Assert.Throws<ArgumentNullException> (() => dict.Remove (new KeyValuePair<NSString, NSDate> (null, value3)), "Remove ANE 1");
			Assert.Throws<ArgumentNullException> (() => dict.Remove (new KeyValuePair<NSString, NSDate> (key3, null)), "Remove ANE 2");
			ClassicAssert.IsFalse (dict.Remove (new KeyValuePair<NSString, NSDate> (key3, value3)), "Remove 1"); // inexistent key
			ClassicAssert.AreEqual (2, dict.Count, "Remove 1 Count");

			ClassicAssert.IsFalse (dict.Remove (new KeyValuePair<NSString, NSDate> (key1, value2)), "Remove 2"); // existing key, wrong value
			ClassicAssert.AreEqual (2, dict.Count, "Remove 2 Count");

			ClassicAssert.IsTrue (dict.Remove (new KeyValuePair<NSString, NSDate> (key1, value1)), "Remove 3"); // existing key,value pair
			ClassicAssert.AreEqual (1, dict.Count, "Remove 3 Count");
			dictobj.Add (key1, value1); // restore state
		}

		[Test]
		public void IEnumerable_KVP2Test ()
		{
			var value1 = NSDate.FromTimeIntervalSinceNow (1);
			var value2 = NSDate.FromTimeIntervalSinceNow (2);
			var value3 = NSDate.FromTimeIntervalSinceNow (3);
			var key1 = new NSString ("key1");
			var key2 = new NSString ("key2");
			var key3 = new NSString ("key3");

			var dictobj = new NSMutableDictionary<NSString, NSDate> (
				new NSString [] { key1, key2 },
				new NSDate [] { value1, value1 }
			);

			var dict = (IEnumerable<KeyValuePair<NSString, NSDate>>) dictobj;

			// GetEnumerator
			var enumerated = Enumerable.ToArray (dict);
			ClassicAssert.AreEqual (2, enumerated.Length, "Enumerator Count");
		}

		[Test]
		public void IEnumerableTest ()
		{
			var value1 = NSDate.FromTimeIntervalSinceNow (1);
			var value2 = NSDate.FromTimeIntervalSinceNow (2);
			var value3 = NSDate.FromTimeIntervalSinceNow (3);
			var key1 = new NSString ("key1");
			var key2 = new NSString ("key2");
			var key3 = new NSString ("key3");

			var dictobj = new NSMutableDictionary<NSString, NSDate> (
				new NSString [] { key1, key2 },
				new NSDate [] { value1, value1 }
			);

			var dict = (IEnumerable) dictobj;

			// GetEnumerator
			var c = 0;
			foreach (var obj in dict)
				c++;
			ClassicAssert.AreEqual (2, c, "Enumerator Count");
		}

		[Test]
		public void AddTest ()
		{
			var value1 = NSDate.FromTimeIntervalSinceNow (1);
			var value2 = NSDate.FromTimeIntervalSinceNow (2);
			var key1 = new NSString ("key1");
			var key2 = new NSString ("key2");

			var dict = new NSMutableDictionary<NSString, NSDate> ();

			Assert.Throws<ArgumentNullException> (() => dict.Add (null, value1), "ANE 1");
			Assert.Throws<ArgumentNullException> (() => dict.Add (key1, null), "ANE 2");

			dict.Add (key1, value1);
			ClassicAssert.AreEqual ((nuint) 1, dict.Count, "a Count");
			ClassicAssert.AreSame (value1, dict [key1], "a idx");

			dict.Add (key1, value1);
			ClassicAssert.AreEqual ((nuint) 1, dict.Count, "b Count");
			ClassicAssert.AreSame (value1, dict [key1], "b idx");

			dict.Add (key2, value1);
			ClassicAssert.AreEqual ((nuint) 2, dict.Count, "c Count");
			ClassicAssert.AreSame (value1, dict [key2], "c idx");
		}

		[Test]
		public void RemoveTest ()
		{
			var value1 = NSDate.FromTimeIntervalSinceNow (1);
			var value2 = NSDate.FromTimeIntervalSinceNow (2);
			var key1 = new NSString ("key1");
			var key2 = new NSString ("key2");

			var dict = new NSMutableDictionary<NSString, NSDate> ();

			Assert.Throws<ArgumentNullException> (() => dict.Remove ((NSString) null), "ANE 1");

			dict.Add (key1, value1);

			dict.Remove (key2);
			ClassicAssert.AreEqual ((nuint) 1, dict.Count, "a Count");
			ClassicAssert.AreSame (value1, dict [key1], "a idx");

			dict.Remove (key1);
			ClassicAssert.AreEqual ((nuint) 0, dict.Count, "b Count");
		}

		[Test]
		public void InvalidType ()
		{
			var kv = (NSString) "a";
			var dt = NSDate.FromTimeIntervalSinceNow (1);
			var obj = new NSDictionary (kv, kv);
			NSDate value = NSDate.FromTimeIntervalSinceNow (3);

			// dict where TValue is wrong
			var dict = new NSMutableDictionary<NSString, NSDate> ();
			dict.Add (kv, kv);
			Assert.Throws<InvalidCastException> (() => GC.KeepAlive (dict [kv]), "idx 1");
			Assert.Throws<InvalidCastException> (() => dict.ObjectForKey (kv), "ObjectForKey");
			Assert.Throws<InvalidCastException> (() => dict.ObjectsForKeys (new NSString [] { kv }, value), "ObjectsForKeys");
			Assert.Throws<InvalidCastException> (() => dict.TryGetValue (kv, out value), "TryGetValue");
			Assert.Throws<InvalidCastException> (() => GC.KeepAlive (dict.Values), "Values");

			// dict where TKey is wrong
			var dictK = new NSMutableDictionary<NSDate, NSString> ();
			dictK.Add (kv, kv);
			Assert.Throws<InvalidCastException> (() => GC.KeepAlive (dictK.Keys), "K Keys");
			Assert.Throws<InvalidCastException> (() => dictK.KeysForObject (kv), "K KeysForObject");
		}

		[Test]
		public void AddEntries ()
		{
			using (var dic1 = new NSMutableDictionary<NSString, NSDate> ()) {
				var now = NSDate.Now;
				using (var dic2 = NSDictionary.FromObjectAndKey ((NSDate) now, (NSString) "key")) {
					ClassicAssert.AreEqual ((nuint) 0, dic1.Count, "Count 0");

					dic1.AddEntries (dic2);

					ClassicAssert.AreEqual ((nuint) 1, dic1.Count, "Count 1");
					ClassicAssert.AreEqual (now, dic1 ["key"], "Value 1");

					dic1.AddEntries (dic2);

					ClassicAssert.AreEqual ((nuint) 1, dic1.Count, "Count 2");
					ClassicAssert.AreEqual (now, dic1 ["key"], "Value 2");
				}

				// Be nasty, and put something of the wrong type in the dictionary
				dic1.Clear ();
				var value = (NSString) "value";
				using (var dic2 = NSDictionary.FromObjectAndKey (value, (NSString) "key")) {
					ClassicAssert.AreEqual ((nuint) 0, dic1.Count, "X Count 0");

					dic1.AddEntries (dic2);

					ClassicAssert.AreEqual ((nuint) 1, dic1.Count, "X Count 1");
					Assert.Throws<InvalidCastException> (() => {
						var obj = dic1 [(NSString) "key"];
						// We shouldn't get this far
						Assert.Fail ($"ICE 1: Expected InvalidCastException, got back object '{obj}' of type '{obj?.GetType ()}' and handle '0x{obj?.Handle.ToString ("x")}'. Original object: '{value}' of type '{value?.GetType ()}' and handle '0x{value?.Handle.ToString ("x")}");
					}, "ICE 1");
				}

				// Use a generic dict of the right types
				dic1.Clear ();
				using (var dic2 = new NSDictionary<NSString, NSDate> ((NSString) "key2", now.AddSeconds (3600))) {
					ClassicAssert.AreEqual ((nuint) 0, dic1.Count, "Y Count 0");

					dic1.AddEntries (dic2);

					ClassicAssert.AreEqual ((nuint) 1, dic1.Count, "Y Count 1");
					var obj = dic1 [(NSString) "key2"];
					ClassicAssert.AreEqual (now.AddSeconds (3600).SecondsSinceReferenceDate, obj.SecondsSinceReferenceDate, "Y Value 1");
				}
			}
		}
	}
}
