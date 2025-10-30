//
// Unit tests for CGFunction
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

#if !MONOMAC
using CoreGraphics;
using UIKit;

namespace MonoTouchFixtures.CoreGraphics {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class FunctionTest {
		[Test]
		public void Test ()
		{
			bool tested = false;
			using (var vc = new UIViewController ()) {
				vc.View = new CustomView () {
					BackgroundColor = UIColor.Green,
					Shaded = () => tested = true,
				};
				AppDelegate.PresentModalViewController (vc, 0.1);
			}

			if (!tested)
				Assert.Inconclusive ("The Shading callback wasn't called.");
		}

		class CustomView : UIView {
			public Action Shaded;
			public unsafe override void Draw (CGRect rect)
			{
				var start = new CGPoint (rect.Left, rect.Bottom);
				var end = new CGPoint (rect.Left, rect.Top);

				using (var context = UIGraphics.GetCurrentContext ())
				using (var rgb = CGColorSpace.CreateDeviceGray ())
				using (var shadingFunction = CreateSlopedFunction (Shaded))
				using (var shading = CGShading.CreateAxial (rgb, start, end, shadingFunction, true, false)) {
					context.DrawShading (shading);
				}

				base.Draw (rect);
			}
		}

		public unsafe static CGFunction CreateSlopedFunction (Action? shadedCallback, nint inputs = 1, nint outputs = 2)
		{
			if (inputs < 1 || outputs < 1)
				throw new ArgumentOutOfRangeException ();

			var domain = new List<nfloat> ();
			for (var i = 0; i < inputs; i++) {
				domain.Add (0);
				domain.Add (1);
			}
			var range = new List<nfloat> ();
			for (var i = 0; i < outputs; i++) {
				range.Add (0);
				range.Add (1);
			}
			return new CGFunction (domain.ToArray (), range.ToArray (), Shading);

			nfloat Slope (nfloat x, nfloat A)
			{
				var p = Math.Pow (x, A);
				return (nfloat) (p / (p + Math.Pow (1.0f - x, A)));
			}

			void Shading (nfloat* data, nfloat* outData)
			{
				outData [0] = 0.0f;
				for (var x = 0; x < inputs; x++) {
					var p = data [x];
					for (var y = 1; y < outputs; y++) {
						outData [y] = (1.0f - Slope (p, 2.0f)) * (1.0f / outputs) * y;
					}
				}
				if (shadedCallback is not null)
					shadedCallback ();
			}
		}

		[Test]
		public void CoreGraphicsStrongDictionary ()
		{
			var rect = new CGRect (10, 10, 100, 100);
			var size = new CGSize (200, 200);
			var point = new CGPoint (10, 20);
			var graphicsDict = new GraphicsDict {
				Rect = rect,
				Size = size,
				Point = point
			};

			var retrievedRect = graphicsDict.Rect;
			Assert.IsTrue (rect == retrievedRect, "CoreGraphicsStrongDictionary CGRect");

			var retrievedSize = graphicsDict.Size;
			Assert.IsTrue (size == retrievedSize, "CoreGraphicsStrongDictionary CGSize");

			var retrievedPoint = graphicsDict.Point;
			Assert.IsTrue (point == retrievedPoint, "CoreGraphicsStrongDictionary CGPoint");
		}

		class GraphicsDict : DictionaryContainer {
			static NSString RectKey = new NSString ("RectKey");
			public CGRect? Rect {
				get { return GetCGRectValue (RectKey); }
				set { SetCGRectValue (RectKey, value); }
			}

			static NSString SizeKey = new NSString ("SizeKey");
			public CGSize? Size {
				get { return GetCGSizeValue (SizeKey); }
				set { SetCGSizeValue (SizeKey, value); }
			}

			static NSString PointKey = new NSString ("PointKey");
			public CGPoint? Point {
				get { return GetCGPointValue (PointKey); }
				set { SetCGPointValue (PointKey, value); }
			}
		}
	}
}
#endif
