// 
// AudioSettings.cs: Implements strongly typed access for AV audio settings
//
// Authors: Marek Safar (marek.safar@gmail.com)
//     
// Copyright 2012, 2014 Xamarin Inc.
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

using System;

using Foundation;
using CoreFoundation;
using ObjCRuntime;
using AudioToolbox;

#nullable enable

namespace AVFoundation {


#if NET
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
#endif
	// Should be called AVAudioSetting but AVAudioSetting has been already used by keys class
	public class AudioSettings : DictionaryContainer {
#if !COREBUILD
		public AudioSettings ()
			: base (new NSMutableDictionary ())
		{
		}

		public AudioSettings (NSDictionary dictionary)
			: base (dictionary)
		{
		}

		/// <summary>Represents the audio format.</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant AVFormatIDKey value to access the underlying dictionary.</remarks>
		public AudioFormatType? Format {
			set {
				SetNumberValue (AVAudioSettings.AVFormatIDKey, (int?) value);
			}
			get {
				return (AudioFormatType?) GetInt32Value (AVAudioSettings.AVFormatIDKey);
			}
		}

		/// <summary>Represents the sample rate in hertz.</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant AVSampleRateKey value to access the underlying dictionary.</remarks>
		public double? SampleRate {
			set {
				SetNumberValue (AVAudioSettings.AVSampleRateKey, value);
			}
			get {
				return GetDoubleValue (AVAudioSettings.AVSampleRateKey);
			}
		}

		/// <summary>Represents the number of channels.</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant AVNumberOfChannelsKey value to access the underlying dictionary.</remarks>
		public int? NumberChannels {
			set {
				SetNumberValue (AVAudioSettings.AVNumberOfChannelsKey, value);
			}
			get {
				return GetInt32Value (AVAudioSettings.AVNumberOfChannelsKey);
			}
		}

		/// <summary>Represents the bit depth for a linear PCM audio format.</summary>
		///         <value>Only values 8, 16, 24, and 32 are allowed</value>
		///         <remarks>The property uses constant AVLinearPCMBitDepthKey value to access the underlying dictionary.</remarks>
		public int? LinearPcmBitDepth {
			set {
				if (!(value == 8 || value == 16 || value == 24 || value == 32))
					throw new ArgumentOutOfRangeException ("value must be of 8, 16, 24 or 32");

				SetNumberValue (AVAudioSettings.AVLinearPCMBitDepthKey, value);
			}
			get {
				return GetInt32Value (AVAudioSettings.AVLinearPCMBitDepthKey);
			}
		}

		/// <summary>Indicates whether the audio format is big endian.</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant AVLinearPCMIsBigEndianKey value to access the underlying dictionary.</remarks>
		public bool? LinearPcmBigEndian {
			set {
				SetBooleanValue (AVAudioSettings.AVLinearPCMIsBigEndianKey, value);
			}
			get {
				return GetBoolValue (AVAudioSettings.AVLinearPCMIsBigEndianKey);
			}
		}

		/// <summary>Indicates whether the audio format is floating point.</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant AVLinearPCMIsFloatKey value to access the underlying dictionary.</remarks>
		public bool? LinearPcmFloat {
			set {
				SetBooleanValue (AVAudioSettings.AVLinearPCMIsFloatKey, value);
			}
			get {
				return GetBoolValue (AVAudioSettings.AVLinearPCMIsFloatKey);
			}
		}

		/// <summary>Indicates that the audio format is non-interleaved.</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant AVLinearPCMIsNonInterleaved value to access the underlying dictionary.</remarks>
		public bool? LinearPcmNonInterleaved {
			set {
				SetBooleanValue (AVAudioSettings.AVLinearPCMIsNonInterleaved, value);
			}
			get {
				return GetBoolValue (AVAudioSettings.AVLinearPCMIsNonInterleaved);
			}
		}

		/// <summary>Represents sample rate conversion quality settings.</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant AVEncoderAudioQualityKey value to access the underlying dictionary.</remarks>
		public AVAudioQuality? AudioQuality {
			set {
				SetNumberValue (AVAudioSettings.AVEncoderAudioQualityKey, (nint?) (long?) value);
			}
			get {
				return (AVAudioQuality?) (long?) GetNIntValue (AVAudioSettings.AVEncoderAudioQualityKey);
			}
		}

		/// <summary>Represents sample rate converter audio quality settings.</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant AVSampleRateConverterAudioQualityKey value to access the underlying dictionary.</remarks>
		public AVAudioQuality? SampleRateConverterAudioQuality {
			set {
				SetNumberValue (AVAudioSettings.AVSampleRateConverterAudioQualityKey, (nint?) (long?) value);
			}
			get {
				return (AVAudioQuality?) (long?) GetNIntValue (AVAudioSettings.AVSampleRateConverterAudioQualityKey);
			}
		}

		/// <summary>Represents the audio bit rate.</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant AVEncoderBitRateKey value to access the underlying dictionary.</remarks>
		public int? EncoderBitRate {
			set {
				SetNumberValue (AVAudioSettings.AVEncoderBitRateKey, value);
			}
			get {
				return GetInt32Value (AVAudioSettings.AVEncoderBitRateKey);
			}
		}

		/// <summary>Represents the audio bit rate per channel.</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant AVEncoderBitRatePerChannelKey value to access the underlying dictionary.</remarks>
		public int? EncoderBitRatePerChannel {
			set {
				SetNumberValue (AVAudioSettings.AVEncoderBitRatePerChannelKey, value);
			}
			get {
				return GetInt32Value (AVAudioSettings.AVEncoderBitRatePerChannelKey);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>Value in range between 8 and 32.</value>
		///         <remarks>The property uses constant AVEncoderBitDepthHintKey value to access the underlying dictionary.</remarks>
		public int? EncoderBitDepthHint {
			set {
				if (value < 8 || value > 32)
					throw new ArgumentOutOfRangeException ("value is required to be between 8 and 32");

				SetNumberValue (AVAudioSettings.AVEncoderBitDepthHintKey, value);
			}
			get {
				return GetInt32Value (AVAudioSettings.AVEncoderBitDepthHintKey);
			}
		}

#if NET
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
#endif
		public AVAudioBitRateStrategy? BitRateStrategy {
			set {
				NSString? v = null;
				switch (value) {
				case AVAudioBitRateStrategy.Constant:
					v = AVAudioSettings._Constant;
					break;
				case AVAudioBitRateStrategy.LongTermAverage:
					v = AVAudioSettings._LongTermAverage;
					break;
				case AVAudioBitRateStrategy.VariableConstrained:
					v = AVAudioSettings._VariableConstrained;
					break;
				case AVAudioBitRateStrategy.Variable:
					v = AVAudioSettings._Variable;
					break;
				case null:
					break;
				default:
					throw new ArgumentOutOfRangeException ("value");
				}
				SetStringValue (AVAudioSettings.AVEncoderBitRateStrategyKey, v);
			}
			get {
				var k = GetNSStringValue (AVAudioSettings.AVEncoderBitRateStrategyKey);
				if (k == AVAudioSettings._Constant)
					return AVAudioBitRateStrategy.Constant;
				if (k == AVAudioSettings._LongTermAverage)
					return AVAudioBitRateStrategy.LongTermAverage;
				if (k == AVAudioSettings._VariableConstrained)
					return AVAudioBitRateStrategy.VariableConstrained;
				if (k == AVAudioSettings._Variable)
					return AVAudioBitRateStrategy.Variable;
				return null;
			}
		}

#if NET
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
#endif
		public AVSampleRateConverterAlgorithm? SampleRateConverterAlgorithm {
			get {
				var k = GetNSStringValue (AVAudioSettings.AVSampleRateConverterAlgorithmKey);
				if (k == AVAudioSettings.AVSampleRateConverterAlgorithm_Normal)
					return AVSampleRateConverterAlgorithm.Normal;
				if (k == AVAudioSettings.AVSampleRateConverterAlgorithm_Mastering)
					return AVSampleRateConverterAlgorithm.Mastering;
				return null;
			}
			set {
				NSString? v = null;
				switch (value) {
				case AVSampleRateConverterAlgorithm.Mastering:
					v = AVAudioSettings.AVSampleRateConverterAlgorithm_Mastering;
					break;
				case AVSampleRateConverterAlgorithm.Normal:
					v = AVAudioSettings.AVSampleRateConverterAlgorithm_Normal;
					break;
				case null:
					break;
				default:
					throw new ArgumentOutOfRangeException ("value");
				}
				SetStringValue (AVAudioSettings.AVSampleRateConverterAlgorithmKey, v);
			}
		}

#if NET
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
#endif
		public AVAudioQuality? EncoderAudioQualityForVBR {
			get {
				return (AVAudioQuality?) (long?) GetNIntValue (AVAudioSettings.AVEncoderAudioQualityForVBRKey);
			}
			set {
				SetNumberValue (AVAudioSettings.AVEncoderAudioQualityForVBRKey, (nint?) (long?) value);
			}
		}

		/// <summary>Represents channel layout information for playback.</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant AVChannelLayoutKey value to access the underlying dictionary.</remarks>
		public AudioChannelLayout ChannelLayout {
			set {
				SetNativeValue (AVAudioSettings.AVChannelLayoutKey, value is null ? null : value.AsData ());
			}
		}

#endif
	}
}
