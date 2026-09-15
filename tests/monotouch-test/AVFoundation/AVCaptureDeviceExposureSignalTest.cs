#nullable enable

#if !__MACOS__

using AVFoundation;
using CoreMedia;

namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AVCaptureDeviceExposureSignalTest {
		[TestCase ((AVCaptureDeviceExposureSignal) 0, 0)]
		[TestCase (AVCaptureDeviceExposureSignal.SubjectMotion, 1)]
		[TestCase (AVCaptureDeviceExposureSignal.GroupPhoto, 1)]
		[TestCase (AVCaptureDeviceExposureSignal.Document, 1)]
		[TestCase (AVCaptureDeviceExposureSignal.Starburst, 1)]
		[TestCase (AVCaptureDeviceExposureSignal.Flicker, 1)]
		[TestCase (AVCaptureDeviceExposureSignal.SubjectMotion | AVCaptureDeviceExposureSignal.GroupPhoto | AVCaptureDeviceExposureSignal.Document | AVCaptureDeviceExposureSignal.Starburst | AVCaptureDeviceExposureSignal.Flicker, 5)]
		public void Flags (AVCaptureDeviceExposureSignal signals, int count)
		{
			TestRuntime.AssertXcodeVersion (27, 0);

			var constants = signals.ToArray ();
			Assert.That (constants.Length, Is.EqualTo (count), "Count");
			Assert.That (AVCaptureDeviceExposureSignalExtensions.ToFlags (constants), Is.EqualTo (signals), "Array");

			using var set = new NSSet<NSString> (constants);
			Assert.That (AVCaptureDeviceExposureSignalExtensions.ToFlags (set), Is.EqualTo (signals), "Set");

			foreach (var constant in constants) {
				var signal = AVCaptureDeviceExposureSignalExtensions.GetValue (constant);
				Assert.That (signal.GetConstant (), Is.EqualTo (constant), "Constant");
			}
		}

		[Test]
		public void ExposureSentinels ()
		{
			TestRuntime.AssertXcodeVersion (27, 0);

			var library = Dlfcn.dlopen (Constants.AVFoundationLibrary, 0);
			Assert.That (library, Is.Not.EqualTo (IntPtr.Zero), "Library");
			try {
				Assert.That (AVCaptureDevice.LensApertureCurrent, Is.EqualTo (ReadSymbol<float> (library, "AVCaptureLensApertureCurrent")), "LensApertureCurrent");
				Assert.That (AVCaptureDevice.LensApertureAuto, Is.EqualTo (ReadSymbol<float> (library, "AVCaptureLensApertureAuto")), "LensApertureAuto");
				Assert.That (AVCaptureDevice.ExposureDurationAuto, Is.EqualTo (ReadSymbol<CMTime> (library, "AVCaptureExposureDurationAuto")), "ExposureDurationAuto");
				Assert.That (AVCaptureDevice.ISOAuto, Is.EqualTo (ReadSymbol<float> (library, "AVCaptureISOAuto")), "ISOAuto");
			} finally {
				Dlfcn.dlclose (library);
			}
		}

		static unsafe T ReadSymbol<T> (IntPtr library, string name) where T : unmanaged
		{
			var symbol = Dlfcn.dlsym (library, name);
			Assert.That (symbol, Is.Not.EqualTo (IntPtr.Zero), name);
			return *(T*) symbol;
		}
	}
}

#endif // !__MACOS__
