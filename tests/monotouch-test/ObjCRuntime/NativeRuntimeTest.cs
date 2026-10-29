using System.Text;

namespace MonoTouchFixtures.ObjCRuntime {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NativeRuntimeTest {
		[DllImport ("__Internal")]
		static extern int x_test_format_diagnostic (string text, StringBuilder buffer, nuint capacity);

		[Test]
		public void FormatDiagnostic ()
		{
			var buffer = new StringBuilder (256);
			const string text = "value-%n-\u00e6";
			var expected = text + ": 42\n";
			Assert.That (x_test_format_diagnostic (text, buffer, (nuint) buffer.Capacity), Is.EqualTo (Encoding.UTF8.GetByteCount (expected)), "Byte count");
			Assert.That (buffer.ToString (), Is.EqualTo (expected), "Literal data");
		}

		[DllImport ("__Internal")]
		static extern byte xamarin_copy_path (IntPtr candidate, byte [] buffer, nuint capacity);

		[TestCase ("bundle/file.dll")]
		[TestCase ("../../Frameworks/Test.framework/\u00e6.dll")]
		public void CopyPath (string path)
		{
			using var candidate = new NSString (path);
			var encoded = Encoding.UTF8.GetBytes (path);
			var buffer = new byte [encoded.Length + 1];
			Assert.That (xamarin_copy_path (candidate.Handle, buffer, (nuint) buffer.Length), Is.Not.EqualTo (0), "Exact fit");
			Assert.That (Encoding.UTF8.GetString (buffer, 0, encoded.Length), Is.EqualTo (path), "Path");
			Assert.That (buffer [encoded.Length], Is.EqualTo (0), "Terminator");

			buffer [0] = 123;
			Assert.That (xamarin_copy_path (candidate.Handle, buffer, (nuint) encoded.Length), Is.EqualTo (0), "Too small");
			Assert.That (buffer [0], Is.EqualTo (0), "No truncated path");
			buffer [0] = 123;
			Assert.That (xamarin_copy_path (candidate.Handle, buffer, 0), Is.EqualTo (0), "Zero capacity");
			Assert.That (buffer [0], Is.EqualTo (123), "Zero capacity must not write");
		}

		[DllImport ("__Internal")]
		static extern IntPtr xamarin_copy_environment_variable (string name);

		[DllImport ("__Internal")]
		static extern void xamarin_free (IntPtr pointer);

		[DllImport (Constants.libcLibrary)]
		static extern int setenv (string name, string value, int overwrite);

		[DllImport (Constants.libcLibrary)]
		static extern int unsetenv (string name);

		[Test]
		public void CopyEnvironmentVariable ()
		{
			var name = "MACIOS_NATIVE_RUNTIME_TEST_" + Guid.NewGuid ().ToString ("N");
			IntPtr first = IntPtr.Zero;
			IntPtr second = IntPtr.Zero;
			try {
				Assert.That (xamarin_copy_environment_variable (name), Is.EqualTo (IntPtr.Zero), "Absent");
				Assert.That (setenv (name, "", 1), Is.EqualTo (0), "Set empty");
				first = xamarin_copy_environment_variable (name);
				Assert.That (first, Is.Not.EqualTo (IntPtr.Zero), "Present empty");
				Assert.That (Marshal.PtrToStringUTF8 (first), Is.EqualTo (""), "Empty value");

				Assert.That (setenv (name, "updated-\u00e6", 1), Is.EqualTo (0), "Update");
				second = xamarin_copy_environment_variable (name);
				Assert.That (Marshal.PtrToStringUTF8 (second), Is.EqualTo ("updated-\u00e6"), "Updated UTF-8 value");
				Assert.That (Marshal.PtrToStringUTF8 (first), Is.EqualTo (""), "Independent snapshot");

				Assert.That (unsetenv (name), Is.EqualTo (0), "Unset");
				Assert.That (xamarin_copy_environment_variable (name), Is.EqualTo (IntPtr.Zero), "Removed");
				Assert.That (Marshal.PtrToStringUTF8 (second), Is.EqualTo ("updated-\u00e6"), "Snapshot survives removal");
			} finally {
				unsetenv (name);
				xamarin_free (first);
				xamarin_free (second);
			}
		}

		[DllImport ("__Internal")]
		static extern int x_test_error_description (int error, StringBuilder buffer, nuint size);

		[TestCase (2)]
		[TestCase (int.MinValue)]
		[TestCase (int.MaxValue)]
		public void ErrorDescription (int error)
		{
			var buffer = new StringBuilder (256);
			Assert.That (x_test_error_description (error, buffer, (nuint) buffer.Capacity), Is.EqualTo (16), "Preserved errno");
			Assert.That (buffer.ToString (), Is.Not.Empty, "Description");
			if (error != 2)
				Assert.That (buffer.ToString (), Does.Contain (error.ToString (System.Globalization.CultureInfo.InvariantCulture)), "Numeric fallback");
		}

		[DllImport ("__Internal")]
		static extern nuint xamarin_objc_type_size (string type);

		[TestCase ("[0i]", 0)]
		[TestCase ("[1i]", 1)]
		[TestCase ("[123i]", 123)]
		[TestCase ("[2[3i]]", 6)]
		public void ArrayTypeSize (string encoding, int pointerCount)
		{
			Assert.That (xamarin_objc_type_size (encoding), Is.EqualTo ((nuint) (pointerCount * IntPtr.Size)), encoding);
		}

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

		[DllImport ("__Internal")]
		static extern int x_get_nsobject_data_with_association_lock (IntPtr handle, out IntPtr data);

		[DllImport ("__Internal")]
		static extern IntPtr xamarin_get_nsobject_data_trampoline (IntPtr handle, IntPtr selector);

		[DllImport ("__Internal")]
		static extern IntPtr xamarin_get_gchandle_with_flags (IntPtr handle, out uint flags);

		[DllImport ("__Internal")]
		static extern byte xamarin_set_gchandle_trampoline (IntPtr handle, IntPtr selector, IntPtr gchandle, uint flags, IntPtr data);

		[Test]
		public void NSObjectDataWithoutAssociationLock ()
		{
			if (!Runtime.DynamicRegistrationSupported || global::XamarinTests.ObjCRuntime.Registrar.IsStaticRegistrar)
				Assert.Ignore ("This test requires the dynamic registrar.");

			using var obj = new DynamicNSObjectData ();
			var expected = xamarin_get_nsobject_data_trampoline (obj.Handle, IntPtr.Zero);
			Assert.That (expected, Is.Not.EqualTo (IntPtr.Zero), "Initial data");

			var result = x_get_nsobject_data_with_association_lock (obj.Handle, out var actual);
			Assert.That (result, Is.EqualTo (0), "Data lookup must complete while another thread holds the association lock (1: lock not acquired, 2: lookup timed out)");
			Assert.That (actual, Is.EqualTo (expected), "Data");
		}

		[Test]
		public void NSObjectDataGCHandleUpdates ()
		{
			if (!Runtime.DynamicRegistrationSupported || global::XamarinTests.ObjCRuntime.Registrar.IsStaticRegistrar)
				Assert.Ignore ("This test requires the dynamic registrar.");

			var obj = new DynamicNSObjectData ();
			var handle = obj.Handle;
			var expected = xamarin_get_nsobject_data_trampoline (handle, IntPtr.Zero);
			Assert.That (expected, Is.Not.EqualTo (IntPtr.Zero), "Initial data");
			var gchandle = xamarin_get_gchandle_with_flags (handle, out var flags);
			Assert.That (gchandle, Is.Not.EqualTo (IntPtr.Zero), "GCHandle");

			try {
				Assert.That (xamarin_set_gchandle_trampoline (handle, IntPtr.Zero, gchandle, flags, IntPtr.Zero), Is.Not.EqualTo (0), "Update GCHandle");
				Assert.That (xamarin_get_nsobject_data_trampoline (handle, IntPtr.Zero), Is.EqualTo (expected), "Null data must preserve existing data");

				Assert.That (xamarin_set_gchandle_trampoline (handle, IntPtr.Zero, IntPtr.Zero, 0, IntPtr.Zero), Is.Not.EqualTo (0), "Clear GCHandle");
				Assert.That (xamarin_get_gchandle_with_flags (handle, out var clearedFlags), Is.EqualTo (IntPtr.Zero), "Cleared GCHandle");
				Assert.That (clearedFlags, Is.EqualTo (0), "Cleared flags");
				Assert.That (xamarin_get_nsobject_data_trampoline (handle, IntPtr.Zero), Is.EqualTo (expected), "Clearing a GCHandle must preserve data");
			} finally {
				xamarin_set_gchandle_trampoline (handle, IntPtr.Zero, gchandle, flags, expected);
				obj.Dispose ();
			}

			Assert.That (xamarin_get_nsobject_data_trampoline (handle, IntPtr.Zero), Is.EqualTo (IntPtr.Zero), "Data must be removed before the native pointer can be reused");
		}

		[Register ("NativeRuntimeTest_NSObjectData")]
		class DynamicNSObjectData : NSObject {
		}
	}
}
