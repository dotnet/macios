// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Runtime.InteropServices;

using CoreGraphics;
using ObjCRuntime;

#nullable enable

namespace Foundation;

public partial class NSValue : NSObject {

	/// <summary>
	/// Converts a native handle to a CGAffineTransform.
	/// </summary>
	/// <param name="handle">The native handle.</param>
	/// <returns>The CGAffineTransform.</returns>
	public static CGAffineTransform ToCGAffineTransform (NativeHandle handle)
	{
		using var nsvalue = Runtime.GetNSObject<NSValue> (handle)!;
		return nsvalue.CGAffineTransformValue;
	}

	/// <summary>
	/// Converts a native handle to a CGPoint.
	/// </summary>
	/// <param name="handle">The native handle.</param>
	/// <returns>The CGPoint.</returns>
	public static CGPoint ToCGPoint (NativeHandle handle)
	{
		using var nsvalue = Runtime.GetNSObject<NSValue> (handle)!;
		return nsvalue.CGPointValue;
	}

	/// <summary>
	/// Converts a native handle to a CGRect.
	/// </summary>
	/// <param name="handle">The native handle.</param>
	/// <returns>The CGRect.</returns>
	public static CGRect ToCGRect (NativeHandle handle)
	{
		using var nsvalue = Runtime.GetNSObject<NSValue> (handle)!;
		return nsvalue.CGRectValue;
	}

	/// <summary>
	/// Converts a native handle to a CGSize.
	/// </summary>
	/// <param name="handle">The native handle.</param>
	/// <returns>The CGSize.</returns>
	public static CGSize ToCGSize (NativeHandle handle)
	{
		using var nsvalue = Runtime.GetNSObject<NSValue> (handle)!;
		return nsvalue.CGSizeValue;
	}

#if MONOMAC

    // @encode(CGAffineTransform) -> "{CGAffineTransform=dddddd}" but...
    // using a C string crash on macOS (while it works fine on iOS)
    [DllImport ("__Internal")]
    extern static IntPtr xamarin_encode_CGAffineTransform ();

    // The `+valueWithCGAffineTransform:` selector comes from UIKit and is not available on macOS
    public unsafe static NSValue FromCGAffineTransform (CGAffineTransform tran)
    {
        return Create ((IntPtr) (void*) &tran, xamarin_encode_CGAffineTransform ());
    }

    // The `CGAffineTransformValue` selector comes from UIKit and is not available on macOS
    public unsafe virtual CGAffineTransform CGAffineTransformValue {
        get {
            var result = new CGAffineTransform ();
            // avoid potential buffer overflow since we use the older `getValue:` API to cover all platforms
            // and we can cheat here with the actual string comparison (since we are the one doing it)
            if (ObjCType == "{CGAffineTransform=dddddd}")
                StoreValueAtAddress ((IntPtr) (void*) &result, (nuint) sizeof (CGAffineTransform));
            return result;
        }
    }

#else

	/// <summary>
	/// Converts a native handle to a CGVector.
	/// </summary>
	/// <param name="handle">The native handle.</param>
	/// <returns>The CGVector.</returns>
	public static CGVector ToCGVector (NativeHandle handle)
	{
		using var nsvalue = Runtime.GetNSObject<NSValue> (handle)!;
		return nsvalue.CGVectorValue;
	}

#endif
}
