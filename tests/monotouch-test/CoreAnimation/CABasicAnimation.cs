#if __MACOS__
using System.Threading.Tasks;

using AppKit;
using CoreAnimation;
using CoreGraphics;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CABasicAnimationTests {
		[Test]
		public void CABasicAnimation_FromToBy_INativeTests ()
		{
			CABasicAnimation test = CABasicAnimation.FromKeyPath ("bounds");
			NSNumber number = new NSNumber (10);
			test.From = number;
			ClassicAssert.AreEqual (test.From, number, "NSObject from");
			test.To = number;
			ClassicAssert.AreEqual (test.To, number, "NSObject to");
			test.By = number;
			ClassicAssert.AreEqual (test.By, number, "NSObject by");

			CGColor color = new CGColor (.5f, .5f, .5f);
			test = CABasicAnimation.FromKeyPath ("color");
			test.SetFrom (color);
			ClassicAssert.AreEqual (test.GetFromAs<CGColor> (), color, "INativeObject from");
			test.SetTo (color);
			ClassicAssert.AreEqual (test.GetToAs<CGColor> (), color, "INativeObject to");
			test.SetBy (color);
			ClassicAssert.AreEqual (test.GetByAs<CGColor> (), color, "INativeObject by");
		}
	}
}
#endif // __MACOS__
