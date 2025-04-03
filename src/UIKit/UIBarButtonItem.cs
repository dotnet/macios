using Foundation;
using ObjCRuntime;
using System;
using System.Diagnostics.CodeAnalysis;

#nullable enable

namespace UIKit {

	[Category (typeof (UIBarButtonItem))]
	static class UIBarButtonItem_Extensions {
		[Export (UIBarButtonItem.actionSelector)]
		static void Call (this UIBarButtonItem item, NSObject sender)
		{
			item.OnClicked (sender);
		}
	}

	public partial class UIBarButtonItem {
		internal const string actionSelector = "xamarinInvokeCallback:";

		[DynamicDependencyAttribute ("Call(UIKit.UIBarButtonItem,Foundation.NSObject)", typeof (UIBarButtonItem_Extensions))]
		static Selector actionSel = new Selector (actionSelector);

		/// <param name="image">Image to be used in the button. If it is too large, the image is scaled to fit.</param>
		///         <param name="style">A style value defined in <see cref="T:UIKit.UIBarButtonItemStyle" />.</param>
		///         <param name="handler">The event handler to be called when the button is pressed.</param>
		///         <summary>Constructor that allows a custom image, style and evnet handler to be specied when the button is created.</summary>
		///         <remarks>Alpha values from the source image, ignoring opaque values, are used to create the image that appears on the button.</remarks>
		public UIBarButtonItem (UIImage image, UIBarButtonItemStyle style, EventHandler handler)
		: this (image, style, null, actionSel)
		{
			Target = this;
			clicked += handler;
			MarkDirty ();
		}

		/// <param name="title">String value used to display the title of the button.</param>
		///         <param name="style">A style value defined in <see cref="T:UIKit.UIBarButtonItemStyle" />.</param>
		///         <param name="handler">The event handler to be called when the button is pressed.</param>
		///         <summary>Constructor that allows a title to be specified for display on the button depending on the style used. Also allows an event handler to be specified that will be called when the button is pressed.</summary>
		///         <remarks>Some <see cref="T:UIKit.UIBarButtonItemStyle" /> values display the title on the button while others display an image.</remarks>
		public UIBarButtonItem (string title, UIBarButtonItemStyle style, EventHandler handler)
		: this (title, style, null, actionSel)
		{
			Target = this;
			clicked += handler;
			MarkDirty ();
		}

		/// <param name="systemItem">The <see cref="T:UIKit.UIBarButtonSystemItem" /> used to create the button.</param>
		///         <param name="handler">The event handler to be called when the button is pressed.</param>
		///         <summary>Constructor that allows a particular <see cref="T:UIKit.UIBarButtonSystemItem" /> to be specified when the button is created along with an event handler.</summary>
		///         <remarks>The event handler will be called when the button is pressed.</remarks>
		public UIBarButtonItem (UIBarButtonSystemItem systemItem, EventHandler handler)
		: this (systemItem, null, actionSel)
		{
			Target = this;
			clicked += handler;
			MarkDirty ();
		}

		/// <param name="systemItem">The <see cref="T:UIKit.UIBarButtonSystemItem" /> used to create the button.</param>
		///         <summary>Constructor that allows a particular <see cref="T:UIKit.UIBarButtonSystemItem" /> to be specified when the button is created.</summary>
		///         <remarks>The <see cref="T:UIKit.UIBarButtonSystemItem" /> allows a number of buttons pre-defined by the system to be used when creating a UIBarButtonItem.</remarks>
		public UIBarButtonItem (UIBarButtonSystemItem systemItem) : this (systemItem: systemItem, target: null, action: null)
		{
		}

		[DynamicDependencyAttribute ("Call(UIKit.UIBarButtonItem,Foundation.NSObject)", typeof (UIBarButtonItem_Extensions))]
		EventHandler? clicked;

		internal void OnClicked (NSObject sender)
		{
			if (clicked is not null)
				clicked (sender, EventArgs.Empty);
		}

		public event EventHandler Clicked {
			add {
				if (clicked is null) {
					Target = this;
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
