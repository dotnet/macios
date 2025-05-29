#if __MACOS__
using NUnit.Framework;
using System;

using AppKit;
using ObjCRuntime;
using Foundation;
using Xamarin.Utils;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSSliderTests {
		[Test]
		public void NSSlider_VertialTests ()
		{
			TestRuntime.AssertXcodeVersion (8, 0);

			NSSlider slider = new NSSlider ();
			var isVert = slider.IsVertical;
			slider.IsVertical = true;
			Assert.IsTrue (slider.IsVertical);
		}
	}
}


#endif // __MACOS__
