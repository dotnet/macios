// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace CoreMediaIO {
	/// <summary>Identifies a property of a Core Media I/O object.</summary>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct CMIOObjectPropertyAddress {
		/// <summary>The selector identifying the property.</summary>
		public CMIOObjectPropertySelector Selector;
		/// <summary>The scope containing the property.</summary>
		public CMIOObjectPropertyScope Scope;
		/// <summary>The element containing the property. Zero selects the main element.</summary>
		public uint Element;

		/// <summary>Creates a property address. The default scope is global and the default element is the main element.</summary>
		public CMIOObjectPropertyAddress (CMIOObjectPropertySelector selector, CMIOObjectPropertyScope scope = CMIOObjectPropertyScope.Global, uint element = 0)
		{
			Selector = selector;
			Scope = scope;
			Element = element;
		}
	}

	/// <summary>Provides access to Core Media I/O hardware objects and their properties.</summary>
	/// <remarks>
	///   <para>Object identifiers are not Objective-C or Core Foundation handles and must not be retained or released. Property buffers use the native representation of the property: for example, device and stream lists contain 32-bit unsigned identifiers.</para>
	///   <para>Discover devices by reading <see cref="CMIOObjectPropertySelector.HardwareDevices" /> on <see cref="SystemObject" />. Discover a device's input streams by reading <see cref="CMIOObjectPropertySelector.DeviceStreams" /> in the input scope. Copy a stream's queue before starting it with <see cref="CMIODevice.StartStream" />, and stop it with <see cref="CMIODevice.StopStream" /> when finished.</para>
	/// </remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	public static class CMIOObject {
		/// <summary>The identifier of the process-wide system object.</summary>
		public const uint SystemObject = 1;

		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern void CMIOObjectShow (uint objectId);

		/// <summary>Writes a description of an object to standard output.</summary>
		public static void Show (uint objectId) => CMIOObjectShow (objectId);

		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern byte CMIOObjectHasProperty (uint objectId, in CMIOObjectPropertyAddress address);

		/// <summary>Determines whether an object has a property.</summary>
		public static bool HasProperty (uint objectId, in CMIOObjectPropertyAddress address)
			=> CMIOObjectHasProperty (objectId, in address) != 0;

		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern int CMIOObjectIsPropertySettable (uint objectId, in CMIOObjectPropertyAddress address, out byte isSettable);

		/// <summary>Queries whether a property can be set, returning an OSStatus error code or zero on success.</summary>
		public static int IsPropertySettable (uint objectId, in CMIOObjectPropertyAddress address, out bool isSettable)
		{
			var status = CMIOObjectIsPropertySettable (objectId, in address, out var value);
			isSettable = value != 0;
			return status;
		}

		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern unsafe int CMIOObjectGetPropertyDataSize (uint objectId, in CMIOObjectPropertyAddress address, uint qualifierDataSize, byte* qualifierData, out uint dataSize);

		/// <summary>Queries a property's size in bytes, returning an OSStatus error code or zero on success.</summary>
		public static int GetPropertyDataSize (uint objectId, in CMIOObjectPropertyAddress address, ReadOnlySpan<byte> qualifierData, out uint dataSize)
		{
			unsafe {
				fixed (byte* qualifier = qualifierData)
					return CMIOObjectGetPropertyDataSize (objectId, in address, (uint) qualifierData.Length, qualifier, out dataSize);
			}
		}

		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern unsafe int CMIOObjectGetPropertyData (uint objectId, in CMIOObjectPropertyAddress address, uint qualifierDataSize, byte* qualifierData, uint dataSize, out uint dataUsed, byte* data);

		/// <summary>Reads a property into a buffer, returning an OSStatus error code or zero on success.</summary>
		/// <remarks>The number of bytes written is returned in <paramref name="dataUsed" />. The caller is responsible for the ownership of any native objects returned in the buffer, as specified by the property.</remarks>
		public static int GetPropertyData (uint objectId, in CMIOObjectPropertyAddress address, ReadOnlySpan<byte> qualifierData, Span<byte> data, out uint dataUsed)
		{
			unsafe {
				fixed (byte* qualifier = qualifierData)
				fixed (byte* buffer = data)
					return CMIOObjectGetPropertyData (objectId, in address, (uint) qualifierData.Length, qualifier, (uint) data.Length, out dataUsed, buffer);
			}
		}

		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern unsafe int CMIOObjectSetPropertyData (uint objectId, in CMIOObjectPropertyAddress address, uint qualifierDataSize, byte* qualifierData, uint dataSize, byte* data);

		/// <summary>Sets a property from a buffer, returning an OSStatus error code or zero on success.</summary>
		public static int SetPropertyData (uint objectId, in CMIOObjectPropertyAddress address, ReadOnlySpan<byte> qualifierData, ReadOnlySpan<byte> data)
		{
			unsafe {
				fixed (byte* qualifier = qualifierData)
				fixed (byte* buffer = data)
					return CMIOObjectSetPropertyData (objectId, in address, (uint) qualifierData.Length, qualifier, (uint) data.Length, buffer);
			}
		}

		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern unsafe int CMIOObjectAddPropertyListener (uint objectId, in CMIOObjectPropertyAddress address, delegate* unmanaged<uint, uint, CMIOObjectPropertyAddress*, IntPtr, int> listener, IntPtr clientData);

		/// <summary>Registers an unmanaged property listener, returning an OSStatus error code or zero on success.</summary>
		/// <remarks>The callback receives the object identifier, address count, address array, and client data, and must return zero. Keep the callback and client data valid until the listener has been successfully removed. Exceptions must not escape the callback.</remarks>
		public static unsafe int AddPropertyListener (uint objectId, in CMIOObjectPropertyAddress address, delegate* unmanaged<uint, uint, CMIOObjectPropertyAddress*, IntPtr, int> listener, IntPtr clientData)
		{
			if (listener is null)
				throw new ArgumentNullException (nameof (listener));
			return CMIOObjectAddPropertyListener (objectId, in address, listener, clientData);
		}

		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern unsafe int CMIOObjectRemovePropertyListener (uint objectId, in CMIOObjectPropertyAddress address, delegate* unmanaged<uint, uint, CMIOObjectPropertyAddress*, IntPtr, int> listener, IntPtr clientData);

		/// <summary>Removes a listener using the same address, callback, and client data used to register it, returning an OSStatus error code or zero on success.</summary>
		public static unsafe int RemovePropertyListener (uint objectId, in CMIOObjectPropertyAddress address, delegate* unmanaged<uint, uint, CMIOObjectPropertyAddress*, IntPtr, int> listener, IntPtr clientData)
		{
			if (listener is null)
				throw new ArgumentNullException (nameof (listener));
			return CMIOObjectRemovePropertyListener (objectId, in address, listener, clientData);
		}
	}
}
