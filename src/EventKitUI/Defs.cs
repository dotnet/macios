//
// Defs.cs: Core Definitions for buildling the API eventkitui.cs
//
// Authors:
//   Miguel de Icaza
// 
// Copyright 2011-2015 Xamarin Inc.
//

#nullable enable

using ObjCRuntime;

namespace EventKitUI {

	// untyped enum -> EKCalendarChooser.h
	// iOS 9 promoted this to an NSInteger - which breaks compatibility
	/// <summary>An enumeration whose values specify whether a single or multiple calendars can be chosen by an <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=T:MonoTuoch.EventKitUI.EKCalendarChooser&amp;scope=Xamarin" title="T:MonoTuoch.EventKitUI.EKCalendarChooser">T:MonoTuoch.EventKitUI.EKCalendarChooser</a></format> object.</summary>
	///     <remarks>To be added.</remarks>
	[Native]
	public enum EKCalendarChooserSelectionStyle : long {
		/// <summary>To be added.</summary>
		Single,
		/// <summary>To be added.</summary>
		Multiple,
	}

	// untyped enum -> EKCalendarChooser.h
	// iOS 9 promoted this to an NSInteger - which breaks compatibility
	/// <summary>An enumeration whose values specify which calendars are displayed by a <see cref="T:EventKitUI.EKCalendarChooser" />.</summary>
	///     <remarks>To be added.</remarks>
	[Native]
	public enum EKCalendarChooserDisplayStyle : long {
		/// <summary>To be added.</summary>
		AllCalendars,
		/// <summary>To be added.</summary>
		WritableCalendarsOnly,
	}

	// untyped enum -> EKEventViewController.h
	// iOS 9 promoted this to an NSInteger - which breaks compatibility
	/// <summary>Enumerates actions that a user can take to dismiss an event view controller.</summary>
	///     <remarks>To be added.</remarks>
	[Native]
	public enum EKEventViewAction : long {
		/// <summary>The user tapped "Done".</summary>
		Done,
		/// <summary>The user responded to the event, saving it.</summary>
		Responded,
		/// <summary>The user deleted the event.</summary>
		Deleted,
	}

	// untyped enum -> EKEventEditViewController.h
	// iOS 9 promoted this to an NSInteger - which breaks compatibility
	/// <summary>Enumerates possible actions that a user can take when editing a view.</summary>
	///     <remarks>To be added.</remarks>
	[Native]
	public enum EKEventEditViewAction : long {
		/// <summary>The user canceled the change to the event.</summary>
		Canceled,
		/// <summary>The user saved the change to the event.</summary>
		Saved,
		/// <summary>The user deleted the event.</summary>
		Deleted,
	}
}
