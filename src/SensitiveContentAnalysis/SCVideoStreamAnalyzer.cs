// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Foundation;
using ObjCRuntime;

namespace SensitiveContentAnalysis {
#if __IOS__ && !__MACCATALYST__
	public partial class SCVideoStreamAnalyzer {
		/// <summary>Create a new <see cref="SCVideoStreamAnalyzer" /> instance with the specified particant and stream direction.</summary>
		/// <param name="participantUuid">The unique identifier for a participant in the conference call.</param>
		/// <param name="streamDirection">Specifies whether the stream comes from the local camera or a remote location.</param>
		/// <param name="error">The error object if an error occurs.</param>
		/// <returns>A new <see cref="SCVideoStreamAnalyzer" /> instance with the specified particant and stream direction if successful, <see langword="null" /> otherwise.</returns>
		public static SCVideoStreamAnalyzer? Create (string participantUuid, SCVideoStreamAnalyzerStreamDirection streamDirection, out NSError? error)
		{
			var rv = new SCVideoStreamAnalyzer (NSObjectFlag.Empty);
			rv.InitializeHandle (rv._InitWithParticipantUuid (participantUuid, streamDirection, out error), "initWithParticipantUUID:streamDirection:error:", false);
			if (rv.Handle == NativeHandle.Zero) {
				rv.Dispose ();
				return null;
			}
			return rv;
		}
	}
#endif // __IOS__ && !__MACCATALYST__
}
