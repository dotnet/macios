// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using AVFoundation;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using System.Runtime.Versioning;

namespace Microsoft.Macios.Generator.Tests.Protocols.Data;

[SupportedOSPlatform ("ios")]
[SupportedOSPlatform ("tvos")]
[SupportedOSPlatform ("macos")]
[SupportedOSPlatform ("maccatalyst13.1")]
[BindingType<ObjCBindings.Protocol>]
public partial interface IMyNSCoding {

	[Export<Method> ("initWithCoder:", Flags = Method.Factory)]
	public virtual partial IMyNSCoding CreateWithCoder (NSObject coder);
}

[SupportedOSPlatform ("macos")]
[SupportedOSPlatform ("ios")]
[SupportedOSPlatform ("tvos")]
[SupportedOSPlatform ("maccatalyst13.1")]
[BindingType<Class>]
public partial class InlineProtocolDuplicatedConstructors : IMyNSCoding {

	// we are testing that the protocol constructor is not added and that we don't get a duplicate
	[Export<Constructor> ("initWithCoder:")]
	public InlineProtocolDuplicatedConstructors (NSObject coder);
}
