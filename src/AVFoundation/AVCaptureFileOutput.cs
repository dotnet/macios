//
// AVCaptureFileOutput.cs
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2014 Xamarin Inc (http://www.xamarin.com)
//

#if !TVOS

#nullable enable

namespace AVFoundation {
	public partial class AVCaptureFileOutput {
		class recordingProxy : AVCaptureFileOutputRecordingDelegate {
			Action<NSObject []> startRecordingFromConnections;
			Action<NSObject [], NSError?> finishedRecording;

			public recordingProxy (Action<NSObject []> startRecordingFromConnections, Action<NSObject [], NSError?> finishedRecording)
			{
				this.startRecordingFromConnections = startRecordingFromConnections;
				this.finishedRecording = finishedRecording;
			}

			public override void DidStartRecording (AVCaptureFileOutput captureOutput, NSUrl outputFileUrl, NSObject [] connections)
			{
				startRecordingFromConnections (connections);
			}

			public override void FinishedRecording (AVCaptureFileOutput captureOutput, NSUrl outputFileUrl, NSObject [] connections, NSError? error)
			{
				finishedRecording (connections, error);
			}

		}

		/// <param name="outputFileUrl">The URL of the output file to record to.</param>
		///         <param name="startRecordingFromConnections">A callback invoked when recording starts, receiving the active connections.</param>
		///         <param name="finishedRecording">A callback invoked when recording finishes, receiving the connections and any error that occurred.</param>
		/// <summary>Starts recording to the specified output file URL with the given callbacks.</summary>
		public void StartRecordingToOutputFile (NSUrl outputFileUrl, Action<NSObject []> startRecordingFromConnections, Action<NSObject [], NSError?> finishedRecording)
		{
			StartRecordingToOutputFile (outputFileUrl, new recordingProxy (startRecordingFromConnections, finishedRecording));
		}
	}
}

#endif // !TVOS
