﻿//
// INWorkoutLocationTypeResolutionResult.cs
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

#if XAMCORE_2_0 && !MONOMAC
using System;
using XamCore.Foundation;
using XamCore.ObjCRuntime;

namespace XamCore.Intents {
	public partial class INWorkoutLocationTypeResolutionResult {

		public static INWorkoutLocationTypeResolutionResult GetSuccess (INWorkoutLocationType resolvedValue)
		{
#if IOS
			if (XamCore.UIKit.UIDevice.CurrentDevice.CheckSystemVersion (11, 0))
#elif WATCH
			if (XamCore.WatchKit.WKInterfaceDevice.CurrentDevice.CheckSystemVersion (4, 0))
#endif
				return SuccessWithResolvedWorkoutLocationType (resolvedValue);
			else
				return SuccessWithResolvedValue (resolvedValue);
		}

		public static INWorkoutLocationTypeResolutionResult GetConfirmationRequired (INWorkoutLocationType valueToConfirm)
		{
#if IOS
			if (XamCore.UIKit.UIDevice.CurrentDevice.CheckSystemVersion (11, 0))
#elif WATCH
			if (XamCore.WatchKit.WKInterfaceDevice.CurrentDevice.CheckSystemVersion (4, 0))
#endif
				return ConfirmationRequiredWithWorkoutLocationTypeToConfirm (valueToConfirm);
			else
				return ConfirmationRequiredWithValueToConfirm (valueToConfirm);
		}
	}
}
#endif
