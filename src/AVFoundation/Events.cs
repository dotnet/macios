//
// C#-like events for AVFoundation classes
//
// Author:
//   Miguel de Icaza (miguel@novell.com)
// Copyright 2009, Novell, Inc.
// Copyright 2010, Novell, Inc.
// Copyright 2011, 2012 Xamarin Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//

using System;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace AVFoundation {

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class AVErrorEventArgs : EventArgs {
		/// <param name="error">To be added.</param>
		///         <summary>Initializes a new instance of the AVErrorEventArgs class.</summary>
		///         <remarks>
		///         </remarks>
		public AVErrorEventArgs (NSError error)
		{
			Error = error;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSError Error { get; private set; }
	}

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class AVStatusEventArgs : EventArgs {
		/// <param name="status">To be added.</param>
		///         <summary>Initializes a new instance of the AVStatusEventArgs class.</summary>
		///         <remarks>
		///         </remarks>
		public AVStatusEventArgs (bool status)
		{
			Status = status;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool Status { get; private set; }
	}

#pragma warning disable 672
	sealed class InternalAVAudioPlayerDelegate : AVAudioPlayerDelegate {
		internal EventHandler? cbEndInterruption;
		internal EventHandler? cbBeginInterruption;
		internal EventHandler<AVStatusEventArgs>? cbFinishedPlaying;
		internal EventHandler<AVErrorEventArgs?>? cbDecoderError;

		public InternalAVAudioPlayerDelegate ()
		{
			IsDirectBinding = false;
		}

		[Preserve (Conditional = true)]
		public override void FinishedPlaying (AVAudioPlayer player, bool flag)
		{
			if (cbFinishedPlaying is not null)
				cbFinishedPlaying (player, new AVStatusEventArgs (flag));
			if (player.Handle == IntPtr.Zero)
				throw new ObjectDisposedException ("player", "the player object was Dispose()d during the callback, this has corrupted the state of the program");
			GC.KeepAlive (player);
		}

		[Preserve (Conditional = true)]
		public override void DecoderError (AVAudioPlayer player, NSError? error)
		{
			if (cbDecoderError is not null)
				cbDecoderError (player, error is not null ? new AVErrorEventArgs (error) : null);
		}
#if !MONOMAC
		[Preserve (Conditional = true)]
		public override void BeginInterruption (AVAudioPlayer player)
		{
			if (cbBeginInterruption is not null)
				cbBeginInterruption (player, EventArgs.Empty);
		}

		[Preserve (Conditional = true)]
		public override void EndInterruption (AVAudioPlayer player)
		{
			if (cbEndInterruption is not null)
				cbEndInterruption (player, EventArgs.Empty);
		}
#endif
	}
#pragma warning restore 672

	public partial class AVAudioPlayer {
		InternalAVAudioPlayerDelegate EnsureEventDelegate ()
		{
			var del = WeakDelegate as InternalAVAudioPlayerDelegate;
			if (del is null) {
				del = new InternalAVAudioPlayerDelegate ();
				WeakDelegate = del;
			}
			return del;
		}

		public event EventHandler<AVStatusEventArgs> FinishedPlaying {
			add {
				EnsureEventDelegate ().cbFinishedPlaying += value;
			}

			remove {
				if (value is not null)
					EnsureEventDelegate ().cbFinishedPlaying -= value;
			}
		}

		public event EventHandler<AVErrorEventArgs> DecoderError {
			add {
				EnsureEventDelegate ().cbDecoderError += value;
			}

			remove {
				if (value is not null)
					EnsureEventDelegate ().cbDecoderError -= value;
			}
		}

		public event EventHandler BeginInterruption {
			add {
				EnsureEventDelegate ().cbBeginInterruption += value;
			}

			remove {
				if (value is not null)
					EnsureEventDelegate ().cbBeginInterruption -= value;
			}
		}

		public event EventHandler EndInterruption {
			add {
				EnsureEventDelegate ().cbEndInterruption += value;
			}

			remove {
				if (value is not null)
					EnsureEventDelegate ().cbEndInterruption -= value;
			}
		}
	}

#if !TVOS
	internal class InternalAVAudioRecorderDelegate : AVAudioRecorderDelegate {
		internal EventHandler? cbEndInterruption;
		internal EventHandler? cbBeginInterruption;
		internal EventHandler<AVStatusEventArgs>? cbFinishedRecording;
		internal EventHandler<AVErrorEventArgs?>? cbEncoderError;

		public InternalAVAudioRecorderDelegate ()
		{
			IsDirectBinding = false;
		}

		[Preserve (Conditional = true)]
		public override void FinishedRecording (AVAudioRecorder recorder, bool flag)
		{
			if (cbFinishedRecording is not null)
				cbFinishedRecording (recorder, new AVStatusEventArgs (flag));
		}

		[Preserve (Conditional = true)]
		public override void EncoderError (AVAudioRecorder recorder, NSError? error)
		{
			if (cbEncoderError is not null)
				cbEncoderError (recorder, error is not null ? new AVErrorEventArgs (error) : null);
		}
#if !MONOMAC
		[Preserve (Conditional = true)]
		public override void BeginInterruption (AVAudioRecorder recorder)
		{
			if (cbBeginInterruption is not null)
				cbBeginInterruption (recorder, EventArgs.Empty);
		}

		[Preserve (Conditional = true)]
		public override void EndInterruption (AVAudioRecorder recorder)
		{
			if (cbEndInterruption is not null)
				cbEndInterruption (recorder, EventArgs.Empty);
		}
#endif
	}

	public partial class AVAudioRecorder {
		InternalAVAudioRecorderDelegate EnsureEventDelegate ()
		{
			var del = WeakDelegate as InternalAVAudioRecorderDelegate;
			if (del is null) {
				del = new InternalAVAudioRecorderDelegate ();
				WeakDelegate = del;
			}
			return del;
		}

		public event EventHandler<AVStatusEventArgs> FinishedRecording {
			add {
				EnsureEventDelegate ().cbFinishedRecording += value;
			}

			remove {
				if (value is not null)
					EnsureEventDelegate ().cbFinishedRecording -= value;
			}
		}

		public event EventHandler<AVErrorEventArgs> EncoderError {
			add {
				EnsureEventDelegate ().cbEncoderError += value;
			}

			remove {
				if (value is not null)
					EnsureEventDelegate ().cbEncoderError -= value;
			}
		}

		public event EventHandler BeginInterruption {
			add {
				EnsureEventDelegate ().cbBeginInterruption += value;
			}

			remove {
				if (value is not null)
					EnsureEventDelegate ().cbBeginInterruption -= value;
			}
		}

		public event EventHandler EndInterruption {
			add {
				EnsureEventDelegate ().cbEndInterruption += value;
			}

			remove {
				if (value is not null)
					EnsureEventDelegate ().cbEndInterruption -= value;
			}
		}
	}
#endif // !TVOS

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class AVSampleRateEventArgs : EventArgs {
		/// <param name="sampleRate">To be added.</param>
		///         <summary>Initializes a new instance of the AVSampleRateEventArgs class.</summary>
		///         <remarks>
		///         </remarks>
		public AVSampleRateEventArgs (double sampleRate)
		{
			SampleRate = sampleRate;
		}
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double SampleRate { get; private set; }
	}

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class AVChannelsEventArgs : EventArgs {
		/// <param name="numberOfChannels">To be added.</param>
		///         <summary>Initializes a new instance of the AVChannelsEventArgs class.</summary>
		///         <remarks>
		///         </remarks>
		public AVChannelsEventArgs (int numberOfChannels)
		{
			NumberOfChannels = numberOfChannels;
		}
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public int NumberOfChannels { get; private set; }
	}

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class AVCategoryEventArgs : EventArgs {
		/// <param name="category">To be added.</param>
		///         <summary>Initializes a new instance of the AVCategoryEventArgs class.</summary>
		///         <remarks>
		///         </remarks>
		public AVCategoryEventArgs (string category)
		{
			Category = category;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string Category { get; private set; }
	}

#if !MONOMAC && !TVOS
	internal class InternalAVAudioSessionDelegate : AVAudioSessionDelegate {
		internal EventHandler? cbEndInterruption;
		internal EventHandler? cbBeginInterruption;
		internal EventHandler<AVCategoryEventArgs>? cbCategoryChanged;
		internal EventHandler<AVStatusEventArgs>? cbInputAvailabilityChanged;
		internal EventHandler<AVSampleRateEventArgs>? cbSampleRateChanged;
		internal EventHandler<AVChannelsEventArgs>? cbInputChanged;
		internal EventHandler<AVChannelsEventArgs>? cbOutputChanged;

		AVAudioSession session;

		[Preserve (Conditional = true)]
		public InternalAVAudioSessionDelegate (AVAudioSession session)
		{
			this.session = session;
		}

		[Preserve (Conditional = true)]
		public override void BeginInterruption ()
		{
			if (cbBeginInterruption is not null)
				cbBeginInterruption (session, EventArgs.Empty);
		}

		[Preserve (Conditional = true)]
		public override void EndInterruption ()
		{
			if (cbEndInterruption is not null)
				cbEndInterruption (session, EventArgs.Empty);
		}

		[Preserve (Conditional = true)]
		public override void InputIsAvailableChanged (bool isInputAvailable)
		{
			if (cbInputAvailabilityChanged is not null)
				cbInputAvailabilityChanged (session, new AVStatusEventArgs (isInputAvailable));
		}

	}

	public partial class AVAudioSession {
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("ios6.0", "Use 'AVAudioSession.Notification.ObserveAudioRouteChange' instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.1", "Use 'AVAudioSession.Notification.ObserveAudioRouteChange' instead.")]
		InternalAVAudioSessionDelegate EnsureEventDelegate ()
		{
			var del = WeakDelegate as InternalAVAudioSessionDelegate;
			if (del is null) {
				del = new InternalAVAudioSessionDelegate (this);
				WeakDelegate = del;
			}
			return del;
		}

		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("ios6.0", "Use 'AVAudioSession.Notification.ObserveInterruption' instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.1", "Use 'AVAudioSession.Notification.ObserveInterruption' instead.")]
		public event EventHandler BeginInterruption {
			add {
				EnsureEventDelegate ().cbBeginInterruption += value;
			}
			remove {
				if (value is not null)
					EnsureEventDelegate ().cbBeginInterruption -= value;
			}
		}

		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("ios6.0", "Use 'AVAudioSession.Notification.ObserveInterruption' instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.1", "Use 'AVAudioSession.Notification.ObserveInterruption' instead.")]
		public event EventHandler EndInterruption {
			add {
				EnsureEventDelegate ().cbEndInterruption += value;
			}
			remove {
				if (value is not null)
					EnsureEventDelegate ().cbBeginInterruption -= value;
			}
		}

		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("ios6.0", "Use 'AVAudioSession.Notification.ObserveAudioRouteChange' instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.1", "Use 'AVAudioSession.Notification.ObserveAudioRouteChange' instead.")]
		public event EventHandler<AVCategoryEventArgs> CategoryChanged {
			add {
				EnsureEventDelegate ().cbCategoryChanged += value;
			}
			remove {
				if (value is not null)
					EnsureEventDelegate ().cbCategoryChanged -= value;
			}
		}

		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("ios6.0", "Use 'AVAudioSession.Notification.ObserveAudioRouteChange' instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.1", "Use 'AVAudioSession.Notification.ObserveAudioRouteChange' instead.")]
		public event EventHandler<AVStatusEventArgs> InputAvailabilityChanged {
			add {
				EnsureEventDelegate ().cbInputAvailabilityChanged += value;
			}
			remove {
				if (value is not null)
					EnsureEventDelegate ().cbInputAvailabilityChanged -= value;
			}
		}

		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("ios6.0", "Use 'AVAudioSession.Notification.ObserveAudioRouteChange' instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.1", "Use 'AVAudioSession.Notification.ObserveAudioRouteChange' instead.")]
		public event EventHandler<AVSampleRateEventArgs> SampleRateChanged {
			add {
				EnsureEventDelegate ().cbSampleRateChanged += value;
			}

			remove {
				if (value is not null)
					EnsureEventDelegate ().cbSampleRateChanged -= value;
			}
		}

		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("ios6.0", "Use 'AVAudioSession.Notification.ObserveAudioRouteChange' instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.1", "Use 'AVAudioSession.Notification.ObserveAudioRouteChange' instead.")]
		public event EventHandler<AVChannelsEventArgs> InputChannelsChanged {
			add {
				EnsureEventDelegate ().cbInputChanged += value;
			}

			remove {
				if (value is not null)
					EnsureEventDelegate ().cbInputChanged += value;
			}
		}

		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("ios6.0", "Use 'AVAudioSession.Notification.ObserveAudioRouteChange' instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.1", "Use 'AVAudioSession.Notification.ObserveAudioRouteChange' instead.")]
		public event EventHandler<AVChannelsEventArgs> OutputChannelsChanged {
			add {
				EnsureEventDelegate ().cbOutputChanged += value;
			}

			remove {
				if (value is not null)
					EnsureEventDelegate ().cbOutputChanged -= value;
			}
		}
	}
#endif
}
