using System;
using System.Runtime.InteropServices;

using ObjCRuntime;
using Foundation;

namespace UIKit {
	static public partial class UIContentSizeCategoryExtensions {

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.UIKitLibrary)]
		static extern nint /* NSComparisonResult */ UIContentSizeCategoryCompareToCategory (IntPtr /* NSString */ lhs, IntPtr /* NSString */ rhs);

		/// <param name="category1">To be added.</param>
		///         <param name="category2">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static NSComparisonResult Compare (UIContentSizeCategory category1, UIContentSizeCategory category2)
		{
			var c1 = category1.GetConstant ();
			if (c1 is null)
				throw new ArgumentException ($"Unknown 'UIContentSizeCategory' value", nameof (category1));

			var c2 = category2.GetConstant ();
			if (c2 is null)
				throw new ArgumentException ($"Unknown 'UIContentSizeCategory' value", nameof (category2));

			NSComparisonResult result = (NSComparisonResult) (long) UIContentSizeCategoryCompareToCategory (c1.Handle, c2.Handle);
			GC.KeepAlive (c1);
			GC.KeepAlive (c2);
			return result;
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.UIKitLibrary)]
		static extern byte UIContentSizeCategoryIsAccessibilityCategory (IntPtr /* NSString */ category);

		/// <param name="self">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		static public bool IsAccessibilityCategory (this UIContentSizeCategory self)
		{
			var c1 = self.GetConstant ();
			if (c1 is null)
				throw new ArgumentException ($"Unknown 'UIContentSizeCategory' value");

			bool result = UIContentSizeCategoryIsAccessibilityCategory (c1.Handle) != 0;
			GC.KeepAlive (c1);
			return result;
		}
	}
}
