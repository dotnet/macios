using System.Runtime.InteropServices;

namespace BindingWithEmbeddedFramework {
	public static class CFunctions {
		[DllImport ("XTest.framework/XTest")]
		public static extern int theUltimateAnswer ();
	}
}
