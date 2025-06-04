//
// NSUuid.cs: support code for the NSUUID binding
//
// Authors:
//    MIguel de Icaza (miguel@xamarin.com)
//
// Copyright 2012-2013 Xamarin Inc
//
using System;
using System.Runtime.InteropServices;
using ObjCRuntime;

namespace Foundation {
	partial class NSUuid {

		/// <summary>Create a new <see cref="NSUuid" /> from the specified bytes.</summary>
		/// <param name="bytes">The array of bytes to initialize the new <see cref="NSUuid" /> with. Must have at least 16 elements.</param>
		public NSUuid (byte [] bytes) : base (NSObjectFlag.Empty)
		{
			if (bytes is null)
				throw new ArgumentNullException (nameof (bytes));
			if (bytes.Length < 16)
				throw new ArgumentException ("length must be at least 16 bytes");

			unsafe {
				fixed (byte* p = bytes) {
					IntPtr ptr = (IntPtr) p;

					if (IsDirectBinding) {
						InitializeHandle (Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.GetHandle ("initWithUUIDBytes:"), ptr), "initWithUUIDBytes:");
					} else {
						InitializeHandle (Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.GetHandle ("initWithUUIDBytes:"), ptr), "initWithUUIDBytes:");
					}
				}
			}
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public byte [] GetBytes ()
		{
			byte [] ret = new byte [16];

			unsafe {
				fixed (byte* buf = ret) {
					GetUuidBytes ((IntPtr) buf);
				}
			}

			return ret;
		}
	}
}
