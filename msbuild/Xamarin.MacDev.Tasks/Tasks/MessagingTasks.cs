namespace Xamarin.MacDev.Tasks.Messaging;

// We subclass the Xamarin.Messaging.Build.Client tasks so that we have all our tasks in the same
// assembly; otherwise we run into problems with task isolation in .NET, where the tasks in one
// assembly can't access shared state from tasks in another assembly (for instance, all remoted tasks
// need to access shared state from the SayHello task).

public class GenerateBuildSessionId : Xamarin.Messaging.Build.Client.Tasks.GenerateBuildSessionId {
}

public class SayHello : Xamarin.Messaging.Build.Client.Tasks.SayHello {
}

public class CopyFileFromBuildServer : Xamarin.Messaging.Build.Client.Tasks.CopyFileFromBuildServer {
}

public class CopyFilesToBuildServer : Xamarin.Messaging.Build.Client.Tasks.CopyFilesToBuildServer {
}

public class CopyLongPaths : Xamarin.Messaging.Build.Client.Tasks.CopyLongPaths {
}

public class VerifyBuildSignature : Xamarin.Messaging.Build.Client.Tasks.VerifyBuildSignature {
}

public class SayGoodbye : Xamarin.Messaging.Build.Client.Tasks.SayGoodbye {
}
