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

		public static UICollectionViewLayoutAttributes CreateForSupplementaryView (UICollectionElementKindSection section, NSIndexPath indexPath)
		{
			return CreateForSupplementaryView (GetKindForSection (section), indexPath);
		}

		public static T CreateForSupplementaryView<T> (UICollectionElementKindSection section, NSIndexPath indexPath) where T : UICollectionViewLayoutAttributes
		{
			return CreateForSupplementaryView<T> (GetKindForSection (section), indexPath);
		}
	}
}
