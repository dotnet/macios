//
// Unit tests for EKEventStore
//
// Authors:
//	Marek Safar (marek.safar@gmail.com)
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

#if !__TVOS__

using EventKit;
using System.Threading;
using System.Linq;

namespace MonoTouchFixtures.EventKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class EventStoreTest {
		[Test]
		[Ignore ("fail on a cleaned iOS 6 simulator and (differently) on devices")]
		public void DefaultCalendar ()
		{
			var store = new EKEventStore ();
			ClassicAssert.AreEqual ("Calendar", store.DefaultCalendarForNewEvents.Title, "DefaultCalendarForNewEvents");
			ClassicAssert.IsNull (store.DefaultCalendarForNewReminders, "DefaultCalendarForNewReminders");
#if !MONOMAC // Not available on Mac
			ClassicAssert.IsNotNull (store.Calendars, "Calendars");
#endif
			ClassicAssert.IsNotNull (store.Sources, "Sources");
		}

#if false
		// The EKEventStore constructor is no longer availble on iOS 6 Beta 4

		[Test]
		[Ignore ("fail on a cleaned iOS 6 simulator and (differently) on devices")]
		public void DefaultReminder ()
		{
			var store = new EKEventStore (EKEntityMask.Reminder);
			ClassicAssert.AreEqual ("Reminders", store.DefaultCalendarForNewReminders.Title, "DefaultCalendarForNewReminders");
			ClassicAssert.IsNull (store.DefaultCalendarForNewEvents, "DefaultCalendarForNewEvents");
			ClassicAssert.IsNotNull (store.Calendars, "Calendars");
			ClassicAssert.IsNotNull (store.Sources, "Sources");
		}

		[Test]
		[Ignore ("fail on a cleaned iOS 6 simulator and (differently) on devices")]
		public void GetCalendars ()
		{
			var store = new EKEventStore (EKEntityMask.Reminder);
			var calendars = store.GetCalendars (EKEntityType.Reminder);
			ClassicAssert.AreEqual ("Reminders", calendars[0].Title, "#1");

			calendars = store.GetCalendars (EKEntityType.Event);
			ClassicAssert.AreEqual (0, calendars.Length, "#2");
		}

		[Test]
		public void Predicates()
		{
			if (Runtime.Arch == Arch.DEVICE)
				Assert.Inconclusive ("defaults are different on devices");

			var store = new EKEventStore (EKEntityMask.Reminder);
			var rem = EKReminder.Create (store);
			rem.Calendar = store.DefaultCalendarForNewReminders;

			NSError error;
			ClassicAssert.IsTrue (store.SaveReminder (rem, true, out error), "SaveReminder");

			var predicate = store.PredicateForIncompleteReminders (null, null, new [] { rem.Calendar });
			var mre = new ManualResetEvent (false);
			bool found = false;
			store.FetchReminders (predicate, l => {
				found = l.Any (ll => ll.ClassHandle == rem.ClassHandle);
				mre.Set ();
			});

			ClassicAssert.IsTrue (mre.WaitOne (3000), "#1");
			ClassicAssert.IsTrue (found, "#2");

			mre.Reset ();
			predicate = store.PredicateForReminders (null);

			store.FetchReminders (predicate, l => mre.Set ());
			ClassicAssert.IsTrue (mre.WaitOne (3000), "#10");

			mre.Reset ();
			predicate = store.PredicateForCompleteReminders (null, null, null);

			store.FetchReminders (predicate, l => mre.Set ());
			ClassicAssert.IsTrue (mre.WaitOne (3000), "#20");

			ClassicAssert.IsTrue (store.RemoveReminder (rem, true, out error), "RemoveReminder");
		}
#endif
	}
}

#endif // !__TVOS__
