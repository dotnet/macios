#if __MACOS__
using System.Threading.Tasks;

using AppKit;
using CoreGraphics;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSThreadTests {
		[Test]
		public void NSThread_CallStack_Test ()
		{
			string [] stack = NSThread.NativeCallStack;
			Assert.That (stack, Is.Not.Null);
			Assert.That (stack.Length > 0, Is.True);
		}
	}
}
#endif // __MACOS__
