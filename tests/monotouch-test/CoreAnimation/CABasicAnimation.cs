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
			Assert.That (number, Is.EqualTo (test.From), "NSObject from");
			test.To = number;
			Assert.That (number, Is.EqualTo (test.To), "NSObject to");
			test.By = number;
			Assert.That (number, Is.EqualTo (test.By), "NSObject by");

			CGColor color = new CGColor (.5f, .5f, .5f);
			test = CABasicAnimation.FromKeyPath ("color");
			test.SetFrom (color);
			Assert.That (color, Is.EqualTo (test.GetFromAs<CGColor> ()), "INativeObject from");
			test.SetTo (color);
			Assert.That (color, Is.EqualTo (test.GetToAs<CGColor> ()), "INativeObject to");
			test.SetBy (color);
			Assert.That (color, Is.EqualTo (test.GetByAs<CGColor> ()), "INativeObject by");
		}
	}
}
#endif // __MACOS__
