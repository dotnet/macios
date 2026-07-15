// Copyright 2011-2012 Xamarin Inc. All rights reserved

#if !__TVOS__ && !MONOMAC

using System.Drawing;
using System.Reflection;
using CoreGraphics;
using UIKit;
using Xamarin.Utils;

namespace MonoTouchFixtures.UIKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ActionSheetTest {

		void CheckDefault (UIActionSheet a)
		{
			Assert.That (a.ButtonCount, Is.EqualTo ((nint) 0), "ButtonCount");
			Assert.That (a.CancelButtonIndex, Is.EqualTo ((nint) (-1)), "CancelButtonIndex");
			Assert.That (a.Delegate, Is.Null, "Delegate");
			Assert.That (a.DestructiveButtonIndex, Is.EqualTo ((nint) (-1)), "DestructiveButtonIndex");
			Assert.That (a.FirstOtherButtonIndex, Is.EqualTo ((nint) (-1)), "FirstOtherButtonIndex");

			var style = TestRuntime.CheckSystemVersion (ApplePlatform.iOS, 8, 0) ? UIActionSheetStyle.Default : UIActionSheetStyle.Automatic;
			Assert.That (a.Style, Is.EqualTo (style), "Style");

			Assert.That (a.Title, Is.Null, "Title");

			Assert.That (a.Visible, Is.False, "Visible");

			Assert.That (a.WeakDelegate, Is.Null, "WeakDelegate");
		}

		[Test]
		public void CtorDefault ()
		{
			using (UIActionSheet a = new UIActionSheet ()) {
				CheckDefault (a);
			}
		}

		[Test]
		public void CtorAllNulls ()
		{
			// http://bugzilla.xamarin.com/show_bug.cgi?id=3081
			using (UIActionSheet a = new UIActionSheet (null, null, null, null)) {
				CheckDefault (a);
			}
		}

		[Test]
		public void InitWithFrame ()
		{
			var frame = new CGRect (10, 10, 100, 100);
			using (UIActionSheet a = new UIActionSheet (frame)) {
				Assert.That (a.Frame, Is.EqualTo (frame), "Frame");
				CheckDefault (a);
			}
		}

		class MyActionSheetDelegate : UIActionSheetDelegate {
		}

		[Test]
		public void CtorDelegate ()
		{
			using (var del = new MyActionSheetDelegate ())
			using (var a = new UIActionSheet ("title", del, null, null, null)) {
				Assert.That (a.Title, Is.EqualTo ("title"), "Title");
				Assert.That (typeof (UIActionSheet).GetField ("__mt_WeakDelegate_var", BindingFlags.Instance | BindingFlags.NonPublic).GetValue (a), Is.Not.Null, "backing field");
				// check properties after the field (so we're not setting it only when calling the properties)
				Assert.That (a.Delegate, Is.Not.Null, "Delegate");
				Assert.That (a.WeakDelegate, Is.Not.Null, "WeakDelegate");
			}
		}
	}
}

#endif // !__TVOS__ && !MONOMAC
