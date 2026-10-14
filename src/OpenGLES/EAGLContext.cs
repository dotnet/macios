using System.Runtime.CompilerServices;

namespace OpenGLES {
	public partial class EAGLContext {
		/// <summary>To be added.</summary>
		///     <remarks>To be added.</remarks>
		public enum PresentationMode {
			/// <summary>To be added.</summary>
			AtTime = 0,
			/// <summary>To be added.</summary>
			AfterMinimumDuration = 1,
		}

		[DllImport (Constants.OpenGLESLibrary)]
		unsafe extern static void EAGLGetVersion (nuint* major, nuint* minor);

		/// <param name="major">To be added.</param>
		/// <param name="minor">To be added.</param>
		/// <summary>Writes the major and minor version numbers in the provided <see langword="out" /> parameters.</summary>
		/// <remarks>To be added.</remarks>
		public unsafe static void EAGLGetVersion (out nuint major, out nuint minor)
		{
			major = default;
			minor = default;
			fixed (nuint* majorPtr = &major, minorPtr = &minor)
				EAGLGetVersion (majorPtr, minorPtr);
		}

		/// <summary>Presents the drawable or renderbuffer at the specified time.</summary>
		/// <param name="target">The target object.</param>
		/// <param name="presentationTime">The presentation time, in seconds.</param>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("tvos12.0", "Use 'Metal' instead.")]
		[ObsoletedOSPlatform ("ios12.0", "Use 'Metal' instead.")]
		public virtual bool PresentRenderBuffer (nuint target, double presentationTime)
		{
			return _PresentRenderbufferAtTime (target, presentationTime);
		}

		/// <summary>Presents the drawable or renderbuffer at the specified time.</summary>
		/// <param name="target">The target object.</param>
		/// <param name="presentationTime">The presentation time, in seconds.</param>
		/// <param name="mode">The mode.</param>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("tvos12.0", "Use 'Metal' instead.")]
		[ObsoletedOSPlatform ("ios12.0", "Use 'Metal' instead.")]
		public virtual bool PresentRenderBuffer (nuint target, double presentationTime, PresentationMode mode)
		{
			switch (mode) {
			case PresentationMode.AtTime:
				return _PresentRenderbufferAtTime (target, presentationTime);
			case PresentationMode.AfterMinimumDuration:
				return _PresentRenderbufferAfterMinimumDuration (target, presentationTime);
			default:
				throw new ArgumentOutOfRangeException ($"Unknown presentation mode: {mode}", nameof (mode));
			}
		}
	}
}
