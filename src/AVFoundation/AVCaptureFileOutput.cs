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

		/// <param name="outputFileUrl">The output file url.</param>
		///         <param name="startRecordingFromConnections">The start recording from connections.</param>
		///         <param name="finishedRecording">The finished recording.</param>
		///         <summary>To be added.</summary>
		public void StartRecordingToOutputFile (NSUrl outputFileUrl, Action<NSObject []> startRecordingFromConnections, Action<NSObject [], NSError?> finishedRecording)
		{
			StartRecordingToOutputFile (outputFileUrl, new recordingProxy (startRecordingFromConnections, finishedRecording));
		}
	}
}

#endif // !TVOS
