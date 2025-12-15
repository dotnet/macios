
namespace monotouchtest {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSMutableDictionaryTest {

		[Test]
		public void IndexerTest ()
		{
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
				using (var dict = Runtime.GetNSObject<NSMutableDictionary> (Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr (Class.GetHandle (typeof (NSMutableDictionary)), Selector.GetHandle ("dictionaryWithObject:forKey:"), objptr, keyptr))) {
					v = (NSString) dict ["key"];
					Assert.AreEqual ("obj", (string) v, "a");

					dict ["key"] = (NSString) "value";
					v = (NSString) dict ["key"];
					Assert.AreEqual ("value", (string) v, "a");
				}

				// this[NSObject]
				keyptr = Messaging.IntPtr_objc_msgSend_IntPtr (Class.GetHandle (typeof (NSString)), Selector.GetHandle ("stringWithUTF8String:"), strkeyptr);
				objptr = Messaging.IntPtr_objc_msgSend_IntPtr (Class.GetHandle (typeof (NSString)), Selector.GetHandle ("stringWithUTF8String:"), strobjptr);
				using (var dict = Runtime.GetNSObject<NSMutableDictionary> (Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr (Class.GetHandle (typeof (NSMutableDictionary)), Selector.GetHandle ("dictionaryWithObject:forKey:"), objptr, keyptr))) {
					v = (NSString) dict [(NSObject) (NSString) "key"];
					Assert.AreEqual ("obj", (string) v, "b");

					dict [(NSObject) (NSString) "key"] = (NSString) "value";
					v = (NSString) dict ["key"];
					Assert.AreEqual ("value", (string) v, "a");
				}

				// this[NSString]
				keyptr = Messaging.IntPtr_objc_msgSend_IntPtr (Class.GetHandle (typeof (NSString)), Selector.GetHandle ("stringWithUTF8String:"), strkeyptr);
				objptr = Messaging.IntPtr_objc_msgSend_IntPtr (Class.GetHandle (typeof (NSString)), Selector.GetHandle ("stringWithUTF8String:"), strobjptr);
				using (var dict = Runtime.GetNSObject<NSMutableDictionary> (Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr (Class.GetHandle (typeof (NSMutableDictionary)), Selector.GetHandle ("dictionaryWithObject:forKey:"), objptr, keyptr))) {
					v = (NSString) dict [(NSString) "key"];
					Assert.AreEqual ("obj", (string) v, "c");

					dict [(NSString) "key"] = (NSString) "value";
					v = (NSString) dict ["key"];
					Assert.AreEqual ("value", (string) v, "a");
				}

			} finally {
				Marshal.FreeHGlobal (strkeyptr);
				Marshal.FreeHGlobal (strobjptr);
			}
		}

		[Test]
		public void Bug39993 ()
		{
			using (NSMutableDictionary testDict = new NSMutableDictionary ()) {
				testDict.Add ((NSString) "Key1", (NSString) "Key1");
				testDict.Add ((NSString) "Key2", (NSString) "KeyTest2");
				Assert.NotNull (testDict ["Key1"], "Key1");
				Assert.NotNull (testDict ["Key2"], "Key2");
			}
		}

		[Test]
		public void AddEntries ()
		{
			using (var dic1 = new NSMutableDictionary ()) {
				using (var dic2 = NSDictionary.FromObjectAndKey ((NSString) "value", (NSString) "key")) {
					Assert.AreEqual ((nuint) 0, dic1.Count, "Count 0");

					dic1.AddEntries (dic2);

					Assert.AreEqual ((nuint) 1, dic1.Count, "Count 1");
					Assert.AreEqual ("value", dic1 ["key"].ToString (), "Value 1");

					dic1.AddEntries (dic2);

					Assert.AreEqual ((nuint) 1, dic1.Count, "Count 2");
					Assert.AreEqual ("value", dic1 ["key"].ToString (), "Value 2");
				}
			}
		}

		[Test]
		public void MissingKey_StringIndexer ()
		{
			using (var dict = new NSMutableDictionary ()) {
				dict ["existingKey"] = (NSString) "value";

				// Accessing a missing key should return null
				var result = dict ["missingKey"];
				Assert.IsNull (result, "Missing key should return null");

				// Verify the existing key still works
				Assert.IsNotNull (dict ["existingKey"], "Existing key should return value");
			}
		}

		[Test]
		public void MissingKey_NSObjectIndexer ()
		{
			using (var dict = new NSMutableDictionary ()) {
				var existingKey = NSDate.Now;
				var missingKey = NSDate.DistantPast;
				dict [existingKey] = NSDate.DistantFuture;

				// Accessing a missing key should return null
				var result = dict [missingKey];
				Assert.IsNull (result, "Missing key should return null");

				// Verify the existing key still works
				Assert.IsNotNull (existingKey, "Existing key should return value");
			}
		}

		[Test]
		public void MissingKey_NSStringIndexer ()
		{
			using (var dict = new NSMutableDictionary ()) {
				dict [(NSString) "existingKey"] = (NSString) "value";

				// Accessing a missing key should return null
				var result = dict [(NSString) "missingKey"];
				Assert.IsNull (result, "Missing key should return null");

				// Verify the existing key still works
				Assert.IsNotNull (dict [(NSString) "existingKey"], "Existing key should return value");
			}
		}

		[Test]
		public void MissingKey_ObjectForKey ()
		{
			using (var dict = new NSMutableDictionary ()) {
				dict [(NSString) "existingKey"] = (NSString) "value";

				// ObjectForKey with missing key should return null
				var result = dict.ObjectForKey ((NSString) "missingKey");
				Assert.IsNull (result, "ObjectForKey with missing key should return null");

				// Verify the existing key still works
				Assert.IsNotNull (dict.ObjectForKey ((NSString) "existingKey"), "ObjectForKey with existing key should return value");
			}
		}

		[Test]
		public void MissingKey_TryGetValue ()
		{
			using (var dict = new NSMutableDictionary ()) {
				dict [(NSString) "existingKey"] = (NSString) "value";

				// TryGetValue with missing key should return false
				var found = dict.TryGetValue ((NSString) "missingKey", out var result);
				Assert.IsFalse (found, "TryGetValue should return false for missing key");
				Assert.IsNull (result, "Output value should be null for missing key");

				// Verify the existing key works
				found = dict.TryGetValue ((NSString) "existingKey", out result);
				Assert.IsTrue (found, "TryGetValue should return true for existing key");
				Assert.IsNotNull (result, "Output value should not be null for existing key");
				Assert.AreEqual ("value", result.ToString (), "Output value should match");
			}
		}

		[Test]
		public void MissingKey_IDictionaryIndexer ()
		{
			using (var dict = new NSMutableDictionary ()) {
				System.Collections.IDictionary idict = dict;
				idict [(NSString) "existingKey"] = (NSString) "value";

				// Accessing a missing key through IDictionary indexer returns IntPtr.Zero (not null)
				// This is different from the typed indexers which return null
				var result = idict [(NSString) "missingKey"];
				// The IDictionary indexer calls _ObjectForKey which returns IntPtr.Zero boxed
				Assert.AreEqual (IntPtr.Zero, result, "IDictionary indexer with missing key returns IntPtr.Zero");

				// Verify the existing key still works
				Assert.IsNotNull (idict [(NSString) "existingKey"], "IDictionary indexer with existing key should return value");
			}
		}

		[Test]
		public void MissingKey_IDictionaryContains ()
		{
			using (var dict = new NSMutableDictionary ()) {
				System.Collections.IDictionary idict = dict;
				idict [(NSString) "existingKey"] = (NSString) "value";

				// Contains should return false for missing key
				Assert.IsFalse (idict.Contains ((NSString) "missingKey"), "Contains should return false for missing key");

				// Contains should return true for existing key
				Assert.IsTrue (idict.Contains ((NSString) "existingKey"), "Contains should return true for existing key");
			}
		}
	}
}
