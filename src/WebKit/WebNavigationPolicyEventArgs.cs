//
// WebNavigationPolicyEventArgs.cs
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)
//
// Copyright 2013 Xamarin Inc

#if __MACOS__

#nullable enable

namespace WebKit {

	// Convenience enum.
	/// <summary>Specifies the mouse button that triggered a web navigation action.</summary>
	public enum WebActionMouseButton {
		/// <summary>No mouse button was pressed.</summary>
		None = -1,
		/// <summary>The left mouse button was pressed.</summary>
		Left = 0,
		/// <summary>The middle mouse button was pressed.</summary>
		Middle = 1,
		/// <summary>The right mouse button was pressed.</summary>
		Right = 2,
	}

	/// <summary>Provides data for the web navigation policy event, including the navigation type, mouse button, and modifier flags.</summary>
	[ObsoletedOSPlatform ("macos10.14", "No longer supported.")]
	[SupportedOSPlatform ("macos")]
	partial class WebNavigationPolicyEventArgs {

		/// <summary>Gets the type of navigation that triggered this policy decision.</summary>
		/// <value>The <see cref="WebNavigationType" /> of the action.</value>
		public WebNavigationType NavigationType {
			get { return (WebNavigationType) (((NSNumber?) ActionInformation [WebPolicyDelegate.WebActionNavigationTypeKey])?.Int32Value ?? 0); }
		}

		/// <summary>Gets the element information dictionary for the action, if available.</summary>
		/// <value>A dictionary containing information about the element that triggered the action, or <see langword="null" /> if not available.</value>
		public NSDictionary? ElementInfo {
			get { return ActionInformation [WebPolicyDelegate.WebActionElementKey] as NSDictionary; }
		}

		/// <summary>Gets the mouse button that triggered the navigation action.</summary>
		/// <value>The <see cref="WebActionMouseButton" /> that was pressed, or <see cref="WebActionMouseButton.None" /> if no button was pressed.</value>
		public WebActionMouseButton MouseButton {
			get {
				var number = ActionInformation [WebPolicyDelegate.WebActionButtonKey] as NSNumber;
				if (number is null) {
					return WebActionMouseButton.None;
				}

				return (WebActionMouseButton) number.Int32Value;
			}
		}

		/// <summary>Gets the keyboard modifier flags that were active during the navigation action.</summary>
		/// <value>The modifier flags as an unsigned integer bitmask.</value>
		public uint Flags {
			get { return ((NSNumber?) ActionInformation [WebPolicyDelegate.WebActionModifierFlagsKey])?.UInt32Value ?? 0; }
		}

		/// <summary>Gets the original URL that triggered the navigation action.</summary>
		/// <value>The original URL, or <see langword="null" /> if not available.</value>
		public NSUrl? OriginalUrl {
			get { return ActionInformation [WebPolicyDelegate.WebActionOriginalUrlKey] as NSUrl; }
		}
	}
}

#endif // __MACOS__
