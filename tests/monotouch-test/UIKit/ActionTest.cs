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
		[TestCase (false)]
		[TestCase (true)]
		public void CreateDefaults (bool constructor)
		{
			TestRuntime.AssertXcodeVersion (11, 0);

			using var action = constructor ? new UIAction ("Title", null, _ => { }) : UIAction.Create ("Title", null, _ => { });
			Assert.That (action.Title, Is.EqualTo ("Title"), "Title");
			Assert.That (action.Image, Is.Null, "Image");
			Assert.That (action.Identifier, Is.Not.Null.And.Not.Empty, "Identifier");
			Assert.That (action.DiscoverabilityTitle, Is.Null, "DiscoverabilityTitle");
			Assert.That (action.Attributes, Is.EqualTo ((UIMenuElementAttributes) 0), "Attributes");
			Assert.That (action.State, Is.EqualTo (UIMenuElementState.Off), "State");
		}

		[TestCase (false, UIMenuElementAttributes.Destructive, UIMenuElementState.On)]
		[TestCase (true, UIMenuElementAttributes.Destructive, UIMenuElementState.On)]
		[TestCase (false, UIMenuElementAttributes.Disabled | UIMenuElementAttributes.Destructive, UIMenuElementState.Mixed)]
		[TestCase (true, UIMenuElementAttributes.Disabled | UIMenuElementAttributes.Destructive, UIMenuElementState.Mixed)]
		public void CreateConfigured (bool constructor, UIMenuElementAttributes attributes, UIMenuElementState state)
		{
			TestRuntime.AssertXcodeVersion (11, 0);

			using var image = new UIImage ();
			using var action = constructor
				? new UIAction ("Title", image, _ => { }, identifier: "Identifier", discoverabilityTitle: "Discoverability", attributes: attributes, state: state)
				: UIAction.Create ("Title", image, _ => { }, identifier: "Identifier", discoverabilityTitle: "Discoverability", attributes: attributes, state: state);
			Assert.That (action.Title, Is.EqualTo ("Title"), "Title");
			Assert.That (action.Image, Is.EqualTo (image), "Image");
			Assert.That (action.Identifier, Is.EqualTo ("Identifier"), "Identifier");
			Assert.That (action.DiscoverabilityTitle, Is.EqualTo ("Discoverability"), "DiscoverabilityTitle");
			Assert.That (action.Attributes, Is.EqualTo (attributes), "Attributes");
			Assert.That (action.State, Is.EqualTo (state), "State");
		}

		[Test]
		public void ConstructorObjectInitializer ()
		{
			TestRuntime.AssertXcodeVersion (12, 0);

			UIAction? invokedAction = null;
			UIAction action;
			using (var pool = new NSAutoreleasePool ()) {
				action = new UIAction ("Delete", null, a => invokedAction = a) {
					Attributes = UIMenuElementAttributes.Destructive,
					State = UIMenuElementState.On,
				};
			}
			using var disposableAction = action;
			using var control = new UIControl ();
			control.AddAction (action, UIControlEvent.TouchUpInside);
			control.SendActionForControlEvents (UIControlEvent.TouchUpInside);
			Assert.That (invokedAction, Is.SameAs (action), "Handler action");
			Assert.That (action.Attributes, Is.EqualTo (UIMenuElementAttributes.Destructive), "Attributes");
			Assert.That (action.State, Is.EqualTo (UIMenuElementState.On), "State");
		}

		[Test]
		public void ConstructorNullArguments ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);

#pragma warning disable CS8625 // Deliberately pass null to verify argument validation.
			Assert.Throws<System.ArgumentNullException> (() => new UIAction (null, null, _ => { }), "Title");
			Assert.Throws<System.ArgumentNullException> (() => new UIAction ("Title", null, null), "Handler");
#pragma warning restore CS8625
		}

		class DerivedAction : UIAction {
			public DerivedAction () : base ("Title", null, _ => { })
			{
			}
		}

		[Test]
		public void ConstructorManagedSubclass ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);

			Assert.Throws<System.NotSupportedException> (() => new DerivedAction ());
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
