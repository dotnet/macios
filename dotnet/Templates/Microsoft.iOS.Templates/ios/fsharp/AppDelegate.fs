namespace iOSApp1

open UIKit
open Foundation

[<Register(nameof AppDelegate)>]
type AppDelegate() =
    inherit UIApplicationDelegate()
       
    override this.FinishedLaunching(application: UIApplication, launchOptions: NSDictionary) =
        // Override point for customization after application launch.
        true

    override this.GetConfiguration(application: UIApplication, connectingSceneSession: UISceneSession, options: UISceneConnectionOptions) =
        // Called when a new scene session is being created.
        // Use this method to select a configuration to create the new scene with.
        // "Default Configuration" is defined in the Info.plist's 'UISceneConfigurationName' key.
        new UISceneConfiguration("Default Configuration", connectingSceneSession.Role)

    override this.DidDiscardSceneSessions(application: UIApplication, sceneSessions: NSSet<UISceneSession>) =
        // Called when the user discards a scene session.
        // If any sessions were discarded while the application was not running, this will be called shortly after 'FinishedLaunching'.
        // Use this method to release any resources that were specific to the discarded scenes, as they will not return.
        ()
