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

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[ObsoletedOSPlatform ("macos10.14", "No longer supported.")]
	[SupportedOSPlatform ("macos")]
	partial class WebNavigationPolicyEventArgs {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public WebNavigationType NavigationType {
			get { return (WebNavigationType) (((NSNumber?) ActionInformation [WebPolicyDelegate.WebActionNavigationTypeKey])?.Int32Value ?? 0); }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSDictionary? ElementInfo {
			get { return ActionInformation [WebPolicyDelegate.WebActionElementKey] as NSDictionary; }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public WebActionMouseButton MouseButton {
			get {
				var number = ActionInformation [WebPolicyDelegate.WebActionButtonKey] as NSNumber;
				if (number is null) {
					return WebActionMouseButton.None;
				}

				return (WebActionMouseButton) number.Int32Value;
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public uint Flags {
			get { return ((NSNumber?) ActionInformation [WebPolicyDelegate.WebActionModifierFlagsKey])?.UInt32Value ?? 0; }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSUrl? OriginalUrl {
			get { return ActionInformation [WebPolicyDelegate.WebActionOriginalUrlKey] as NSUrl; }
		}
	}
}

#endif // __MACOS__
