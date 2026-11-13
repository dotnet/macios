// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Foundation;

namespace macOSXpcService1;

public delegate void ServiceReply (string text);

[Protocol (Name = "macOSXpcService1ServiceProtocol"), Model]
[BaseType (typeof (NSObject))]
[XpcInterface]
interface ServiceProtocol {
	[Abstract]
	[Export ("echo:reply:")]
	void Echo (string text, ServiceReply reply);
}
