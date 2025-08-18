// 
// AudioFile.cs:
//
// Authors:
//    Miguel de Icaza (miguel@xamarin.com)
//    Marek Safar (marek.safar@gmail.com)
//     
// Copyright 2009 Novell, Inc
// Copyright 2011-2013 Xamarin Inc.
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

#nullable enable

using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using ObjCRuntime;
using CoreFoundation;
using Foundation;

using AudioFileID = System.IntPtr;

namespace AudioToolbox {

	/// <summary>Known audio file types.   Used to specify the kind of audio file to create, or as a hint to the audio parser about the contents of the file.</summary>
	///     <remarks>To be added.</remarks>
	public enum AudioFileType {  // UInt32 AudioFileTypeID
		/// <summary>Audio Interchange File Format.</summary>
		AIFF = 0x41494646, // AIFF
		/// <summary>Compressed Audio Interchange File Format.</summary>
		AIFC = 0x41494643, // AIFC
		/// <summary>Microsoft WAVE format.</summary>
		WAVE = 0x57415645, // WAVE
		/// <summary>BWF-compatible RF64 multichannel sound format.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		RF64 = 0x52463634, // RF64
		/// <summary>Sound Designer 2 file.</summary>
		SoundDesigner2 = 0x53643266, // Sd2f
		/// <summary>NeXT/Sun audio file format</summary>
		Next = 0x4e655854, // NeXT
		/// <summary>MPEG-1 Audio Layer 3.</summary>
		MP3 = 0x4d504733, // MPG3
		/// <summary>MPEG-1 Audio Layer 2.</summary>
		MP2 = 0x4d504732, // MPG2
		/// <summary>MPEG-1 Audio Layer 1, largely outdated</summary>
		MP1 = 0x4d504731, // MPG1
		/// <summary>Digital Audio Compression Standard (also known as Dolby Digital or Audio Codec 3)</summary>
		AC3 = 0x61632d33, // ac-3
		/// <summary>Audio Transport Stream, a contains for Advanced Audio Coding (AAC) data.</summary>
		AAC_ADTS = 0x61647473, // adts
		/// <summary>MPEG-4 file.</summary>
		MPEG4 = 0x6d703466, // mp4f
		/// <summary>MPEG-4 Audio Layer with no bookmark metadata (use M4B for that).</summary>
		M4A = 0x6d346166, // m4af
		/// <summary>MPEG-4 Audio Layer with metadata for bookmarks, chapter markers, images and hyperlinks.</summary>
		M4B = 0x6d346266, // m4bf
		/// <summary>Apple Core Audio Format.   CAF files are containers that can contain multiple audio formats, metadata tracks.   Uses 64-bit offsetes, so it is not limited to 4GB.</summary>
		CAF = 0x63616666, // caff
		/// <summary>3GP (3GPP file format) is a multimedia container format defined by the Third Generation Partnership Project (3GPP) for 3G UMTS multimedia services. It is used on 3G mobile phones but can also be played on some 2G and 4G phones.</summary>
		ThreeGP = 0x33677070, // 3gpp
		/// <summary>3G2 (3GPP2 file format) is a multimedia container format defined by the 3GPP2 for 3G CDMA2000 multimedia services. It is very similar to the 3GP file format, but has some extensions and limitations in comparison to 3GP.</summary>
		ThreeGP2 = 0x33677032, // 3gp2
		/// <summary>Adaptive Multi-Rate format, optimized for speech coding   Used widely in GSM an UMTS.</summary>
		AMR = 0x616d7266, // amrf
		/// <summary>Free Lossless Audio Codec format.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		FLAC = 0x666c6163, // flac
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		LatmInLoas = 0x6c6f6173, // loas
	}

	/// <summary>The error codes returned by <see cref="AudioToolbox.AudioFile" />.</summary>
	///     <remarks>
	///     </remarks>
	public enum AudioFileError {// Implictly cast to OSType in AudioFile.h
		/// <summary>To be added.</summary>
		Success = 0, // noErr
		/// <summary>An unspecified error has occurred.</summary>
		Unspecified = 0x7768743f, // wht?
		/// <summary>The file type is not supported.</summary>
		UnsupportedFileType = 0x7479703f, // typ?
		/// <summary>The data format is not supported.</summary>
		UnsupportedDataFormat = 0x666d743f, // fmt?
		/// <summary>The property is not supported.</summary>
		UnsupportedProperty = 0x7074793f, // pty?
		/// <summary>The size of the property data was invalid.</summary>
		BadPropertySize = 0x2173697a, // !siz
		/// <summary>To be added.</summary>
		Permissions = 0x70726d3f, // prm?
		/// <summary>The file must be optimized in order to write more audio data.</summary>
		NotOptimized = 0x6f70746d, // optm
		/// <summary>The chunk does not exist or is not supported by the file.</summary>
		InvalidChunk = 0x63686b3f, // chk?
		/// <summary>The a file offset was too large for the file type.</summary>
		DoesNotAllow64BitDataSize = 0x6f66663f, // off?
		/// <summary>A packet offset is not valid.</summary>
		InvalidPacketOffset = 0x70636b3f, // pck?
		/// <summary>The file is invalid.</summary>
		InvalidFile = 0x6474613f, // dta?
		/// <summary>To be added.</summary>
		OperationNotSupported = 0x6F703F3F, // op??
		/// <summary>The file is not opened.</summary>
		FileNotOpen = -38,
		/// <summary>The end of file.</summary>
		EndOfFile = -39,
		/// <summary>File not found.</summary>
		FileNotFound = -43,
		/// <summary>Invalid file position.</summary>
		FilePosition = -40,
	}

	/// <summary>An enumeration whose values specify the <c>permissions</c> argument when opening an <see cref="AudioToolbox.AudioFile" />.</summary>
	///     <remarks>To be added.</remarks>
	[Flags]
	public enum AudioFilePermission {
		/// <summary>To be added.</summary>
		Read = 0x01,
		/// <summary>To be added.</summary>
		Write = 0x02,
		/// <summary>To be added.</summary>
		ReadWrite = 0x03,
	}

	/// <summary>An enumeration whose values to select creation options for <see cref="AudioToolbox.AudioFile" />.</summary>
	///     <remarks>To be added.</remarks>
	[Flags]
	public enum AudioFileFlags { // UInt32 in AudioFileCreateWithURL()
		/// <summary>To be added.</summary>
		EraseFlags = 1,
		/// <summary>If this flag is set, audio data will be written without page alignment. This will make the data more compact but possibly slow readout.</summary>
		DontPageAlignAudioData = 2,
	}

	/// <summary>An enumeration whose values represent information about a <see cref="AudioToolbox.AudioFile" />. See the <see cref="AudioToolbox.AudioFileStream.GetProperty(AudioToolbox.AudioFileStreamProperty,ref System.Int32,System.IntPtr)" /> and <see cref="AudioToolbox.AudioFile.SetProperty(AudioToolbox.AudioFileProperty,System.Int32,System.IntPtr)" /> methods.</summary>
	///     <remarks>To be added.</remarks>
	public enum AudioFileProperty { // typedef UInt32 AudioFilePropertyID
		/// <summary>To be added.</summary>
		FileFormat = 0x66666d74,
		/// <summary>To be added.</summary>
		DataFormat = 0x64666d74,
		/// <summary>To be added.</summary>
		IsOptimized = 0x6f70746d,
		/// <summary>To be added.</summary>
		MagicCookieData = 0x6d676963,
		/// <summary>To be added.</summary>
		AudioDataByteCount = 0x62636e74,
		/// <summary>To be added.</summary>
		AudioDataPacketCount = 0x70636e74,
		/// <summary>To be added.</summary>
		MaximumPacketSize = 0x70737a65,
		/// <summary>To be added.</summary>
		DataOffset = 0x646f6666,
		/// <summary>To be added.</summary>
		ChannelLayout = 0x636d6170,
		/// <summary>To be added.</summary>
		DeferSizeUpdates = 0x64737a75,
		/// <summary>To be added.</summary>
		DataFormatName = 0x666e6d65,
		/// <summary>To be added.</summary>
		MarkerList = 0x6d6b6c73,
		/// <summary>To be added.</summary>
		RegionList = 0x72676c73,
		/// <summary>To be added.</summary>
		PacketToFrame = 0x706b6672,
		/// <summary>To be added.</summary>
		FrameToPacket = 0x6672706b,
		/// <summary>To be added.</summary>
		PacketToByte = 0x706b6279,
		/// <summary>To be added.</summary>
		ByteToPacket = 0x6279706b,
		/// <summary>To be added.</summary>
		ChunkIDs = 0x63686964,
		/// <summary>To be added.</summary>
		InfoDictionary = 0x696e666f,
		/// <summary>To be added.</summary>
		PacketTableInfo = 0x706e666f,
		/// <summary>To be added.</summary>
		FormatList = 0x666c7374,
		/// <summary>To be added.</summary>
		PacketSizeUpperBound = 0x706b7562,
		/// <summary>To be added.</summary>
		ReserveDuration = 0x72737276,
		/// <summary>To be added.</summary>
		EstimatedDuration = 0x65647572,
		/// <summary>To be added.</summary>
		BitRate = 0x62726174,
		/// <summary>To be added.</summary>
		ID3Tag = 0x69643374,
		/// <summary>To be added.</summary>
		SourceBitDepth = 0x73627464,
		/// <summary>To be added.</summary>
		AlbumArtwork = 0x61617274,
		/// <summary>To be added.</summary>
		ReadyToProducePackets = 0x72656479,
		/// <summary>The average number of bytes per audio packet.</summary>
		AverageBytesPerPacket = 0x61627070,
		/// <summary>To be added.</summary>
		AudioTrackCount = 0x61746374,
		/// <summary>To be added.</summary>
		UseAudioTrack = 0x7561746b,
	}

	/// <summary>An enumeration whose values specify an audio-loop's direction.</summary>
	///     <remarks>To be added.</remarks>
	public enum AudioFileLoopDirection { // Unused?
		/// <summary>To be added.</summary>
		NoLooping = 0,
		/// <summary>To be added.</summary>
		Forward = 1,
		/// <summary>To be added.</summary>
		ForwardAndBackward = 2,
		/// <summary>To be added.</summary>
		Backward = 3,
	}

	/// <summary>An enumeration whose values specify different types of chunks appropriate to audio files.</summary>
	///     <remarks>To be added.</remarks>
	public enum AudioFileChunkType : uint // CoreAudio.framework - CoreAudioTypes.h - "four char code IDs"
	{
		/// <summary>To be added.</summary>
		CAFStreamDescription = 0x64657363,  // 'desc'
		/// <summary>To be added.</summary>
		CAFAudioData = 0x64617461,  // 'data'
		/// <summary>To be added.</summary>
		CAFChannelLayout = 0x6368616e,  // 'chan'
		/// <summary>To be added.</summary>
		CAFFiller = 0x66726565, // 'free'
		/// <summary>To be added.</summary>
		CAFMarker = 0x6d61726b, // 'mark'
		/// <summary>To be added.</summary>
		CAFRegion = 0x7265676e, // 'regn'
		/// <summary>To be added.</summary>
		CAFInstrument = 0x696e7374, // 'inst'
		/// <summary>To be added.</summary>
		CAFMagicCookieID = 0x6b756b69,  // 'kuki'
		/// <summary>To be added.</summary>
		CAFInfoStrings = 0x696e666f,    // 'info'
		/// <summary>To be added.</summary>
		CAFEditComments = 0x65646374,   // 'edct'
		/// <summary>To be added.</summary>
		CAFPacketTable = 0x70616b74,    // 'pakt'
		/// <summary>To be added.</summary>
		CAFStrings = 0x73747267,    // 'strg'
		/// <summary>To be added.</summary>
		CAFUUID = 0x75756964,   // 'uuid'
		/// <summary>To be added.</summary>
		CAFPeak = 0x7065616b,   // 'peak'
		/// <summary>To be added.</summary>
		CAFOverview = 0x6f767677,   // 'ovvw'
		/// <summary>To be added.</summary>
		CAFMIDI = 0x6d696469,   // 'midi'
		/// <summary>To be added.</summary>
		CAFUMID = 0x756d6964,   // 'umid'
		/// <summary>To be added.</summary>
		CAFFormatListID = 0x6c647363,   // 'ldsc'
		/// <summary>To be added.</summary>
		CAFiXML = 0x69584d4c,   // 'iXML'
	}

	[StructLayout (LayoutKind.Sequential)]
	struct AudioFramePacketTranslation {
		public long Frame;
		public long Packet;
		public int FrameOffsetInPacket;
	}

	[StructLayout (LayoutKind.Sequential)]
	struct AudioBytePacketTranslation {
		public long Byte;
		public long Packet;
		public int ByteOffsetInPacket;
		public BytePacketTranslationFlags Flags;
	}

	[Flags]
	enum BytePacketTranslationFlags : uint  // Stored in UInt32 in AudioBytePacketTranslation - AudioFile.h
	{
		IsEstimate = 1,
	}

	/// <summary>A struct that encapsulates a Society of Motion Picture and Television Engineers time.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AudioFileSmpteTime { // AudioFile_SMPTE_Time
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public sbyte Hours;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public byte Minutes;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public byte Seconds;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public byte Frames;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public uint SubFrameSampleOffset;
	}

	/// <summary>A class that represents a specific named position within an audio file.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AudioFileMarker {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double FramePosition;
		internal IntPtr Name_cfstringref;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public int MarkerID;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public AudioFileSmpteTime SmpteTime;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public AudioFileMarkerType Type;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public ushort Reserved;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public ushort Channel;

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Name {
			get {
				return CFString.FromHandle (Name_cfstringref);
			}
		}
	}

	[SupportedOSPlatform ("ios13.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos13.0")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AudioPacketRangeByteCountTranslation {
		public long Packet;
		public long PacketCount;
		public long ByteCountUpperBound;
	}

	[SupportedOSPlatform ("ios13.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos13.0")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AudioPacketRollDistanceTranslation {
		public long Packet;
		public long RollDistance;
	}

	[SupportedOSPlatform ("ios13.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos13.0")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AudioIndependentPacketTranslation {
		public long Packet;
		public long IndependentlyDecodablePacket;
	}

	[SupportedOSPlatform ("ios13.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos13.0")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AudioPacketDependencyInfoTranslation {
		public long Packet;
		uint isIndependentlyDecodable;
		public uint NumberPrerollPackets;
		public bool IsIndependentlyDecodable {
			get { return isIndependentlyDecodable != 0; }
			set { isIndependentlyDecodable = (value) ? 1U : 0U; }
		}
	}

	/// <summary>An enumeration whose values specify the <see cref="AudioFileMarker.Type" /> property.</summary>
	///     <remarks>To be added.</remarks>
	public enum AudioFileMarkerType : uint // UInt32 in AudioFileMarkerType - AudioFile.h
	{
		/// <summary>To be added.</summary>
		Generic = 0,

		/// <summary>To be added.</summary>
		CAFProgramStart = 0x70626567,   // 'pbeg'
		/// <summary>To be added.</summary>
		CAFProgramEnd = 0x70656e64, // 'pend'
		/// <summary>To be added.</summary>
		CAFTrackStart = 0x74626567, // 'tbeg'
		/// <summary>To be added.</summary>
		CAFTrackEnd = 0x74656e54,   // 'tend'
		/// <summary>To be added.</summary>
		CAFIndex = 0x696e6478,  // 'indx'
		/// <summary>To be added.</summary>
		CAFRegionStart = 0x72626567,    // 'rbeg'
		/// <summary>To be added.</summary>
		CAFRegionEnd = 0x72626567,  // 'rend'
		/// <summary>To be added.</summary>
		CAFRegionSyncPoint = 0x72737963,    // 'rsyc'
		/// <summary>To be added.</summary>
		CAFSelectionStart = 0x73626567, // 'sbeg'
		/// <summary>To be added.</summary>
		CAFSelectionEnd = 0x73626567,   // 'send'
		/// <summary>To be added.</summary>
		CAFEditSourceBegin = 0x63626567,    // 'cbeg'
		/// <summary>To be added.</summary>
		CAFEditSourceEnd = 0x63626567,  // 'cend'
		/// <summary>To be added.</summary>
		CAFEditDestinationBegin = 0x64626567,   // 'dbeg'
		/// <summary>To be added.</summary>
		CAFEditDestinationEnd = 0x64626567, // 'dend'
		/// <summary>To be added.</summary>
		CAFSustainLoopStart = 0x736c6267,   // 'slbg'
		/// <summary>To be added.</summary>
		CAFSustainLoopEnd = 0x736c6265, // 'slen'
		/// <summary>To be added.</summary>
		CAFReleaseLoopStart = 0x726c6267,   // 'rlbg'
		/// <summary>To be added.</summary>
		CAFReleaseLoopEnd = 0x726c6265, // 'rlen'
		/// <summary>To be added.</summary>
		CAFSavedPlayPosition = 0x73706c79,  // 'sply'
		/// <summary>To be added.</summary>
		CAFTempo = 0x746d706f,  // 'tmpo'
		/// <summary>To be added.</summary>
		CAFTimeSignature = 0x74736967,  // 'tsig'
		/// <summary>To be added.</summary>
		CAFKeySignature = 0x6b736967,   // 'ksig'
	}

	/// <summary>A collection of <see cref="AudioToolbox.AudioFileMarker" />s.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class AudioFileMarkerList : IDisposable {
		IntPtr ptr;
		readonly bool owns;

		/// <param name="ptr">To be added.</param>
		///         <param name="owns">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public AudioFileMarkerList (IntPtr ptr, bool owns)
		{
			this.ptr = ptr;
			this.owns = owns;
		}

		~AudioFileMarkerList ()
		{
			Dispose (false);
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SmpteTimeType SmpteTimeType {
			get {
				return (SmpteTimeType) Marshal.ReadInt32 (ptr);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public uint Count {
			get {
				return (uint) Marshal.ReadInt32 (ptr, 4);
			}
		}

		public AudioFileMarker this [int index] {
			get {
				if (index >= Count || index < 0)
					throw new ArgumentOutOfRangeException (nameof (index));

				//
				// Decodes
				//
				// struct AudioFileMarkerList
				// {
				//	UInt32				mSMPTE_TimeType;
				//	UInt32				mNumberMarkers;
				//	AudioFileMarker		mMarkers[1]; // this is a variable length array of mNumberMarkers elements
				// }
				//
				unsafe {
					var ptr = (AudioFileMarker*) this.ptr + 2 * sizeof (int) + index * sizeof (AudioFileMarker);
					return *ptr;
				}
			}
		}

		/// <summary>Releases the resources used by the AudioFileMarkerList object.</summary>
		///         <remarks>
		///           <para>The Dispose method releases the resources used by the AudioFileMarkerList class.</para>
		///           <para>Calling the Dispose method when the application is finished using the AudioFileMarkerList ensures that all external resources used by this managed object are released as soon as possible.  Once developers have invoked the Dispose method, the object is no longer useful and developers should no longer make any calls to it.  For more information on releasing resources see ``Cleaning up Unmananaged Resources'' at https://msdn.microsoft.com/en-us/library/498928w2.aspx</para>
		///         </remarks>
		public void Dispose ()
		{
			Dispose (true);
		}

		/// <include file="../../docs/api/AudioToolbox/AudioFileMarkerList.xml" path="/Documentation/Docs[@DocId='M:AudioToolbox.AudioFileMarkerList.Dispose(System.Boolean)']/*" />
		protected virtual void Dispose (bool disposing)
		{
			if (!owns || ptr == IntPtr.Zero)
				return;

			for (int i = 0; i < Count; ++i) {
				CFObject.CFRelease (this [i].Name_cfstringref);
			}

			Marshal.FreeHGlobal (ptr);
			ptr = IntPtr.Zero;
			GC.SuppressFinalize (this);
		}
	}

	/// <summary>Represents the number of valid frames in a file and where they begin or end.</summary>
	///     <remarks>Not all audio file data formats guarantee that their contents are 100% valid; some have priming or remainder frames. This class can be used with such data formats to identify the valid frames in a file.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AudioFilePacketTableInfo {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public long ValidFrames;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public int PrimingFrames;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public int RemainderFrames;
	}

	/// <summary>Represents a named region within an audio file.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AudioFileRegion {
		readonly IntPtr ptr;
		//
		// Wraps
		//
		// struct AudioFileRegion
		// {
		//	UInt32				mRegionID;
		//	CFStringRef			mName;
		//	UInt32				mFlags;
		//	UInt32				mNumberMarkers;
		//	AudioFileMarker		mMarkers[1]; // this is a variable length array of mNumberMarkers elements
		// }

		/// <param name="ptr">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public AudioFileRegion (IntPtr ptr)
		{
			this.ptr = ptr;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public uint RegionID {
			get {
				return (uint) Marshal.ReadInt32 (ptr);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Name {
			get {
				return CFString.FromHandle (NameWeak);
			}
		}

		internal IntPtr NameWeak {
			get {
				return Marshal.ReadIntPtr (ptr, sizeof (uint));
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public unsafe AudioFileRegionFlags Flags {
			get {
				return (AudioFileRegionFlags) Marshal.ReadInt32 (ptr, sizeof (uint) + sizeof (IntPtr));
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public unsafe int Count {
			get {
				return Marshal.ReadInt32 (ptr, 2 * sizeof (uint) + sizeof (IntPtr));
			}
		}

		public AudioFileMarker this [int index] {
			get {
				if (index >= Count || index < 0)
					throw new ArgumentOutOfRangeException (nameof (index));

				unsafe {
					var ptr = (AudioFileMarker*) this.ptr + 3 * sizeof (int) + sizeof (IntPtr) + index * sizeof (AudioFileMarker);
					return *ptr;
				}
			}
		}

		internal unsafe int TotalSize {
			get {
				return Count * sizeof (AudioFileMarker);
			}
		}
	}

	/// <summary>A flagging enumeration whose values are used in the <see cref="AudioToolbox.AudioFileRegion.Flags" /> property.</summary>
	///     <remarks>To be added.</remarks>
	[Flags]
	public enum AudioFileRegionFlags : uint // UInt32 in AudioFileRegion
	{
		/// <summary>In conjunction with at least one other flag, loops the region.</summary>
		LoopEnable = 1,
		/// <summary>The region is played normally.</summary>
		PlayForward = 2,
		/// <summary>The region is played in reverse.</summary>
		PlayBackward = 4,
	}

	/// <summary>A list of <see cref="AudioToolbox.AudioFileRegion" />s.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class AudioFileRegionList : IDisposable {
		IntPtr ptr;
		readonly bool owns;

		/// <param name="ptr">To be added.</param>
		///         <param name="owns">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public AudioFileRegionList (IntPtr ptr, bool owns)
		{
			this.ptr = ptr;
			this.owns = owns;
		}

		~AudioFileRegionList ()
		{
			Dispose (false);
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SmpteTimeType SmpteTimeType {
			get {
				return (SmpteTimeType) Marshal.ReadInt32 (ptr);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public uint Count {
			get {
				return (uint) Marshal.ReadInt32 (ptr, sizeof (uint));
			}
		}

		public AudioFileRegion this [int index] {
			get {
				if (index >= Count || index < 0)
					throw new ArgumentOutOfRangeException (nameof (index));

				//
				// Decodes
				//
				// struct AudioFileRegionList
				// {
				//	UInt32				mSMPTE_TimeType;
				//	UInt32				mNumberRegions;
				//	AudioFileRegion		mRegions[1]; // this is a variable length array of mNumberRegions elements
				// }
				//
				unsafe {
					var ptr = (byte*) this.ptr + 2 * sizeof (uint);
					for (int i = 0; i < index; ++i) {
						var region = new AudioFileRegion ((IntPtr) ptr);
						ptr += region.TotalSize;
					}

					return new AudioFileRegion ((IntPtr) ptr);
				}
			}
		}

		/// <summary>Releases the resources used by the AudioFileRegionList object.</summary>
		///         <remarks>
		///           <para>The Dispose method releases the resources used by the AudioFileRegionList class.</para>
		///           <para>Calling the Dispose method when the application is finished using the AudioFileRegionList ensures that all external resources used by this managed object are released as soon as possible.  Once developers have invoked the Dispose method, the object is no longer useful and developers should no longer make any calls to it.  For more information on releasing resources see ``Cleaning up Unmananaged Resources'' at https://msdn.microsoft.com/en-us/library/498928w2.aspx</para>
		///         </remarks>
		public void Dispose ()
		{
			Dispose (true);
		}

		/// <include file="../../docs/api/AudioToolbox/AudioFileRegionList.xml" path="/Documentation/Docs[@DocId='M:AudioToolbox.AudioFileRegionList.Dispose(System.Boolean)']/*" />
		protected virtual void Dispose (bool disposing)
		{
			if (!owns || ptr == IntPtr.Zero)
				return;

			for (int i = 0; i < Count; ++i) {
				CFObject.CFRelease (this [i].NameWeak);
			}

			Marshal.FreeHGlobal (ptr);
			ptr = IntPtr.Zero;
			GC.SuppressFinalize (this);
		}
	}

	/// <summary>Class used to create audio files or read audio files.</summary>
	///     <remarks>
	///       <para>Use the Create, Open and OpenRead factory methods to create instances of this class. </para>
	///       <para>This class provides access to the encoder and decoder for compressed audio files.</para>
	///     </remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class AudioFile : DisposableObject {
		internal AudioFile ()
		{
			// This ctor is used by AudioSource that will set the handle later.
		}

		[Preserve (Conditional = true)]
		internal AudioFile (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioFileClose (AudioFileID handle);

		/// <include file="../../docs/api/AudioToolbox/AudioFile.xml" path="/Documentation/Docs[@DocId='M:AudioToolbox.AudioFile.Dispose(System.Boolean)']/*" />
		protected override void Dispose (bool disposing)
		{
			if (Handle != IntPtr.Zero && Owns)
				AudioFileClose (Handle);
			base.Dispose (disposing);
		}

		/// <summary>Audio file size, in bytes.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public long Length {
			get {
				return GetLong (AudioFileProperty.AudioDataByteCount);
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static OSStatus AudioFileCreateWithURL (IntPtr cfurlref_infile, AudioFileType inFileType, AudioStreamBasicDescription* inFormat, AudioFileFlags inFlags, AudioFileID* file_id);

		/// <param name="url">The url of the file to create</param>
		///         <param name="fileType">The file type for the created file</param>
		///         <param name="format">Description of the data that is going to be passed to the AudioFile object</param>
		///         <param name="inFlags">Creation flags.</param>
		///         <summary>Creates a new audio file.</summary>
		///         <returns>The initialized audio file, or null if there is an error creating the file</returns>
		///         <remarks>
		///         </remarks>
		public static AudioFile? Create (string url, AudioFileType fileType, AudioStreamBasicDescription format, AudioFileFlags inFlags)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));

			using (var cfurl = CFUrl.FromUrlString (url, null)!)
				return Create (cfurl, fileType, format, inFlags);
		}

		/// <param name="url">The url of the file to create</param>
		///         <param name="fileType">The file type for the created file</param>
		///         <param name="format">Description of the data that is going to be passed to the AudioFile object</param>
		///         <param name="inFlags">Creation flags.</param>
		///         <summary>Creates a new audio file.</summary>
		///         <returns>The initialized audio file, or null if there is an error creating the file</returns>
		///         <remarks>
		///         </remarks>
		public static AudioFile? Create (CFUrl url, AudioFileType fileType, AudioStreamBasicDescription format, AudioFileFlags inFlags)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));

			var h = default (IntPtr);

			unsafe {
				var urlHandle = url.Handle;
				if (AudioFileCreateWithURL (urlHandle, fileType, &format, inFlags, &h) == 0) {
					GC.KeepAlive (url);
					return new AudioFile (h, true);
				}
			}
			return null;
		}

		/// <param name="url">The url of the file to create</param>
		///         <param name="fileType">The file type for the created file</param>
		///         <param name="format">Description of the data that is going to be passed to the AudioFile object</param>
		///         <param name="inFlags">Creation flags.</param>
		///         <summary>Creates a new audio file.</summary>
		///         <returns>The initialized audio file, or null if there is an error creating the file</returns>
		///         <remarks>
		///         </remarks>
		public static AudioFile? Create (NSUrl url, AudioFileType fileType, AudioStreamBasicDescription format, AudioFileFlags inFlags)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));

			var h = default (IntPtr);

			unsafe {
				var urlHandle = url.Handle;
				if (AudioFileCreateWithURL (urlHandle, fileType, &format, inFlags, &h) == 0) {
					GC.KeepAlive (url);
					return new AudioFile (h, true);
				}
			}
			return null;
		}


		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static AudioFileError AudioFileOpenURL (IntPtr cfurlref_infile, byte permissions, AudioFileType fileTypeHint, IntPtr* file_id);

		/// <param name="url">An url to a local file name.</param>
		///         <param name="fileTypeHint">A hint indicating the file format expected, this is necessary for audio files where the operating system can not probe the type by looking at the file signature or file extension (for example AC3.   Pass zero to auto detect the format.</param>
		///         <summary>Opens an audio file for reading.</summary>
		///         <returns>An instance of AudioFile on success, or null on error.</returns>
		///         <remarks>The hint is necessary as sometimes it is not possible to determine the file type merely based on the contents of the file.</remarks>
		public static AudioFile? OpenRead (string url, AudioFileType fileTypeHint = 0)
		{
			return Open (url, AudioFilePermission.Read, fileTypeHint);
		}

		/// <param name="url">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <param name="fileTypeHint">To be added.</param>
		///         <summary>Opens an audio file for reading.</summary>
		///         <returns>An instance of AudioFile on success, or null on error.</returns>
		///         <remarks>The hint is necessary as sometimes it is not possible to determine the file type merely based on the contents of the file.</remarks>
		public static AudioFile? OpenRead (string url, out AudioFileError error, AudioFileType fileTypeHint = 0)
		{
			return Open (url, AudioFilePermission.Read, out error, fileTypeHint);
		}

		/// <param name="url">Url pointing to the file to read.</param>
		///         <param name="fileTypeHint">A hint indicating the file format expected, this is necessary for audio files where the operating system can not probe the type by looking at the file signature or file extension (for example AC3.   Pass zero to auto detect the format.</param>
		///         <summary>Opens the specified audio file for reading, frames will be decoded from the native format to raw audio data.</summary>
		///         <returns>An instance of AudioFile on success, or null on error.</returns>
		///         <remarks>Once you have opened the file for reading, you can use the various Read methods to decode the audio packets contained in the file.</remarks>
		public static AudioFile? OpenRead (CFUrl url, AudioFileType fileTypeHint = 0)
		{
			return Open (url, AudioFilePermission.Read, fileTypeHint);
		}

		/// <param name="url">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <param name="fileTypeHint">To be added.</param>
		///         <summary>Opens an audio file for reading.</summary>
		///         <returns>An instance of AudioFile on success, or null on error.</returns>
		///         <remarks>The hint is necessary as sometimes it is not possible to determine the file type merely based on the contents of the file.</remarks>
		public static AudioFile? OpenRead (CFUrl url, out AudioFileError error, AudioFileType fileTypeHint = 0)
		{
			return Open (url, AudioFilePermission.Read, out error, fileTypeHint);
		}

		/// <param name="url">Url pointing to the file to read.</param>
		///         <param name="fileTypeHint">A hint indicating the file format expected, this is necessary for audio files where the operating system can not probe the type by looking at the file signature or file extension (for example AC3.   Pass zero to auto detect the format.</param>
		///         <summary>Opens the specified audio file for reading, frames will be decoded from the native format to raw audio data.</summary>
		///         <returns>An instance of AudioFile on success, or null on error.</returns>
		///         <remarks>Once you have opened the file for reading, you can use the various Read methods to decode the audio packets contained in the file.</remarks>
		public static AudioFile? OpenRead (NSUrl url, AudioFileType fileTypeHint = 0)
		{
			return Open (url, AudioFilePermission.Read, fileTypeHint);
		}

		/// <param name="url">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <param name="fileTypeHint">To be added.</param>
		///         <summary>Opens an audio file for reading.</summary>
		///         <returns>An instance of AudioFile on success, or null on error.</returns>
		///         <remarks>The hint is necessary as sometimes it is not possible to determine the file type merely based on the contents of the file.</remarks>
		public static AudioFile? OpenRead (NSUrl url, out AudioFileError error, AudioFileType fileTypeHint = 0)
		{
			return Open (url, AudioFilePermission.Read, out error, fileTypeHint);
		}

		/// <param name="url">To be added.</param>
		///         <param name="permissions">To be added.</param>
		///         <param name="fileTypeHint">A hint for the decoder.</param>
		///         <summary>Opens an audio file.</summary>
		///         <returns>An instance of AudioFile on success, null on failure.</returns>
		///         <remarks>The hint is necessary as sometimes it is not possible to determine the file type merely based on the contents of the file.</remarks>
		public static AudioFile? Open (string url, AudioFilePermission permissions, AudioFileType fileTypeHint = 0)
		{
			AudioFileError error;
			return Open (url, permissions, out error, fileTypeHint);
		}

		/// <param name="url">To be added.</param>
		///         <param name="permissions">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <param name="fileTypeHint">To be added.</param>
		///         <summary>Opens an audio file.</summary>
		///         <returns>An instance of AudioFile on success, null on failure.</returns>
		///         <remarks>The hint is necessary as sometimes it is not possible to determine the file type merely based on the contents of the file.</remarks>
		public static AudioFile? Open (string url, AudioFilePermission permissions, out AudioFileError error, AudioFileType fileTypeHint = 0)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));

			using (var cfurl = CFUrl.FromUrlString (url, null)!)
				return Open (cfurl, permissions, out error, fileTypeHint);
		}

		/// <param name="url">The url to a local file name.</param>
		///         <param name="permissions">The permissions used for the file (reading, writing or both).</param>
		///         <param name="fileTypeHint">A hint for the decoder.</param>
		///         <summary>Opens an audio file.</summary>
		///         <returns>An instance of AudioFile on success, null on failure.</returns>
		///         <remarks>The hint is necessary as sometimes it is not possible to determine the file type merely based on the contents of the file.</remarks>
		public static AudioFile? Open (CFUrl url, AudioFilePermission permissions, AudioFileType fileTypeHint = 0)
		{
			AudioFileError error;
			return Open (url, permissions, out error, fileTypeHint);
		}

		/// <param name="url">To be added.</param>
		///         <param name="permissions">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <param name="fileTypeHint">To be added.</param>
		///         <summary>Opens an audio file.</summary>
		///         <returns>An instance of AudioFile on success, null on failure.</returns>
		///         <remarks>The hint is necessary as sometimes it is not possible to determine the file type merely based on the contents of the file.</remarks>
		public static AudioFile? Open (CFUrl url, AudioFilePermission permissions, out AudioFileError error, AudioFileType fileTypeHint = 0)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));

			AudioFile? audioFile = Open (url.Handle, permissions, fileTypeHint, out error);
			GC.KeepAlive (url);
			return audioFile;
		}

		/// <param name="url">To be added.</param>
		///         <param name="permissions">To be added.</param>
		///         <param name="fileTypeHint">To be added.</param>
		///         <summary>Opens an audio file.</summary>
		///         <returns>An instance of AudioFile on success, null on failure.</returns>
		///         <remarks>The hint is necessary as sometimes it is not possible to determine the file type merely based on the contents of the file.</remarks>
		public static AudioFile? Open (NSUrl url, AudioFilePermission permissions, AudioFileType fileTypeHint = 0)
		{
			AudioFileError error;
			return Open (url, permissions, out error, fileTypeHint);
		}

		/// <param name="url">To be added.</param>
		///         <param name="permissions">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <param name="fileTypeHint">To be added.</param>
		///         <summary>Opens an audio file.</summary>
		///         <returns>An instance of AudioFile on success, null on failure.</returns>
		///         <remarks>The hint is necessary as sometimes it is not possible to determine the file type merely based on the contents of the file.</remarks>
		public static AudioFile? Open (NSUrl url, AudioFilePermission permissions, out AudioFileError error, AudioFileType fileTypeHint = 0)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));

			AudioFile? audioFile = Open (url.Handle, permissions, fileTypeHint, out error);
			GC.KeepAlive (url);
			return audioFile;
		}

		static AudioFile? Open (IntPtr urlHandle, AudioFilePermission permissions, AudioFileType fileTypeHint, out AudioFileError error)
		{
			var file = default (IntPtr);
			unsafe {
				error = AudioFileOpenURL (urlHandle, (byte) permissions, fileTypeHint, &file);
			}
			if (error == AudioFileError.Success)
				return new AudioFile (file, true);
			return null;
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioFileOptimize (AudioFileID handle);

		/// <summary>Optimizes the audio file, thus preparing it to receive audio data.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Optimize ()
		{
			return AudioFileOptimize (Handle) == 0;
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static OSStatus AudioFileReadBytes (AudioFileID inAudioFile, byte useCache, long startingByte, int* numBytes, IntPtr outBuffer);

		/// <param name="startingByte">To be added.</param>
		///         <param name="buffer">To be added.</param>
		///         <param name="offset">To be added.</param>
		///         <param name="count">To be added.</param>
		///         <param name="useCache">To be added.</param>
		///         <summary>Reads <paramref name="count" /> bytes from <paramref name="buffer" />, starting at <paramref name="startingByte" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public int Read (long startingByte, byte [] buffer, int offset, int count, bool useCache)
		{
			if (offset < 0)
				throw new ArgumentException (nameof (offset), "<0");
			if (count < 0)
				throw new ArgumentException (nameof (count), "<0");
			if (startingByte < 0)
				throw new ArgumentException (nameof (startingByte), "<0");
			int len = buffer.Length;
			if (offset > len)
				throw new ArgumentException ("destination offset is beyond array size");
			// reordered to avoid possible integer overflow
			if (offset > len - count)
				throw new ArgumentException ("Reading would overrun buffer");

			unsafe {
				fixed (byte* p = &buffer [offset]) {
					var res = AudioFileReadBytes (Handle, useCache ? (byte) 1 : (byte) 0, startingByte, &count, (IntPtr) p);

					if (res == (int) AudioFileError.EndOfFile)
						return count <= 0 ? -1 : count;

					if (res == 0)
						return count;

					return -1;
				}
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static OSStatus AudioFileWriteBytes (AudioFileID audioFile, byte useCache, long startingByte, int* numBytes, IntPtr buffer);

		/// <param name="startingByte">The starting byte in the file where the data will be written.</param>
		///         <param name="buffer">The buffer that holds the data.</param>
		///         <param name="offset">The offset within the buffer where the data to be saved starts.</param>
		///         <param name="count">The number of bytes to write to the file.</param>
		///         <param name="useCache">Whether the data should be cached.</param>
		///         <summary>Writes a block of data to the audio file.</summary>
		///         <returns>The number of bytes written to the stream, or -1 on error.</returns>
		///         <remarks>This API merely writes bytes to the file without any encoding.   Use WritePackets to write with encoding.</remarks>
		public int Write (long startingByte, byte [] buffer, int offset, int count, bool useCache)
		{
			if (offset < 0)
				throw new ArgumentOutOfRangeException (nameof (offset), "< 0");
			if (count < 0)
				throw new ArgumentOutOfRangeException (nameof (count), "< 0");
			if (offset > buffer.Length - count)
				throw new ArgumentException ("Reading would overrun buffer");

			unsafe {
				fixed (byte* p = &buffer [offset]) {
					if (AudioFileWriteBytes (Handle, useCache ? (byte) 1 : (byte) 0, startingByte, &count, (IntPtr) p) == 0)
						return count;
					else
						return -1;
				}
			}
		}

		/// <param name="startingByte">To be added.</param>
		///         <param name="buffer">To be added.</param>
		///         <param name="offset">To be added.</param>
		///         <param name="count">To be added.</param>
		///         <param name="useCache">To be added.</param>
		///         <param name="errorCode">To be added.</param>
		///         <summary>Writes data to an audo file.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public int Write (long startingByte, byte [] buffer, int offset, int count, bool useCache, out int errorCode)
		{
			if (offset < 0)
				throw new ArgumentOutOfRangeException (nameof (offset), "< 0");
			if (count < 0)
				throw new ArgumentOutOfRangeException (nameof (count), "< 0");
			if (offset > buffer.Length - count)
				throw new ArgumentException ("Reading would overrun buffer");

			unsafe {
				fixed (byte* p = &buffer [offset]) {
					errorCode = AudioFileWriteBytes (Handle, useCache ? (byte) 1 : (byte) 0, startingByte, &count, (IntPtr) p);
					if (errorCode == 0)
						return count;
					else
						return -1;
				}
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static OSStatus AudioFileReadPacketData (
			AudioFileID audioFile, byte useCache, int* numBytes,
			AudioStreamPacketDescription* packetDescriptions, long inStartingPacket, int* numPackets, IntPtr outBuffer);

		/// <param name="inStartingPacket">The index of the first packet to read.</param>
		///         <param name="nPackets">The number of packets to read.</param>
		///         <param name="buffer">The output buffer where packets are written.</param>
		///         <summary>Reads packets of audio data from an audio file.</summary>
		///         <returns>Array of packet descriptors for the packets that were read.</returns>
		///         <remarks>
		///         </remarks>
		public AudioStreamPacketDescription []? ReadPacketData (long inStartingPacket, int nPackets, byte [] buffer)
		{
			AudioFileError error;
			return ReadPacketData (inStartingPacket, nPackets, buffer, out error);
		}

		/// <param name="inStartingPacket">To be added.</param>
		///         <param name="nPackets">To be added.</param>
		///         <param name="buffer">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Reads packets of audio data.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioStreamPacketDescription []? ReadPacketData (long inStartingPacket, int nPackets, byte [] buffer, out AudioFileError error)
		{
			if (buffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (buffer));
			int count = buffer.Length;
			return RealReadPacketData (false, inStartingPacket, ref nPackets, buffer, 0, ref count, out error);
		}

		/// <param name="useCache">If the data should be cached.</param>
		///         <param name="inStartingPacket">The index of the first packet to read.</param>
		///         <param name="nPackets">The number of packets to read.</param>
		///         <param name="buffer">The output buffer where packets are written.</param>
		///         <param name="offset">The offset in the output buffer where to start writing packets to.</param>
		///         <param name="count">The size of the output buffer (in bytes).</param>
		///         <summary>Reads packets of audio data from an audio file.</summary>
		///         <returns>Array of packet descriptors for the packets that were read.</returns>
		///         <remarks>
		///         </remarks>
		public AudioStreamPacketDescription []? ReadPacketData (bool useCache, long inStartingPacket, int nPackets, byte [] buffer, int offset, int count)
		{
			return ReadPacketData (useCache, inStartingPacket, ref nPackets, buffer, offset, ref count);
		}

		/// <param name="useCache">To be added.</param>
		///         <param name="inStartingPacket">To be added.</param>
		///         <param name="nPackets">To be added.</param>
		///         <param name="buffer">To be added.</param>
		///         <param name="offset">To be added.</param>
		///         <param name="count">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Reads packets of audio data.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioStreamPacketDescription []? ReadPacketData (bool useCache, long inStartingPacket, int nPackets, byte [] buffer, int offset, int count, out AudioFileError error)
		{
			return ReadPacketData (useCache, inStartingPacket, ref nPackets, buffer, offset, ref count, out error);
		}

		static internal AudioStreamPacketDescription []? PacketDescriptionFrom (int nPackets, IntPtr b)
		{
			if (b == IntPtr.Zero)
				return new AudioStreamPacketDescription [0];

			var ret = new AudioStreamPacketDescription [nPackets];
			int p = 0;
			for (int i = 0; i < nPackets; i++) {
				ret [i].StartOffset = Marshal.ReadInt64 (b, p);
				ret [i].VariableFramesInPacket = Marshal.ReadInt32 (b, p + 8);
				ret [i].DataByteSize = Marshal.ReadInt32 (b, p + 12);
				p += 16;
			}

			return ret;
		}

		/// <param name="useCache">If the data should be cached.</param>
		///         <param name="inStartingPacket">The index of the first packet to read.</param>
		///         <param name="nPackets">On input the number of packets to read, upon return the number of packets actually read.</param>
		///         <param name="buffer">The output buffer where packets are written.</param>
		///         <param name="offset">The offset in the output buffer where to start writing packets to.</param>
		///         <param name="count">On input the size of the output buffer (in bytes), upon return the actual number of bytes read.</param>
		///         <summary>Reads packets of audio data from an audio file.</summary>
		///         <returns>Array of packet descriptors for the packets that were read.</returns>
		///         <remarks>
		///         </remarks>
		public AudioStreamPacketDescription []? ReadPacketData (bool useCache, long inStartingPacket, ref int nPackets, byte [] buffer, int offset, ref int count)
		{
			AudioFileError error;
			return ReadPacketData (useCache, inStartingPacket, ref nPackets, buffer, offset, ref count, out error);
		}

		/// <param name="useCache">To be added.</param>
		///         <param name="inStartingPacket">To be added.</param>
		///         <param name="nPackets">To be added.</param>
		///         <param name="buffer">To be added.</param>
		///         <param name="offset">To be added.</param>
		///         <param name="count">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Reads packets of audio data.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioStreamPacketDescription []? ReadPacketData (bool useCache, long inStartingPacket, ref int nPackets, byte [] buffer, int offset, ref int count, out AudioFileError error)
		{
			if (buffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (buffer));
			if (offset < 0)
				throw new ArgumentException (nameof (offset), "<0");
			if (count < 0)
				throw new ArgumentException (nameof (count), "<0");
			int len = buffer.Length;
			if (offset > len)
				throw new ArgumentException ("destination offset is beyond array size");
			// reordered to avoid possible integer overflow
			if (offset > len - count)
				throw new ArgumentException ("Reading would overrun buffer");
			return RealReadPacketData (useCache, inStartingPacket, ref nPackets, buffer, offset, ref count, out error);
		}

		/// <param name="useCache">If the data should be cached.</param>
		///         <param name="inStartingPacket">The index of the first packet to read.</param>
		///         <param name="nPackets">The number of packets to read.</param>
		///         <param name="buffer">The output buffer where packets are written.</param>
		///         <param name="count">On input the size of the output buffer (in bytes), upon return the actual number of bytes read.</param>
		///         <summary>Reads packets of audio data from an audio file.</summary>
		///         <returns>Array of packet descriptors for the packets that were read.</returns>
		///         <remarks>
		///         </remarks>
		public AudioStreamPacketDescription []? ReadPacketData (bool useCache, long inStartingPacket, ref int nPackets, IntPtr buffer, ref int count)
		{
			AudioFileError error;
			return ReadPacketData (useCache, inStartingPacket, ref nPackets, buffer, ref count, out error);
		}

		/// <param name="useCache">To be added.</param>
		///         <param name="inStartingPacket">To be added.</param>
		///         <param name="nPackets">To be added.</param>
		///         <param name="buffer">To be added.</param>
		///         <param name="count">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Reads packets of audio data.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioStreamPacketDescription []? ReadPacketData (bool useCache, long inStartingPacket, ref int nPackets, IntPtr buffer, ref int count, out AudioFileError error)
		{
			var descriptions = new AudioStreamPacketDescription [nPackets];
			return ReadPacketData (useCache, inStartingPacket, ref nPackets, buffer, ref count, out error, descriptions);
		}

		/// <param name="useCache">To be added.</param>
		///         <param name="inStartingPacket">To be added.</param>
		///         <param name="nPackets">To be added.</param>
		///         <param name="buffer">To be added.</param>
		///         <param name="count">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <param name="descriptions">To be added.</param>
		///         <summary>Reads packets of audio data.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public unsafe AudioStreamPacketDescription []? ReadPacketData (bool useCache, long inStartingPacket, ref int nPackets, IntPtr buffer, ref int count, out AudioFileError error, AudioStreamPacketDescription [] descriptions)
		{
			if (buffer == IntPtr.Zero)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (buffer));
			if (count < 0)
				throw new ArgumentException (nameof (count), "<0");
			if (descriptions is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (descriptions));

			fixed (AudioStreamPacketDescription* p = descriptions) {
				return RealReadPacketData (useCache, inStartingPacket, ref nPackets, buffer, ref count, out error, descriptions);
			}
		}

		unsafe AudioStreamPacketDescription []? RealReadPacketData (bool useCache, long inStartingPacket, ref int nPackets, byte [] buffer, int offset, ref int count, out AudioFileError error)
		{
			var descriptions = new AudioStreamPacketDescription [nPackets];
			fixed (byte* bop = &buffer [offset]) {
				fixed (AudioStreamPacketDescription* p = descriptions) {
					return RealReadPacketData (useCache, inStartingPacket, ref nPackets, (IntPtr) bop, ref count, out error, descriptions);
				}
			}
		}

		unsafe AudioStreamPacketDescription []? RealReadPacketData (bool useCache, long inStartingPacket, ref int nPackets, IntPtr buffer, ref int count, out AudioFileError error, AudioStreamPacketDescription [] descriptions)
		{
			OSStatus r;
			fixed (AudioStreamPacketDescription* pdesc = descriptions) {
				r = AudioFileReadPacketData (Handle,
						useCache ? (byte) 1 : (byte) 0,
						(int*) Unsafe.AsPointer<int> (ref count),
						pdesc,
						inStartingPacket,
						(int*) Unsafe.AsPointer<int> (ref nPackets),
						buffer);
			}

			error = (AudioFileError) r;

			if (r == (int) AudioFileError.EndOfFile) {
				if (count == 0)
					return null;
			} else if (r != 0) {
				return null;
			}

			if (descriptions.Length > nPackets) {
				// Didn't read as many descriptions as we requested.
				Array.Resize (ref descriptions, nPackets);
			}

			return descriptions;
		}

		/// <param name="inStartingPacket">To be added.</param>
		///         <param name="nPackets">To be added.</param>
		///         <param name="buffer">To be added.</param>
		///         <summary>Reads <paramref name="nPackets" /> bytes into <paramref name="buffer" />, starting at <paramref name="inStartingPacket" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioStreamPacketDescription []? ReadFixedPackets (long inStartingPacket, int nPackets, byte [] buffer)
		{
			AudioFileError error;
			return ReadFixedPackets (inStartingPacket, nPackets, buffer, out error);
		}

		/// <param name="inStartingPacket">To be added.</param>
		///         <param name="nPackets">To be added.</param>
		///         <param name="buffer">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Reads a fixed amount of audio data.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioStreamPacketDescription []? ReadFixedPackets (long inStartingPacket, int nPackets, byte [] buffer, out AudioFileError error)
		{
			if (buffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (buffer));
			return RealReadFixedPackets (false, inStartingPacket, nPackets, buffer, 0, buffer.Length, out error);
		}

		/// <param name="useCache">To be added.</param>
		///         <param name="inStartingPacket">To be added.</param>
		///         <param name="nPackets">To be added.</param>
		///         <param name="buffer">To be added.</param>
		///         <param name="offset">To be added.</param>
		///         <param name="count">To be added.</param>
		///         <summary>Reads a fixed amount of audio data.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioStreamPacketDescription []? ReadFixedPackets (bool useCache, long inStartingPacket, int nPackets, byte [] buffer, int offset, int count)
		{
			AudioFileError error;
			return ReadFixedPackets (useCache, inStartingPacket, nPackets, buffer, offset, count, out error);
		}

		/// <param name="useCache">To be added.</param>
		///         <param name="inStartingPacket">To be added.</param>
		///         <param name="nPackets">To be added.</param>
		///         <param name="buffer">To be added.</param>
		///         <param name="offset">To be added.</param>
		///         <param name="count">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Reads a fixed amount of audio data.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioStreamPacketDescription []? ReadFixedPackets (bool useCache, long inStartingPacket, int nPackets, byte [] buffer, int offset, int count, out AudioFileError error)
		{
			if (buffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (buffer));
			if (offset < 0)
				throw new ArgumentException (nameof (offset), "<0");
			if (count < 0)
				throw new ArgumentException (nameof (count), "<0");
			int len = buffer.Length;
			if (offset > len)
				throw new ArgumentException ("destination offset is beyond array size");
			// reordered to avoid possible integer overflow
			if (offset > len - count)
				throw new ArgumentException ("Reading would overrun buffer");
			return RealReadFixedPackets (useCache, inStartingPacket, nPackets, buffer, offset, count, out error);
		}

		unsafe AudioStreamPacketDescription []? RealReadFixedPackets (bool useCache, long inStartingPacket, int nPackets, byte [] buffer, int offset, int count, out AudioFileError error)
		{
			var descriptions = new AudioStreamPacketDescription [nPackets];
			fixed (byte* bop = &buffer [offset]) {
				OSStatus r;
				fixed (AudioStreamPacketDescription* pdesc = descriptions) {
					r = AudioFileReadPacketData (Handle, useCache ? (byte) 1 : (byte) 0, &count, pdesc, inStartingPacket, &nPackets, (IntPtr) bop);
				}
				error = (AudioFileError) r;
				if (r == (int) AudioFileError.EndOfFile) {
					if (count == 0)
						return null;
				} else if (r != 0) {
					return null;
				}
			}
			return descriptions;
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static AudioFileError AudioFileWritePackets (
			AudioFileID audioFile, byte useCache, int inNumBytes, AudioStreamPacketDescription* inPacketDescriptions,
						long inStartingPacket, int* numPackets, IntPtr buffer);

		/// <param name="useCache">To be added.</param>
		///         <param name="startingPacket">The starting packet in the packetDescriptions that should be written.</param>
		///         <param name="numPackets">To be added.</param>
		///         <param name="buffer">To be added.</param>
		///         <param name="byteCount">To be added.</param>
		///         <summary>Writes packets to an audo file.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public int WritePackets (bool useCache, long startingPacket, int numPackets, IntPtr buffer, int byteCount)
		{
			if (buffer == IntPtr.Zero)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (buffer));

			unsafe {
				if (AudioFileWritePackets (Handle, useCache ? (byte) 1 : (byte) 0, byteCount, null, startingPacket, &numPackets, buffer) == 0)
					return numPackets;
			}

			return -1;
		}

		/// <param name="useCache">Whether the data should be kept in the cache.</param>
		///         <param name="startingPacket">The starting packet in the packetDescriptions that should be written.</param>
		///         <param name="packetDescriptions">An array of packet descriptions that describe the content of the buffer.</param>
		///         <param name="buffer">The buffer containing the audio data.</param>
		///         <param name="byteCount">To be added.</param>
		///         <summary>Write audio packets to the audio file.</summary>
		///         <returns>The number of packets written or -1 on error.</returns>
		///         <remarks>To be added.</remarks>
		public int WritePackets (bool useCache, long startingPacket, AudioStreamPacketDescription [] packetDescriptions, IntPtr buffer, int byteCount)
		{
			if (packetDescriptions is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (packetDescriptions));
			if (buffer == IntPtr.Zero)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (buffer));
			int nPackets = packetDescriptions.Length;
			unsafe {
				fixed (AudioStreamPacketDescription* packetDescriptionsPtr = packetDescriptions) {
					if (AudioFileWritePackets (Handle, useCache ? (byte) 1 : (byte) 0, byteCount, packetDescriptionsPtr, startingPacket, &nPackets, buffer) == 0)
						return nPackets;
				}
			}
			return -1;
		}

		/// <param name="useCache">Whether the data should be kept in the cache.</param>
		///         <param name="startingPacket">The starting packet in the packetDescriptions that should be written.</param>
		///         <param name="packetDescriptions">An array of packet descriptions that describe the contents of the buffer.</param>
		///         <param name="buffer">The buffer containing the audio data.</param>
		///         <param name="offset">The first packet to write from the packetDescriptions.</param>
		///         <param name="byteCount">To be added.</param>
		///         <summary>Writes audio packets to the file.</summary>
		///         <returns>The number of packets written or -1 on error.</returns>
		///         <remarks>To be added.</remarks>
		unsafe public int WritePackets (bool useCache, long startingPacket, AudioStreamPacketDescription [] packetDescriptions, byte [] buffer, int offset, int byteCount)
		{
			if (packetDescriptions is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (packetDescriptions));
			if (buffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (buffer));
			if (offset < 0)
				throw new ArgumentOutOfRangeException (nameof (offset), "< 0");
			if (byteCount < 0)
				throw new ArgumentOutOfRangeException (nameof (byteCount), "< 0");
			if (offset > buffer.Length - byteCount)
				throw new ArgumentException ("Reading would overrun buffer");

			int nPackets = packetDescriptions.Length;
			fixed (byte* bop = &buffer [offset]) {
				fixed (AudioStreamPacketDescription* packetDescriptionsPtr = packetDescriptions) {
					if (AudioFileWritePackets (Handle, useCache ? (byte) 1 : (byte) 0, byteCount, packetDescriptionsPtr, startingPacket, &nPackets, (IntPtr) bop) == 0)
						return nPackets;
				}
				return -1;
			}
		}

		/// <param name="useCache">Whether the data should be kept in the cache.</param>
		///         <param name="startingPacket">The starting packet in the packetDescriptions that should be written.</param>
		///         <param name="packetDescriptions">An array of packet descriptions that describe the content of the buffer.</param>
		///         <param name="buffer">To be added.</param>
		///         <param name="byteCount">To be added.</param>
		///         <param name="errorCode">To be added.</param>
		///         <summary>Writes packets to an audo file.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public int WritePackets (bool useCache, long startingPacket, AudioStreamPacketDescription [] packetDescriptions, IntPtr buffer, int byteCount, out int errorCode)
		{
			if (packetDescriptions is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (packetDescriptions));
			if (buffer == IntPtr.Zero)
				throw new ArgumentException (nameof (buffer));
			int nPackets = packetDescriptions.Length;

			unsafe {
				fixed (AudioStreamPacketDescription* packetDescriptionsPtr = packetDescriptions) {
					errorCode = (int) AudioFileWritePackets (Handle, useCache ? (byte) 1 : (byte) 0, byteCount, packetDescriptionsPtr, startingPacket, &nPackets, buffer);
				}
			}
			if (errorCode == 0)
				return nPackets;
			return -1;
		}

		/// <param name="useCache">Whether the data should be kept in the cache.</param>
		///         <param name="startingPacket">The starting packet in the packetDescriptions that should be written.</param>
		///         <param name="packetDescriptions">An array of packet descriptions that describe the content of the buffer.</param>
		///         <param name="buffer">To be added.</param>
		///         <param name="offset">To be added.</param>
		///         <param name="byteCount">To be added.</param>
		///         <param name="errorCode">To be added.</param>
		///         <summary>Writes packets to an audo file.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		unsafe public int WritePackets (bool useCache, long startingPacket, AudioStreamPacketDescription [] packetDescriptions, byte [] buffer, int offset, int byteCount, out int errorCode)
		{
			if (packetDescriptions is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (packetDescriptions));
			if (buffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (buffer));
			if (offset < 0)
				throw new ArgumentOutOfRangeException (nameof (offset), "< 0");
			if (byteCount < 0)
				throw new ArgumentOutOfRangeException (nameof (byteCount), "< 0");
			if (offset > buffer.Length - byteCount)
				throw new ArgumentException ("Reading would overrun buffer");

			int nPackets = packetDescriptions.Length;
			fixed (byte* bop = &buffer [offset]) {
				fixed (AudioStreamPacketDescription* packetDescriptionsPtr = packetDescriptions) {
					errorCode = (int) AudioFileWritePackets (Handle, useCache ? (byte) 1 : (byte) 0, byteCount, packetDescriptionsPtr, startingPacket, &nPackets, (IntPtr) bop);
				}
				if (errorCode == 0)
					return nPackets;
				return -1;
			}
		}

		/// <param name="useCache">Whether the data should be kept in the cache.</param>
		///         <param name="numBytes">The number of bytes to write.</param>
		///         <param name="packetDescriptions">An array of packet descriptions that describe the content of the buffer.</param>
		///         <param name="startingPacket">The starting packet in the packetDescriptions that should be written.</param>
		///         <param name="numPackets">The number of packets to write replaced with the number of packets actually written.</param>
		///         <param name="buffer">The buffer containing the audio data.</param>
		///         <summary>Writes audio packets to the file.</summary>
		///         <returns>A status error code.</returns>
		///         <remarks>
		///         </remarks>
		public AudioFileError WritePackets (bool useCache, int numBytes, AudioStreamPacketDescription [] packetDescriptions, long startingPacket, ref int numPackets, IntPtr buffer)
		{
			if (buffer == IntPtr.Zero)
				throw new ArgumentException ("buffer");

			unsafe {
				fixed (AudioStreamPacketDescription* packetDescriptionsPtr = packetDescriptions) {
					return AudioFileWritePackets (Handle, useCache ? (byte) 1 : (byte) 0, numBytes, packetDescriptionsPtr, startingPacket, (int*) Unsafe.AsPointer<int> (ref numPackets), buffer);
				}
			}
		}

		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static /* OSStatus */ AudioFileError AudioFileWritePacketsWithDependencies (
			AudioFileID inAudioFile,
			byte /* Boolean */ inUseCache,
			uint /* UInt32 */ inNumBytes,
			AudioStreamPacketDescription* /* const AudioStreamPacketDescription * __nullable */ inPacketDescriptions,
			AudioStreamPacketDependencyDescription* /* const AudioStreamPacketDependencyDescription * */ inPacketDependencies,
			long /* SInt64 */ inStartingPacket,
			uint* /* UInt32 * */ ioNumPackets,
			IntPtr /* const void * */inBuffer);

		/// <summary>Writes audio packets to the file.</summary>
		/// <param name="useCache">Whether the data should be kept in the cache.</param>
		/// <param name="packetDescriptions">An array of packet descriptions that describe the content of the buffer.</param>
		/// <param name="packetDependencies">An array of packet dependencies for the audio data.</param>
		/// <param name="startingPacket">The index the first packet in the buffer to write.</param>
		/// <param name="numPackets">The number of packets to write replaced with the number of packets actually written.</param>
		/// <param name="buffer">The buffer containing the audio data to write.</param>
		/// <param name="numBytes">The number of bytes to write.</param>
		/// <returns><see cref="AudioFileError.Success" /> if successful, otherwise a status error code.</returns>
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		public AudioFileError WritePackets (bool useCache, AudioStreamPacketDescription []? packetDescriptions, AudioStreamPacketDependencyDescription [] packetDependencies, long startingPacket, ref int numPackets, IntPtr buffer, int numBytes)
		{
			if (buffer == IntPtr.Zero)
				throw new ArgumentException (nameof (buffer));

			if (packetDependencies is null)
				ThrowHelper.ThrowArgumentNullException (nameof (packetDependencies));

			unsafe {
				fixed (AudioStreamPacketDescription* packetDescriptionsPtr = packetDescriptions) {
					fixed (AudioStreamPacketDependencyDescription* packetDependenciesPtr = packetDependencies) {
						return AudioFileWritePacketsWithDependencies (
							GetCheckedHandle (),
							useCache.AsByte (),
							(uint) numBytes,
							packetDescriptionsPtr,
							packetDependenciesPtr,
							startingPacket,
							(uint*) Unsafe.AsPointer<int> (ref numPackets),
							buffer);
					}
				}
			}
		}

		/// <summary>Writes audio packets to the file.</summary>
		/// <param name="useCache">Whether the data should be kept in the cache.</param>
		/// <param name="numBytes">The number of bytes to write.</param>
		/// <param name="packetDescriptions">An array of packet descriptions that describe the content of the buffer.</param>
		/// <param name="packetDependencies">An array of packet dependencies for the audio data.</param>
		/// <param name="startingPacket">The index the first packet in the buffer to write.</param>
		/// <param name="numPackets">The number of packets to write replaced with the number of packets actually written.</param>
		/// <param name="buffer">The buffer containing the audio data to write.</param>
		/// <param name="offset">An offset into <paramref name="buffer" /> where the audio data to write starts.</param>
		/// <returns><see cref="AudioFileError.Success" /> if successful, otherwise a status error code.</returns>
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		public AudioFileError WritePackets (bool useCache, AudioStreamPacketDescription []? packetDescriptions, AudioStreamPacketDependencyDescription [] packetDependencies, long startingPacket, ref int numPackets, byte [] buffer, int offset, int numBytes)
		{
			if (buffer is null)
				ThrowHelper.ThrowArgumentNullException (nameof (buffer));
			if (offset < 0)
				throw new ArgumentOutOfRangeException (nameof (offset), "< 0");
			if (numBytes < 0)
				throw new ArgumentOutOfRangeException (nameof (numBytes), "< 0");
			if (offset > buffer.Length - numBytes)
				throw new ArgumentException ("Reading would overrun buffer");
			unsafe {
				fixed (byte* bufferPtr = &buffer [offset])
					return WritePackets (useCache, packetDescriptions, packetDependencies, startingPacket, ref numPackets, (IntPtr) bufferPtr, numBytes);
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static OSStatus AudioFileCountUserData (AudioFileID handle, uint userData, int* count);

		/// <summary>Get the number of user data for the specified chunk type.</summary>
		/// <param name="userData">The fourcc of the ID whose count to retrieve.</param>
		/// <returns>The number of user udata for the specified ID.</returns>
		public int CountUserData (uint userData)
		{
			int count;
			unsafe {
				if (AudioFileCountUserData (Handle, userData, &count) == 0)
					return count;
			}
			return -1;
		}

		/// <summary>Get the number of user data for the specified chunk type.</summary>
		/// <param name="chunkType">The fourcc of the chunk.</param>
		/// <returns>The number of user data for the specified chunk type.</returns>
		public int CountUserData (AudioFileChunkType chunkType)
		{
			return CountUserData ((uint) chunkType);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static OSStatus AudioFileGetUserDataSize (AudioFileID audioFile, uint userDataID, int index, int* userDataSize);

		/// <summary>Get the size of the specified user data.</summary>
		/// <param name="userDataId">The fourcc of the chunk.</param>
		/// <param name="index">The index of the chunk if there are more than one chunk.</param>
		/// <returns>Returns the (non-negative) size on success, otherwise -1.</returns>
		public int GetUserDataSize (uint userDataId, int index)
		{
			int ds;

			unsafe {
				if (AudioFileGetUserDataSize (Handle, userDataId, index, &ds) != 0)
					return -1;
			}
			return ds;
		}

		/// <summary>Get the size of the specified user data.</summary>
		/// <param name="chunkType">The fourcc of the chunk.</param>
		/// <param name="index">The index of the chunk if there are more than one chunk.</param>
		/// <returns>Returns the (non-negative) size on success, otherwise -1.</returns>
		public int GetUserDataSize (AudioFileChunkType chunkType, int index)
		{
			return GetUserDataSize ((uint) chunkType, index);
		}

		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		[SupportedOSPlatform ("macos14.0")]
		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static OSStatus AudioFileGetUserDataSize64 (AudioFileID audioFile, uint userDataID, int index, ulong* userDataSize);

		/// <summary>Get the 64-bit size of the specified user data.</summary>
		/// <param name="userDataId">The fourcc of the chunk.</param>
		/// <param name="index">The index of the chunk if there are more than one chunk.</param>
		/// <param name="size">The retrieved 64-bit size of the specified user data.</param>
		/// <returns>Returns <see cref="AudioFileError.Success" /> on success, otherwise an <see cref="AudioFileError" /> error code.</returns>
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		[SupportedOSPlatform ("macos14.0")]
		public AudioFileError GetUserDataSize (uint userDataId, int index, out ulong size)
		{
			size = 0;
			unsafe {
				return (AudioFileError) AudioFileGetUserDataSize64 (Handle, userDataId, index, (ulong*) Unsafe.AsPointer<ulong> (ref size));
			}
		}

		/// <summary>Get the 64-bit size of the specified user data.</summary>
		/// <param name="chunkType">The fourcc of the chunk.</param>
		/// <param name="index">The index of the chunk if there are more than one chunk.</param>
		/// <param name="size">The retrieved 64-bit size of the specified user data.</param>
		/// <returns>Returns <see cref="AudioFileError.Success" /> on success, otherwise an <see cref="AudioFileError" /> error code.</returns>
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		[SupportedOSPlatform ("macos14.0")]
		public AudioFileError GetUserDataSize (AudioFileChunkType chunkType, int index, out ulong size)
		{
			return GetUserDataSize ((uint) chunkType, index, out size);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static OSStatus AudioFileGetUserData (AudioFileID audioFile, int userDataID, int index, int* userDataSize, IntPtr userData);

		/// <summary>Get part of the data of a chunk in a file.</summary>
		/// <param name="userDataID">The fourcc of the chunk.</param>
		/// <param name="index">The index of the chunk if there are more than one chunk.</param>
		/// <param name="size">On input the size of the memory <paramref name="userData" /> points to. On output the number of bytes written.</param>
		/// <param name="userData">A pointer to memory where the data will be copied.</param>
		/// <returns>Returns <see cref="AudioFileError.Success" /> on success, otherwise an <see cref="AudioFileError" /> error code.</returns>
#if XAMCORE_5_0
		public AudioFileError GetUserData (int userDataID, int index, ref int size, IntPtr userData)
#else
		public int GetUserData (int userDataID, int index, ref int size, IntPtr userData)
#endif
		{
			unsafe {
				return AudioFileGetUserData (Handle, userDataID, index, (int*) Unsafe.AsPointer<int> (ref size), userData);
			}
		}

		/// <summary>Get part of the data of a chunk in a file.</summary>
		/// <param name="chunkType">The fourcc of the chunk.</param>
		/// <param name="index">The index of the chunk if there are more than one chunk.</param>
		/// <param name="size">On input the size of the memory <paramref name="userData" /> points to. On output the number of bytes written.</param>
		/// <param name="userData">A pointer to memory where the data will be copied.</param>
		/// <returns>Returns <see cref="AudioFileError.Success" /> on success, otherwise an <see cref="AudioFileError" /> error code.</returns>
		public AudioFileError GetUserData (AudioFileChunkType chunkType, int index, ref int size, IntPtr userData)
		{
			return (AudioFileError) GetUserData ((int) chunkType, index, ref size, userData);
		}

		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		[SupportedOSPlatform ("macos14.0")]
		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static OSStatus AudioFileGetUserDataAtOffset (AudioFileID audioFile, uint userDataID, int index, long inOffset, int* userDataSize, IntPtr userData);

		/// <summary>Get part of the data of a chunk in a file.</summary>
		/// <param name="userDataId">The fourcc of the chunk.</param>
		/// <param name="index">The index of the chunk if there are more than one chunk.</param>
		/// <param name="offset">The offset from the first byte of the chunk of the data to get.</param>
		/// <param name="size">On input the size of the memory <paramref name="userData" /> points to. On output the number of bytes written.</param>
		/// <param name="userData">A pointer to memory where the data will be copied.</param>
		/// <returns>Returns <see cref="AudioFileError.Success" /> on success, otherwise an <see cref="AudioFileError" /> error code.</returns>
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		[SupportedOSPlatform ("macos14.0")]
		public AudioFileError GetUserData (uint userDataId, int index, long offset, ref int size, IntPtr userData)
		{
			unsafe {
				return (AudioFileError) AudioFileGetUserDataAtOffset (Handle, userDataId, index, offset, (int*) Unsafe.AsPointer<int> (ref size), userData);
			}
		}

		/// <summary>Get part of the data of a chunk in a file.</summary>
		/// <param name="chunkType">The fourcc of the chunk.</param>
		/// <param name="index">The index of the chunk if there are more than one chunk.</param>
		/// <param name="offset">The offset from the first byte of the chunk of the data to get.</param>
		/// <param name="size">On input the size of the memory <paramref name="userData" /> points to. On output the number of bytes written.</param>
		/// <param name="userData">A pointer to memory where the data will be copied.</param>
		/// <returns>Returns <see cref="AudioFileError.Success" /> on success, otherwise an <see cref="AudioFileError" /> error code.</returns>
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		[SupportedOSPlatform ("macos14.0")]
		public AudioFileError GetUserData (AudioFileChunkType chunkType, int index, long offset, ref int size, IntPtr userData)
		{
			return GetUserData ((uint) chunkType, index, offset, ref size, userData);
		}

		/// <summary>Get part of the data of a chunk in a file.</summary>
		/// <param name="userDataId">The fourcc of the chunk.</param>
		/// <param name="index">The index of the chunk if there are more than one chunk.</param>
		/// <param name="offset">The offset from the first byte of the chunk of the data to get.</param>
		/// <param name="size">The number of bytes written into the byte array.</param>
		/// <param name="data">An array of bytes where the data will be copied.</param>
		/// <returns>Returns <see cref="AudioFileError.Success" /> on success, otherwise an <see cref="AudioFileError" /> error code.</returns>
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		[SupportedOSPlatform ("macos14.0")]
		public AudioFileError GetUserData (uint userDataId, int index, long offset, byte [] data, out int size)
		{
			size = data.Length;
			unsafe {
				fixed (byte* dataPtr = data)
					return GetUserData (userDataId, index, offset, ref size, (IntPtr) dataPtr);
			}
		}

		/// <summary>Get part of the data of a chunk in a file.</summary>
		/// <param name="chunkType">The fourcc of the chunk.</param>
		/// <param name="index">The index of the chunk if there are more than one chunk.</param>
		/// <param name="offset">The offset from the first byte of the chunk of the data to get.</param>
		/// <param name="size">The number of bytes written into the byte array.</param>
		/// <param name="data">An array of bytes where the data will be copied.</param>
		/// <returns>Returns <see cref="AudioFileError.Success" /> on success, otherwise an <see cref="AudioFileError" /> error code.</returns>
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		[SupportedOSPlatform ("macos14.0")]
		public AudioFileError GetUserData (AudioFileChunkType chunkType, int index, long offset, byte [] data, out int size)
		{
			return GetUserData ((uint) chunkType, index, offset, data, out size);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioFileSetUserData (AudioFileID inAudioFile, int userDataID, int index, int userDataSize, IntPtr userData);

		/// <param name="userDataId">To be added.</param>
		///         <param name="index">To be added.</param>
		///         <param name="userDataSize">To be added.</param>
		///         <param name="userData">To be added.</param>
		///         <summary>Sets the value at the specified <paramref name="index" /> into the specified <paramref name="userDataId" /> to <paramref name="userData" />, which must have the size that is specified in <paramref name="userDataSize" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public int SetUserData (int userDataId, int index, int userDataSize, IntPtr userData)
		{
			if (userData == IntPtr.Zero)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (userData));
			return AudioFileSetUserData (Handle, userDataId, index, userDataSize, userData);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioFileRemoveUserData (AudioFileID audioFile, int userDataID, int index);

		/// <param name="userDataId">To be added.</param>
		///         <param name="index">To be added.</param>
		///         <summary>Removes the chunk of user data at the specified <paramref name="index" /> in the user data that is identified by <paramref name="userDataId" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public int RemoveUserData (int userDataId, int index)
		{
			return AudioFileRemoveUserData (Handle, userDataId, index);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static OSStatus AudioFileGetPropertyInfo (AudioFileID audioFile, AudioFileProperty propertyID, int* outDataSize, int* isWritable);

		/// <param name="property">To be added.</param>
		///         <param name="size">To be added.</param>
		///         <param name="writable">To be added.</param>
		///         <summary>Returns the value of the specified audio property, and stores the number of bytes allocated to store it in <paramref name="size" />, and indicates whether the value is writeable.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool GetPropertyInfo (AudioFileProperty property, out int size, out int writable)
		{
			size = default;
			writable = default;
			unsafe {
				return AudioFileGetPropertyInfo (Handle, property, (int*) Unsafe.AsPointer<int> (ref size), (int*) Unsafe.AsPointer<int> (ref writable)) == 0;
			}
		}

		/// <param name="property">The property being queried.</param>
		///         <summary>Checks whether the property value is settable.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		public bool IsPropertyWritable (AudioFileProperty property)
		{
			return GetPropertyInfo (property, out var _, out var writable) && writable != 0;
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static OSStatus AudioFileGetProperty (AudioFileID audioFile, AudioFileProperty property, int* dataSize, IntPtr outdata);

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static OSStatus AudioFileGetProperty (AudioFileID audioFile, AudioFileProperty property, int* dataSize, void* outdata);

		/// <param name="property">To be added.</param>
		///         <param name="dataSize">To be added.</param>
		///         <param name="outdata">To be added.</param>
		///         <summary>Returns the value of the specified audio property, stores it in <paramref name="outdata" />, and stores the number of bytes allocated to store it in <paramref name="dataSize" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool GetProperty (AudioFileProperty property, ref int dataSize, IntPtr outdata)
		{
			unsafe {
				return AudioFileGetProperty (Handle, property, (int*) Unsafe.AsPointer<int> (ref dataSize), outdata) == 0;
			}
		}

		/// <param name="property">To be added.</param>
		///         <param name="size">To be added.</param>
		///         <summary>Returns the value of the specified audio property, and stores the number of bytes allocated to store it in <paramref name="size" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public IntPtr GetProperty (AudioFileProperty property, out int size)
		{
			int writable;

			if (!GetPropertyInfo (property, out size, out writable))
				return IntPtr.Zero;

			var buffer = Marshal.AllocHGlobal (size);
			if (buffer == IntPtr.Zero)
				return IntPtr.Zero;

			unsafe {
				var rv = AudioFileGetProperty (Handle, property, (int*) Unsafe.AsPointer<int> (ref size), buffer);
				if (rv == 0)
					return buffer;
			}
			Marshal.FreeHGlobal (buffer);
			return IntPtr.Zero;
		}

		unsafe T? GetProperty<[DynamicallyAccessedMembers (DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T> (AudioFileProperty property) where T : unmanaged
		{
			int size, writable;

			if (!GetPropertyInfo (property, out size, out writable))
				return null;
			var buffer = Marshal.AllocHGlobal (size);
			if (buffer == IntPtr.Zero)
				return null;
			try {
				var ptype = typeof (T);
				var r = AudioFileGetProperty (Handle, property, &size, buffer);
				switch (ptype.Name) {
				case nameof (AudioFilePacketTableInfo):
					PacketTableInfoStatus = (AudioFileError) r;
					break;
				case nameof (AudioStreamBasicDescription):
					StreamBasicDescriptionStatus = (AudioFileError) r;
					break;
				}
				if (r == 0) {
					return Marshal.PtrToStructure<T> (buffer)!;
				}

				return null;
			} finally {
				Marshal.FreeHGlobal (buffer);
			}
		}

		int GetInt (AudioFileProperty property)
		{
			unsafe {
				int val = 0;
				int size = 4;
				if (AudioFileGetProperty (Handle, property, &size, (IntPtr) (&val)) == 0)
					return val;
				return 0;
			}
		}

		IntPtr GetIntPtr (AudioFileProperty property)
		{
			unsafe {
				IntPtr val = IntPtr.Zero;
				int size = sizeof (IntPtr);
				if (AudioFileGetProperty (Handle, property, &size, (IntPtr) (&val)) == 0)
					return val;
				return IntPtr.Zero;
			}
		}

		double GetDouble (AudioFileProperty property)
		{
			unsafe {
				double val = 0;
				int size = 8;
				if (AudioFileGetProperty (Handle, property, &size, (IntPtr) (&val)) == 0)
					return val;
				return 0;
			}
		}

		long GetLong (AudioFileProperty property)
		{
			unsafe {
				long val = 0;
				int size = 8;
				if (AudioFileGetProperty (Handle, property, &size, (IntPtr) (&val)) == 0)
					return val;
				return 0;
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static AudioFileError AudioFileSetProperty (AudioFileID audioFile, AudioFileProperty property, int dataSize, IntPtr propertyData);

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static AudioFileError AudioFileSetProperty (AudioFileID audioFile, AudioFileProperty property, int dataSize, AudioFilePacketTableInfo* propertyData);

		/// <param name="property">To be added.</param>
		///         <param name="dataSize">To be added.</param>
		///         <param name="propertyData">To be added.</param>
		///         <summary>Sets the value of the specified <paramref name="property" /> to <paramref name="propertyData" />, which must have the size that is specified in <paramref name="dataSize" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool SetProperty (AudioFileProperty property, int dataSize, IntPtr propertyData)
		{
			if (propertyData == IntPtr.Zero)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (propertyData));
			return AudioFileSetProperty (Handle, property, dataSize, propertyData) == 0;
		}

		void SetInt (AudioFileProperty property, int value)
		{
			unsafe {
				AudioFileSetProperty (Handle, property, 4, (IntPtr) (&value));
			}
		}

		unsafe AudioFileError SetDouble (AudioFileProperty property, double value)
		{
			return AudioFileSetProperty (Handle, property, sizeof (double), (IntPtr) (&value));
		}

		/// <summary>Audio file type.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioFileType FileType {
			get {
				return (AudioFileType) GetInt (AudioFileProperty.FileFormat);
			}
		}

		/// <summary>The audio basic description, as determined by decoding the file.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		[Advice ("Use 'DataFormat' instead.")]
		public AudioStreamBasicDescription StreamBasicDescription {
			get {
				return GetProperty<AudioStreamBasicDescription> (AudioFileProperty.DataFormat) ?? default (AudioStreamBasicDescription);
			}
		}

		/// <summary>Gets the status of the stream's basic description.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioFileError StreamBasicDescriptionStatus { get; private set; }

		/// <summary>Gets the <see cref="AudioToolbox.AudioStreamBasicDescription" />, if present, that describes the format of the audio data.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioStreamBasicDescription? DataFormat {
			get {
				return GetProperty<AudioStreamBasicDescription> (AudioFileProperty.DataFormat);
			}
		}

		/// <summary>Returns a list of the supported audio formats.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioFormat []? AudioFormats {
			get {
				unsafe {
					int size;
					var r = GetProperty (AudioFileProperty.FormatList, out size);
					var records = (AudioFormat*) r;
					if (r == IntPtr.Zero)
						return null;
					int itemSize = sizeof (AudioFormat);
					int items = size / itemSize;
					var ret = new AudioFormat [items];

					for (int i = 0; i < items; i++)
						ret [i] = records [i];

					Marshal.FreeHGlobal (r);
					return ret;
				}
			}
		}

		/// <summary>Gets a Boolean value that tells whether the audio file has been optimized and is ready to receive sound data.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsOptimized {
			get {
				return GetInt (AudioFileProperty.IsOptimized) == 1;
			}
		}

		/// <summary>The magic cookie for this file.</summary>
		///         <value />
		///         <remarks>Certain files require the magic cookie to be set before they can be written to.   Set this property before you write packets from your source (AudioQueue).</remarks>
		public byte [] MagicCookie {
			get {
				int size;
				var h = GetProperty (AudioFileProperty.MagicCookieData, out size);
				if (h == IntPtr.Zero)
					return Array.Empty<byte> ();

				byte [] cookie = new byte [size];
				Marshal.Copy (h, cookie, 0, size);
				Marshal.FreeHGlobal (h);

				return cookie;
			}

			set {
				if (value is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));

				unsafe {
					fixed (byte* bp = value) {
						SetProperty (AudioFileProperty.MagicCookieData, value.Length, (IntPtr) bp);
					}
				}
			}
		}

		/// <summary>Gets the number of audio data packets in the audio file.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public long DataPacketCount {
			get {
				return GetLong (AudioFileProperty.AudioDataPacketCount);
			}
		}

		/// <summary>Gets the maximum audio packet size.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public int MaximumPacketSize {
			get {
				return GetInt (AudioFileProperty.MaximumPacketSize);
			}
		}

		/// <summary>Gets the offset, in bytes, to the beginning of the audio data in the audio file.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public long DataOffset {
			get {
				return GetLong (AudioFileProperty.DataOffset);
			}
		}

		/// <summary>Gets the album artwork for the audio file.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSData? AlbumArtwork {
			get {
				return Runtime.GetNSObject<NSData> (GetIntPtr (AudioFileProperty.AlbumArtwork));
			}
		}

		/// <summary>Gets the channel layout of the audio file.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioChannelLayout? ChannelLayout {
			get {
				int size;
				var h = GetProperty (AudioFileProperty.ChannelLayout, out size);
				if (h == IntPtr.Zero)
					return null;

				var layout = AudioChannelLayout.FromHandle (h);
				Marshal.FreeHGlobal (h);

				return layout;
			}
		}

		/// <summary>Gets or sets a Boolean value that controls whether the updating of file size information in the header will be deferred until the file is read, optimized, or closed. The default, which is safer, is <see langword="false" /></summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool DeferSizeUpdates {
			get {
				return GetInt (AudioFileProperty.DeferSizeUpdates) == 1;
			}
			set {
				SetInt (AudioFileProperty.DeferSizeUpdates, value ? 1 : 0);
			}
		}

		/// <summary>Audio file bit rate.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public int BitRate {
			get {
				return GetInt (AudioFileProperty.BitRate);
			}
		}

		/// <summary>Gets the estimated duration, in seconds, of the audio data in the file.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double EstimatedDuration {
			get {
				return GetDouble (AudioFileProperty.EstimatedDuration);
			}
		}

		/// <summary>Gets the theoretical upper bound for the audio packet size for audio data in the file.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public int PacketSizeUpperBound {
			get {
				return GetInt (AudioFileProperty.PacketSizeUpperBound);
			}
		}

		/// <summary>Gets the amount of recording time to reserve in the audio file.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double ReserveDuration {
			get {
				return GetDouble (AudioFileProperty.ReserveDuration);
			}
		}

		/// <summary>Gets the <see cref="AudioToolbox.AudioFileMarkerList" /> that contains the markers for the audio file.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioFileMarkerList? MarkerList {
			get {
				var ptr = GetProperty (AudioFileProperty.MarkerList, out var _);
				if (ptr == IntPtr.Zero)
					return null;

				return new AudioFileMarkerList (ptr, true);
			}
		}

		/// <summary>Gets a list of all the audio regions in the audio file.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioFileRegionList? RegionList {
			get {
				var ptr = GetProperty (AudioFileProperty.RegionList, out var _);
				if (ptr == IntPtr.Zero)
					return null;

				return new AudioFileRegionList (ptr, true);
			}
		}

		/// <summary>Gets the status of the audio packet table..</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioFileError PacketTableInfoStatus { get; private set; }

		/// <summary>Gets or sets the <see cref="AudioToolbox.AudioFilePacketTableInfo" /> structure that describes the audio file packet table.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public unsafe AudioFilePacketTableInfo? PacketTableInfo {
			get {
				return GetProperty<AudioFilePacketTableInfo> (AudioFileProperty.PacketTableInfo);
			}
			set {
				if (value is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));

				AudioFilePacketTableInfo afpti = value.Value;
				var res = AudioFileSetProperty (Handle, AudioFileProperty.PacketTableInfo, sizeof (AudioFilePacketTableInfo), &afpti);
				if (res != 0)
					throw new ArgumentException (res.ToString ());
			}
		}

		/// <summary>Gets an array of four-character codes that describe the kind of each chunk in the audio file.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public unsafe AudioFileChunkType []? ChunkIDs {
			get {
				int size;
				int writable;
				var res = GetPropertyInfo (AudioFileProperty.ChunkIDs, out size, out writable);
				if (size == 0)
					return null;

				var data = new AudioFileChunkType [size / sizeof (AudioFileChunkType)];
				fixed (AudioFileChunkType* ptr = data) {
					if (AudioFileGetProperty (Handle, AudioFileProperty.ChunkIDs, &size, (IntPtr) ptr) != 0)
						return null;

					return data;
				}
			}
		}

		/// <summary>Gets a byte array that contains the ID3Tag for the audio data.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public unsafe byte []? ID3Tag {
			get {
				int size;
				int writable;
				var res = GetPropertyInfo (AudioFileProperty.ID3Tag, out size, out writable);
				if (size == 0)
					return null;

				var data = new byte [size];
				fixed (byte* ptr = data) {
					if (AudioFileGetProperty (Handle, AudioFileProperty.ID3Tag, &size, (IntPtr) ptr) != 0)
						return null;

					return data;
				}
			}
		}

		/// <summary>Gets the CF dictionary that contains audio file metadata.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioFileInfoDictionary? InfoDictionary {
			get {
				var ptr = GetIntPtr (AudioFileProperty.InfoDictionary);
				if (ptr == IntPtr.Zero)
					return null;

				return new AudioFileInfoDictionary (new NSMutableDictionary (ptr, true));
			}
		}

		/// <param name="packet">To be added.</param>
		///         <summary>Returns the frame number for the specified <paramref name="packet" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public long PacketToFrame (long packet)
		{
			AudioFramePacketTranslation buffer = default;
			buffer.Packet = packet;

			unsafe {
				int size = sizeof (AudioFramePacketTranslation);
				if (AudioFileGetProperty (Handle, AudioFileProperty.PacketToFrame, &size, &buffer) == 0)
					return buffer.Frame;
				return -1;
			}
		}

		/// <param name="frame">The frame.</param>
		///         <param name="frameOffsetInPacket">The offset inside the packet that the frame points to.</param>
		///         <summary>Converts an audio frame into a packet offset.</summary>
		///         <returns>-1 on failure, otherwise the packet that represents the specified frame.   Additionally, the offset within the packet is returned in the out parameter.</returns>
		///         <remarks>To be added.</remarks>
		public long FrameToPacket (long frame, out int frameOffsetInPacket)
		{
			AudioFramePacketTranslation buffer = default;
			buffer.Frame = frame;

			unsafe {
				int size = sizeof (AudioFramePacketTranslation);
				if (AudioFileGetProperty (Handle, AudioFileProperty.FrameToPacket, &size, &buffer) == 0) {
					frameOffsetInPacket = buffer.FrameOffsetInPacket;
					return buffer.Packet;
				}
				frameOffsetInPacket = 0;
				return -1;
			}
		}

		/// <param name="packet">To be added.</param>
		///         <param name="isEstimate">To be added.</param>
		///         <summary>Returns the byte offset for the <paramref name="packet" /> and indicates whether this is an estimated value in <paramref name="isEstimate" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public long PacketToByte (long packet, out bool isEstimate)
		{
			AudioBytePacketTranslation buffer = default;
			buffer.Packet = packet;

			unsafe {
				int size = sizeof (AudioBytePacketTranslation);
				if (AudioFileGetProperty (Handle, AudioFileProperty.PacketToByte, &size, &buffer) == 0) {
					isEstimate = (buffer.Flags & BytePacketTranslationFlags.IsEstimate) != 0;
					return buffer.Byte;
				}
				isEstimate = false;
				return -1;
			}
		}

		/// <param name="byteval">The byte position.</param>
		///         <param name="byteOffsetInPacket">Offset within the packet.</param>
		///         <param name="isEstimate">True if the return value is an estimate.</param>
		///         <summary>Converts a position on a stream to its packet location.</summary>
		///         <returns>The packet where the byte position would be, or -1 on error.</returns>
		///         <remarks>To be added.</remarks>
		public long ByteToPacket (long byteval, out int byteOffsetInPacket, out bool isEstimate)
		{
			AudioBytePacketTranslation buffer = default;
			buffer.Byte = byteval;

			unsafe {
				int size = sizeof (AudioBytePacketTranslation);
				if (AudioFileGetProperty (Handle, AudioFileProperty.ByteToPacket, &size, &buffer) == 0) {
					isEstimate = (buffer.Flags & BytePacketTranslationFlags.IsEstimate) != 0;
					byteOffsetInPacket = buffer.ByteOffsetInPacket;
					return buffer.Packet;
				}
				byteOffsetInPacket = 0;
				isEstimate = false;
				return -1;
			}
		}
	}

	/// <summary>Metadata-like information relating to a particular audio file.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class AudioFileInfoDictionary : DictionaryContainer {
		internal AudioFileInfoDictionary (NSDictionary dict)
			: base (dict)
		{
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Album {
			get {
				return GetStringValue ("album");
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? ApproximateDurationInSeconds {
			get {
				return GetStringValue ("approximate duration in seconds");
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Artist {
			get {
				return GetStringValue ("artist");
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? ChannelLayout {
			get {
				return GetStringValue ("channel layout");
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Composer {
			get {
				return GetStringValue ("composer");
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Comments {
			get {
				return GetStringValue ("comments");
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Copyright {
			get {
				return GetStringValue ("copyright");
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? EncodingApplication {
			get {
				return GetStringValue ("encoding application");
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Genre {
			get {
				return GetStringValue ("genre");
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? ISRC {
			get {
				return GetStringValue ("ISRC");
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? KeySignature {
			get {
				return GetStringValue ("key signature");
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Lyricist {
			get {
				return GetStringValue ("lyricist");
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? NominalBitRate {
			get {
				return GetStringValue ("nominal bit rate");
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? RecordedDate {
			get {
				return GetStringValue ("recorded date");
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? SourceBitDepth {
			get {
				return GetStringValue ("source bit depth");
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? SourceEncoder {
			get {
				return GetStringValue ("source encoder");
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? SubTitle {
			get {
				return GetStringValue ("subtitle");
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Tempo {
			get {
				return GetStringValue ("tempo");
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? TimeSignature {
			get {
				return GetStringValue ("time signature");
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Title {
			get {
				return GetStringValue ("title");
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? TrackNumber {
			get {
				return GetStringValue ("track number");
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Year {
			get {
				return GetStringValue ("year");
			}
		}
	}

	delegate int ReadProc (IntPtr clientData, long position, int requestCount, IntPtr buffer, out int actualCount);
	delegate int WriteProc (IntPtr clientData, long position, int requestCount, IntPtr buffer, out int actualCount);
	delegate long GetSizeProc (IntPtr clientData);
	delegate int SetSizeProc (IntPtr clientData, long size);

	/// <summary>A derived class from AudioFile that exposes virtual methods that can be hooked into (for reading and writing)</summary>
	///     <remarks>
	///       <para>
	/// AudioSource is an abstract class that derives from AudioFile that allows developers to hook up into the reading and writing stages of the AudioFile.   This can be used for example to read from an in-memory audio file, or to write to an in-memory buffer.
	/// </para>
	///       <para>
	/// When you write data into the AudioSource using any of the methods from AudioFile, instead of writing the encoded data into a file, the data is sent to the Read abstract method.
	/// </para>
	///       <para>
	/// To use this class, you must create a class that derives from AudioSource and override the Read, Write methods and the Size property.
	/// </para>
	///     </remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public abstract class AudioSource : AudioFile {
		GCHandle gch;

		[UnmanagedCallersOnly]
		static unsafe int SourceRead (IntPtr clientData, long inPosition, int requestCount, IntPtr buffer, int* actualCount)
		{
			GCHandle handle = GCHandle.FromIntPtr (clientData);
			var audioSource = handle.Target as AudioSource;
			var localCount = 0;
			var result = audioSource?.Read (inPosition, requestCount, buffer, out localCount) == true ? 0 : 1;
			*actualCount = localCount;
			return result;
		}

		/// <param name="position">Position in the audio stream that the data should be read from.</param>
		///         <param name="requestCount">Number of bytes to read.</param>
		///         <param name="buffer">Pointer to the buffer where the data should be stored.</param>
		///         <param name="actualCount">On return, set this value to the number of bytes actually read.</param>
		///         <summary>Callback invoked to read encoded audio data.</summary>
		///         <returns>true on success, false on failure.</returns>
		///         <remarks>This method is called by the AudioSource when more data is requested.</remarks>
		public abstract bool Read (long position, int requestCount, IntPtr buffer, out int actualCount);

		[UnmanagedCallersOnly]
		static unsafe int SourceWrite (IntPtr clientData, long position, int requestCount, IntPtr buffer, int* actualCount)
		{
			GCHandle handle = GCHandle.FromIntPtr (clientData);
			var audioSource = handle.Target as AudioSource;
			var localCount = 0;
			var result = audioSource?.Write (position, requestCount, buffer, out localCount) == true ? 0 : 1;
			*actualCount = localCount;
			return result;
		}
		/// <param name="position">Position where the data should be stored.</param>
		///         <param name="requestCount">Number of bytes to write.</param>
		///         <param name="buffer">Pointer to the buffer that contains the data to be written.</param>
		///         <param name="actualCount">Set this value to indicate the number of bytes actually written.</param>
		///         <summary>Callback used to write audio data into the audio stream.</summary>
		///         <returns>True on success, false on failure.</returns>
		///         <remarks>This method is called by the AudioSource when it has encoded the data and it need to write it out.</remarks>
		public abstract bool Write (long position, int requestCount, IntPtr buffer, out int actualCount);

		[UnmanagedCallersOnly]
		static long SourceGetSize (IntPtr clientData)
		{
			GCHandle handle = GCHandle.FromIntPtr (clientData);
			var audioSource = handle.Target as AudioSource;
			return audioSource?.Size ?? 0;
		}

		[UnmanagedCallersOnly]
		static int SourceSetSize (IntPtr clientData, long size)
		{
			GCHandle handle = GCHandle.FromIntPtr (clientData);
			var audioSource = handle.Target as AudioSource;

			if (audioSource is not null)
				audioSource.Size = size;
			return 0;
		}
		/// <summary>Used to set or get the size of the audio stream.</summary>
		///         <value>The size of the file.</value>
		///         <remarks>If the AudioSource is created in reading mode, this method should return the size of the audio data.   If the AudioSource is created to write data, this method is invoked to set the audio file size.</remarks>
		public abstract long Size { get; set; }

		/// <include file="../../docs/api/AudioToolbox/AudioSource.xml" path="/Documentation/Docs[@DocId='M:AudioToolbox.AudioSource.Dispose(System.Boolean)']/*" />
		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			if (gch.IsAllocated)
				gch.Free ();
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern unsafe static OSStatus AudioFileInitializeWithCallbacks (
			IntPtr inClientData,
			delegate* unmanaged<IntPtr, long, int, IntPtr, int*, int> inReadFunc,
			delegate* unmanaged<IntPtr, long, int, IntPtr, int*, int> inWriteFunc,
			delegate* unmanaged<IntPtr, long> inGetSizeFunc,
			delegate* unmanaged<IntPtr, long, int> inSetSizeFunc,
			AudioFileType inFileType, AudioStreamBasicDescription* format, uint flags, IntPtr* id);

		/// <param name="inFileType">To be added.</param>
		///         <param name="format">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public AudioSource (AudioFileType inFileType, AudioStreamBasicDescription format)
		{
			Initialize (inFileType, format);
		}

		/// <summary>Constructor used when creating subclasses</summary>
		///         <remarks>
		/// 	  This constructor is provided as a convenience for
		/// 	  developers that need to decouple the creation of the
		/// 	  AudioSource from starting the read and write process.    Once you have created this object, you need to invoke the <see cref="AudioToolbox.AudioSource.Initialize(AudioToolbox.AudioFileType,AudioToolbox.AudioStreamBasicDescription)" /> method to complete the setup.
		/// 	</remarks>
		public AudioSource ()
		{
		}

		/// <param name="inFileType">To be added.</param>
		///         <param name="format">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		protected void Initialize (AudioFileType inFileType, AudioStreamBasicDescription format)
		{
			gch = GCHandle.Alloc (this);
			int code = 0;
			IntPtr handle = IntPtr.Zero;
			unsafe {
				code = AudioFileInitializeWithCallbacks (GCHandle.ToIntPtr (gch), &SourceRead, &SourceWrite, &SourceGetSize, &SourceSetSize, inFileType, &format, 0, &handle);
			}
			if (code == 0) {
				InitializeHandle (handle);
				return;
			}
			throw new Exception (String.Format ("Unable to create AudioSource, code: 0x{0:x}", code));
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static unsafe int AudioFileOpenWithCallbacks (
			IntPtr inClientData,
			delegate* unmanaged<IntPtr, long, int, IntPtr, int*, int> inReadFunc,
			delegate* unmanaged<IntPtr, long, int, IntPtr, int*, int> inWriteFunc,
			delegate* unmanaged<IntPtr, long> inGetSizeFunc,
			delegate* unmanaged<IntPtr, long, int> inSetSizeFunc,
			AudioFileType inFileTypeHint, IntPtr* outAudioFile);

		/// <param name="fileTypeHint">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public AudioSource (AudioFileType fileTypeHint)
		{
			Open (fileTypeHint);
		}

		/// <param name="fileTypeHint">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		protected void Open (AudioFileType fileTypeHint)
		{
			gch = GCHandle.Alloc (this);
			int code = 0;
			IntPtr handle = IntPtr.Zero;
			unsafe {
				code = AudioFileOpenWithCallbacks (GCHandle.ToIntPtr (gch), &SourceRead, &SourceWrite, &SourceGetSize, &SourceSetSize, fileTypeHint, &handle);
			}
			if (code == 0) {
				InitializeHandle (handle);
				return;
			}
			throw new Exception (String.Format ("Unable to create AudioSource, code: 0x{0:x}", code));
		}
	}
}

