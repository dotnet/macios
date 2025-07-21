using System;
using System.Runtime.Versioning;
using Foundation;
using CoreMedia;


namespace AVFoundation {
	public partial class AVAssetTrack {

		/// <summary>
		/// An array of <see cref="CoreMedia.CMFormatDescription" />s that describe the formats of the samples in the <see cref="AVFoundation.AVAssetTrack" />.
		/// </summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		public CMFormatDescription [] FormatDescriptions {
			get {
				return (Array.ConvertAll (FormatDescriptionsAsObjects,
					obj => {
						var description = CMFormatDescription.Create (obj.Handle, false);
						GC.KeepAlive (obj);
						return description;
					}
				))!;
			}
		}
	}
}
