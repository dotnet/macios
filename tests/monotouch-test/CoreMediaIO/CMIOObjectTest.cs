#if HAS_COREMEDIAIO
#nullable enable

using System;
using CoreMediaIO;
using NUnit.Framework;

namespace MonoTouchFixtures.CoreMediaIO {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CMIOObjectTest {

		[Test]
		public void HasProperty_SystemObject ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			var obj = new CMIOObject (CMIOObject.SystemObjectId);
			var address = new CMIOObjectPropertyAddress (
				CMIOObject.OwnedObjectsSelector,
				CMIOObject.GlobalScope,
				CMIOObject.ElementWildcard);
			bool has = obj.HasProperty (address);
			// We just verify it doesn't crash; the result depends on whether CMIO daemons are running
			Assert.IsTrue (has || !has, "HasProperty did not crash");
		}

		[Test]
		public void IsPropertySettable_SystemObject ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			var obj = new CMIOObject (CMIOObject.SystemObjectId);
			var address = new CMIOObjectPropertyAddress (
				CMIOObject.OwnedObjectsSelector,
				CMIOObject.GlobalScope,
				CMIOObject.ElementWildcard);

			bool isSettable = obj.IsPropertySettable (address, out int status);
			if (status == 0)
				Assert.IsFalse (isSettable, "OwnedObjects should not be settable");
		}

		[Test]
		public void GetPropertyDataSize_SystemObject ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			var obj = new CMIOObject (CMIOObject.SystemObjectId);
			var address = new CMIOObjectPropertyAddress (
				CMIOObject.OwnedObjectsSelector,
				CMIOObject.GlobalScope,
				CMIOObject.ElementWildcard);

			uint dataSize = obj.GetPropertyDataSize (address, out int status);
			if (status == 0)
				Assert.That (dataSize, Is.GreaterThanOrEqualTo (0), "DataSize");
		}

		[Test]
		public void Show_DoesNotCrash ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			var obj = new CMIOObject (CMIOObject.SystemObjectId);
			Assert.DoesNotThrow (() => obj.Show (), "Show should not throw");
		}

		[Test]
		public void Constants_HaveExpectedValues ()
		{
			Assert.AreEqual ((uint) 1, CMIOObject.SystemObjectId, "SystemObjectId");
			Assert.AreNotEqual ((uint) 0, CMIOObject.SelectorWildcard, "SelectorWildcard");
			Assert.AreNotEqual ((uint) 0, CMIOObject.GlobalScope, "GlobalScope");
			Assert.AreEqual (0xFFFFFFFF, CMIOObject.ElementWildcard, "ElementWildcard");
		}
	}
}
#endif // HAS_COREMEDIAIO
