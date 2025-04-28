// 
// UIControl.cs: Support for events in C# speak.
//
// Authors:
//   Miguel de Icaza
//     
// Copyright 2009 Novell, Inc
//

using Foundation;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ObjCRuntime;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace UIKit {
	[Register]
	class UIControlEventProxy : NSObject {
		EventHandler eh;
		UIControl source;
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
		static ConditionalWeakTable<UIControl, Dictionary<EventHandler, Dictionary<UIControlEvent, UIControlEventProxy>>> allTargets;
		/// <param name="notification">EventHandler to invoke.</param>
		///         <param name="events">Event mask that will trigger the event to be invoked.</param>
		///         <summary>Adds an event handler for the specified set of events.</summary>
		///         <remarks>The <paramref name="notification" /> handler will be invoked when the control receives any of the events listed in the <paramref name="events" /> mask.   The </remarks>
		public void AddTarget (EventHandler notification, UIControlEvent events)
		{
			if (allTargets is null)
				allTargets = new ();

			var targets = allTargets.GetValue (this, k => {
				MarkDirty ();
				return new Dictionary<EventHandler, Dictionary<UIControlEvent, UIControlEventProxy>> ();
			});

			Dictionary<UIControlEvent, UIControlEventProxy> t;
			if (!targets.TryGetValue (notification, out t)) {
				t = new Dictionary<UIControlEvent, UIControlEventProxy> ();
				targets [notification] = t;
			}

			UIControlEventProxy ep;

			if (!t.TryGetValue (events, out ep)) {
				ep = new UIControlEventProxy (this, notification);
				t [events] = ep;
				AddTarget (ep, Selector.GetHandle (UIControlEventProxy.BridgeSelector), events);
			} else {
				ep.Counter++;
			}
		}

		/// <param name="notification">The event handler previously specified in AddTarget.</param>
		///         <param name="events">The event mask to remove.</param>
		///         <summary>Removes a previously installed event handler for the specified event list.</summary>
		///         <remarks>
		///         </remarks>
		public void RemoveTarget (EventHandler notification, UIControlEvent events)
		{
			Dictionary<EventHandler, Dictionary<UIControlEvent, UIControlEventProxy>> targets;

			if (allTargets is null)
				return;

			if (!allTargets.TryGetValue (this, out targets))
				return;

			Dictionary<UIControlEvent, UIControlEventProxy> t;
			if (!targets.TryGetValue (notification, out t))
				return;

			UIControlEventProxy ep;
			if (!t.TryGetValue (events, out ep))
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
		/// <remarks>To be added.</remarks>
		public event EventHandler TouchDown {
			add {
				AddTarget (value, UIControlEvent.TouchDown);
			}
			remove {
				RemoveTarget (value, UIControlEvent.TouchDown);
			}
		}

		/// <summary>Raised when the user double taps the control.</summary>
		/// <remarks>To be added.</remarks>
		public event EventHandler TouchDownRepeat {
			add {
				AddTarget (value, UIControlEvent.TouchDownRepeat);
			}
			remove {
				RemoveTarget (value, UIControlEvent.TouchDownRepeat);
			}
		}

		/// <summary>Raised oN TouchDragInside events.</summary>
		/// <remarks>To be added.</remarks>
		public event EventHandler TouchDragInside {
			add {
				AddTarget (value, UIControlEvent.TouchDragInside);
			}
			remove {
				RemoveTarget (value, UIControlEvent.TouchDragInside);
			}
		}

		/// <summary>Raised on TouchDragOutside events.</summary>
		/// <remarks>To be added.</remarks>
		public event EventHandler TouchDragOutside {
			add {
				AddTarget (value, UIControlEvent.TouchDragOutside);
			}
			remove {
				RemoveTarget (value, UIControlEvent.TouchDragOutside);
			}
		}

		/// <summary>Raised on TouchDragEnter events.</summary>
		/// <remarks>To be added.</remarks>
		public event EventHandler TouchDragEnter {
			add {
				AddTarget (value, UIControlEvent.TouchDragEnter);
			}
			remove {
				RemoveTarget (value, UIControlEvent.TouchDragEnter);
			}
		}

		/// <summary>Raised on TouchDragExit events.</summary>
		/// <remarks>To be added.</remarks>
		public event EventHandler TouchDragExit {
			add {
				AddTarget (value, UIControlEvent.TouchDragExit);
			}
			remove {
				RemoveTarget (value, UIControlEvent.TouchDragExit);
			}
		}

		/// <summary>Raised on TouchUpInside events.</summary>
		/// <remarks>To be added.</remarks>
		public event EventHandler TouchUpInside {
			add {
				AddTarget (value, UIControlEvent.TouchUpInside);
			}
			remove {
				RemoveTarget (value, UIControlEvent.TouchUpInside);
			}
		}

		/// <summary>Raised on TouchUpOutside events.</summary>
		/// <remarks>To be added.</remarks>
		public event EventHandler TouchUpOutside {
			add {
				AddTarget (value, UIControlEvent.TouchUpOutside);
			}
			remove {
				RemoveTarget (value, UIControlEvent.TouchUpOutside);
			}
		}

		/// <summary>The touch event has been canceled.</summary>
		/// <remarks>To be added.</remarks>
		public event EventHandler TouchCancel {
			add {
				AddTarget (value, UIControlEvent.TouchCancel);
			}
			remove {
				RemoveTarget (value, UIControlEvent.TouchCancel);
			}
		}

		/// <summary>The value has changed.</summary>
		/// <remarks>To be added.</remarks>
		public event EventHandler ValueChanged {
			add {
				AddTarget (value, UIControlEvent.ValueChanged);
			}
			remove {
				RemoveTarget (value, UIControlEvent.ValueChanged);
			}
		}

#if NET
		/// <summary>Event associated with the most-likely behavior of the <see cref="T:UIKit.UIControl" />.</summary>
		/// <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		public event EventHandler PrimaryActionTriggered {
			add {
				AddTarget (value, UIControlEvent.PrimaryActionTriggered);
			}
			remove {
				RemoveTarget (value, UIControlEvent.PrimaryActionTriggered);
			}
		}

		/// <summary>Raised when editing has started.</summary>
		/// <remarks>To be added.</remarks>
		public event EventHandler EditingDidBegin {
			add {
				AddTarget (value, UIControlEvent.EditingDidBegin);
			}
			remove {
				RemoveTarget (value, UIControlEvent.EditingDidBegin);
			}
		}

		/// <summary>The component changed.</summary>
		/// <remarks>To be added.</remarks>
		public event EventHandler EditingChanged {
			add {
				AddTarget (value, UIControlEvent.EditingChanged);
			}
			remove {
				RemoveTarget (value, UIControlEvent.EditingChanged);
			}
		}

		/// <summary>Raised when editing ended.</summary>
		/// <remarks>To be added.</remarks>
		public event EventHandler EditingDidEnd {
			add {
				AddTarget (value, UIControlEvent.EditingDidEnd);
			}
			remove {
				RemoveTarget (value, UIControlEvent.EditingDidEnd);
			}
		}

		/// <summary>Raised on didEndOnexit</summary>
		/// <remarks>To be added.</remarks>
		public event EventHandler EditingDidEndOnExit {
			add {
				AddTarget (value, UIControlEvent.EditingDidEndOnExit);
			}
			remove {
				RemoveTarget (value, UIControlEvent.EditingDidEndOnExit);
			}
		}

		/// <summary>Raised for any touch event produced.</summary>
		/// <remarks>To be added.</remarks>
		public event EventHandler AllTouchEvents {
			add {
				AddTarget (value, UIControlEvent.AllTouchEvents);
			}
			remove {
				RemoveTarget (value, UIControlEvent.AllTouchEvents);
			}
		}

		/// <summary>Raised on any editing events produced.</summary>
		/// <remarks>To be added.</remarks>
		public event EventHandler AllEditingEvents {
			add {
				AddTarget (value, UIControlEvent.AllEditingEvents);
			}
			remove {
				RemoveTarget (value, UIControlEvent.AllEditingEvents);
			}
		}

		/// <summary>Raised for any event produced.</summary>
		/// <remarks>To be added.</remarks>
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
