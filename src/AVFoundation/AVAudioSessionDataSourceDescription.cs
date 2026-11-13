// Copyright 2014 Xamarin Inc
//
// Authors:
//   Miguel de Icaza (miguel@xamarin.com)
//

#nullable enable

#if !MONOMAC

namespace AVFoundation {
	/// <summary>Enumerates physical locations of data sources on AV devices.</summary>
	///     <remarks>To be added.</remarks>
	public enum AVAudioDataSourceLocation {
		/// <summary>To be added.</summary>
		Unknown,
		/// <summary>To be added.</summary>
		Upper,
		/// <summary>To be added.</summary>
		Lower,
	}

	/// <summary>Enumerates physical orientations of data sources on AV devices.</summary>
	///     <remarks>To be added.</remarks>
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

	/// <summary>Describes a data source of an <see cref="AVFoundation.AVAudioSession" /> object.</summary>
	///     <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/documentation/avfaudio/avaudiosessiondatasourcedescription">Apple documentation for <c>AVAudioSessionDataSourceDescription</c></related>
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

		/// <summary>Indicates the location of the data source on the device.</summary>
		///         <value>The value can be either LocationUpper or LocationLower from <see cref="AVFoundation.AVAudioSession" />.<para tool="nullallowed">This value can be <see langword="null" />.</para></value>
		///         <remarks>To be added.</remarks>
		public AVAudioDataSourceLocation Location {
			get {
				return ToLocation (Location_);
			}
		}

		/// <summary>Indicates the orientation of the data source on the device.</summary>
		///         <value>One of the constants in <see cref="AVFoundation.AVAudioSession" />’s Orientation properties (OrientationTop, OrientationBotton, OrientationFront, OrientationBack, OrientationLeft, OrientationRight).<para tool="nullallowed">This value can be <see langword="null" />.</para></value>
		///         <remarks>The data source can be pointing upwards, downwards, towards the user, away from the user, left or right.</remarks>
		public AVAudioDataSourceOrientation Orientation {
			get {
				return ToOrientation (Orientation_);
			}
		}

		/// <summary>The supported polar configurations by this audio data source.</summary>
		/// <value>The supported microphone polar patterns, or <see langword="null" /> if the data source has no selectable patterns.</value>
		public AVAudioDataSourcePolarPattern []? SupportedPolarPatterns {
			get {
				var x = SupportedPolarPatterns_;
				if (x is null)
					return null;

				int n = x.Length;
				var r = new AVAudioDataSourcePolarPattern [n];
				for (int i = 0; i < n; i++)
					r [i] = AVAudioDataSourcePolarPatternExtensions.GetValue (x [i]);
				return r;
			}
		}

		/// <summary>Indicates the currently selected microphone direction.</summary>
		/// <value>The selected polar pattern, or <see cref="AVAudioDataSourcePolarPattern.Unknown" /> if no pattern is selected or recognized.</value>
		public AVAudioDataSourcePolarPattern SelectedPolarPattern {
			get {
				return AVAudioDataSourcePolarPatternExtensions.GetValue (SelectedPolarPattern_);
			}
		}

		/// <summary>The preferred microphone direction.</summary>
		/// <value>The preferred polar pattern, or <see cref="AVAudioDataSourcePolarPattern.Unknown" /> if no preference is set or recognized.</value>
		/// <remarks>To change this property, call <see cref="SetPreferredPolarPattern" />.</remarks>
		public AVAudioDataSourcePolarPattern PreferredPolarPattern {
			get {
				return AVAudioDataSourcePolarPatternExtensions.GetValue (PreferredPolarPattern_);
			}
		}

		/// <summary>Sets the preferred directivity for the data source.</summary>
		/// <param name="pattern">The preferred polar pattern, or <see cref="AVAudioDataSourcePolarPattern.Unknown" /> to clear the preference.</param>
		/// <param name="outError">The error if the operation fails.</param>
		/// <returns><see langword="true" /> if the preference was set; otherwise, <see langword="false" />.</returns>
		public bool SetPreferredPolarPattern (AVAudioDataSourcePolarPattern pattern, out NSError outError)
		{
			return SetPreferredPolarPattern_ (pattern.GetConstant (), out outError);
		}
	}
}
#endif
