// 
// CFException.cs: Convert CFError into an CFException
//
// Authors: Mono Team
//     
// Copyright (C) 2009 Novell, Inc
// Copyright 2012 Xamarin Inc.
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

namespace CoreFoundation {
	/// <summary>A class whose static fields define error domains for <see cref="CoreFoundation.CFException.Domain" />.</summary>
	public static partial class CFErrorDomain {

#if !XAMCORE_5_0
		/// <summary>Identifies errors reported by Cocoa APIs.</summary>
		public static readonly NSString? Cocoa = _Cocoa;

		/// <summary>Identifies errors reported by Mach APIs.</summary>
		public static readonly NSString? Mach = _Mach;

		/// <summary>Identifies errors reported using OSStatus values.</summary>
		public static readonly NSString? OSStatus = _OSStatus;

		/// <summary>Identifies errors reported using POSIX error codes.</summary>
		public static readonly NSString? Posix = _Posix;
#endif
	}

	/// <summary>Class that contains keys that identify exception data values.</summary>
	///     <!-- TODO: Unused? Couldn't find any references 2013-05-02 -->
	public static partial class CFExceptionDataKey {

#if !XAMCORE_5_0
		/// <summary>Gets the key for an error's nonlocalized description.</summary>
		public static readonly NSString? Description = _Description;

		/// <summary>Gets the key for an error's localized description.</summary>
		public static readonly NSString? LocalizedDescription = _LocalizedDescription;

		/// <summary>Gets the key for an error's localized failure reason.</summary>
		public static readonly NSString? LocalizedFailureReason = _LocalizedFailureReason;

		/// <summary>Gets the key for an error's localized recovery suggestion.</summary>
		public static readonly NSString? LocalizedRecoverySuggestion = _LocalizedRecoverySuggestion;

		/// <summary>Gets the key for an underlying error.</summary>
		public static readonly NSString? UnderlyingError = _UnderlyingError;
#endif
	}

	/// <summary>Represents an exception arising from a Core Foundation <c>CFError</c>, having an error domain, a domain-specific error code, and perhaps additional information.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CFException : Exception {

		public CFException (string? description, NSString? domain, nint code, string? failureReason, string? recoverySuggestion)
			: base (description)
		{
			Code = code;
			Domain = domain;
			FailureReason = failureReason;
			RecoverySuggestion = recoverySuggestion;
		}

		/// <param name="cfErrorHandle">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CFException FromCFError (IntPtr cfErrorHandle)
		{
			return FromCFError (cfErrorHandle, true);
		}

		/// <param name="cfErrorHandle">To be added.</param>
		///         <param name="release">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CFException FromCFError (IntPtr cfErrorHandle, bool release)
		{
			if (cfErrorHandle == IntPtr.Zero)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (cfErrorHandle));

			var e = new CFException (
					CFString.FromHandle (CFErrorCopyDescription (cfErrorHandle), releaseHandle: true),
					Runtime.GetNSObject (CFErrorGetDomain (cfErrorHandle)) as NSString,
					CFErrorGetCode (cfErrorHandle),
					CFString.FromHandle (CFErrorCopyFailureReason (cfErrorHandle), releaseHandle: true),
					CFString.FromHandle (CFErrorCopyRecoverySuggestion (cfErrorHandle), releaseHandle: true));

			var cfUserInfo = CFErrorCopyUserInfo (cfErrorHandle);
			if (cfUserInfo != IntPtr.Zero) {
				using (var userInfo = new NSDictionary (cfUserInfo)) {
					foreach (var i in userInfo) {
						if (i.Key is not null)
							e.Data.Add (i.Key.ToString () ?? "", i.Value?.ToString () ?? "");
					}
				}
			}
			if (release)
				CFObject.CFRelease (cfErrorHandle);
			return e;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nint Code { get; private set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		///         <altmember cref="CoreFoundation.CFErrorDomain" />
		public NSString? Domain { get; private set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? FailureReason { get; private set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? RecoverySuggestion { get; private set; }

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern IntPtr CFErrorCopyDescription (IntPtr err);

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern IntPtr CFErrorCopyFailureReason (IntPtr err);

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern IntPtr CFErrorCopyRecoverySuggestion (IntPtr err);

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern IntPtr CFErrorCopyUserInfo (IntPtr err);

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern nint CFErrorGetCode (IntPtr err);

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern IntPtr CFErrorGetDomain (IntPtr err);
	}
}
