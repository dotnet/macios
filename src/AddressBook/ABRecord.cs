// 
// ABRecord.cs: Implements the managed ABRecord
//
// Authors: Mono Team
//     
// Copyright (C) 2009 Novell, Inc
// Copyright 2011, 2012 Xamarin Inc.
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

#nullable enable

#if !MONOMAC

using System;
using System.Runtime.InteropServices;

using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace AddressBook {

	/// <summary>
	///       Base type for
	///       <see cref="AddressBook.ABGroup" /> and
	///       <see cref="AddressBook.ABPerson" />.
	///     </summary>
	///     <remarks>
	///       <para>
	///         Supported operations:
	///       </para>
	///       <list type="bullet">
	///         <item>
	///           <term>
	///             Getting record information:
	///             <see cref="AddressBook.ABRecord.Id" />,
	///             <see cref="AddressBook.ABRecord.Type" />.
	///           </term>
	///         </item>
	///       </list>
	///     </remarks>
	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	public class ABRecord : NativeObject {

		/// <summary>
		///           An invalid value for a record id.
		///         </summary>
		///         <remarks>
		///           <para>
		///             <see cref="AddressBook.ABRecord.Id" /> returns this
		///             value when the record hasn't been saved to the database.
		///           </para>
		///         </remarks>
		public const int InvalidRecordId = -1;
		/// <summary>
		///           An invalid value for a property id.
		///         </summary>
		///         <remarks>
		///         </remarks>
		public const int InvalidPropertyId = -1;

		[Preserve (Conditional = true)]
		internal ABRecord (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <param name="handle">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static ABRecord? FromHandle (IntPtr handle)
		{
			return FromHandle (handle, false);
		}

		internal static ABRecord? FromHandle (IntPtr handle, bool owns)
		{
			if (handle == IntPtr.Zero)
				return null;
			return FromHandle (handle, null, owns);
		}

		internal static ABRecord FromHandle (IntPtr handle, ABAddressBook? addressbook, bool owns = true)
		{
			if (handle == IntPtr.Zero)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (handle));
			// TODO: does ABGroupCopyArrayOfAllMembers() have Create or Get
			// semantics for the array elements?
			var type = ABRecordGetRecordType (handle);
			ABRecord rec;

			switch (type) {
			case ABRecordType.Person:
				rec = new ABPerson (handle, owns);
				break;
			case ABRecordType.Group:
				rec = new ABGroup (handle, owns);
				break;
			case ABRecordType.Source:
				rec = new ABSource (handle, owns);
				break;
			default:
				throw new NotSupportedException ("Could not determine record type.");
			}

			rec.AddressBook = addressbook;
			return rec;
		}

		/// <include file="../../docs/api/AddressBook/ABRecord.xml" path="/Documentation/Docs[@DocId='M:AddressBook.ABRecord.Dispose(System.Boolean)']/*" />
		protected override void Dispose (bool disposing)
		{
			AddressBook = null;
			base.Dispose (disposing);
		}

		internal ABAddressBook? AddressBook {
			get; set;
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static int ABRecordGetRecordID (IntPtr record);
		/// <summary>Gets the unique ID of the record.</summary>
		///         <value>
		///           A <see cref="System.Int32" /> which is the unique ID of the record.
		///         </value>
		///         <remarks>
		///           <para>
		///             If the record hasn't been saved into the database, this is
		///             <see cref="AddressBook.ABRecord.InvalidRecordId" />.
		///           </para>
		///         </remarks>
		///         <altmember cref="AddressBook.ABAddressBook.Save" />
		public int Id {
			get { return ABRecordGetRecordID (Handle); }
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static ABRecordType ABRecordGetRecordType (IntPtr record);
		/// <summary>Gets the type of the record.</summary>
		///         <value>
		///           A <see cref="AddressBook.ABRecordType" /> containing
		///           the type of the record.
		///         </value>
		///         <remarks>
		///         </remarks>
		public ABRecordType Type {
			get { return ABRecordGetRecordType (Handle); }
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABRecordCopyCompositeName (IntPtr record);
		/// <summary>
		///           Returns the composite name of the <see cref="AddressBook.ABRecord" />.
		///         </summary>
		///         <returns>
		///           A <see cref="System.String" /> containing
		///           the composite name of the <see cref="AddressBook.ABRecord" />.
		///         </returns>
		///         <remarks>
		///         </remarks>
		public override string? ToString ()
		{
			return CFString.FromHandle (ABRecordCopyCompositeName (Handle));
		}

		// TODO: Should SetValue/CopyValue/RemoveValue be public?

		[DllImport (Constants.AddressBookLibrary)]
		unsafe extern static byte ABRecordSetValue (IntPtr record, int /* ABPropertyID = int32_t */ property, IntPtr value, IntPtr* error);
		internal void SetValue (int property, IntPtr value)
		{
			IntPtr error;
			unsafe {
				if (ABRecordSetValue (Handle, property, value, &error) == 0)
					throw CFException.FromCFError (error);
			}
		}

		internal void SetValue (int property, NSObject? value)
		{
			SetValue (property, value.GetHandle ());
			GC.KeepAlive (value);
		}

		internal void SetValue (int property, string? value)
		{
			var valueHandle = CFString.CreateNative (value);
			try {
				SetValue (property, valueHandle);
			} finally {
				CFString.ReleaseNative (valueHandle);
			}
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABRecordCopyValue (IntPtr record, int /* ABPropertyID = int32_t */ property);
		internal IntPtr CopyValue (int property)
		{
			return ABRecordCopyValue (Handle, property);
		}

		[DllImport (Constants.AddressBookLibrary)]
		unsafe extern static byte ABRecordRemoveValue (IntPtr record, int /* ABPropertyID = int32_t */ property, IntPtr* error);
		internal void RemoveValue (int property)
		{
			IntPtr error;
			bool r;
			unsafe {
				r = ABRecordRemoveValue (Handle, property, &error) != 0;
			}
			if (!r && error != IntPtr.Zero)
				throw CFException.FromCFError (error);
		}

		internal T? PropertyTo<T> (int id)
			where T : NSObject
		{
			IntPtr value = CopyValue (id);
			return (T?) Runtime.GetNSObject (value);
		}

		internal string? PropertyToString (int id)
		{
			return CFString.FromHandle (CopyValue (id));
		}
	}
}

#endif // !MONOMAC
