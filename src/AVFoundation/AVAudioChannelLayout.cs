//
// Copyright 2014 Xamarin Inc
//
// Authors:
//   Miguel de Icaza (miguel@xamarin.com)
//

// 'AVAudioChannelLayout' defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable 0660
// 'AVAudioChannelLayout' defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning disable 0661
// In both of these cases, the NSObject Equals/GetHashCode implementation works fine, so we can ignore these warnings.

using AudioToolbox;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable enable

namespace AVFoundation {
	/// <summary>Corresponds to an <see cref="AudioToolbox.AudioChannelLayout" /> channel layout.</summary>
	/// <related type="externalDocumentation" href="https://developer.apple.com/documentation/avfaudio/avaudiochannellayout">Apple documentation for <c>AVAudioChannelLayout</c></related>
	public partial class AVAudioChannelLayout {
		static IntPtr CreateLayoutPtr (AudioChannelLayout layout, out IntPtr handleToLayout)
		{
			int size;
			handleToLayout = layout.ToBlock (out size);
			return handleToLayout;
		}

		/// <summary>Creates a channel layout from the specified Audio Toolbox channel layout.</summary>
		/// <param name="layout">The Audio Toolbox channel layout to use.</param>
		[DesignatedInitializer]
		public AVAudioChannelLayout (AudioChannelLayout layout)
			: this (CreateLayoutPtr (layout, out var handleToLayout))
		{
			Marshal.FreeHGlobal (handleToLayout);
		}

		/// <summary>The underlying <see cref="AudioToolbox.AudioChannelLayout" />.</summary>
		/// <value>The underlying channel layout, or <see langword="null" /> if one is unavailable.</value>
		public AudioChannelLayout? Layout {
			get {
				return AudioChannelLayout.FromHandle (_Layout);
			}
		}

		public static bool operator == (AVAudioChannelLayout a, AVAudioChannelLayout b)
		{
			return a.Equals (b);
		}

		public static bool operator != (AVAudioChannelLayout a, AVAudioChannelLayout b)
		{
			return !a.Equals (b);
		}
	}
}
