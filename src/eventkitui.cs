//
// This file describes the API that the generator will produce
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2010, Novell, Inc.
// Copyright 2014 Xamarin Inc. All rights reserved.
//
using ObjCRuntime;
using Foundation;
using CoreGraphics;
using CoreLocation;
using UIKit;
using EventKit;
using System;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace EventKitUI {
	/// <summary>UIViewController used to display the details of a calendar event.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/EventKitUI/Reference/EKEventViewControllerClassRef/index.html">Apple documentation for <c>EKEventViewController</c></related>
	[BaseType (typeof (UIViewController), Delegates = new string [] { "WeakDelegate" }, Events = new Type [] { typeof (EKEventViewDelegate) })]
	interface EKEventViewController {
		[Export ("initWithNibName:bundle:")]
		[PostGet ("NibBundle")]
		NativeHandle Constructor ([NullAllowed] string nibName, [NullAllowed] NSBundle bundle);

		/// <summary>Gets or sets the calendar event to display.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("event")]
		EKEvent Event { get; set; }

		/// <summary>Gets or sets a Boolean value that controls whether the user can edit the event.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("allowsEditing")]
		bool AllowsEditing { get; set; }

		/// <summary>Gets or sets a Boolean value that controls whether the event can be viewed in the day preview.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("allowsCalendarPreview")]
		bool AllowsCalendarPreview { get; set; }

		/// <summary>An object that can respond to the delegate protocol for this type</summary>
		///         <value>The instance that will respond to events and data requests.</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>   Methods must be decorated with the [Export ("selectorName")] attribute to respond to each method from the protocol.   Alternatively use the Delegate method which is strongly typed and does not require the [Export] attributes on methods.</para>
		///         </remarks>
		[Export ("delegate", ArgumentSemantic.Weak), NullAllowed]
		NSObject WeakDelegate { get; set; }

		/// <summary>An instance of the EventKitUI.IEKEventViewDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the EventKitUI.IEKEventViewDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		IEKEventViewDelegate Delegate { get; set; }
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="T:EventKitUI.EKEventViewDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="T:EventKitUI.EKEventViewDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="T:EventKitUI.EKEventViewDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="T:EventKitUI.EKEventViewDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IEKEventViewDelegate { }

	/// <summary>A delegate object that provides the application developer fine-grained control over events in the life-cycle of a <see cref="T:EventKitUI.EKEventViewController" /> object.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/EventKitUI/Reference/EKEventViewDelegateProtocolRef/index.html">Apple documentation for <c>EKEventViewDelegate</c></related>
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface EKEventViewDelegate {
		[Abstract]
		[Export ("eventViewController:didCompleteWithAction:"), EventArgs ("EKEventView")]
		void Completed (EKEventViewController controller, EKEventViewAction action);
	}

	/// <summary>UIViewController used to create or edit calendar events.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/EventKitUI/Reference/EKEventEditViewControllerClassRef/index.html">Apple documentation for <c>EKEventEditViewController</c></related>
	[BaseType (typeof (UINavigationController), Delegates = new string [] { "WeakEditViewDelegate" }, Events = new Type [] { typeof (EKEventEditViewDelegate) })]
	interface EKEventEditViewController : UIAppearance {
		[Export ("initWithNibName:bundle:")]
		[PostGet ("NibBundle")]
		NativeHandle Constructor ([NullAllowed] string nibName, [NullAllowed] NSBundle bundle);

		[Export ("initWithRootViewController:")]
		[PostGet ("ViewControllers")] // that will PostGet TopViewController and VisibleViewController too
		NativeHandle Constructor (UIViewController rootViewController);

		/// <summary>Gets or sets a weak reference to the view delegate.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("editViewDelegate", ArgumentSemantic.Weak), NullAllowed]
		NSObject WeakEditViewDelegate { get; set; }

		/// <summary>Gets or sets the delegate object to notify when editing is complete.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Wrap ("WeakEditViewDelegate")]
		IEKEventEditViewDelegate EditViewDelegate { get; set; }

		/// <summary>Gets or sets the event store in which the event is saved.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("eventStore")]
		EKEventStore EventStore { get; set; }

		/// <summary>Gets or sets the event to edit.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("event")]
		EKEvent Event { get; set; }

		[Export ("cancelEditing")]
		void CancelEditing ();
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="T:EventKitUI.EKEventEditViewDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="T:EventKitUI.EKEventEditViewDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="T:EventKitUI.EKEventEditViewDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="T:EventKitUI.EKEventEditViewDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IEKEventEditViewDelegate { }

	/// <summary>Delegate class used to receive notifications from the EKEventEditViewController class.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/EventKitUI/Reference/EKEventEditViewDelegateRef/index.html">Apple documentation for <c>EKEventEditViewDelegate</c></related>
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface EKEventEditViewDelegate {
		[Abstract]
		[Export ("eventEditViewController:didCompleteWithAction:"), EventArgs ("EKEventEdit")]
		void Completed (EKEventEditViewController controller, EKEventEditViewAction action);

		[Export ("eventEditViewControllerDefaultCalendarForNewEvents:"), DelegateName ("EKEventEditController"), DefaultValue (null)]
		EKCalendar GetDefaultCalendarForNewEvents (EKEventEditViewController controller);
	}

	/// <summary>A <see cref="T:UIKit.UIViewController" /> that manages the selection of one or more calendars.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/EventKitUI/Reference/EKCalendarChooserClassRef/index.html">Apple documentation for <c>EKCalendarChooser</c></related>
	[BaseType (typeof (UIViewController),
		   Delegates = new string [] { "WeakDelegate" },
		   Events = new Type [] { typeof (EKCalendarChooserDelegate) })]
	interface EKCalendarChooser {
		[Export ("initWithNibName:bundle:")]
		[PostGet ("NibBundle")]
		NativeHandle Constructor ([NullAllowed] string nibName, [NullAllowed] NSBundle bundle);

		[Export ("initWithSelectionStyle:displayStyle:eventStore:")]
		NativeHandle Constructor (EKCalendarChooserSelectionStyle selectionStyle, EKCalendarChooserDisplayStyle displayStyle, EKEventStore eventStore);

		[Export ("initWithSelectionStyle:displayStyle:entityType:eventStore:")]
		NativeHandle Constructor (EKCalendarChooserSelectionStyle selectionStyle, EKCalendarChooserDisplayStyle displayStyle, EKEntityType entityType, EKEventStore eventStore);

		/// <summary>Gets or sets a value that controls whether multiple selections may be made.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("selectionStyle")]
		EKCalendarChooserSelectionStyle SelectionStyle {
			get;
		}

		/// <summary>An object that can respond to the delegate protocol for this type</summary>
		///         <value>The instance that will respond to events and data requests.</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>   Methods must be decorated with the [Export ("selectorName")] attribute to respond to each method from the protocol.   Alternatively use the Delegate method which is strongly typed and does not require the [Export] attributes on methods.</para>
		///         </remarks>
		[Export ("delegate", ArgumentSemantic.Weak), NullAllowed]
		NSObject WeakDelegate { get; set; }

		/// <summary>An instance of the EventKitUI.IEKCalendarChooserDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the EventKitUI.IEKCalendarChooserDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		IEKCalendarChooserDelegate Delegate { get; set; }

		/// <summary>Get or sets a Boolean value that controls whether to display a "Done" button when the picker is displayed modally.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("showsDoneButton")]
		bool ShowsDoneButton { get; set; }

		/// <summary>Get or sets a Boolean value that controls whether to display a cancellation button when the picker is displayed modally.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("showsCancelButton")]
		bool ShowsCancelButton { get; set; }

		/// <summary>Gets or sets a set that contains the calendars that the user has currently selected.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("selectedCalendars", ArgumentSemantic.Copy)]
		NSSet SelectedCalendars { get; set; }
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="T:EventKitUI.EKCalendarChooserDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="T:EventKitUI.EKCalendarChooserDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="T:EventKitUI.EKCalendarChooserDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="T:EventKitUI.EKCalendarChooserDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IEKCalendarChooserDelegate { }

	/// <summary>A delegate object that provides the application developer fine-grained control over events relating to the lifecycle of a <see cref="T:EventKitUI.EKCalendarChooser" /> object.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/EventKitUI/Reference/EKCalendarChooserDelegateProtocolRef/index.html">Apple documentation for <c>EKCalendarChooserDelegate</c></related>
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface EKCalendarChooserDelegate {
		[Export ("calendarChooserSelectionDidChange:")]
		void SelectionChanged (EKCalendarChooser calendarChooser);

		[Export ("calendarChooserDidFinish:")]
		void Finished (EKCalendarChooser calendarChooser);

		[Export ("calendarChooserDidCancel:")]
		void Cancelled (EKCalendarChooser calendarChooser);
	}

}
