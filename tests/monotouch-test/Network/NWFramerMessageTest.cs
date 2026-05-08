using System.Text;

using Network;

namespace MonoTouchFixtures.Network {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NWFramerMessageTest {
		NWFramerMessage message;
		NWFramer framer;

		string identifier = "TestFramer";

		[OneTimeSetUp]
		public void Init () => TestRuntime.AssertXcodeVersion (11, 0);

		NWFramerStartResult StartCallback (NWFramer nWFramer)
		{
			framer = nWFramer;
			return NWFramerStartResult.Ready;
		}

		[SetUp]
		public void SetUp ()
		{
			using (var definition = NWProtocolDefinition.CreateFramerDefinition (identifier, NWFramerCreateFlags.Default, StartCallback)) {
				message = NWFramerMessage.Create (definition);
			}
		}

		[TearDown]
		public void TearDown ()
		{
			message.Dispose ();
		}

		[Test]
		public void TestGetObject ()
		{
			// store an NSObject
			var storedValue = new NSNumber (30);
			message.SetObject ("test", storedValue);

			var result = message.GetObject<NSNumber> ("test");
			Assert.That (result, Is.Not.Null, "Null");
			Assert.That (result, Is.EqualTo (storedValue), "Equal");
		}

		[Test]
		public void TestGetObjectMissingKey ()
		{
			var result = message.GetObject<NSNumber> ("test");
			Assert.That (result, Is.Null, "Null");
		}

		[Test]
		public void TestGetData ()
		{
			var dataString = "My super string.";
			var data = Encoding.UTF8.GetBytes (dataString);
			message.SetData ("test", data);

			ReadOnlySpan<byte> outData;
			var found = message.GetData ("test", data.Length, out outData);

			Assert.That (found, Is.True, "Found");
			Assert.That (outData.Length, Is.EqualTo (data.Length), "Legth");
			Assert.That (Encoding.UTF8.GetString (outData), Is.EqualTo (dataString), "Equal");
		}

		[Test]
		public void TestGetDataMissingKey ()
		{
			ReadOnlySpan<byte> outData;
			var found = message.GetData ("test", 23, out outData);
			Assert.That (found, Is.False, "Found");
			Assert.That (outData.Length, Is.EqualTo (0), "Length");
		}
	}
}
