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
			Assert.IsNotNull (stack);
			Assert.IsTrue (stack.Length > 0);
		}
	}
}
#endif // __MACOS__
