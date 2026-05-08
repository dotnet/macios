//
// Unit tests for UISpringLoadedInteractionSupportingTest
//
// Authors:
//	Vincent Dondain <vidondai@microsoft.com>
//
//
// Copyright 2017 Microsoft.
//

#if !__TVOS__ && !MONOMAC

using CoreGraphics;
using UIKit;
#if HAS_INTENTUI
using IntentsUI;
#endif

namespace MonoTouchFixtures.UIKit {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class UISpringLoadedInteractionSupportingTest {

		[SetUp]
		public void Setup ()
		{
			TestRuntime.AssertXcodeVersion (9, 0);
		}

		[Test]
		public void UIAlertControllerSpringLoadTest ()
		{
			var alertController = new UIAlertController ();
			alertController.SpringLoaded = true;
			Assert.That (alertController.SpringLoaded, Is.True);
		}

		[Test]
		public void UIBarButtonItemSpringLoadTest ()
		{
			var barButtonItem = new UIBarButtonItem ();
			barButtonItem.SpringLoaded = true;
			Assert.That (barButtonItem.SpringLoaded, Is.True);
		}

		[Test]
		public void UIButtonSpringLoadTest ()
		{
			var button = new UIButton ();
			button.SpringLoaded = true;
			Assert.That (button.SpringLoaded, Is.True);
		}

		[Test]
		public void UICollectionViewSpringLoadTest ()
		{
			var collectionView = new UICollectionView (new CGRect (0, 0, 100, 100), new UICollectionViewLayout ());
			collectionView.SpringLoaded = true;
			Assert.That (collectionView.SpringLoaded, Is.True);
		}

		[Test]
		public void UISegmentedControlSpringLoadTest ()
		{
			var segmentedControl = new UISegmentedControl ();
			segmentedControl.SpringLoaded = true;
			Assert.That (segmentedControl.SpringLoaded, Is.True);
		}

		[Test]
		public void UITabBarItemSpringLoadTest ()
		{
			var tabBarItem = new UITabBarItem ();
			tabBarItem.SpringLoaded = true;
			Assert.That (tabBarItem.SpringLoaded, Is.True);
		}

		[Test]
		public void UITabBarSpringLoadTest ()
		{
			var tabBar = new UITabBar ();
			tabBar.SpringLoaded = true;
			Assert.That (tabBar.SpringLoaded, Is.True);
		}

		[Test]
		public void UITableViewSpringLoadTest ()
		{
			var tableView = new UITableView ();
			tableView.SpringLoaded = true;
			Assert.That (tableView.SpringLoaded, Is.True);
		}

		[Test]
		public void UISearchTabSpringLoadTest ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);
			using var instance = new UISearchTab (null);
			instance.SpringLoaded = true;
			Assert.That (instance.SpringLoaded, Is.True);
		}

		[Test]
		public void UITabSpringLoadTest ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);
			using var instance = new UITab ("title", null, "identifier", null);
			instance.SpringLoaded = true;
			Assert.That (instance.SpringLoaded, Is.True);
		}

		[Test]
		public void UITabGroupSpringLoadTest ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);
			using var instance = new UITabGroup ("title", null, "identifier", new UITab [0], null);
			instance.SpringLoaded = true;
			Assert.That (instance.SpringLoaded, Is.True);
		}

#if HAS_INTENTUI
		[Test]
		public void INUIAddVoiceShortcutButtonTest ()
		{
			TestRuntime.AssertXcodeVersion (10, 0);
			TestRuntime.AssertDevice ();
			var shortcutButton = new INUIAddVoiceShortcutButton (INUIAddVoiceShortcutButtonStyle.Black);
			shortcutButton.SpringLoaded = true;
			Assert.That (shortcutButton.SpringLoaded, Is.True);
		}
#endif
	}
}

#endif // !__TVOS__
