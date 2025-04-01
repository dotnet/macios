using System;
using ObjCRuntime;
using Foundation;
using CoreGraphics;
using CoreLocation;

#if !MONOMAC
using UIKit;
#endif

namespace EventKit {

	// untyped enum -> EKTypes.h
	// iOS 9 promoted this to an NSInteger - which breaks compatibility
	/// <summary>Specifies the type of a calendar.</summary>
	[Native]
	public enum EKCalendarType : long {
		/// <summary>The calendar is stored locally.</summary>
		Local,
		/// <summary>The calendar is being accessed via the CalDAV protocol.</summary>
		CalDav,
		/// <summary>The calendar is being accessed via Microsoft Exchange Server protocols.</summary>
		Exchange,
		/// <summary>The calendar is being accessed via subscription (iCal).</summary>
		Subscription,
		/// <summary>The calendar is a birthday calendar.</summary>
		Birthday,
	}

	// untyped enum -> EKTypes.h
	// iOS 9 promoted this to an NSInteger - which breaks compatibility
	/// <summary>The kind of participant to the event.</summary>
	[Native]
	public enum EKParticipantType : long {
		/// <summary>To be added.</summary>
		Unknown,
		/// <summary>A person.</summary>
		Person,
		/// <summary>Use to book a room.</summary>
		Room,
		/// <summary>Use to book resources (photocopier, company limo or private jets).</summary>
		Resource,
		/// <summary>Groups of people.</summary>
		Group,
	}

	// untyped enum -> EKTypes.h
	// iOS 9 promoted this to an NSInteger - which breaks compatibility
	/// <summary>The role of an <see cref="T:EventKit.EKParticipant" /></summary>
	[Native]
	public enum EKParticipantRole : long {
		/// <summary>Uknonwn role.   The participant might want to consider his role in the group.</summary>
		Unknown,
		/// <summary>Attendance is required.</summary>
		Required,
		/// <summary>Attendance is optional.   This means that you can get away by not showing up at the meeting.   Or you can use this to politely invite someone, hoping they wont show up.</summary>
		Optional,
		/// <summary>Chairing the meeting.</summary>
		Chair,
		/// <summary>Not a participant.</summary>
		NonParticipant,
	}

	// untyped enum -> EKTypes.h
	// iOS 9 promoted this to an NSInteger - which breaks compatibility
	/// <summary>The status of a participant.</summary>
	[Native]
	public enum EKParticipantStatus : long {
		/// <summary>We do not know what the participant's intentions are at this point.</summary>
		Unknown,
		/// <summary>The participant has not yet responded to this event.   Probably trying to find someone he can EKParticipantStatus.Delegate this too.   If you know what I mean.</summary>
		Pending,
		/// <summary>The participant has expressed his commitment to this meeting by accepting your invitation.</summary>
		Accepted,
		/// <summary>The participant has declined to participate in this event.</summary>
		Declined,
		/// <summary>The participant will tentatively will join this event.</summary>
		Tentative,
		/// <summary>The participant delegate this event to someone else.</summary>
		Delegated,
		/// <summary>The event has completed.</summary>
		Completed,
		/// <summary>In process.   </summary>
		InProcess,
	}

	// untyped enum -> EKError.h
	// iOS 9 promoted this to an NSInteger - which breaks compatibility
	/// <summary>Errors returned by Event Kit.</summary>
	[Native]
	[ErrorDomain ("EKErrorDomain")]
	public enum EKErrorCode : long {
		/// <summary>The event cannot be modified.</summary>
		EventNotMutable,
		/// <summary>The event does not have an associated calendar.</summary>
		NoCalendar,
		/// <summary>The StartDate property has not been set for the event.</summary>
		NoStartDate,
		/// <summary>The EndDate property has not been set for the event.</summary>
		NoEndDate,
		/// <summary>The event EndDate cannot occur before the event StartDate</summary>
		DatesInverted,
		/// <summary>An internal error has occurred.</summary>
		InternalFailure,
		/// <summary>The calendar cannot be modified.</summary>
		CalendarReadOnly,
		/// <summary>The event duration is too large for the interval specified by the event's recurrence rule.</summary>
		DurationGreaterThanRecurrence,
		/// <summary>The alarm's RelativeOffset is too large for the interval specified by the event's recurrence rule.</summary>
		AlarmGreaterThanRecurrence,
		/// <summary>The event's StartDate is beyond the end of the calendar.</summary>
		StartDateTooFarInFuture,
		/// <summary>The event's StartDate is set to collide with other occurrences of the same event.</summary>
		StartDateCollidesWithOtherOccurrence,
		/// <summary>The object is not stored in the same event store.</summary>
		ObjectBelongsToDifferentStore,
		/// <summary>Invites cannot be modified.</summary>
		InvitesCannotBeMoved,
		/// <summary>The time span for the event was invalid.</summary>
		InvalidSpan,
		/// <summary>The calendar could not be saved because its source was not set.</summary>
		CalendarHasNoSource,
		/// <summary>The calendar source can not be modified.</summary>
		CalendarSourceCannotBeModified,
		/// <summary>The calendar is immutable.</summary>
		CalendarIsImmutable,
		/// <summary>The source will not allow the developer to add or delete calendars.</summary>
		SourceDoesNotAllowCalendarAddDelete,
		/// <summary>A recurring reminder's due date was missing.</summary>
		RecurringReminderRequiresDueDate,
		/// <summary>The soure does not support structured locations.</summary>
		StructuredLocationsNotSupported,
		/// <summary>Locations are not supported for reminders on the source.</summary>
		ReminderLocationsNotSupported,
		/// <summary>Geofenced alarms are not supported.</summary>
		AlarmProximityNotSupported,
		/// <summary>The calendar does not allow events.</summary>
		CalendarDoesNotAllowEvents,
		/// <summary>The calendar does not allow reminders.</summary>
		CalendarDoesNotAllowReminders,
		/// <summary>The source does not allow reminders.</summary>
		SourceDoesNotAllowReminders,
		/// <summary>The source does not allow events.</summary>
		SourceDoesNotAllowEvents,
		/// <summary>To be added.</summary>
		PriorityIsInvalid,
		/// <summary>To be added.</summary>
		InvalidEntityType,
		/// <summary>To be added.</summary>
		ProcedureAlarmsNotMutable,
		/// <summary>To be added.</summary>
		EventStoreNotAuthorized,
		/// <summary>To be added.</summary>
		OSNotSupported,
		InvalidInviteReplyCalendar,
		NotificationsCollectionFlagNotSet,
		SourceMismatch,
		NotificationCollectionMismatch,
		NotificationSavedWithoutCollection,
		ReminderAlarmContainsEmailOrUrl,
	}

	// untyped enum -> EKTypes.h
	// Special note: some API (like `dayOfWeek:` and `dayOfWeek:weekNumber:` use an `NSInteger` instead of the enum
	/// <summary>Enumeration of the days of the week.</summary>
	[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'EKWeekday'.")]
	[Deprecated (PlatformName.MacOSX, 10, 11, message: "Use 'EKWeekday'.")]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'EKWeekday'.")]
	public enum EKDay {
		/// <summary>Not set</summary>
		NotSet = 0,
		/// <summary>Sunday</summary>
		Sunday = 1,
		/// <summary>Monday</summary>
		Monday,
		/// <summary>Tuesday</summary>
		Tuesday,
		/// <summary>Wednesday</summary>
		Wednesday,
		/// <summary>Thursday</summary>
		Thursday,
		/// <summary>Friday</summary>
		Friday,
		/// <summary>Saturday</summary>
		Saturday,
	}

	/// <summary>Enumerates the days of the week.</summary>
	[MacCatalyst (13, 1)]
	[Native] // NSInteger (size change from previously untyped enum)
	public enum EKWeekday : long {
		/// <summary>To be added.</summary>
		NotSet = 0,
		/// <summary>To be added.</summary>
		Sunday = 1,
		/// <summary>To be added.</summary>
		Monday,
		/// <summary>To be added.</summary>
		Tuesday,
		/// <summary>To be added.</summary>
		Wednesday,
		/// <summary>To be added.</summary>
		Thursday,
		/// <summary>To be added.</summary>
		Friday,
		/// <summary>To be added.</summary>
		Saturday,
	}

	// untyped enum -> EKTypes.h
	// iOS 9 promoted this to an NSInteger - which breaks compatibility
	/// <summary>The recurring frequency for the event.</summary>
	[Native]
	public enum EKRecurrenceFrequency : long {
		/// <summary>Recurs daily.</summary>
		Daily,
		/// <summary>Recurs weekly.</summary>
		Weekly,
		/// <summary>Recurs monthly.</summary>
		Monthly,
		/// <summary>Recurs yearly.</summary>
		Yearly,
	}

	// untyped enum -> EKEventStore.h
	// iOS 9 promoted this to an NSInteger - which breaks compatibility
	/// <summary>Determines how changes to an event will be applied, either it affects a single instance or affects all instances.</summary>
	[Native]
	public enum EKSpan : long {
		/// <summary>The change only affects a particular instance of the event.</summary>
		ThisEvent,
		/// <summary>The change affects all future events.</summary>
		FutureEvents,
	}

	// NSUInteger -> EKTypes.h
	/// <summary>Specifies the types of availability information the calendar is capable of providing for its associated events.</summary>
	[Native ("EKCalendarEventAvailabilityMask")]
	[Flags]
	public enum EKCalendarEventAvailability : ulong {
		/// <summary>The calendar cannot provide availability status for its associated events.</summary>
		None = 0,
		/// <summary>The calendar can provide busy status for its associated events.</summary>
		Busy = 1,
		/// <summary>The calendar can provide free status for its associated events.</summary>
		Free = 2,
		/// <summary>The calendar can provide tentative status for its associated events.</summary>
		Tentative = 4,
		/// <summary>The calendar can provide unavailable status for its associated events.</summary>
		Unavailable = 8,
	}

	// untyped enum -> EKEvent.h
	// iOS 9 promoted this to an NSInteger - which breaks compatibility
	/// <summary>Types of availability </summary>
	[Native]
	public enum EKEventAvailability : long {
		/// <summary>Calendar does not provide availability information for events.</summary>
		NotSupported = -1,
		/// <summary>Duration of event is reported as busy.</summary>
		Busy = 0,
		/// <summary>Duration of event is reported as free.</summary>
		Free,
		/// <summary>Duration of event is reported as tentative.</summary>
		Tentative,
		/// <summary>Duration of event is reported as unavailable.</summary>
		Unavailable,
	}

	// untyped enum -> EKEvent.h
	// iOS 9 promoted this to an NSInteger - which breaks compatibility
	/// <summary>The event status.</summary>
	[Native]
	public enum EKEventStatus : long {
		/// <summary>This event has no status set.</summary>
		None,
		/// <summary>The event has been confirmed.</summary>
		Confirmed,
		/// <summary>The event is flagged as tentative.</summary>
		Tentative,
		/// <summary>The event has been cancelled.</summary>
		Cancelled,
	}

	// untyped enum -> EKTypes.h
	// iOS 9 promoted this to an NSInteger - which breaks compatibility
	/// <summary>An enumeration whose values indicate the type of a <see cref="T:EventKit.EKSource" /> object.</summary>
	[Native]
	public enum EKSourceType : long {
		/// <summary>To be added.</summary>
		Local,
		/// <summary>To be added.</summary>
		Exchange,
		/// <summary>To be added.</summary>
		CalDav,
		/// <summary>To be added.</summary>
		MobileMe,
		/// <summary>To be added.</summary>
		Subscribed,
		/// <summary>To be added.</summary>
		Birthdays,
	}

	// NSInteger -> EKTypes.h
	/// <summary>An enumeration whose values specify whether an event was raised by entering or leaving a region.</summary>
	[Native]
	public enum EKAlarmProximity : long {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		Enter,
		/// <summary>To be added.</summary>
		Leave,
	}

	// NSUInteger -> EKTypes.h
	/// <summary>An enumeration whose values represent the allowed entity types for a <see cref="T:EventKit.EKCalendar" />.</summary>
	[Native]
	[Flags]
	public enum EKEntityMask : ulong {
		/// <summary>To be added.</summary>
		Event = 1 << (int) EKEntityType.Event,
		/// <summary>To be added.</summary>
		Reminder = 1 << (int) EKEntityType.Reminder,
	}

	// NSUInteger -> EKTypes.h
	/// <summary>An enumeration that specifies whether an entity is an event or a reminder.</summary>
	[Native]
	public enum EKEntityType : ulong {
		/// <summary>To be added.</summary>
		Event,
		/// <summary>To be added.</summary>
		Reminder,
	}

#if MONOMAC
	// untyped enum -> EKTypes.h (but not in the iOS SDK, only OSX)
	// turned into a typed (NSInteger) enum in El Capitan (and also an NSInteger in watchOS)
	[Native]
	public enum EKAlarmType : long {
		/// <summary>To be added.</summary>
		Display,
		/// <summary>To be added.</summary>
		Audio,
		/// <summary>To be added.</summary>
		Procedure,
		/// <summary>To be added.</summary>
		Email,
	}
#endif
	// NSInteger -> EKEventStore.h
	/// <summary>An enumeration whose values specify the restrictions on access to the <see cref="T:EventKit.EKEventStore" />. Retrieved by calling the <see cref="M:EventKit.EKEventStore.GetAuthorizationStatus(EventKit.EKEntityType)" /> method.</summary>
	[Native]
	public enum EKAuthorizationStatus : long {
		/// <summary>The user has not interacted with the permissions dialog.</summary>
		NotDetermined = 0,
		/// <summary>Access is denied and the user cannot change permission.</summary>
		Restricted,
		/// <summary>The user has denied access.</summary>
		Denied,
		/// <summary>The user has granted access.</summary>
		Authorized,
		WriteOnly,
	}

	[Native]
	public enum EKParticipantScheduleStatus : long {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		Pending,
		/// <summary>To be added.</summary>
		Sent,
		/// <summary>To be added.</summary>
		Delivered,
		/// <summary>To be added.</summary>
		RecipientNotRecognized,
		/// <summary>To be added.</summary>
		NoPrivileges,
		/// <summary>To be added.</summary>
		DeliveryFailed,
		/// <summary>To be added.</summary>
		CannotDeliver,
		/// <summary>To be added.</summary>
		RecipientNotAllowed,
	}

	[Native]
	public enum EKReminderPriority : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		High = 1,
		/// <summary>To be added.</summary>
		Medium = 5,
		/// <summary>To be added.</summary>
		Low = 9,
	}

}
