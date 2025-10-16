#if MONOMAC
using MediaToolbox;
using Xamarin.Utils;

namespace MonoTouchFixtures.MediaToolbox {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ProfessionalVideoWorkflowTest {

		[Test]
		public void RegisterFormatReaders ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 10, throwIfOtherPlatform: false);
			MTProfessionalVideoWorkflow.RegisterFormatReaders ();
		}
	}
}

#endif
