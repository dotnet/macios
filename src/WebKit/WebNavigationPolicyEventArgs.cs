//
// WebNavigationPolicyEventArgs.cs
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)
//
// Copyright 2013 Xamarin Inc

#if __MACOS__

#nullable enable

using System;

using Foundation;

namespace WebKit {

	// Convenience enum.
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	public enum WebActionMouseButton {
		/// <summary>To be added.</summary>
		None = -1,
		/// <summary>To be added.</summary>
		Left = 0,
		/// <summary>To be added.</summary>
		Middle = 1,
		/// <summary>To be added.</summary>
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
			get { return (WebNavigationType) ((NSNumber) ActionInformation [WebPolicyDelegate.WebActionNavigationTypeKey]).Int32Value; }
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
			get { return ((NSNumber) ActionInformation [WebPolicyDelegate.WebActionModifierFlagsKey]).UInt32Value; }
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
