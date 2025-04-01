//
// UICollectionViewLayout.cs: extensions for the binding
//
// Authors:
//   Miguel de Icaza
//
// Copyrigh 2012, Xamarin Inc.
//

using System;
using Foundation;
using ObjCRuntime;

namespace UIKit {

	public partial class UICollectionViewLayout {

		/// <param name="viewType">The type of the class that will provide the decoration.   Use null to unregister the previous.</param>
		///         <param name="kind">The element kind for which the registered type will be used.</param>
		///         <summary>Registers the class identified by kind as a decoration view.</summary>
		///         <remarks>To be added.</remarks>
		public void RegisterClassForDecorationView (Type viewType, NSString kind)
		{
			RegisterClassForDecorationView (viewType is null ? IntPtr.Zero : Class.GetHandle (viewType), kind);
		}

		/// <param name="section">To be added.</param>
		///         <param name="indexPath">To be added.</param>
		///         <summary>The attributes for the supplementary view at the specified indexPath.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public UICollectionViewLayoutAttributes LayoutAttributesForSupplementaryView (UICollectionElementKindSection section, NSIndexPath indexPath)
		{
			NSString kind;
			switch (section) {
			case UICollectionElementKindSection.Header:
				kind = UICollectionElementKindSectionKey.Header;
				break;
			case UICollectionElementKindSection.Footer:
				kind = UICollectionElementKindSectionKey.Footer;
				break;
			default:
				throw new ArgumentOutOfRangeException ("section");
			}

			return LayoutAttributesForSupplementaryView (kind, indexPath);
		}
	}
}
