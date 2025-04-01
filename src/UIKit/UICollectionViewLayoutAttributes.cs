// 
// UICollectionViewLayoutAttributes.cs
//
// Authors:
//   Rolf Bjarne Kvinge <rolf@xamarin.com>
//     
// Copyright 2013 Xamarin Inc
//

using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Foundation;
using ObjCRuntime;
using CoreGraphics;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace UIKit {
	public partial class UICollectionViewLayoutAttributes : NSObject {
		/// <typeparam name="T">The type of the layout attributes object to return.</typeparam>
		///         <param name="indexPath">The index path describing the cell to create a layout attributes object for.</param>
		///         <summary>Creates a layout attributes object of the specified type for the cell at the specified index path.</summary>
		///         <returns>A layout attributes object representing the cell at the specified index path.</returns>
		///         <remarks>Use this method to create a layout attributes object of a UICollectionViewLayoutAttributes subclass.</remarks>
		[CompilerGenerated]
		public static T CreateForCell<T> (NSIndexPath indexPath) where T : UICollectionViewLayoutAttributes
		{
			global::UIKit.UIApplication.EnsureUIThread ();
			if (indexPath is null)
				throw new ArgumentNullException ("indexPath");
			T result = (T) Runtime.GetNSObject (ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (Class.GetHandle (typeof (T)), Selector.GetHandle ("layoutAttributesForCellWithIndexPath:"), indexPath.Handle));
			GC.KeepAlive (indexPath);
			return result;
		}

		/// <typeparam name="T">The type of the layout attributes object to return.</typeparam>
		///         <param name="kind">The kind identifier for the decoration view.</param>
		///         <param name="indexPath">An index path related to the decoration view.</param>
		///         <summary>Creates a layout attributes object of a specific type representing the decoration view.</summary>
		///         <returns>A layout attributes object of a specific type that represents the decoration view.</returns>
		///         <remarks>Use this method to create a layout attributes object of a specific type representing a decoration view of a specific kind.</remarks>
		[CompilerGenerated]
		public static T CreateForDecorationView<T> (NSString kind, NSIndexPath indexPath) where T : UICollectionViewLayoutAttributes
		{
			global::UIKit.UIApplication.EnsureUIThread ();
			if (kind is null)
				throw new ArgumentNullException ("kind");
			if (indexPath is null)
				throw new ArgumentNullException ("indexPath");
			T result = (T) Runtime.GetNSObject (ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr (Class.GetHandle (typeof (T)), Selector.GetHandle ("layoutAttributesForDecorationViewOfKind:withIndexPath:"), kind.Handle, indexPath.Handle));
			GC.KeepAlive (kind);
			GC.KeepAlive (indexPath);
			return result;
		}

		/// <typeparam name="T">The type of the layout attributes object to return.</typeparam>
		///         <param name="kind">The kind identifier for the supplementary view.</param>
		///         <param name="indexPath">An index path for the supplementary view.</param>
		///         <summary>Creates a layout attributes object of a specific type representing the supplementary view.</summary>
		///         <returns>A layout attributes object of a specific type that represents the supplementary view.</returns>
		///         <remarks>Use this method to create a layout attributes object of a specific type representing a supplementary view of the specified.</remarks>
		[CompilerGenerated]
		public static T CreateForSupplementaryView<T> (NSString kind, NSIndexPath indexPath) where T : UICollectionViewLayoutAttributes
		{
			global::UIKit.UIApplication.EnsureUIThread ();
			if (kind is null)
				throw new ArgumentNullException ("kind");
			if (indexPath is null)
				throw new ArgumentNullException ("indexPath");
			T result = (T) Runtime.GetNSObject (ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr (Class.GetHandle (typeof (T)), Selector.GetHandle ("layoutAttributesForSupplementaryViewOfKind:withIndexPath:"), kind.Handle, indexPath.Handle));
			GC.KeepAlive (kind);
			GC.KeepAlive (indexPath);
			return result;
		}

		static NSString GetKindForSection (UICollectionElementKindSection section)
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

		/// <param name="section">The supplementary view kind.</param>
		///         <param name="indexPath">An index path for the supplementary view.</param>
		///         <summary>Creates a layout attributes object representing the supplementary view.</summary>
		///         <returns>A layout attributes object that represents the supplementary view.</returns>
		///         <remarks>Use this method to create a layout attributes object representing a supplementary view of a specific kind. If you've subclassed UICollectionViewLayoutAttributes and need to return an instance of the subclass, use <see cref="M:UIKit.UICollectionViewLayoutAttributes.CreateForSupplementaryView(Foundation.NSString,Foundation.NSIndexPath)" /> instead. This method is equivalent to calling CreateForSupplementaryView&lt;UICollectionViewLayoutAttributes&gt;.</remarks>
		public static UICollectionViewLayoutAttributes CreateForSupplementaryView (UICollectionElementKindSection section, NSIndexPath indexPath)
		{
			return CreateForSupplementaryView (GetKindForSection (section), indexPath);
		}

		/// <typeparam name="T">The type of the layout attributes object to return.</typeparam>
		///         <param name="section">The supplementary view kind.</param>
		///         <param name="indexPath">An index path for the supplementary view.</param>
		///         <summary>Creates a layout attributes object representing the supplementary view.</summary>
		///         <returns>A layout attributes object that represents the supplementary view.</returns>
		///         <remarks>Use this method to create a layout attributes object of a specific type representing a supplementary view of the specified.</remarks>
		public static T CreateForSupplementaryView<T> (UICollectionElementKindSection section, NSIndexPath indexPath) where T : UICollectionViewLayoutAttributes
		{
			return CreateForSupplementaryView<T> (GetKindForSection (section), indexPath);
		}
	}
}
