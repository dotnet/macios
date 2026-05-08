// Copyright 2011-2012 Xamarin Inc. All rights reserved

#if !__TVOS__

using System.Drawing;
using System.Reflection;
using MapKit;
using CoreGraphics;
#if MONOMAC
using PlatformImage = AppKit.NSImage;
using PlatformView = AppKit.NSView;
#else
using UIKit;
using PlatformImage = UIKit.UIImage;
using PlatformView = UIKit.UIView;
#endif
using Xamarin.Utils;

namespace MonoTouchFixtures.MapKit {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AnnotationViewTest {

		[SetUp]
		public void Setup ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 9, throwIfOtherPlatform: false);
		}

		[Test]
		public void InitWithFrame ()
		{
			var frame = new CGRect (10, 10, 100, 100);
			using (MKAnnotationView av = new MKAnnotationView (frame)) {
				Assert.That (av.Frame, Is.EqualTo (frame), "Frame");
				ClassicAssert.Null (av.Annotation, "Annotation");
			}
		}

		[Test]
		public void InitWithAnnotation ()
		{
			// using a null 'annotation' crash - but the property can be set to null later
			using (var a = new MKPolygon ())
			using (MKAnnotationView av = new MKAnnotationView (a, "reuse")) {
				ClassicAssert.AreSame (a, av.Annotation, "Annotation");
				av.Annotation = null;
			}
		}

		[Test]
		public void Default ()
		{
			using (var def = new MKAnnotationView ()) {
				ClassicAssert.IsNull (def.Annotation, "Annotation");
				ClassicAssert.AreEqual (def.CalloutOffset, CGPoint.Empty, "CalloutOffset");
				ClassicAssert.IsFalse (def.CanShowCallout, "CanShowCallout");
				ClassicAssert.AreEqual (def.CenterOffset, CGPoint.Empty, "CenterOffset");
				ClassicAssert.IsFalse (def.Draggable, "Draggable");
				Assert.That (def.DragState, Is.EqualTo (MKAnnotationViewDragState.None), "DragState");
				ClassicAssert.IsTrue (def.Enabled, "Enabled");
				ClassicAssert.IsFalse (def.Highlighted, "Highlighted");
				ClassicAssert.IsNull (def.Image, "Image");
				ClassicAssert.IsNull (def.LeftCalloutAccessoryView, "LeftCalloutAccessoryView");
				ClassicAssert.IsNull (def.ReuseIdentifier, "ReuseIdentifier");
				ClassicAssert.IsNull (def.RightCalloutAccessoryView, "RightCalloutAccessoryView");
				ClassicAssert.IsFalse (def.Selected, "Selected");
			}
		}

		[Test]
		public void Null ()
		{
			using (var def = new MKAnnotationView ()) {
				def.Annotation = null;
				def.Annotation = new MKPolygon ();
				ClassicAssert.IsNotNull (def.Annotation, "Annotation NN");
				def.Annotation = null;
				ClassicAssert.IsNull (def.Annotation, "Annotation N");

				def.Image = null;
				def.Image = new PlatformImage ();
				ClassicAssert.IsNotNull (def.Image, "Image NN");
				def.Image = null;
				ClassicAssert.IsNull (def.Image, "Image N");

				def.LeftCalloutAccessoryView = null;
				def.LeftCalloutAccessoryView = new PlatformView ();
				ClassicAssert.IsNotNull (def.LeftCalloutAccessoryView, "LeftCalloutAccessoryView NN");
				def.LeftCalloutAccessoryView = null;
				ClassicAssert.IsNull (def.LeftCalloutAccessoryView, "LeftCalloutAccessoryView N");

				def.RightCalloutAccessoryView = null;
				def.RightCalloutAccessoryView = new PlatformView ();
				ClassicAssert.IsNotNull (def.RightCalloutAccessoryView, "RightCalloutAccessoryView NN");
				def.RightCalloutAccessoryView = null;
				ClassicAssert.IsNull (def.RightCalloutAccessoryView, "RightCalloutAccessoryView N");
			}
		}
	}
}

#endif // !__TVOS__
