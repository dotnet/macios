// Copyright 2016 Xamarin Inc. All rights reserved.

#if !XAMCORE_3_0 && !MONOMAC

using System;

namespace XamCore.Social {

	partial class SLComposeSheetConfigurationItem {

		[Obsolete ("Use the 'TapHandler' property.")]
		public virtual void SetTapHandler (Action tapHandler)
		{
			TapHandler = tapHandler;
		}
	}
}

#endif