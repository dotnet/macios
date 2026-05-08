using System.Threading;

using Network;

namespace MonoTouchFixtures.Network {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NWTxtRecordTest {
		NWTxtRecord record;
		string randomKey = "MyData";

		[OneTimeSetUp]
		public void Init () => TestRuntime.AssertXcodeVersion (11, 0);


		[SetUp]
		public void SetUp ()
		{
			record = NWTxtRecord.CreateDictionary ();
			record.Add (randomKey, new byte [3] { 0, 0, 0 });
		}

		[Test]
		public void TestFromBytes ()
		{
			// get the raw data from the dictionary create txt record, and recreate a new one
			var e = new AutoResetEvent (false);
			record.GetRawBytes (
				(d) => {
					ClassicAssert.AreNotEqual (0, d.Length, "Raw data length.");
					e.Set ();
				}
			);
			e.WaitOne ();
		}

		[TearDown]
		public void TearDown ()
		{
			record.Dispose ();
		}

		[Test]
		public void TestMissingKey () => ClassicAssert.AreEqual (NWTxtRecordFindKey.NotPresent, record.FindKey ("foo"));

		[Test]
		public void TestPresentKey () => ClassicAssert.AreEqual (NWTxtRecordFindKey.NonEmptyValue, record.FindKey (randomKey));

		[Test]
		public void TestAddByteValue ()
		{
			var data = new byte [] { 10, 20, 30, 40 };
			var mySecondKey = "secondKey";
			ClassicAssert.True (record.Add (mySecondKey, data), "Add");
			ClassicAssert.AreEqual (NWTxtRecordFindKey.NonEmptyValue, record.FindKey (mySecondKey));
		}

		[Test]
		public void TestAddNoValue ()
		{
			var mySecondKey = "secondLKey";
			ClassicAssert.True (record.Add (mySecondKey), "Add");
			ClassicAssert.AreEqual (NWTxtRecordFindKey.NoValue, record.FindKey (mySecondKey));
		}

		[Test]
		public void TestAddStringValue ()
		{
			var data = "hello";
			var mySecondKey = "secondLKey";
			ClassicAssert.True (record.Add (mySecondKey, data), "Add");
			ClassicAssert.AreEqual (NWTxtRecordFindKey.NonEmptyValue, record.FindKey (mySecondKey));
		}

		[Test]
		public void TestAddNullStringValue ()
		{
			string data = null;
			var mySecondKey = "secondLKey";
			ClassicAssert.True (record.Add (mySecondKey, data), "Add");
			ClassicAssert.AreEqual (NWTxtRecordFindKey.NoValue, record.FindKey (mySecondKey));
		}

		[Test]
		public void TestRemoveMissingKey () => ClassicAssert.IsFalse (record.Remove ("NotPresentKey"));

		[Test]
		public void TestRemovePresentKey ()
		{
			ClassicAssert.True (record.Remove (randomKey), "Remove");
			ClassicAssert.AreEqual (NWTxtRecordFindKey.NotPresent, record.FindKey (randomKey), "FindKey");
		}

		[Test]
		public void TestKeyCount () => ClassicAssert.AreEqual (1, record.KeyCount);

		[Test]
		public void TestIsDictionary () => ClassicAssert.IsTrue (record.IsDictionary);

		[Test]
		public void TestNotNullEquals () => ClassicAssert.IsFalse (record.Equals (null));

		[Test]
		public void TestApply ()
		{
			// fill the txt with several keys to be iterated
			var keys = new List<string> { "first", "second", "third", randomKey };
			foreach (var key in keys) {
				record.Add (key, key);
			}
			// apply and ensure that we do get all the keys
			var keyCount2 = 0;
			record.Apply ((k, r, v) => {
				keyCount2++;
				ClassicAssert.IsTrue (keys.Contains (k), k);
				return true;
			});
			ClassicAssert.AreEqual (keys.Count, keyCount2, "keycount2");
		}

		[Test]
		public void TestGetValueMissing ()
		{
			var missing = "missingKey";
			record.GetValue (missing, (k, r, value) => {
				ClassicAssert.AreEqual (missing, k, "key");
				ClassicAssert.AreEqual (NWTxtRecordFindKey.NotPresent, r, "result");
				ClassicAssert.AreEqual (0, value.Length, "value");
			});
		}

		[Test]
		public void TestGetValuePresent ()
		{
			record.GetValue (randomKey, (k, r, value) => {
				ClassicAssert.AreEqual (randomKey, k, "key");
				ClassicAssert.AreEqual (NWTxtRecordFindKey.NonEmptyValue, r, "result");
				ClassicAssert.AreNotEqual (0, value.Length, "value");
			});
		}

		[Test]
		public void TestGetRaw ()
		{
			var e = new AutoResetEvent (false);
			record.GetRawBytes (
				(d) => {
					ClassicAssert.AreNotEqual (0, d.Length);
					e.Set ();
				}
			);
			e.WaitOne ();

		}
	}
}
