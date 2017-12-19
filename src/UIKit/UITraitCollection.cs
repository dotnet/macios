// 
// UITraitCollection.cs: support for UITraitCollection
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

using System;
using XamCore.ObjCRuntime;
using XamCore.Foundation;

namespace XamCore.UIKit {
#if !WATCH
	public partial class UITraitCollection {
		public UITraitCollection FromPreferredContentSizeCategory (UIContentSizeCategory category)
		{
			return FromPreferredContentSizeCategory (category.GetConstant ());
		}
	}
#endif
}
