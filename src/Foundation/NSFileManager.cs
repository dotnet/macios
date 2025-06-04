//
// NSFileManager.cs:
// Author:
//   Miguel de Icaza
//
// Copyright 2011, Novell, Inc.
// Copyright 2011 - 2014 Xamarin Inc
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
//
using CoreFoundation;
using ObjCRuntime;

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

#nullable enable

namespace Foundation {

	// This is a convenience enum around a set of native strings.
	/// <summary>File kind enumeration.</summary>
	///     <remarks>To be added.</remarks>
	public enum NSFileType {
		/// <summary>A directory</summary>
		Directory,
		/// <summary>A regular file.</summary>
		Regular,
		/// <summary>A symbolic link.</summary>
		SymbolicLink,
		/// <summary>A Unix file system socket.</summary>
		Socket,
		/// <summary>A special system character device.</summary>
		CharacterSpecial,
		/// <summary>To be added.</summary>
		BlockSpecial,
		/// <summary>Unknown file type.</summary>
		Unknown,
	}

#if !MONOMAC
	/// <summary>Enumerates file protection levels.</summary>
	///     <remarks>To be added.</remarks>
	public enum NSFileProtection {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		Complete,
		/// <summary>To be added.</summary>
		CompleteUnlessOpen,
		/// <summary>To be added.</summary>
		CompleteUntilFirstUserAuthentication,
	}
#endif

	/// <summary>Encapsulates file attributes for use with <see cref="Foundation.NSFileManager" />.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class NSFileAttributes {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? AppendOnly { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? Busy { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? ExtensionHidden { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSDate? CreationDate { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? OwnerAccountName { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? GroupOwnerAccountName { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nint? SystemNumber { get; set; } // NSInteger
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nuint? DeviceIdentifier { get; set; } // unsigned long
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nuint? GroupOwnerAccountID { get; set; } // unsigned long

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? Immutable { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSDate? ModificationDate { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nuint? OwnerAccountID { get; set; } // unsigned long
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nuint? HfsCreatorCode { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nuint? HfsTypeCode { get; set; } // unsigned long

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public short? PosixPermissions { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nuint? ReferenceCount { get; set; } // unsigned long
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nuint? SystemFileNumber { get; set; } // unsigned long
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public ulong? Size { get; set; } // unsigned long long
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSFileType? Type { get; set; }

#if !MONOMAC
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSFileProtection? ProtectionKey { get; set; }
#endif

		internal NSDictionary ToDictionary ()
		{
			NSFileType? type;
			NSString? v = null;
			var dict = new NSMutableDictionary ();
			if (AppendOnly.HasValue)
				dict.SetObject (NSNumber.FromBoolean (AppendOnly.Value), NSFileManager.AppendOnly);
			if (Busy.HasValue)
				dict.SetObject (NSNumber.FromBoolean (Busy.Value), NSFileManager.Busy);
			if (ExtensionHidden.HasValue)
				dict.SetObject (NSNumber.FromBoolean (ExtensionHidden.Value), NSFileManager.ExtensionHidden);
			if (CreationDate is not null)
				dict.SetObject (CreationDate, NSFileManager.CreationDate);
			if (OwnerAccountName is not null)
				dict.SetObject (new NSString (OwnerAccountName), NSFileManager.OwnerAccountName);
			if (GroupOwnerAccountName is not null)
				dict.SetObject (new NSString (GroupOwnerAccountName), NSFileManager.GroupOwnerAccountName);
			if (SystemNumber.HasValue)
				dict.SetObject (NSNumber.FromLong (SystemNumber.Value), NSFileManager.SystemNumber);
			if (DeviceIdentifier.HasValue)
				dict.SetObject (NSNumber.FromUnsignedLong (DeviceIdentifier.Value), NSFileManager.DeviceIdentifier);
			if (GroupOwnerAccountID.HasValue)
				dict.SetObject (NSNumber.FromUnsignedLong (GroupOwnerAccountID.Value), NSFileManager.GroupOwnerAccountID);
			if (Immutable.HasValue)
				dict.SetObject (NSNumber.FromBoolean (Immutable.Value), NSFileManager.Immutable);
			if (ModificationDate is not null)
				dict.SetObject (ModificationDate, NSFileManager.ModificationDate);
			if (OwnerAccountID.HasValue)
				dict.SetObject (NSNumber.FromUnsignedLong (OwnerAccountID.Value), NSFileManager.OwnerAccountID);
			if (HfsCreatorCode.HasValue)
				dict.SetObject (NSNumber.FromUnsignedLong (HfsCreatorCode.Value), NSFileManager.HfsCreatorCode);
			if (HfsTypeCode.HasValue)
				dict.SetObject (NSNumber.FromUnsignedLong (HfsTypeCode.Value), NSFileManager.HfsTypeCode);
			if (PosixPermissions.HasValue)
				dict.SetObject (NSNumber.FromInt16 ((short) PosixPermissions.Value), NSFileManager.PosixPermissions);
			if (ReferenceCount.HasValue)
				dict.SetObject (NSNumber.FromUnsignedLong (ReferenceCount.Value), NSFileManager.ReferenceCount);
			if (SystemFileNumber.HasValue)
				dict.SetObject (NSNumber.FromUnsignedLong (SystemFileNumber.Value), NSFileManager.SystemFileNumber);
			if (Size.HasValue)
				dict.SetObject (NSNumber.FromUInt64 (Size.Value), NSFileManager.Size);

			type = Type;

			if (type.HasValue) {
				v = null;
				switch (type.Value) {
				case NSFileType.Directory:
					v = NSFileManager.TypeDirectory; break;
				case NSFileType.Regular:
					v = NSFileManager.TypeRegular; break;
				case NSFileType.SymbolicLink:
					v = NSFileManager.TypeSymbolicLink; break;
				case NSFileType.Socket:
					v = NSFileManager.TypeSocket; break;
				case NSFileType.CharacterSpecial:
					v = NSFileManager.TypeCharacterSpecial; break;
				case NSFileType.BlockSpecial:
					v = NSFileManager.TypeBlockSpecial; break;
				default:
					v = NSFileManager.TypeUnknown; break;
				}
				dict.SetObject (v, NSFileManager.NSFileType);
			}

#if !MONOMAC
			if (ProtectionKey.HasValue) {
				v = null;
				switch (ProtectionKey.Value) {
				case NSFileProtection.None:
					v = NSFileManager.FileProtectionNone; break;
				case NSFileProtection.Complete:
					v = NSFileManager.FileProtectionComplete; break;
				case NSFileProtection.CompleteUnlessOpen:
					v = NSFileManager.FileProtectionCompleteUnlessOpen; break;
				case NSFileProtection.CompleteUntilFirstUserAuthentication:
					v = NSFileManager.FileProtectionCompleteUntilFirstUserAuthentication; break;
				}
				if (v is not null)
					dict.SetObject (v, NSFileManager.FileProtectionKey);
			}
#endif
			return dict;
		}

		#region fetch
		internal static bool? fetch_bool (NSDictionary dict, NSString key)
		{
			var k = dict.ObjectForKey (key) as NSNumber;
			if (k is null)
				return null;
			return k.BoolValue;
		}

		internal static uint? fetch_uint (NSDictionary dict, NSString key)
		{
			var k = dict.ObjectForKey (key) as NSNumber;
			if (k is null)
				return null;
			return k.UInt32Value;
		}

		internal static nuint? fetch_nuint (NSDictionary dict, NSString key)
		{
			var k = dict.ObjectForKey (key) as NSNumber;
			if (k is null)
				return null;
			return k.UnsignedLongValue;
		}

		internal static nint? fetch_nint (NSDictionary dict, NSString key)
		{
			var k = dict.ObjectForKey (key) as NSNumber;
			if (k is null)
				return null;
			return k.LongValue;
		}

		internal static ulong? fetch_ulong (NSDictionary dict, NSString key)
		{
			var k = dict.ObjectForKey (key) as NSNumber;
			if (k is null)
				return null;
			return k.UInt64Value;
		}

		internal static long? fetch_long (NSDictionary dict, NSString key)
		{
			var k = dict.ObjectForKey (key) as NSNumber;
			if (k is null)
				return null;
			return k.Int64Value;
		}

		internal static short? fetch_short (NSDictionary dict, NSString key)
		{
			var k = dict.ObjectForKey (key) as NSNumber;
			if (k is null)
				return null;
			return k.Int16Value;
		}
		#endregion

		/// <param name="dict">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSFileAttributes? FromDictionary (NSDictionary dict)
		{
			if (dict is null)
				return null;
			var ret = new NSFileAttributes ();

			ret.AppendOnly = fetch_bool (dict, NSFileManager.AppendOnly);
			ret.Busy = fetch_bool (dict, NSFileManager.Busy);
			ret.ExtensionHidden = fetch_bool (dict, NSFileManager.ExtensionHidden);
			ret.CreationDate = dict.ObjectForKey (NSFileManager.CreationDate) as NSDate;
			ret.OwnerAccountName = dict.ObjectForKey (NSFileManager.OwnerAccountName) as NSString;
			ret.GroupOwnerAccountName = dict.ObjectForKey (NSFileManager.GroupOwnerAccountName) as NSString;
			ret.SystemNumber = fetch_nint (dict, NSFileManager.SystemNumber);
			ret.DeviceIdentifier = fetch_nuint (dict, NSFileManager.DeviceIdentifier);
			ret.GroupOwnerAccountID = fetch_nuint (dict, NSFileManager.GroupOwnerAccountID);
			ret.Immutable = fetch_bool (dict, NSFileManager.Immutable);
			ret.ModificationDate = dict.ObjectForKey (NSFileManager.ModificationDate) as NSDate;
			ret.OwnerAccountID = fetch_nuint (dict, NSFileManager.OwnerAccountID);
			ret.HfsCreatorCode = fetch_nuint (dict, NSFileManager.HfsCreatorCode);
			ret.HfsTypeCode = fetch_nuint (dict, NSFileManager.HfsTypeCode);
			ret.PosixPermissions = fetch_short (dict, NSFileManager.PosixPermissions);
			ret.ReferenceCount = fetch_nuint (dict, NSFileManager.ReferenceCount);
			ret.SystemFileNumber = fetch_nuint (dict, NSFileManager.SystemFileNumber);
			ret.Size = fetch_ulong (dict, NSFileManager.Size);

			var name = dict.ObjectForKey (NSFileManager.NSFileType) as NSString;
			if (name is not null) {
				NSFileType? type = null;

				if (name == NSFileManager.TypeDirectory)
					type = NSFileType.Directory;
				else if (name == NSFileManager.TypeRegular)
					type = NSFileType.Regular;
				else if (name == NSFileManager.TypeSymbolicLink)
					type = NSFileType.SymbolicLink;
				else if (name == NSFileManager.TypeSocket)
					type = NSFileType.Socket;
				else if (name == NSFileManager.TypeCharacterSpecial)
					type = NSFileType.CharacterSpecial;
				else if (name == NSFileManager.TypeBlockSpecial)
					type = NSFileType.BlockSpecial;
				else if (name == NSFileManager.TypeUnknown)
					type = NSFileType.Unknown;

				ret.Type = type;
			}

#if !MONOMAC
			name = dict.ObjectForKey (NSFileManager.FileProtectionKey) as NSString;
			if (name is not null) {
				NSFileProtection? protection = null;

				if (name == NSFileManager.FileProtectionNone)
					protection = NSFileProtection.None;
				else if (name == NSFileManager.FileProtectionComplete)
					protection = NSFileProtection.Complete;
				else if (name == NSFileManager.FileProtectionCompleteUnlessOpen)
					protection = NSFileProtection.CompleteUnlessOpen;
				else if (name == NSFileManager.FileProtectionCompleteUntilFirstUserAuthentication)
					protection = NSFileProtection.CompleteUntilFirstUserAuthentication;

				ret.ProtectionKey = protection;
			}
#endif
			return ret;
		}
	}

	/// <summary>File system attributes (size, blocks and free information).</summary>
	///     <remarks>This is a strong wrapper around the underlying NSDictionary returned by NSFileSystem APIs.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class NSFileSystemAttributes {
		NSDictionary dict;

		internal NSFileSystemAttributes (NSDictionary dict)
		{
			this.dict = dict;
		}

		// The documentation only says these are NSNumbers, it doesn't say which type of number.
		/// <summary>The volume size in bytes.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public ulong Size { get; internal set; }
		/// <summary>The number of free bytes in the volume.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public ulong FreeSize { get; internal set; }
		/// <summary>The total number of nodes in the volume (maximum number of files).</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public long Nodes { get; internal set; }
		/// <summary>The number of free nodes in the volume.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public long FreeNodes { get; internal set; }
		// "The value corresponds to the value of st_dev, as returned by stat(2)" => st_dev is defined to be int32_t in all architectures.
		/// <summary>The device number (corresponds to st_dev on the Unix stat structure).</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public uint Number { get; internal set; }

		internal static NSFileSystemAttributes? FromDictionary (NSDictionary dict)
		{
			if (dict is null)
				return null;
			var ret = new NSFileSystemAttributes (dict);
			ret.Size = NSFileAttributes.fetch_ulong (dict, NSFileManager.SystemSize) ?? 0;
			ret.FreeSize = NSFileAttributes.fetch_ulong (dict, NSFileManager.SystemFreeSize) ?? 0;
			ret.Nodes = NSFileAttributes.fetch_long (dict, NSFileManager.SystemNodes) ?? 0;
			ret.FreeNodes = NSFileAttributes.fetch_long (dict, NSFileManager.SystemFreeNodes) ?? 0;
			ret.Number = NSFileAttributes.fetch_uint (dict, NSFileManager.SystemFreeNodes) ?? 0;

			return ret;
		}

		// For source code compatibility with users that had done manual NSDictionary lookups before
		public static implicit operator NSDictionary (NSFileSystemAttributes attr)
		{
			return attr.dict;
		}

	}

	public partial class NSFileManager {

		[DllImport (Constants.FoundationLibrary)]
		static extern IntPtr NSUserName ();

		public static string? UserName {
			get {
				return CFString.FromHandle (NSUserName ());
			}
		}

		[DllImport (Constants.FoundationLibrary)]
		static extern IntPtr NSFullUserName ();

		public static string? FullUserName {
			get {
				return CFString.FromHandle (NSFullUserName ());
			}
		}

		[DllImport (Constants.FoundationLibrary)]
		static extern IntPtr NSHomeDirectory ();

		public static string? HomeDirectory {
			get {
				return CFString.FromHandle (NSHomeDirectory ());
			}
		}

		[DllImport (Constants.FoundationLibrary)]
		static extern IntPtr NSHomeDirectoryForUser (/* NSString */IntPtr userName);

		public static string? GetHomeDirectory (string userName)
		{
			if (userName is null)
				throw new ArgumentNullException (nameof (userName));

			var userNamePtr = CFString.CreateNative (userName);
			var rv = CFString.FromHandle (NSHomeDirectoryForUser (userNamePtr));
			CFString.ReleaseNative (userNamePtr);
			return rv;
		}

		[DllImport (Constants.FoundationLibrary)]
		static extern IntPtr NSTemporaryDirectory ();

		public static string? TemporaryDirectory {
			get {
				return CFString.FromHandle (NSTemporaryDirectory ());
			}
		}

		/// <param name="attributes">To be added.</param>
		///         <param name="path">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool SetAttributes (NSFileAttributes attributes, string path, out NSError error)
		{
			if (attributes is null)
				throw new ArgumentNullException (nameof (attributes));
			return SetAttributes (attributes.ToDictionary (), path, out error);
		}

		/// <param name="attributes">To be added.</param>
		///         <param name="path">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool SetAttributes (NSFileAttributes attributes, string path)
		{
			if (attributes is null)
				throw new ArgumentNullException (nameof (attributes));

			return SetAttributes (attributes.ToDictionary (), path, out _);
		}

		/// <param name="path">To be added.</param>
		///         <param name="createIntermediates">To be added.</param>
		///         <param name="attributes">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool CreateDirectory (string path, bool createIntermediates, NSFileAttributes? attributes, out NSError error)
		{
			return CreateDirectory (path, createIntermediates, attributes?.ToDictionary (), out error);
		}

		/// <param name="path">To be added.</param>
		///         <param name="createIntermediates">To be added.</param>
		///         <param name="attributes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool CreateDirectory (string path, bool createIntermediates, NSFileAttributes? attributes)
		{
			return CreateDirectory (path, createIntermediates, attributes?.ToDictionary (), out var _);
		}

		/// <param name="path">To be added.</param>
		///         <param name="data">To be added.</param>
		///         <param name="attributes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool CreateFile (string path, NSData data, NSFileAttributes? attributes)
		{
			return CreateFile (path, data, attributes?.ToDictionary ());
		}

		/// <param name="path">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSFileAttributes? GetAttributes (string path, out NSError error)
		{
			return NSFileAttributes.FromDictionary (_GetAttributes (path, out error));
		}

		/// <param name="path">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSFileAttributes? GetAttributes (string path)
		{
			return NSFileAttributes.FromDictionary (_GetAttributes (path, out var _));
		}

		/// <param name="path">Path to the any file in the volume to probe for information.</param>
		///         <summary>Returns the file system attributes for a given volume.</summary>
		///         <returns>A NSFileSystemAttributes object that contains the file system properties or null on error.</returns>
		///         <remarks>This function returns the file system information associated with the specified path.   The path is any path name that is contained in a volume.</remarks>
		public NSFileSystemAttributes? GetFileSystemAttributes (string path)
		{
			return NSFileSystemAttributes.FromDictionary (_GetFileSystemAttributes (path, out var _));
		}

		/// <param name="path">Path to the any file in the volume to probe for information.</param>
		///         <param name="error">Error object, to return any error conditions.</param>
		///         <summary>Returns the file system attributes for a given volume.</summary>
		///         <returns>A NSFileSystemAttributes object that contains the file system properties, or null on error.</returns>
		///         <remarks>This function returns the file system information associated with the specified path.   The path is any path name that is contained in a volume.</remarks>
		public NSFileSystemAttributes? GetFileSystemAttributes (string path, out NSError error)
		{
			return NSFileSystemAttributes.FromDictionary (_GetFileSystemAttributes (path, out error));
		}

		/// <param name="properties">To be added.</param>
		///         <param name="options">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSUrl [] GetMountedVolumes (NSString [] properties, NSVolumeEnumerationOptions options)
		{
			using var array = NSArray.FromNSObjects (properties);
			return GetMountedVolumes (array, options);
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string CurrentDirectory {
			get { return GetCurrentDirectory (); }
			// ignore boolean return value
			set { ChangeCurrentDirectory (value); }
		}

		/// <param name="filename">Path of file</param>
		///         <param name="skipBackup">True if you want to flag this file to be skipped for backups or false if you want to have this file backed up to iCloud.</param>
		///         <summary>To be added.</summary>
		///         <returns>A null return value will indicate success, while a non-null error will contain an instance of NSError detailing the problem</returns>
		///         <remarks>If you set the SkipBackup attribute on a file, it will inform the operating system that this file should not be backed up into iCloud.   
		///         This high-level API automagically adjust itself based on the version of iOS being executed. 
		///         On iOS 5.0.1 (only) it will use the old setxattr API to set (or remove) the "com.apple.MobileBackup" attribute. 
		///         On iOS 5.1 (and later) it will use NSUrlIsExcludedFromBackupKey to accomplish the same.</remarks>
		public static NSError SetSkipBackupAttribute (string filename, bool skipBackup)
		{
			if (filename is null)
				throw new ArgumentNullException (nameof (filename));

			using (var url = NSUrl.FromFilename (filename)) {
				url.SetResource (NSUrl.IsExcludedFromBackupKey, (NSNumber) (skipBackup ? 1 : 0), out var error);
				return error;
			}
		}

		/// <param name="filename">Path of the file to probe.</param>
		///         <summary>Returns the status of the SkipBackup to iCloud attribute is set on the file.</summary>
		///         <returns>true if the extended attribute is set.</returns>
		///         <remarks>This returns true if the file is marked not to be backed up by iCloud, otherwise it will return false.
		///         This high-level API automagically adjust itself based on the version of iOS being executed. 
		///         On iOS 5.0.1 (only) it will use the old getxattr API to get the value of the "com.apple.MobileBackup" attribute. 
		///         On iOS 5.1 (and later) it will use NSUrlIsExcludedFromBackupKey to accomplish the same.</remarks>
		public static bool GetSkipBackupAttribute (string filename)
		{
			return GetSkipBackupAttribute (filename, out var _);
		}

		/// <param name="filename">Path of the file to probe.</param>
		///         <param name="error">The error will be set to null if there was no error, or it will point to an instance of NSError if there was a problem.</param>
		///         <summary>Returns the status of the SkipBackup to iCloud attribute is set on the file.</summary>
		///         <returns>true if the extended attribute is set.</returns>
		///         <remarks>This returns true if the file is marked not to be backed up by iCloud, otherwise it will return false.
		///         This high-level API automagically adjust itself based on the version of iOS being executed. 
		///         On iOS 5.0.1 (only) it will use the old getxattr API to get the value of the "com.apple.MobileBackup" attribute. 
		///         On iOS 5.1 (and later) it will use NSUrlIsExcludedFromBackupKey to accomplish the same.</remarks>
		public static bool GetSkipBackupAttribute (string filename, out NSError error)
		{
			if (filename is null)
				throw new ArgumentNullException (nameof (filename));

			using (var url = NSUrl.FromFilename (filename)) {
				url.TryGetResource (NSUrl.IsExcludedFromBackupKey, out var value, out error);
				return (error is null) && ((long) ((NSNumber) value) == 1);
			}
		}
	}
}
