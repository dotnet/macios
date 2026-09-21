// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace FSKit {
	public partial class FSEntityIdentifier {
		/// <summary>Creates an entity identifier with the specified UUID and qualifier data.</summary>
		/// <param name="uuid">The UUID for the entity identifier.</param>
		/// <param name="qualifierData">The eight bytes of data that distinguish entities sharing the same UUID.</param>
		/// <returns>A new entity identifier, or <see langword="null" /> if <paramref name="qualifierData" /> isn't exactly eight bytes long.</returns>
		[SupportedOSPlatform ("macos27.0")]
		public static FSEntityIdentifier? Create (NSUuid uuid, NSData qualifierData)
		{
			if (uuid is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (uuid));
			if (qualifierData is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (qualifierData));

			var rv = new FSEntityIdentifier (NSObjectFlag.Empty);
			rv.InitializeHandle (rv._InitWithUuidQualifierData (uuid, qualifierData), "initWithUUID:qualifierData:", false);
			if (rv.Handle == NativeHandle.Zero) {
				rv.Dispose ();
				return null;
			}
			return rv;
		}
	}
}
