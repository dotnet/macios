// Copyright 2014 Xamarin Inc
//
// Authors:
//   Miguel de Icaza (miguel@xamarin.com)
//
using Foundation;
using ObjCRuntime;
using System;

#nullable enable

#if !MONOMAC

namespace AVFoundation {
	public enum AVAudioDataSourceLocation {
		/// <summary>To be added.</summary>
		Unknown,
		/// <summary>To be added.</summary>
		Upper,
		/// <summary>To be added.</summary>
		Lower,
	}

	public enum AVAudioDataSourceOrientation {
		/// <summary>To be added.</summary>
		Unknown,
		/// <summary>To be added.</summary>
		Top,
		/// <summary>To be added.</summary>
		Bottom,
		/// <summary>To be added.</summary>
		Front,
		/// <summary>To be added.</summary>
		Back,
		/// <summary>To be added.</summary>
		Left,
		/// <summary>To be added.</summary>
		Right,
	}

	public enum AVAudioDataSourcePolarPattern {
		/// <summary>To be added.</summary>
		Unknown,
		/// <summary>To be added.</summary>
		Omnidirectional,
		/// <summary>To be added.</summary>
		Cardioid,
		/// <summary>To be added.</summary>
		Subcardioid,
	}

	public partial class AVAudioSessionDataSourceDescription {
		static internal AVAudioDataSourceLocation ToLocation (NSString? l)
		{
			if (l == AVAudioSession.LocationLower_)
				return AVAudioDataSourceLocation.Lower;
			else if (l == AVAudioSession.LocationUpper_)
				return AVAudioDataSourceLocation.Upper;
			else
				return AVAudioDataSourceLocation.Unknown;
		}

		static internal AVAudioDataSourceOrientation ToOrientation (NSString? o)
		{
			if (o == AVAudioSession.OrientationTop_)
				return AVAudioDataSourceOrientation.Top;
			if (o == AVAudioSession.OrientationBottom_)
				return AVAudioDataSourceOrientation.Bottom;
			if (o == AVAudioSession.OrientationFront_)
				return AVAudioDataSourceOrientation.Front;
			if (o == AVAudioSession.OrientationBack_)
				return AVAudioDataSourceOrientation.Back;
			return AVAudioDataSourceOrientation.Unknown;
		}

		static internal AVAudioDataSourcePolarPattern ToPolarPattern (NSString? p)
		{
			if (p == AVAudioSession.PolarPatternOmnidirectional_)
				return AVAudioDataSourcePolarPattern.Omnidirectional;
			if (p == AVAudioSession.PolarPatternCardioid_)
				return AVAudioDataSourcePolarPattern.Cardioid;
			if (p == AVAudioSession.PolarPatternSubcardioid_)
				return AVAudioDataSourcePolarPattern.Subcardioid;
			return AVAudioDataSourcePolarPattern.Unknown;
		}

		static internal NSString? ToToken (AVAudioDataSourcePolarPattern p)
		{
			switch (p) {
			case AVAudioDataSourcePolarPattern.Omnidirectional:
				return AVAudioSession.PolarPatternOmnidirectional_;
			case AVAudioDataSourcePolarPattern.Cardioid:
				return AVAudioSession.PolarPatternCardioid_;
			case AVAudioDataSourcePolarPattern.Subcardioid:
				return AVAudioSession.PolarPatternSubcardioid_;
			default:
				return null;
			}
		}

		/// <summary>Indicates the location of the data source on the device.</summary>
		///         <value>The value can be either LocationUpper or LocationLower from <see cref="T:AVFoundation.AVAudioSession" />.<para tool="nullallowed">This value can be <see langword="null" />.</para></value>
		///         <remarks>To be added.</remarks>
		public AVAudioDataSourceLocation Location {
			get {
				return ToLocation (Location_);
			}
		}

		/// <summary>Indicates the orientation of the data source on the device.</summary>
		///         <value>One of the constants in <see cref="T:AVFoundation.AVAudioSession" />’s Orientation properties (OrientationTop, OrientationBotton, OrientationFront, OrientationBack, OrientationLeft, OrientationRight).<para tool="nullallowed">This value can be <see langword="null" />.</para></value>
		///         <remarks>The data source can be pointing upwards, downwards, towards the user, away from the user, left or right.</remarks>
		public AVAudioDataSourceOrientation Orientation {
			get {
				return ToOrientation (Orientation_);
			}
		}

		/// <summary>The supported polar configurations by this audio data source.</summary>
		///         <value>Array of strings containing the names of the supported microphone directions for the source, one of PolarPatternOmnidirectional, PolarPatternCardioid, PolarPatternSubcardioid from <see cref="T:AVFoundation.AVAudioSession" />.</value>
		///         <remarks>To be added.</remarks>
		public AVAudioDataSourcePolarPattern []? SupportedPolarPatterns {
			get {
				var x = SupportedPolarPatterns_;
				if (x is null)
					return null;

				int n = x.Length;
				var r = new AVAudioDataSourcePolarPattern [n];
				for (int i = 0; i < n; i++)
					r [i] = ToPolarPattern (x [i]);
				return r;
			}
		}

		/// <summary>Indicates the currenly selected microphone direction.</summary>
		///         <value>The current setting for the microphone direction, one of PolarPatternOmnidirectional, PolarPatternCardioid, PolarPatternSubcardioid from <see cref="T:AVFoundation.AVAudioSession" /></value>
		///         <remarks>To be added.</remarks>
		public AVAudioDataSourcePolarPattern SelectedPolarPattern {
			get {
				return ToPolarPattern (SelectedPolarPattern_);
			}
		}

		/// <summary>The preferred microphone direction.</summary>
		///         <value>The current setting for the microphone direction, one of PolarPatternOmnidirectional, PolarPatternCardioid, PolarPatternSubcardioid from <see cref="T:AVFoundation.AVAudioSession" /></value>
		///         <remarks>To change this property, call the <see cref="M:AVFoundation.AVAudioSessionDataSourceDescription.SetPreferredPolarPattern(AVFoundation.AVAudioDataSourcePolarPattern,Foundation.NSError@)" />.</remarks>
		public AVAudioDataSourcePolarPattern PreferredPolarPattern {
			get {
				return ToPolarPattern (PreferredPolarPattern_);
			}
		}

		/// <param name="pattern">To be added.</param>
		///         <param name="outError">To be added.</param>
		///         <summary>Sets the preferred directivity for the data source.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool SetPreferredPolarPattern (AVAudioDataSourcePolarPattern pattern, out NSError outError)
		{
			return SetPreferredPolarPattern_ (ToToken (pattern), out outError);
		}
	}
}
#endif
