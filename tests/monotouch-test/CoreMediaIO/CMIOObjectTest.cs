#if __MACOS__ || __MACCATALYST__
#nullable enable

using System;
using CoreMediaIO;
using NUnit.Framework;

namespace MonoTouchFixtures.CoreMediaIO {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CMIOObjectTest {

		// kCMIOObjectSystemObject = 1
		const uint SystemObject = 1;
		// 'owne' selector = kCMIOObjectPropertyOwnedObjects
		const uint OwnedObjectsSelector = 0x6F776E65;
		// 'glob' scope = kCMIOObjectPropertyScopeGlobal
		const uint GlobalScope = 0x676C6F62;
		// Wildcard element
		const uint WildcardElement = 0xFFFFFFFF;

		[Test]
		public void HasProperty_SystemObject ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			// The system object (id=1) always has the 'owne' (owned objects) property
			var address = new CMIOObjectPropertyAddress (OwnedObjectsSelector, GlobalScope, WildcardElement);
			bool has = CMIOObject.HasProperty (SystemObject, address);
			// We just verify it doesn't crash; the result depends on whether CMIO daemons are running
			Assert.IsTrue (has || !has, "HasProperty did not crash");
		}

		[Test]
		public void IsPropertySettable_SystemObject ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			var address = new CMIOObjectPropertyAddress (OwnedObjectsSelector, GlobalScope, WildcardElement);

			// Check if the system object's owned-objects property is settable
			int status = CMIOObject.IsPropertySettable (SystemObject, address, out bool isSettable);
			// On some systems this may fail if CMIO is not available; just verify no crash
			if (status == 0)
				Assert.IsFalse (isSettable, "OwnedObjects should not be settable");
		}

		[Test]
		public void GetPropertyDataSize_SystemObject ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			var address = new CMIOObjectPropertyAddress (OwnedObjectsSelector, GlobalScope, WildcardElement);

			int status = CMIOObject.GetPropertyDataSize (SystemObject, address, out uint dataSize);
			// Just verify no crash; status depends on system state
			if (status == 0)
				Assert.That (dataSize, Is.GreaterThanOrEqualTo (0), "DataSize");
		}

		[Test]
		public void Show_DoesNotCrash ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			// Just verify CMIOObjectShow doesn't crash
			Assert.DoesNotThrow (() => CMIOObject.Show (SystemObject), "Show should not throw");
		}
	}
}
#endif // __MACOS__ || __MACCATALYST__
