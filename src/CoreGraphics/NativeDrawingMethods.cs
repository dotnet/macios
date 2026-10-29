using System.Runtime.CompilerServices;

#nullable enable

namespace CoreGraphics {
	static class NativeDrawingMethods {
		internal const string CG = Constants.CoreGraphicsLibrary;
		[DllImport (CG)]
		internal unsafe extern static byte CGRectMakeWithDictionaryRepresentation (IntPtr dict, CGRect* rect);
		[DllImport (CG)]
		internal unsafe extern static byte CGPointMakeWithDictionaryRepresentation (IntPtr dict, CGPoint* point);
		[DllImport (CG)]
		internal unsafe extern static byte CGSizeMakeWithDictionaryRepresentation (IntPtr dict, CGSize* point);

		[DllImport (CG)]
		internal extern static IntPtr CGRectCreateDictionaryRepresentation (CGRect rect);
		[DllImport (CG)]
		internal extern static IntPtr CGSizeCreateDictionaryRepresentation (CGSize size);
		[DllImport (CG)]
		internal extern static IntPtr CGPointCreateDictionaryRepresentation (CGPoint point);
	}
}
