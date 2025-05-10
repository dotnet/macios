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
}
