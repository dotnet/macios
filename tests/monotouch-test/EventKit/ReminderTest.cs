//
// Unit tests for EKReminder
//
// Authors:
//	Marek Safar (marek.safar@gmail.com)
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

#if !__TVOS__

using EventKit;
using Xamarin.Utils;

namespace MonoTouchFixtures.EventKit {

	[Ignore ("default EKReminder constructor fails in xcode 11 beta 3 - https://github.com/xamarin/maccore/issues/1832")]
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ReminderTest {
		[SetUp]
		public void Setup ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 8, throwIfOtherPlatform: false);
		}

		[Test]
		public void DefaultProperties ()
		{
			using var store = new EKEventStore ();
			using (var rem = EKReminder.Create (store)) {
				Assert.That (rem.Priority, Is.EqualTo (0), "Priority");
				Assert.That (rem.Completed, Is.False, "Completed");
				Assert.That (rem.CompletionDate, Is.Null, "CompletionDate");
				Assert.That (rem.StartDateComponents, Is.Null, "StartDateComponents");
				Assert.That (rem.DueDateComponents, Is.Null, "DueDateComponents");

				rem.Completed = true;
				Assert.That (rem.Completed, Is.True, "Completed - Changed");
			}
		}

		[Test]
		public void NullableProperties ()
		{
			using var store = new EKEventStore ();
			using (var rem = EKReminder.Create (store)) {
				rem.StartDateComponents = null;
				rem.DueDateComponents = null;
				rem.CompletionDate = null;
			}
		}

		[Test]
		public void Range ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 9, throwIfOtherPlatform: false);

			using var store = new EKEventStore ();
			using (var rem = EKReminder.Create (store)) {
				// priority is documented to have a range of 0-9 but there's no validation in ObjC
				// this test is here to ensure Apple does not start throwing native exceptions at some points
				rem.Priority = -1;
				Assert.That (rem.Priority, Is.EqualTo ((nint) (-1)), "-1");
				rem.Priority = 10;
				Assert.That (rem.Priority, Is.EqualTo ((nint) 10), "10");

				// The following tests fail randomly.
				//				// at some point values are ?normalized? but no exception is thrown
				//				rem.Priority = nint.MinValue;
				//				Assert.That (rem.Priority, Is.EqualTo ((nint) 0), "MinValue");
				//				// exposed as an NSInteger but internal storage looks different
				//				rem.Priority = nint.MaxValue;
				//				Assert.That (rem.Priority, Is.EqualTo ((nint) (-1)), "MaxValue");
			}
		}
	}
}

#endif // !__TVOS__
