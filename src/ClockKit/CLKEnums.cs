﻿//
// ClockKit Enums
//
// Authors:
//	Alex Soto  <alex.soto@xamarin.com>
//
// Copyright 2015-2016 Xamarin Inc. All rights reserved.
//

using System;
using ObjCRuntime;

namespace ClockKit {

	[Native]
	public enum CLKComplicationFamily : long {
		ModularSmall,
		ModularLarge,
		UtilitarianSmall,
		UtilitarianLarge,
		CircularSmall,
		// nothing has the value of 5
		[Watch (3,0)]
		UtilitarianSmallFlat = 6,
		[Watch (3,0)]
		ExtraLarge = 7,
		[Watch (5,0)]
		GraphicCorner,
		[Watch (5,0)]
		GraphicBezel,
		[Watch (5,0)]
		GraphicCircular,
		[Watch (5,0)]
		GraphicRectangular,
	}

	[Native]
	public enum CLKComplicationTimelineAnimationBehavior : ulong {
		Never,
		Grouped,
		Always
	}

	[Native]
	[Flags]
	public enum CLKComplicationTimeTravelDirections : ulong {
		None = 0,
		Forward = 1 << 0,
		Backward = 1 << 1
	}

	[Native]
	public enum CLKComplicationPrivacyBehavior : ulong {
		ShowOnLockScreen,
		HideOnLockScreen
	}

	[Native]
	public enum CLKComplicationColumnAlignment : long {
		Leading,	// renamed from Left in watchOS 2.1
		Trailing,	// renamed from Right in watchOS 2.1
	}

	[Native]
	public enum CLKComplicationRingStyle : long {
		Closed,
		Open
	}

	[Native]
	public enum CLKRelativeDateStyle : long {
		Natural,
		Offset,
		Timer,
		[Watch (5,0)]
		NaturalAbbreviated,
	}

	[Watch (5,0)]
	[Native]
	public enum CLKGaugeProviderStyle : long {
		Ring,
		Fill,
	}
}

