//
// Elements.cs: defines the various components of our view
//
// Author:
//   Miguel de Icaza (miguel@gnome.org)
//
// Copyright 2010, Novell, Inc.
//
// Code licensed under the MIT X11 license
//
// TODO: StyledStringElement: merge with multi-line?
// TODO: StyledStringElement: add image scaling features?
// TODO: StyledStringElement: add sizing based on image size?
// TODO: Move image rendering to StyledImageElement, reason to do this: the image loader would only be imported in this case, linked out otherwise
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using UIKit;
using CoreGraphics;
using Foundation;
using CoreAnimation;
using ObjCRuntime;
using NSAction = global::System.Action;

namespace MonoTouch.Dialog {
	/// <summary>
	/// Base class for all elements in MonoTouch.Dialog
	/// </summary>
	public partial class Element : IDisposable {
		/// <summary>
		///  Handle to the container object.
		/// </summary>
		/// <remarks>
		/// For sections this points to a RootElement, for every
		/// other object this points to a Section and it is null
		/// for the root RootElement.
		/// </remarks>
		public Element? Parent;

		/// <summary>
		///  The caption to display for this given element
		/// </summary>
		public string? Caption;

		/// <summary>
		///  Initializes the element with the given caption.
		/// </summary>
		/// <param name="caption">
		/// The caption.
		/// </param>
		public Element (string? caption)
		{
			this.Caption = caption;
		}

		public void Dispose ()
		{
			Dispose (true);
		}

		protected virtual void Dispose (bool disposing)
		{
		}

		static NSString cellkey = new NSString ("xx");
		/// <summary>
		/// Subclasses that override the GetCell method should override this method as well
		/// </summary>
		/// <remarks>
		/// This method should return the key passed to UITableView.DequeueReusableCell.
		/// If your code overrides the GetCell method to change the cell, you must also 
		/// override this method and return a unique key for it.
		/// 
		/// This works in most subclasses with a couple of exceptions: StringElement and
		/// various derived classes do not use this setting as they need a wider range
		/// of keys for different uses, so you need to look at the source code for those
		/// if you are trying to override StringElement or StyledStringElement.
		/// </remarks>
		protected virtual NSString CellKey {
			get {
				return cellkey;
			}
		}

		/// <summary>
		/// Gets a UITableViewCell for this element.   Can be overridden, but if you 
		/// customize the style or contents of the cell you must also override the CellKey 
		/// property in your derived class.
		/// </summary>
		public virtual UITableViewCell GetCell (UITableView tv)
		{
			return new UITableViewCell (UITableViewCellStyle.Default, CellKey);
		}

		static protected void RemoveTag (UITableViewCell cell, int tag)
		{
			var viewToRemove = cell.ContentView.ViewWithTag (tag);
			if (viewToRemove is not null)
				viewToRemove.RemoveFromSuperview ();
		}

		/// <summary>
		/// Returns a summary of the value represented by this object, suitable 
		/// for rendering as the result of a RootElement with child objects.
		/// </summary>
		/// <returns>
		/// The return value must be a short description of the value.
		/// </returns>
		public virtual string Summary ()
		{
			return "";
		}

		/// <summary>
		/// Invoked when the given element has been deslected by the user.
		/// </summary>
		/// <param name="dvc">
		/// The <see cref="DialogViewController"/> where the deselection took place
		/// </param>
		/// <param name="tableView">
		/// The <see cref="UITableView"/> that contains the element.
		/// </param>
		/// <param name="path">
		/// The <see cref="NSIndexPath"/> that contains the Section and Row for the element.
		/// </param>
		public virtual void Deselected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
		}

		/// <summary>
		/// Invoked when the given element has been selected by the user.
		/// </summary>
		/// <param name="dvc">
		/// The <see cref="DialogViewController"/> where the selection took place
		/// </param>
		/// <param name="tableView">
		/// The <see cref="UITableView"/> that contains the element.
		/// </param>
		/// <param name="path">
		/// The <see cref="NSIndexPath"/> that contains the Section and Row for the element.
		/// </param>
		public virtual void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
		}

		/// <summary>
		/// If the cell is attached will return the immediate RootElement
		/// </summary>
		public RootElement? GetImmediateRootElement ()
		{
			var section = Parent as Section;
			if (section is null)
				section = this as Section;
			if (section is null)
				return null;
			return section.Parent as RootElement;
		}

		/// <summary>
		/// Returns the UITableView associated with this element, or null if this cell is not currently attached to a UITableView
		/// </summary>
		public UITableView? GetContainerTableView ()
		{
			var root = GetImmediateRootElement ();
			if (root is null)
				return null;
			return root.TableView;
		}

		/// <summary>
		/// Returns the currently active UITableViewCell for this element, or null if the element is not currently visible
		/// </summary>
		public UITableViewCell? GetActiveCell ()
		{
			var tv = GetContainerTableView ();
			if (tv is null)
				return null;
			var path = IndexPath;
			if (path is null)
				return null;
			return tv.CellAt (path);
		}

		/// <summary>
		///  Returns the IndexPath of a given element.   This is only valid for leaf elements,
		///  it does not work for a toplevel RootElement or a Section of if the Element has
		///  not been attached yet.
		/// </summary>
		public NSIndexPath? IndexPath {
			get {
				var section = Parent as Section;
				if (section is null)
					return null;
				var root = section.Parent as RootElement;
				if (root is null)
					return null;

				int row = 0;
				foreach (var element in section.Elements) {
					if (element == this) {
						int nsect = 0;
						foreach (var sect in root.Sections) {
							if (section == sect) {
								return NSIndexPath.FromRowSection (row, nsect);
							}
							nsect++;
						}
					}
					row++;
				}
				return null;
			}
		}

		/// <summary>
		///   Method invoked to determine if the cell matches the given text, never invoked with a null value or an empty string.
		/// </summary>
		public virtual bool Matches (string text)
		{
			if (Caption is null)
				return false;
			return Caption.IndexOf (text, StringComparison.CurrentCultureIgnoreCase) != -1;
		}
	}

	public abstract partial class BoolElement : Element {
		bool val;
		public virtual bool Value {
			get {
				return val;
			}
			set {
				bool emit = val != value;
				val = value;
				if (emit && ValueChanged is not null)
					ValueChanged (this, EventArgs.Empty);
			}
		}
		public event EventHandler? ValueChanged;

		public BoolElement (string caption, bool value) : base (caption)
		{
			val = value;
		}

		public override string Summary ()
		{
			return val ? "On".GetText () : "Off".GetText ();
		}
	}

	/// <summary>
	/// Used to display switch on the screen.
	/// </summary>
	public partial class BooleanElement : BoolElement {
		static NSString bkey = new NSString ("BooleanElement");
#if !__TVOS__
		UISwitch? sw;
#endif // !__TVOS__

		public BooleanElement (string caption, bool value) : base (caption, value)
		{ }

		public BooleanElement (string caption, bool value, string key) : base (caption, value)
		{ }

		protected override NSString CellKey {
			get {
				return bkey;
			}
		}
		public override UITableViewCell GetCell (UITableView tv)
		{
#if __TVOS__
			var cell = ConfigCell (base.GetCell (tv));
#else
			if (sw is null) {
				sw = new UISwitch () {
					BackgroundColor = UIColor.Clear,
					Tag = 1,
					On = Value
				};
				sw.AddTarget (delegate
				{
					Value = sw.On;
				}, UIControlEvent.ValueChanged);
			} else
				sw.On = Value;

			var cell = tv.DequeueReusableCell (CellKey);
			if (cell is null) {
				cell = new UITableViewCell (UITableViewCellStyle.Default, CellKey);
				cell.SelectionStyle = UITableViewCellSelectionStyle.None;
			} else
				RemoveTag (cell, 1);

			cell.TextLabel.Text = Caption;
			cell.AccessoryView = sw;
#endif // !__TVOS__
			return cell;
		}

#if __TVOS__
		UITableViewCell ConfigCell (UITableViewCell cell)
		{
			cell.Accessory = Value ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;
			cell.TextLabel.Text = Caption;
			return cell;
		}

		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			Value = !Value;
			var cell = tableView.CellAt (path);
			if (cell is not null)
				ConfigCell (cell);
			base.Selected (dvc, tableView, path);
		}
#endif // !__TVOS__

		protected override void Dispose (bool disposing)
		{
			if (disposing) {
#if !__TVOS__
				if (sw is not null) {
					sw.Dispose ();
					sw = null;
				}
#endif // !__TVOS__
			}
		}

		public override bool Value {
			get {
				return base.Value;
			}
			set {
				base.Value = value;
#if __TVOS__
				// Not sure what to do here
#else
				if (sw is not null)
					sw.On = value;
#endif  // !__TVOS__
			}
		}
	}

	/// <summary>
	///  Used to display a cell that will launch a web browser when selected.
	/// </summary>
	public partial class HtmlElement : Element {
		NSUrl? nsUrl;
		static NSString hkey = new NSString ("HtmlElement");
#if !__TVOS__ && !__MACCATALYST__
		// There is no UIWebView in tvOS, so we can't launch anything.
		UIWebView? web;
#endif // !__TVOS__ && !__MACCATALYST__

		public HtmlElement (string caption, string url) : base (caption)
		{
			Url = url;
		}

		public HtmlElement (string caption, NSUrl url) : base (caption)
		{
			nsUrl = url;
		}

		protected override NSString CellKey {
			get {
				return hkey;
			}
		}
		public string Url {
			get {
				return nsUrl?.ToString () ?? "";
			}
			set {
				nsUrl = new NSUrl (value);
			}
		}

		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (CellKey);
			if (cell is null) {
				cell = new UITableViewCell (UITableViewCellStyle.Default, CellKey);
				cell.SelectionStyle = UITableViewCellSelectionStyle.Blue;
			}
			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;

			cell.TextLabel.Text = Caption;
			return cell;
		}

		static bool NetworkActivity {
			set {
#if !__TVOS__
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = value;
#endif // !__TVOS__
			}
		}

#if !__TVOS__ && !__MACCATALYST__
		// We use this class to dispose the web control when it is not
		// in use, as it could be a bit of a pig, and we do not want to
		// wait for the GC to kick-in.
		class WebViewController : UIViewController {
#pragma warning disable 414
			HtmlElement container;
#pragma warning restore 414

			public WebViewController (HtmlElement container) : base ()
			{
				this.container = container;
			}

			public bool Autorotate { get; set; }

			public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
			{
				return Autorotate;
			}
		}

		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			int i = 0;
			var vc = new WebViewController (this) {
				Autorotate = dvc.Autorotate
			};

			web = new UIWebView (UIScreen.MainScreen.Bounds) {
				BackgroundColor = UIColor.White,
				ScalesPageToFit = true,
				AutoresizingMask = UIViewAutoresizing.All
			};
			web.LoadStarted += delegate
			{
				// this is called several times and only one UIActivityIndicatorView is needed
				if (i++ == 0) {
					var indicator = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.White);
					vc.NavigationItem.RightBarButtonItem = new UIBarButtonItem (indicator);
					indicator.StartAnimating ();
				}
				NetworkActivity = true;
			};
			web.LoadFinished += delegate
			{
				if (--i == 0) {
					// we stopped loading, remove indicator and dispose of UIWebView
					vc.NavigationItem.RightBarButtonItem = null;
					web.StopLoading ();
					web.Dispose ();
				}
				NetworkActivity = false;
			};
			web.LoadError += (webview, args) => {
				NetworkActivity = false;
				vc.NavigationItem.RightBarButtonItem = null;
				if (web is not null)
					web.LoadHtmlString (
						String.Format ("<html><center><font size=+5 color='red'>{0}:<br>{1}</font></center></html>",
						"An error occurred:".GetText (), args.Error.LocalizedDescription), null);
			};
			vc.NavigationItem.Title = Caption;

			var view = vc.View;
			if (view is not null) {
				view.AutosizesSubviews = true;
				view.AddSubview (web);
			}

			dvc.ActivateController (vc);
			if (nsUrl is not null)
				web.LoadRequest (NSUrlRequest.FromUrl (nsUrl));
		}
#endif // !__TVOS__ && !__MACCATALYST__
	}

	/// <summary>
	///   The string element can be used to render some text in a cell 
	///   that can optionally respond to tap events.
	/// </summary>
	public partial class StringElement : Element {
		static NSString skey = new NSString ("StringElement");
		static NSString skeyvalue = new NSString ("StringElementValue");
		public UITextAlignment Alignment = UITextAlignment.Left;
		public string? Value;

		public StringElement (string? caption) : base (caption) { }

		public StringElement (string? caption, string? value) : base (caption)
		{
			this.Value = value;
		}

		public StringElement (string? caption, NSAction tapped) : base (caption)
		{
			Tapped += tapped;
		}

		public event NSAction? Tapped;

		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (Value is null ? skey : skeyvalue);
			cell = null;
			if (cell is null) {
				cell = new UITableViewCell (Value is null ? UITableViewCellStyle.Default : UITableViewCellStyle.Value1, Value is null ? skey : skeyvalue);
				cell.SelectionStyle = (Tapped is not null) ? UITableViewCellSelectionStyle.Blue : UITableViewCellSelectionStyle.None;
			}
			cell.Accessory = UITableViewCellAccessory.None;
			cell.TextLabel.Text = Caption;
			cell.TextLabel.TextAlignment = Alignment;

			// The check is needed because the cell might have been recycled.
			if (cell.DetailTextLabel is not null)
				cell.DetailTextLabel.Text = Value is null ? "" : Value;

			return cell;
		}

		public override string Summary ()
		{
			return Caption ?? "";
		}

		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath indexPath)
		{
			if (Tapped is not null)
				Tapped ();
			tableView.DeselectRow (indexPath, true);
		}

		public override bool Matches (string text)
		{
			return (Value is not null ? Value.IndexOf (text, StringComparison.CurrentCultureIgnoreCase) != -1 : false) || base.Matches (text);
		}
	}

	/// <summary>
	///   A version of the StringElement that can be styled with a number of formatting 
	///   options and can render images or background images either from UIImage parameters 
	///   or by downloading them from the net.
	/// </summary>
	public partial class StyledStringElement : StringElement, IColorizeBackground {
		static NSString [] skey = { new NSString (".1"), new NSString (".2"), new NSString (".3"), new NSString (".4") };

		internal static UIFont GetBoldFont (nfloat size)
		{
			return UIFont.BoldSystemFontOfSize (size) ?? throw new InvalidOperationException ("Unable to create a bold system font.");
		}

		internal static UIFont GetSystemFont (nfloat size)
		{
			return UIFont.SystemFontOfSize (size) ?? throw new InvalidOperationException ("Unable to create a system font.");
		}

		public StyledStringElement (string caption) : base (caption) { }
		public StyledStringElement (string caption, NSAction tapped) : base (caption, tapped) { }
		public StyledStringElement (string caption, string value) : base (caption, value)
		{
			style = UITableViewCellStyle.Value1;
		}
		public StyledStringElement (string caption, string value, UITableViewCellStyle style) : base (caption, value)
		{
			this.style = style;
		}

		protected UITableViewCellStyle style;
		public event NSAction? AccessoryTapped;
		public UIFont? Font;
		public UIFont? SubtitleFont;
		public UIColor? TextColor;
		public UILineBreakMode LineBreakMode = UILineBreakMode.WordWrap;
		public int Lines = 0;
		public UITableViewCellAccessory Accessory = UITableViewCellAccessory.None;

		// To keep the size down for a StyleStringElement, we put all the image information
		// on a separate structure, and create this on demand.
		ExtraInfo? extraInfo;

		class ExtraInfo {
			public UIColor? BackgroundColor, DetailColor;
		}

		ExtraInfo OnImageInfo ()
		{
			if (extraInfo is null)
				extraInfo = new ExtraInfo ();
			return extraInfo;
		}

		public UIColor? BackgroundColor {
			get {
				return extraInfo is null ? null : extraInfo.BackgroundColor;
			}
			set {
				OnImageInfo ().BackgroundColor = value;
			}
		}

		public UIColor? DetailColor {
			get {
				return extraInfo is null ? null : extraInfo.DetailColor;
			}
			set {
				OnImageInfo ().DetailColor = value;
			}
		}

		protected virtual string GetKey (int style)
		{
			return skey [style];
		}

		public override UITableViewCell GetCell (UITableView tv)
		{
			var key = GetKey ((int) style);
			var cell = tv.DequeueReusableCell (key);
			if (cell is null) {
				cell = new UITableViewCell (style, key);
				cell.SelectionStyle = UITableViewCellSelectionStyle.Blue;
			}
			PrepareCell (cell);
			return cell;
		}

		protected void PrepareCell (UITableViewCell cell)
		{
			cell.Accessory = Accessory;
			var tl = cell.TextLabel;
			tl.Text = Caption;
			tl.TextAlignment = Alignment;
			tl.TextColor = TextColor ?? UIColor.Black;
			tl.Font = Font ?? GetBoldFont (17);
			tl.LineBreakMode = LineBreakMode;
			tl.Lines = Lines;

			// The check is needed because the cell might have been recycled.
			if (cell.DetailTextLabel is not null)
				cell.DetailTextLabel.Text = Value is null ? "" : Value;

			if (extraInfo is null) {
				ClearBackground (cell);
			} else {
				if (cell.DetailTextLabel is not null)
					cell.DetailTextLabel.TextColor = extraInfo.DetailColor ?? UIColor.Gray;
			}

			if (cell.DetailTextLabel is not null) {
				cell.DetailTextLabel.Lines = Lines;
				cell.DetailTextLabel.LineBreakMode = LineBreakMode;
				cell.DetailTextLabel.Font = SubtitleFont ?? GetSystemFont (14);
				cell.DetailTextLabel.TextColor = (extraInfo is null || extraInfo.DetailColor is null) ? UIColor.Gray : extraInfo.DetailColor;
			}
		}

		void ClearBackground (UITableViewCell cell)
		{
			cell.BackgroundColor = UITableViewCell.Appearance.BackgroundColor;
			cell.TextLabel.BackgroundColor = UIColor.Clear;
		}

		void IColorizeBackground.WillDisplay (UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
		{
			if (extraInfo is null) {
				ClearBackground (cell);
				return;
			}

			if (extraInfo.BackgroundColor is not null) {
				cell.TextLabel.BackgroundColor = UIColor.Clear;
				cell.BackgroundColor = extraInfo.BackgroundColor;
			} else
				ClearBackground (cell);
		}

		internal void AccessoryTap ()
		{
			var tapped = AccessoryTapped;
			if (tapped is not null)
				tapped ();
		}
	}

#if __TVOS__
	internal static class Helper {

		static public CGSize StringSize (this string self, UIFont font)
		{
			using (var str = (NSString) self) {
				return str.GetSizeUsingAttributes (new UIStringAttributes () {
					Font = font,
				});
			}
		}

		static public CGSize StringSize (this string self, UIFont font, CGSize constrainedToSize, UILineBreakMode lineBreakMode)
		{
			using (var str = (NSString) self) {
				return StringSize (str, font, constrainedToSize, lineBreakMode);
			}
		}

		static public CGSize StringSize (this NSString self, UIFont font, CGSize constrainedToSize, UILineBreakMode lineBreakMode)
		{
			return self.GetBoundingRect (constrainedToSize, NSStringDrawingOptions.UsesLineFragmentOrigin, new UIStringAttributes () {
				Font = font,
				ParagraphStyle = new NSMutableParagraphStyle () {
					LineBreakMode = lineBreakMode,
				},
			}, null).Size;
		}

		static public CGSize StringSize (this NSString self, UIFont font, float forWidth, UILineBreakMode lineBreakMode)
		{
			return StringSize (self, font, new CGSize (forWidth, nfloat.MaxValue), lineBreakMode);
		}

		static public void DrawString (this string self, CGRect rect, UIFont font)
		{
			using (var str = (NSString) self) {
				NSStringDrawing.DrawString (str, rect, new UIStringAttributes () {
					Font = font,
				});
			}
		}

		static public void DrawString (this string self, CGPoint point, float width, UIFont font, UILineBreakMode lineBreakMode)
		{
			using (var str = (NSString) self) {
				NSStringDrawing.DrawString (str, point, new UIStringAttributes () {
					Font = font,
					ParagraphStyle = new NSMutableParagraphStyle () {
						LineBreakMode = lineBreakMode,
					},
				});
			}
		}

		static public void DrawString (this string self, CGRect rect, UIFont font, UILineBreakMode lineBreakMode, UITextAlignment alignment)
		{
			using (var str = (NSString) self) {
				NSStringDrawing.DrawString (str, rect, new UIStringAttributes () {
					Font = font,
					ParagraphStyle = new NSMutableParagraphStyle () {
						LineBreakMode = lineBreakMode,
						Alignment = alignment,
					},
				});
			}
		}
	}
#endif

	public partial class StyledMultilineElement : StyledStringElement, IElementSizing {
		public StyledMultilineElement (string caption) : base (caption) { }
		public StyledMultilineElement (string caption, string value) : base (caption, value) { }
		public StyledMultilineElement (string caption, NSAction tapped) : base (caption, tapped) { }
		public StyledMultilineElement (string caption, string value, UITableViewCellStyle style) : base (caption, value)
		{
			this.style = style;
		}

		public virtual nfloat GetHeight (UITableView tableView, NSIndexPath indexPath)
		{
			float margin = UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone ? 40f : 110f;
			CGSize maxSize = new CGSize (tableView.Bounds.Width - margin, float.MaxValue);

			if (this.Accessory != UITableViewCellAccessory.None)
				maxSize.Width -= 20;

			string? c = Caption;
			string? v = Value;
			// ensure the (multi-line) Value will be rendered inside the cell when no Caption is present
			if (String.IsNullOrEmpty (c))
				c = " ";

			var captionFont = Font ?? StyledStringElement.GetBoldFont (17);
			var height = c.StringSize (captionFont, maxSize, LineBreakMode).Height;

			if (!String.IsNullOrEmpty (v)) {
				var subtitleFont = SubtitleFont ?? StyledStringElement.GetSystemFont (14);
				if (this.style == UITableViewCellStyle.Subtitle) {
					height += v.StringSize (subtitleFont, maxSize, LineBreakMode).Height;
				} else {
					var vheight = v.StringSize (subtitleFont, maxSize, LineBreakMode).Height;
					if (vheight > height)
						height = vheight;
				}
			}

			return height + 10;
		}
	}

	/// <summary>
	///   This interface is implemented by Element classes that will have
	///   different heights
	/// </summary>
	public interface IElementSizing {
		nfloat GetHeight (UITableView tableView, NSIndexPath indexPath);
	}

	/// <summary>
	///   This interface is implemented by Elements that needs to update
	///   their cells Background properties just before they are displayed
	///   to the user.   This is an iOS 3 requirement to properly render
	///   a cell.
	/// </summary>
	public interface IColorizeBackground {
		void WillDisplay (UITableView tableView, UITableViewCell cell, NSIndexPath indexPath);
	}

	public partial class MultilineElement : StringElement, IElementSizing {
		public MultilineElement (string caption) : base (caption)
		{
		}

		public MultilineElement (string caption, string value) : base (caption, value)
		{
		}

		public MultilineElement (string caption, NSAction tapped) : base (caption, tapped)
		{
		}

		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = base.GetCell (tv);
			var tl = cell.TextLabel;
			tl.LineBreakMode = UILineBreakMode.WordWrap;
			tl.Lines = 0;

			return cell;
		}

		public virtual nfloat GetHeight (UITableView tableView, NSIndexPath indexPath)
		{
			float margin = UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone ? 40f : 110f;
			CGSize size = new CGSize (tableView.Bounds.Width - margin, float.MaxValue);
			var font = StyledStringElement.GetBoldFont (17);
			string? c = Caption;
			// ensure the (single-line) Value will be rendered inside the cell
			if (String.IsNullOrEmpty (c) && !String.IsNullOrEmpty (Value))
				c = " ";
			return (c ?? "").StringSize (font, size, UILineBreakMode.WordWrap).Height + 10;
		}
	}

	/// <summary>
	/// An element that can be used to enter text.
	/// </summary>
	/// <remarks>
	/// This element can be used to enter text both regular and password protected entries. 
	///     
	/// The Text fields in a given section are aligned with each other.
	/// </remarks>
	public partial class EntryElement : Element {
		/// <summary>
		///   The value of the EntryElement
		/// </summary>
		public string? Value {
			get {
				if (entry is null)
					return val;
				var newValue = entry.Text;
				if (newValue == val)
					return val;
				val = newValue;

				if (Changed is not null)
					Changed (this, EventArgs.Empty);
				return val;
			}
			set {
				val = value;
				if (entry is not null)
					entry.Text = value;
			}
		}
		protected string? val;

		/// <summary>
		/// The key used for reusable UITableViewCells.
		/// </summary>
		static NSString entryKey = new NSString ("EntryElement");
		protected virtual NSString EntryKey {
			get {
				return entryKey;
			}
		}

		/// <summary>
		/// The type of keyboard used for input, you can change
		/// this to use this for numeric input, email addressed,
		/// urls, phones.
		/// </summary>
		public UIKeyboardType KeyboardType {
			get {
				return keyboardType;
			}
			set {
				keyboardType = value;
				if (entry is not null)
					entry.KeyboardType = value;
			}
		}

		/// <summary>
		/// The type of Return Key that is displayed on the
		/// keyboard, you can change this to use this for
		/// Done, Return, Save, etc. keys on the keyboard
		/// </summary>
		public UIReturnKeyType? ReturnKeyType {
			get {
				return returnKeyType;
			}
			set {
				returnKeyType = value;
				if (entry is not null && returnKeyType.HasValue)
					entry.ReturnKeyType = returnKeyType.Value;
			}
		}

		/// <summary>
		/// The default value for this property is <c>false</c>. If you set it to <c>true</c>, the keyboard disables the return key when the text entry area contains no text. As soon as the user enters any text, the return key is automatically enabled.
		/// </summary>
		public bool EnablesReturnKeyAutomatically {
			get {
				return enablesReturnKeyAutomatically;
			}
			set {
				enablesReturnKeyAutomatically = value;
				if (entry is not null)
					entry.EnablesReturnKeyAutomatically = value;
			}
		}

		public UITextAutocapitalizationType AutocapitalizationType {
			get {
				return autocapitalizationType;
			}
			set {
				autocapitalizationType = value;
				if (entry is not null)
					entry.AutocapitalizationType = value;
			}
		}

		public UITextAutocorrectionType AutocorrectionType {
			get {
				return autocorrectionType;
			}
			set {
				autocorrectionType = value;
				if (entry is not null)
					this.autocorrectionType = value;
			}
		}

		public UITextFieldViewMode ClearButtonMode {
			get {
				return clearButtonMode;
			}
			set {
				clearButtonMode = value;
				if (entry is not null)
					entry.ClearButtonMode = value;
			}
		}

		public UITextAlignment TextAlignment {
			get {
				return textalignment;
			}
			set {
				textalignment = value;
				if (entry is not null) {
					entry.TextAlignment = textalignment;
				}
			}
		}

		public bool AlignEntryWithAllSections { get; set; }

		public bool NotifyChangedOnKeyStroke { get; set; }

		UITextAlignment textalignment = UITextAlignment.Left;
		UIKeyboardType keyboardType = UIKeyboardType.Default;
		UIReturnKeyType? returnKeyType = null;
		bool enablesReturnKeyAutomatically = false;
		UITextAutocapitalizationType autocapitalizationType = UITextAutocapitalizationType.Sentences;
		UITextAutocorrectionType autocorrectionType = UITextAutocorrectionType.Default;
		UITextFieldViewMode clearButtonMode = UITextFieldViewMode.Never;
		bool isPassword, becomeResponder;
		UITextField? entry;
		string? placeholder;
		static UIFont font = StyledStringElement.GetBoldFont (17);

		public event EventHandler? Changed;
		public event Func<bool>? ShouldReturn;
		public EventHandler? EntryStarted { get; set; }
		public EventHandler? EntryEnded { get; set; }
		/// <summary>
		/// Constructs an EntryElement with the given caption, placeholder and initial value.
		/// </summary>
		/// <param name="caption">
		/// The caption to use
		/// </param>
		/// <param name="placeholder">
		/// Placeholder to display when no value is set.
		/// </param>
		/// <param name="value">
		/// Initial value.
		/// </param>
		public EntryElement (string caption, string? placeholder, string? value) : base (caption)
		{
			Value = value;
			this.placeholder = placeholder;
		}

		/// <summary>
		/// Constructs an EntryElement for password entry with the given caption, placeholder and initial value.
		/// </summary>
		/// <param name="caption">
		/// The caption to use.
		/// </param>
		/// <param name="placeholder">
		/// Placeholder to display when no value is set.
		/// </param>
		/// <param name="value">
		/// Initial value.
		/// </param>
		/// <param name="isPassword">
		/// True if this should be used to enter a password.
		/// </param>
		public EntryElement (string caption, string? placeholder, string? value, bool isPassword) : base (caption)
		{
			Value = value;
			this.isPassword = isPassword;
			this.placeholder = placeholder;
		}

		public override string Summary ()
		{
			return Value ?? "";
		}

		// 
		// Computes the X position for the entry by aligning all the entries in the Section
		//
		CGSize ComputeEntryPosition (UITableView tv, UITableViewCell cell)
		{
			nfloat maxWidth = -15; // If all EntryElements have a null Caption, align UITextField with the Caption offset of normal cells (at 10px).
			nfloat maxHeight = font.LineHeight;

			// Determine if we should calculate accross all sections or just the current section.
			IEnumerable<Section> sections;
			if (AlignEntryWithAllSections) {
				var root = Parent?.Parent as RootElement;
				sections = root is not null ? root.Sections : Enumerable.Empty<Section> ();
			} else if (Parent is Section section) {
				sections = new [] { section };
			} else {
				sections = Enumerable.Empty<Section> ();
			}

			foreach (Section s in sections) {

				foreach (var e in s.Elements) {

					var ee = e as EntryElement;

					if (ee is not null
						&& !String.IsNullOrEmpty (ee.Caption)) {

						var size = ee.Caption.StringSize (font);

						maxWidth = (nfloat) Math.Max (size.Width, maxWidth);
						maxHeight = (nfloat) Math.Max (size.Height, maxHeight);
					}
				}
			}

			return new CGSize (25 + (nfloat) Math.Min (maxWidth, 160), maxHeight);
		}

		protected virtual UITextField CreateTextField (CGRect frame)
		{
			return new UITextField (frame) {
				AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleLeftMargin,
				Placeholder = placeholder ?? "",
				SecureTextEntry = isPassword,
				Text = Value ?? "",
				Tag = 1,
				TextAlignment = textalignment,
				ClearButtonMode = ClearButtonMode
			};
		}

		static readonly NSString passwordKey = new NSString ("EntryElement+Password");
		static readonly NSString cellkey = new NSString ("EntryElement");

		protected override NSString CellKey {
			get {
				return isPassword ? passwordKey : cellkey;
			}
		}

		UITableViewCell? cell;
		public override UITableViewCell GetCell (UITableView tv)
		{
			if (cell is null) {
				cell = new UITableViewCell (UITableViewCellStyle.Default, CellKey);
				cell.SelectionStyle = UITableViewCellSelectionStyle.None;
				cell.TextLabel.Font = font;

			}
			cell.TextLabel.Text = Caption;

			var offset = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) ? 20 : 90;
			cell.Frame = new CGRect (cell.Frame.X, cell.Frame.Y, tv.Frame.Width - offset, cell.Frame.Height);
			CGSize size = ComputeEntryPosition (tv, cell);
			nfloat yOffset = (cell.ContentView.Bounds.Height - size.Height) / 2 - 1;
			nfloat width = cell.ContentView.Bounds.Width - size.Width;
			if (textalignment == UITextAlignment.Right) {
				// Add padding if right aligned
				width -= 10;
			}
#if __TVOS__
			var entryFrame = new CGRect (size.Width, yOffset, width, size.Height + 20 /* FIXME: figure out something better than adding a magic number */);
#else
			var entryFrame = new CGRect (size.Width, yOffset, width, size.Height);
#endif

			if (entry is null) {
				entry = CreateTextField (entryFrame);
				entry.EditingChanged += delegate
				{
					if (NotifyChangedOnKeyStroke) {
						FetchValue ();
					}
				};
				entry.ValueChanged += delegate
				{
					FetchValue ();
				};
				entry.Ended += delegate
				{
					FetchValue ();
					if (EntryEnded is not null) {
						EntryEnded (this, EventArgs.Empty);
					}
				};
				entry.ShouldReturn += delegate
				{

					if (ShouldReturn is not null)
						return ShouldReturn ();

					RootElement? root = GetImmediateRootElement ();
					EntryElement? focus = null;

					if (root is null)
						return true;

					foreach (var s in root.Sections) {
						foreach (var e in s.Elements) {
							if (e == this) {
								focus = this;
							} else if (focus is not null && e is EntryElement) {
								focus = e as EntryElement;
								break;
							}
						}

						if (focus is not null && focus != this)
							break;
					}

					if (focus != this)
						focus?.BecomeFirstResponder (true);
					else
						focus?.ResignFirstResponder (true);

					return true;
				};
				entry.Started += delegate
				{
					EntryElement? self = null;

					if (EntryStarted is not null) {
						EntryStarted (this, EventArgs.Empty);
					}

					if (!returnKeyType.HasValue && Parent is Section parentSection) {
						var returnType = UIReturnKeyType.Default;

						foreach (var e in parentSection.Elements) {
							if (e == this)
								self = this;
							else if (self is not null && e is EntryElement)
								returnType = UIReturnKeyType.Next;
						}
						entry.ReturnKeyType = returnType;
					} else if (returnKeyType.HasValue) {
						entry.ReturnKeyType = returnKeyType.Value;
					}

					var path = IndexPath;
					if (path is not null)
						tv.ScrollToRow (path, UITableViewScrollPosition.Middle, true);
				};
				cell.ContentView.AddSubview (entry);
			}

			if (becomeResponder) {
				entry.BecomeFirstResponder ();
				becomeResponder = false;
			}
			entry.KeyboardType = KeyboardType;
			entry.EnablesReturnKeyAutomatically = EnablesReturnKeyAutomatically;
			entry.AutocapitalizationType = AutocapitalizationType;
			entry.AutocorrectionType = AutocorrectionType;

			return cell;
		}

		/// <summary>
		///  Copies the value from the UITextField in the EntryElement to the
		//   Value property and raises the Changed event if necessary.
		/// </summary>
		public void FetchValue ()
		{
			if (entry is null)
				return;

			var newValue = entry.Text;
			if (newValue == Value)
				return;

			Value = newValue;

			if (Changed is not null)
				Changed (this, EventArgs.Empty);
		}

		protected override void Dispose (bool disposing)
		{
			if (disposing) {
				if (entry is not null) {
					entry.Dispose ();
					entry = null;
				}
			}
		}

		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath indexPath)
		{
			BecomeFirstResponder (true);
			tableView.DeselectRow (indexPath, true);
		}

		public override bool Matches (string text)
		{
			return (Value is not null ? Value.IndexOf (text, StringComparison.CurrentCultureIgnoreCase) != -1 : false) || base.Matches (text);
		}

		/// <summary>
		/// Makes this cell the first responder (get the focus)
		/// </summary>
		/// <param name="animated">
		/// Whether scrolling to the location of this cell should be animated
		/// </param>
		public virtual void BecomeFirstResponder (bool animated)
		{
			becomeResponder = true;
			var tv = GetContainerTableView ();
			if (tv is null)
				return;
			var path = IndexPath;
			if (path is not null)
				tv.ScrollToRow (path, UITableViewScrollPosition.Middle, animated);
			if (entry is not null) {
				entry.BecomeFirstResponder ();
				becomeResponder = false;
			}
		}

		public virtual void ResignFirstResponder (bool animated)
		{
			becomeResponder = false;
			var tv = GetContainerTableView ();
			if (tv is null)
				return;
			var path = IndexPath;
			if (path is not null)
				tv.ScrollToRow (path, UITableViewScrollPosition.Middle, animated);
			if (entry is not null)
				entry.ResignFirstResponder ();
		}
	}

	/// <summary>
	/// Sections contain individual Element instances that are rendered by MonoTouch.Dialog
	/// </summary>
	/// <remarks>
	/// Sections are used to group elements in the screen and they are the
	/// only valid direct child of the RootElement.    Sections can contain
	/// any of the standard elements, including new RootElements.
	/// 
	/// RootElements embedded in a section are used to navigate to a new
	/// deeper level.
	/// 
	/// You can assign a header and a footer either as strings (Header and Footer)
	/// properties, or as UIViews to be shown (HeaderView and FooterView).   Internally
	/// this uses the same storage, so you can only show one or the other.
	/// </remarks>
	public class Section : Element, IEnumerable {
		object? header, footer;
		public List<Element> Elements = new List<Element> ();

		// X corresponds to the alignment, Y to the height of the password
		public CGSize EntryAlignment;

		/// <summary>
		///  Constructs a Section without header or footers.
		/// </summary>
		public Section () : base (null) { }

		/// <summary>
		///  Constructs a Section with the specified header
		/// </summary>
		/// <param name="caption">
		/// The header to display
		/// </param>
		public Section (string? caption) : base (caption)
		{
		}

		/// <summary>
		/// Constructs a Section with a header and a footer
		/// </summary>
		/// <param name="caption">
		/// The caption to display (or null to not display a caption)
		/// </param>
		/// <param name="footer">
		/// The footer to display.
		/// </param>
		public Section (string? caption, string? footer) : base (caption)
		{
			Footer = footer;
		}

		public Section (UIView header) : base (null)
		{
			HeaderView = header;
		}

		public Section (UIView header, UIView footer) : base (null)
		{
			HeaderView = header;
			FooterView = footer;
		}

		/// <summary>
		///    The section header, as a string
		/// </summary>
		public string? Header {
			get {
				return header as string;
			}
			set {
				header = value;
			}
		}

		/// <summary>
		/// The section footer, as a string.
		/// </summary>
		public string? Footer {
			get {
				return footer as string;
			}

			set {
				footer = value;
			}
		}

		/// <summary>
		/// The section's header view.  
		/// </summary>
		public UIView? HeaderView {
			get {
				return header as UIView;
			}
			set {
				header = value;
			}
		}

		/// <summary>
		/// The section's footer view.
		/// </summary>
		public UIView? FooterView {
			get {
				return footer as UIView;
			}
			set {
				footer = value;
			}
		}

		/// <summary>
		/// Adds a new child Element to the Section
		/// </summary>
		/// <param name="element">
		/// An element to add to the section.
		/// </param>
		public void Add (Element element)
		{
			if (element is null)
				return;

			Elements.Add (element);
			element.Parent = this;

			if (Parent is not null)
				InsertVisual (Elements.Count - 1, UITableViewRowAnimation.None, 1);
		}

		/// <summary>
		/// Adds a new child RootElement to the Section. This only exists to fix a compiler breakage when the mono 3.0 mcs is used.
		/// </summary>
		/// <param name="element">
		/// An element to add to the section.
		/// </param>
		public void Add (RootElement element)
		{
			Add ((Element) element);
		}

		/// <summary>
		///    Add version that can be used with LINQ
		/// </summary>
		/// <param name="elements">
		/// An enumerable list that can be produced by something like:
		///    from x in ... select (Element) new MyElement (...)
		/// </param>
		public int AddAll (IEnumerable<Element> elements)
		{
			int count = 0;
			foreach (var e in elements) {
				Add (e);
				count++;
			}
			return count;
		}

		/// <summary>
		///    This method is being obsoleted, use AddAll to add an IEnumerable<Element> instead.
		/// </summary>
		[Obsolete ("Please use AddAll since this version will not work in future versions of MonoTouch when we introduce 4.0 covariance")]
		public int Add (IEnumerable<Element> elements)
		{
			return AddAll (elements);
		}

		/// <summary>
		/// Inserts a series of elements into the Section using the specified animation
		/// </summary>
		/// <param name="idx">
		/// The index where the elements are inserted
		/// </param>
		/// <param name="anim">
		/// The animation to use
		/// </param>
		/// <param name="newElements">
		/// A series of elements.
		/// </param>
		public void Insert (int idx, UITableViewRowAnimation anim, params Element [] newElements)
		{
			if (newElements is null)
				return;

			int pos = idx;
			foreach (var e in newElements) {
				Elements.Insert (pos++, e);
				e.Parent = this;
			}
			var root = Parent as RootElement;
			if (root is not null && root.TableView is not null) {
				if (anim == UITableViewRowAnimation.None)
					root.TableView.ReloadData ();
				else
					InsertVisual (idx, anim, newElements.Length);
			}
		}

		public int Insert (int idx, UITableViewRowAnimation anim, IEnumerable<Element> newElements)
		{
			if (newElements is null)
				return 0;

			int pos = idx;
			int count = 0;
			foreach (var e in newElements) {
				Elements.Insert (pos++, e);
				e.Parent = this;
				count++;
			}
			var root = Parent as RootElement;
			if (root is not null && root.TableView is not null) {
				if (anim == UITableViewRowAnimation.None)
					root.TableView.ReloadData ();
				else
					InsertVisual (idx, anim, pos - idx);
			}
			return count;
		}

		/// <summary>
		/// Inserts a single RootElement into the Section using the specified animation
		/// </summary>
		/// <param name="idx">
		/// The index where the elements are inserted
		/// </param>
		/// <param name="anim">
		/// The animation to use
		/// </param>
		/// <param name="newElements">
		/// A series of elements.
		/// </param>
		public void Insert (int idx, UITableViewRowAnimation anim, RootElement newElement)
		{
			Insert (idx, anim, (Element) newElement);
		}

		void InsertVisual (int idx, UITableViewRowAnimation anim, int count)
		{
			var root = Parent as RootElement;

			if (root is null || root.TableView is null)
				return;

			int sidx = root.IndexOf (this);
			var paths = new NSIndexPath [count];
			for (int i = 0; i < count; i++)
				paths [i] = NSIndexPath.FromRowSection (idx + i, sidx);

			root.TableView.InsertRows (paths, anim);
		}

		public void Insert (int index, params Element [] newElements)
		{
			Insert (index, UITableViewRowAnimation.None, newElements);
		}

		public void Remove (Element e)
		{
			if (e is null)
				return;
			for (int i = Elements.Count; i > 0;) {
				i--;
				if (Elements [i] == e) {
					RemoveRange (i, 1);
					return;
				}
			}
		}

		public void Remove (int idx)
		{
			RemoveRange (idx, 1);
		}

		/// <summary>
		/// Removes a range of elements from the Section
		/// </summary>
		/// <param name="start">
		/// Starting position
		/// </param>
		/// <param name="count">
		/// Number of elements to remove from the section
		/// </param>
		public void RemoveRange (int start, int count)
		{
			RemoveRange (start, count, UITableViewRowAnimation.Fade);
		}

		/// <summary>
		/// Remove a range of elements from the section with the given animation
		/// </summary>
		/// <param name="start">
		/// Starting position
		/// </param>
		/// <param name="count">
		/// Number of elements to remove form the section
		/// </param>
		/// <param name="anim">
		/// The animation to use while removing the elements
		/// </param>
		public void RemoveRange (int start, int count, UITableViewRowAnimation anim)
		{
			if (start < 0 || start >= Elements.Count)
				return;
			if (count == 0)
				return;

			var root = Parent as RootElement;

			if (start + count > Elements.Count)
				count = Elements.Count - start;

			Elements.RemoveRange (start, count);

			if (root is null || root.TableView is null)
				return;

			int sidx = root.IndexOf (this);
			var paths = new NSIndexPath [count];
			for (int i = 0; i < count; i++)
				paths [i] = NSIndexPath.FromRowSection (start + i, sidx);
			root.TableView.DeleteRows (paths, anim);
		}

		/// <summary>
		/// Enumerator to get all the elements in the Section.
		/// </summary>
		/// <returns>
		/// A <see cref="IEnumerator"/>
		/// </returns>
		public IEnumerator GetEnumerator ()
		{
			foreach (var e in Elements)
				yield return e;
		}

		public int Count {
			get {
				return Elements.Count;
			}
		}

		public Element this [int idx] {
			get {
				return Elements [idx];
			}
		}

		public void Clear ()
		{
			if (Elements is not null) {
				foreach (var e in Elements)
					e.Dispose ();
			}
			Elements = new List<Element> ();

			var root = Parent as RootElement;
			if (root is not null && root.TableView is not null)
				root.TableView.ReloadData ();
		}

		protected override void Dispose (bool disposing)
		{
			if (disposing) {
				Parent = null;
				Clear ();
			}
			base.Dispose (disposing);
		}

		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = new UITableViewCell (UITableViewCellStyle.Default, "");
			cell.TextLabel.Text = "Section was used for Element";

			return cell;
		}
	}

	/// <summary>
	/// Used by root elements to fetch information when they need to
	/// render a summary (Checkbox count or selected radio group).
	/// </summary>
	public class Group {
		public string? Key;
		public Group (string? key)
		{
			Key = key;
		}
	}

	/// <summary>
	///    RootElements are responsible for showing a full configuration page.
	/// </summary>
	/// <remarks>
	///    At least one RootElement is required to start the MonoTouch.Dialogs
	///    process.   
	/// 
	///    RootElements can also be used inside Sections to trigger
	///    loading a new nested configuration page.   When used in this mode
	///    the caption provided is used while rendered inside a section and
	///    is also used as the Title for the subpage.
	/// 
	///    If a RootElement is initialized with a section/element value then
	///    this value is used to locate a child Element that will provide
	///    a summary of the configuration which is rendered on the right-side
	///    of the display.
	/// 
	///    RootElements are also used to coordinate radio elements.  The
	///    RadioElement members can span multiple Sections (for example to
	///    implement something similar to the ring tone selector and separate
	///    custom ring tones from system ringtones).
	/// 
	///    Sections are added by calling the Add method which supports the
	///    C# 4.0 syntax to initialize a RootElement in one pass.
	/// </remarks>
	public partial class RootElement : Element, IEnumerable, IEnumerable<Section> {
		static NSString rkey1 = new NSString ("RootElement1");
		static NSString rkey2 = new NSString ("RootElement2");
		int summarySection, summaryElement;
		internal Group? group;
		public bool UnevenRows;
		public Func<RootElement, UIViewController>? createOnSelected;
		public UITableView? TableView;

		// This is used to indicate that we need the DVC to dispatch calls to
		// WillDisplayCell so we can prepare the color of the cell before 
		// display
		public bool NeedColorUpdate;

		/// <summary>
		///  Initializes a RootSection with a caption
		/// </summary>
		/// <param name="caption">
		///  The caption to render.
		/// </param>
		public RootElement (string caption) : base (caption)
		{
			summarySection = -1;
			Sections = new List<Section> ();
		}

		/// <summary>
		/// Initializes a RootSection with a caption and a callback that will
		/// create the nested UIViewController that is activated when the user
		/// taps on the element.
		/// </summary>
		/// <param name="caption">
		///  The caption to render.
		/// </param>
		public RootElement (string caption, Func<RootElement, UIViewController> createOnSelected) : base (caption)
		{
			summarySection = -1;
			this.createOnSelected = createOnSelected;
			Sections = new List<Section> ();
		}

		/// <summary>
		///   Initializes a RootElement with a caption with a summary fetched from the specified section and leement
		/// </summary>
		/// <param name="caption">
		/// The caption to render cref="System.String"/>
		/// </param>
		/// <param name="section">
		/// The section that contains the element with the summary.
		/// </param>
		/// <param name="element">
		/// The element index inside the section that contains the summary for this RootSection.
		/// </param>
		public RootElement (string caption, int section, int element) : base (caption)
		{
			summarySection = section;
			summaryElement = element;
		}

		/// <summary>
		/// Initializes a RootElement that renders the summary based on the radio settings of the contained elements. 
		/// </summary>
		/// <param name="caption">
		/// The caption to ender
		/// </param>
		/// <param name="group">
		/// The group that contains the checkbox or radio information.  This is used to display
		/// the summary information when a RootElement is rendered inside a section.
		/// </param>
		public RootElement (string caption, Group group) : base (caption)
		{
			this.group = group;
		}

		internal List<Section> Sections = new List<Section> ();

		public int Count {
			get {
				return Sections.Count;
			}
		}

		public Section this [int idx] {
			get {
				return Sections [idx];
			}
		}

		internal int IndexOf (Section target)
		{
			int idx = 0;
			foreach (Section s in Sections) {
				if (s == target)
					return idx;
				idx++;
			}
			return -1;
		}

		public void Prepare ()
		{
			foreach (Section s in Sections) {
				foreach (Element e in s.Elements) {
					if (UnevenRows == false && e is IElementSizing)
						UnevenRows = true;
					if (NeedColorUpdate == false && e is IColorizeBackground)
						NeedColorUpdate = true;
				}
			}
		}

		/// <summary>
		/// Adds a new section to this RootElement
		/// </summary>
		/// <param name="section">
		/// The section to add, if the root is visible, the section is inserted with no animation
		/// </param>
		public void Add (Section section)
		{
			if (section is null)
				return;

			Sections.Add (section);
			section.Parent = this;
			if (TableView is null)
				return;

			TableView.InsertSections (MakeIndexSet (Sections.Count - 1, 1), UITableViewRowAnimation.None);
		}

		//
		// This makes things LINQ friendly;  You can now create RootElements
		// with an embedded LINQ expression, like this:
		// new RootElement ("Title") {
		//     from x in names
		//         select new Section (x) { new StringElement ("Sample") }
		//
		public void Add (IEnumerable<Section> sections)
		{
			foreach (var s in sections)
				Add (s);
		}

		NSIndexSet MakeIndexSet (int start, int count)
		{
			NSRange range;
			range.Location = start;
			range.Length = count;
			return NSIndexSet.FromNSRange (range);
		}

		/// <summary>
		/// Inserts a new section into the RootElement
		/// </summary>
		/// <param name="idx">
		/// The index where the section is added <see cref="System.Int32"/>
		/// </param>
		/// <param name="anim">
		/// The <see cref="UITableViewRowAnimation"/> type.
		/// </param>
		/// <param name="newSections">
		/// A <see cref="Section[]"/> list of sections to insert
		/// </param>
		/// <remarks>
		///    This inserts the specified list of sections (a params argument) into the
		///    root using the specified animation.
		/// </remarks>
		public void Insert (int idx, UITableViewRowAnimation anim, params Section [] newSections)
		{
			if (idx < 0 || idx > Sections.Count)
				return;
			if (newSections is null)
				return;

			if (TableView is not null)
				TableView.BeginUpdates ();

			int pos = idx;
			foreach (var s in newSections) {
				s.Parent = this;
				Sections.Insert (pos++, s);
			}

			if (TableView is null)
				return;

			TableView.InsertSections (MakeIndexSet (idx, newSections.Length), anim);
			TableView.EndUpdates ();
		}

		/// <summary>
		/// Inserts a new section into the RootElement
		/// </summary>
		/// <param name="idx">
		/// The index where the section is added <see cref="System.Int32"/>
		/// </param>
		/// <param name="newSections">
		/// A <see cref="Section[]"/> list of sections to insert
		/// </param>
		/// <remarks>
		///    This inserts the specified list of sections (a params argument) into the
		///    root using the Fade animation.
		/// </remarks>
		public void Insert (int idx, Section section)
		{
			Insert (idx, UITableViewRowAnimation.None, section);
		}

		/// <summary>
		/// Removes a section at a specified location
		/// </summary>
		public void RemoveAt (int idx)
		{
			RemoveAt (idx, UITableViewRowAnimation.Fade);
		}

		/// <summary>
		/// Removes a section at a specified location using the specified animation
		/// </summary>
		/// <param name="idx">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <param name="anim">
		/// A <see cref="UITableViewRowAnimation"/>
		/// </param>
		public void RemoveAt (int idx, UITableViewRowAnimation anim)
		{
			if (idx < 0 || idx >= Sections.Count)
				return;

			Sections.RemoveAt (idx);

			if (TableView is null)
				return;

			TableView.DeleteSections (NSIndexSet.FromIndex (idx), anim);
		}

		public void Remove (Section s)
		{
			if (s is null)
				return;
			int idx = Sections.IndexOf (s);
			if (idx == -1)
				return;
			RemoveAt (idx, UITableViewRowAnimation.Fade);
		}

		public void Remove (Section s, UITableViewRowAnimation anim)
		{
			if (s is null)
				return;
			int idx = Sections.IndexOf (s);
			if (idx == -1)
				return;
			RemoveAt (idx, anim);
		}

		public void Clear ()
		{
			foreach (var s in Sections)
				s.Dispose ();
			Sections = new List<Section> ();
			if (TableView is not null)
				TableView.ReloadData ();
		}

		protected override void Dispose (bool disposing)
		{
			if (disposing) {
				if (Sections is null)
					return;

				TableView = null;
				Clear ();
			}
		}

		/// <summary>
		/// Enumerator that returns all the sections in the RootElement.
		/// </summary>
		/// <returns>
		/// A <see cref="IEnumerator"/>
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			foreach (var s in Sections)
				yield return s;
		}

		IEnumerator<Section> IEnumerable<Section>.GetEnumerator ()
		{
			foreach (var s in Sections)
				yield return s;
		}

		public override UITableViewCell GetCell (UITableView tv)
		{
			NSString key = summarySection == -1 ? rkey1 : rkey2;
			var cell = tv.DequeueReusableCell (key);
			if (cell is null) {
				var style = summarySection == -1 ? UITableViewCellStyle.Default : UITableViewCellStyle.Value1;

				cell = new UITableViewCell (style, key);
				cell.SelectionStyle = UITableViewCellSelectionStyle.Blue;
			}

			cell.TextLabel.Text = Caption;
			if (group is not null) {
				int count = 0;

				foreach (var s in Sections) {
					foreach (var e in s.Elements) {
						var be = e as BoolElement;
						if (be is not null) {
							if (be.Value)
								count++;
							continue;
						}
					}
				}
				cell.DetailTextLabel.Text = count.ToString ();
			} else if (summarySection != -1 && summarySection < Sections.Count) {
				var s = Sections [summarySection];
				if (summaryElement < s.Elements.Count && cell.DetailTextLabel is not null)
					cell.DetailTextLabel.Text = s.Elements [summaryElement].Summary ();
			}
			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;

			return cell;
		}

		/// <summary>
		///    This method does nothing by default, but gives a chance to subclasses to
		///    customize the UIViewController before it is presented
		/// </summary>
		protected virtual void PrepareDialogViewController (UIViewController dvc)
		{
		}

		/// <summary>
		/// Creates the UIViewController that will be pushed by this RootElement
		/// </summary>
		protected virtual UIViewController MakeViewController ()
		{
			if (createOnSelected is not null)
				return createOnSelected (this);

			return new DialogViewController (this, true) {
				Autorotate = true
			};
		}

		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			tableView.DeselectRow (path, false);
			var newDvc = MakeViewController ();
			PrepareDialogViewController (newDvc);
			dvc.ActivateController (newDvc);
		}

		public void Reload (Section section, UITableViewRowAnimation animation)
		{
			if (section is null)
				throw new ArgumentNullException ("section");
			if (section.Parent is null || section.Parent != this)
				throw new ArgumentException ("Section is not attached to this root");

			int idx = 0;
			foreach (var sect in Sections) {
				if (sect == section) {
					if (TableView is null)
						return;
					TableView.ReloadSections (new NSIndexSet ((uint) idx), animation);
					return;
				}
				idx++;
			}
		}

		public void Reload (Element element, UITableViewRowAnimation animation)
		{
			if (element is null)
				throw new ArgumentNullException ("element");
			var section = element.Parent as Section;
			if (section is null)
				throw new ArgumentException ("Element is not attached to this root");
			var root = section.Parent as RootElement;
			if (root is null)
				throw new ArgumentException ("Element is not attached to this root");
			var path = element.IndexPath;
			if (path is null || TableView is null)
				return;
			TableView.ReloadRows (new NSIndexPath [] { path }, animation);
		}

	}
}
