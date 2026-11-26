//
// NSSegmentedControl: Support for the NSSegmentedControl class
//
// Author:
//   Pavel Sich (pavel.sich@me.com)
//

#if !__MACCATALYST__

#nullable enable

namespace AppKit {

	public partial class NSSegmentedControl {
		NSActionDispatcher? dispatcher;

		/// <summary>Gets or sets the cell that implements the segmented control.</summary>
		/// <value>The <see cref="NSSegmentedCell" /> that implements the segmented control.</value>
		/// <remarks>
		///   <para>This property is obsolete. Use the properties and methods on <see cref="NSSegmentedControl" /> directly instead.</para>
		/// </remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.10")]
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
#pragma warning disable CA1422 // This call site is reachable on: 'macOS/OSX' 14.0 and later. 'NSSegmentedControl.Cell' is obsoleted on: 'macOS/OSX' 10.10 and later.
			NSSegmentSwitchTracking current = this.Cell.TrackingMode;
			this.Cell.TrackingMode = NSSegmentSwitchTracking.Momentary;

			for (nint i = 0; i < this.SegmentCount; i++)
				SetSelected (false, i);

			this.Cell.TrackingMode = current;
#pragma warning restore CA1422
		}

	}
}
#endif // !__MACCATALYST__
