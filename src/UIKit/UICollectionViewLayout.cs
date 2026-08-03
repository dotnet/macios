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

		/// <param name="viewType">The type of the class that will provide the decoration.   Use null to unregister the previous.</param>
		///         <param name="kind">The element kind for which the registered type will be used.</param>
		///         <summary>Registers the class identified by kind as a decoration view.</summary>
		public void RegisterClassForDecorationView (Type viewType, NSString kind)
		{
			RegisterClassForDecorationView (Class.GetHandle (viewType), kind);
		}

		/// <param name="section">The section kind of the supplementary view.</param>
		///         <param name="indexPath">The index path of the supplementary view.</param>
		///         <summary>The attributes for the supplementary view at the specified indexPath.</summary>
		/// <returns>The layout attributes for the supplementary view, or <see langword="null" /> if no supplementary view exists at the specified index path.</returns>
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
