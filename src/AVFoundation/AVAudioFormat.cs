//
// Copyright 2014 Xamarin Inc
//
// Authors:
//   Miguel de Icaza (miguel@xamarin.com)
//

// 'AVAudioFormat' defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable 0660
// 'AVAudioFormat' defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning disable 0661
// In both of these cases, the NSObject Equals/GetHashCode implementation works fine, so we can ignore these warnings.

using AudioToolbox;
using Foundation;
using ObjCRuntime;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable enable

namespace AVFoundation {
	/// <summary>Corresponds to a Core Audio AudioStreamBasicDescription struct.</summary>
	///     <remarks>To be added.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/AVFoundation/Reference/AVAudioFormat_Class/index.html">Apple documentation for <c>AVAudioFormat</c></related>
	public partial class AVAudioFormat {
		public static bool operator == (AVAudioFormat a, AVAudioFormat b)
		{
			if ((object) a == (object) b)
				return true;
			if (a is null || b is null)
				return false;
			return a.Equals (b);
		}

		public static bool operator != (AVAudioFormat a, AVAudioFormat b)
		{
			return !(a == b);
		}

		/// <summary>Gets the audio stream description.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("StreamDescription")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public virtual AudioStreamBasicDescription StreamDescription {
			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("tvos")]
			[SupportedOSPlatform ("maccatalyst")]
			get {
				var ptr = _StreamDescription;
				if (ptr == IntPtr.Zero)
					return default (AudioStreamBasicDescription);
				unsafe {
					AudioStreamBasicDescription* p = (AudioStreamBasicDescription*) ptr;
					return *p;
				}
			}
		}
	}
}
