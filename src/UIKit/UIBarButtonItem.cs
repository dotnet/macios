using Foundation;
using ObjCRuntime;
using System;
using System.Diagnostics.CodeAnalysis;

#nullable enable

namespace UIKit {

	public partial class UIBarButtonItem {
		const string actionSel = "InvokeAction:";

		class Callback : NSObject {
			WeakReference<UIBarButtonItem> container;

			[DynamicDependency ("Call")]
			public Callback (UIBarButtonItem item)
			{
				container = new WeakReference<UIBarButtonItem> (item);
				IsDirectBinding = false;
			}

			[Export (actionSel)]
			public void Call (NSObject sender)
			{
				if (!container.TryGetTarget (out var obj) || obj is null)
					return;
				var clicked = obj.clicked;
				if (clicked is not null)
					clicked (sender, EventArgs.Empty);
			}
		}

		/// <param name="image">Image to be used in the button. If it is too large, the image is scaled to fit.</param>
		/// <param name="style">A style value defined in <see cref="UIKit.UIBarButtonItemStyle" />.</param>
		/// <param name="handler">The event handler to be called when the button is pressed.</param>
		/// <summary>Constructor that allows a custom image, style and event handler to be specied when the button is created.</summary>
		/// <remarks>Alpha values from the source image, ignoring opaque values, are used to create the image that appears on the button.</remarks>
		public UIBarButtonItem (UIImage image, UIBarButtonItemStyle style, EventHandler handler)
		: this (image, style, null, null)
		{
			Clicked += handler;
		}


		/// <param name="title">String value used to display the title of the button.</param>
		/// <param name="style">A style value defined in <see cref="UIKit.UIBarButtonItemStyle" />.</param>
		/// <param name="handler">The event handler to be called when the button is pressed.</param>
		/// <summary>Constructor that allows a title to be specified for display on the button depending on the style used. Also allows an event handler to be specified that will be called when the button is pressed.</summary>
		/// <remarks>Some <see cref="UIKit.UIBarButtonItemStyle" /> values display the title on the button while others display an image.</remarks>
		public UIBarButtonItem (string title, UIBarButtonItemStyle style, EventHandler handler)
		: this (title, style, null, null)
		{
			Clicked += handler;
		}


		/// <param name="systemItem">The <see cref="UIKit.UIBarButtonSystemItem" /> used to create the button.</param>
		/// <param name="handler">The event handler to be called when the button is pressed.</param>
		/// <summary>Constructor that allows a particular <see cref="UIKit.UIBarButtonSystemItem" /> to be specified when the button is created along with an event handler.</summary>
		/// <remarks>The event handler will be called when the button is pressed.</remarks>
		public UIBarButtonItem (UIBarButtonSystemItem systemItem, EventHandler handler)
		: this (systemItem, target: null, action: null)
		{
			Clicked += handler;
		}

		/// <param name="systemItem">The <see cref="UIKit.UIBarButtonSystemItem" /> used to create the button.</param>
		/// <summary>Constructor that allows a particular <see cref="UIKit.UIBarButtonSystemItem" /> to be specified when the button is created.</summary>
		/// <remarks>The <see cref="UIKit.UIBarButtonSystemItem" /> allows a number of buttons pre-defined by the system to be used when creating a UIBarButtonItem.</remarks>
		public UIBarButtonItem (UIBarButtonSystemItem systemItem) : this (systemItem: systemItem, target: null, action: null)
		{
		}

		Callback? callback;
		EventHandler? clicked;

		/// <summary>This event is raised when the user clicks/taps on this UIBarButtonItem.</summary>
		/// <remarks>To be added.</remarks>
		public event EventHandler Clicked {
			add {
				if (clicked is null) {
					callback = new Callback (this);
					this.Target = callback;
					this.Action = new Selector (actionSel);
					MarkDirty ();
				}

				clicked += value;
			}

			remove {
				clicked -= value;
			}
		}
	}
}
