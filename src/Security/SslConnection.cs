//
// SslConnection
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2014 Xamarin Inc.
//

#nullable enable

using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;

using ObjCRuntime;

namespace Security {

#if !NET
	unsafe delegate SslStatus SslReadFunc (IntPtr connection, IntPtr data, /* size_t* */ nint* dataLength);

	unsafe delegate SslStatus SslWriteFunc (IntPtr connection, IntPtr data, /* size_t* */ nint* dataLength);
#endif

#if NET
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[ObsoletedOSPlatform ("macos10.15", Constants.UseNetworkInstead)]
	[ObsoletedOSPlatform ("tvos13.0", Constants.UseNetworkInstead)]
	[ObsoletedOSPlatform ("ios13.0", Constants.UseNetworkInstead)]
	[ObsoletedOSPlatform ("maccatalyst13.0", Constants.UseNetworkInstead)]
#else
	[Deprecated (PlatformName.MacOSX, 10, 15, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.iOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.TvOS, 13, 0, message: Constants.UseNetworkInstead)]
#endif
	public abstract class SslConnection : IDisposable {

		GCHandle handle;

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		protected SslConnection ()
		{
			handle = GCHandle.Alloc (this);
			ConnectionId = GCHandle.ToIntPtr (handle);
#if !NET
			unsafe {
				ReadFunc = Read;
				WriteFunc = Write;
			}
#endif
		}

		~SslConnection ()
		{
			Dispose (false);
		}

		/// <summary>Releases the resources used by the SslConnection object.</summary>
		///         <remarks>
		///           <para>The Dispose method releases the resources used by the SslConnection class.</para>
		///           <para>Calling the Dispose method when the application is finished using the SslConnection ensures that all external resources used by this managed object are released as soon as possible.  Once developers have invoked the Dispose method, the object is no longer useful and developers should no longer make any calls to it.  For more information on releasing resources see ``Cleaning up Unmananaged Resources'' at https://msdn.microsoft.com/en-us/library/498928w2.aspx</para>
		///         </remarks>
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		/// <include file="../../docs/api/Security/SslConnection.xml" path="/Documentation/Docs[@DocId='M:Security.SslConnection.Dispose(System.Boolean)']/*" />
		protected virtual void Dispose (bool disposing)
		{
			if (handle.IsAllocated)
				handle.Free ();
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public IntPtr ConnectionId { get; private set; }

#if NET
		unsafe internal delegate* unmanaged<IntPtr, IntPtr, nint*, SslStatus> ReadFunc { get { return &Read; } }
		unsafe internal delegate* unmanaged<IntPtr, IntPtr, nint*, SslStatus> WriteFunc { get { return &Write; } }
#else
		internal SslReadFunc ReadFunc { get; private set; }
		internal SslWriteFunc WriteFunc { get; private set; }
#endif

		public abstract SslStatus Read (IntPtr data, ref nint dataLength);

		public abstract SslStatus Write (IntPtr data, ref nint dataLength);

#if NET
		[UnmanagedCallersOnly]
#else
		[MonoPInvokeCallback (typeof (SslReadFunc))]
#endif
		unsafe static SslStatus Read (IntPtr connection, IntPtr data, nint* dataLength)
		{
			var c = (SslConnection) GCHandle.FromIntPtr (connection).Target!;
			return c.Read (data, ref System.Runtime.CompilerServices.Unsafe.AsRef<nint> (dataLength));
		}

#if NET
		[UnmanagedCallersOnly]
#else
		[MonoPInvokeCallback (typeof (SslWriteFunc))]
#endif
		unsafe static SslStatus Write (IntPtr connection, IntPtr data, nint* dataLength)
		{
			var c = (SslConnection) GCHandle.FromIntPtr (connection).Target!;
			return c.Write (data, ref System.Runtime.CompilerServices.Unsafe.AsRef<nint> (dataLength));
		}
	}


#if NET
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
#endif
	// a concrete connection based on a managed Stream
	public class SslStreamConnection : SslConnection {

		byte [] buffer;

		/// <param name="stream">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public SslStreamConnection (Stream stream)
		{
			if (stream is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (stream));
			InnerStream = stream;
			// a bit higher than the default maximum fragment size
			buffer = new byte [16384];
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public Stream InnerStream { get; private set; }

		public override SslStatus Read (IntPtr data, ref nint dataLength)
		{
			// SSL state prevents multiple simultaneous reads (internal MAC would break)
			// so it's possible to reuse a single buffer (not re-allocate one each time)
			int len = (int) Math.Min (dataLength, buffer.Length);
			int read = InnerStream.Read (buffer, 0, len);
			Marshal.Copy (buffer, 0, data, read);
			bool block = (read < dataLength);
			dataLength = read;
			return block ? SslStatus.WouldBlock : SslStatus.Success;
		}

		public unsafe override SslStatus Write (IntPtr data, ref nint dataLength)
		{
			using (var ms = new UnmanagedMemoryStream ((byte*) data, dataLength)) {
				try {
					ms.CopyTo (InnerStream);
				} catch (IOException) {
					return SslStatus.ClosedGraceful;
				} catch {
					return SslStatus.Internal;
				}
			}
			return SslStatus.Success;
		}
	}
}
