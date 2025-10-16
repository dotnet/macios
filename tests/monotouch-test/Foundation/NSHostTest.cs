#if __MACOS__
using System.Collections;

#nullable enable

namespace MonoTouchFixtures.Foundation {
	public class NSHostTest {

		public void EqualsNullAllowed ()
		{
			using var host = NSHost.FromAddress ("http://microsoft.com");
			Assert.False (host.Equals (null));
		}
	}
}
#endif
