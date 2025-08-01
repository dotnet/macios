// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#pragma warning disable APL0003

using System;
using System.Runtime.Versioning;
using AVFoundation;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using nfloat = System.Runtime.InteropServices.NFloat;

namespace TestNamespace;

public partial class OuterClass {

	public partial class InnerClass {

		[BindingType<ObjCBindings.Class>]
		public partial class NestedClasses {

			// simple value type
			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("maccatalyst13.1")]
			[Export<Property> ("count")]
			public virtual partial nuint Count { get; set; }

			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("maccatalyst13.1")]
			[Export<Property> ("lineSpacing")]
			public virtual partial nfloat LineSpacing { get; set; }

			// array
			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("maccatalyst13.1")]
			[Export<Property> ("sizes")]
			public virtual partial nuint [] Sizes { get; set; }

			// boolean
			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("maccatalyst13.1")]
			[Export<Property> ("containsAttachments")]
			public virtual partial bool ContainsAttachments { get; set; }

			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("maccatalyst13.1")]
			[Export<Method> ("valueForKey:", Flags = Method.MarshalNativeExceptions)]
			public virtual unsafe partial NSObject ValueForKey (NSString key);

			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("maccatalyst13.1")]
			[Export<Method> ("setValue:forKey:")]
			public virtual unsafe partial void SetValueForKey (NSObject value, NSString key);

			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("maccatalyst13.1")]
			[Export<Method> ("writeToFile:atomically:")]
			public virtual unsafe partial bool WriteToFile (string path, bool useAuxiliaryFile);

			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("maccatalyst13.1")]
			[Export<Method> ("arrayWithContentsOfFile:")]
			public virtual unsafe partial static NSArray FromFile (string path);

			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("maccatalyst13.1")]
			[Export<Method> ("sortedArrayUsingComparator:")]
			public virtual unsafe partial NSArray Sort (NSComparator cmptr);

		}
	}

}
