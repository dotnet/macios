//
// UICollectionView.cs: Extensions to the UICollectionView class
//
// Copyright 2012 Xamarin Inc.
//
// Authors:
//   Miguel de Icaza
//

#nullable enable

namespace UIKit {
	public partial class UICollectionView {

		/// <summary>Returns a new or reused <see cref="UICollectionReusableView" />.</summary>
		/// <param name="reuseIdentifier">The reuse identifier for the cell type to dequeue.</param>
		/// <param name="indexPath">The index path specifying the location of the cell.</param>
		/// <returns>A <see cref="UICollectionReusableView" /> that is either newly allocated or recycled from the reuse queue.</returns>
		public UICollectionReusableView DequeueReusableCell (string reuseIdentifier, NSIndexPath indexPath)
		{
			using (var str = (NSString) reuseIdentifier)
				return (UICollectionReusableView) DequeueReusableCell (str, indexPath);
		}

		/// <summary>Returns a <see cref="UICollectionReusableView" /> for a supplementary view.</summary>
		/// <param name="kind">An <see cref="NSString" /> specifying the kind of supplementary view to dequeue.</param>
		/// <param name="reuseIdentifier">The reuse identifier for the supplementary view type to dequeue.</param>
		/// <param name="indexPath">The index path specifying the location of the supplementary view.</param>
		/// <returns>A <see cref="UICollectionReusableView" /> that is either newly allocated or recycled from the reuse queue.</returns>
		public UICollectionReusableView DequeueReusableSupplementaryView (NSString kind, string reuseIdentifier, NSIndexPath indexPath)
		{
			using (var str = (NSString) reuseIdentifier)
				return (UICollectionReusableView) DequeueReusableSupplementaryView (kind, str, indexPath);
		}

		/// <summary>Returns a <see cref="UICollectionReusableView" /> for a supplementary view.</summary>
		/// <param name="kind">A <see cref="UICollectionElementKindSection" /> specifying the kind of supplementary view to dequeue.</param>
		/// <param name="reuseIdentifier">The reuse identifier for the supplementary view type to dequeue.</param>
		/// <param name="indexPath">The index path specifying the location of the supplementary view.</param>
		/// <returns>A <see cref="UICollectionReusableView" /> that is either newly allocated or recycled from the reuse queue.</returns>
		public UICollectionReusableView DequeueReusableSupplementaryView (UICollectionElementKindSection kind, string reuseIdentifier, NSIndexPath indexPath)
		{
			using (var str = (NSString) reuseIdentifier)
				return (UICollectionReusableView) DequeueReusableSupplementaryView (KindToString (kind), str, indexPath);
		}

		/// <summary>Registers the Nib file that will be used for cell UI.</summary>
		/// <param name="nib">The <see cref="UINib" /> to be used to populate the cell.</param>
		/// <param name="reuseIdentifier">A non-empty string to be associated with the <paramref name="nib" />.</param>
		public void RegisterNibForCell (UINib? nib, string reuseIdentifier)
		{
			using (var str = (NSString) reuseIdentifier)
				RegisterNibForCell (nib, str);
		}

		/// <summary>Specifies the type to be used to populate cells.</summary>
		/// <param name="cellType">A subtype of <see cref="UICollectionViewCell" />.</param>
		/// <param name="reuseIdentifier">A non-empty string to be associated with the <paramref name="cellType" />.</param>
		/// <remarks>
		///   <para>The <see cref="UICollectionView" /> maintains a highly-efficient reuse queue for offscreen components. This requires that the <see cref="UICollectionView" /> be responsible for the lifecycle management of its component views. This method (and related methods such as <see cref="RegisterClassForSupplementaryView(Type,UICollectionElementKindSection,string)" />) provide the <see cref="UICollectionView" /> the knowledge of which types it needs to instantiate.</para>
		///   <para>The application developer may pass <see langword="null" /> as the <paramref name="cellType" />, in which case the <paramref name="cellType" /> will be "un-registered" and no longer instantiated. The application developer may pass in a <paramref name="reuseIdentifier" /> previously associated with another type, in which case the old type will be "de-registered" and the new <paramref name="cellType" /> will be used.</para>
		///   <para>It is very important that the type that you specify implements a public constructor that takes a <see cref="CoreGraphics.CGRect" /> parameter, this is used to initialize the class from an object allocated by Objective-C.</para>
		///   <para>Developers should not call this method if they have prototyped their <see cref="UICollectionViewCell" /> type using a Storyboard. If they do so, they will overwrite the Storyboard-defined definition instantiation of the <see cref="UICollectionViewCell" /> object's children.</para>
		/// </remarks>
		public void RegisterClassForCell (Type? cellType, string reuseIdentifier)
		{
			using (var str = (NSString) reuseIdentifier)
				RegisterClassForCell (cellType, str);
		}

		/// <summary>Specifies the type to be used to populate cells.</summary>
		/// <param name="cellType">A subtype of <see cref="UICollectionViewCell" />.</param>
		/// <param name="reuseIdentifier">A non-empty string to be associated with the <paramref name="cellType" />.</param>
		/// <remarks>
		/// <para>The <see cref="UICollectionView" /> maintains a highly-efficient reuse queue for offscreen components. This requires that the <see cref="UICollectionView" /> be responsible for the lifecycle management of its component views. This method (and related methods such as <see cref="RegisterClassForSupplementaryView(Type,UICollectionElementKindSection,string)" />) provide the <see cref="UICollectionView" /> the knowledge of which types it needs to instantiate.</para>
		/// <para>The application developer may pass <see langword="null" /> as the <paramref name="cellType" />, in which case the <paramref name="cellType" /> will be "un-registered" and no longer instantiated. The application developer may pass in a <paramref name="reuseIdentifier" /> previously associated with another type, in which case the old type will be "de-registered" and the new <paramref name="cellType" /> will be used.</para>
		/// <para>It is very important that the type that you specify implements a public constructor that takes a <see cref="CoreGraphics.CGRect" /> parameter, this is used to initialize the class from an object allocated by Objective-C.</para>
		/// <para>Developers should not call this method if they have prototyped their <see cref="UICollectionViewCell" /> type using a Storyboard. If they do so, they will overwrite the Storyboard-defined definition instantiation of the <see cref="UICollectionViewCell" /> object's children.</para>
		/// </remarks>
		public void RegisterClassForCell (Type? cellType, NSString reuseIdentifier)
		{
			RegisterClassForCell (Class.GetHandle (cellType), reuseIdentifier);
		}

		/// <summary>Specifies the type to be used to populate supplementary views.</summary>
		/// <param name="cellType">A subtype of <see cref="UICollectionReusableView" /> to be used for supplementary views.</param>
		/// <param name="kind">The kind of supplementary view being registered (e.g., "UICollectionElementKindSectionHeader").</param>
		/// <param name="reuseIdentifier">A non-empty string to be associated with the <paramref name="cellType" />.</param>
		public void RegisterClassForSupplementaryView (Type? cellType, NSString kind, string reuseIdentifier)
		{
			using (var str = (NSString) reuseIdentifier)
				RegisterClassForSupplementaryView (Class.GetHandle (cellType), kind, str);
		}

		/// <summary>Specifies the type to be used to populate supplementary views.</summary>
		/// <param name="cellType">A subtype of <see cref="UICollectionReusableView" /> to be used for supplementary views.</param>
		/// <param name="kind">The kind of supplementary view being registered (e.g., "UICollectionElementKindSectionHeader").</param>
		/// <param name="reuseIdentifier">A non-empty string to be associated with the <paramref name="cellType" />.</param>
		public void RegisterClassForSupplementaryView (Type? cellType, NSString kind, NSString reuseIdentifier)
		{
			RegisterClassForSupplementaryView (Class.GetHandle (cellType), kind, reuseIdentifier);
		}

		/// <summary>Specifies the type to be used to populate supplementary views.</summary>
		/// <param name="cellType">A subtype of <see cref="UICollectionReusableView" /> to be used for supplementary views.</param>
		/// <param name="section">The kind of supplementary view being registered.</param>
		/// <param name="reuseIdentifier">A non-empty string to be associated with the <paramref name="cellType" />.</param>
		public void RegisterClassForSupplementaryView (Type? cellType, UICollectionElementKindSection section, string reuseIdentifier)
		{
			using (var str = (NSString) reuseIdentifier)
				RegisterClassForSupplementaryView (cellType, section, str);
		}

		/// <summary>Specifies the type to be used to populate supplementary views.</summary>
		/// <param name="cellType">A subtype of <see cref="UICollectionReusableView" /> to be used for supplementary views.</param>
		/// <param name="section">The kind of supplementary view being registered.</param>
		/// <param name="reuseIdentifier">A non-empty string to be associated with the <paramref name="cellType" />.</param>
		/// <remarks>
		///   <para>The <see cref="UICollectionView" /> maintains a highly-efficient reuse queue for offscreen components. This requires that the <see cref="UICollectionView" /> be responsible for the lifecycle management of its component views. This method (and related methods such as <see cref="RegisterClassForCell(Type,string)" />) provide the <see cref="UICollectionView" /> the knowledge of which types it needs to instantiate.</para>
		///   <para>The application developer may pass <see langword="null" /> as the <paramref name="cellType" />, in which case the <paramref name="cellType" /> will be "un-registered" and no longer instantiated. The application developer may pass in a <paramref name="reuseIdentifier" /> previously associated with another type, in which case the old type will be "de-registered" and the new <paramref name="cellType" /> will be used.</para>
		///   <para>It is very important that you provide a constructor that takes an <see cref="IntPtr" /> argument in any subclasses that you register. This is required because the classes are actually allocated by the Objective-C runtime, and you must initialize them.</para>
		/// </remarks>
		public void RegisterClassForSupplementaryView (Type? cellType, UICollectionElementKindSection section, NSString reuseIdentifier)
		{
			RegisterClassForSupplementaryView (Class.GetHandle (cellType), KindToString (section), reuseIdentifier);
		}

		/// <summary>Registers the Nib file that will be used for UI in supplementary views.</summary>
		/// <param name="nib">The <see cref="UINib" /> to be used to populate the supplementary view.</param>
		/// <param name="section">The kind of supplementary view being registered.</param>
		/// <param name="reuseIdentifier">A non-empty string to be associated with the <paramref name="nib" />.</param>
		public void RegisterNibForSupplementaryView (UINib? nib, UICollectionElementKindSection section, string reuseIdentifier)
		{
			using (var str = (NSString) reuseIdentifier)
				RegisterNibForSupplementaryView (nib, section, str);
		}

		/// <summary>Specifies the nib to be used for populating the supplementary view.</summary>
		/// <param name="nib">The <see cref="UINib" /> to be used to populate the supplementary view.</param>
		/// <param name="section">The kind of supplementary view being registered.</param>
		/// <param name="reuseIdentifier">A non-empty string to be associated with the <paramref name="nib" />.</param>
		/// <remarks>
		///   <para>The <see cref="UICollectionView" /> maintains a highly-efficient reuse queue for offscreen components. This requires that the <see cref="UICollectionView" /> be responsible for the lifecycle management of its component views. This method (and related methods such as <see cref="RegisterNibForCell(UINib,string)" />) provide the <see cref="UICollectionView" /> the knowledge of which types it needs to instantiate.</para>
		///   <para>The application developer may pass <see langword="null" /> as the <paramref name="nib" />, in which case the <paramref name="nib" /> will be "un-registered" and no longer instantiated. The application developer may pass in a <paramref name="reuseIdentifier" /> previously associated with another nib, in which case the old nib will be "de-registered" and the new <paramref name="nib" /> will be used.</para>
		/// </remarks>
		public void RegisterNibForSupplementaryView (UINib? nib, UICollectionElementKindSection section, NSString reuseIdentifier)
		{
			RegisterNibForSupplementaryView (nib, KindToString (section), reuseIdentifier);
		}

		/// <summary>Returns a newly-allocated or reused supplementary <see cref="UICollectionReusableView" />.</summary>
		/// <param name="section">A <see cref="UICollectionElementKindSection" /> specifying what kind of supplementary view is desired.</param>
		/// <param name="reuseIdentifier">The reuse identifier for the supplementary view type to dequeue.</param>
		/// <param name="indexPath">The <see cref="Foundation.NSIndexPath" /> specifying the location of the supplementary view.</param>
		/// <returns>A supplementary <see cref="UICollectionReusableView" /> that is either newly allocated or recycled from the reuse queue.</returns>
		/// <remarks>
		///   <para>The application developer must have registered a class or nib file using either <see cref="RegisterClassForSupplementaryView(Type,UICollectionElementKindSection,string)" /> or <see cref="RegisterNibForSupplementaryView(UINib,UICollectionElementKindSection,string)" /> prior to calling this method.</para>
		///   <para>If the <see cref="UICollectionReusableView" /> is not newly allocated but is being recycled, this method will call that cell's <see cref="UICollectionReusableView.PrepareForReuse" /> method.</para>
		/// </remarks>
		public NSObject DequeueReusableSupplementaryView (UICollectionElementKindSection section, NSString reuseIdentifier, NSIndexPath indexPath)
		{
			return DequeueReusableSupplementaryView (KindToString (section), reuseIdentifier, indexPath);
		}

		static NSString KindToString (UICollectionElementKindSection section)
		{
			switch (section) {
			case UICollectionElementKindSection.Header:
				return UICollectionElementKindSectionKey.Header;
			case UICollectionElementKindSection.Footer:
				return UICollectionElementKindSectionKey.Footer;
			default:
				throw new ArgumentOutOfRangeException (nameof (section));
			}
		}

		/// <summary>An optional property that can substitute for the <see cref="DataSource" /> and <see cref="Delegate" /> properties.</summary>
		/// <value>The <see cref="UICollectionViewSource" /> that provides both data source and delegate functionality, or <see langword="null" /> if not set.</value>
		/// <remarks>
		///   <para>Rather than specify separate classes and provide two objects for the <see cref="DataSource" /> and <see cref="Delegate" /> properties, one can provide a single class of type <see cref="UICollectionViewSource" />.</para>
		/// </remarks>
		public UICollectionViewSource? Source {
			get => WeakDelegate as UICollectionViewSource;
			set {
				WeakDelegate = value;
				WeakDataSource = value;
			}
		}
	}
}
