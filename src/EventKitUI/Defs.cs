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
	[Native]
	public enum EKCalendarChooserSelectionStyle : long {
		/// <summary>To be added.</summary>
		Single,
		/// <summary>To be added.</summary>
		Multiple,
	}

	// untyped enum -> EKCalendarChooser.h
	// iOS 9 promoted this to an NSInteger - which breaks compatibility
	[Native]
	public enum EKCalendarChooserDisplayStyle : long {
		/// <summary>To be added.</summary>
		AllCalendars,
		/// <summary>To be added.</summary>
		WritableCalendarsOnly,
	}

	// untyped enum -> EKEventViewController.h
	// iOS 9 promoted this to an NSInteger - which breaks compatibility
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
