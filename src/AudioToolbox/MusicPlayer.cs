//
// MusicPlayer.cs: Bindings to the AudioToolbox's MusicPlayers APIs
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2012-2014 Xamarin Inc.
//
//

#nullable enable

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using ObjCRuntime;
using Foundation;

namespace AudioToolbox {

	// untyped enum (used as an OSStatus in the API) -> MusicPlayer.h
	public enum MusicPlayerStatus {
		/// <summary>To be added.</summary>
		Success = 0,
		/// <summary>To be added.</summary>
		InvalidSequenceType = -10846,
		/// <summary>To be added.</summary>
		TrackIndexError = -10859,
		/// <summary>To be added.</summary>
		TrackNotFound = -10858,
		/// <summary>To be added.</summary>
		EndOfTrack = -10857,
		/// <summary>To be added.</summary>
		StartOfTrack = -10856,
		/// <summary>To be added.</summary>
		IllegalTrackDestination = -10855,
		/// <summary>To be added.</summary>
		NoSequence = -10854,
		/// <summary>To be added.</summary>
		InvalidEventType = -10853,
		/// <summary>To be added.</summary>
		InvalidPlayerState = -10852,
		/// <summary>To be added.</summary>
		CannotDoInCurrentContext = -10863,
		/// <summary>To be added.</summary>
		NoTrackDestination = -66720,
	}

	// typedef UInt32 -> MusicPlayer.h
	public enum MusicEventType : uint {
		/// <summary>To be added.</summary>
		Null,
		/// <summary>To be added.</summary>
		ExtendedNote = 1,
		/// <summary>To be added.</summary>
		ExtendedTempo = 3,
		/// <summary>To be added.</summary>
		User = 4,
		/// <summary>To be added.</summary>
		Meta = 5,
		/// <summary>To be added.</summary>
		MidiNoteMessage = 6,
		/// <summary>To be added.</summary>
		MidiChannelMessage = 7,
		/// <summary>To be added.</summary>
		MidiRawData = 8,
		/// <summary>To be added.</summary>
		Parameter = 9,
		/// <summary>To be added.</summary>
		AUPreset = 10,
	}

	// typedef UInt32 -> MusicPlayer.h
	[Flags]
	public enum MusicSequenceLoadFlags {
		/// <summary>Indicates that the input tracks will be preserved in the output.</summary>
		PreserveTracks = 0,
		/// <summary>Indicates that the output will contain a tempo track, a track for each SMF MIDI channel, and a track for SysEx or MetaEvents.</summary>
		ChannelsToTracks = 1 << 0,
	}

	// typedef UInt32 -> MusicPlayer.h
	public enum MusicSequenceFileTypeID : uint {
		/// <summary>Indicates that the type is not specified.</summary>
		Any = 0,
		/// <summary>Indicates a MIDI file.</summary>
		Midi = 0x6d696469, // 'midi'
		/// <summary>Indicates an iMelody file.</summary>
		iMelody = 0x696d656c, // 'imel'
	}

	// typedef UInt32 -> MusicPlayer.h
	[Flags]
	public enum MusicSequenceFileFlags {
		/// <summary>Indicates that the existing file should not be erased.</summary>
		Default = 0,
		/// <summary>Indicates that the existing file should be erased.</summary>
		EraseFile = 1,
	}


	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	// MusicPlayer.h
	public class MusicPlayer : DisposableObject {
		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static /* OSStatus */ MusicPlayerStatus NewMusicPlayer (/* MusicPlayer* */ IntPtr* outPlayer);

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static /* OSStatus */ MusicPlayerStatus DisposeMusicPlayer (/* MusicPlayer */ IntPtr inPlayer);

		[Preserve (Conditional = true)]
		MusicPlayer (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		protected override void Dispose (bool disposing)
		{
			currentSequence = null;
			if (Owns && Handle != IntPtr.Zero)
				DisposeMusicPlayer (Handle);
			base.Dispose (disposing);
		}

		unsafe static IntPtr Create ()
		{
			IntPtr handle;
			var result = NewMusicPlayer (&handle);
			if (result == MusicPlayerStatus.Success)
				return handle;
			throw new Exception ("Unable to create MusicPlayer: " + result);
		}

		public MusicPlayer ()
			: base (Create (), true)
		{
		}

		static public MusicPlayer? Create (out MusicPlayerStatus OSstatus)
		{
			IntPtr handle;
			unsafe {
				OSstatus = NewMusicPlayer (&handle);
			}
			if (OSstatus == 0)
				return new MusicPlayer (handle, true);
			return null;
		}

		MusicSequence? currentSequence;
#if !COREBUILD
		// note: MusicTimeStamp -> Float64

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static /* OSStatus */ MusicPlayerStatus MusicPlayerGetTime (/* MusicPlayer */ IntPtr inPlayer, /* MusicTimeStamp* */ double* outTime);

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static /* OSStatus */ MusicPlayerStatus MusicPlayerSetTime (/* MusicPlayer */ IntPtr inPlayer, /* MusicTimeStamp* */ double inTime);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double Time {
			get {
				double time;
				unsafe {
					MusicPlayerGetTime (Handle, &time);
				}
				return time;
			}
			set {
				MusicPlayerSetTime (Handle, value);
			}
		}

		public MusicPlayerStatus GetTime (out double time)
		{
			time = 0;
			unsafe {
				return MusicPlayerGetTime (Handle, (double*) Unsafe.AsPointer<double> (ref time));
			}
		}

		public MusicPlayerStatus SetTime (double time)
		{
			return MusicPlayerSetTime (Handle, time);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static /* OSStatus */ MusicPlayerStatus MusicPlayerPreroll (/* MusicPlayer */ IntPtr inPlayer);

		public MusicPlayerStatus Preroll ()
		{
			return MusicPlayerPreroll (Handle);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static /* OSStatus */ MusicPlayerStatus MusicPlayerStart (/* MusicPlayer */ IntPtr inPlayer);

		public MusicPlayerStatus Start ()
		{
			return MusicPlayerStart (Handle);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static /* OSStatus */ MusicPlayerStatus MusicPlayerStop (/* MusicPlayer */ IntPtr inPlayer);

		public MusicPlayerStatus Stop ()
		{
			return MusicPlayerStop (Handle);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static /* OSStatus */ MusicPlayerStatus MusicPlayerIsPlaying (/* MusicPlayer */ IntPtr inPlayer, /* Boolean* */ byte* outIsPlaying);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsPlaying {
			get {
				byte res;
				unsafe {
					MusicPlayerIsPlaying (Handle, &res);
				}
				return res != 0;
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static /* OSStatus */ MusicPlayerStatus MusicPlayerSetPlayRateScalar (/* MusicPlayer */ IntPtr inPlayer, /* Float64 */ double inScaleRate);

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static /* OSStatus */ MusicPlayerStatus MusicPlayerGetPlayRateScalar (/* MusicPlayer */ IntPtr inPlayer, /* Float64* */ double* outScaleRate);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double PlayRateScalar {
			get {
				double rate;
				unsafe {
					MusicPlayerGetPlayRateScalar (Handle, &rate);
				}
				return rate;
			}
			set {
				MusicPlayerSetPlayRateScalar (Handle, value);
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static /* OSStatus */ MusicPlayerStatus MusicPlayerGetHostTimeForBeats (/* MusicPlayer */ IntPtr inPlayer, /* MusicTimeStamp */ double inBeats, /* UInt64* */ long* outHostTime);

		public MusicPlayerStatus GetHostTimeForBeats (double beats, out long hostTime)
		{
			hostTime = 0;
			unsafe {
				return MusicPlayerGetHostTimeForBeats (Handle, beats, (long*) Unsafe.AsPointer<long> (ref hostTime));
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static /* OSStatus */ MusicPlayerStatus MusicPlayerGetBeatsForHostTime (/* MusicPlayer */ IntPtr inPlayer, /* UInt64 */ long inHostTime, /* MusicTimeStamp* */ double* outBeats);

		public MusicPlayerStatus GetBeatsForHostTime (long hostTime, out double beats)
		{
			beats = 0;
			unsafe {
				return MusicPlayerGetBeatsForHostTime (Handle, hostTime, (double*) Unsafe.AsPointer<double> (ref beats));
			}
		}


		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static /* OSStatus */ MusicPlayerStatus MusicPlayerGetSequence (/* MusicPlayer */ IntPtr inPlayer, /* MusicSequence* */ IntPtr* outSequence);

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static /* OSStatus */ MusicPlayerStatus MusicPlayerSetSequence (/* MusicPlayer */ IntPtr inPlayer, IntPtr inSequence);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public MusicSequence? MusicSequence {
			get {
				IntPtr seqHandle;
				unsafe {
					if (MusicPlayerGetSequence (Handle, &seqHandle) == MusicPlayerStatus.Success)
						return MusicSequence.Lookup (seqHandle);
				}

				return null;
			}
			set {
				currentSequence = value;
				MusicPlayerSetSequence (Handle, value.GetHandle ());
				GC.KeepAlive (value);
			}
		}
#endif
	}
}
