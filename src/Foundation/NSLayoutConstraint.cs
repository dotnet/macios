//
// Helper functions to make FromVisualLayout more palattable
//
// Author:
//   Miguel de Icaza
//
// Copyright 2014 Xamarin INc
//

using System;
using Foundation;
using ObjCRuntime;

#if MONOMAC
using View = AppKit.NSView;
#else
using View = UIKit.UIView;
#endif

// Disable until we get around to enable + fix any issues.
#nullable disable

#if MONOMAC
namespace AppKit
#else
namespace UIKit
#endif
{
	public partial class NSLayoutConstraint {
		static NSNumber AsNumber (object o)
		{
			if (o is NSNumber) return (NSNumber) o;
			if (o is double) return new NSNumber ((double) o);
			if (o is int) return new NSNumber ((int) o);
			if (o is float) return new NSNumber ((float) o);
			if (o is long) return new NSNumber ((long) o);
			if (o is uint) return new NSNumber ((uint) o);
			if (o is ulong) return new NSNumber ((ulong) o);
			if (o is byte) return new NSNumber ((byte) o);
			if (o is sbyte) return new NSNumber ((sbyte) o);
			if (o is ushort) return new NSNumber ((ushort) o);
			if (o is short) return new NSNumber ((short) o);
			if (o is nint) return new NSNumber ((nint) o);
			if (o is nuint) return new NSNumber ((nuint) o);
			if (o is nfloat) return new NSNumber ((nfloat) o);
			return null;
		}

		/// <include file="../../docs/api/UIKit/NSLayoutConstraint.xml" path="/Documentation/Docs[@DocId='M:UIKit.NSLayoutConstraint.FromVisualFormat(System.String,UIKit.NSLayoutFormatOptions,System.Object[])']/*" />
		static public NSLayoutConstraint [] FromVisualFormat (string format, NSLayoutFormatOptions formatOptions, params object [] viewsAndMetrics)
		{
			NSMutableDictionary views = null, metrics = null;
			var count = viewsAndMetrics.Length;
			if (count != 0) {
				if ((count % 2) != 0)
					throw new ArgumentException ("You should provide pairs and values, the parameter passed is not even", "viewsAndMetrics");

				for (int i = 0; i < count; i += 2) {
					var key = viewsAndMetrics [i];
					NSString nskey;

					if (key is string)
						nskey = new NSString ((string) key);
					else if (key is NSString)
						nskey = (NSString) key;
					else
						throw new ArgumentException (String.Format ("Item at {0} is not a string or an NSString", i), "viewsAndMetrics");

					var value = viewsAndMetrics [i + 1];
					if (value is View) {
						if (views is null)
							views = new NSMutableDictionary ();
						views [nskey] = (NSObject) value;
						continue;
					} else if (value is INativeObject && Messaging.bool_objc_msgSend_IntPtr (((INativeObject) value).Handle, Selector.GetHandle ("isKindOfClass:"), Class.GetHandle (typeof (View))) != 0) {
						if (views is null)
							views = new NSMutableDictionary ();
						views.LowlevelSetObject (((INativeObject) value).Handle, nskey.Handle);
						GC.KeepAlive (value);
						GC.KeepAlive (nskey);
						continue;
					}
#if !MONOMAC
					// This requires UILayoutSupport class which is not exist on Mac
					else if (value is INativeObject && Messaging.bool_objc_msgSend_IntPtr (((INativeObject) value).Handle, Selector.GetHandle ("conformsToProtocol:"), Protocol.GetHandle (typeof (UILayoutSupport).Name)) != 0) {
						if (views is null)
							views = new NSMutableDictionary ();
						views.LowlevelSetObject (((INativeObject) value).Handle, nskey.Handle);
						GC.KeepAlive (value);
						GC.KeepAlive (nskey);
						continue;
					}
#endif // !MONOMAC

					var number = AsNumber (value);
					if (number is null)
						throw new ArgumentException (String.Format ("Item at {0} is not a number or a view", i + 1), "viewsAndMetrics");
					if (metrics is null)
						metrics = new NSMutableDictionary ();
					metrics [nskey] = number;
				}
			}
			if (views is null)
				throw new ArgumentException ("You should at least provide a pair of name, view", "viewAndMetrics");

			return FromVisualFormat (format, formatOptions, metrics, views);
		}

		public static NSLayoutConstraint Create (NSObject view1, NSLayoutAttribute attribute1, NSLayoutRelation relation, nfloat multiplier, nfloat constant)
		{
			return NSLayoutConstraint.Create (view1, attribute1, relation, null, NSLayoutAttribute.NoAttribute, multiplier, constant);
		}

		/// <param name="view1">To be added.</param>
		///         <param name="attribute1">To be added.</param>
		///         <param name="relation">To be added.</param>
		///         <summary>Static factory method to create a constraint based on a <see cref="T:UIKit.UIView" />, an <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=T:NSLayoutAttribute&amp;scope=Xamarin" title="T:NSLayoutAttribute">T:NSLayoutAttribute</a></format>, and an <see cref="T:UIKit.NSLayoutRelation" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSLayoutConstraint Create (NSObject view1, NSLayoutAttribute attribute1, NSLayoutRelation relation)
		{
			return NSLayoutConstraint.Create (view1, attribute1, relation, null, NSLayoutAttribute.NoAttribute, 1.0f, 0f);
		}

		// This solves the duplicate selector export problem while not breaking the API.
		public static NSLayoutConstraint Create (NSObject view1, NSLayoutAttribute attribute1, NSLayoutRelation relation,
				NSObject view2, NSLayoutAttribute attribute2, nfloat multiplier, nfloat constant)
		{
			return Create ((INativeObject) view1, attribute1, relation, view2, attribute2, multiplier, constant);
		}

#if !MONOMAC || NET
#if NET
		/// <typeparam name="AnchorType">To be added.</typeparam>
		///         <summary>For an anchor-based constraint, returns the first anchor, properly downcast to <c>AnchorType</c>.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
#endif
		public NSLayoutAnchor<AnchorType> FirstAnchor<AnchorType> () where AnchorType : NSObject
		{
			return Runtime.GetNSObject<NSLayoutAnchor<AnchorType>> (_FirstAnchor ());
		}

#if NET
		/// <typeparam name="AnchorType">To be added.</typeparam>
		///         <summary>For an anchor-based constraint, returns the second anchor, properly downcast to <c>AnchorType</c>.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
#endif
		public NSLayoutAnchor<AnchorType> SecondAnchor<AnchorType> () where AnchorType : NSObject
		{
			return Runtime.GetNSObject<NSLayoutAnchor<AnchorType>> (_SecondAnchor ());
		}
#endif // !MONOMAC || NET
	}
}
