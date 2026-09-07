#if IOS

#nullable enable

namespace AVFoundation {
	public partial class AVAssetDownloadStorageManagementPolicy {

		/// <summary>Gets the eviction priority for the downloaded asset.</summary>
		/// <value>The eviction priority that determines when the downloaded asset may be purged.</value>
		public virtual AVAssetDownloadedAssetEvictionPriority Priority {
			get { return AVAssetDownloadedAssetEvictionPriorityExtensions.GetValue (_Priority); }
			set { throw new NotImplementedException (); }
		}
	}

	public partial class AVMutableAssetDownloadStorageManagementPolicy {

		/// <summary>Gets or sets the eviction priority for the downloaded asset.</summary>
		/// <value>The eviction priority that determines when the downloaded asset may be purged.</value>
		public override AVAssetDownloadedAssetEvictionPriority Priority {
			get { return AVAssetDownloadedAssetEvictionPriorityExtensions.GetValue (_Priority); }
			set { _Priority = value.GetConstant () ?? throw new ArgumentOutOfRangeException (nameof (Priority)); }
		}
	}
}

#endif
