// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if __MACOS__
using ApplicationServices;

namespace MonoTouchFixtures.ApplicationServices {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AXUIElementTest {
		[Test]
		public void Application ()
		{
			using var element = AXUIElement.CreateApplication (Environment.ProcessId);
			Assert.That (element.Handle, Is.Not.EqualTo (NativeHandle.Zero), "Handle");
			Assert.That ((nuint) CFType.GetTypeID (element.Handle), Is.EqualTo (AXUIElement.GetTypeID ()), "Type ID");
			Assert.That (element.GetPid (out var pid), Is.EqualTo (AXError.Success), "Result");
			Assert.That (pid, Is.EqualTo (Environment.ProcessId), "PID");
			Assert.That (element.SetMessagingTimeout (0), Is.EqualTo (AXError.Success), "Reset timeout");
			Assert.That (element.SetMessagingTimeout (-1), Is.EqualTo (AXError.IllegalArgument), "Negative timeout");
		}

		[Test]
		public void SystemWide ()
		{
			using var element = AXUIElement.CreateSystemWide ();
			Assert.That ((nuint) CFType.GetTypeID (element.Handle), Is.EqualTo (AXUIElement.GetTypeID ()), "Type ID");
		}

		[Test]
		public void TrustWithoutPrompt ()
		{
			var trusted = AXUIElement.IsProcessTrusted;
			Assert.That (AXUIElement.IsProcessTrustedWithOptions (null), Is.EqualTo (trusted), "Default options");
			using var options = NSDictionary.FromObjectAndKey (NSNumber.FromBoolean (false), AXTrustedCheckOptions.Prompt);
			Assert.That (AXUIElement.IsProcessTrustedWithOptions (options), Is.EqualTo (trusted), "No prompt");
			Assert.That ((string) AXTrustedCheckOptions.Prompt, Is.EqualTo ("AXTrustedCheckOptionPrompt"), "Prompt key");
		}

		[Test]
		public void InvalidApplication ()
		{
			using var element = AXUIElement.CreateApplication (int.MaxValue);
			Assert.That (element.GetAttributeNames (out var attributes), Is.Not.EqualTo (AXError.Success), "Attribute names result");
			Assert.That (attributes, Is.Null, "Attribute names");
			Assert.That (element.GetAttributeValue<NSString> ("AXTitle", out var title), Is.Not.EqualTo (AXError.Success), "Attribute result");
			Assert.That (title, Is.Null, "Attribute");
			Assert.That (element.GetAttributeValues ("AXChildren", 0, 1, out var children), Is.Not.EqualTo (AXError.Success), "Attribute values result");
			Assert.That (children, Is.Null, "Attribute values");
			Assert.That (element.GetAttributeValueCount ("AXChildren", out var count), Is.Not.EqualTo (AXError.Success), "Attribute count result");
			Assert.That (element.IsAttributeSettable ("AXTitle", out var settable), Is.Not.EqualTo (AXError.Success), "Settable result");
			using var value = new NSString ("Title");
			Assert.That (element.SetAttributeValue ("AXTitle", value), Is.Not.EqualTo (AXError.Success), "Set attribute");
			Assert.That (element.GetMultipleAttributeValues (["AXTitle", "AXRole"], AXCopyMultipleAttributeOptions.None, out var values), Is.Not.EqualTo (AXError.Success), "Multiple attributes result");
			Assert.That (values, Is.Null, "Multiple attributes");
			Assert.That (element.GetParameterizedAttributeNames (out var parameterized), Is.Not.EqualTo (AXError.Success), "Parameterized names result");
			Assert.That (parameterized, Is.Null, "Parameterized names");
			using var parameter = new AXValue (new CFRange (0, 1));
			Assert.That (element.GetParameterizedAttributeValue<NSString> ("AXStringForRange", parameter, out var text), Is.Not.EqualTo (AXError.Success), "Parameterized attribute result");
			Assert.That (text, Is.Null, "Parameterized attribute");
			Assert.That (element.GetActionNames (out var actions), Is.Not.EqualTo (AXError.Success), "Actions result");
			Assert.That (actions, Is.Null, "Actions");
			Assert.That (element.GetActionDescription ("AXPress", out var description), Is.Not.EqualTo (AXError.Success), "Action description result");
			Assert.That (description, Is.Null, "Action description");
			Assert.That (element.PerformAction ("AXPress"), Is.Not.EqualTo (AXError.Success), "Perform action");
			Assert.That (element.GetElementAtPosition (0, 0, out var found), Is.Not.EqualTo (AXError.Success), "Element at position result");
			Assert.That (found, Is.Null, "Element at position");
		}

		[Test]
		public void InvalidArguments ()
		{
			using var element = AXUIElement.CreateSystemWide ();
			Assert.Throws<ArgumentNullException> (() => element.GetAttributeValue<NSString> (null, out var value), "Null attribute");
			Assert.Throws<ArgumentNullException> (() => element.SetAttributeValue ("AXTitle", null), "Null value");
			Assert.Throws<ArgumentNullException> (() => element.GetMultipleAttributeValues (null, AXCopyMultipleAttributeOptions.None, out var values), "Null attributes");
			Assert.Throws<ArgumentNullException> (() => element.GetParameterizedAttributeValue<NSString> ("AXStringForRange", null, out var value), "Null parameter");
			Assert.Throws<ArgumentNullException> (() => element.PerformAction (null), "Null action");
		}

		[Test]
		public void Disposed ()
		{
			var element = AXUIElement.CreateApplication (Environment.ProcessId);
			element.Dispose ();
			Assert.Throws<ObjectDisposedException> (() => element.GetPid (out var pid), "PID");
			Assert.Throws<ObjectDisposedException> (() => element.GetAttributeNames (out var names), "Attribute names");
			Assert.Throws<ObjectDisposedException> (() => element.PerformAction ("AXPress"), "Action");
		}

		[Test]
		public void NativeWrapperOwnership ()
		{
			using var original = AXUIElement.CreateApplication (Environment.ProcessId);
			using var wrapper = Runtime.GetINativeObject<AXUIElement> (original.Handle, false);
			Assert.That (wrapper, Is.Not.Null, "Wrapper");
			original.Dispose ();
			Assert.That (wrapper.GetPid (out var pid), Is.EqualTo (AXError.Success), "Result");
			Assert.That (pid, Is.EqualTo (Environment.ProcessId), "PID");
		}
	}
}
#endif
