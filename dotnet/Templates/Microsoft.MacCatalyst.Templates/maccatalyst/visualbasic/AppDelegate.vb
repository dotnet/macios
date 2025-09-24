Imports Foundation
Imports UIKit

Namespace MacCatalystApp1
	<Register("AppDelegate")>
	Public Class AppDelegate
		Inherits UIApplicationDelegate

		Public Overrides Function FinishedLaunching(ByVal application As UIApplication, ByVal launchOptions As NSDictionary) As Boolean
			' Override point for customization after application launch.
			Return True
		End Function

		Public Overrides Function GetConfiguration(ByVal application As UIApplication, ByVal connectingSceneSession As UISceneSession, ByVal options As UISceneConnectionOptions) As UISceneConfiguration
			' Called when a new scene session is being created.
			' Use this method to select a configuration to create the new scene with.
			' "Default Configuration" is defined in the Info.plist's 'UISceneConfigurationName' key.
			Return New UISceneConfiguration("Default Configuration", connectingSceneSession.Role)
		End Function

		Public Overrides Sub DidDiscardSceneSessions(ByVal application As UIApplication, ByVal sceneSessions As NSSet(Of UISceneSession))
			' Called when the user discards a scene session.
			' If any sessions were discarded while the application was not running, this will be called shortly after 'FinishedLaunching'.
			' Use this method to release any resources that were specific to the discarded scenes, as they will not return.
		End Sub
	End Class
End Namespace
