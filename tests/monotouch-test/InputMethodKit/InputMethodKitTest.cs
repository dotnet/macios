// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if __MACOS__
using System;
using Foundation;
using InputMethodKit;
using NUnit.Framework;

namespace MonoTouchFixtures.InputMethodKit {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class InputMethodKitTest {
		[Test]
		public void Constants ()
		{
			Assert.Multiple (() => {
				Assert.That (IMKServer.ModeDictionary, Is.Not.Null, "ModeDictionary");
				Assert.That (IMKServer.ControllerClass, Is.Not.Null, "ControllerClass");
				Assert.That (IMKServer.DelegateClass, Is.Not.Null, "DelegateClass");
				Assert.That (IMKInputController.CommandMenuItemName, Is.Not.Null, "CommandMenuItemName");
				Assert.That (IMKInputController.CommandClientName, Is.Not.Null, "CommandClientName");
				Assert.That (IMKCandidates.OpacityAttributeName, Is.Not.Null, "OpacityAttributeName");
				Assert.That (IMKCandidates.SendServerKeyEventFirst, Is.Not.Null, "SendServerKeyEventFirst");
			});
		}

		[Test]
		public void Candidates ()
		{
			TestRuntime.IgnoreIfLockedScreen ();

			var bundleIdentifier = NSBundle.MainBundle.BundleIdentifier;
			Assert.That (bundleIdentifier, Is.Not.Null, "BundleIdentifier");
			if (bundleIdentifier is null)
				throw new InvalidOperationException ("The test application must have a bundle identifier.");

			using var server = new IMKServer ($"com.microsoft.macios.inputmethodkit.{Guid.NewGuid ():N}", bundleIdentifier);
			using var candidates = new IMKCandidates (server, IMKCandidatePanelType.SingleColumnScrolling);
			Assert.That (candidates.PanelType, Is.EqualTo (IMKCandidatePanelType.SingleColumnScrolling), "PanelType");

			ushort [] keys = [18, 19, 20];
			candidates.SelectionKeys = keys;
			Assert.That (candidates.SelectionKeys, Is.EqualTo (keys), "SelectionKeys");

			candidates.DismissesAutomatically = false;
			Assert.That (candidates.DismissesAutomatically, Is.False, "DismissesAutomatically/false");
			candidates.DismissesAutomatically = true;
			Assert.That (candidates.DismissesAutomatically, Is.True, "DismissesAutomatically/true");
		}
	}
}
#endif
