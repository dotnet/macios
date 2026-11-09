// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Foundation;

namespace macOSXpcService1;

sealed class ServiceDelegate : NSXpcListenerDelegate {
	public override bool ShouldAcceptConnection (NSXpcListener listener, NSXpcConnection connection)
	{
		connection.ExportedInterface = NSXpcInterface.Create (typeof (IServiceProtocol));
		connection.ExportedObject = new Service ();
		connection.Activate ();
		return true;
	}
}

sealed class Service : ServiceProtocol {
	public override void Echo (string text, ServiceReply reply)
	{
		reply (text);
	}
}
