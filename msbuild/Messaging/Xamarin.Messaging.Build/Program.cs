using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Messaging.Client;
using System.IO;

namespace Xamarin.Messaging.Build {
	class Program {
		static async Task Main (string [] args)
		{
			var topicGenerator = new TopicGenerator ();
			var arguments = new AgentArgumentsParser ().ParseArguments (args);
			var agent = new BuildAgent (topicGenerator, arguments.Version, arguments.VersionInfo);
			var runner = new AgentConsoleRunner<BuildAgent> (agent, arguments);

			//Hack to support legacy paths from Windows (likely Dev17 versions)
			var index = MessagingContext.BasePath.IndexOf ("Xamarin", StringComparison.Ordinal);

			if (index >= 0) {
				var xamarinPath = MessagingContext.BasePath.Substring (0, index + "Xamarin".Length);

				MessagingContext.BuildsPath = Path.Combine (xamarinPath, "mtbs", "builds");
			}

			await runner.RunAsync (CancellationToken.None).ConfigureAwait (continueOnCapturedContext: false);
		}
	}
}
