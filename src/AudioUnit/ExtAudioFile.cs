//
// ExtAudioFile.cs: ExtAudioFile wrapper class
//
// Authors:
//   AKIHIRO Uehara (u-akihiro@reinforce-lab.com)
//   Marek Safar (marek.safar@gmail.com)
//
// Copyright 2010 Reinforce Lab.
// Copyright 2012 Xamarin Inc.
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

#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using ObjCRuntime;
using CoreFoundation;
using AudioToolbox;
using Foundation;
using System.Runtime.Versioning;

namespace AudioUnit {
	/// <summary>An enumeration whose values indicate various errors relating to <see cref="ExtAudioFile" />s.</summary>
	///     <remarks>To be added.</remarks>
	public enum ExtAudioFileError // Implictly cast to OSType
	{
		/// <summary>To be added.</summary>
		OK = 0,
		/// <summary>To be added.</summary>
		CodecUnavailableInputConsumed = -66559,
		/// <summary>To be added.</summary>
		CodecUnavailableInputNotConsumed = -66560,
		/// <summary>To be added.</summary>
		InvalidProperty = -66561,
		/// <summary>To be added.</summary>
		InvalidPropertySize = -66562,
		/// <summary>To be added.</summary>
		NonPCMClientFormat = -66563,
		/// <summary>To be added.</summary>
		InvalidChannelMap = -66564,
		/// <summary>To be added.</summary>
		InvalidOperationOrder = -66565,
		/// <summary>To be added.</summary>
		InvalidDataFormat = -66566,
		/// <summary>To be added.</summary>
		MaxPacketSizeUnknown = -66567,
		/// <summary>To be added.</summary>
		InvalidSeek = -66568,
		/// <summary>To be added.</summary>
		AsyncWriteTooLarge = -66569,
		/// <summary>To be added.</summary>
		AsyncWriteBufferOverflow = -66570,

		// Shared error codes
		/// <summary>To be added.</summary>
		NotOpenError = -38,
		/// <summary>To be added.</summary>
		EndOfFileError = -39,
		/// <summary>To be added.</summary>
		PositionError = -40,
		/// <summary>To be added.</summary>
		FileNotFoundError = -43,
		/// <summary>To be added.</summary>
		BadFilePathError = 0x21707468, // '!pth'
		/// <summary>To be added.</summary>
		FilePermissionError = -54,
		/// <summary>To be added.</summary>
		TooManyFilesOpenError = -42,
	}

#if NET
	/// <summary>The ExtendedAudioFile provides high-level audio file access. It provides a single unified interface to reading and writing both encoded and unencoded files with access to <see cref="AudioToolbox.AudioFile" /> and <see cref="AudioToolbox.AudioConverter" /> API. 
	///     </summary>
	///     <remarks>
	///     </remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
#endif
	public class ExtAudioFile : IDisposable {
		IntPtr _extAudioFile;

		/// <summary>The client data format's maximum packet size in bytes.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public uint? ClientMaxPacketSize {
			get {
				uint size = sizeof (uint);
				uint value;
				unsafe {
					if (ExtAudioFileGetProperty (_extAudioFile, PropertyIDType.ClientMaxPacketSize, &size, &value) != ExtAudioFileError.OK)
						return null;
				}

				return value;
			}
		}

		/// <summary>The file data format's maximum packet size in bytes.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public uint? FileMaxPacketSize {
			get {
				uint size = sizeof (uint);
				uint value;

				unsafe {
					if (ExtAudioFileGetProperty (_extAudioFile, PropertyIDType.FileMaxPacketSize, &size, &value) != ExtAudioFileError.OK)
						return null;
				}

				return value;
			}
		}


		/// <summary>The underlying AudioFile.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public IntPtr? AudioFile {
			get {
				uint size = (uint) IntPtr.Size;
				IntPtr value;

				unsafe {
					if (ExtAudioFileGetProperty (_extAudioFile, PropertyIDType.AudioFile, &size, &value) != ExtAudioFileError.OK)
						return null;
				}

				return value;
			}
		}

		/// <summary>Returns underlying <see cref="AudioToolbox.AudioConverter" /> instance.</summary>
		///         <value>
		///         </value>
		///         <remarks>AudioConverter changes are not propagated automatically. After changing any property <see cref="SynchronizeAudioConverter" /> method has to be called to synchronize the converter output format with the file data format.</remarks>
		public AudioConverter? AudioConverter {
			get {
				uint size = sizeof (uint);
				IntPtr value;

				unsafe {
					if (ExtAudioFileGetProperty (_extAudioFile, PropertyIDType.AudioConverter, &size, &value) != ExtAudioFileError.OK)
						return null;
				}

				return new AudioConverter (value, false);
			}
		}

		/// <summary>The file's length in sample frames.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public long FileLengthFrames {
			get {
				long length;
				uint size = sizeof (long);

				unsafe {
					var err = ExtAudioFileGetProperty (_extAudioFile, PropertyIDType.FileLengthFrames, &size, &length);
					if (err != 0) {
						throw new InvalidOperationException (String.Format ("Error code:{0}", err));
					}
				}

				return length;
			}
		}

		/// <summary>The file's actual data format.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public AudioStreamBasicDescription FileDataFormat {
			get {
				AudioStreamBasicDescription dc = new AudioStreamBasicDescription ();
				uint size = (uint) Marshal.SizeOf<AudioStreamBasicDescription> ();
				unsafe {
					int err = ExtAudioFileGetProperty (_extAudioFile, PropertyIDType.FileDataFormat, &size, &dc);
					if (err != 0) {
						throw new InvalidOperationException (String.Format ("Error code:{0}", err));
					}
				}

				return dc;
			}
		}

		/// <summary>Specified the format of linear PCM.</summary>
		///         <value>
		///         </value>
		///         <remarks>The property has to be set when encoding or decoding a non-PCM file data format.</remarks>
		public AudioStreamBasicDescription ClientDataFormat {
			get {
				uint size = (uint) Marshal.SizeOf<AudioStreamBasicDescription> ();
				AudioStreamBasicDescription value = new AudioStreamBasicDescription ();
				unsafe {
					if (ExtAudioFileGetProperty (_extAudioFile, PropertyIDType.ClientDataFormat, &size, &value) != (int) ExtAudioFileError.OK)
						return default (AudioStreamBasicDescription);
				}

				return value;
			}

			set {
				int err;
				unsafe {
					err = ExtAudioFileSetProperty (_extAudioFile, PropertyIDType.ClientDataFormat,
					(uint) Marshal.SizeOf<AudioStreamBasicDescription> (), &value);
				}
				if (err != 0) {
					throw new InvalidOperationException (String.Format ("Error code:{0}", err));
				}
			}
		}

		private ExtAudioFile (IntPtr ptr)
		{
			_extAudioFile = ptr;
		}

		~ExtAudioFile ()
		{
			Dispose (false);
		}

		// Since throwing and ArgumentException seems like a bad idea due to you do not get acces programatically
		// to the actual error code from the native API and we are not allowed to make Breaking Changes
		// lets reimplement the method in a way to return the actual native value if any
		// also we can share the underliying implementation so we so not break api and reduce code suplication
		/// <param name="url">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static ExtAudioFile? OpenUrl (NSUrl url, out ExtAudioFileError error)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));

			ExtAudioFile? audioFile = OpenUrl (url.Handle, out error);
			GC.KeepAlive (url);
			return audioFile;
		}

		/// <param name="url">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static ExtAudioFile? OpenUrl (CFUrl url, out ExtAudioFileError error)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));

			ExtAudioFile? audioFile = OpenUrl (url.Handle, out error);
			GC.KeepAlive (url);
			return audioFile;
		}

		/// <param name="url">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static ExtAudioFile OpenUrl (CFUrl url)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));

			ExtAudioFileError err;
			var audioFile = OpenUrl (url.Handle, out err);
			GC.KeepAlive (url);

			if (err != ExtAudioFileError.OK) // if (err != 0)  <- to keep old implementation
				throw new ArgumentException (String.Format ("Error code:{0}", err));
			if (audioFile is null) // if (ptr == IntPtr.Zero)  <- to keep old implementation
				throw new InvalidOperationException ("Can not get object instance");

			return audioFile;
		}

		static ExtAudioFile? OpenUrl (IntPtr urlHandle, out ExtAudioFileError error)
		{
			IntPtr ptr;
			unsafe {
				error = ExtAudioFileOpenUrl (urlHandle, &ptr);
			}

			if (error != ExtAudioFileError.OK || ptr == IntPtr.Zero)
				return null;
			else
				return new ExtAudioFile (ptr);
		}

		// Since throwing and ArgumentException seems like a bad idea due to you do not get acces programatically
		// to the actual error code from the native API and we are not allowed to make Breaking Changes
		// lets reimplement the method in a way to return the actual native value if any
		// also we can share the underliying implementation so we so not break api and reduce code suplication
		/// <param name="url">To be added.</param>
		///         <param name="fileType">To be added.</param>
		///         <param name="inStreamDesc">To be added.</param>
		///         <param name="fileFlags">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static ExtAudioFile? CreateWithUrl (NSUrl url, AudioFileType fileType, AudioStreamBasicDescription inStreamDesc, AudioFileFlags fileFlags, out ExtAudioFileError error)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));

			ExtAudioFile? audioFile = CreateWithUrl (url.Handle, fileType, inStreamDesc, fileFlags, out error);
			GC.KeepAlive (url);
			return audioFile;
		}

		/// <param name="url">To be added.</param>
		///         <param name="fileType">To be added.</param>
		///         <param name="inStreamDesc">To be added.</param>
		///         <param name="flag">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static ExtAudioFile? CreateWithUrl (CFUrl url, AudioFileType fileType, AudioStreamBasicDescription inStreamDesc, AudioFileFlags flag, out ExtAudioFileError error)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));

			ExtAudioFile? audioFile = CreateWithUrl (url.Handle, fileType, inStreamDesc, flag, out error);
			GC.KeepAlive (url);
			return audioFile;
		}

		/// <param name="url">To be added.</param>
		///         <param name="fileType">To be added.</param>
		///         <param name="inStreamDesc">To be added.</param>
		///         <param name="flag">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static ExtAudioFile CreateWithUrl (CFUrl url,
			AudioFileType fileType,
			AudioStreamBasicDescription inStreamDesc,
			//AudioChannelLayout channelLayout, 
			AudioFileFlags flag)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));

			ExtAudioFileError err;
			var audioFile = CreateWithUrl (url.Handle, fileType, inStreamDesc, flag, out err);
			GC.KeepAlive (url);

			if (err != ExtAudioFileError.OK) // if (err != 0)  <- to keep old implementation
				throw new ArgumentException (String.Format ("Error code:{0}", err));
			if (audioFile is null) // if (ptr == IntPtr.Zero)  <- to keep old implementation
				throw new InvalidOperationException ("Can not get object instance");

			return audioFile;
		}

		static ExtAudioFile? CreateWithUrl (IntPtr urlHandle, AudioFileType fileType, AudioStreamBasicDescription inStreamDesc, AudioFileFlags flag, out ExtAudioFileError error)
		{
			IntPtr ptr;
			unsafe {
				error = (ExtAudioFileError) ExtAudioFileCreateWithUrl (urlHandle, fileType, &inStreamDesc, IntPtr.Zero, (uint) flag, &ptr);
			}
			if (error != ExtAudioFileError.OK || ptr == IntPtr.Zero)
				return null;
			else
				return new ExtAudioFile (ptr);
		}

		/// <param name="audioFileID">To be added.</param>
		///         <param name="forWriting">To be added.</param>
		///         <param name="outAudioFile">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static ExtAudioFileError WrapAudioFileID (IntPtr audioFileID, bool forWriting, out ExtAudioFile? outAudioFile)
		{
			IntPtr ptr;
			ExtAudioFileError res;
			unsafe {
				res = ExtAudioFileWrapAudioFileID (audioFileID, forWriting ? (byte) 1 : (byte) 0, (IntPtr) (&ptr));
			}

			if (res != ExtAudioFileError.OK) {
				outAudioFile = null;
				return res;
			}

			outAudioFile = new ExtAudioFile (ptr);
			return res;
		}

		/// <param name="frameOffset">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void Seek (long frameOffset)
		{
			int err = ExtAudioFileSeek (_extAudioFile, frameOffset);
			if (err != 0) {
				throw new ArgumentException (String.Format ("Error code:{0}", err));
			}
		}
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public long FileTell ()
		{
			long frame = 0;
			int err;
			unsafe {
				err = ExtAudioFileTell (_extAudioFile, &frame);
			}
			if (err != 0) {
				throw new ArgumentException (String.Format ("Error code:{0}", err));
			}

			return frame;
		}

		/// <param name="numberFrames">To be added.</param>
		///         <param name="audioBufferList">To be added.</param>
		///         <param name="status">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public uint Read (uint numberFrames, AudioBuffers audioBufferList, out ExtAudioFileError status)
		{
			if (audioBufferList is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (audioBufferList));

			unsafe {
				status = ExtAudioFileRead (_extAudioFile, &numberFrames, (IntPtr) audioBufferList);
			}
			return numberFrames;
		}

		/// <param name="numberFrames">To be added.</param>
		///         <param name="audioBufferList">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public ExtAudioFileError WriteAsync (uint numberFrames, AudioBuffers audioBufferList)
		{
			if (audioBufferList is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (audioBufferList));

			return ExtAudioFileWriteAsync (_extAudioFile, numberFrames, (IntPtr) audioBufferList);
		}

		/// <param name="numberFrames">To be added.</param>
		///         <param name="audioBufferList">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public ExtAudioFileError Write (uint numberFrames, AudioBuffers audioBufferList)
		{
			if (audioBufferList is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (audioBufferList));

			return ExtAudioFileWrite (_extAudioFile, numberFrames, (IntPtr) audioBufferList);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public ExtAudioFileError SynchronizeAudioConverter ()
		{
			IntPtr value = IntPtr.Zero;
			return ExtAudioFileSetProperty (_extAudioFile, PropertyIDType.ConverterConfig,
				IntPtr.Size, value);
		}

		/// <summary>Releases the resources used by the ExtAudioFile object.</summary>
		///         <remarks>
		///           <para>The Dispose method releases the resources used by the ExtAudioFile class.</para>
		///           <para>Calling the Dispose method when the application is finished using the ExtAudioFile ensures that all external resources used by this managed object are released as soon as possible.  Once developers have invoked the Dispose method, the object is no longer useful and developers should no longer make any calls to it.  For more information on releasing resources see ``Cleaning up Unmananaged Resources'' at https://msdn.microsoft.com/en-us/library/498928w2.aspx</para>
		///         </remarks>
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		/// <include file="../../docs/api/AudioUnit/ExtAudioFile.xml" path="/Documentation/Docs[@DocId='M:AudioUnit.ExtAudioFile.Dispose(System.Boolean)']/*" />
		protected virtual void Dispose (bool disposing)
		{
			if (_extAudioFile != IntPtr.Zero) {
				ExtAudioFileDispose (_extAudioFile);
				_extAudioFile = IntPtr.Zero;
			}
		}

		#region Interop
		[DllImport (Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileOpenURL")]
		unsafe static extern ExtAudioFileError ExtAudioFileOpenUrl (IntPtr inUrl, IntPtr* outExtAudioFile);

		[DllImport (Constants.AudioToolboxLibrary)]
		static extern ExtAudioFileError ExtAudioFileWrapAudioFileID (IntPtr inFileID, byte inForWriting, IntPtr outExtAudioFile);

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe static extern ExtAudioFileError ExtAudioFileRead (IntPtr inExtAudioFile, uint* /* UInt32* */ ioNumberFrames, IntPtr ioData);

		[DllImport (Constants.AudioToolboxLibrary)]
		static extern ExtAudioFileError ExtAudioFileWrite (IntPtr inExtAudioFile, uint /* UInt32 */ inNumberFrames, IntPtr ioData);

		[DllImport (Constants.AudioToolboxLibrary)]
		static extern ExtAudioFileError ExtAudioFileWriteAsync (IntPtr inExtAudioFile, uint /* UInt32 */ inNumberFrames, IntPtr ioData);

		[DllImport (Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileDispose")]
		static extern int /* OSStatus */ ExtAudioFileDispose (IntPtr inExtAudioFile);

		[DllImport (Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileSeek")]
		static extern int /* OSStatus */ ExtAudioFileSeek (IntPtr inExtAudioFile, long /* SInt64 */ inFrameOffset);

		[DllImport (Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileTell")]
		unsafe static extern int /* OSStatus */ ExtAudioFileTell (IntPtr inExtAudioFile, long* /* SInt64* */ outFrameOffset);

		[DllImport (Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileCreateWithURL")]
		unsafe static extern int /* OSStatus */ ExtAudioFileCreateWithUrl (IntPtr inURL,
			AudioFileType inFileType,
			AudioStreamBasicDescription* inStreamDesc,
			IntPtr inChannelLayout, //AudioChannelLayout inChannelLayout, AudioChannelLayout results in compilation error (error code 134.)
			UInt32 /* UInt32 */ flags,
			IntPtr* outExtAudioFile);

		[DllImport (Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileGetProperty")]
		unsafe static extern int ExtAudioFileGetProperty (
			IntPtr inExtAudioFile,
			PropertyIDType inPropertyID,
			uint* /* UInt32* */ ioPropertyDataSize,
			IntPtr outPropertyData);

		[DllImport (Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileGetProperty")]
		unsafe static extern int ExtAudioFileGetProperty (
			IntPtr inExtAudioFile,
			PropertyIDType inPropertyID,
			uint* /* UInt32* */ ioPropertyDataSize,
			AudioStreamBasicDescription* outPropertyData);

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe static extern ExtAudioFileError ExtAudioFileGetProperty (IntPtr inExtAudioFile, PropertyIDType inPropertyID, uint* /* UInt32* */ ioPropertyDataSize, IntPtr* outPropertyData);

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe static extern ExtAudioFileError ExtAudioFileGetProperty (IntPtr inExtAudioFile, PropertyIDType inPropertyID, uint* /* UInt32* */ ioPropertyDataSize, long* outPropertyData);

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe static extern ExtAudioFileError ExtAudioFileGetProperty (IntPtr inExtAudioFile, PropertyIDType inPropertyID, uint* /* UInt32* */ ioPropertyDataSize, uint* outPropertyData);

		[DllImport (Constants.AudioToolboxLibrary)]
		static extern ExtAudioFileError ExtAudioFileSetProperty (IntPtr inExtAudioFile, PropertyIDType inPropertyID, int /* UInt32 */ ioPropertyDataSize, IntPtr outPropertyData);

		[DllImport (Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileSetProperty")]
		unsafe static extern int ExtAudioFileSetProperty (
			IntPtr inExtAudioFile,
			PropertyIDType inPropertyID,
			uint /* UInt32 */ ioPropertyDataSize,
			AudioStreamBasicDescription* outPropertyData);

		enum PropertyIDType { // UInt32 ExtAudioFilePropertyID
			FileDataFormat = 0x66666d74,       // 'ffmt'
											   //kExtAudioFileProperty_FileChannelLayout		= 'fclo',   // AudioChannelLayout

			ClientDataFormat = 0x63666d74, //'cfmt',   // AudioStreamBasicDescription
										   //kExtAudioFileProperty_ClientChannelLayout	= 'cclo',   // AudioChannelLayout
			CodecManufacturer = 0x636d616e,      // 'cman'

			// read-only:
			AudioConverter = 0x61636e76,      // 'acnv'
			AudioFile = 0x6166696c,      // 'afil'
			FileMaxPacketSize = 0x666d7073,      // 'fmps'
			ClientMaxPacketSize = 0x636d7073,      // 'cmps'
			FileLengthFrames = 0x2366726d,      // '#frm'

			// writable:
			ConverterConfig = 0x61636366,      // 'accf'
											   //kExtAudioFileProperty_IOBufferSizeBytes		= 'iobs',	// UInt32
											   //kExtAudioFileProperty_IOBuffer				= 'iobf',	// void *
											   //kExtAudioFileProperty_PacketTable			= 'xpti'	// AudioFilePacketTableInfo             
		};
		#endregion
	}
}
