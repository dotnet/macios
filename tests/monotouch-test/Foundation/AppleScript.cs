#if __MACOS__

using AppKit;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AppleScriptTests {
		[Test]
		public void AppleScript_BasicTest ()
		{
#pragma warning disable 0219
			const string script = @"tell application ""Finder""
end tell";
			NSAppleScript s = new NSAppleScript (script);

			NSDictionary errorInfo;
			bool success = s.CompileAndReturnError (out errorInfo);
			ClassicAssert.IsTrue (success);
			ClassicAssert.IsNull (errorInfo);
			ClassicAssert.IsTrue (s.Compiled);

			NSAppleEventDescriptor descriptor = s.ExecuteAndReturnError (out errorInfo);
			ClassicAssert.IsNull (errorInfo);
#pragma warning restore 0219
		}
	}
}
#endif // __MACOS__
