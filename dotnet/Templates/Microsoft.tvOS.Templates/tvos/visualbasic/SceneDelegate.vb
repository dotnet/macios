Imports Foundation
Imports UIKit

Namespace tvOSApp1
	<Register("SceneDelegate")>
	Public Class SceneDelegate
		Inherits UIWindowSceneDelegate

		<Export("window")>
		Public Overrides Property Window As UIWindow

		Public Overrides Sub WillConnect(ByVal scene As UIScene, ByVal session As UISceneSession, ByVal connectionOptions As UISceneConnectionOptions)
			' The storyboard automatically initializes the window and attaches it to the scene.
		End Sub
	End Class
End Namespace
