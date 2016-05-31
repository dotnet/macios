using System;
using System.Threading.Tasks;
using NUnit.Framework;

#if !XAMCORE_2_0
using MonoMac.AppKit;
using MonoMac.CoreAnimation;
using MonoMac.CoreGraphics;
using MonoMac.Foundation;
using CGRect = System.Drawing.RectangleF;
#else
using AppKit;
using CoreAnimation;
using CoreGraphics;
using Foundation;
#endif

namespace Xamarin.Mac.Tests
{
	[TestFixture]
	public class CABasicAnimationTests
	{
		[Test]
		public void CABasicAnimation_FromToBy_INativeTests ()
		{
			CABasicAnimation test = CABasicAnimation.FromKeyPath ("bounds");
			NSNumber number = new NSNumber (10);
			test.From = number;
			Assert.AreEqual (test.From, number, "NSObject from");
			test.To = number;
			Assert.AreEqual (test.To, number, "NSObject to");
			test.By = number;
			Assert.AreEqual (test.By, number, "NSObject by");

			CGColor color = new CGColor (.5f, .5f, .5f);
			test = CABasicAnimation.FromKeyPath ("color");
			test.SetFrom (color);
			Assert.AreEqual (test.GetFromAs<CGColor> (), color, "INativeObject from");
			test.SetTo (color);
			Assert.AreEqual (test.GetToAs<CGColor> (), color, "INativeObject to");
			test.SetBy (color);
			Assert.AreEqual (test.GetByAs<CGColor> (), color, "INativeObject by");
		}
	}
}