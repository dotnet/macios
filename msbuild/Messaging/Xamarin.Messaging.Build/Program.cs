using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Messaging.Client;

namespace Xamarin.Messaging.Build {
	class Program {
		static async Task Main (string [] args)
		{
			var topicGenerator = new TopicGenerator ();
			var arguments = new AgentArgumentsParser ().ParseArguments (args);
			var agent = new BuildAgent (topicGenerator, arguments.Version, arguments.VersionInfo);
			var runner = new AgentConsoleRunner<BuildAgent> (agent, arguments);

			//Hack to support legacy paths from Windows (likely Dev17 versions)
			if (MessagingContext.BasePath.Contains("Xamarin")) {
				var xamarinPath = MessagingContext.BasePath.Substring (0, MessagingContext.BasePath.IndexOf ("Xamarin") + "Xamarin".Length);

				MessagingContext.BuildsPath = Path.Combine (xamarinPath, "mtbs", "builds");
			}

			await runner.RunAsync (CancellationToken.None).ConfigureAwait (continueOnCapturedContext: false);
		}
	}
}
