//
// CPEnums.cs
//
// Authors:
//	Whitney Schmidt <whschm@microsoft.com>
//
// Copyright (c) Microsoft Corporation.
// Licensed under MIT License.
//

using System;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace CarPlay {

	[NoTV, NoMac, iOS (14, 0)]
	public enum CPMessageListItemType {
		Identifier,
		FullName,
	}

	[NoTV, NoMac, iOS (26, 0), MacCatalyst (26, 0)]
	[Native]
	public enum CPListImageRowItemCondensedElementShape : long {
		RoundedRect = 0,
		Circle = 1,
	}

	[NoTV, NoMac, iOS (26, 0), MacCatalyst (26, 0)]
	[Native]
	public enum CPListImageRowItemImageGridElementShape : long {
		RoundedRect = 0,
		Circle = 1,
	}
}
