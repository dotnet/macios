#if HAS_COREMEDIAIO
#nullable enable

using System;
using CoreMediaIO;
using Foundation;
using NUnit.Framework;

namespace MonoTouchFixtures.CoreMediaIO {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CMIOExtensionPropertyAttributesTest {

		[Test]
		public void ReadOnlyPropertyAttribute ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			var attrs = CMIOExtensionPropertyAttributes.ReadOnlyPropertyAttribute;
			Assert.IsNotNull (attrs, "ReadOnlyPropertyAttribute");
			Assert.IsTrue (attrs.IsReadOnly, "IsReadOnly");
		}

		[Test]
		public void Create_WithValues ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			using var min = NSNumber.FromInt32 (0);
			using var max = NSNumber.FromInt32 (100);

			var attrs = CMIOExtensionPropertyAttributes.Create (min, max, null, false);
			Assert.IsNotNull (attrs, "Created attributes");
			Assert.IsFalse (attrs.IsReadOnly, "IsReadOnly");
			Assert.IsNotNull (attrs.MinValue, "MinValue");
			Assert.IsNotNull (attrs.MaxValue, "MaxValue");
		}

		[Test]
		public void Create_ReadOnly ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			var attrs = CMIOExtensionPropertyAttributes.Create (null, null, null, true);
			Assert.IsNotNull (attrs, "Created attributes");
			Assert.IsTrue (attrs.IsReadOnly, "IsReadOnly");
			Assert.IsNull (attrs.MinValue, "MinValue");
			Assert.IsNull (attrs.MaxValue, "MaxValue");
			Assert.IsNull (attrs.ValidValues, "ValidValues");
		}
	}
}
#endif // HAS_COREMEDIAIO
