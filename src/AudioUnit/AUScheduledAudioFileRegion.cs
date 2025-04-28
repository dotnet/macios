// 
// ScheduledAudioFileRegion.cs: A wrapper class around ScheduledAudioFileRegionProxy struct
//
// Authors:
//    Rustam Zaitov (rustam.zaitov@xamarin.com)
//    Alex Soto (alex.soto@xamarin.com)
// 
// Copyright 2015 Xamarin Inc.
//

#nullable enable

using System;
using System.Runtime.InteropServices;

using AudioToolbox;
using ObjCRuntime;
using System.Runtime.Versioning;

namespace AudioUnit {

	/// <param name="audioFileRegion">To be added.</param>
	///     <param name="status">To be added.</param>
	///     <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	public delegate void AUScheduledAudioFileRegionCompletionHandler (AUScheduledAudioFileRegion audioFileRegion, AudioUnitStatus status);

#if NET
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
#endif
	public class AUScheduledAudioFileRegion : IDisposable {

		[StructLayout (LayoutKind.Sequential)]
		internal struct ScheduledAudioFileRegion {
			public AudioTimeStamp TimeStamp;
#if NET
			public unsafe delegate* unmanaged<IntPtr, IntPtr, AudioUnitStatus, void> CompletionHandler;
#else
			public IntPtr CompletionHandler;
#endif
			public /* void * */ IntPtr CompletionHandlerUserData;
			public IntPtr AudioFile;
			public uint LoopCount;
			public long StartFrame;
			public uint FramesToPlay;
		}

		GCHandle handle;
		AUScheduledAudioFileRegionCompletionHandler? completionHandler;
		bool alreadyUsed = false;

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioTimeStamp TimeStamp { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioFile AudioFile { get; private set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public uint LoopCount { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public long StartFrame { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public uint FramesToPlay { get; set; }

		/// <param name="audioFile">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public AUScheduledAudioFileRegion (AudioFile audioFile, AUScheduledAudioFileRegionCompletionHandler? completionHandler = null)
		{
			if (audioFile is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (audioFile));

			AudioFile = audioFile;
			this.completionHandler = completionHandler;
		}

#if !NET
		internal delegate void ScheduledAudioFileRegionCompletionHandler (
			/* void * */IntPtr userData,
			/* ScheduledAudioFileRegion * */ IntPtr fileRegion,
			/* OSStatus */ AudioUnitStatus result);

		static readonly ScheduledAudioFileRegionCompletionHandler static_ScheduledAudioFileRegionCompletionHandler = new ScheduledAudioFileRegionCompletionHandler (ScheduledAudioFileRegionCallback);

#if !MONOMAC
		[MonoPInvokeCallback (typeof (ScheduledAudioFileRegionCompletionHandler))]
#endif
#else
		[UnmanagedCallersOnly]
#endif
		static void ScheduledAudioFileRegionCallback (IntPtr userData, IntPtr fileRegion, AudioUnitStatus status)
		{
			if (userData == IntPtr.Zero)
				return;

			var handle = GCHandle.FromIntPtr (userData);
			var inst = (AUScheduledAudioFileRegion?) handle.Target;
			if (inst?.completionHandler is not null)
				inst.completionHandler (inst, status);
		}

		internal ScheduledAudioFileRegion GetAudioFileRegion ()
		{
			if (alreadyUsed)
				throw new InvalidOperationException ("You should not call SetScheduledFileRegion with a previously set region instance");

			IntPtr ptr = IntPtr.Zero;
			if (completionHandler is not null) {
				handle = GCHandle.Alloc (this);
				ptr = GCHandle.ToIntPtr (handle);
			}

			var ret = new ScheduledAudioFileRegion {
				TimeStamp = TimeStamp,
				CompletionHandlerUserData = ptr,
				AudioFile = AudioFile.Handle,
				LoopCount = LoopCount,
				StartFrame = StartFrame,
				FramesToPlay = FramesToPlay,
			};

			if (ptr != IntPtr.Zero) {
				unsafe {
#if NET
					ret.CompletionHandler = &ScheduledAudioFileRegionCallback;
#else
					ret.CompletionHandler = Marshal.GetFunctionPointerForDelegate (static_ScheduledAudioFileRegionCompletionHandler);
#endif
				}
			}

			alreadyUsed = true;
			return ret;
		}

		~AUScheduledAudioFileRegion ()
		{
			Dispose (false);
		}

		/// <summary>Releases the resources used by the AUScheduledAudioFileRegion object.</summary>
		///         <remarks>
		///           <para>The Dispose method releases the resources used by the AUScheduledAudioFileRegion class.</para>
		///           <para>Calling the Dispose method when the application is finished using the AUScheduledAudioFileRegion ensures that all external resources used by this managed object are released as soon as possible.  Once developers have invoked the Dispose method, the object is no longer useful and developers should no longer make any calls to it.  For more information on releasing resources see ``Cleaning up Unmananaged Resources'' at https://msdn.microsoft.com/en-us/library/498928w2.aspx</para>
		///         </remarks>
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		/// <include file="../../docs/api/AudioUnit/AUScheduledAudioFileRegion.xml" path="/Documentation/Docs[@DocId='M:AudioUnit.AUScheduledAudioFileRegion.Dispose(System.Boolean)']/*" />
		protected virtual void Dispose (bool disposing)
		{
			if (disposing)
				completionHandler = null;

			if (handle.IsAllocated)
				handle.Free ();
		}
	}
}
