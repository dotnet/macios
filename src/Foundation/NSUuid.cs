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

		/// <param name="bytes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSUuid (byte [] bytes) : base (NSObjectFlag.Empty)
		{
			if (bytes is null)
				throw new ArgumentNullException ("bytes");
			if (bytes.Length < 16)
				throw new ArgumentException ("length must be at least 16 bytes");

			unsafe {
				fixed (byte* p = bytes) {
					IntPtr ptr = (IntPtr) p;

					if (IsDirectBinding) {
						Handle = Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.GetHandle ("initWithUUIDBytes:"), ptr);
					} else {
						Handle = Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.GetHandle ("initWithUUIDBytes:"), ptr);
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
