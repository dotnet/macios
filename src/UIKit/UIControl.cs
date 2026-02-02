// 
// UIControl.cs: Support for events in C# speak.
//
// Authors:
//   Miguel de Icaza
//     
// Copyright 2009 Novell, Inc
//

using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable enable

namespace UIKit {
	[Register]
	class UIControlEventProxy : NSObject {
		EventHandler? eh;
		UIControl? source;
		internal int Counter = 1;
		internal const string BridgeSelector = "BridgeSelector";

		public UIControlEventProxy (UIControl source, EventHandler eh)
		{
			IsDirectBinding = false;
			this.source = source;
			this.eh = eh;
		}

		[Export (BridgeSelector)]
		[Preserve (Conditional = true)]
		public void Activated ()
		{
			if (eh is null)
				return;

			for (int i = 0; i < Counter; i++)
				eh (source, EventArgs.Empty);
		}

		protected override void Dispose (bool disposing)
		{
			eh = null;
			source = null;
			base.Dispose (disposing);
		}
	}

	public partial class UIControl {
		static ConditionalWeakTable<UIControl, Dictionary<EventHandler, Dictionary<UIControlEvent, UIControlEventProxy>>>? allTargets;

		/// <summary>Adds an event handler for the specified set of events.</summary>
		/// <param name="notification">The event handler to invoke.</param>
		/// <param name="events">The event mask that will trigger the event to be invoked.</param>
		/// <remarks>The <paramref name="notification" /> handler will be invoked when the control receives any of the events listed in the <paramref name="events" /> mask.</remarks>
		public void AddTarget (EventHandler notification, UIControlEvent events)
		{
			if (allTargets is null)
				allTargets = new ();

			var targets = allTargets.GetValue (this, k => {
				MarkDirty ();
				return new Dictionary<EventHandler, Dictionary<UIControlEvent, UIControlEventProxy>> ();
			});

			if (!targets.TryGetValue (notification, out var t)) {
				t = new Dictionary<UIControlEvent, UIControlEventProxy> ();
				targets [notification] = t;
			}

			if (!t.TryGetValue (events, out var ep)) {
				ep = new UIControlEventProxy (this, notification);
				t [events] = ep;
				AddTarget (ep, Selector.GetHandle (UIControlEventProxy.BridgeSelector), events);
			} else {
				ep.Counter++;
			}
		}

		/// <summary>Removes a previously installed event handler for the specified event list.</summary>
		/// <param name="notification">The event handler previously specified in <see cref="AddTarget(EventHandler, UIControlEvent)" />.</param>
		/// <param name="events">The event mask to remove.</param>
		public void RemoveTarget (EventHandler notification, UIControlEvent events)
		{
			if (allTargets is null)
				return;

			if (!allTargets.TryGetValue (this, out var targets))
				return;

			if (!targets.TryGetValue (notification, out var t))
				return;

			if (!t.TryGetValue (events, out var ep))
				return;

			ep.Counter--;
			if (ep.Counter > 1)
				return;

			RemoveTarget (ep, Selector.GetHandle (UIControlEventProxy.BridgeSelector), events);
			t.Remove (events);
			ep.Dispose ();
			if (t.Count == 0)
				targets.Remove (notification);
		}

		/// <summary>Raised when the user touches the control.</summary>
		public event EventHandler TouchDown {
			add {
				AddTarget (value, UIControlEvent.TouchDown);
			}
			remove {
				RemoveTarget (value, UIControlEvent.TouchDown);
			}
		}

		/// <summary>Raised when the user double taps the control.</summary>
		public event EventHandler TouchDownRepeat {
			add {
				AddTarget (value, UIControlEvent.TouchDownRepeat);
			}
			remove {
				RemoveTarget (value, UIControlEvent.TouchDownRepeat);
			}
		}

		/// <summary>Raised on <see cref="UIControlEvent.TouchDragInside" /> events.</summary>
		public event EventHandler TouchDragInside {
			add {
				AddTarget (value, UIControlEvent.TouchDragInside);
			}
			remove {
				RemoveTarget (value, UIControlEvent.TouchDragInside);
			}
		}

		/// <summary>Raised on <see cref="UIControlEvent.TouchDragOutside" /> events.</summary>
		public event EventHandler TouchDragOutside {
			add {
				AddTarget (value, UIControlEvent.TouchDragOutside);
			}
			remove {
				RemoveTarget (value, UIControlEvent.TouchDragOutside);
			}
		}

		/// <summary>Raised on <see cref="UIControlEvent.TouchDragEnter" /> events.</summary>
		public event EventHandler TouchDragEnter {
			add {
				AddTarget (value, UIControlEvent.TouchDragEnter);
			}
			remove {
				RemoveTarget (value, UIControlEvent.TouchDragEnter);
			}
		}

		/// <summary>Raised on <see cref="UIControlEvent.TouchDragExit" /> events.</summary>
		public event EventHandler TouchDragExit {
			add {
				AddTarget (value, UIControlEvent.TouchDragExit);
			}
			remove {
				RemoveTarget (value, UIControlEvent.TouchDragExit);
			}
		}

		/// <summary>Raised on <see cref="UIControlEvent.TouchUpInside" /> events.</summary>
		public event EventHandler TouchUpInside {
			add {
				AddTarget (value, UIControlEvent.TouchUpInside);
			}
			remove {
				RemoveTarget (value, UIControlEvent.TouchUpInside);
			}
		}

		/// <summary>Raised on <see cref="UIControlEvent.TouchUpOutside" /> events.</summary>
		public event EventHandler TouchUpOutside {
			add {
				AddTarget (value, UIControlEvent.TouchUpOutside);
			}
			remove {
				RemoveTarget (value, UIControlEvent.TouchUpOutside);
			}
		}

		/// <summary>Raised when the touch event has been canceled.</summary>
		public event EventHandler TouchCancel {
			add {
				AddTarget (value, UIControlEvent.TouchCancel);
			}
			remove {
				RemoveTarget (value, UIControlEvent.TouchCancel);
			}
		}

		/// <summary>Raised when the value has changed.</summary>
		public event EventHandler ValueChanged {
			add {
				AddTarget (value, UIControlEvent.ValueChanged);
			}
			remove {
				RemoveTarget (value, UIControlEvent.ValueChanged);
			}
		}

		/// <summary>Raised when the primary action associated with this <see cref="UIControl" /> is triggered.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public event EventHandler PrimaryActionTriggered {
			add {
				AddTarget (value, UIControlEvent.PrimaryActionTriggered);
			}
			remove {
				RemoveTarget (value, UIControlEvent.PrimaryActionTriggered);
			}
		}

		/// <summary>Raised when editing has started.</summary>
		public event EventHandler EditingDidBegin {
			add {
				AddTarget (value, UIControlEvent.EditingDidBegin);
			}
			remove {
				RemoveTarget (value, UIControlEvent.EditingDidBegin);
			}
		}

		/// <summary>Raised when the editing value has changed.</summary>
		public event EventHandler EditingChanged {
			add {
				AddTarget (value, UIControlEvent.EditingChanged);
			}
			remove {
				RemoveTarget (value, UIControlEvent.EditingChanged);
			}
		}

		/// <summary>Raised when editing has ended.</summary>
		public event EventHandler EditingDidEnd {
			add {
				AddTarget (value, UIControlEvent.EditingDidEnd);
			}
			remove {
				RemoveTarget (value, UIControlEvent.EditingDidEnd);
			}
		}

		/// <summary>Raised when editing has ended and the user exited the control.</summary>
		public event EventHandler EditingDidEndOnExit {
			add {
				AddTarget (value, UIControlEvent.EditingDidEndOnExit);
			}
			remove {
				RemoveTarget (value, UIControlEvent.EditingDidEndOnExit);
			}
		}

		/// <summary>Raised for any touch event.</summary>
		public event EventHandler AllTouchEvents {
			add {
				AddTarget (value, UIControlEvent.AllTouchEvents);
			}
			remove {
				RemoveTarget (value, UIControlEvent.AllTouchEvents);
			}
		}

		/// <summary>Raised for any editing event.</summary>
		public event EventHandler AllEditingEvents {
			add {
				AddTarget (value, UIControlEvent.AllEditingEvents);
			}
			remove {
				RemoveTarget (value, UIControlEvent.AllEditingEvents);
			}
		}

		/// <summary>Raised for any event.</summary>
		public event EventHandler AllEvents {
			add {
				AddTarget (value, UIControlEvent.AllEvents);
			}
			remove {
				RemoveTarget (value, UIControlEvent.AllEvents);
			}
		}
	}
}
