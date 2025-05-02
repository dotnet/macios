// 
// CTRunDelegate.cs: Implements the managed CTRunDelegate
//
// Authors: Mono Team
//     
// Copyright 2010 Novell, Inc
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using ObjCRuntime;
using Foundation;
using CoreFoundation;
using CoreGraphics;

namespace CoreText {

	#region Run Delegate Callbacks
	delegate void CTRunDelegateDeallocateCallback (IntPtr refCon);
	delegate nfloat CTRunDelegateGetCallback (IntPtr refCon);

	[StructLayout (LayoutKind.Sequential)]
	struct CTRunDelegateCallbacks {
		public /* CFIndex */ nint version;
		public unsafe delegate* unmanaged<IntPtr, void> dealloc;
		public unsafe delegate* unmanaged<IntPtr, nfloat> getAscent;
		public unsafe delegate* unmanaged<IntPtr, nfloat> getDescent;
		public unsafe delegate* unmanaged<IntPtr, nfloat> getWidth;
	}
	#endregion

	/// <summary>A class that represents the operations possible on a <see cref="CoreText.CTRunDelegate" />.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CTRunDelegateOperations : IDisposable {
		// This instance is kept alive using a GCHandle until the Deallocate callback has been called,
		// which is called when the corresponding CTRunDelegate is freed (retainCount reaches 0).
		// This even means that the GCHandle is not freed if Dispose is called manually.
		GCHandle handle;

		public IntPtr Handle {
			get { return GCHandle.ToIntPtr (handle); }
		}

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		protected CTRunDelegateOperations ()
		{
			handle = GCHandle.Alloc (this);
		}

		~CTRunDelegateOperations ()
		{
			Dispose (false);
		}

		/// <summary>Releases the resources used by the CTRunDelegateOperations object.</summary>
		///         <remarks>
		///           <para>The Dispose method releases the resources used by the CTRunDelegateOperations class.</para>
		///           <para>Calling the Dispose method when the application is finished using the CTRunDelegateOperations ensures that all external resources used by this managed object are released as soon as possible.  Once developers have invoked the Dispose method, the object is no longer useful and developers should no longer make any calls to it.  For more information on releasing resources see ``Cleaning up Unmananaged Resources'' at https://msdn.microsoft.com/en-us/library/498928w2.aspx</para>
		///         </remarks>
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		/// <include file="../../docs/api/CoreText/CTRunDelegateOperations.xml" path="/Documentation/Docs[@DocId='M:CoreText.CTRunDelegateOperations.Dispose(System.Boolean)']/*" />
		protected virtual void Dispose (bool disposing)
		{
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public virtual nfloat GetAscent ()
		{
			return 0;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public virtual nfloat GetDescent ()
		{
			return 0;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public virtual nfloat GetWidth ()
		{
			return 0;
		}

		CTRunDelegateCallbacks? callbacks; // prevent GC since they are called from native code
		internal CTRunDelegateCallbacks GetCallbacks ()
		{
			if (!callbacks.HasValue) {
				unsafe {
					callbacks = new CTRunDelegateCallbacks () {
						version = 1,
						dealloc = &Deallocate,
						getAscent = &GetAscent,
						getDescent = &GetDescent,
						getWidth = &GetWidth,
					};
				}
			}
			return callbacks.Value;
		}

		[UnmanagedCallersOnly]
		static void Deallocate (IntPtr refCon)
		{
			var self = GetOperations (refCon);
			if (self is null)
				return;

			self.Dispose ();

			if (self.handle.IsAllocated)
				self.handle.Free ();
			self.handle = new GCHandle ();
		}

		internal static CTRunDelegateOperations? GetOperations (IntPtr refCon)
		{
			GCHandle c = GCHandle.FromIntPtr (refCon);

			return c.Target as CTRunDelegateOperations;
		}

		[UnmanagedCallersOnly]
		static nfloat GetAscent (IntPtr refCon)
		{
			var self = GetOperations (refCon);
			if (self is null)
				return 0;
			return (nfloat) self.GetAscent ();
		}

		[UnmanagedCallersOnly]
		static nfloat GetDescent (IntPtr refCon)
		{
			var self = GetOperations (refCon);
			if (self is null)
				return 0;
			return (nfloat) self.GetDescent ();
		}

		[UnmanagedCallersOnly]
		static nfloat GetWidth (IntPtr refCon)
		{
			var self = GetOperations (refCon);
			if (self is null)
				return 0;
			return (nfloat) self.GetWidth ();
		}
	}

	/// <summary>A delegate object that can be used to handle <see cref="CoreText.CTRunDelegateOperations" /> on a <see cref="CoreText.CTRun" />.</summary>
	///     <remarks>To be added.</remarks>
	///     <altmember cref="CoreText.CTStringAttributes.RunDelegate" />
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CTRunDelegate : NativeObject, IDisposable {
		[Preserve (Conditional = true)]
		internal CTRunDelegate (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		#region RunDelegate Creation
		[DllImport (Constants.CoreTextLibrary)]
		unsafe static extern IntPtr CTRunDelegateCreate (CTRunDelegateCallbacks* callbacks, IntPtr refCon);

		static IntPtr Create (CTRunDelegateOperations operations)
		{
			if (operations is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (operations));

			CTRunDelegateCallbacks callbacks = operations.GetCallbacks ();
			unsafe {
				return CTRunDelegateCreate (&callbacks, operations.Handle);
			}
		}

		/// <param name="operations">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CTRunDelegate (CTRunDelegateOperations operations)
			: base (Create (operations), true)
		{
		}
		#endregion

		#region Run Delegate Access
		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTRunDelegateGetRefCon (IntPtr runDelegate);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CTRunDelegateOperations? Operations {
			get {
				return CTRunDelegateOperations.GetOperations (CTRunDelegateGetRefCon (Handle));
			}
		}
		#endregion
	}
}
