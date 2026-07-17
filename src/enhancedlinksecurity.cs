#nullable enable

using System;

using Foundation;
using ObjCRuntime;

namespace EnhancedLinkSecurity {

	[TV (27, 0), Mac (27, 0), iOS (27, 0), MacCatalyst (27, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface IMEnhancedLinkSecurityManager {

		[Static]
		[Export ("sharedManager", ArgumentSemantic.Strong)]
		IMEnhancedLinkSecurityManager SharedManager { get; }

		[Export ("hasURLsRequiringEnhancedSecurity")]
		bool HasUrlsRequiringEnhancedSecurity { get; }

		[Export ("requestEnhancedSecurityForURL:")]
		void RequestEnhancedSecurity (NSUrl url);

		[Export ("requestEnhancedSecurityForURLs:")]
		void RequestEnhancedSecurity (NSUrl [] urls);

		[Export ("shouldUseEnhancedSecurityForURL:")]
		bool ShouldUseEnhancedSecurity (NSUrl url);

		[Async]
		[Export ("shouldUseEnhancedSecurityForURL:completion:")]
		void ShouldUseEnhancedSecurity (NSUrl url, Action<bool> completionHandler);
	}
}
