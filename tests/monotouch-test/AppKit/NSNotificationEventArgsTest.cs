// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#if __MACOS__

using AppKit;
using Foundation;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSNotificationEventArgsTest {
		[Test]
		public void NSViewColumnMoveEventArgs_Properties ()
		{
			using var userInfo = new NSMutableDictionary ();
			userInfo ["NSOldColumn"] = NSNumber.FromNInt (3);
			userInfo ["NSNewColumn"] = NSNumber.FromNInt (7);

			using var notification = NSNotification.FromName ("test", null, userInfo);
			var args = new NSViewColumnMoveEventArgs (notification);

			Assert.That (args.OldColumn, Is.EqualTo ((nint) 3), "OldColumn");
			Assert.That (args.NewColumn, Is.EqualTo ((nint) 7), "NewColumn");
		}

		[Test]
		public void NSViewColumnMoveEventArgs_NullUserInfo ()
		{
			using var notification = NSNotification.FromName ("test", null);
			var args = new NSViewColumnMoveEventArgs (notification);

			Assert.That (args.OldColumn, Is.EqualTo ((nint) 0), "OldColumn");
			Assert.That (args.NewColumn, Is.EqualTo ((nint) 0), "NewColumn");
		}

		[Test]
		public void NSViewColumnResizeEventArgs_Properties ()
		{
			using var column = new NSTableColumn ("testCol");
			using var userInfo = new NSMutableDictionary ();
			userInfo ["NSTableColumn"] = column;
			userInfo ["NSOldWidth"] = NSNumber.FromNInt (42);

			using var notification = NSNotification.FromName ("test", null, userInfo);
			var args = new NSViewColumnResizeEventArgs (notification);

			Assert.That (args.Column, Is.Not.Null, "Column not null");
			Assert.That (args.Column!.Handle, Is.EqualTo (column.Handle), "Column");
			Assert.That (args.OldWidth, Is.EqualTo ((nint) 42), "OldWidth");
		}

		[Test]
		public void NSViewColumnResizeEventArgs_NullUserInfo ()
		{
			using var notification = NSNotification.FromName ("test", null);
			var args = new NSViewColumnResizeEventArgs (notification);

			Assert.That (args.Column, Is.Null, "Column");
			Assert.That (args.OldWidth, Is.EqualTo ((nint) 0), "OldWidth");
		}

		[Test]
		public void NSOutlineViewItemEventArgs_Properties ()
		{
			using var item = new NSObject ();
			using var userInfo = new NSMutableDictionary ();
			userInfo ["NSObject"] = item;

			using var notification = NSNotification.FromName ("test", null, userInfo);
			var args = new NSOutlineViewItemEventArgs (notification);

			Assert.That (args.Item, Is.Not.Null, "Item not null");
			Assert.That (args.Item!.Handle, Is.EqualTo (item.Handle), "Item");
		}

		[Test]
		public void NSOutlineViewItemEventArgs_NullUserInfo ()
		{
			using var notification = NSNotification.FromName ("test", null);
			var args = new NSOutlineViewItemEventArgs (notification);

			Assert.That (args.Item, Is.Null, "Item");
		}

		[Test]
		public void NSTextViewDidChangeSelectionEventArgs_Properties ()
		{
			using var value = NSValue.FromRange (new NSRange (5, 10));
			using var userInfo = new NSMutableDictionary ();
			userInfo ["NSOldSelectedCharacterRange"] = value;

			using var notification = NSNotification.FromName ("test", null, userInfo);
			var args = new NSTextViewDidChangeSelectionEventArgs (notification);

			Assert.That (args.OldSelectedCharacterRange, Is.Not.Null, "OldSelectedCharacterRange not null");
			Assert.That (args.OldSelectedCharacterRange!.Handle, Is.EqualTo (value.Handle), "OldSelectedCharacterRange");
		}

		[Test]
		public void NSTextViewDidChangeSelectionEventArgs_NullUserInfo ()
		{
			using var notification = NSNotification.FromName ("test", null);
			var args = new NSTextViewDidChangeSelectionEventArgs (notification);

			Assert.That (args.OldSelectedCharacterRange, Is.Null, "OldSelectedCharacterRange");
		}

		[Test]
		public void NSTextViewWillChangeNotifyingTextViewEventArgs_Properties ()
		{
			using var oldView = new NSTextView ();
			using var newView = new NSTextView ();
			using var userInfo = new NSMutableDictionary ();
			userInfo ["NSOldNotifyingTextView"] = oldView;
			userInfo ["NSNewNotifyingTextView"] = newView;

			using var notification = NSNotification.FromName ("test", null, userInfo);
			var args = new NSTextViewWillChangeNotifyingTextViewEventArgs (notification);

			Assert.That (args.OldView, Is.Not.Null, "OldView not null");
			Assert.That (args.OldView!.Handle, Is.EqualTo (oldView.Handle), "OldView");
			Assert.That (args.NewView, Is.Not.Null, "NewView not null");
			Assert.That (args.NewView!.Handle, Is.EqualTo (newView.Handle), "NewView");
		}

		[Test]
		public void NSTextViewWillChangeNotifyingTextViewEventArgs_NullUserInfo ()
		{
			using var notification = NSNotification.FromName ("test", null);
			var args = new NSTextViewWillChangeNotifyingTextViewEventArgs (notification);

			Assert.That (args.OldView, Is.Null, "OldView");
			Assert.That (args.NewView, Is.Null, "NewView");
		}

		[Test]
		public void NSControlTextEditingEventArgs_Properties ()
		{
			using var textView = new NSTextView ();
			using var userInfo = new NSMutableDictionary ();
			userInfo ["NSFieldEditor"] = textView;

			using var notification = NSNotification.FromName ("test", null, userInfo);
			var args = new NSControlTextEditingEventArgs (notification);

			Assert.That (args.FieldEditor, Is.Not.Null, "FieldEditor not null");
			Assert.That (args.FieldEditor!.Handle, Is.EqualTo (textView.Handle), "FieldEditor");
		}

		[Test]
		public void NSControlTextEditingEventArgs_NullUserInfo ()
		{
			using var notification = NSNotification.FromName ("test", null);
			var args = new NSControlTextEditingEventArgs (notification);

			Assert.That (args.FieldEditor, Is.Null, "FieldEditor");
		}

		[Test]
		public void NSTextAlternativesSelectedAlternativeStringEventArgs_Properties ()
		{
			using var userInfo = new NSMutableDictionary ();
			userInfo ["NSAlternativeString"] = new NSString ("hello");

			using var notification = NSNotification.FromName ("test", null, userInfo);
			var args = new NSTextAlternativesSelectedAlternativeStringEventArgs (notification);

			Assert.That (args.AlternativeString, Is.EqualTo ("hello"), "AlternativeString");
		}

		[Test]
		public void NSTextAlternativesSelectedAlternativeStringEventArgs_NullUserInfo ()
		{
			using var notification = NSNotification.FromName ("test", null);
			var args = new NSTextAlternativesSelectedAlternativeStringEventArgs (notification);

			Assert.That (args.AlternativeString, Is.Null, "AlternativeString");
		}

		[Test]
		public void NSMenuItemIndexEventArgs_Properties ()
		{
			using var userInfo = new NSMutableDictionary ();
			userInfo ["NSMenuItemIndex"] = NSNumber.FromNInt (5);

			using var notification = NSNotification.FromName ("test", null, userInfo);
			var args = new NSMenuItemIndexEventArgs (notification);

			Assert.That (args.MenuItemIndex, Is.EqualTo ((nint) 5), "MenuItemIndex");
		}

		[Test]
		public void NSMenuItemIndexEventArgs_NullUserInfo ()
		{
			using var notification = NSNotification.FromName ("test", null);
			var args = new NSMenuItemIndexEventArgs (notification);

			Assert.That (args.MenuItemIndex, Is.EqualTo ((nint) 0), "MenuItemIndex");
		}

		[Test]
		public void NSMenuItemEventArgs_Properties ()
		{
			using var menu = new NSMenu ("testMenu");
			using var userInfo = new NSMutableDictionary ();
			userInfo ["MenuItem"] = menu;

			using var notification = NSNotification.FromName ("test", null, userInfo);
			var args = new NSMenuItemEventArgs (notification);

			Assert.That (args.MenuItem, Is.Not.Null, "MenuItem not null");
			Assert.That (args.MenuItem!.Handle, Is.EqualTo (menu.Handle), "MenuItem");
		}

		[Test]
		public void NSMenuItemEventArgs_NullUserInfo ()
		{
			using var notification = NSNotification.FromName ("test", null);
			var args = new NSMenuItemEventArgs (notification);

			Assert.That (args.MenuItem, Is.Null, "MenuItem");
		}

		[Test]
		public void NSWorkspaceFileOperationEventArgs_Properties ()
		{
			using var userInfo = new NSMutableDictionary ();
			userInfo ["NSOperationNumber"] = NSNumber.FromNInt (99);

			using var notification = NSNotification.FromName ("test", null, userInfo);
			var args = new NSWorkspaceFileOperationEventArgs (notification);

			Assert.That (args.FileType, Is.EqualTo ((nint) 99), "FileType");
		}

		[Test]
		public void NSWorkspaceFileOperationEventArgs_NullUserInfo ()
		{
			using var notification = NSNotification.FromName ("test", null);
			var args = new NSWorkspaceFileOperationEventArgs (notification);

			Assert.That (args.FileType, Is.EqualTo ((nint) 0), "FileType");
		}

		[Test]
		public void NSTextDidEndEditingEventArgs_Properties ()
		{
			using var userInfo = new NSMutableDictionary ();
			userInfo ["NSTextMovement"] = NSNumber.FromNInt (1);

			using var notification = NSNotification.FromName ("test", null, userInfo);
			var args = new NSTextDidEndEditingEventArgs (notification);

			Assert.That (args.Movement, Is.EqualTo ((nint) 1), "Movement");
		}

		[Test]
		public void NSTextDidEndEditingEventArgs_NullUserInfo ()
		{
			using var notification = NSNotification.FromName ("test", null);
			var args = new NSTextDidEndEditingEventArgs (notification);

			Assert.That (args.Movement, Is.EqualTo ((nint) 0), "Movement");
		}
	}
}
#endif // __MACOS__
