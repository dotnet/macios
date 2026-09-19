Imports Foundation
Imports UIKit

Namespace tvOSApp1
	<Register("AppDelegate")>
	Public Class AppDelegate
		Inherits UIApplicationDelegate

		Public Overrides Function FinishedLaunching(ByVal application As UIApplication, ByVal launchOptions As NSDictionary) As Boolean
			' Override point for customization after application launch.
			' If not required for your application you can safely delete this method

			Return True
		End Function

		Public Overrides Function GetConfiguration(ByVal application As UIApplication, ByVal connectingSceneSession As UISceneSession, ByVal options As UISceneConnectionOptions) As UISceneConfiguration
			' Called when a new scene session is being created.
			' Use this method to select a configuration to create the new scene with.
			' "Default Configuration" is defined in the Info.plist's 'UISceneConfigurationName' key.
			Return New UISceneConfiguration("Default Configuration", connectingSceneSession.Role)
		End Function
	End Class
End Namespace
