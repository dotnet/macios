#if __MACOS__
using System.Threading;

using AppKit;
using CoreGraphics;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSObjectTests {
		[Test]
		public void NSObjectTests_InvokeTest ()
		{
			bool hit = false;
			NSApplication.SharedApplication.Invoke (() => hit = true, 1);
			TestRuntime.RunAsync (TimeSpan.FromSeconds (10), () => { }, () => hit);
			Assert.IsTrue (hit, "Did not see events after 10 seconds");
		}
	}
}
#endif // __MACOS__
