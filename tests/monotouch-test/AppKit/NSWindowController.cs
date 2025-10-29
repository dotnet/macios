#if __MACOS__
using System.Reflection;

using AppKit;

namespace Xamarin.Mac.Tests {
	[Preserve (AllMembers = true)]
	public class NSWindowControllerTests {
		[Test]
		public void NSWindowController_ShowWindowTest ()
		{
			NSWindowController c = new NSWindowController ();
			c.ShowWindow (null);
		}
	}
}
#endif // __MACOS__
