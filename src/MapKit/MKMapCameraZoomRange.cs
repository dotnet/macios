using System;

using Foundation;
using ObjCRuntime;

#nullable enable

namespace MapKit {

	/// <summary>This enum is used to select how to initialize a new instance of a <see cref="MKMapCameraZoomRange" />.</summary>
	public enum MKMapCameraZoomRangeType {
		/// <summary>The specified distance is the minimum center coordinate distance.</summary>
		Min,
		/// <summary>The specified distance is the maximum center coordinate distance.</summary>
		Max,
	}

	public partial class MKMapCameraZoomRange {
		/// <summary>Create a new <see cref="MKMapCameraZoomRange" /> instance.</summary>
		/// <param name="distance">The minimum center coordinate distance.</param>
		public MKMapCameraZoomRange (double distance) : this (distance, MKMapCameraZoomRangeType.Min)
		{
		}

		/// <summary>Create a new <see cref="MKMapCameraZoomRange" /> instance.</summary>
		/// <param name="distance">The minimum or maximum center coordinate distance.</param>
		/// <param name="type">Specify whether <paramref name="distance" /> is the minimum or the maximum coordinate distance.</param>
		public MKMapCameraZoomRange (double distance, MKMapCameraZoomRangeType type)
			: base (NSObjectFlag.Empty)
		{
			switch (type) {
			case MKMapCameraZoomRangeType.Min:
				InitializeHandle (_InitWithMinCenterCoordinateDistance (distance));
				break;
			case MKMapCameraZoomRangeType.Max:
				InitializeHandle (_InitWithMaxCenterCoordinateDistance (distance));
				break;
			default:
				throw new ArgumentException (nameof (type));
			}
		}
	}
}
