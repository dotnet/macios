#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using ObjCRuntime;

namespace CoreMediaIO {

#if !COREBUILD
	/// <summary>Provides managed wrappers for CoreMediaIO hardware object property C functions.</summary>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst15.4")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	public static unsafe class CMIOObject {

		/// <summary>Prints a textual description of the object to stdout.</summary>
		/// <param name="objectId">The object to display.</param>
		public static void Show (uint objectId)
		{
			CMIOInterop.CMIOObjectShow (objectId);
		}

		/// <summary>Queries whether the given object has the specified property.</summary>
		/// <param name="objectId">The object to query.</param>
		/// <param name="address">The address of the property to check.</param>
		/// <returns><see langword="true" /> if the property exists; otherwise, <see langword="false" />.</returns>
		public static bool HasProperty (uint objectId, CMIOObjectPropertyAddress address)
		{
			return CMIOInterop.CMIOObjectHasProperty (objectId, &address) != 0;
		}

		/// <summary>Queries whether the given property is settable.</summary>
		/// <param name="objectId">The object to query.</param>
		/// <param name="address">The address of the property to check.</param>
		/// <param name="isSettable">On return, indicates whether the property can be set.</param>
		/// <returns>An <see cref="int" /> status code; 0 indicates success.</returns>
		public static int IsPropertySettable (uint objectId, CMIOObjectPropertyAddress address, out bool isSettable)
		{
			byte result;
			int status = CMIOInterop.CMIOObjectIsPropertySettable (objectId, &address, &result);
			isSettable = result != 0;
			return status;
		}

		/// <summary>Gets the size in bytes of the data for the specified property.</summary>
		/// <param name="objectId">The object to query.</param>
		/// <param name="address">The address of the property.</param>
		/// <param name="dataSize">On return, the size of the property data in bytes.</param>
		/// <returns>An <see cref="int" /> status code; 0 indicates success.</returns>
		public static int GetPropertyDataSize (uint objectId, CMIOObjectPropertyAddress address, out uint dataSize)
		{
			uint size;
			int status = CMIOInterop.CMIOObjectGetPropertyDataSize (objectId, &address, 0, IntPtr.Zero, &size);
			dataSize = size;
			return status;
		}

		/// <summary>Gets the size in bytes of the data for the specified property, with qualifier data.</summary>
		/// <param name="objectId">The object to query.</param>
		/// <param name="address">The address of the property.</param>
		/// <param name="qualifierDataSize">The size of the qualifier data.</param>
		/// <param name="qualifierData">A pointer to the qualifier data.</param>
		/// <param name="dataSize">On return, the size of the property data in bytes.</param>
		/// <returns>An <see cref="int" /> status code; 0 indicates success.</returns>
		public static int GetPropertyDataSize (uint objectId, CMIOObjectPropertyAddress address, uint qualifierDataSize, IntPtr qualifierData, out uint dataSize)
		{
			uint size;
			int status = CMIOInterop.CMIOObjectGetPropertyDataSize (objectId, &address, qualifierDataSize, qualifierData, &size);
			dataSize = size;
			return status;
		}

		/// <summary>Gets the value of the specified property.</summary>
		/// <param name="objectId">The object to query.</param>
		/// <param name="address">The address of the property.</param>
		/// <param name="dataSize">The size of the data buffer.</param>
		/// <param name="dataUsed">On return, the actual number of bytes written.</param>
		/// <param name="data">A pointer to the buffer that will receive the property data.</param>
		/// <returns>An <see cref="int" /> status code; 0 indicates success.</returns>
		public static int GetPropertyData (uint objectId, CMIOObjectPropertyAddress address, uint dataSize, out uint dataUsed, IntPtr data)
		{
			uint used;
			int status = CMIOInterop.CMIOObjectGetPropertyData (objectId, &address, 0, IntPtr.Zero, dataSize, &used, data);
			dataUsed = used;
			return status;
		}

		/// <summary>Gets the value of the specified property, with qualifier data.</summary>
		/// <param name="objectId">The object to query.</param>
		/// <param name="address">The address of the property.</param>
		/// <param name="qualifierDataSize">The size of the qualifier data.</param>
		/// <param name="qualifierData">A pointer to the qualifier data.</param>
		/// <param name="dataSize">The size of the data buffer.</param>
		/// <param name="dataUsed">On return, the actual number of bytes written.</param>
		/// <param name="data">A pointer to the buffer that will receive the property data.</param>
		/// <returns>An <see cref="int" /> status code; 0 indicates success.</returns>
		public static int GetPropertyData (uint objectId, CMIOObjectPropertyAddress address, uint qualifierDataSize, IntPtr qualifierData, uint dataSize, out uint dataUsed, IntPtr data)
		{
			uint used;
			int status = CMIOInterop.CMIOObjectGetPropertyData (objectId, &address, qualifierDataSize, qualifierData, dataSize, &used, data);
			dataUsed = used;
			return status;
		}

		/// <summary>Sets the value of the specified property.</summary>
		/// <param name="objectId">The object to modify.</param>
		/// <param name="address">The address of the property.</param>
		/// <param name="dataSize">The size of the data being set.</param>
		/// <param name="data">A pointer to the buffer containing the new property data.</param>
		/// <returns>An <see cref="int" /> status code; 0 indicates success.</returns>
		public static int SetPropertyData (uint objectId, CMIOObjectPropertyAddress address, uint dataSize, IntPtr data)
		{
			return CMIOInterop.CMIOObjectSetPropertyData (objectId, &address, 0, IntPtr.Zero, dataSize, data);
		}

		/// <summary>Sets the value of the specified property, with qualifier data.</summary>
		/// <param name="objectId">The object to modify.</param>
		/// <param name="address">The address of the property.</param>
		/// <param name="qualifierDataSize">The size of the qualifier data.</param>
		/// <param name="qualifierData">A pointer to the qualifier data.</param>
		/// <param name="dataSize">The size of the data being set.</param>
		/// <param name="data">A pointer to the buffer containing the new property data.</param>
		/// <returns>An <see cref="int" /> status code; 0 indicates success.</returns>
		public static int SetPropertyData (uint objectId, CMIOObjectPropertyAddress address, uint qualifierDataSize, IntPtr qualifierData, uint dataSize, IntPtr data)
		{
			return CMIOInterop.CMIOObjectSetPropertyData (objectId, &address, qualifierDataSize, qualifierData, dataSize, data);
		}
	}
#endif // !COREBUILD
}
