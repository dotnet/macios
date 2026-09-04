using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xamarin.Messaging.Build.Contracts;
using Xamarin.Messaging.Client;

namespace Xamarin.Messaging.Build {
	public class CompareFilesMessageHandler : RequestHandler<CompareItemsMessage, CompareItemsResult> {
		static readonly ITracer tracer = Tracer.Get<CompareFilesMessageHandler> ();

		protected override async Task<CompareItemsResult> ExecuteAsync (CompareItemsMessage message)
		{
			return await Task.Run (() => {
				var buildPath = Path.Combine (MessagingContext.BuildsPath, message.AppName, message.SessionId);
				var files = new List<string> ();

				using (var hashAlgorithm = Hash.GetAlgorithm ()) {
					foreach (var file in message.Items) {
						var targetPath = Path.Combine (buildPath, PlatformPath.GetPathForCurrentPlatform (file.ItemSpec));

						if (!File.Exists (targetPath)) {
							tracer.Info ($"CompareFiles: '{file.ItemSpec}' is missing on the Mac (expected at '{targetPath}'), it will be copied.");
							files.Add (file.ItemSpec);
						} else {

							using (var stream = File.OpenRead (targetPath)) {
								var localHash = hashAlgorithm.ComputeHashAsString (stream);

								if (file.Hash != localHash) {
									tracer.Info ($"CompareFiles: '{file.ItemSpec}' has a different hash on the Mac (local: '{localHash}', remote: '{file.Hash}'), it will be copied.");
									files.Add (file.ItemSpec);
								} else {
									tracer.Info ($"CompareFiles: '{file.ItemSpec}' already exists on the Mac with a matching hash, it will not be copied.");
								}
							}
						}
					}
				}

				return new CompareItemsResult { MissingFiles = files };
			}).ConfigureAwait (continueOnCapturedContext: false);
		}
	}
}
