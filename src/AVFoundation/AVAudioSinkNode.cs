using System.ComponentModel;

using AudioToolbox;

namespace AVFoundation {
#if XAMCORE_5_0
	public delegate int AVAudioSinkNodeReceiverHandler (AudioTimeStamp timestamp, uint frameCount, AudioBuffers inputData);
#else
	public delegate int AVAudioSinkNodeReceiverHandler (AudioTimeStamp timestamp, uint frameCount, ref AudioBuffers inputData);
	public delegate int AVAudioSinkNodeReceiverHandler2 (AudioTimeStamp timestamp, uint frameCount, AudioBuffers inputData);
#endif // XAMCORE_5_0

	public partial class AVAudioSinkNode {
		/// <summary>Creates an <see cref="AVAudioSinkNode" /> with a realtime-safe receiver block to receive audio data from the input.</summary>
		/// <param name="receiverHandler">The realtime-safe callback that receives audio data from the input. It is called on the realtime thread, so it must be handled in a thread-safe manner and must not make any blocking calls.</param>
		/// <returns>A new <see cref="AVAudioSinkNode" />.</returns>
		/// <remarks>This is the preferred way to create an <see cref="AVAudioSinkNode" />.</remarks>
		[SupportedOSPlatform ("ios27.0")]
		[SupportedOSPlatform ("tvos27.0")]
		[SupportedOSPlatform ("macos27.0")]
		[SupportedOSPlatform ("maccatalyst27.0")]
#if XAMCORE_5_0
		public static AVAudioSinkNode CreateRealtimeSafe (AVAudioSinkNodeReceiverHandler receiverHandler)
#else
		public static AVAudioSinkNode CreateRealtimeSafe (AVAudioSinkNodeReceiverHandler2 receiverHandler)
#endif
			=> new AVAudioSinkNode (GetHandler (receiverHandler), realtimeSafe: true);

		AVAudioSinkNode (AVAudioSinkNodeReceiverHandlerRaw receiverHandler, bool realtimeSafe)
			: base (NSObjectFlag.Empty)
		{
			InitializeHandle (_InitWithRealtimeSafeReceiverBlock (receiverHandler), "initWithRealtimeSafeReceiverBlock:");
		}

#if !XAMCORE_5_0
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use the overload that takes a delegate that does not take a 'ref AudioBuffers' instead. Assigning a value to the 'inputData' parameter in the callback has no effect.")]
#endif // !XAMCORE_5_0
		public AVAudioSinkNode (AVAudioSinkNodeReceiverHandler receiverHandler)
			: this (GetHandler (receiverHandler))
		{
		}

#if !XAMCORE_5_0
		public AVAudioSinkNode (AVAudioSinkNodeReceiverHandler2 receiverHandler)
			: this (GetHandler (receiverHandler))
		{
		}
#endif // !XAMCORE_5_0

		static AVAudioSinkNodeReceiverHandlerRaw GetHandler (AVAudioSinkNodeReceiverHandler receiverHandler)
		{
			AVAudioSinkNodeReceiverHandlerRaw rv = (timestamp, frameCount, inputData) => {
				unsafe {
					var ts = *(AudioTimeStamp*) timestamp;
					var abuffers = new AudioBuffers (inputData);
#if XAMCORE_5_0
					return receiverHandler (ts, frameCount, abuffers);
#else
					return receiverHandler (ts, frameCount, ref abuffers);
#endif // XAMCORE_5_0
				}
			};
			return rv;
		}

#if !XAMCORE_5_0
		static AVAudioSinkNodeReceiverHandlerRaw GetHandler (AVAudioSinkNodeReceiverHandler2 receiverHandler)
		{
			AVAudioSinkNodeReceiverHandlerRaw rv = (timestamp, frameCount, inputData) => {
				unsafe {
					var ts = *(AudioTimeStamp*) timestamp;
					var abuffers = new AudioBuffers (inputData);
					return receiverHandler (ts, frameCount, abuffers);
				}
			};
			return rv;
		}
#endif // !XAMCORE_5_0
	}
}
