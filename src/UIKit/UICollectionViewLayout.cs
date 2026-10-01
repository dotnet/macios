//
// UICollectionViewLayout.cs: extensions for the binding
//
// Authors:
//   Miguel de Icaza
//
// Copyrigh 2012, Xamarin Inc.
//

namespace UIKit {

	public partial class UICollectionViewLayout {

		/// <summary>Registers a class to use when creating decoration views of the specified kind.</summary>
		/// <param name="viewType">The class to use for the decoration view, or <see langword="null" /> to unregister the current class.</param>
		/// <param name="kind">The element kind for which to use the registered class.</param>
		public void RegisterClassForDecorationView (Type viewType, NSString kind)
		{
			RegisterClassForDecorationView (Class.GetHandle (viewType), kind);
		}

		/// <summary>Gets the layout attributes for the supplementary view at the specified index path.</summary>
		/// <param name="section">The section kind of the supplementary view.</param>
		/// <param name="indexPath">The index path of the supplementary view.</param>
		/// <returns>The layout attributes for the supplementary view.</returns>
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
