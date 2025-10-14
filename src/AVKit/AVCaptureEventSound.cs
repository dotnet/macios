using Foundation;
using ObjCRuntime;

namespace AVKit {
#if __IOS__ && !__MACCATALYST__
	public partial class AVCaptureEventSound {
		/// <summary>Create a new <see cref="AVCaptureEventSound" /> instance for the specified <paramref name="url" />.</summary>
		/// <param name="url">The url for a local file from within the app bundle.</param>
		/// <param name="error">The error in case of failure.</param>
		/// <returns>A new <see cref="AVCaptureEventSound" /> instance if successful, otherwise <paramref name="error" /> will be an error for the failure.</returns>
		public static AVCaptureEventSound? Create (NSUrl url, out NSError? error)
		{
			var rv = new AVCaptureEventSound (NSObjectFlag.Empty);
			rv.InitializeHandle (rv._InitWithUrl (url, out error), "initWithURL:error:", false);
			if (rv.Handle == NativeHandle.Zero) {
				rv.Dispose ();
				return null;
			}
			return rv;
		}
	}
#endif // __IOS__ && !__MACCATALYST__
}
