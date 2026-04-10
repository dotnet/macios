#if HAS_COREMEDIAIO
#nullable enable

using System;
using CoreMediaIO;
using Foundation;
using NUnit.Framework;

namespace MonoTouchFixtures.CoreMediaIO {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CMIOExtensionClientTest {

		// CMIOExtensionClient cannot be created directly (DisableDefaultCtor),
		// but we can test the type exists and has the expected members.

		[Test]
		public void Type_Exists ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			var t = typeof (CMIOExtensionClient);
			Assert.IsNotNull (t, "Type");
			Assert.IsTrue (typeof (NSObject).IsAssignableFrom (t), "IsNSObject");
		}
	}
}
#endif // HAS_COREMEDIAIO
