using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using CoreFoundation;
using ObjCRuntime;

namespace CoreGraphics {


#if NET
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
#endif
	// the remaining of the struct is defined inside src/NativeTypes/Drawing.tt
	public partial struct CGRect {

#if NET
		public override int GetHashCode ()
		{
			return HashCode.Combine (x, y, width, height);
		}

#if MONOMAC
		// <quote>When building for 64 bit systems, or building 32 bit like 64 bit, NSRect is typedef’d to CGRect.</quote>
		// https://developer.apple.com/documentation/foundation/nsrect?language=objc
		[DllImport (Constants.FoundationLibrary, EntryPoint = "NSStringFromRect")]
		extern static /* NSString* */ IntPtr NSStringFromCGRect (/* NSRect */ CGRect rect);
#else
		[DllImport (Constants.UIKitLibrary)]
		extern static /* NSString* */ IntPtr NSStringFromCGRect (CGRect rect);
#endif // MONOMAC

#if !COREBUILD
		public override string ToString ()
		{
			return CFString.FromHandle (NSStringFromCGRect (this));
		}
#endif

#endif // !NET
	}
}
