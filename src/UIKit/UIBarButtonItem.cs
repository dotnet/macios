//
// Sanitize callbacks
//

using Foundation;
using ObjCRuntime;
using System;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace UIKit {

	public partial class UIBarButtonItem {
		static Selector actionSel = new Selector ("InvokeAction:");

		[Register]
		internal class Callback : NSObject {
			internal UIBarButtonItem container;

			public Callback ()
			{
				IsDirectBinding = false;
			}

			[Export ("InvokeAction:")]
			[Preserve (Conditional = true)]
			public void Call (NSObject sender)
			{
				if (container.clicked is not null)
					container.clicked (sender, EventArgs.Empty);
			}
		}

		/// <param name="image">Image to be used in the button. If it is too large, the image is scaled to fit.</param>
		///         <param name="style">A style value defined in <see cref="T:UIKit.UIBarButtonItemStyle" />.</param>
		///         <param name="handler">The event handler to be called when the button is pressed.</param>
		///         <summary>Constructor that allows a custom image, style and evnet handler to be specied when the button is created.</summary>
		///         <remarks>Alpha values from the source image, ignoring opaque values, are used to create the image that appears on the button.</remarks>
		public UIBarButtonItem (UIImage image, UIBarButtonItemStyle style, EventHandler handler)
		: this (image, style, new Callback (), actionSel)
		{
			callback = (Callback) Target;
			callback.container = this;
			clicked += handler;
			MarkDirty ();
		}


		/// <param name="title">String value used to display the title of the button.</param>
		///         <param name="style">A style value defined in <see cref="T:UIKit.UIBarButtonItemStyle" />.</param>
		///         <param name="handler">The event handler to be called when the button is pressed.</param>
		///         <summary>Constructor that allows a title to be specified for display on the button depending on the style used. Also allows an event handler to be specified that will be called when the button is pressed.</summary>
		///         <remarks>Some <see cref="T:UIKit.UIBarButtonItemStyle" /> values display the title on the button while others display an image.</remarks>
		public UIBarButtonItem (string title, UIBarButtonItemStyle style, EventHandler handler)
		: this (title, style, new Callback (), actionSel)
		{
			callback = (Callback) Target;
			callback.container = this;
			clicked += handler;
			MarkDirty ();
		}


		/// <param name="systemItem">The <see cref="T:UIKit.UIBarButtonSystemItem" /> used to create the button.</param>
		///         <param name="handler">The event handler to be called when the button is pressed.</param>
		///         <summary>Constructor that allows a particular <see cref="T:UIKit.UIBarButtonSystemItem" /> to be specified when the button is created along with an event handler.</summary>
		///         <remarks>The event handler will be called when the button is pressed.</remarks>
		public UIBarButtonItem (UIBarButtonSystemItem systemItem, EventHandler handler)
		: this (systemItem, new Callback (), actionSel)
		{
			callback = (Callback) Target;
			callback.container = this;
			clicked += handler;
			MarkDirty ();
		}

		/// <param name="systemItem">The <see cref="T:UIKit.UIBarButtonSystemItem" /> used to create the button.</param>
		///         <summary>Constructor that allows a particular <see cref="T:UIKit.UIBarButtonSystemItem" /> to be specified when the button is created.</summary>
		///         <remarks>The <see cref="T:UIKit.UIBarButtonSystemItem" /> allows a number of buttons pre-defined by the system to be used when creating a UIBarButtonItem.</remarks>
		public UIBarButtonItem (UIBarButtonSystemItem systemItem) : this (systemItem: systemItem, target: null, action: null)
		{
		}

		internal EventHandler clicked;
		internal Callback callback;

		public event EventHandler Clicked {
			add {
				if (clicked is null) {
					callback = new Callback ();
					callback.container = this;
					this.Target = callback;
					this.Action = actionSel;
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
