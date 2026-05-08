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
				Assert.That (av.Annotation, Is.Null, "Annotation");
			}
		}

		[Test]
		public void InitWithAnnotation ()
		{
			// using a null 'annotation' crash - but the property can be set to null later
			using (var a = new MKPolygon ())
			using (MKAnnotationView av = new MKAnnotationView (a, "reuse")) {
				Assert.That (av.Annotation, Is.SameAs (a), "Annotation");
				av.Annotation = null;
			}
		}

		[Test]
		public void Default ()
		{
			using (var def = new MKAnnotationView ()) {
				Assert.That (def.Annotation, Is.Null, "Annotation");
				Assert.That (CGPoint.Empty, Is.EqualTo (def.CalloutOffset), "CalloutOffset");
				Assert.That (def.CanShowCallout, Is.False, "CanShowCallout");
				Assert.That (CGPoint.Empty, Is.EqualTo (def.CenterOffset), "CenterOffset");
				Assert.That (def.Draggable, Is.False, "Draggable");
				Assert.That (def.DragState, Is.EqualTo (MKAnnotationViewDragState.None), "DragState");
				Assert.That (def.Enabled, Is.True, "Enabled");
				Assert.That (def.Highlighted, Is.False, "Highlighted");
				Assert.That (def.Image, Is.Null, "Image");
				Assert.That (def.LeftCalloutAccessoryView, Is.Null, "LeftCalloutAccessoryView");
				Assert.That (def.ReuseIdentifier, Is.Null, "ReuseIdentifier");
				Assert.That (def.RightCalloutAccessoryView, Is.Null, "RightCalloutAccessoryView");
				Assert.That (def.Selected, Is.False, "Selected");
			}
		}

		[Test]
		public void Null ()
		{
			using (var def = new MKAnnotationView ()) {
				def.Annotation = null;
				def.Annotation = new MKPolygon ();
				Assert.That (def.Annotation, Is.Not.Null, "Annotation NN");
				def.Annotation = null;
				Assert.That (def.Annotation, Is.Null, "Annotation N");

				def.Image = null;
				def.Image = new PlatformImage ();
				Assert.That (def.Image, Is.Not.Null, "Image NN");
				def.Image = null;
				Assert.That (def.Image, Is.Null, "Image N");

				def.LeftCalloutAccessoryView = null;
				def.LeftCalloutAccessoryView = new PlatformView ();
				Assert.That (def.LeftCalloutAccessoryView, Is.Not.Null, "LeftCalloutAccessoryView NN");
				def.LeftCalloutAccessoryView = null;
				Assert.That (def.LeftCalloutAccessoryView, Is.Null, "LeftCalloutAccessoryView N");

				def.RightCalloutAccessoryView = null;
				def.RightCalloutAccessoryView = new PlatformView ();
				Assert.That (def.RightCalloutAccessoryView, Is.Not.Null, "RightCalloutAccessoryView NN");
				def.RightCalloutAccessoryView = null;
				Assert.That (def.RightCalloutAccessoryView, Is.Null, "RightCalloutAccessoryView N");
			}
		}
	}
}

#endif // !__TVOS__
