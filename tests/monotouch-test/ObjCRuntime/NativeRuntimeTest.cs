using System;
using System.Runtime.InteropServices;
using System.Text;

using Foundation;
using ObjCRuntime;

using NUnit.Framework;

namespace MonoTouchFixtures.ObjCRuntime {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NativeRuntimeTest {
		// bool xamarin_collapse_struct_name (const char *type, char struct_name[], int max_char, GCHandle *exception_gchandle)
		[DllImport ("__Internal")]
		unsafe static extern byte xamarin_collapse_struct_name (string type, StringBuilder struct_name, int max_char, ref IntPtr exception_gchandle);

		[Test]
		public void XamarinCollapseStructName ()
		{
			AssertCollapsed ("{MKCoordinateRegion={CLLocationCoordinate2D=dd}{MKCoordinateSpan=dd}}", "dddd", "1");
			AssertCollapsed ("{CGRect=dddd}", "dddd", "2");
			AssertCollapsed ("^q", "^", "3");
			AssertCollapsed ("@?", "@", "4");
			AssertCollapsed ("^{CGRect={CGPoint=dd}{CGSize=dd}}", "^", "5");
		}

		void AssertCollapsed (string input, string expected, string message)
		{
			var sb = new StringBuilder (255);
			var exception_gchandle = IntPtr.Zero;
			var rv = xamarin_collapse_struct_name (input, sb, sb.Length - 1, ref exception_gchandle);
			Assert.That (rv, Is.Not.EqualTo (0), $"rv/{message}");
			Assert.That (exception_gchandle, Is.EqualTo (IntPtr.Zero), $"exc/{message}");
			Assert.That (sb.ToString (), Is.EqualTo (expected), $"actual/{message}");
		}
	}
}
