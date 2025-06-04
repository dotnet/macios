using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using System.Threading.Tasks;

using ObjCRuntime;

#nullable enable

namespace AVFoundation {

	/// <include file="../../docs/api/AVFoundation/AVSpeechSynthesizer.xml" path="/Documentation/Docs[@DocId='T:AVFoundation.AVSpeechSynthesizer']/*" />
	public partial class AVSpeechSynthesizer {
#if !XAMCORE_5_0
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		[Obsolete ("Do not use this API, it doesn't work correctly. Use the non-Async version (WriteUtterance) instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public unsafe virtual Task<AVAudioBuffer> WriteUtteranceAsync (AVSpeechUtterance utterance)
		{
			var tcs = new TaskCompletionSource<AVAudioBuffer> ();
			WriteUtterance (utterance, (obj_) => {
				tcs.SetResult (obj_!);
			});
			return tcs.Task;
		}
#endif
	}
}
