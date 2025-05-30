using System;
using System.Runtime.InteropServices;

#if !COREBUILD
using Vector3 = global::System.Numerics.Vector3;
#endif // !COREBUILD
using CoreGraphics;
using ObjCRuntime;

#nullable enable

namespace AVFoundation {
	/// <summary>Defines the listener's position in 3D space as orthogonal 'Up' and 'Forward' vectors.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AVAudio3DVectorOrientation {
#if !COREBUILD
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public Vector3 Forward;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public Vector3 Up;

		public AVAudio3DVectorOrientation (Vector3 forward, Vector3 up)
		{
			Forward = forward;
			Up = up;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			return String.Format ("({0}:{1})", Forward, Up);
		}

		public static bool operator == (AVAudio3DVectorOrientation left, AVAudio3DVectorOrientation right)
		{
			return left.Equals (right);
		}
		public static bool operator != (AVAudio3DVectorOrientation left, AVAudio3DVectorOrientation right)
		{
			return !left.Equals (right);
		}

		/// <param name="obj">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override bool Equals (object? obj)
		{
			if (!(obj is AVAudio3DVectorOrientation))
				return false;

			return this.Equals ((AVAudio3DVectorOrientation) obj);
		}

		/// <param name="other">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Equals (AVAudio3DVectorOrientation other)
		{
			return Forward == other.Forward && Up == other.Up;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override int GetHashCode ()
		{
			return HashCode.Combine (Forward, Up);
		}
#endif
	}

	/// <summary>Holds the angular orientation of the listener in 3D space.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AVAudio3DAngularOrientation {

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float Yaw;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float Pitch;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float Roll;

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			return String.Format ("(Yaw={0},Pitch={1},Roll={2})", Yaw, Pitch, Roll);
		}

		public static bool operator == (AVAudio3DAngularOrientation left, AVAudio3DAngularOrientation right)
		{
			return (left.Yaw == right.Yaw &&
				left.Pitch == right.Pitch &&
				left.Roll == right.Roll);
		}
		public static bool operator != (AVAudio3DAngularOrientation left, AVAudio3DAngularOrientation right)
		{
			return (left.Yaw != right.Yaw ||
				left.Pitch != right.Pitch ||
				left.Roll != right.Roll);

		}

		/// <param name="obj">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override bool Equals (object? obj)
		{
			if (!(obj is AVAudio3DAngularOrientation))
				return false;

			return this.Equals ((AVAudio3DAngularOrientation) obj);
		}

		/// <param name="other">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Equals (AVAudio3DAngularOrientation other)
		{
			return this == other;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override int GetHashCode ()
		{
			return HashCode.Combine (Yaw, Pitch, Roll);
		}
	}

	/// <summary>Contains RGB gain values for white balance.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AVCaptureWhiteBalanceGains {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float RedGain;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float GreenGain;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float BlueGain;

		/// <param name="redGain">To be added.</param>
		///         <param name="greenGain">To be added.</param>
		///         <param name="blueGain">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public AVCaptureWhiteBalanceGains (float redGain, float greenGain, float blueGain)
		{
			RedGain = redGain;
			GreenGain = greenGain;
			BlueGain = blueGain;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			return String.Format ("(RedGain={0},GreenGain={1},BlueGain={2})", RedGain, GreenGain, BlueGain);
		}

		public static bool operator == (AVCaptureWhiteBalanceGains left, AVCaptureWhiteBalanceGains right)
		{
			return (left.RedGain == right.RedGain &&
				left.GreenGain == right.GreenGain &&
				left.BlueGain == right.BlueGain);
		}

		public static bool operator != (AVCaptureWhiteBalanceGains left, AVCaptureWhiteBalanceGains right)
		{
			return (left.RedGain != right.RedGain ||
				left.GreenGain != right.GreenGain ||
				left.BlueGain != right.BlueGain);
		}

		/// <param name="obj">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override bool Equals (object? obj)
		{
			if (!(obj is AVCaptureWhiteBalanceGains))
				return false;

			return this.Equals ((AVCaptureWhiteBalanceGains) obj);
		}

		/// <param name="other">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Equals (AVCaptureWhiteBalanceGains other)
		{
			return this == other;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override int GetHashCode ()
		{
			return HashCode.Combine (RedGain, GreenGain, BlueGain);
		}
	}

	/// <summary>Structure holding CIE 1931 xy chromaticity values.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AVCaptureWhiteBalanceChromaticityValues {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float X;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float Y;

		/// <param name="x">To be added.</param>
		///         <param name="y">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public AVCaptureWhiteBalanceChromaticityValues (float x, float y)
		{
			X = x;
			Y = y;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			return String.Format ("({0},{1})", X, Y);
		}

		public static bool operator == (AVCaptureWhiteBalanceChromaticityValues left, AVCaptureWhiteBalanceChromaticityValues right)
		{
			return left.X == right.X && left.Y == right.Y;
		}

		public static bool operator != (AVCaptureWhiteBalanceChromaticityValues left, AVCaptureWhiteBalanceChromaticityValues right)
		{
			return left.X != right.X || left.Y != right.Y;
		}

		/// <param name="obj">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override bool Equals (object? obj)
		{
			if (!(obj is AVCaptureWhiteBalanceChromaticityValues))
				return false;

			return this.Equals ((AVCaptureWhiteBalanceChromaticityValues) obj);
		}

		/// <param name="other">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Equals (AVCaptureWhiteBalanceChromaticityValues other)
		{
			return this == other;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override int GetHashCode ()
		{
			return HashCode.Combine (X, Y);
		}
	}

	/// <summary>Values used for white-balancing; including correlated temperatures and tints.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AVCaptureWhiteBalanceTemperatureAndTintValues {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float Temperature;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float Tint;

		/// <param name="temperature">To be added.</param>
		///         <param name="tint">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public AVCaptureWhiteBalanceTemperatureAndTintValues (float temperature, float tint)
		{
			Temperature = temperature;
			Tint = tint;
		}
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			return String.Format ("(Temperature={0},Tint={1})", Temperature, Tint);
		}

		public static bool operator == (AVCaptureWhiteBalanceTemperatureAndTintValues left, AVCaptureWhiteBalanceTemperatureAndTintValues right)
		{
			return left.Temperature == right.Temperature && left.Tint == right.Tint;
		}

		public static bool operator != (AVCaptureWhiteBalanceTemperatureAndTintValues left, AVCaptureWhiteBalanceTemperatureAndTintValues right)
		{
			return left.Temperature != right.Temperature || left.Tint != right.Tint;

		}

		/// <param name="obj">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override bool Equals (object? obj)
		{
			if (!(obj is AVCaptureWhiteBalanceTemperatureAndTintValues))
				return false;

			return this.Equals ((AVCaptureWhiteBalanceTemperatureAndTintValues) obj);
		}

		/// <param name="other">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Equals (AVCaptureWhiteBalanceTemperatureAndTintValues other)
		{
			return this == other;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override int GetHashCode ()
		{
			return HashCode.Combine (Temperature, Tint);
		}
	}

#if !COREBUILD
	/// <summary>AV metadata identifiers.</summary>
	///     <remarks>To be added.</remarks>
	public static partial class AVMetadataIdentifiers {
	}
#endif

	/// <summary>Defines an extension method for <see cref="CGRect" /> that generates another rectangle with a specified aspect ratio.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public static class AVUtilities {

		[DllImport (Constants.AVFoundationLibrary)]
		static extern /* CGRect */ CGRect AVMakeRectWithAspectRatioInsideRect (/* CGSize */ CGSize aspectRatio, /* CGRect */ CGRect boundingRect);

		/// <param name="self">To be added.</param>
		///         <param name="aspectRatio">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CGRect WithAspectRatio (this CGRect self, CGSize aspectRatio)
		{
			return AVMakeRectWithAspectRatioInsideRect (aspectRatio, self);
		}
	}

	/// <summary>Contains media sample synchronization attributes.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AVSampleCursorSyncInfo {
#if XAMCORE_5_0
		byte isFullSync;
		byte isPartialSync;
		byte isDroppable;

		public bool IsFullSync {
			get => isFullSync != 0;
			set => isFullSync = value.AsByte ();
		}

		public bool IsPartialSync {
			get => isPartialSync != 0;
			set => isPartialSync = value.AsByte ();
		}

		public bool IsDroppable {
			get => isDroppable != 0;
			set => isDroppable = value.AsByte ();
		}
#else
		/// <summary>
		///           <see langword="true" /> if the sample is an Instantaneous Decoder Refresh sample and the developer can rely on it, by itself, to resynchronize a decoder.</summary>
		///         <remarks>To be added.</remarks>
		[MarshalAs (UnmanagedType.I1)]
		public bool IsFullSync;

		/// <summary>
		///           <see langword="true" /> if the sample is not Instantaneous Decoder Refresh sample.</summary>
		///         <remarks>To be added.</remarks>
		[MarshalAs (UnmanagedType.I1)]
		public bool IsPartialSync;

		/// <summary>
		///           <see langword="true" /> if the sample can be dropped.</summary>
		///         <remarks>To be added.</remarks>
		[MarshalAs (UnmanagedType.I1)]
		public bool IsDroppable;
#endif
	}

#if !XAMCORE_5_0
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[StructLayout (LayoutKind.Sequential)]
	[NativeName ("AVSampleCursorSyncInfo")]
#if COREBUILD
	public
#endif
	struct AVSampleCursorSyncInfo_Blittable {
		byte isFullSync;
		public bool IsFullSync {
			get => isFullSync != 0;
			set => isFullSync = value.AsByte ();
		}

		byte isPartialSync;
		public bool IsPartialSync {
			get => isPartialSync != 0;
			set => isPartialSync = value.AsByte ();
		}

		byte isDroppable;
		public bool IsDroppable {
			get => isDroppable != 0;
			set => isDroppable = value.AsByte ();
		}

		public AVSampleCursorSyncInfo ToAVSampleCursorSyncInfo ()
		{
			var rv = new AVSampleCursorSyncInfo ();
			rv.IsFullSync = IsFullSync;
			rv.IsPartialSync = IsPartialSync;
			rv.IsDroppable = IsDroppable;
			return rv;
		}
	}
#endif // !XAMCORE_5_0

	/// <summary>Contains media sample interdependency data for a sample and other samples in the sequence.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AVSampleCursorDependencyInfo {
#if XAMCORE_5_0
		byte indicatesWhetherItHasDependentSamples;
		byte hasDependentSamples;
		byte indicatesWhetherItDependsOnOthers;
		byte dependsOnOthers;
		byte indicatesWhetherItHasRedundantCoding;
		byte hasRedundantCoding;

		public bool IndicatesWhetherItHasDependentSamples {
			get => indicatesWhetherItHasDependentSamples != 0;
			set => indicatesWhetherItHasDependentSamples = value.AsByte ();
		}

		public bool HasDependentSamples {
			get => hasDependentSamples != 0;
			set => hasDependentSamples = value.AsByte ();
		}

		public bool IndicatesWhetherItDependsOnOthers {
			get => indicatesWhetherItDependsOnOthers != 0;
			set => indicatesWhetherItDependsOnOthers = value.AsByte ();
		}

		public bool DependsOnOthers {
			get => dependsOnOthers != 0;
			set => dependsOnOthers = value.AsByte ();
		}

		public bool IndicatesWhetherItHasRedundantCoding {
			get => indicatesWhetherItHasRedundantCoding != 0;
			set => indicatesWhetherItHasRedundantCoding = value.AsByte ();
		}

		public bool HasRedundantCoding {
			get => hasRedundantCoding != 0;
			set => hasRedundantCoding = value.AsByte ();
		}
#else
		/// <summary>
		///           <see langword="true" /> if and only if the sample indicates whether other samples in the sequence depend on the sample.</summary>
		///         <remarks>To be added.</remarks>
		[MarshalAs (UnmanagedType.I1)]
		public bool IndicatesWhetherItHasDependentSamples;

		/// <summary>
		///           <see langword="true" /> if and only if the sample has dependent samples.</summary>
		///         <remarks>To be added.</remarks>
		[MarshalAs (UnmanagedType.I1)]
		public bool HasDependentSamples;

		/// <summary>
		///           <see langword="true" /> if and only if the sample indicates whether it depends on other samples in the sequence.</summary>
		///         <remarks>To be added.</remarks>
		[MarshalAs (UnmanagedType.I1)]
		public bool IndicatesWhetherItDependsOnOthers;

		/// <summary>
		///           <see langword="true" /> if and only if the sample depends on other samples in the sequence.</summary>
		///         <remarks>To be added.</remarks>
		[MarshalAs (UnmanagedType.I1)]
		public bool DependsOnOthers;

		/// <summary>
		///           <see langword="true" /> if and only if the sample indicates whether it has redundant coding.</summary>
		///         <remarks>To be added.</remarks>
		[MarshalAs (UnmanagedType.I1)]
		public bool IndicatesWhetherItHasRedundantCoding;

		/// <summary>
		///           <see langword="true" /> if and only if the sample has redundant coding.</summary>
		///         <remarks>To be added.</remarks>
		[MarshalAs (UnmanagedType.I1)]
		public bool HasRedundantCoding;
#endif
	}

#if !XAMCORE_5_0
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[StructLayout (LayoutKind.Sequential)]
	[NativeName ("AVSampleCursorDependencyInfo")]
#if COREBUILD
	public
#endif
	struct AVSampleCursorDependencyInfo_Blittable {
		byte indicatesWhetherItHasDependentSamples;
		byte hasDependentSamples;
		byte indicatesWhetherItDependsOnOthers;
		byte dependsOnOthers;
		byte indicatesWhetherItHasRedundantCoding;
		byte hasRedundantCoding;

		public bool IndicatesWhetherItHasDependentSamples {
			get => indicatesWhetherItHasDependentSamples != 0;
			set => indicatesWhetherItHasDependentSamples = value.AsByte ();
		}

		public bool HasDependentSamples {
			get => hasDependentSamples != 0;
			set => hasDependentSamples = value.AsByte ();
		}

		public bool IndicatesWhetherItDependsOnOthers {
			get => indicatesWhetherItDependsOnOthers != 0;
			set => indicatesWhetherItDependsOnOthers = value.AsByte ();
		}

		public bool DependsOnOthers {
			get => dependsOnOthers != 0;
			set => dependsOnOthers = value.AsByte ();
		}

		public bool IndicatesWhetherItHasRedundantCoding {
			get => indicatesWhetherItHasRedundantCoding != 0;
			set => indicatesWhetherItHasRedundantCoding = value.AsByte ();
		}

		public bool HasRedundantCoding {
			get => hasRedundantCoding != 0;
			set => hasRedundantCoding = value.AsByte ();
		}

		public AVSampleCursorDependencyInfo ToAVSampleCursorDependencyInfo ()
		{
			var rv = new AVSampleCursorDependencyInfo ();
			rv.IndicatesWhetherItHasDependentSamples = IndicatesWhetherItHasDependentSamples;
			rv.HasDependentSamples = HasDependentSamples;
			rv.IndicatesWhetherItDependsOnOthers = IndicatesWhetherItDependsOnOthers;
			rv.DependsOnOthers = DependsOnOthers;
			rv.IndicatesWhetherItHasRedundantCoding = IndicatesWhetherItHasRedundantCoding;
			rv.HasRedundantCoding = HasRedundantCoding;
			return rv;
		}
	}
#endif // !XAMCORE_5_0

	/// <summary>Contains the location and size of a media sample or chunk.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AVSampleCursorStorageRange {
		/// <summary>The location of the first byte.</summary>
		///         <remarks>To be added.</remarks>
		public long Offset;
		/// <summary>The number of bytes in the sample or chunk.</summary>
		///         <remarks>To be added.</remarks>
		public long Length;
	}

	/// <summary>Contains media sample chunk metadata.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AVSampleCursorChunkInfo {
		/// <summary>The number of samples present.</summary>
		///         <remarks>To be added.</remarks>
		public long SampleCount;

#if XAMCORE_5_0
		byte hasUniformSampleSizes;
		byte hasUniformSampleDurations;
		byte hasUniformFormatDescriptions;

		public bool HasUniformSampleSizes {
			get => hasUniformSampleSizes != 0;
			set => hasUniformSampleSizes = value.AsByte ();
		}

		public bool HasUniformSampleDurations {
			get => hasUniformSampleDurations != 0;
			set => hasUniformSampleDurations = value.AsByte ();
		}

		public bool HasUniformFormatDescriptions {
			get => hasUniformFormatDescriptions != 0;
			set => hasUniformFormatDescriptions = value.AsByte ();
		}
#else
		/// <summary>
		///           <see langword="true" /> if and only if every chunk has the same sample size.</summary>
		///         <remarks>To be added.</remarks>
		[MarshalAs (UnmanagedType.I1)]
		public bool HasUniformSampleSizes;

		/// <summary>
		///           <see langword="true" /> if and only if every chunk has the same duration.</summary>
		///         <remarks>To be added.</remarks>
		[MarshalAs (UnmanagedType.I1)]
		public bool HasUniformSampleDurations;

		/// <summary>
		///           <see langword="true" /> if and only if every chunk has the same format description.</summary>
		///         <remarks>To be added.</remarks>
		[MarshalAs (UnmanagedType.I1)]
		public bool HasUniformFormatDescriptions;

		internal AVSampleCursorChunkInfo_Blittable ToBlittable ()
		{
			var rv = new AVSampleCursorChunkInfo_Blittable ();
			rv.HasUniformSampleSizes = HasUniformSampleSizes;
			rv.HasUniformSampleDurations = HasUniformSampleDurations;
			rv.HasUniformFormatDescriptions = HasUniformFormatDescriptions;
			return rv;
		}
#endif
	}

#if !XAMCORE_5_0
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[StructLayout (LayoutKind.Sequential)]
	[NativeName ("AVSampleCursorChunkInfo")]
#if COREBUILD
	public
#endif
	struct AVSampleCursorChunkInfo_Blittable {
		public long SampleCount;

		byte hasUniformSampleSizes;
		public bool HasUniformSampleSizes {
			get => hasUniformSampleSizes != 0;
			set => hasUniformSampleSizes = value.AsByte ();
		}

		byte hasUniformSampleDurations;
		public bool HasUniformSampleDurations {
			get => hasUniformSampleDurations != 0;
			set => hasUniformSampleDurations = value.AsByte ();
		}

		byte hasUniformFormatDescriptions;
		public bool HasUniformFormatDescriptions {
			get => hasUniformFormatDescriptions != 0;
			set => hasUniformFormatDescriptions = value.AsByte ();
		}

		public AVSampleCursorChunkInfo ToAVSampleCursorChunkInfo ()
		{
			var rv = new AVSampleCursorChunkInfo ();
			rv.HasUniformSampleSizes = HasUniformSampleSizes;
			rv.HasUniformSampleDurations = HasUniformSampleDurations;
			rv.HasUniformFormatDescriptions = HasUniformFormatDescriptions;
			return rv;
		}
	}
#endif // !XAMCORE_5_0

	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AVSampleCursorAudioDependencyInfo {
#if XAMCORE_5_0 || (__IOS__ && !__MACCATALYST__) || __TVOS__
		byte isIndependentlyDecodable;

		public bool IsIndependentlyDecodable {
			get => isIndependentlyDecodable != 0;
			set => isIndependentlyDecodable = value.AsByte ();
		}
#else
		[MarshalAs (UnmanagedType.I1)]
		public bool IsIndependentlyDecodable;
#endif

		public nint PacketRefreshCount;
	}

#if !XAMCORE_5_0 && !(__IOS__ && !__MACCATALYST__) && !__TVOS__
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[StructLayout (LayoutKind.Sequential)]
	[NativeName ("AVSampleCursorAudioDependencyInfo")]
#if COREBUILD
	public
#endif
	struct AVSampleCursorAudioDependencyInfo_Blittable {
		byte isIndependentlyDecodable;
		public bool IsIndependentlyDecodable {
			get => isIndependentlyDecodable != 0;
			set => isIndependentlyDecodable = value.AsByte ();
		}
		public nint PacketRefreshCount;
		public AVSampleCursorAudioDependencyInfo ToAVSampleCursorAudioDependencyInfo ()
		{
			var rv = new AVSampleCursorAudioDependencyInfo ();
			rv.IsIndependentlyDecodable = IsIndependentlyDecodable;
			rv.PacketRefreshCount = PacketRefreshCount;
			return rv;
		}
	}
#endif // !XAMCORE_5_0 && !__IOS__ && !__TVOS__

#if !__TVOS__
	[SupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("ios18.0")]
	[SupportedOSPlatform ("maccatalyst18.0")]
	[Native]
	public enum AVCaptionUnitsType : long {
		Unspecified = 0,
		Cells,
		Percent,
	}
#endif // __TVOS__

#if !__TVOS__
	[SupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("ios18.0")]
	[SupportedOSPlatform ("maccatalyst18.0")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AVCaptionDimension {
		public nfloat Value;
		nuint units;

		public AVCaptionUnitsType Units {
			get => (AVCaptionUnitsType) (long) units;
			set => units = (nuint) (long) value;
		}

		[DllImport (Constants.AVFoundationLibrary)]
		static extern AVCaptionDimension AVCaptionDimensionMake (nfloat dimension, /* AVCaptionUnitsType */ nuint units);

		public static AVCaptionDimension Create (nfloat dimension, AVCaptionUnitsType units)
			=> AVCaptionDimensionMake (dimension, (nuint) (long) units);
	}
#endif // __TVOS__

#if !__TVOS__
	[SupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("ios18.0")]
	[SupportedOSPlatform ("maccatalyst18.0")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AVCaptionPoint {
		public AVCaptionDimension X;
		public AVCaptionDimension Y;

		[DllImport (Constants.AVFoundationLibrary)]
		static extern AVCaptionPoint AVCaptionPointMake (AVCaptionDimension x, AVCaptionDimension y);

		public static AVCaptionPoint Create (AVCaptionDimension x, AVCaptionDimension y)
			=> AVCaptionPointMake (x, y);
	}
#endif // __TVOS__

#if !__TVOS__
	[SupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("ios18.0")]
	[SupportedOSPlatform ("maccatalyst18.0")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AVCaptionSize {
		public AVCaptionDimension Width;
		public AVCaptionDimension Height;

		[DllImport (Constants.AVFoundationLibrary)]
		static extern AVCaptionSize AVCaptionSizeMake (AVCaptionDimension width, AVCaptionDimension height);

		public static AVCaptionSize Create (AVCaptionDimension width, AVCaptionDimension height)
			=> AVCaptionSizeMake (width, height);
	}
#endif // __TVOS__
}
