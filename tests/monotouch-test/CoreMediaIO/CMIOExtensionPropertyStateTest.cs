#if HAS_COREMEDIAIO
#nullable enable

using System;
using CoreMediaIO;
using Foundation;
using NUnit.Framework;

namespace MonoTouchFixtures.CoreMediaIO {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CMIOExtensionPropertyStateTest {

		[Test]
		public void Create_WithValue ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			using var value = NSNumber.FromInt32 (42);
			var state = CMIOExtensionPropertyState.Create (value);
			Assert.IsNotNull (state, "Created state");
			Assert.IsNotNull (state.Value, "Value");
		}

		[Test]
		public void Create_WithNullValue ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			var state = CMIOExtensionPropertyState.Create (null);
			Assert.IsNotNull (state, "Created state");
			Assert.IsNull (state.Value, "Value should be null");
		}

		[Test]
		public void Create_WithValueAndAttributes ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			using var value = NSNumber.FromInt32 (50);
			var attrs = CMIOExtensionPropertyAttributes.Create (
				NSNumber.FromInt32 (0),
				NSNumber.FromInt32 (100),
				null,
				false);

			var state = CMIOExtensionPropertyState.Create (value, attrs);
			Assert.IsNotNull (state, "Created state");
			Assert.IsNotNull (state.Value, "Value");
			Assert.IsNotNull (state.Attributes, "Attributes");
			Assert.IsFalse (state.Attributes!.IsReadOnly, "IsReadOnly");
		}
	}
}
#endif // HAS_COREMEDIAIO
