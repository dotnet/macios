//
// Custom methods for SWCollaborationMetadata
//
// Authors:
//	Israel Soto  <issoto@microsoft.com>
//
// Copyright 2022 Microsoft Corporation.
//
#nullable enable

using System;
using Foundation;
using ObjCRuntime;

#if !TVOS

namespace SharedWithYouCore {
	[UnsupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos13.0")]
	[SupportedOSPlatform ("ios16.0")]
	[SupportedOSPlatform ("maccatalyst16.0")]
	public enum SWIdentifierType {
		Local,
		Collaboration,
	}

	public partial class SWCollaborationMetadata : NSObject {
		public SWCollaborationMetadata (string localIdentifier) : base (NSObjectFlag.Empty) =>
			InitializeHandle (_InitWithLocalIdentifier (localIdentifier), "initWithLocalIdentifier:");

		[UnsupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.1")]
		[SupportedOSPlatform ("maccatalyst16.1")]
		public SWCollaborationMetadata (string identifier, SWIdentifierType identifierType) : base (NSObjectFlag.Empty)
		{
			switch (identifierType) {
			case SWIdentifierType.Local:
				InitializeHandle (_InitWithLocalIdentifier (identifier), "initWithLocalIdentifier:");
				break;
			case SWIdentifierType.Collaboration:
				InitializeHandle (_InitWithCollaborationIdentifier (identifier), "initWithCollaborationIdentifier:");
				break;
			default:
				ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (identifierType), $"Unknown identifier type: {identifierType}");
				break;
			}
		}
	}
}
#endif
