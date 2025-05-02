// 
// ABAddressBook.cs: Implements the managed ABAddressBook
//
// Authors: Mono Team
//     
// Copyright (C) 2009 Novell, Inc
// Copyright 2011-2013 Xamarin Inc.
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

using Foundation;
using CoreFoundation;
using ObjCRuntime;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace AddressBook {
	/// <summary>Provides data for the <see cref="AddressBook.ABAddressBook.ExternalChange" /> event.</summary>
	///     <remarks>
	///     </remarks>
	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	public class ExternalChangeEventArgs : EventArgs {
		/// <param name="addressBook">
		///           The <see cref="AddressBook.ABAddressBook" />
		///           instance raising the
		///           <see cref="AddressBook.ABAddressBook.ExternalChange" />
		///           event.
		///         </param>
		///         <param name="info">
		///           A <see cref="Foundation.NSDictionary" /> containing
		///           additional information about the event.
		///         </param>
		///         <summary>Initializes a new instance of the ExternalChangeEventArgs class.</summary>
		///         <remarks>
		///           This constructor initializes the
		///           <see cref="AddressBook.ExternalChangeEventArgs.AddressBook" />
		///           property of the new instance using with <paramref name="addressBook" />,
		///           and initializes the
		///           <see cref="AddressBook.ExternalChangeEventArgs.Info" />
		///           property of the new instance using <paramref name="info" />.
		///         </remarks>
		public ExternalChangeEventArgs (ABAddressBook addressBook, NSDictionary? info)
		{
			AddressBook = addressBook;
			Info = info;
		}

		/// <summary>
		///           The <see cref="AddressBook.ABAddressBook" />
		///           which raised the
		///           <see cref="AddressBook.ABAddressBook.ExternalChange" />
		///           event.
		///         </summary>
		///         <value>
		///           A <see cref="AddressBook.ABAddressBook" /> which raised the
		///           <see cref="AddressBook.ABAddressBook.ExternalChange" />
		///           event.
		///         </value>
		///         <remarks>
		///         </remarks>
		public ABAddressBook AddressBook { get; private set; }
		/// <summary>
		///           Additional informationa about the
		///           <see cref="AddressBook.ABAddressBook.ExternalChange" />
		///           event.
		///         </summary>
		///         <value>
		///           A <see cref="Foundation.NSDictionary" /> containing
		///           additional information about the
		///           <see cref="AddressBook.ABAddressBook.ExternalChange" />
		///           event.  This may be <see langword="null" />.
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSDictionary? Info { get; private set; }
	}

	// Quoth the docs: 
	//    http://developer.apple.com/iphone/library/documentation/AddressBook/Reference/ABPersonRef_iPhoneOS/Reference/reference.html#//apple_ref/c/func/ABPersonGetSortOrdering
	//
	//   "The value of these constants is undefined until one of the following has
	//    been called: ABAddressBookCreate, ABPersonCreate, ABGroupCreate."
	//
	// Meaning we can't rely on static constructors, as they could be invoked
	// before those functions have been invoked. :-/
	//
	// Note that the above comment was removed from iOS 6.0+ documentation (and were not part of OSX docs AFAIK).
	// It make sense since it's not possible to call those functions, from 6.0+ they will return NULL on devices,
	// unless the application has been authorized to access the address book.
	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	static class InitConstants {
		public static void Init () { }

		static InitConstants ()
		{
#if __MACCATALYST__
			// avoid TypeLoadException if used before macOS 11.x
			if (!SystemVersion.CheckiOS (14, 0))
				return;
#endif
			// ensure we can init. This is needed before iOS6 (as per doc).
			IntPtr p = ABAddressBook.ABAddressBookCreate ();

			ABGroupProperty.Init ();
			ABLabel.Init ();
			ABPersonAddressKey.Init ();
			ABPersonDateLabel.Init ();
			ABPersonInstantMessageKey.Init ();
			ABPersonInstantMessageService.Init ();
			ABPersonKindId.Init ();
			ABPersonPhoneLabel.Init ();
			ABPersonPropertyId.Init ();
			ABPersonRelatedNamesLabel.Init ();
			ABPersonUrlLabel.Init ();
			ABSourcePropertyId.Init ();

			// From iOS 6.0+ this might return NULL, e.g. if the application is not authorized to access the
			// address book, and we would crash if we tried to release a null pointer
			if (p != IntPtr.Zero)
				CFObject.CFRelease (p);
		}
	}

	/// <include file="../../docs/api/AddressBook/ABAddressBook.xml" path="/Documentation/Docs[@DocId='T:AddressBook.ABAddressBook']/*" />
	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	public class ABAddressBook : NativeObject, IEnumerable<ABRecord> {

		/// <summary>
		///           Identifies the error domain under which address book errors are grouped.
		///         </summary>
		///         <remarks>
		///           When an <see cref="CoreFoundation.CFException" /> is
		///           thrown from a <see cref="AddressBook.ABAddressBook" />
		///           method, the
		///           <see cref="CoreFoundation.CFException.Domain" /> property
		///           will be equal to <c>ErrorDomain</c>.
		///         </remarks>
		public static readonly NSString ErrorDomain;

		GCHandle sender;

		[DllImport (Constants.AddressBookLibrary)]
		internal extern static IntPtr ABAddressBookCreate ();

		/// <summary>Developers should not use this deprecated constructor. Developers should use the static Create method instead</summary>
		///         <remarks>
		///           <para>
		///             Changes made to the <c>ABAddressBook</c> instance are visible to
		///             other applications only after
		///             <see cref="AddressBook.ABAddressBook.Save" /> is called.
		///           </para>
		///         </remarks>
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("ios")]
		[ObsoletedOSPlatform ("ios", "Use the static Create method instead")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("maccatalyst", "Use the static Create method instead")]
		[UnsupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("tvos")]
		public ABAddressBook ()
			: this (ABAddressBookCreate (), true)
		{
		}

		[DllImport (Constants.AddressBookLibrary)]
		unsafe extern static IntPtr ABAddressBookCreateWithOptions (IntPtr dictionary, IntPtr* cfError);

		/// <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static ABAddressBook? Create (out NSError? error)
		{
			IntPtr e;
			IntPtr handle;
			unsafe {
				handle = ABAddressBookCreateWithOptions (IntPtr.Zero, &e);
			}
			if (handle == IntPtr.Zero) {
				error = Runtime.GetNSObject<NSError> (e);
				return null;
			}
			error = null;
			return new ABAddressBook (handle, true);
		}

		[Preserve (Conditional = true)]
		internal ABAddressBook (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
			InitConstants.Init ();
		}

		static ABAddressBook ()
		{
			ErrorDomain = Dlfcn.GetStringConstant (Libraries.AddressBook.Handle, "ABAddressBookErrorDomain")!;
		}

		/// <include file="../../docs/api/AddressBook/ABAddressBook.xml" path="/Documentation/Docs[@DocId='M:AddressBook.ABAddressBook.Dispose(System.Boolean)']/*" />
		protected override void Dispose (bool disposing)
		{
			if (sender.IsAllocated)
				sender.Free ();
			base.Dispose (disposing);
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static nint ABAddressBookGetAuthorizationStatus ();

		/// <summary>What permissions the user has allowed the app.</summary>
		///         <returns>The current authorization status of the app.</returns>
		///         <remarks>
		///           <para>This method checks the application's current authorization status, which can change due to the user interacting with the permissions dialog (see <see cref="AddressBook.ABAddressBook" />) or the system's Privacy settings.</para>
		///         </remarks>
		///         <altmember cref="RequestAccess" />
		public static ABAuthorizationStatus GetAuthorizationStatus ()
		{
			return (ABAuthorizationStatus) (long) ABAddressBookGetAuthorizationStatus ();
		}

		[DllImport (Constants.AddressBookLibrary)]
		unsafe extern static void ABAddressBookRequestAccessWithCompletion (IntPtr addressbook, BlockLiteral* completion);

		/// <include file="../../docs/api/AddressBook/ABAddressBook.xml" path="/Documentation/Docs[@DocId='M:AddressBook.ABAddressBook.RequestAccess(System.Action{System.Boolean,Foundation.NSError})']/*" />
		[BindingImpl (BindingImplOptions.Optimizable)]
		public void RequestAccess (Action<bool, NSError?> onCompleted)
		{
			if (onCompleted is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (onCompleted));

			unsafe {
#if NET
				delegate* unmanaged<IntPtr, byte, IntPtr, void> trampoline = &TrampolineCompletionHandler;
				using var block = new BlockLiteral (trampoline, onCompleted, typeof (ABAddressBook), nameof (TrampolineCompletionHandler));
#else
				using var block = new BlockLiteral ();
				block.SetupBlockUnsafe (static_completionHandler, onCompleted);
#endif
				ABAddressBookRequestAccessWithCompletion (Handle, &block);
			}
		}

#if !NET
		internal delegate void InnerCompleted (IntPtr block, byte success, IntPtr error);
		static readonly InnerCompleted static_completionHandler = TrampolineCompletionHandler;
		[MonoPInvokeCallback (typeof (InnerCompleted))]
#else
		[UnmanagedCallersOnly]
#endif
		static unsafe void TrampolineCompletionHandler (IntPtr block, byte success, IntPtr error)
		{
			var descriptor = (BlockLiteral*) block;
			var del = descriptor->Target as Action<bool, NSError?>;
			if (del is not null)
				del (success != 0, Runtime.GetNSObject<NSError> (error));
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static byte ABAddressBookHasUnsavedChanges (IntPtr addressBook);
		/// <summary>
		///           Indicates whether or not this instance has changes which haven't been
		///           saved to the global address book.
		///         </summary>
		///         <value>
		///           <see langword="true" /> if the current instance has unsaved changes;
		///           otherwise, <see langword="false" />.
		///         </value>
		///         <remarks>
		///         </remarks>
		///         <altmember cref="AddressBook.ABAddressBook.Save" />
		///         <altmember cref="AddressBook.ABAddressBook.Revert" />
		public bool HasUnsavedChanges {
			get {
				return ABAddressBookHasUnsavedChanges (GetCheckedHandle ()) != 0;
			}
		}

		[DllImport (Constants.AddressBookLibrary)]
		unsafe extern static byte ABAddressBookSave (IntPtr addressBook, IntPtr* error);
		/// <summary>
		///           Saves unsaved changes made to the current instance to the
		///           global Address Book database.
		///         </summary>
		///         <remarks>
		///         </remarks>
		///         <exception cref="CoreFoundation.CFException">
		///           The record couldn't be removed from the address book.
		///         </exception>
		public void Save ()
		{
			IntPtr error;
			unsafe {
				if (ABAddressBookSave (GetCheckedHandle (), &error) == 0)
					throw CFException.FromCFError (error);
			}
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static void ABAddressBookRevert (IntPtr addressBook);
		/// <summary>
		///           Discards unsaved changes to the address book.
		///         </summary>
		///         <remarks>
		///         </remarks>
		public void Revert ()
		{
			ABAddressBookRevert (GetCheckedHandle ());
		}

		[DllImport (Constants.AddressBookLibrary)]
		unsafe extern static byte ABAddressBookAddRecord (IntPtr addressBook, IntPtr record, IntPtr* error);
		/// <param name="record">
		///           A <see cref="AddressBook.ABRecord" /> containing
		///           the record to add to the address book.
		///         </param>
		///         <summary>
		///           Adds a record to the address book.
		///         </summary>
		///         <remarks>
		///         </remarks>
		///         <exception cref="CoreFoundation.CFException">
		///           The record couldn't be added to the address book.
		///         </exception>
		public void Add (ABRecord record)
		{
			if (record is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (record));

			IntPtr error;
			NativeHandle recordHandle = record.Handle;
			unsafe {
				if (ABAddressBookAddRecord (GetCheckedHandle (), recordHandle, &error) == 0)
					throw CFException.FromCFError (error);
			}
			record.AddressBook = this;
		}

		[DllImport (Constants.AddressBookLibrary)]
		unsafe extern static byte ABAddressBookRemoveRecord (IntPtr addressBook, IntPtr record, IntPtr* error);
		/// <param name="record">
		///           A <see cref="AddressBook.ABRecord" /> containing
		///           the record to remove from the address book.
		///         </param>
		///         <summary>
		///           Removes a record from the address book.
		///         </summary>
		///         <remarks>
		///         </remarks>
		///         <exception cref="CoreFoundation.CFException">
		///           The record couldn't be removed from the address book.
		///         </exception>
		public void Remove (ABRecord record)
		{
			if (record is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (record));

			IntPtr error;
			NativeHandle recordHandle = record.Handle;
			unsafe {
				if (ABAddressBookRemoveRecord (GetCheckedHandle (), recordHandle, &error) == 0)
					throw CFException.FromCFError (error);
			}
			record.AddressBook = null;
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static nint ABAddressBookGetPersonCount (IntPtr addressBook);
		/// <summary>
		///           Gets the number of <see cref="AddressBook.ABPerson" />
		///           records in the address book.
		///         </summary>
		///         <value>
		///           A <see cref="System.Int32" /> containing the number of
		///           <see cref="AddressBook.ABPerson" />
		///           records in the address book.
		///         </value>
		///         <remarks>
		///         </remarks>
		public nint PeopleCount {
			get {
				return ABAddressBookGetPersonCount (GetCheckedHandle ());
			}
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABAddressBookCopyArrayOfAllPeople (IntPtr addressBook);
		/// <summary>
		///           Gets all people in the address book.
		///         </summary>
		///         <returns>
		///           A <see cref="AddressBook.ABPerson" /> array containing
		///           all people in the address book.
		///         </returns>
		///         <remarks>
		///         </remarks>
		public ABPerson [] GetPeople ()
		{
			var cfArrayRef = ABAddressBookCopyArrayOfAllPeople (GetCheckedHandle ());
			return NSArray.ArrayFromHandle (cfArrayRef, h => new ABPerson (h, this));
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABAddressBookCopyArrayOfAllPeopleInSource (IntPtr addressBook, IntPtr source);

		/// <param name="source">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public ABPerson [] GetPeople (ABRecord source)
		{
			if (source is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (source));
			var cfArrayRef = ABAddressBookCopyArrayOfAllPeopleInSource (GetCheckedHandle (), source.Handle);
			GC.KeepAlive (source);
			return NSArray.ArrayFromHandle (cfArrayRef, l => new ABPerson (l, this));
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABAddressBookCopyArrayOfAllPeopleInSourceWithSortOrdering (IntPtr addressBook, IntPtr source, ABPersonSortBy sortOrdering);

		/// <param name="source">To be added.</param>
		///         <param name="sortOrdering">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public ABPerson [] GetPeople (ABRecord source, ABPersonSortBy sortOrdering)
		{
			if (source is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (source));
			var cfArrayRef = ABAddressBookCopyArrayOfAllPeopleInSourceWithSortOrdering (GetCheckedHandle (), source.Handle, sortOrdering);
			GC.KeepAlive (source);
			return NSArray.ArrayFromHandle (cfArrayRef, l => new ABPerson (l, this));
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static nint ABAddressBookGetGroupCount (IntPtr addressBook);
		/// <summary>
		///           Gets the number of groups in the address book.
		///         </summary>
		///         <value>
		///           A <see cref="System.Int32" /> containing the number of
		///           <see cref="AddressBook.ABGroup" /> records
		///           in the address book.
		///         </value>
		///         <remarks>
		///         </remarks>
		public nint GroupCount {
			get {
				return ABAddressBookGetGroupCount (GetCheckedHandle ());
			}
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABAddressBookCopyArrayOfAllGroups (IntPtr addressBook);
		/// <summary>
		///           Gets all groups in the address book.
		///         </summary>
		///         <returns>
		///           A <see cref="AddressBook.ABGroup" /> array containing
		///           all groups in the address book.
		///         </returns>
		///         <remarks>
		///         </remarks>
		public ABGroup [] GetGroups ()
		{
			var cfArrayRef = ABAddressBookCopyArrayOfAllGroups (GetCheckedHandle ());
			return NSArray.ArrayFromHandle (cfArrayRef, h => new ABGroup (h, this));
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABAddressBookCopyArrayOfAllGroupsInSource (IntPtr addressBook, IntPtr source);

		/// <param name="source">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public ABGroup [] GetGroups (ABRecord source)
		{
			if (source is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (source));

			var cfArrayRef = ABAddressBookCopyArrayOfAllGroupsInSource (GetCheckedHandle (), source.Handle);
			GC.KeepAlive (source);
			return NSArray.ArrayFromHandle (cfArrayRef, l => new ABGroup (l, this));
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABAddressBookCopyLocalizedLabel (IntPtr label);
		/// <param name="label">
		///           A <see cref="Foundation.NSString" /> containing
		///           the label to localize.
		///         </param>
		///         <summary>
		///           Localize a record-property label into the current UI language.
		///         </summary>
		///         <returns>
		///           A <see cref="System.String" /> containing the localized
		///           version of the record-property label <paramref name="label" />.
		///         </returns>
		///         <remarks>
		///           <para>
		///             <paramref name="label" /> should be a field from one of the
		///             <c>ABPerson*Label</c> types, e.g.
		///             <see cref="AddressBook.ABPersonPhoneLabel.iPhone" />.
		///           </para>
		///         </remarks>
		public static string? LocalizedLabel (NSString label)
		{
			if (label is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (label));

			string? result = CFString.FromHandle (ABAddressBookCopyLocalizedLabel (label.Handle));
			GC.KeepAlive (label);
			return result;
		}

		[DllImport (Constants.AddressBookLibrary)]
#if NET
		extern unsafe static void ABAddressBookRegisterExternalChangeCallback (IntPtr addressBook,
			delegate* unmanaged<IntPtr, IntPtr, IntPtr, void> callback, IntPtr context);
#else
		extern static void ABAddressBookRegisterExternalChangeCallback (IntPtr addressBook, ABExternalChangeCallback callback, IntPtr context);
#endif

		[DllImport (Constants.AddressBookLibrary)]
#if NET
		extern unsafe static void ABAddressBookUnregisterExternalChangeCallback (IntPtr addressBook, delegate* unmanaged<IntPtr, IntPtr, IntPtr, void> callback, IntPtr context);
#else
		extern static void ABAddressBookUnregisterExternalChangeCallback (IntPtr addressBook, ABExternalChangeCallback callback, IntPtr context);
#endif

#if !NET
		delegate void ABExternalChangeCallback (IntPtr addressBook, IntPtr info, IntPtr context);

		[MonoPInvokeCallback (typeof (ABExternalChangeCallback))]
#else
		[UnmanagedCallersOnly]
#endif
		static void ExternalChangeCallback (IntPtr addressBook, IntPtr info, IntPtr context)
		{
			GCHandle s = GCHandle.FromIntPtr (context);
			var self = s.Target as ABAddressBook;
			if (self is null)
				return;
			self.OnExternalChange (new ExternalChangeEventArgs (
						   new ABAddressBook (addressBook, false),
						   Runtime.GetNSObject<NSDictionary> (info)));
		}

		object eventLock = new object ();

		EventHandler<ExternalChangeEventArgs>? externalChange;

		/// <param name="e">
		///           A <see cref="AddressBook.ExternalChangeEventArgs" />
		///           instance containing information about the external change.
		///         </param>
		///         <summary>
		///           Raises the
		///           <see cref="AddressBook.ABAddressBook.ExternalChange" />
		///           event.
		///         </summary>
		///         <remarks>
		///           <block subset="none" type="overrides">
		///             Call <c>base.OnExternalChange(e)</c> in order to raise
		///             the
		///             <see cref="AddressBook.ABAddressBook.ExternalChange" />
		///             event.
		///           </block>
		///         </remarks>
		protected virtual void OnExternalChange (ExternalChangeEventArgs e)
		{
			GetCheckedHandle ();
			var h = externalChange;
			if (h is not null)
				h (this, e);
		}

		public event EventHandler<ExternalChangeEventArgs> ExternalChange {
			add {
				lock (eventLock) {
					if (externalChange is null) {
						sender = GCHandle.Alloc (this);
#if NET
						unsafe {
							ABAddressBookRegisterExternalChangeCallback (Handle, &ExternalChangeCallback, GCHandle.ToIntPtr (sender));
						}
#else
						ABAddressBookRegisterExternalChangeCallback (Handle, ExternalChangeCallback, GCHandle.ToIntPtr (sender));
#endif
					}
					externalChange += value;
				}
			}
			remove {
				lock (eventLock) {
					externalChange -= value;
					if (externalChange is null) {
#if NET
						unsafe {
							ABAddressBookUnregisterExternalChangeCallback (Handle, &ExternalChangeCallback, GCHandle.ToIntPtr (sender));
						}
#else
						ABAddressBookUnregisterExternalChangeCallback (Handle, ExternalChangeCallback, GCHandle.ToIntPtr (sender));
#endif
						sender.Free ();
					}
				}
			}
		}

		/// <summary>
		///           Returns an enumerator that iterates through all records and groups
		///           in the address book.
		///         </summary>
		///         <returns>
		///           An <see cref="System.Collections.IEnumerator" />
		///           which will return all records and groups in the address book.
		///         </returns>
		///         <remarks>
		///         </remarks>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

		/// <summary>
		///           Returns an enumerator that iterates through all records and groups
		///           in the address book.
		///         </summary>
		///         <returns>
		///           An
		///           <see cref="System.Collections.Generic.IEnumerator{T}" /> of <see cref="AddressBook.ABRecord" />
		///           which will return all records and groups in the address book.
		///         </returns>
		///         <remarks>
		///         </remarks>
		public IEnumerator<ABRecord> GetEnumerator ()
		{
			GetCheckedHandle ();
			foreach (var p in GetPeople ())
				yield return p;
			foreach (var g in GetGroups ())
				yield return g;
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABAddressBookGetGroupWithRecordID (IntPtr addressBook, int /* ABRecordID */ recordId);
		/// <param name="recordId">
		///           A <see cref="System.Int32" /> containing the record ID.
		///         </param>
		///         <summary>
		///           Returns the <see cref="AddressBook.ABGroup" />
		///           with the given record ID.
		///         </summary>
		///         <returns>
		///           A <see cref="AddressBook.ABGroup" /> where
		///           <see cref="AddressBook.ABRecord.Id" /> is
		///           equal to <paramref name="recordId" />, or
		///           <see langword="null" /> if there is no such group.
		///         </returns>
		///         <remarks>
		///         </remarks>
		public ABGroup? GetGroup (int recordId)
		{
			var h = ABAddressBookGetGroupWithRecordID (Handle, recordId);
			if (h == IntPtr.Zero)
				return null;
			return new ABGroup (h, this);
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABAddressBookGetPersonWithRecordID (IntPtr addressBook, int /* ABRecordID */ recordId);
		/// <param name="recordId">
		///           A <see cref="System.Int32" /> containing the record ID.
		///         </param>
		///         <summary>
		///           Returns the <see cref="AddressBook.ABPerson" />
		///           with the given record ID.
		///         </summary>
		///         <returns>
		///           A <see cref="AddressBook.ABPerson" /> where
		///           <see cref="AddressBook.ABRecord.Id" /> is
		///           equal to <paramref name="recordId" />, or
		///           <see langword="null" /> if there is no such group.
		///         </returns>
		///         <remarks>
		///         </remarks>
		public ABPerson? GetPerson (int recordId)
		{
			var h = ABAddressBookGetPersonWithRecordID (Handle, recordId);
			if (h == IntPtr.Zero)
				return null;
			return new ABPerson (h, this);
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABAddressBookCopyPeopleWithName (IntPtr addressBook, IntPtr name);
		/// <param name="name">
		///           A <see cref="System.String" /> containing the name of the person
		///           to search for.
		///         </param>
		///         <summary>
		///           Gets all <see cref="AddressBook.ABPerson" /> array
		///           containing all records with a matching name.
		///         </summary>
		///         <returns>
		///           A <see cref="AddressBook.ABPerson" /> array
		///           containing all records where <paramref name="name" />
		///           matches the records composite name.
		///         </returns>
		///         <remarks>
		///         </remarks>
		public ABPerson [] GetPeopleWithName (string name)
		{
			var nameHandle = CFString.CreateNative (name);
			try {
				var cfArrayRef = ABAddressBookCopyPeopleWithName (Handle, nameHandle);
				return NSArray.ArrayFromHandle (cfArrayRef, h => new ABPerson (h, this));
			} finally {
				CFString.ReleaseNative (nameHandle);
			}
		}

		// ABSource
		// http://developer.apple.com/library/IOs/#documentation/AddressBook/Reference/ABSourceRef_iPhoneOS/Reference/reference.html

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr /* CFArrayRef */ ABAddressBookCopyArrayOfAllSources (IntPtr /* ABAddressBookRef */ addressBook);
		/// <summary>Returns all of the addresbook sources available on the system.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		public ABSource []? GetAllSources ()
		{
			var cfArrayRef = ABAddressBookCopyArrayOfAllSources (GetCheckedHandle ());
			return NSArray.ArrayFromHandle (cfArrayRef, h => new ABSource (h, this));
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr /* ABRecordRef */ ABAddressBookCopyDefaultSource (IntPtr /* ABAddressBookRef */ addressBook);
		/// <summary>Returns the default addressbook source for the system.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		public ABSource? GetDefaultSource ()
		{
			var h = ABAddressBookCopyDefaultSource (GetCheckedHandle ());
			if (h == IntPtr.Zero)
				return null;
			return new ABSource (h, this);
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr /* ABRecordRef */ ABAddressBookGetSourceWithRecordID (IntPtr /* ABAddressBookRef */ addressBook, int /* ABRecordID */ sourceID);
		/// <param name="sourceID">To be added.</param>
		///         <summary>Returns a specific addressbook source</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		public ABSource? GetSource (int sourceID)
		{
			var h = ABAddressBookGetSourceWithRecordID (GetCheckedHandle (), sourceID);
			if (h == IntPtr.Zero)
				return null;
			return new ABSource (h, this);
		}
	}
}

#endif // !MONOMAC
