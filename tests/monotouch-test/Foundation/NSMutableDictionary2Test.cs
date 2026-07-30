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
			Assert.That (dict.Count, Is.EqualTo ((nuint) 0), "Count");
		}

		[Test]
		public void Ctor_NSDictionary ()
		{
			var other = new NSDictionary<NSString, NSString> ((NSString) "key", (NSString) "value");
			var j = new NSMutableDictionary<NSString, NSString> (other);

			Assert.That (j.Count, Is.EqualTo ((nuint) 1), "count");
			Assert.That ((string) (NSString) (j [(NSString) "key"]), Is.EqualTo ("value"), "key lookup");
		}

		[Test]
		public void Ctor_NSMutableDictionary ()
		{
			var other = new NSMutableDictionary<NSString, NSString> ();
			other.Add ((NSString) "key", (NSString) "value");
			var j = new NSMutableDictionary<NSString, NSString> (other);

			Assert.That (j.Count, Is.EqualTo ((nuint) 1), "count");
			Assert.That ((string) (NSString) (j [(NSString) "key"]), Is.EqualTo ("value"), "key lookup");
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
			Assert.That (dict.Count, Is.EqualTo ((nuint) 5), "count");
			for (int i = 0; i < values.Length; i++)
				Assert.That (values [i], Is.EqualTo (dict [keys [i]]), $"key lookup, Iteration: {i}");
		}

		[Test]
		public void Ctor_WithNullValue ()
		{
			var key = (NSString) "key";
			using (var dict = new NSMutableDictionary<NSString, NSString> (key, null)) {
				Assert.That (dict.Count, Is.EqualTo ((nuint) 1), "count");
				var baseDict = (NSDictionary) dict;
				var rawValue = baseDict.ObjectForKey (key);
				Assert.That (rawValue, Is.InstanceOf<NSNull> (), "Null value should be NSNull");
			}
		}

		[Test]
		public void FromObjectsAndKeys_Generic_WithNull ()
		{
			var keys = new NSString [] { (NSString) "key1", (NSString) "key2" };
			var values = new NSString? [] { (NSString) "value1", null };

			using (var dict = NSMutableDictionary<NSString, NSString>.FromObjectsAndKeys (values, keys)) {
				Assert.That (dict, Is.Not.Null, "Dictionary should not be null");
				Assert.That (dict!.Count, Is.EqualTo ((nuint) 2), "Count");
				Assert.That (dict [keys [0]].ToString (), Is.EqualTo ("value1"), "First value");
				var baseDict = (NSDictionary) dict;
				var rawValue = baseDict.ObjectForKey (keys [1]);
				Assert.That (rawValue, Is.InstanceOf<NSNull> (), "Null value should be NSNull");
			}
		}

		[Test]
		public void FromObjectsAndKeys_Generic_WithCount_WithNull ()
		{
			var keys = new NSString [] { (NSString) "key1", (NSString) "key2", (NSString) "key3" };
			var values = new NSString? [] { (NSString) "value1", null, (NSString) "value3" };

			using (var dict = NSMutableDictionary<NSString, NSString>.FromObjectsAndKeys (values, keys, 2)) {
				Assert.That (dict, Is.Not.Null, "Dictionary should not be null");
				Assert.That (dict!.Count, Is.EqualTo ((nuint) 2), "Count");
				Assert.That (dict [keys [0]].ToString (), Is.EqualTo ("value1"), "First value");
				var baseDict = (NSDictionary) dict;
				var rawValue = baseDict.ObjectForKey (keys [1]);
				Assert.That (rawValue, Is.InstanceOf<NSNull> (), "Null value should be NSNull");
			}
		}

		[Test]
		public void FromObjectsAndKeys_Object_WithCount ()
		{
			var keys = new object [] { "key1", "key2", "key3" };
			var objs = new object [] { "value1", "value2", "value3" };

			using (var dict = NSMutableDictionary<NSString, NSString>.FromObjectsAndKeys (objs, keys, 2)) {
				Assert.That (dict, Is.Not.Null, "Dictionary should not be null");
				Assert.That (dict!.Count, Is.EqualTo ((nuint) 2), "Count");
				Assert.That (dict [(NSString) "key1"].ToString (), Is.EqualTo ("value1"), "First value");
				Assert.That (dict [(NSString) "key2"].ToString (), Is.EqualTo ("value2"), "Second value");
			}
		}

		[Test]
		public void FromObjectsAndKeys_NSObject_WithCount_WithNull ()
		{
			var keys = new NSObject [] { new NSString ("key1"), new NSString ("key2"), new NSString ("key3") };
			var objs = new NSObject? [] { new NSString ("value1"), null, new NSString ("value3") };

			using (var dict = NSMutableDictionary<NSString, NSString>.FromObjectsAndKeys (objs, keys, 2)) {
				Assert.That (dict, Is.Not.Null, "Dictionary should not be null");
				Assert.That (dict!.Count, Is.EqualTo ((nuint) 2), "Count");
				Assert.That (dict [(NSString) keys [0]].ToString (), Is.EqualTo ("value1"), "First value");
				var baseDict = (NSDictionary) dict;
				var rawValue = baseDict.ObjectForKey (keys [1]);
				Assert.That (rawValue, Is.InstanceOf<NSNull> (), "Null value should be NSNull");
			}
		}

		[Test]
		public void FromObjectsAndKeys_NSObject_WithCount ()
		{
			var keys = new NSObject [] { new NSString ("key1"), new NSString ("key2"), new NSString ("key3") };
			var objs = new NSObject [] { new NSString ("value1"), new NSString ("value2"), new NSString ("value3") };

			using (var dict = NSMutableDictionary<NSString, NSString>.FromObjectsAndKeys (objs, keys, 2)) {
				Assert.That (dict, Is.Not.Null, "Dictionary should not be null");
				Assert.That (dict!.Count, Is.EqualTo ((nuint) 2), "Count");
				Assert.That (dict [(NSString) keys [0]].ToString (), Is.EqualTo ("value1"), "First value");
				Assert.That (dict [(NSString) keys [1]].ToString (), Is.EqualTo ("value2"), "Second value");
			}
		}

		[Test]
		public void FromObjectsAndKeys_Generic_WithCountZero ()
		{
			var keys = new NSString [] { (NSString) "key1", (NSString) "key2" };
			var values = new NSString [] { (NSString) "value1", (NSString) "value2" };

			using (var dict = NSMutableDictionary<NSString, NSString>.FromObjectsAndKeys (values, keys, 0)) {
				Assert.That (dict, Is.Not.Null, "Dictionary should not be null");
				Assert.That (dict!.Count, Is.EqualTo ((nuint) 0), "Count should be 0");
			}
		}

		[Test]
		public void FromObjectsAndKeys_DifferentArrayLengths_WithCount ()
		{
			var keys = new NSString [] { (NSString) "key1", (NSString) "key2" };
			var values = new NSString [] { (NSString) "value1", (NSString) "value2", (NSString) "value3", (NSString) "value4" };

			// Should work fine since we only use first 2 items from each array
			using (var dict = NSMutableDictionary<NSString, NSString>.FromObjectsAndKeys (values, keys, 2)) {
				Assert.That (dict, Is.Not.Null, "Dictionary should not be null");
				Assert.That (dict!.Count, Is.EqualTo ((nuint) 2), "Count");
				Assert.That (dict [keys [0]].ToString (), Is.EqualTo ("value1"), "First value");
				Assert.That (dict [keys [1]].ToString (), Is.EqualTo ("value2"), "Second value");
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
				using (var d = new NSMutableDictionary<NSString, NSString> (k, v)) {
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
		public void Copy ()
		{
			using (var k = new NSString ("key"))
			using (var v = new NSString ("value"))
			using (var d = new NSMutableDictionary<NSString, NSString> (k, v)) {
				// NSObject.Copy works because NSDictionary conforms to NSCopying
				using (var copy1 = (NSDictionary) d.Copy ()) {
					Assert.That (copy1, Is.Not.SameAs (d), "1");
					Assert.That (copy1, Is.Not.TypeOf<NSMutableDictionary> (), "NSDictionary-1");
					Assert.That (copy1.Count, Is.EqualTo ((nuint) 1), "Count-1");
				}

				using (var copy2 = (NSDictionary) d.Copy (null)) {
					Assert.That (copy2, Is.Not.SameAs (d), "2");
					Assert.That (copy2, Is.Not.TypeOf<NSMutableDictionary> (), "NSDictionary-2");
					Assert.That (copy2.Count, Is.EqualTo ((nuint) 1), "Count-2");
				}

				using (var copy3 = (NSDictionary) d.Copy (NSZone.Default)) {
					Assert.That (copy3, Is.Not.SameAs (d), "3");
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
			Assert.That (dict.ObjectForKey (key), Is.SameAs (value), "right");
			Assert.That (dict.ObjectForKey ((NSString) "wrong key"), Is.Null, "wrong");
		}

		[Test]
		public void KeysTest ()
		{
			var value = NSDate.FromTimeIntervalSinceNow (23);
			var key = new NSString ("right key");
			var dict = new NSMutableDictionary<NSString, NSDate> (key, value);

			var keys = dict.Keys;
			Assert.That (keys.Length, Is.EqualTo (1), "Length");
			Assert.That (keys [0], Is.SameAs (key), "1");
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
			Assert.That (rv.Length, Is.EqualTo (2), "v1");

			rv = dict.KeysForObject (value2);
			Assert.That (rv.Length, Is.EqualTo (1), "v2");
			Assert.That (rv [0], Is.SameAs (key3), "v2 key");

			rv = dict.KeysForObject (value3);
			Assert.That (rv.Length, Is.EqualTo (0), "v3");

			Assert.Throws<ArgumentNullException> (() => dict.KeysForObject (null), "ANE");
		}

		[Test]
		public void ValuesTest ()
		{
			var value = NSDate.FromTimeIntervalSinceNow (23);
			var key = new NSString ("right key");
			var dict = new NSMutableDictionary<NSString, NSDate> (key, value);

			var keys = dict.Values;
			Assert.That (dict.Values.Length, Is.EqualTo (1), "Length");
			Assert.That (dict [key], Is.SameAs (value), "1");
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
			Assert.That (rv.Length, Is.EqualTo (2), "a");
			Assert.That (rv [0], Is.SameAs (value1), "a 0");
			Assert.That (rv [1], Is.SameAs (value3), "a 1");

			rv = dict.ObjectsForKeys (new NSString [] { }, value3);
			Assert.That (rv.Length, Is.EqualTo (0), "b length");

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

			Assert.That (dict.ContainsKey (key1), Is.True, "a");
			Assert.That (dict.ContainsKey (key3), Is.False, "b");
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

			Assert.That (dict.TryGetValue (key1, out value), Is.True, "a");
			Assert.That (value, Is.SameAs (value1), "a same");

			Assert.That (dict.TryGetValue (key3, out value), Is.False, "b");
			Assert.That (value, Is.Null, "b null");
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

			Assert.That (dict [key1], Is.SameAs (value1), "a");
			Assert.That (dict [key3], Is.Null, "b");
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
			Assert.That (dict [keyMissing], Is.Null, "missing key");

			// Accessing via IDictionary interface should return null too
			IDictionary<NSString, NSDate> idict = dict;
			Assert.That (idict [keyMissing], Is.Null, "missing key via interface");
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
			Assert.That (dict.ObjectForKey (keyMissing), Is.Null, "ObjectForKey missing");

			// TryGetValue should return false for missing keys
			NSDate value;
			Assert.That (dict.TryGetValue (keyMissing, out value), Is.False, "TryGetValue missing");
			Assert.That (value, Is.Null, "TryGetValue out value");

			// ContainsKey should return false for missing keys
			Assert.That (dict.ContainsKey (keyMissing), Is.False, "ContainsKey missing");

			// Indexer getter should return null
			Assert.That (dict [keyMissing], Is.Null, "Indexer missing");

			// IDictionary indexer should also return null
			IDictionary<NSString, NSDate> idict = dict;
			Assert.That (idict [keyMissing], Is.Null, "IDictionary indexer missing");
		}

		[Test]
		public void EmptyDictionaryMissingKeyTest ()
		{
			var dict = new NSMutableDictionary<NSString, NSDate> ();
			var keyMissing = new NSString ("missing");

			// All access methods should handle missing keys in empty dictionary
			Assert.That (dict.ObjectForKey (keyMissing), Is.Null, "ObjectForKey");
			Assert.That (dict.ContainsKey (keyMissing), Is.False, "ContainsKey");

			NSDate value;
			Assert.That (dict.TryGetValue (keyMissing, out value), Is.False, "TryGetValue");
			Assert.That (value, Is.Null, "TryGetValue out");

			Assert.That (dict [keyMissing], Is.Null, "Indexer");

			IDictionary<NSString, NSDate> idict = dict;
			Assert.That (idict [keyMissing], Is.Null, "IDictionary indexer");
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
			Assert.That (result.Length, Is.EqualTo (4), "Length");
			Assert.That (result [0], Is.SameAs (value1), "0 - existing");
			Assert.That (result [1], Is.SameAs (marker), "1 - missing");
			Assert.That (result [2], Is.SameAs (value2), "2 - existing");
			Assert.That (result [3], Is.SameAs (marker), "3 - missing");

			// Request all missing keys
			result = dict.ObjectsForKeys (new NSString [] { keyMissing1, keyMissing2 }, marker);
			Assert.That (result.Length, Is.EqualTo (2), "All missing length");
			Assert.That (result [0], Is.SameAs (marker), "All missing 0");
			Assert.That (result [1], Is.SameAs (marker), "All missing 1");
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
			Assert.That (dictobj [key3], Is.SameAs (value3), "Add 1");
			Assert.That (dict.Count, Is.EqualTo (3), "Add Count");
			dictobj.Remove (key3); // restore state.

			// Clear
			dict.Clear ();
			Assert.That (dict.Count, Is.EqualTo (0), "Clear Count");
			dictobj.Add (key1, value1); // restore state
			dictobj.Add (key2, value1); // restore state

			// Contains
			Assert.That (dict.Contains (new KeyValuePair<NSString, NSDate> (key1, value1)), Is.True, "Contains 1"); // both key and value matches
			Assert.That (dict.Contains (new KeyValuePair<NSString, NSDate> (key1, value2)), Is.False, "Contains 2"); // found key, wrong value
			Assert.That (dict.Contains (new KeyValuePair<NSString, NSDate> (key3, value2)), Is.False, "Contains 3"); // wrong key

			// ContainsKey
			Assert.That (dict.ContainsKey (key1), Is.True, "ContainsKey 1");
			Assert.That (dict.ContainsKey (key3), Is.False, "ContainsKey 2");

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
			Assert.That (kvp_array [0].Value, Is.SameAs (value1), "CopyTo V1");
			Assert.That (key2, Is.SameAs (kvp_array [0].Key).Or.SameAs (kvp_array [1].Key), "CopyTo K2");
			Assert.That (kvp_array [1].Value, Is.SameAs (value1), "CopyTo V2");

			// Count
			Assert.That (dict.Count, Is.EqualTo (2), "Count");

			// GetEnumerator
			var enumerated = Enumerable.ToArray (dict);
			Assert.That (enumerated.Length, Is.EqualTo (2), "Enumerator Count");

			// IsReadOnly
			Assert.That (dict.IsReadOnly, Is.False, "IsReadOnly");

			// Keys
			Assert.That (dict.Keys.Count, Is.EqualTo (2), "Keys Count");

			// Remove
			Assert.Throws<ArgumentNullException> (() => dict.Remove (new KeyValuePair<NSString, NSDate> (null, value3)), "Remove ANE 1");
			Assert.Throws<ArgumentNullException> (() => dict.Remove (new KeyValuePair<NSString, NSDate> (key3, null)), "Remove ANE 2");
			Assert.That (dict.Remove (new KeyValuePair<NSString, NSDate> (key3, value3)), Is.False, "Remove 1"); // inexistent key
			Assert.That (dict.Count, Is.EqualTo (2), "Remove 1 Count");

			Assert.That (dict.Remove (new KeyValuePair<NSString, NSDate> (key1, value2)), Is.False, "Remove 2"); // existing key, wrong value
			Assert.That (dict.Count, Is.EqualTo (2), "Remove 2 Count");

			Assert.That (dict.Remove (new KeyValuePair<NSString, NSDate> (key1, value1)), Is.True, "Remove 3"); // existing key,value pair
			Assert.That (dict.Count, Is.EqualTo (1), "Remove 3 Count");
			dictobj.Add (key1, value1); // restore state

			// TryGetValue
			NSDate value;
			Assert.Throws<ArgumentNullException> (() => dict.TryGetValue (null, out value), "TryGetValue ANE");
			Assert.That (dict.TryGetValue (key1, out value), Is.True, "TryGetValue K1");
			Assert.That (value, Is.SameAs (value1), "TryGetValue V1");
			Assert.That (dict.TryGetValue (key3, out value), Is.False, "TryGetValue K2");

			// Values
			Assert.That (dict.Values.Count, Is.EqualTo (2), "Values Count");

			// Indexer
			Assert.That (dict [key1], Is.SameAs (value1), "this [1]");
			Assert.That (dict [key3], Is.Null, "this [2]");
			Assert.Throws<ArgumentNullException> (() => GC.KeepAlive (dict [null]), "this [null]");

			dict [key3] = value3;
			Assert.That (dict.Count, Is.EqualTo (3), "this [3] Count");
			Assert.That (dict [key3], Is.SameAs (value3), "this [3] = 3");
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
			Assert.That (dictobj [key3], Is.SameAs (value3), "Add 1");
			Assert.That (dict.Count, Is.EqualTo (3), "Add Count");
			dictobj.Remove (key3); // restore state.

			// Clear
			dict.Clear ();
			Assert.That (dict.Count, Is.EqualTo (0), "Clear Count");
			dictobj.Add (key1, value1); // restore state
			dictobj.Add (key2, value1); // restore state

			// Contains
			Assert.That (dict.Contains (new KeyValuePair<NSString, NSDate> (key1, value1)), Is.True, "Contains 1"); // both key and value matches
			Assert.That (dict.Contains (new KeyValuePair<NSString, NSDate> (key1, value2)), Is.False, "Contains 2"); // found key, wrong value
			Assert.That (dict.Contains (new KeyValuePair<NSString, NSDate> (key3, value2)), Is.False, "Contains 3"); // wrong key


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
			Assert.That (kvp_array [0].Value, Is.SameAs (value1), "CopyTo V1");
			Assert.That (key2, Is.SameAs (kvp_array [0].Key).Or.SameAs (kvp_array [1].Key), "CopyTo K2");
			Assert.That (kvp_array [1].Value, Is.SameAs (value1), "CopyTo V2");

			// Count
			Assert.That (dict.Count, Is.EqualTo (2), "Count");

			// GetEnumerator
			var enumerated = Enumerable.ToArray (dict);
			Assert.That (enumerated.Length, Is.EqualTo (2), "Enumerator Count");

			// IsReadOnly
			Assert.That (dict.IsReadOnly, Is.False, "IsReadOnly");

			// Remove
			Assert.Throws<ArgumentNullException> (() => dict.Remove (new KeyValuePair<NSString, NSDate> (null, value3)), "Remove ANE 1");
			Assert.Throws<ArgumentNullException> (() => dict.Remove (new KeyValuePair<NSString, NSDate> (key3, null)), "Remove ANE 2");
			Assert.That (dict.Remove (new KeyValuePair<NSString, NSDate> (key3, value3)), Is.False, "Remove 1"); // inexistent key
			Assert.That (dict.Count, Is.EqualTo (2), "Remove 1 Count");

			Assert.That (dict.Remove (new KeyValuePair<NSString, NSDate> (key1, value2)), Is.False, "Remove 2"); // existing key, wrong value
			Assert.That (dict.Count, Is.EqualTo (2), "Remove 2 Count");

			Assert.That (dict.Remove (new KeyValuePair<NSString, NSDate> (key1, value1)), Is.True, "Remove 3"); // existing key,value pair
			Assert.That (dict.Count, Is.EqualTo (1), "Remove 3 Count");
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
			Assert.That (enumerated.Length, Is.EqualTo (2), "Enumerator Count");
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
			Assert.That (c, Is.EqualTo (2), "Enumerator Count");
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
			Assert.That (dict.Count, Is.EqualTo ((nuint) 1), "a Count");
			Assert.That (dict [key1], Is.SameAs (value1), "a idx");

			dict.Add (key1, value1);
			Assert.That (dict.Count, Is.EqualTo ((nuint) 1), "b Count");
			Assert.That (dict [key1], Is.SameAs (value1), "b idx");

			dict.Add (key2, value1);
			Assert.That (dict.Count, Is.EqualTo ((nuint) 2), "c Count");
			Assert.That (dict [key2], Is.SameAs (value1), "c idx");
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
			Assert.That (dict.Count, Is.EqualTo ((nuint) 1), "a Count");
			Assert.That (dict [key1], Is.SameAs (value1), "a idx");

			dict.Remove (key1);
			Assert.That (dict.Count, Is.EqualTo ((nuint) 0), "b Count");
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
					Assert.That (dic1.Count, Is.EqualTo ((nuint) 0), "Count 0");

					dic1.AddEntries (dic2);

					Assert.That (dic1.Count, Is.EqualTo ((nuint) 1), "Count 1");
					Assert.That (dic1 ["key"], Is.EqualTo (now), "Value 1");

					dic1.AddEntries (dic2);

					Assert.That (dic1.Count, Is.EqualTo ((nuint) 1), "Count 2");
					Assert.That (dic1 ["key"], Is.EqualTo (now), "Value 2");
				}

				// Be nasty, and put something of the wrong type in the dictionary
				dic1.Clear ();
				var value = (NSString) "value";
				using (var dic2 = NSDictionary.FromObjectAndKey (value, (NSString) "key")) {
					Assert.That (dic1.Count, Is.EqualTo ((nuint) 0), "X Count 0");

					dic1.AddEntries (dic2);

					Assert.That (dic1.Count, Is.EqualTo ((nuint) 1), "X Count 1");
					Assert.Throws<InvalidCastException> (() => {
						var obj = dic1 [(NSString) "key"];
						// We shouldn't get this far
						Assert.Fail ($"ICE 1: Expected InvalidCastException, got back object '{obj}' of type '{obj?.GetType ()}' and handle '0x{obj?.Handle.ToString ("x")}'. Original object: '{value}' of type '{value?.GetType ()}' and handle '0x{value?.Handle.ToString ("x")}");
					}, "ICE 1");
				}

				// Use a generic dict of the right types
				dic1.Clear ();
				using (var dic2 = new NSDictionary<NSString, NSDate> ((NSString) "key2", now.AddSeconds (3600))) {
					Assert.That (dic1.Count, Is.EqualTo ((nuint) 0), "Y Count 0");

					dic1.AddEntries (dic2);

					Assert.That (dic1.Count, Is.EqualTo ((nuint) 1), "Y Count 1");
					var obj = dic1 [(NSString) "key2"];
					Assert.That (obj.SecondsSinceReferenceDate, Is.EqualTo (now.AddSeconds (3600).SecondsSinceReferenceDate), "Y Value 1");
				}
			}
		}
	}
}
