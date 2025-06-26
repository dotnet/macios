//
// Compat.cs: Stuff we won't provide anymore the next time we can make breaking changes.
//
// Authors:
//   Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013, 2015 Xamarin, Inc.
// Copyright 2019 Microsoft Corporation
//

using System;
using System.ComponentModel;

using CoreGraphics;
using Foundation;
using ObjCRuntime;

namespace UIKit {
#if !XAMCORE_5_0 && IOS
	public partial class UIDocViewController {
		[Obsolete ("Do not use; this constructor doesn't work.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public UIDocViewController ()
			: base (ThrowInvalidOperationException ())
		{
		}
		static NSObjectFlag ThrowInvalidOperationException ()
		{
			throw new InvalidOperationException ("Do not call this constructor, it may crash the app.");
		}
	}
#endif

	public static partial class UIAccessibilityTraitsExtensions {
#if XAMCORE_6_0
		// .NET: GetConstant (manually written, only exists for binary compat) is completely broken, use GetConstantValue (generated).
		// XAMCORE_5_0: GetConstant works (generated), provide GetConstantValue (manually written) which does the same thing, but is obsoleted.
		// XAMCORE_6_0: GetConstant works (generated), remove GetConstantValue since it was obsoleted in the previous version.
#elif XAMCORE_5_0
		// This method is to provide source-compatibility with the previous version. No need to break people's builds if we don't have to.
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use 'GetConstant' instead.")]
		public static ulong? GetConstantValue (this UIAccessibilityTraits self)
		{
			return GetConstant (self);
		}
#else
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use 'GetConstantValue' instead.")]
		public static NSString? GetConstant (this UIAccessibilityTraits self)
		{
			throw new NotSupportedException ($"Use 'GetConstantValue' instead, the return type for this method is incorrect.");
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use 'GetValue (ulong)' instead.")]
		public static UIAccessibilityTraits GetValue (NSString constant)
		{
			throw new NotSupportedException ($"Use 'GetValue (ulong)' instead.");
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use 'GetValue (ulong)' instead.")]
		public static UIAccessibilityTraits GetValue (NativeHandle handle)
		{
			throw new NotSupportedException ($"Use 'GetValue (ulong)' instead.");
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use 'GetValue (ulong)' instead.")]
		public static UIAccessibilityTraits? GetNullableValue (NativeHandle handle)
		{
			throw new NotSupportedException ($"Use 'GetValue (ulong)' instead.");
		}
#endif
	}
}
