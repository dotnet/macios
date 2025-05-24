// 
// ABGroup.cs: Implements the managed ABGroup
//
// Authors: Mono Team
//          Marek Safar (marek.safar@gmail.com)
//     
// Copyright (C) 2009 Novell, Inc
// Copyright (C) 2012 Xamarin Inc.
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
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace AddressBook {
	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	static class ABGroupProperty {

		public static int Name { get; private set; }

		static ABGroupProperty ()
		{
			InitConstants.Init ();
		}

		internal static void Init ()
		{
			Name = Dlfcn.GetInt32 (Libraries.AddressBook.Handle, "kABGroupNameProperty");
		}
	}

	/// <summary>
	///       A grouping of <see cref="AddressBook.ABPerson" /> and
	///       other <see cref="AddressBook.ABGroup" /> records.
	///     </summary>
	///     <remarks>
	///       <para>
	///         <c>ABGroup</c> supports:
	///       </para>
	///       <list type="bullet">
	///         <item>
	///           <term>
	///             Creating groups:
	///             <see cref="AddressBook.ABGroup" />.
	///           </term>
	///         </item>
	///         <item>
	///           <term>
	///             Managing group members:
	///             <see cref="AddressBook.ABGroup.Add(AddressBook.ABRecord)" />,
	///             <see cref="AddressBook.ABGroup.Remove(AddressBook.ABRecord)" />,
	///             <see cref="AddressBook.ABGroup.GetEnumerator" />,
	///             <see cref="AddressBook.ABGroup.GetMembers(AddressBook.ABPersonSortBy)" />.
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
	public class ABGroup : ABRecord, IEnumerable<ABRecord> {

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABGroupCreate ();

		/// <summary>
		///           Constructs and initializes a
		///           <see cref="AddressBook.ABGroup" /> instance.
		///         </summary>
		///         <remarks>To be added.</remarks>
		public ABGroup ()
			: base (ABGroupCreate (), true)
		{
			InitConstants.Init ();
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABGroupCreateInSource (IntPtr source);

		/// <param name="source">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public ABGroup (ABRecord source)
			: base (IntPtr.Zero, true)
		{
			if (source is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (source));

			Handle = ABGroupCreateInSource (source.Handle);
			GC.KeepAlive (source);
		}

		[Preserve (Conditional = true)]
		internal ABGroup (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		internal ABGroup (NativeHandle handle, ABAddressBook addressbook)
			: base (handle, false)
		{
			AddressBook = addressbook;
		}

		/// <summary>
		///           The name of the group.
		///         </summary>
		///         <value>
		///           A <see cref="System.String" /> containing the name of the group.
		///         </value>
		///         <remarks>
		///         </remarks>
		public string? Name {
			get { return PropertyToString (ABGroupProperty.Name); }
			set { SetValue (ABGroupProperty.Name, value); }
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABGroupCopySource (IntPtr group);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public ABRecord? Source {
			get {
				var h = ABGroupCopySource (Handle);
				if (h == IntPtr.Zero)
					return null;

				return FromHandle (h, null);
			}
		}

		[DllImport (Constants.AddressBookLibrary)]
		unsafe extern static byte ABGroupAddMember (IntPtr group, IntPtr person, IntPtr* error);
		/// <param name="person">
		///           The <see cref="AddressBook.ABRecord" /> to add to the group.
		///         </param>
		///         <summary>
		///           Adds a <see cref="AddressBook.ABRecord" /> to the group.
		///         </summary>
		///         <remarks>
		///         </remarks>
		///         <exception cref="CoreFoundation.CFException">
		///           The record couldn't be added to group.
		///         </exception>
		public void Add (ABRecord person)
		{
			if (person is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (person));
			IntPtr error;
			unsafe {
				if (ABGroupAddMember (Handle, person.Handle, &error) == 0)
					throw CFException.FromCFError (error);
				GC.KeepAlive (person);
			}
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABGroupCopyArrayOfAllMembers (IntPtr group);

		/// <summary>
		///           Returns an enumerator that iterates through all members in the group.
		///         </summary>
		///         <returns>
		///           An <see cref="System.Collections.IEnumerator" />
		///           which will return all members in the group.
		///         </returns>
		///         <remarks>
		///         </remarks>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

		/// <summary>
		///           Returns an enumerator that iterates through all members in the group.
		///         </summary>
		///         <returns>
		///           An
		///           <see cref="System.Collections.Generic.IEnumerator{T}" /> of <see cref="AddressBook.ABRecord" />
		///           which will return all members in the group.
		///         </returns>
		///         <remarks>
		///         </remarks>
		public IEnumerator<ABRecord> GetEnumerator ()
		{
			var cfArrayRef = ABGroupCopyArrayOfAllMembers (Handle);
			IEnumerable<ABRecord>? e = null;
			if (cfArrayRef == IntPtr.Zero)
				e = new ABRecord [0];
			else
				e = NSArray.ArrayFromHandle (cfArrayRef, h => ABRecord.FromHandle (h, AddressBook));
			return e.GetEnumerator ();
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABGroupCopyArrayOfAllMembersWithSortOrdering (IntPtr group, ABPersonSortBy sortOrdering);

		/// <param name="sortOrdering">
		///           A <see cref="AddressBook.ABPersonSortBy" /> which
		///           specifies the odering of members in the returned array.
		///         </param>
		///         <summary>
		///           Returns the group members sorted by the specified
		///           <paramref name="sortOrdering" />.
		///         </summary>
		///         <returns>
		///           A <see cref="AddressBook.ABRecord" /> array
		///           containing the members of the group sorted by the
		///           specified <paramref name="sortOrdering" />.
		///         </returns>
		///         <remarks>
		///         </remarks>
		public ABRecord [] GetMembers (ABPersonSortBy sortOrdering)
		{
			var cfArrayRef = ABGroupCopyArrayOfAllMembersWithSortOrdering (Handle, sortOrdering);
			if (cfArrayRef == IntPtr.Zero)
				return new ABRecord [0];
			return NSArray.ArrayFromHandle (cfArrayRef, h => ABRecord.FromHandle (h, AddressBook));
		}

		[DllImport (Constants.AddressBookLibrary)]
		unsafe extern static byte ABGroupRemoveMember (IntPtr group, IntPtr member, IntPtr* error);
		/// <param name="member">
		///           A <see cref="AddressBook.ABRecord" /> containing
		///           the record to remove from the group.
		///         </param>
		///         <summary>
		///           Removes <paramref name="member" /> from the group.
		///         </summary>
		///         <remarks>
		///         </remarks>
		///         <exception cref="CoreFoundation.CFException">
		///           The record couldn't be remove from the group.
		///         </exception>
		public void Remove (ABRecord member)
		{
			if (member is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (member));
			IntPtr error;
			unsafe {
				if (ABGroupRemoveMember (Handle, member.Handle, &error) == 0)
					throw CFException.FromCFError (error);
				GC.KeepAlive (member);
			}
		}
	}
}

#endif // !MONOMAC
