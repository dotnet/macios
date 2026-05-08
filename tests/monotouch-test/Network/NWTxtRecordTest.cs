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
					Assert.That (d.Length, Is.Not.EqualTo (0), "Raw data length.");
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
		public void TestMissingKey () => Assert.That (record.FindKey ("foo"), Is.EqualTo (NWTxtRecordFindKey.NotPresent));

		[Test]
		public void TestPresentKey () => Assert.That (record.FindKey (randomKey), Is.EqualTo (NWTxtRecordFindKey.NonEmptyValue));

		[Test]
		public void TestAddByteValue ()
		{
			var data = new byte [] { 10, 20, 30, 40 };
			var mySecondKey = "secondKey";
			Assert.That (record.Add (mySecondKey, data), Is.True, "Add");
			Assert.That (record.FindKey (mySecondKey), Is.EqualTo (NWTxtRecordFindKey.NonEmptyValue));
		}

		[Test]
		public void TestAddNoValue ()
		{
			var mySecondKey = "secondLKey";
			Assert.That (record.Add (mySecondKey), Is.True, "Add");
			Assert.That (record.FindKey (mySecondKey), Is.EqualTo (NWTxtRecordFindKey.NoValue));
		}

		[Test]
		public void TestAddStringValue ()
		{
			var data = "hello";
			var mySecondKey = "secondLKey";
			Assert.That (record.Add (mySecondKey, data), Is.True, "Add");
			Assert.That (record.FindKey (mySecondKey), Is.EqualTo (NWTxtRecordFindKey.NonEmptyValue));
		}

		[Test]
		public void TestAddNullStringValue ()
		{
			string data = null;
			var mySecondKey = "secondLKey";
			Assert.That (record.Add (mySecondKey, data), Is.True, "Add");
			Assert.That (record.FindKey (mySecondKey), Is.EqualTo (NWTxtRecordFindKey.NoValue));
		}

		[Test]
		public void TestRemoveMissingKey () => Assert.That (record.Remove ("NotPresentKey"), Is.False);

		[Test]
		public void TestRemovePresentKey ()
		{
			Assert.That (record.Remove (randomKey), Is.True, "Remove");
			Assert.That (record.FindKey (randomKey), Is.EqualTo (NWTxtRecordFindKey.NotPresent), "FindKey");
		}

		[Test]
		public void TestKeyCount () => Assert.That (record.KeyCount, Is.EqualTo (1));

		[Test]
		public void TestIsDictionary () => Assert.That (record.IsDictionary, Is.True);

		[Test]
		public void TestNotNullEquals () => Assert.That (record.Equals (null), Is.False);

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
				Assert.That (keys.Contains (k), Is.True, k);
				return true;
			});
			Assert.That (keyCount2, Is.EqualTo (keys.Count), "keycount2");
		}

		[Test]
		public void TestGetValueMissing ()
		{
			var missing = "missingKey";
			record.GetValue (missing, (k, r, value) => {
				Assert.That (k, Is.EqualTo (missing), "key");
				Assert.That (r, Is.EqualTo (NWTxtRecordFindKey.NotPresent), "result");
				Assert.That (value.Length, Is.EqualTo (0), "value");
			});
		}

		[Test]
		public void TestGetValuePresent ()
		{
			record.GetValue (randomKey, (k, r, value) => {
				Assert.That (k, Is.EqualTo (randomKey), "key");
				Assert.That (r, Is.EqualTo (NWTxtRecordFindKey.NonEmptyValue), "result");
				Assert.That (value.Length, Is.Not.EqualTo (0), "value");
			});
		}

		[Test]
		public void TestGetRaw ()
		{
			var e = new AutoResetEvent (false);
			record.GetRawBytes (
				(d) => {
					Assert.That (d.Length, Is.Not.EqualTo (0));
					e.Set ();
				}
			);
			e.WaitOne ();

		}
	}
}
