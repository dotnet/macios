// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using CoreFoundation;
using CoreGraphics;
using Foundation;
using ObjCRuntime;

namespace NativeFactories {
	delegate void FactoryCallback (CGPath path, CGPath [] paths, ref CGPath result);
	delegate void PartialFactoryCallback (DispatchData data, DispatchData [] values, ref DispatchData result);

	[BaseType (typeof (NSObject))]
	interface Consumer {
		[Export ("path")]
		CGPath Path { get; }

		[Export ("paths")]
		CGPath [] Paths { get; }

		[Export ("updatePath:")]
		void UpdatePath (ref CGPath path);

		[Export ("updatePaths:")]
		void UpdatePaths (ref CGPath [] paths);

		[Export ("callback:")]
		void SetCallback (FactoryCallback callback);

		[Export ("foreign")]
		ForeignWrapper Foreign { get; }

		[Export ("inherited")]
		InheritedForeignWrapper Inherited { get; }

		[Export ("data")]
		DispatchData Data { get; }

		[Export ("dataValues")]
		DispatchData [] DataValues { get; }

		[Export ("updateData:")]
		void UpdateData (ref DispatchData data);

		[Export ("updateDataValues:")]
		void UpdateDataValues (ref DispatchData [] values);

		[Export ("partialCallback:")]
		void SetPartialCallback (PartialFactoryCallback callback);
	}

	[Static]
	interface Keys {
		[Field ("FactoryPathKey", "__Internal")]
		NSString PathKey { get; }

		[Field ("FactoryPathsKey", "__Internal")]
		NSString PathsKey { get; }

		[Field ("FactoryPath", "__Internal")]
		CGPath NativePath { get; }

		[Field ("FactoryDataKey", "__Internal")]
		NSString DataKey { get; }

		[Field ("FactoryDataValuesKey", "__Internal")]
		NSString DataValuesKey { get; }
	}

	[StrongDictionary ("Keys")]
	interface Options {
		CGPath Path { get; set; }
		CGPath [] Paths { get; set; }
		DispatchData Data { get; set; }
		DispatchData [] DataValues { get; set; }
	}
}

namespace CoreFoundation {
	[Partial]
	interface DispatchData { }
}
