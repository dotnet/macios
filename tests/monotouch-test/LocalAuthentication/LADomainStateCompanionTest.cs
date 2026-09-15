#if HAS_LOCALAUTHENTICATION
using System.Collections;

using LocalAuthentication;

namespace MonoTouchFixtures.LocalAuthentication {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class LADomainStateCompanionTest {

		[Test]
		public void AvailableCompanionTypes ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			using var context = new LAContext ();
			Assert.That (context.DomainState, Is.Not.Null, "DomainState");
			Assert.That (context.DomainState.Companion, Is.Not.Null, "DomainState.Companion");
			Assert.That (context.DomainState.Companion.WeakAvailableCompanionTypes, Is.Not.Null, "DomainState.Companion.WeakAvailableCompanionTypes");
			Assert.That (context.DomainState.Companion.AvailableCompanionTypes, Is.EqualTo (LACompanionType.None).Or.EqualTo (LACompanionType.Watch), "DomainState.Companion.AvailableCompanionTypes");
		}
	}
}
#endif // HAS_LOCALAUTHENTICATION
