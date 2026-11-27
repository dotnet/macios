// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Foundation;

namespace macOSXpcService1;

static class Program {
	static void Main ()
	{
		using var listenerDelegate = new ServiceDelegate ();
		using var listener = NSXpcListener.ServiceListener;
		listener.Delegate = listenerDelegate;
		// Unlike other listeners, the service listener's Resume does not return.
		listener.Resume ();
		GC.KeepAlive (listenerDelegate);
	}
}
