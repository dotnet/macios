#nullable enable

#if __MACOS__

#pragma warning disable APL0002

using Foundation;
using FSKit;
using Xamarin.Utils;

namespace MonoTouchFixtures.FSKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class FSEntityIdentifierTest {

		[Test]
		public void Create ()
		{
			TestRuntime.AssertXcodeVersion (27, 0);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 27, 0);

			using var uuid = new NSUuid ();
			using var qualifierData = NSData.FromArray (new byte [8]);
			using var identifier = FSEntityIdentifier.Create (uuid, qualifierData);
			Assert.That (identifier, Is.Not.Null, "valid qualifier data");

			using var invalidQualifierData = NSData.FromArray (new byte [7]);
			using var invalidIdentifier = FSEntityIdentifier.Create (uuid, invalidQualifierData);
			Assert.That (invalidIdentifier, Is.Null, "invalid qualifier data");
		}

		[Test]
		public void CreateNullArguments ()
		{
			using var uuid = new NSUuid ();
			using var qualifierData = NSData.FromArray (new byte [8]);
			Assert.Throws<ArgumentNullException> (() => FSEntityIdentifier.Create (null, qualifierData), "uuid");
			Assert.Throws<ArgumentNullException> (() => FSEntityIdentifier.Create (uuid, null), "qualifierData");
		}
	}
}

#pragma warning restore APL0002

#endif
