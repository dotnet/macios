//
// NSSegmentedControl: Support for the NSSegmentedControl class
//
// Author:
//   Pavel Sich (pavel.sich@me.com)
//

#if !__MACCATALYST__

using System;
using ObjCRuntime;
using Foundation;

#nullable enable

namespace AppKit {

	public partial class NSSegmentedControl {
		NSActionDispatcher? dispatcher;

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public new NSSegmentedCell Cell {
			get { return (NSSegmentedCell) base.Cell; }
			set { base.Cell = value; }
		}

		/// <param name="labels">To be added.</param>
		///         <param name="trackingMode">To be added.</param>
		///         <param name="action">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		public static NSSegmentedControl FromLabels (string [] labels, NSSegmentSwitchTracking trackingMode, Action action)
		{
			var dispatcher = new NSActionDispatcher (action);
			var control = _FromLabels (labels, trackingMode, dispatcher, NSActionDispatcher.Selector);
			control.dispatcher = dispatcher;
			return control;
		}

		/// <param name="images">To be added.</param>
		///         <param name="trackingMode">To be added.</param>
		///         <param name="action">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		public static NSSegmentedControl FromImages (NSImage [] images, NSSegmentSwitchTracking trackingMode, Action action)
		{
			var dispatcher = new NSActionDispatcher (action);
			var control = _FromImages (images, trackingMode, dispatcher, NSActionDispatcher.Selector);
			control.dispatcher = dispatcher;
			return control;
		}

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void UnselectAllSegments ()
		{
			NSSegmentSwitchTracking current = this.Cell.TrackingMode;
			this.Cell.TrackingMode = NSSegmentSwitchTracking.Momentary;

			for (nint i = 0; i < this.SegmentCount; i++)
				SetSelected (false, i);

			this.Cell.TrackingMode = current;
		}

	}
}
#endif // !__MACCATALYST__
