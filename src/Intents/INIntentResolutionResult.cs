//
// INIntentResolutionResult Generic variant
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

#nullable enable

namespace Intents {

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos14.0")]
	[SupportedOSPlatform ("maccatalyst")]
	[Register ("INIntentResolutionResult", SkipRegistration = true)]
	public sealed partial class INIntentResolutionResult<ObjectType> : INIntentResolutionResult
		where ObjectType : class, INativeObject {
		internal INIntentResolutionResult (NativeHandle handle) : base (handle)
		{
		}
	}

	public partial class INIntentResolutionResult {

		/// <summary>Gets a resolution result indicating that a value is required for the parameter.</summary>
		public static INIntentResolutionResult NeedsValue {
			get {
				throw new NotImplementedException ("All subclasses of INIntentResolutionResult must re-implement this property");
			}
		}

		/// <summary>Gets a resolution result indicating that a value is not required for the parameter.</summary>
		public static INIntentResolutionResult NotRequired {
			get {
				throw new NotImplementedException ("All subclasses of INIntentResolutionResult must re-implement this property");
			}
		}

		/// <summary>Gets a resolution result indicating that the developer's app does not support the parameter.</summary>
		public static INIntentResolutionResult Unsupported {
			get {
				throw new NotImplementedException ("All subclasses of INIntentResolutionResult must re-implement this property");
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos14.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public static INIntentResolutionResult GetUnsupported (nint reason) => throw new NotImplementedException ("All subclasses of INIntentResolutionResult must re-implement this method");

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos14.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public static INIntentResolutionResult GetConfirmationRequired (NSObject itemToConfirm, nint reason) => throw new NotImplementedException ("All subclasses of INIntentResolutionResult must re-implement this method");

	}
}
