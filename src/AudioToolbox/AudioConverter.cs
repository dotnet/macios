//
// AudioConverter.cs: AudioConverter wrapper class
//
// Authors:
//   Marek Safar (marek.safar@gmail.com)
//
// Copyright 2013 Xamarin Inc.
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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace AudioToolbox {
	/// <summary>An enumeration whose values specify various types of errors relating to the <see cref="AudioToolbox.AudioConverter" />.</summary>
	///     <remarks>To be added.</remarks>
	public enum AudioConverterError // Impliclty cast to OSStatus in AudioConverter.h
	{
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>One or more of the parameters are invalid.</summary>
		ParameterError = -50,
		/// <summary>To be added.</summary>
		FormatNotSupported = 0x666d743f, // 'fmt?'
		/// <summary>To be added.</summary>
		OperationNotSupported = 0x6f703f3f, // 'op??'
		/// <summary>To be added.</summary>
		PropertyNotSupported = 0x70726f70, // 'prop'
		/// <summary>To be added.</summary>
		InvalidInputSize = 0x696e737a, // 'insz'
		/// <summary>To be added.</summary>
		InvalidOutputSize = 0x6f74737a, // 'otsz'
		/// <summary>To be added.</summary>
		UnspecifiedError = 0x77686174, // 'what'
		/// <summary>To be added.</summary>
		BadPropertySizeError = 0x2173697a, // '!siz'
		/// <summary>To be added.</summary>
		RequiresPacketDescriptionsError = 0x21706b64, // '!pkd'
		/// <summary>To be added.</summary>
		InputSampleRateOutOfRange = 0x21697372, // '!isr'
		/// <summary>To be added.</summary>
		OutputSampleRateOutOfRange = 0x216f7372, // '!osr'
		/// <summary>To be added.</summary>
		HardwareInUse = 0x68776975, // 'hwiu'
		/// <summary>To be added.</summary>
		NoHardwarePermission = 0x7065726d, // 'perm'
		/// <summary>To be added.</summary>
		AudioFormatUnsupported = 0x21646174, // '!dat' From http://lists.apple.com/archives/coreaudio-api/2009/Feb/msg00082.html
	}

	/// <summary>Constants for the sample rate conversion algorithm.</summary>
	public enum AudioConverterSampleRateConverterComplexity // typedef UInt32 AudioConverterPropertyID
	{
		/// <summary>Represents lowest quality sample rate.</summary>
		Linear = 0x6c696e65, // 'line'
		/// <summary>Represents normal quality sample rate.</summary>
		Normal = 0x6e6f726d, // 'norm'
		/// <summary>Represents mastering quality sample rate.</summary>
		Mastering = 0x62617473, // 'bats'
	}

	/// <summary>Constants for the rendering quality of the sample rate converter.</summary>
	public enum AudioConverterQuality // typedef UInt32 AudioConverterPropertyID
	{
		/// <summary>Represents maximum quality.</summary>
		Max = 0x7F,
		/// <summary>Represents high quality.</summary>
		High = 0x60,
		/// <summary>Represents medium quality.</summary>
		Medium = 0x40,
		/// <summary>Represents low quality.</summary>
		Low = 0x20,
		/// <summary>Represents minimum quality.</summary>
		Min = 0,
	}

	/// <summary>The prime method constants.</summary>
	public enum AudioConverterPrimeMethod // typedef UInt32 AudioConverterPropertyID
	{
		/// <summary>Represents primes with both leading and trailing input frames.</summary>
		Pre = 0,
		/// <summary>Represents primes with trailing input frames only.</summary>
		Normal = 1,
		/// <summary>Represents primes with both leading and trailing input frames to be silence.</summary>
		None = 2,
	}

	[Flags]
	[SupportedOSPlatform ("ios18.0")]
	[SupportedOSPlatform ("maccatalyst18.0")]
	[SupportedOSPlatform ("macos15.0")]
	[SupportedOSPlatform ("tvos18.0")]
	public enum AudioConverterOptions : uint {
		None = 0,
		Unbuffered = 1 << 16,
	}

	/// <summary>The priming information for an audio converter.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AudioConverterPrimeInfo {
		/// <summary>The number of leading input frames.</summary>
		public int LeadingFrames;
		/// <summary>The number of trailing input frames.</summary>
		public int TrailingFrames;
	}

	/// <include file="../../docs/api/AudioToolbox/AudioConverterComplexInputData.xml" path="/Documentation/Docs[@DocId='T:AudioToolbox.AudioConverterComplexInputData']/*" />
	public delegate AudioConverterError AudioConverterComplexInputData (ref int numberDataPackets, AudioBuffers data,
		ref AudioStreamPacketDescription []? dataPacketDescription);

	/// <summary>The linear PCM audio formats converter.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class AudioConverter : DisposableObject {
		IntPtr packetDescriptions;
		int packetDescriptionSize;

		public event AudioConverterComplexInputData? InputData;

		[Preserve (Conditional = true)]
		internal AudioConverter (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <summary>The size in bytes of the smallest buffer of input data.</summary>
		public uint MinimumInputBufferSize {
			get {
				return GetProperty<uint> (AudioConverterPropertyID.MinimumInputBufferSize);
			}
		}

		/// <summary>The size in bytes of the smallest buffer of output data.</summary>
		public uint MinimumOutputBufferSize {
			get {
				return GetProperty<uint> (AudioConverterPropertyID.MinimumOutputBufferSize);
			}
		}

		/// <summary>The size in bytes of the largest single packet of data in the input format.</summary>
		public uint MaximumInputPacketSize {
			get {
				return GetProperty<uint> (AudioConverterPropertyID.MaximumInputPacketSize);
			}
		}

		/// <summary>The size in bytes of the largest single packet of data in the output format.</summary>
		public uint MaximumOutputPacketSize {
			get {
				return GetProperty<uint> (AudioConverterPropertyID.MaximumOutputPacketSize);
			}
		}

		/// <summary>On input, the desired size (in bytes) of the output data. On output, the size (in bytes) of the input required to generate the desired output data size.</summary>
		public uint CalculateInputBufferSize {
			get {
				return GetProperty<uint> (AudioConverterPropertyID.CalculateInputBufferSize);
			}
		}

		/// <summary>On input, the desired size (in bytes) of the input data. On output, the size (in bytes) of the output data that will be generated from the desired input data size.</summary>
		public uint CalculateOutputBufferSize {
			get {
				return GetProperty<uint> (AudioConverterPropertyID.CalculateOutputBufferSize);
			}
		}

		/// <summary>The initial sub-sample position of the sample rate converter.</summary>
		public double SampleRateConverterInitialPhase {
			get {
				return GetProperty<double> (AudioConverterPropertyID.SampleRateConverterInitialPhase);
			}
			set {
				SetProperty (AudioConverterPropertyID.SampleRateConverterInitialPhase, value);
			}
		}

		/// <summary>The sample rate converter algorithm.</summary>
		public AudioConverterSampleRateConverterComplexity SampleRateConverterComplexity {
			get {
				return GetProperty<AudioConverterSampleRateConverterComplexity> (AudioConverterPropertyID.SampleRateConverterComplexity);
			}
			set {
				SetProperty (AudioConverterPropertyID.SampleRateConverterComplexity, (uint) value);
			}
		}

		/// <summary>The rendering quality of the sample rate converter.</summary>
		public AudioConverterQuality SampleRateConverterQuality {
			get {
				return GetProperty<AudioConverterQuality> (AudioConverterPropertyID.SampleRateConverterQuality);
			}
			set {
				SetProperty (AudioConverterPropertyID.SampleRateConverterQuality, (uint) value);
			}
		}

		/// <summary>Rendering quality of the converter codec.</summary>
		public AudioConverterQuality CodecQuality {
			get {
				return GetProperty<AudioConverterQuality> (AudioConverterPropertyID.CodecQuality);
			}
			set {
				SetProperty (AudioConverterPropertyID.CodecQuality, (uint) value);
			}
		}

		/// <summary>The priming information for converter's priming method.</summary>
		public AudioConverterPrimeMethod PrimeMethod {
			get {
				return GetProperty<AudioConverterPrimeMethod> (AudioConverterPropertyID.PrimeMethod);
			}
			set {
				SetProperty (AudioConverterPropertyID.PrimeMethod, (uint) value);
			}
		}

		/// <summary>The priming method.</summary>
		public unsafe AudioConverterPrimeInfo PrimeInfo {
			get {
				return GetProperty<AudioConverterPrimeInfo> (AudioConverterPropertyID.PrimeInfo);
			}
		}

		/// <summary>Input to Output channel mapping.</summary>
		public int []? ChannelMap {
			get {
				return GetArray<int> (AudioConverterPropertyID.ChannelMap);
			}
		}

		/// <summary>Gets or sets a magic cookie that is used for compression.</summary>
		public byte []? CompressionMagicCookie {
			get {
				return GetArray<byte> (AudioConverterPropertyID.CompressionMagicCookie);
			}

			set {
				SetArray (AudioConverterPropertyID.CompressionMagicCookie, value);
			}
		}

		/// <summary>Gets or sets a magic cookie that is used for decompression.</summary>
		/// <remarks>If the audio data format has a magic cookie associated with it, you must add this information to appropriately decompress the data.</remarks>
		public byte []? DecompressionMagicCookie {
			get {
				return GetArray<byte> (AudioConverterPropertyID.DecompressionMagicCookie);
			}
			set {
				SetArray (AudioConverterPropertyID.DecompressionMagicCookie, value);
			}
		}

		/// <summary>The number of bits per second to aim for when encoding data.</summary>
		public uint EncodeBitRate {
			get {
				return GetProperty<uint> (AudioConverterPropertyID.EncodeBitRate);
			}
			set {
				SetProperty (AudioConverterPropertyID.EncodeBitRate, value);
			}
		}

		/// <summary>An an output sample rate.</summary>
		public double EncodeAdjustableSampleRate {
			get {
				return GetProperty<double> (AudioConverterPropertyID.EncodeAdjustableSampleRate);
			}
			set {
				SetProperty (AudioConverterPropertyID.EncodeAdjustableSampleRate, value);
			}
		}

		/// <summary>Input audio channels layout.</summary>
		public AudioChannelLayout? InputChannelLayout {
			get {
				var res = GetPropertyIntoNativeMemory (AudioConverterPropertyID.InputChannelLayout, out var ptr);
				var layout = res == AudioConverterError.None ? new AudioChannelLayout (ptr) : null;
				Marshal.FreeHGlobal (ptr);
				return layout;
			}
		}

		/// <summary>Output audio channels layout.</summary>
		public AudioChannelLayout? OutputChannelLayout {
			get {
				var res = GetPropertyIntoNativeMemory (AudioConverterPropertyID.OutputChannelLayout, out var ptr);
				var layout = res == AudioConverterError.None ? new AudioChannelLayout (ptr) : null;
				Marshal.FreeHGlobal (ptr);
				return layout;
			}
		}

		/// <summary>All applicable bit rates based on current settings.</summary>
		public AudioValueRange []? ApplicableEncodeBitRates {
			get {
				return GetArray<AudioValueRange> (AudioConverterPropertyID.ApplicableEncodeBitRates);
			}
		}

		/// <summary>All available bit rates for the input format.</summary>
		public AudioValueRange []? AvailableEncodeBitRates {
			get {
				return GetArray<AudioValueRange> (AudioConverterPropertyID.AvailableEncodeBitRates);
			}
		}

		/// <summary>All applicable sample rates based on current settings.</summary>
		public AudioValueRange []? ApplicableEncodeSampleRates {
			get {
				return GetArray<AudioValueRange> (AudioConverterPropertyID.ApplicableEncodeSampleRates);
			}
		}

		/// <summary>All applicable sample rates based on current settings.</summary>
		public AudioValueRange []? AvailableEncodeSampleRates {
			get {
				return GetArray<AudioValueRange> (AudioConverterPropertyID.AvailableEncodeSampleRates);
			}
		}

		/// <summary>All audio channel layouts for the input format.</summary>
		public AudioChannelLayoutTag []? AvailableEncodeChannelLayoutTags {
			get {
				return GetArray<AudioChannelLayoutTag> (AudioConverterPropertyID.AvailableEncodeChannelLayoutTags);
			}
		}

		/// <summary>Completely filled output audio description.</summary>
		/// <remarks>The property can be used to obtain converter filled <see cref="AudioToolbox.AudioStreamBasicDescription" /> for output audio stream.</remarks>
		public unsafe AudioStreamBasicDescription CurrentOutputStreamDescription {
			get {
				return GetProperty<AudioStreamBasicDescription> (AudioConverterPropertyID.CurrentOutputStreamDescription);
			}
		}

		/// <summary>Completely filled input audio description.</summary>
		/// <remarks>The property can be used to obtain converter filled <see cref="AudioToolbox.AudioStreamBasicDescription" /> for input audio stream.</remarks>
		public unsafe AudioStreamBasicDescription CurrentInputStreamDescription {
			get {
				return GetProperty<AudioStreamBasicDescription> (AudioConverterPropertyID.CurrentInputStreamDescription);
			}
		}

		/// <summary>The source bit depth to preserve.</summary>
		public int BitDepthHint {
			get {
				return GetProperty<int> (AudioConverterPropertyID.PropertyBitDepthHint);
			}
			set {
				SetProperty (AudioConverterPropertyID.PropertyBitDepthHint, value);
			}
		}

		/// <summary>All the data formats produced by the converter encoder.</summary>
		public unsafe AudioFormat []? FormatList {
			get {
				return GetArray<AudioFormat> (AudioConverterPropertyID.PropertyFormatList);
			}
		}

#if !MONOMAC
		/// <summary>The underlying codec supports resumption following an interruption.</summary>
		public bool CanResumeFromInterruption {
			get {
				return GetProperty<uint> (AudioConverterPropertyID.CanResumeFromInterruption) != 0;
			}
		}
#endif

		/// <summary>Whether to perform a mix from input to output channels.</summary>
		/// <remarks>Use <see cref="ChannelMixMap" /> to specify how the mix is done.</remarks>
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		public bool PerformDownmix {
			get {
				return GetProperty<uint> (AudioConverterPropertyID.PerformDownmix) != 0;
			}
			set {
				SetProperty (AudioConverterPropertyID.PerformDownmix, value ? 1 : 0);
			}
		}

		/// <summary>An array of gain values to apply to input and output channels. Each gain value is a value between 0.0 and 1.0.</summary>
		/// <remarks><see cref="PerformDownmix" /> must be set to <see langword="true" /> first.</remarks>
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		public float []? ChannelMixMap {
			get {
				return GetArray<float> (AudioConverterPropertyID.ChannelMixMap);
			}
			set {
				SetArray (AudioConverterPropertyID.ChannelMixMap, value);
			}
		}
		/// <summary>Creates a new audio converter instance based on specified audio formats.</summary>
		/// <param name="sourceFormat">Input audio format.</param>
		/// <param name="destinationFormat">Output audio format.</param>
		/// <returns>A new <see cref="AudioConverter" /> instance if successful, <see langword="null" /> otherwise.</returns>
		public static AudioConverter? Create (AudioStreamBasicDescription sourceFormat, AudioStreamBasicDescription destinationFormat)
		{
			AudioConverterError res;
			return Create (sourceFormat, destinationFormat, out res);
		}

		/// <summary>Creates a new audio converter instance using a specified codec.</summary>
		/// <param name="sourceFormat">The format of the source audio.</param>
		/// <param name="destinationFormat">The destination audio format.</param>
		/// <param name="error">In case of failure, will contain the error code for the failure. Otherwise the value <see cref="AudioConverterError.None" /> will be returned.</param>
		/// <returns>A new <see cref="AudioConverter" /> instance if successful, <see langword="null" /> otherwise.</returns>
		public static AudioConverter? Create (AudioStreamBasicDescription sourceFormat, AudioStreamBasicDescription destinationFormat, out AudioConverterError error)
		{
			IntPtr ptr = new IntPtr ();
			unsafe {
				error = AudioConverterNew (&sourceFormat, &destinationFormat, &ptr);
			}
			if (error != AudioConverterError.None)
				return null;

			return new AudioConverter (ptr, true);
		}

		/// <summary>Creates a new audio converter instance using a specified codec.</summary>
		/// <param name="sourceFormat">Input audio format.</param>
		/// <param name="destinationFormat">Output audio format.</param>
		/// <param name="descriptions">A list of codec to be used.</param>
		/// <returns>A new <see cref="AudioConverter" /> instance if successful, <see langword="null" /> otherwise.</returns>
		public static AudioConverter? Create (AudioStreamBasicDescription sourceFormat, AudioStreamBasicDescription destinationFormat, AudioClassDescription [] descriptions)
		{
			if (descriptions is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (descriptions));

			IntPtr ptr = new IntPtr ();
			unsafe {
				fixed (AudioClassDescription* descriptionsPtr = descriptions) {
					var res = AudioConverterNewSpecific (&sourceFormat, &destinationFormat, descriptions.Length, descriptionsPtr, &ptr);
					if (res != AudioConverterError.None)
						return null;
				}
			}

			return new AudioConverter (ptr, true);
		}

		/// <summary>Create a new AudioConverter with the specified options.</summary>
		/// <param name="sourceFormat">The format of the source audio to be converted.</param>
		/// <param name="destinationFormat">The format to convert the source audio to.</param>
		/// <param name="options">Any <see cref="AudioConverterOptions" /> to use.</param>
		/// <param name="error">In case of failure, will contain the error code for the failure. Otherwise the value <see cref="AudioConverterError.None" /> will be returned.</param>
		/// <returns>A new AudioConverter instance, or null in case of failure.</returns>
		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("tvos18.0")]
		public static AudioConverter? Create (AudioStreamBasicDescription sourceFormat, AudioStreamBasicDescription destinationFormat, AudioConverterOptions options, out AudioConverterError error)
		{
			IntPtr ptr = default (IntPtr);
			unsafe {
				error = AudioConverterNewWithOptions (&sourceFormat, &destinationFormat, options, &ptr);
			}
			if (error != AudioConverterError.None)
				return null;
			return new AudioConverter (ptr, true);
		}

		/// <summary>Create a new AudioConverter with the specified options.</summary>
		/// <param name="sourceFormat">The format of the source audio to be converted.</param>
		/// <param name="destinationFormat">The format to convert the source audio to.</param>
		/// <param name="options">Any <see cref="AudioConverterOptions" /> to use.</param>
		/// <returns>A new AudioConverter instance, or null in case of failure.</returns>
		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("tvos18.0")]
		public static AudioConverter? Create (AudioStreamBasicDescription sourceFormat, AudioStreamBasicDescription destinationFormat, AudioConverterOptions options)
		{
			return Create (sourceFormat, destinationFormat, options, out var _);
		}

		/// <summary>All valid converter input formats.</summary>
		public static AudioFormatType []? DecodeFormats {
			get {
				return GetFormats (AudioFormatProperty.DecodeFormatIDs);
			}
		}

		/// <summary>All valid converter output formats.</summary>
		public static AudioFormatType []? EncodeFormats {
			get {
				return GetFormats (AudioFormatProperty.EncodeFormatIDs);
			}
		}

		/// <include file="../../docs/api/AudioToolbox/AudioConverter.xml" path="/Documentation/Docs[@DocId='M:AudioToolbox.AudioConverter.Dispose(System.Boolean)']/*" />
		protected override void Dispose (bool disposing)
		{
			if (Handle != IntPtr.Zero && Owns)
				AudioConverterDispose (Handle);

			if (packetDescriptions != IntPtr.Zero) {
				Marshal.FreeHGlobal (packetDescriptions);
				packetDescriptions = IntPtr.Zero;
			}

			base.Dispose (disposing);
		}

		/// <summary>Converts audio data from one linear PCM format to another.</summary>
		/// <param name="input">The input audio data.</param>
		/// <param name="output">The output audio data.</param>
		/// <returns>In case of failure, will the error code for the failure. Otherwise the value <see cref="AudioConverterError.None" /> will be returned.</returns>
		public AudioConverterError ConvertBuffer (byte [] input, byte [] output)
		{
			if (input is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (input));
			if (output is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (output));

			int outSize = output.Length;
			unsafe {
				fixed (byte* inputPtr = input) {
					fixed (byte* outputPtr = output) {
						return AudioConverterConvertBuffer (Handle, input.Length, inputPtr, &outSize, outputPtr);
					}
				}
			}
		}

		/// <summary>Converts audio data from one linear PCM format to another where both use the same sample rate.</summary>
		/// <param name="numberPCMFrames">The number of linear PCM frames to convert.</param>
		/// <param name="inputData">The input audio data.</param>
		/// <param name="outputData">The output audio data.</param>
		/// <returns>In case of failure, will the error code for the failure. Otherwise the value <see cref="AudioConverterError.None" /> will be returned.</returns>
		public AudioConverterError ConvertComplexBuffer (int numberPCMFrames, AudioBuffers inputData, AudioBuffers outputData)
		{
			if (inputData is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (inputData));
			if (outputData is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (outputData));

			return AudioConverterConvertComplexBuffer (Handle, numberPCMFrames, (IntPtr) inputData, (IntPtr) outputData);
		}

		/// <summary>Converts audio data supporting non-interleaved and packetized formats.</summary>
		/// <param name="outputDataPacketSize">The capacity of converted output data expressed in packets</param>
		/// <param name="outputData">The converted output data.</param>
		/// <param name="packetDescription">An array of packet descriptions.</param>
		/// <param name="newInputDataHandler">A callback that will be called to supply audio data for the conversion.</param>
		/// <returns>In case of failure, will the error code for the failure. Otherwise the value <see cref="AudioConverterError.None" /> will be returned.</returns>
		public AudioConverterError FillComplexBuffer (ref int outputDataPacketSize,
			AudioBuffers outputData, AudioStreamPacketDescription [] packetDescription, AudioConverterComplexInputData newInputDataHandler)
		{
			if (outputData is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (outputData));

			if (newInputDataHandler is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (newInputDataHandler));

			return FillComplexBuffer (ref outputDataPacketSize, outputData, packetDescription, new Tuple<AudioConverter, AudioConverterComplexInputData?> (this, newInputDataHandler));
		}

		/// <summary>Converts audio data supporting non-interleaved and packetized formats.</summary>
		/// <param name="outputDataPacketSize">The capacity of converted output data expressed in packets</param>
		/// <param name="outputData">The converted output data.</param>
		/// <param name="packetDescription">An array of packet descriptions.</param>
		/// <returns>In case of failure, will the error code for the failure. Otherwise the value <see cref="AudioConverterError.None" /> will be returned.</returns>
		/// <remarks>
		///   <para>The <see cref="AudioToolbox.AudioConverter.InputData" /> event is invoked to supply the input data for the conversion.</para>
		/// </remarks>
		public AudioConverterError FillComplexBuffer (ref int outputDataPacketSize,
			AudioBuffers outputData, AudioStreamPacketDescription [] packetDescription)
		{
			if (outputData is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (outputData));

			return FillComplexBuffer (ref outputDataPacketSize, outputData, packetDescription, new Tuple<AudioConverter, AudioConverterComplexInputData?> (this, null));
		}

		AudioConverterError FillComplexBuffer (ref int outputDataPacketSize,
			AudioBuffers outputData, AudioStreamPacketDescription [] packetDescription, Tuple<AudioConverter, AudioConverterComplexInputData?> instanceData)
		{
			var this_handle = GCHandle.Alloc (instanceData);

			try {
				var this_ptr = GCHandle.ToIntPtr (this_handle);
				unsafe {
					var packetSize = outputDataPacketSize;
					int* packetSizePtr = &packetSize;
					if (packetDescription is null) {
						var returnOne = AudioConverterFillComplexBuffer (Handle, &FillComplexBufferShared, this_ptr, (IntPtr) packetSizePtr, (IntPtr) outputData, IntPtr.Zero);
						outputDataPacketSize = packetSize;
						return returnOne;
					}

					fixed (AudioStreamPacketDescription* pdesc = packetDescription) {
						var returnTwo = AudioConverterFillComplexBuffer (Handle, &FillComplexBufferShared, this_ptr, (IntPtr) packetSizePtr, (IntPtr) outputData, (IntPtr) pdesc);
						outputDataPacketSize = packetSize;
						return returnTwo;
					}
				}
			} finally {
				this_handle.Free ();
			}
		}

		//
		// outDataPacketDescription should be `ref IntPtr' but using IntPtr we get easier access to pointer address
		//
		[UnmanagedCallersOnly]
		unsafe static AudioConverterError FillComplexBufferShared (IntPtr inAudioConverter, uint* ioNumberDataPackets, IntPtr ioData,
															IntPtr outDataPacketDescription, IntPtr inUserData)
		{
			var handler = GCHandle.FromIntPtr (inUserData);
			var instanceData = handler.Target as Tuple<AudioConverter, AudioConverterComplexInputData?>;

			if (instanceData is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (instanceData));

			var inst = instanceData.Item1;
			var callback = instanceData.Item2;

			// Invoke event handler with an argument
			// since callback is not provided, must come from the old FillComplexBuffer call
			if (callback is null && inst.InputData is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException ("InputData");
			// Check if subscribed to event and provided a callback, error out if true
			else if (callback is not null && inst.InputData is not null)
				throw new InvalidOperationException ("Please either only subscribe to InputData event or provide newInputDataHandler in FillComplexBuffer, using both is unsuported.");

			using (var buffers = new AudioBuffers (ioData)) {
				//
				// Callback is supposed to fill outDataPacketDescription when outDataPacketDescription is not NULL
				// Using 0-size array as marker because the size of pre-allocated memory is not known
				//
				var data = outDataPacketDescription == IntPtr.Zero ? null : new AudioStreamPacketDescription [0];
				var res = inst.InputData is not null ?
					inst.InputData (ref Unsafe.AsRef<int> (ioNumberDataPackets), buffers, ref data) :
					callback! (ref Unsafe.AsRef<int> (ioNumberDataPackets), buffers, ref data);

				if (outDataPacketDescription != IntPtr.Zero) {
					if (*ioNumberDataPackets > 0) {
						if (data is null || data.Length == 0)
							throw new ArgumentException ("ref argument outDataPacketDescription has to be set");

						//
						// Apple doc says the output buffer has to be pre-allocated using last argument to 
						// AudioConverterFillComplexBuffer but even if NULL is passed and convertor requires
						// packet description outDataPacketDescription is not NULL and it such case we will
						// write at some unknown pointer location, similar situation happens when initialization
						// size does not match data size
						//
						int size = Marshal.SizeOf<AudioStreamPacketDescription> ();
						// Clear our buffer if it's not big enough
						if (inst.packetDescriptionSize < data.Length && inst.packetDescriptions != IntPtr.Zero) {
							Marshal.FreeHGlobal (inst.packetDescriptions);
							inst.packetDescriptions = IntPtr.Zero;
						}
						// Create a new buffer if we don't already have one
						if (inst.packetDescriptions == IntPtr.Zero) {
							inst.packetDescriptionSize = data.Length;
							inst.packetDescriptions = Marshal.AllocHGlobal (data.Length * size);
						}
						unsafe {
							fixed (void* source = data) {
								Buffer.MemoryCopy (source, (void*) inst.packetDescriptions, inst.packetDescriptionSize * size, data.Length * size);
							}
						}
						Marshal.WriteIntPtr (outDataPacketDescription, inst.packetDescriptions);
					} else {
						Marshal.WriteIntPtr (outDataPacketDescription, IntPtr.Zero);
					}
				}

				return res;
			}
		}

		public delegate void PrepareCompletionCallback (AudioConverterError status);

		[UnmanagedCallersOnly]
		static void PrepareCompletionHandler (IntPtr block, int status)
		{
			var del = BlockLiteral.GetTarget<PrepareCompletionCallback> (block);
			if (del is not null)
				del ((AudioConverterError) status);
		}

		/// <summary>Optimizes any subsequent creation of audio converters in this process.</summary>
		/// <param name="flags">Reserved; always pass 0.</param>
		/// <param name="ioReserved">Reserved; always pass IntPtr.Zero.</param>
		/// <param name="completionCallback">Optional callback to invoke when the preparation is complete.</param>
		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("tvos18.0")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public unsafe static void Prepare (uint flags = 0, IntPtr ioReserved = default (IntPtr), PrepareCompletionCallback? completionCallback = null)
		{
			if (completionCallback is null) {
				AudioConverterPrepare (flags, ioReserved, null);
			} else {
				delegate* unmanaged<IntPtr, int, void> trampoline = &PrepareCompletionHandler;
				using var block = new BlockLiteral (trampoline, completionCallback, typeof (AudioConverter), nameof (PrepareCompletionHandler));
				AudioConverterPrepare (flags, ioReserved, &block);
			}
		}

		/// <summary>Optimizes any subsequent creation of audio converters in this process.</summary>
		/// <param name="completionCallback">Callback to invoke when the preparation is complete.</param>
		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("tvos18.0")]
		public unsafe static void Prepare (PrepareCompletionCallback completionCallback)
		{
			Prepare (0, IntPtr.Zero, completionCallback);
		}

		/// <summary>Resets an audio converter.</summary>
		public AudioConverterError Reset ()
		{
			return AudioConverterReset (Handle);
		}

		unsafe static AudioFormatType []? GetFormats (AudioFormatProperty prop)
		{
			int size;
			if (AudioFormatPropertyNative.AudioFormatGetPropertyInfo (prop, 0, null, &size) != 0)
				return null;

			var elementSize = sizeof (AudioFormatType);
			var elementCount = size / elementSize;
			var data = new AudioFormatType [elementCount];
			fixed (AudioFormatType* ptr = data) {
				var res = AudioFormatPropertyNative.AudioFormatGetProperty (prop, 0, null, &size, (IntPtr) ptr);
				if (res != 0)
					return null;

				elementCount = size / elementSize;
				Array.Resize (ref data, elementCount);
				return data;
			}
		}

		unsafe T []? GetArray<T> (AudioConverterPropertyID prop) where T : unmanaged
		{
			int size;
			if (AudioConverterGetPropertyInfo (Handle, prop, &size, null) != AudioConverterError.None)
				return null;

			if (size == 0)
				return Array.Empty<T> ();

			var elementSize = sizeof (T);
			var data = new T [size / elementSize];

			fixed (T* ptr = data) {
				var res = AudioConverterGetProperty (Handle, prop, &size, ptr);
				if (res != 0)
					return null;

				Array.Resize (ref data, size / elementSize);
				return data;
			}
		}

		unsafe void SetArray<T> (AudioConverterPropertyID propertyId, T []? value) where T : unmanaged
		{
			// 'inPropertyData' is nullable because the properties are declared as nullable, which is because the getters can return null.
			if (value is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));

			AudioConverterError res;
			fixed (T* valuePtr = value)
				res = AudioConverterSetProperty (GetCheckedHandle (), propertyId, sizeof (T) * (value?.Length ?? 0), valuePtr);
			if (res != AudioConverterError.None)
				throw new ArgumentException (res.ToString ());
		}

		unsafe T GetProperty<T> (AudioConverterPropertyID propertyID) where T : unmanaged
		{
			T value;
			var size = sizeof (T);
			var res = AudioConverterGetProperty (Handle, propertyID, &size, &value);
			if (res != AudioConverterError.None)
				throw new ArgumentException (res.ToString ());

			return value;
		}

		unsafe void SetProperty<T> (AudioConverterPropertyID propertyID, T value) where T : unmanaged
		{
			var res = AudioConverterSetProperty (Handle, propertyID, sizeof (T), &value);
			if (res != AudioConverterError.None)
				throw new ArgumentException (res.ToString ());
		}

		/// <summary>Get a converter property and copy it into a newly allocated block of native memory.</summary>
		/// <param name="propertyId">The property to fetch.</param>
		/// <param name="memory">The native memory with the property value. Must be freed with Marshal.FreeHGlobal.</param>
		unsafe AudioConverterError GetPropertyIntoNativeMemory (AudioConverterPropertyID propertyId, out IntPtr memory)
		{
			int size;

			memory = IntPtr.Zero;

			var res = AudioConverterGetPropertyInfo (Handle, AudioConverterPropertyID.InputChannelLayout, &size, null);
			if (res != AudioConverterError.None)
				return res;

			memory = Marshal.AllocHGlobal (size);
			return AudioConverterGetProperty (Handle, AudioConverterPropertyID.InputChannelLayout, &size, (void*) memory);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe static extern AudioConverterError AudioConverterNew (AudioStreamBasicDescription* inSourceFormat, AudioStreamBasicDescription* inDestinationFormat, IntPtr* outAudioConverter);

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe static extern AudioConverterError AudioConverterNewSpecific (AudioStreamBasicDescription* inSourceFormat, AudioStreamBasicDescription* inDestinationFormat,
			int inNumberClassDescriptions, AudioClassDescription* inClassDescriptions, IntPtr* outAudioConverter);

		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("tvos18.0")]
		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe static extern void AudioConverterPrepare (uint inFlags, IntPtr ioReserved, BlockLiteral* block);

		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("tvos18.0")]
		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe static extern /* OSStatus */ AudioConverterError AudioConverterNewWithOptions (
				/* const AudioStreamBasicDescription * */ AudioStreamBasicDescription* inSourceFormat,
				/* const AudioStreamBasicDescription * */ AudioStreamBasicDescription* inDestinationFormat,
				/* AudioConverterOptions */ AudioConverterOptions inOptions,
				/* AudioConverterRef __nullable * __nonnull */ IntPtr* outAudioConverter);

		[DllImport (Constants.AudioToolboxLibrary)]
		static extern AudioConverterError AudioConverterDispose (IntPtr inAudioConverter);

		[DllImport (Constants.AudioToolboxLibrary)]
		static extern AudioConverterError AudioConverterReset (IntPtr inAudioConverter);

		[DllImport (Constants.AudioToolboxLibrary)]
		static extern AudioConverterError AudioConverterConvertComplexBuffer (IntPtr inAudioConverter, int inNumberPCMFrames,
			IntPtr inInputData, IntPtr outOutputData);

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe static extern AudioConverterError AudioConverterGetProperty (IntPtr inAudioConverter, AudioConverterPropertyID inPropertyID, int* ioPropertyDataSize, void* outPropertyData);

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe static extern AudioConverterError AudioConverterGetPropertyInfo (IntPtr inAudioConverter, AudioConverterPropertyID inPropertyID, int* outSize, byte* outWritable);

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe static extern AudioConverterError AudioConverterSetProperty (IntPtr inAudioConverter, AudioConverterPropertyID inPropertyID, int inPropertyDataSize, void* inPropertyData);

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe static extern AudioConverterError AudioConverterConvertBuffer (IntPtr inAudioConverter, int inInputDataSize, byte* inInputData,
			int* ioOutputDataSize, byte* outOutputData);

		[DllImport (Constants.AudioToolboxLibrary)]
		static unsafe extern AudioConverterError AudioConverterFillComplexBuffer (IntPtr inAudioConverter,
			delegate* unmanaged<IntPtr, uint*, IntPtr, IntPtr, IntPtr, AudioConverterError> inInputDataProc, IntPtr inInputDataProcUserData,
			IntPtr ioOutputDataPacketSize, IntPtr outOutputData,
			IntPtr outPacketDescription);

		/// <summary>Converts audio data, supplied by an event handler, supporting packet dependencies.</summary>
		/// <param name="outputDataPacketSize">On input, the size if the output buffer (<paramref name="outputData" />). On output, the number of converted audio packets.</param>
		/// <param name="outputData">The buffer where the converted audio data is to be written.</param>
		/// <param name="packetDescription">An array of packet descriptions that will contain the decoded packet descriptions upon return.</param>
		/// <param name="packetDependencies">An array of packet dependencies that will contain the decoded packet dependencies (if any) upon return.</param>
		/// <returns>An error code in case of failure, <see cref="AudioConverterError.None" /> otherwise.</returns>
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		public AudioConverterError FillComplexBuffer (
			ref int outputDataPacketSize,
			AudioBuffers outputData,
			AudioStreamPacketDescription [] packetDescription,
			AudioStreamPacketDependencyDescription [] packetDependencies)
		{
			if (InputData is null)
				throw new InvalidOperationException ($"No event handler has been added to the '{nameof (InputData)}' event.");

			return FillComplexBufferWithPacketDependencies (ref outputDataPacketSize, outputData, packetDescription, null, packetDependencies);
		}

		/// <summary>Converts audio data, supplied by a callback function, supporting packet dependencies.</summary>
		/// <param name="outputDataPacketSize">On input, the size if the output buffer (<paramref name="outputData" />). On output, the number of converted audio packets.</param>
		/// <param name="outputData">The buffer where the converted audio data is to be written.</param>
		/// <param name="packetDescription">An array of packet descriptions that will contain the decoded packet descriptions upon return.</param>
		/// <param name="dataHandler">The callback that will be called to supply audio data for the conversion.</param>
		/// <param name="packetDependencies">An array of packet dependencies that will contain the decoded packet dependencies (if any) upon return.</param>
		/// <returns>An error code in case of failure, <see cref="AudioConverterError.None" /> otherwise.</returns>
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		public AudioConverterError FillComplexBuffer (
			ref int outputDataPacketSize,
			AudioBuffers outputData,
			AudioStreamPacketDescription []? packetDescription,
			AudioConverterComplexInputData dataHandler,
			AudioStreamPacketDependencyDescription [] packetDependencies)
		{
			if (dataHandler is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (dataHandler));

			return FillComplexBufferWithPacketDependencies (ref outputDataPacketSize, outputData, packetDescription, dataHandler, packetDependencies);
		}

		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		AudioConverterError FillComplexBufferWithPacketDependencies (
			ref int outputDataPacketSize,
			AudioBuffers outputData,
			AudioStreamPacketDescription []? packetDescriptions,
			AudioConverterComplexInputData? dataHandler,
			AudioStreamPacketDependencyDescription [] packetDependencies
			)
		{
			if (outputData is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (outputData));

			if (packetDependencies is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (packetDependencies));
			if (packetDependencies.Length < outputDataPacketSize)
				ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (packetDependencies), $"Length must be equal to or greater than '{nameof (outputDataPacketSize)}'.");

			// A null value for 'packetDescriptions' is allowed, but not a too small array.
			if (packetDescriptions is not null && packetDescriptions.Length < outputDataPacketSize)
				ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (packetDescriptions), $"Length must be equal to or greater than '{nameof (outputDataPacketSize)}'.");

			var instanceData = new Tuple<AudioConverter, AudioConverterComplexInputData?> (this, dataHandler);
			var this_handle = GCHandle.Alloc (instanceData);
			try {
				unsafe {
					fixed (AudioStreamPacketDependencyDescription* packetDependenciesPtr = packetDependencies) {
						fixed (AudioStreamPacketDescription* packetDescriptionPtr = packetDescriptions) {
							return AudioConverterFillComplexBufferWithPacketDependencies (
								GetCheckedHandle (),
								&FillComplexBufferShared,
								(IntPtr) this_handle,
								(uint*) Unsafe.AsPointer<int> (ref outputDataPacketSize),
								(IntPtr) outputData,
								packetDescriptionPtr,
								packetDependenciesPtr);
						}
					}
				}
			} finally {
				this_handle.Free ();
				GC.KeepAlive (outputData);
			}
		}

		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[DllImport (Constants.AudioToolboxLibrary)]
		static unsafe extern AudioConverterError /* OSStatus */ AudioConverterFillComplexBufferWithPacketDependencies (
			IntPtr /* AudioConverterRef */ inAudioConverter,
			delegate* unmanaged<IntPtr /* AudioConverterRef */, uint* /* UInt32* */, IntPtr /* AudioBufferList* */, IntPtr /* AudioStreamPacketDescription * __nullable * __nullable */, IntPtr /* void * __nullable */, AudioConverterError /* OSStatus */> inInputDataProc,
			IntPtr /* void * __nullable */ inInputDataProcUserData,
			uint* /* UInt32 * */ ioOutputDataPacketSize,
			IntPtr /* AudioBufferList * */ outOutputData,
			AudioStreamPacketDescription* /* AudioStreamPacketDescription * __nullable */ outPacketDescription,
			AudioStreamPacketDependencyDescription* /* AudioStreamPacketDependencyDescription * */ outPacketDependencies);

	}

	enum AudioConverterPropertyID // typedef UInt32 AudioConverterPropertyID
	{
		MinimumInputBufferSize = 0x6d696273, // 'mibs'
		MinimumOutputBufferSize = 0x6d6f6273, // 'mobs'
											  // Deprecated
											  // MaximumInputBufferSize		= 0x78696273, // 'xibs'
		MaximumInputPacketSize = 0x78697073, // 'xips'
		MaximumOutputPacketSize = 0x786f7073, // 'xops'
		CalculateInputBufferSize = 0x63696273, // 'cibs'
		CalculateOutputBufferSize = 0x636f6273, // 'cobs'

		// TODO: Format specific
		// InputCodecParameters         = 'icdp'
		// OutputCodecParameters        = 'ocdp'

		// Deprecated
		// SampleRateConverterAlgorithm = 'srci'
		SampleRateConverterComplexity = 0x73726361, // 'srca'
		SampleRateConverterQuality = 0x73726371, // 'srcq'
		SampleRateConverterInitialPhase = 0x73726370, // 'srcp'
		CodecQuality = 0x63647175, // 'cdqu'
		PrimeMethod = 0x70726d6d, // 'prmm'
		PrimeInfo = 0x7072696d, // 'prim'
		ChannelMap = 0x63686d70, // 'chmp'
		DecompressionMagicCookie = 0x646d6763, // 'dmgc'
		CompressionMagicCookie = 0x636d6763, // 'cmgc'
		EncodeBitRate = 0x62726174, // 'brat'
		EncodeAdjustableSampleRate = 0x616a7372, // 'ajsr'
		InputChannelLayout = 0x69636c20, // 'icl '
		OutputChannelLayout = 0x6f636c20, // 'ocl '
		ApplicableEncodeBitRates = 0x61656272, // 'aebr'
		AvailableEncodeBitRates = 0x76656272, // 'vebr'
		ApplicableEncodeSampleRates = 0x61657372, // 'aesr'
		AvailableEncodeSampleRates = 0x76657372, // 'vesr'
		AvailableEncodeChannelLayoutTags = 0x6165636c, // 'aecl'
		CurrentOutputStreamDescription = 0x61636f64, // 'acod'
		CurrentInputStreamDescription = 0x61636964, // 'acid'
		PropertySettings = 0x61637073, // 'acps'	// TODO
		PropertyBitDepthHint = 0x61636264, // 'acbd'
		PropertyFormatList = 0x666c7374, // 'flst'
		CanResumeFromInterruption = 0x63726669, // 'crfi'

		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		PerformDownmix = 0x646d6978, // 'dmix'

		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		ChannelMixMap = 0x6d6d6170, // 'mmap'
	}
}
