// Copyright 2011 Xamarin Inc. All rights reserved

#if !MONOMAC

using System.Drawing;
using System.Reflection;

using CoreGraphics;
using UIKit;

namespace MonoTouchFixtures.UIKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ControlTest {

		[Test]
		public void InitWithFrame ()
		{
			var frame = new CGRect (10, 10, 100, 100);
			using (UIControl c = new UIControl (frame)) {
				Assert.That (c.Frame, Is.EqualTo (frame), "Frame");
			}
		}

		[Test]
		public void CancelTrackingTest ()
		{
			using (var c = new UIControl ()) {
				c.CancelTracking (null);
				c.CancelTracking (new UIEvent ());
			}
		}

		[Test]
		public void AddTargetTable ()
		{
			const int items = 100;
			var handles = new GCHandle [items];
			var handler = new EventHandler (delegate (object sender, EventArgs e)
			{

			});
			for (int i = 0; i < items; i++) {
				var ctrl = new UIControl ();
				ctrl.AddTarget (handler, UIControlEvent.EditingChanged);
				handles [i] = GCHandle.Alloc (ctrl, GCHandleType.Weak);
			}
			GC.Collect ();

			// If at least one object was collected, then we know the static ConditionalWeakTable
			// of object -> event handlers doesn't keep strong references.
			var any_collected = false;
			for (int i = 0; i < items; i++) {
				if (handles [i].Target is null)
					any_collected = true;
				handles [i].Free ();
			}
			Assert.That (any_collected, Is.True, "Nothing collected");
		}

		[Test]
		public void AddTargetMakeDirty ()
		{
			using (var ctrl = new UIControl ()) {
				ctrl.AddTarget ((a, b) => { }, UIControlEvent.EditingDidBegin);
				Assert.That (TestRuntime.GetFlags (ctrl) & 0x8, Is.EqualTo (0x8), "RegisteredToggleRef");
			}
		}

		[Test]
		public void TargetOverloadResolution ()
		{
			using (var ctrl = new UIControl ()) {
				ctrl.AddTarget ((sender, args) => Assert.That (args, Is.SameAs (EventArgs.Empty), "Lambda"), UIControlEvent.ValueChanged);
				var count = 0;
				ctrl.AddTarget (delegate { count++; }, UIControlEvent.ValueChanged);
				ctrl.SendActionForControlEvents (UIControlEvent.ValueChanged);
				Assert.That (count, Is.EqualTo (1), "Anonymous method");

				void Handler (object sender, object args)
				{
					Assert.That (args, Is.SameAs (EventArgs.Empty), "Method group");
					count++;
				}

				ctrl.AddTarget (Handler, UIControlEvent.EditingChanged);
				ctrl.SendActionForControlEvents (UIControlEvent.EditingChanged);
				Assert.That (count, Is.EqualTo (2), "Method group added");
				ctrl.RemoveTarget (Handler, UIControlEvent.EditingChanged);
				ctrl.SendActionForControlEvents (UIControlEvent.EditingChanged);
				Assert.That (count, Is.EqualTo (2), "Method group removed");
			}
		}

		[TestCase (0)]
		[TestCase (1)]
		[TestCase (2)]
		public void TargetCallbacks (int signature)
		{
			using (var ctrl = new UIControl ()) {
				var count = 0;
				Action<UIControl, UIEvent> callback = (sender, uievent) => {
					Assert.That (sender, Is.SameAs (ctrl), "Sender");
					Assert.That (uievent, Is.Null, "Programmatic event");
					count++;
				};
				Action<UIControlEvent> add;
				Action<UIControlEvent> remove;
				switch (signature) {
				case 0:
					EventHandler handler = (sender, args) => {
						Assert.That (args, Is.SameAs (EventArgs.Empty), "EventArgs");
						callback ((UIControl) sender, null);
					};
					add = events => ctrl.AddTarget (handler, events);
					remove = events => ctrl.RemoveTarget (handler, events);
					break;
				case 1:
					Action<UIControl> action = sender => callback (sender, null);
					add = events => ctrl.AddTarget (action, events);
					remove = events => ctrl.RemoveTarget (action, events);
					break;
				default:
					add = events => ctrl.AddTarget (callback, events);
					remove = events => ctrl.RemoveTarget (callback, events);
					break;
				}

				remove (UIControlEvent.ValueChanged);
				add (UIControlEvent.ValueChanged);
				Assert.That (TestRuntime.GetFlags (ctrl) & 0x8, Is.EqualTo (0x8), "RegisteredToggleRef");
				ctrl.SendActionForControlEvents (UIControlEvent.ValueChanged);
				Assert.That (count, Is.EqualTo (1), "Single registration");

				add (UIControlEvent.ValueChanged);
				ctrl.SendActionForControlEvents (UIControlEvent.ValueChanged);
				Assert.That (count, Is.EqualTo (3), "Duplicate registration");

				remove (UIControlEvent.ValueChanged);
				ctrl.SendActionForControlEvents (UIControlEvent.ValueChanged);
				Assert.That (count, Is.EqualTo (4), "One registration remaining");

				add (UIControlEvent.EditingChanged);
				remove (UIControlEvent.ValueChanged);
				ctrl.SendActionForControlEvents (UIControlEvent.ValueChanged);
				Assert.That (count, Is.EqualTo (4), "ValueChanged removed");
				ctrl.SendActionForControlEvents (UIControlEvent.EditingChanged);
				Assert.That (count, Is.EqualTo (5), "Other event retained");

				remove (UIControlEvent.EditingChanged);
				remove (UIControlEvent.EditingChanged);
				ctrl.SendActionForControlEvents (UIControlEvent.EditingChanged);
				Assert.That (count, Is.EqualTo (5), "All registrations removed");

				add (UIControlEvent.ValueChanged);
				ctrl.SendActionForControlEvents (UIControlEvent.ValueChanged);
				Assert.That (count, Is.EqualTo (6), "Re-registration");
				remove (UIControlEvent.ValueChanged);
			}
		}

		[Test]
		public void TargetWithEvent ()
		{
			using (var ctrl = new UIControl ())
			using (var uievent = new UIEvent ()) {
				var count = 0;
				Action<UIControl, UIEvent> callback = (sender, receivedEvent) => {
					Assert.That (sender, Is.SameAs (ctrl), "Sender");
					Assert.That (receivedEvent, Is.SameAs (uievent), "Event");
					count++;
				};
				ctrl.AddTarget (callback, UIControlEvent.TouchDown);
				using (var targets = ctrl.AllTargets) {
					Assert.That (targets.Count, Is.EqualTo ((nuint) 1), "Targets");
					var target = targets.AnyObject;
					var actions = ctrl.GetActions (target, UIControlEvent.TouchDown);
					Assert.That (actions, Has.Length.EqualTo (1), "Actions");
					ctrl.SendAction (new Selector (actions [0]), target, uievent);
				}
				Assert.That (count, Is.EqualTo (1), "Callback invoked");
				ctrl.RemoveTarget (callback, UIControlEvent.TouchDown);
				ctrl.SendActionForControlEvents (UIControlEvent.TouchDown);
				Assert.That (count, Is.EqualTo (1), "Callback removed");
			}
		}
	}
}

#endif // !MONOMAC
