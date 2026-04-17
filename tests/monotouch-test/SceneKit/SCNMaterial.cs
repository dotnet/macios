#if __MACOS__
using System.Threading.Tasks;

using AppKit;
using SceneKit;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SCNMaterialTests {
		[Test]
		public void SCNMaterial_ShaderModifierTest_Weak ()
		{
			if (IntPtr.Size == 8) // API is 64-bit only
			{
				SCNMaterial m = new SCNMaterial ();
				m.WeakShaderModifiers = new NSDictionary ();
			}
		}

		[Test]
		public void SCNMaterial_ShaderModifierTest ()
		{
			if (IntPtr.Size == 8) // API is 64-bit only
			{
				SCNMaterial m = new SCNMaterial ();
				m.ShaderModifiers = new SCNShaderModifiers ();
			}
		}
	}
}
#endif // __MACOS__
