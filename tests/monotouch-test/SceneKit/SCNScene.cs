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
			ClassicAssert.IsNotNull (SCNSceneSourceLoading.AnimationImportPolicyPlay);
			ClassicAssert.IsNotNull (SCNSceneSourceLoading.AnimationImportPolicyPlayRepeatedly);
			ClassicAssert.IsNotNull (SCNSceneSourceLoading.AnimationImportPolicyDoNotPlay);
			ClassicAssert.IsNotNull (SCNSceneSourceLoading.AnimationImportPolicyPlayUsingSceneTimeBase);
		}

		void RoundTrip (SCNSceneLoadingOptions o, SCNAnimationImportPolicy policy)
		{
			o.AnimationImportPolicy = policy;
			ClassicAssert.IsTrue (o.AnimationImportPolicy == policy);
		}
	}
}
#endif // __MACOS__
