#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using ObjCRuntime;

namespace CoreMediaIO {

#if !COREBUILD
	/// <summary>Represents a CoreMediaIO hardware object and provides methods to query and modify its properties.</summary>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst15.4")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	public class CMIOObject {

		/// <summary>The object ID of the CoreMediaIO system object.</summary>
		public const uint SystemObjectId = 1;

		/// <summary>A wildcard selector that matches any property selector.</summary>
		public const uint SelectorWildcard = 0x2A2A2A2A; // '****'

		/// <summary>A wildcard scope that matches any property scope.</summary>
		public const uint ScopeWildcard = 0x2A2A2A2A; // '****'

		/// <summary>A wildcard element that matches any property element.</summary>
		public const uint ElementWildcard = 0xFFFFFFFF;

		/// <summary>The property selector for owned objects.</summary>
		public const uint OwnedObjectsSelector = 0x6F776E65; // 'owne'

		/// <summary>The property scope for global properties.</summary>
		public const uint GlobalScope = 0x676C6F62; // 'glob'

		/// <summary>Gets the native object ID.</summary>
		public uint ObjectId { get; }

		/// <summary>Creates a new <see cref="CMIOObject" /> wrapping the specified native object ID.</summary>
		/// <param name="objectId">The native CoreMediaIO object identifier.</param>
		public CMIOObject (uint objectId)
		{
			ObjectId = objectId;
		}

		/// <summary>Prints a textual description of the object to stdout.</summary>
		public void Show ()
		{
			CMIOInterop.CMIOObjectShow (ObjectId);
		}

		/// <summary>Queries whether this object has the specified property.</summary>
		/// <param name="address">The address of the property to check.</param>
		/// <returns><see langword="true" /> if the property exists; otherwise, <see langword="false" />.</returns>
		public bool HasProperty (CMIOObjectPropertyAddress address)
		{
			unsafe {
				return CMIOInterop.CMIOObjectHasProperty (ObjectId, &address) != 0;
			}
		}

		/// <summary>Queries whether the specified property can be modified.</summary>
		/// <param name="address">The address of the property to check.</param>
		/// <param name="status">On return, the status code from the native call; 0 indicates success.</param>
		/// <returns><see langword="true" /> if the property is settable; otherwise, <see langword="false" />.</returns>
		public bool IsPropertySettable (CMIOObjectPropertyAddress address, out int status)
		{
			unsafe {
				byte result;
				status = CMIOInterop.CMIOObjectIsPropertySettable (ObjectId, &address, &result);
				return result != 0;
			}
		}

		/// <summary>Gets the size in bytes of the data for the specified property.</summary>
		/// <param name="address">The address of the property.</param>
		/// <param name="status">On return, the status code from the native call; 0 indicates success.</param>
		/// <returns>The size of the property data in bytes.</returns>
		public uint GetPropertyDataSize (CMIOObjectPropertyAddress address, out int status)
		{
			unsafe {
				uint size;
				status = CMIOInterop.CMIOObjectGetPropertyDataSize (ObjectId, &address, 0, IntPtr.Zero, &size);
				return size;
			}
		}

		/// <summary>Gets the size in bytes of the data for the specified property, with qualifier data.</summary>
		/// <param name="address">The address of the property.</param>
		/// <param name="qualifierDataSize">The size of the qualifier data.</param>
		/// <param name="qualifierData">A pointer to the qualifier data.</param>
		/// <param name="status">On return, the status code from the native call; 0 indicates success.</param>
		/// <returns>The size of the property data in bytes.</returns>
		public uint GetPropertyDataSize (CMIOObjectPropertyAddress address, uint qualifierDataSize, IntPtr qualifierData, out int status)
		{
			unsafe {
				uint size;
				status = CMIOInterop.CMIOObjectGetPropertyDataSize (ObjectId, &address, qualifierDataSize, qualifierData, &size);
				return size;
			}
		}

		/// <summary>Gets the value of the specified property.</summary>
		/// <param name="address">The address of the property.</param>
		/// <param name="dataSize">The size of the data buffer.</param>
		/// <param name="dataUsed">On return, the actual number of bytes written.</param>
		/// <param name="data">A pointer to the buffer that will receive the property data.</param>
		/// <returns>An <see cref="int" /> status code; 0 indicates success.</returns>
		public int GetPropertyData (CMIOObjectPropertyAddress address, uint dataSize, out uint dataUsed, IntPtr data)
		{
			unsafe {
				uint used;
				int status = CMIOInterop.CMIOObjectGetPropertyData (ObjectId, &address, 0, IntPtr.Zero, dataSize, &used, data);
				dataUsed = used;
				return status;
			}
		}

		/// <summary>Gets the value of the specified property, with qualifier data.</summary>
		/// <param name="address">The address of the property.</param>
		/// <param name="qualifierDataSize">The size of the qualifier data.</param>
		/// <param name="qualifierData">A pointer to the qualifier data.</param>
		/// <param name="dataSize">The size of the data buffer.</param>
		/// <param name="dataUsed">On return, the actual number of bytes written.</param>
		/// <param name="data">A pointer to the buffer that will receive the property data.</param>
		/// <returns>An <see cref="int" /> status code; 0 indicates success.</returns>
		public int GetPropertyData (CMIOObjectPropertyAddress address, uint qualifierDataSize, IntPtr qualifierData, uint dataSize, out uint dataUsed, IntPtr data)
		{
			unsafe {
				uint used;
				int status = CMIOInterop.CMIOObjectGetPropertyData (ObjectId, &address, qualifierDataSize, qualifierData, dataSize, &used, data);
				dataUsed = used;
				return status;
			}
		}

		/// <summary>Sets the value of the specified property.</summary>
		/// <param name="address">The address of the property.</param>
		/// <param name="dataSize">The size of the data being set.</param>
		/// <param name="data">A pointer to the buffer containing the new property data.</param>
		/// <returns>An <see cref="int" /> status code; 0 indicates success.</returns>
		public int SetPropertyData (CMIOObjectPropertyAddress address, uint dataSize, IntPtr data)
		{
			unsafe {
				return CMIOInterop.CMIOObjectSetPropertyData (ObjectId, &address, 0, IntPtr.Zero, dataSize, data);
			}
		}

		/// <summary>Sets the value of the specified property, with qualifier data.</summary>
		/// <param name="address">The address of the property.</param>
		/// <param name="qualifierDataSize">The size of the qualifier data.</param>
		/// <param name="qualifierData">A pointer to the qualifier data.</param>
		/// <param name="dataSize">The size of the data being set.</param>
		/// <param name="data">A pointer to the buffer containing the new property data.</param>
		/// <returns>An <see cref="int" /> status code; 0 indicates success.</returns>
		public int SetPropertyData (CMIOObjectPropertyAddress address, uint qualifierDataSize, IntPtr qualifierData, uint dataSize, IntPtr data)
		{
			unsafe {
				return CMIOInterop.CMIOObjectSetPropertyData (ObjectId, &address, qualifierDataSize, qualifierData, dataSize, data);
			}
		}
	}
#endif // !COREBUILD
}
