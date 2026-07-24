#if HAS_APPMANAGEDFEATURES
#nullable enable

using AppManagedFeatures;

namespace MonoTouchFixtures.AppManagedFeatures {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AppManagedFeaturesTest {

		[Test]
		public void Version ()
		{
			TestRuntime.AssertDevice ();
			TestRuntime.AssertXcodeVersion (27, 0);

			Assert.That (AppManagedFeaturesConstants.AppManagedFeaturesVersionNumber, Is.GreaterThan (0), "VersionNumber");
			Assert.That (AppManagedFeaturesConstants.AppManagedFeaturesVersionString, Is.Not.Empty, "VersionString");
		}
	}
}
#endif
