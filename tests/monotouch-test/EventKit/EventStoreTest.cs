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
			Assert.That (store.DefaultCalendarForNewEvents.Title, Is.EqualTo ("Calendar"), "DefaultCalendarForNewEvents");
			Assert.That (store.DefaultCalendarForNewReminders, Is.Null, "DefaultCalendarForNewReminders");
#if !MONOMAC // Not available on Mac
			Assert.That (store.Calendars, Is.Not.Null, "Calendars");
#endif
			Assert.That (store.Sources, Is.Not.Null, "Sources");
		}

#if false
		// The EKEventStore constructor is no longer availble on iOS 6 Beta 4

		[Test]
		[Ignore ("fail on a cleaned iOS 6 simulator and (differently) on devices")]
		public void DefaultReminder ()
		{
			var store = new EKEventStore (EKEntityMask.Reminder);
			Assert.That (store.DefaultCalendarForNewReminders.Title, Is.EqualTo ("Reminders"), "DefaultCalendarForNewReminders");
			Assert.That (store.DefaultCalendarForNewEvents, Is.Null, "DefaultCalendarForNewEvents");
			Assert.That (store.Calendars, Is.Not.Null, "Calendars");
			Assert.That (store.Sources, Is.Not.Null, "Sources");
		}

		[Test]
		[Ignore ("fail on a cleaned iOS 6 simulator and (differently) on devices")]
		public void GetCalendars ()
		{
			var store = new EKEventStore (EKEntityMask.Reminder);
			var calendars = store.GetCalendars (EKEntityType.Reminder);
			Assert.That (calendars[0].Title, Is.EqualTo ("Reminders"), "#1");

			calendars = store.GetCalendars (EKEntityType.Event);
			Assert.That (calendars.Length, Is.EqualTo (0), "#2");
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
			Assert.That (store.SaveReminder (rem, true, out error), Is.True, "SaveReminder");

			var predicate = store.PredicateForIncompleteReminders (null, null, new [] { rem.Calendar });
			var mre = new ManualResetEvent (false);
			bool found = false;
			store.FetchReminders (predicate, l => {
				found = l.Any (ll => ll.ClassHandle == rem.ClassHandle);
				mre.Set ();
			});

			Assert.That (mre.WaitOne (3000), Is.True, "#1");
			Assert.That (found, Is.True, "#2");

			mre.Reset ();
			predicate = store.PredicateForReminders (null);

			store.FetchReminders (predicate, l => mre.Set ());
			Assert.That (mre.WaitOne (3000), Is.True, "#10");

			mre.Reset ();
			predicate = store.PredicateForCompleteReminders (null, null, null);

			store.FetchReminders (predicate, l => mre.Set ());
			Assert.That (mre.WaitOne (3000), Is.True, "#20");

			Assert.That (store.RemoveReminder (rem, true, out error), Is.True, "RemoveReminder");
		}
#endif
	}
}

#endif // !__TVOS__
