// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if !MONOMAC

using Foundation;
using NUnit.Framework;
using UIKit;
using Xamarin.Utils;

namespace MonoTouchFixtures.UIKit {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ActionTest {
		[Test]
		public void CreateDefaults ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);

			using var action = UIAction.Create ("Title", null, _ => { });
			Assert.That (action.Title, Is.EqualTo ("Title"), "Title");
			Assert.That (action.Image, Is.Null, "Image");
			Assert.That (action.Identifier, Is.Not.Null.And.Not.Empty, "Identifier");
			Assert.That (action.DiscoverabilityTitle, Is.Null, "DiscoverabilityTitle");
			Assert.That (action.Attributes, Is.EqualTo ((UIMenuElementAttributes) 0), "Attributes");
			Assert.That (action.State, Is.EqualTo (UIMenuElementState.Off), "State");
		}

		[TestCase (UIMenuElementAttributes.Destructive, UIMenuElementState.On)]
		[TestCase (UIMenuElementAttributes.Disabled | UIMenuElementAttributes.Destructive, UIMenuElementState.Mixed)]
		public void CreateConfigured (UIMenuElementAttributes attributes, UIMenuElementState state)
		{
			TestRuntime.AssertXcodeVersion (11, 0);

			using var image = new UIImage ();
			using var action = UIAction.Create ("Title", image, _ => { },
				identifier: "Identifier", discoverabilityTitle: "Discoverability",
				attributes: attributes, state: state);
			Assert.That (action.Title, Is.EqualTo ("Title"), "Title");
			Assert.That (action.Image, Is.EqualTo (image), "Image");
			Assert.That (action.Identifier, Is.EqualTo ("Identifier"), "Identifier");
			Assert.That (action.DiscoverabilityTitle, Is.EqualTo ("Discoverability"), "DiscoverabilityTitle");
			Assert.That (action.Attributes, Is.EqualTo (attributes), "Attributes");
			Assert.That (action.State, Is.EqualTo (state), "State");
		}

		[Test]
		public void CreateWithAttributesAndHandler ()
		{
			TestRuntime.AssertXcodeVersion (12, 0);

			var invoked = false;
			using var action = UIAction.Create ("Delete", null,
				attributes: UIMenuElementAttributes.Destructive,
				handler: a => {
					Assert.That (a.Title, Is.EqualTo ("Delete"), "Handler title");
					Assert.That (a.Attributes, Is.EqualTo (UIMenuElementAttributes.Destructive), "Handler attributes");
					invoked = true;
				});
			using var control = new UIControl ();
			control.AddAction (action, UIControlEvent.TouchUpInside);
			control.SendActionForControlEvents (UIControlEvent.TouchUpInside);
			Assert.That (invoked, Is.True, "Handler invoked");
		}
	}
}

#endif
