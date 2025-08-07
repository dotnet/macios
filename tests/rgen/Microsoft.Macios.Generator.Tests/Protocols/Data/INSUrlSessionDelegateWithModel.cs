// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using System.Runtime.Versioning;

namespace Microsoft.Macios.Generator.Tests.Protocols.Data;

[SupportedOSPlatform ("ios")]
[SupportedOSPlatform ("tvos")]
[SupportedOSPlatform ("macos")]
[SupportedOSPlatform ("maccatalyst13.1")]
[BindingType<ObjCBindings.Protocol> (ObjCBindings.Protocol.CreateModel, Name = "NSURLSessionDelegate")]
public partial interface INSUrlSessionDelegate {

	[Export<Method> ("URLSession:didBecomeInvalidWithError:")]
	public partial void DidBecomeInvalid (NSUrlSession session, NSError error);

	[Export<Method> ("URLSession:didReceiveChallenge:completionHandler:")]
	public partial void DidReceiveChallenge (NSUrlSession session, NSUrlAuthenticationChallenge challenge, Action<NSUrlSessionAuthChallengeDisposition, NSUrlCredential> completionHandler);

	[Export<Method> ("URLSessionDidFinishEventsForBackgroundURLSession:")]
	public partial void DidFinishEventsForBackgroundSession (NSUrlSession session);
}
