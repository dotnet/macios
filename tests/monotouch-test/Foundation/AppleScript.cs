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
			Assert.That (success, Is.True);
			Assert.That (errorInfo, Is.Null);
			Assert.That (s.Compiled, Is.True);

			NSAppleEventDescriptor descriptor = s.ExecuteAndReturnError (out errorInfo);
			Assert.That (errorInfo, Is.Null);
#pragma warning restore 0219
		}
	}
}
#endif // __MACOS__
