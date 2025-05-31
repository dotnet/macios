// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Runtime.InteropServices;

using Foundation;

#nullable enable

namespace FSKit {
	public partial class FSBlockDeviceResource {
		static void ValidateArray (byte [] buffer, long offset, nuint length)
		{
			if (buffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (buffer));

			if (offset < 0)
				ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (offset), offset, "Must be positive.");

			var minLength = (ulong) offset + length;
			// first check for overflow
			if (minLength < (ulong) offset || minLength < length)
				ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (length), minLength, "The sum of 'offset' and 'length' can't be larger than the buffer length.");
			// actually validate
			if (minLength > (ulong) buffer.Length)
				ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (length), minLength, "The sum of 'offset' and 'length' can't be larger than the buffer length.");
		}

		/// <summary>Read data from a resource and execute a callback when completed.</summary>
		/// <param name="buffer">An array of bytes where the read data will be stored.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data read from the current resource.</param>
		/// <param name="length">The maximum number of bytes to be read from the current resource.</param>
		/// <param name="reply">The callback to call when the read operation is complete.</param>
		public unsafe void Read (byte [] buffer, long offset, nuint length, FSBlockDeviceResourceReadReplyHandler reply)
		{
			ValidateArray (buffer, offset, length);

			var handle = GCHandle.Alloc (buffer, GCHandleType.Pinned);
			var bufferPtr = handle.AddrOfPinnedObject ();
			Read (bufferPtr, offset, length, (actuallyRead, error) => {
				handle.Free ();
				reply (actuallyRead, error);
			});
		}

		/// <summary>Read data from a resource.</summary>
		/// <param name="buffer">An array of bytes where the read data will be stored.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data read from the current resource.</param>
		/// <param name="length">The maximum number of bytes to be read from the current resource.</param>
		/// <param name="error">The error object if the operation fails, <see langword="null" /> otherwise.</param>
		/// <returns>The number of bytes read.</returns>
		public unsafe nuint Read (byte [] buffer, long offset, nuint length, out NSError? error)
		{
			ValidateArray (buffer, offset, length);

			fixed (byte* bufferPtr = buffer) {
				return Read ((IntPtr) bufferPtr, offset, length, out error);
			}
		}

		/// <summary>Write data to a resource and execute a callback when completed.</summary>
		/// <param name="buffer">An array of bytes with the data to write.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> where to begin reading the data to be written to the current resource.</param>
		/// <param name="length">The maximum number of bytes to write to the current resource.</param>
		/// <param name="reply">The callback to call when the write operation is complete.</param>
		public unsafe void Write (byte [] buffer, long offset, nuint length, FSBlockDeviceResourceWriteReplyHandler reply)
		{
			ValidateArray (buffer, offset, length);

			var handle = GCHandle.Alloc (buffer, GCHandleType.Pinned);
			var bufferPtr = handle.AddrOfPinnedObject ();
			Write (bufferPtr, offset, length, (actuallyWritten, error) => {
				handle.Free ();
				reply (actuallyWritten, error);
			});
		}

		/// <summary>Write data to a resource and execute a callback when completed.</summary>
		/// <param name="buffer">An array of bytes with the data to write.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> where to begin reading the data to be written to the current resource.</param>
		/// <param name="length">The maximum number of bytes to write to the current resource.</param>
		/// <param name="error">The error object if the operation fails, <see langword="null" /> otherwise.</param>
		/// <returns>The number of bytes written.</returns>
		public unsafe nuint Write (byte [] buffer, long offset, nuint length, out NSError? error)
		{
			ValidateArray (buffer, offset, length);

			fixed (byte* bufferPtr = buffer) {
				return Write ((IntPtr) bufferPtr, offset, length, out error);
			}
		}

		/// <summary>Synchronously reads metadata from a resource.</summary>
		/// <param name="buffer">An array of bytes where the read data will be stored.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data read from the current resource.</param>
		/// <param name="length">The maximum number of bytes to be read from the current resource.</param>
		/// <param name="error">The error object if the operation fails, <see langword="null" /> otherwise.</param>
		/// <returns>If the operation succeeded or not.</returns>
		public unsafe bool MetadataRead (byte [] buffer, long offset, nuint length, out NSError? error)
		{
			ValidateArray (buffer, offset, length);

			fixed (byte* bufferPtr = buffer) {
				return MetadataRead ((IntPtr) bufferPtr, offset, length, out error);
			}
		}

		/// <summary>Synchronously writes metadata to a resource.</summary>
		/// <param name="buffer">An array of bytes with the data to write.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> where to begin reading the data to be written to the current resource.</param>
		/// <param name="length">The maximum number of bytes to write to the current resource.</param>
		/// <param name="error">The error object if the operation fails, <see langword="null" /> otherwise.</param>
		/// <returns>If the operation succeeded or not.</returns>
		public unsafe bool MetadataWrite (byte [] buffer, long offset, nuint length, out NSError? error)
		{
			ValidateArray (buffer, offset, length);

			fixed (byte* bufferPtr = buffer) {
				return MetadataWrite ((IntPtr) bufferPtr, offset, length, out error);
			}
		}

		/// <summary>Asynchronously writes metadata to a resource.</summary>
		/// <param name="buffer">An array of bytes with the data to write.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> where to begin reading the data to be written to the current resource.</param>
		/// <param name="length">The maximum number of bytes to write to the current resource.</param>
		/// <param name="error">The error object if the operation fails, <see langword="null" /> otherwise.</param>
		/// <returns>If the operation succeeded or not.</returns>
		public unsafe bool DelayedMetadataWrite (byte [] buffer, long offset, nuint length, out NSError? error)
		{
			ValidateArray (buffer, offset, length);

			fixed (byte* bufferPtr = buffer) {
				return DelayedMetadataWrite ((IntPtr) bufferPtr, offset, length, out error);
			}
		}
	}
}
