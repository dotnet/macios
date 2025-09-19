// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

using ObjCRuntime;
using Foundation;
using CoreFoundation;
using CoreGraphics;

namespace ImageIO {
#if !XAMCORE_5_0
	public partial class CGCopyImageSourceOptions {
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use 'NullableMergeMetadata' instead.")]
		public bool MergeMetadata {
			get => NullableMergeMetadata.HasValue ? NullableMergeMetadata.Value : false;
			set => NullableMergeMetadata = value;
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use 'NullableExcludeXmp' instead.")]
		public bool ShouldExcludeXMP {
			get => NullableShouldExcludeXmp.HasValue ? NullableShouldExcludeXmp.Value : false;
			set => NullableShouldExcludeXmp = value;
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use 'NullableExcludeGps' instead.")]
		public bool ShouldExcludeGPS {
			get => NullableShouldExcludeGps.HasValue ? NullableShouldExcludeGps.Value : false;
			set => NullableShouldExcludeGps = value;
		}
	}
#endif // !XAMCORE_5_0
}
