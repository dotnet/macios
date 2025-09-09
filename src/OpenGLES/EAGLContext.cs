using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ObjCRuntime;

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
			EAGLGetVersion ((nuint*) Unsafe.AsPointer<nuint> (ref major), (nuint*) Unsafe.AsPointer<nuint> (ref minor));
		}

		/// <param name="target">To be added.</param>
		/// <param name="presentationTime">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("tvos12.0", "Use 'Metal' instead.")]
		[ObsoletedOSPlatform ("ios12.0", "Use 'Metal' instead.")]
		public virtual bool PresentRenderBuffer (nuint target, double presentationTime)
		{
			return _PresentRenderbufferAtTime (target, presentationTime);
		}

		/// <param name="target">To be added.</param>
		/// <param name="presentationTime">To be added.</param>
		/// <param name="mode">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
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
