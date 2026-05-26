#if __MACOS__
using System.Threading.Tasks;

using SceneKit;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SCNSceneTests {
		[Test]
		public void SCNSceneLoadingOptions_AnimationImportPolicyTest ()
		{
			SCNSceneLoadingOptions o = new SCNSceneLoadingOptions ();
			RoundTrip (o, SCNAnimationImportPolicy.Play);
			RoundTrip (o, SCNAnimationImportPolicy.PlayRepeatedly);
			RoundTrip (o, SCNAnimationImportPolicy.DoNotPlay);
			RoundTrip (o, SCNAnimationImportPolicy.PlayUsingSceneTimeBase);
		}

		[Test]
		public void SCNSceneLoadingOptions_AnimationImportPolicyTestKeysNonNull ()
		{
			Assert.That (SCNSceneSourceLoading.AnimationImportPolicyPlay, Is.Not.Null);
			Assert.That (SCNSceneSourceLoading.AnimationImportPolicyPlayRepeatedly, Is.Not.Null);
			Assert.That (SCNSceneSourceLoading.AnimationImportPolicyDoNotPlay, Is.Not.Null);
			Assert.That (SCNSceneSourceLoading.AnimationImportPolicyPlayUsingSceneTimeBase, Is.Not.Null);
		}

		void RoundTrip (SCNSceneLoadingOptions o, SCNAnimationImportPolicy policy)
		{
			o.AnimationImportPolicy = policy;
			Assert.That (o.AnimationImportPolicy, Is.EqualTo (policy));
		}
	}
}
#endif // __MACOS__
