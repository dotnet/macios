// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using Foundation;

namespace AllowNullProperties {
	[BaseType (typeof (NSObject))]
	interface Properties {
		[Export ("object")]
		[AllowNull]
		NSObject Object { get; set; }

		[Export ("text")]
		[AllowNull]
		string Text { get; set; }

		[Export ("objects")]
		[AllowNull]
		NSObject [] Objects { get; set; }

		[Export ("strings")]
		[AllowNull]
		string [] Strings { get; set; }

		[Export ("callback")]
		[AllowNull]
		Action Callback { get; set; }

		[Wrap ("Object")]
		[AllowNull]
		NSObject WrappedObject { get; set; }

		[Export ("nullableObject")]
		[NullAllowed]
		NSObject NullableObject { get; set; }

		[Export ("requiredObject")]
		NSObject RequiredObject { get; set; }
	}

	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface PropertiesProtocol {
		[Abstract]
		[Export ("object")]
		[AllowNull]
		NSObject Object { get; set; }

		[Export ("optionalObject")]
		[AllowNull]
		NSObject OptionalObject { get; set; }
	}

	[BaseType (typeof (NSObject))]
	interface ProtocolImplementation : PropertiesProtocol {
	}
}
