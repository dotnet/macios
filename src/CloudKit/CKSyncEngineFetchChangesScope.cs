using System;
using ObjCRuntime;
using Foundation;

#nullable enable

namespace CloudKit {
	public partial class CKSyncEngineFetchChangesScope {
		/// <summary>Creates a new <see cref="CKSyncEngineFetchChangesScope" /> using the specified <paramref name="zoneIds" /> and whether the zone identifiers are included or excluded.</summary>
		/// <param name="zoneIds">A set of <see cref="CKRecordZoneID" />s to include or exclude.</param>
		/// <param name="excluded">Determines whether the specified <paramref name="zoneIds" /> are included or excluded.</param>
		/// <returns>A new <see cref="CKSyncEngineFetchChangesScope" /> instance using the specified <paramref name="zoneIds" />.</returns>
		public CKSyncEngineFetchChangesScope (NSSet<CKRecordZoneID>? zoneIds, bool excluded)
			: base (NSObjectFlag.Empty)
		{
			if (excluded) {
				// needs to be converted to an empty set
				zoneIds ??= new NSSet<CKRecordZoneID> ();
				InitializeHandle (_InitWithExcludedZoneIds (zoneIds), "initWithExcludedZoneIDs:");
			} else {
				// supports a null parameter
				InitializeHandle (_InitWithZoneIds (zoneIds), "initWithZoneIDs:");
			}
		}
	}
}

