// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;

using Foundation;
using UIKit;

namespace AppWithOnDemandResources {
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate {
		public override UISceneConfiguration GetConfiguration (UIApplication application, UISceneSession connectingSceneSession, UISceneConnectionOptions options)
		{
			return new UISceneConfiguration ("Default Configuration", connectingSceneSession.Role);
		}
	}
}
