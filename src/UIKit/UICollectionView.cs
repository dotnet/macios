//
// UICollectionView.cs: Extensions to the UICollectionView class
//
// Copyright 2012 Xamarin Inc.
//
// Authors:
//   Miguel de Icaza
//

using System;
using ObjCRuntime;
using Foundation;
using System.Threading.Tasks;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace UIKit {
	public partial class UICollectionView {

		/// <param name="reuseIdentifier">To be added.</param>
		///         <param name="indexPath">To be added.</param>
		///         <summary>Returns a new or reused <see cref="T:UIKit.UICollectionReusableView" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public UICollectionReusableView DequeueReusableCell (string reuseIdentifier, NSIndexPath indexPath)
		{
			using (var str = (NSString) reuseIdentifier)
				return (UICollectionReusableView) DequeueReusableCell (str, indexPath);
		}

		/// <param name="kind">To be added.</param>
		///         <param name="reuseIdentifier">To be added.</param>
		///         <param name="indexPath">To be added.</param>
		///         <summary>Returns a <see cref="T:UIKit.UICollectionReusableView" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public UICollectionReusableView DequeueReusableSupplementaryView (NSString kind, string reuseIdentifier, NSIndexPath indexPath)
		{
			using (var str = (NSString) reuseIdentifier)
				return (UICollectionReusableView) DequeueReusableSupplementaryView (kind, str, indexPath);
		}

		/// <param name="kind">To be added.</param>
		///         <param name="reuseIdentifier">To be added.</param>
		///         <param name="indexPath">To be added.</param>
		///         <summary>Returns a <see cref="T:UIKit.UICollectionReusableView" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public UICollectionReusableView DequeueReusableSupplementaryView (UICollectionElementKindSection kind, string reuseIdentifier, NSIndexPath indexPath)
		{
			using (var str = (NSString) reuseIdentifier)
				return (UICollectionReusableView) DequeueReusableSupplementaryView (KindToString (kind), str, indexPath);
		}

		/// <param name="nib">To be added.</param>
		///         <param name="reuseIdentifier">To be added.</param>
		///         <summary>Registers the Nib file that will be used for cell UI.</summary>
		///         <remarks>To be added.</remarks>
		public void RegisterNibForCell (UINib nib, string reuseIdentifier)
		{
			using (var str = (NSString) reuseIdentifier)
				RegisterNibForCell (nib, str);
		}

		/// <include file="../../docs/api/UIKit/UICollectionView.xml" path="/Documentation/Docs[@DocId='M:UIKit.UICollectionView.RegisterClassForCell(System.Type,System.String)']/*" />
		public void RegisterClassForCell (Type cellType, string reuseIdentifier)
		{
			using (var str = (NSString) reuseIdentifier)
				RegisterClassForCell (cellType, str);
		}

		/// <include file="../../docs/api/UIKit/UICollectionView.xml" path="/Documentation/Docs[@DocId='M:UIKit.UICollectionView.RegisterClassForCell(System.Type,Foundation.NSString)']/*" />
		public void RegisterClassForCell (Type cellType, NSString reuseIdentifier)
		{
			if (cellType is null)
				throw new ArgumentNullException ("cellType");

			RegisterClassForCell (Class.GetHandle (cellType), reuseIdentifier);
		}

		/// <param name="cellType">To be added.</param>
		///         <param name="kind">To be added.</param>
		///         <param name="reuseIdentifier">To be added.</param>
		///         <summary>Specifies the type to be used to populate supplementary views.</summary>
		///         <remarks>To be added.</remarks>
		public void RegisterClassForSupplementaryView (Type cellType, NSString kind, string reuseIdentifier)
		{
			using (var str = (NSString) reuseIdentifier)
				RegisterClassForSupplementaryView (Class.GetHandle (cellType), kind, str);
		}

		/// <param name="cellType">A subtype of <see cref="T:UIKit.UICollectionReusableView" /> to be used for supplementary views.</param>
		///         <param name="kind">The type of supplementary view being registered (e.g., "UICollectionElementKindSectionHeader").</param>
		///         <param name="reuseIdentifier">A non-empty string to be associated with the <paramref name="kind" />.</param>
		///         <summary>Specifies the type to be used to populate supplementary views.</summary>
		///         <remarks>To be added.</remarks>
		public void RegisterClassForSupplementaryView (Type cellType, NSString kind, NSString reuseIdentifier)
		{
			RegisterClassForSupplementaryView (Class.GetHandle (cellType), kind, reuseIdentifier);
		}

		/// <param name="cellType">To be added.</param>
		///         <param name="section">To be added.</param>
		///         <param name="reuseIdentifier">To be added.</param>
		///         <summary>Specifies the type to be used to populate supplementary views.</summary>
		///         <remarks>To be added.</remarks>
		public void RegisterClassForSupplementaryView (Type cellType, UICollectionElementKindSection section, string reuseIdentifier)
		{
			using (var str = (NSString) reuseIdentifier)
				RegisterClassForSupplementaryView (cellType, section, str);
		}

		/// <include file="../../docs/api/UIKit/UICollectionView.xml" path="/Documentation/Docs[@DocId='M:UIKit.UICollectionView.RegisterClassForSupplementaryView(System.Type,UIKit.UICollectionElementKindSection,Foundation.NSString)']/*" />
		public void RegisterClassForSupplementaryView (Type cellType, UICollectionElementKindSection section, NSString reuseIdentifier)
		{
			if (cellType is null)
				throw new ArgumentNullException ("cellType");

			RegisterClassForSupplementaryView (Class.GetHandle (cellType), KindToString (section), reuseIdentifier);
		}

		/// <param name="nib">To be added.</param>
		///         <param name="section">To be added.</param>
		///         <param name="reuseIdentifier">To be added.</param>
		///         <summary>Registers the Nib file that will be used for UI in supplementary views.</summary>
		///         <remarks>To be added.</remarks>
		public void RegisterNibForSupplementaryView (UINib nib, UICollectionElementKindSection section, string reuseIdentifier)
		{
			using (var str = (NSString) reuseIdentifier)
				RegisterNibForSupplementaryView (nib, section, str);
		}

		/// <include file="../../docs/api/UIKit/UICollectionView.xml" path="/Documentation/Docs[@DocId='M:UIKit.UICollectionView.RegisterNibForSupplementaryView(UIKit.UINib,UIKit.UICollectionElementKindSection,Foundation.NSString)']/*" />
		public void RegisterNibForSupplementaryView (UINib nib, UICollectionElementKindSection section, NSString reuseIdentifier)
		{
			RegisterNibForSupplementaryView (nib, KindToString (section), reuseIdentifier);
		}

		/// <include file="../../docs/api/UIKit/UICollectionView.xml" path="/Documentation/Docs[@DocId='M:UIKit.UICollectionView.DequeueReusableSupplementaryView(UIKit.UICollectionElementKindSection,Foundation.NSString,Foundation.NSIndexPath)']/*" />
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
				throw new ArgumentOutOfRangeException ("section");
			}
		}

		/// <summary>An optional property that can substitute for the <see cref="P:UIKit.UICollectionView.DataSource" /> and <see cref="P:UIKit.UICollectionView.Delegate" /> properties</summary>
		///         <value>The default value is <see langword="null" />.</value>
		///         <remarks>
		///           <para>Rather than specify separate classes and provide two objects for the  <see cref="P:UIKit.UICollectionView.DataSource" /> and <see cref="P:UIKit.UICollectionView.Delegate" /> properties, one can provide a single class of type <see cref="T:UIKit.UICollectionViewSource" /> (which itself is simply defined as ).</para>
		///         </remarks>
		public UICollectionViewSource Source {
			get {
				var d = WeakDelegate as UICollectionViewSource;
				if (d is not null)
					return d;
				return null;
			}

			set {
				WeakDelegate = value;
				WeakDataSource = value;
			}
		}
	}
}
