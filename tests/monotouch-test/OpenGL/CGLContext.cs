#if __MACOS__
using OpenGL;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CGLContextTests {
		[Test]
		public void CurrentContextAllowsNull ()
		{
			Assert.DoesNotThrow (() => {
				CGLContext.CurrentContext = null;
			});
		}
	}
}
#endif // __MACOS__
