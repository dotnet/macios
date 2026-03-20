//
// Unit tests for UIDragDropSessionExtensionsTest
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

namespace MonoTouchFixtures.UIKit {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class UIDragDropSessionExtensionsTest {

		[Test]
		public void LoadObjectsTest ()
		{
			if (!TestRuntime.CheckXcodeVersion (9, 0))
				Assert.Ignore ("Ignoring tests: Requires iOS11+");

			var test = new DropSession ();
			Assert.That (test.CalledCanLoadObjects, Is.False, "CalledCanLoadObjects #1");
			test.CanLoadObjects (typeof (UIImage));
			Assert.That (test.CalledCanLoadObjects, Is.True, "CalledCanLoadObjects #2");

			Assert.That (test.CalledLoadObjects, Is.False, "CalledLoadObjects #1");
			test.LoadObjects<UIImage> (null);
			Assert.That (test.CalledLoadObjects, Is.True, "CalledLoadObjects #2");

			test.CalledLoadObjects = false;
			Assert.That (test.CalledLoadObjects, Is.False, "CalledLoadObjects #1");
			var calledCallback = false;
			test.LoadObjects<UIImage> ((arr) => {
				calledCallback = true;
			});
			Assert.That (calledCallback, Is.True, "calledCallback");
			Assert.That (test.CalledLoadObjects, Is.True, "CalledLoadObjects #2");

		}
	}

	class DropSession : NSObject, IUIDropSession {
		public bool CalledLoadObjects;
		public bool CalledCanLoadObjects;

		public IUIDragSession LocalDragSession => throw new NotImplementedException ();

		public UIDropSessionProgressIndicatorStyle ProgressIndicatorStyle { get => throw new NotImplementedException (); set => throw new NotImplementedException (); }

		public UIDragItem [] Items => throw new NotImplementedException ();

		public bool AllowsMoveOperation => throw new NotImplementedException ();

		public bool RestrictedToDraggingApplication => throw new NotImplementedException ();

		public bool CanLoadObjects (Class itemProviderReadingClass)
		{
			CalledCanLoadObjects = true;
			Assert.That (itemProviderReadingClass.Handle, Is.EqualTo (new Class (typeof (UIImage)).Handle), "UIDragDropSessionExtensionsTest did not convert the type properly for 'CanLoadObjects'.");
			return true;
		}

		public bool HasConformingItems (string [] typeIdentifiers)
		{
			throw new NotImplementedException ();
		}

		public NSProgress LoadObjects (Class itemProviderReadingClass, Action<INSItemProviderReading []> completion)
		{
			CalledLoadObjects = true;
			Assert.That (itemProviderReadingClass.Handle, Is.EqualTo (new Class (typeof (UIImage)).Handle), "UIDragDropSessionExtensionsTest did not convert the type properly for 'LoadObjects'.");
			completion (Array.Empty<INSItemProviderReading> ());
			return new NSProgress ();
		}

		public CGPoint LocationInView (UIView view)
		{
			throw new NotImplementedException ();
		}

		public NSProgress Progress { get { throw new NotImplementedException (); } }
	}
}

#endif // !__TVOS__ && !MONOMAC
