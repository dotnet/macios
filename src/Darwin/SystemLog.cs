//
// SystemLog.cs: Bindings to the asl(3) API from Apple
//
// Authors:
//  Miguel de Icaza (miguel@xamarin.com)
//
// Copyright 2012, Xamarin Inc.
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

#nullable enable

#if MONOMAC

using System;
using Foundation;
using ObjCRuntime;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Darwin {

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	public class SystemLog : DisposableObject {
		static SystemLog? _default;

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public static SystemLog Default {
			get {
				if (_default is null)
					_default = new SystemLog ();
				return _default!;
			}
		}

		/// <summary>To be added.</summary>
		///     <remarks>To be added.</remarks>
		[Flags]
		public enum Option {
			/// <summary>To be added.</summary>
			Stderr,
			/// <summary>To be added.</summary>
			NoDelay,
			/// <summary>To be added.</summary>
			NoRemote,
		}

		/// <param name="disposing">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		protected override void Dispose (bool disposing)
		{
			if (Handle != IntPtr.Zero && Owns)
				asl_close (Handle);
			base.Dispose (disposing);
		}

		[DllImport (Constants.SystemLibrary)]
		extern static void asl_close (IntPtr handle);

		[DllImport (Constants.SystemLibrary)]
		extern static IntPtr asl_open (IntPtr ident, IntPtr facility, Option /* uint32_t */ options);

		static IntPtr asl_open (string ident, string facility, Option options)
		{
			if (ident is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (ident));
			if (facility is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (facility));
			using var identStr = new TransientString (ident);
			using var facilityStr = new TransientString (facility);
			return asl_open (identStr, facilityStr, options);
		}

		SystemLog ()
			: base (IntPtr.Zero, false, false)
		{
		}

		[Preserve (Conditional = true)]
		SystemLog (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <param name="ident">To be added.</param>
		///         <param name="facility">To be added.</param>
		///         <param name="options">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public SystemLog (string ident, string facility, Option options = 0)
			: base (asl_open (ident, facility, options), true)
		{
		}

		[DllImport (Constants.SystemLibrary)]
		extern static IntPtr asl_open_from_file (int /* int */ fd, IntPtr ident, IntPtr facility);

		static IntPtr asl_open_from_file (int /* int */ fd, string ident, string facility)
		{
			if (ident is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (ident));
			if (facility is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (facility));
			using var identStr = new TransientString (ident);
			using var facilityStr = new TransientString (facility);
			return asl_open_from_file (fd, identStr, facilityStr);
		}

		/// <param name="fileDescriptor">To be added.</param>
		///         <param name="ident">To be added.</param>
		///         <param name="facility">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public SystemLog (int fileDescriptor, string ident, string facility)
			: base (asl_open_from_file (fileDescriptor, ident, facility), true)
		{
		}

		[DllImport (Constants.SystemLibrary)]
		extern static IntPtr asl_add_log_file (IntPtr handle, int /* int */ fd);

		[DllImport (Constants.SystemLibrary)]
		extern static IntPtr asl_remove_log_file (IntPtr handle, int /* int */ fd);

		/// <param name="descriptor">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void AddLogFile (int descriptor)
		{
			asl_add_log_file (Handle, descriptor);
		}

		/// <param name="descriptor">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void RemoveLogFile (int descriptor)
		{
			asl_remove_log_file (Handle, descriptor);
		}

		[DllImport (Constants.SystemLibrary)]
		extern static int asl_log (IntPtr handle, IntPtr msgHandle, IntPtr text);

		static int asl_log (IntPtr handle, IntPtr msgHandle, string text)
		{
			using var textStr = new TransientString (text);
			return asl_log (handle, msgHandle, textStr);
		}

		/// <param name="msg">To be added.</param>
		///         <param name="text">To be added.</param>
		///         <param name="args">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public int Log (Message msg, string text, params object [] args)
		{
			var txt = text is null ? string.Empty : String.Format (text, args);
			if (txt.IndexOf ('%') != -1)
				txt = txt.Replace ("%", "%%");
			int result = asl_log (Handle, msg.GetHandle (), txt);
			GC.KeepAlive (msg);
			return result;
		}

		/// <param name="text">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public int Log (string text)
		{
			if (text is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (text));

			return asl_log (Handle, IntPtr.Zero, text);
		}

		[DllImport (Constants.SystemLibrary)]
		extern static int asl_send (IntPtr handle, IntPtr msgHandle);

		/// <param name="msg">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public int Log (Message msg)
		{
			if (msg is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (msg));

			int result = asl_send (Handle, msg.Handle);
			GC.KeepAlive (msg);
			return result;
		}

		[DllImport (Constants.SystemLibrary)]
		extern static int asl_set_filter (IntPtr handle, int /* int */ f);

		/// <param name="level">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public int SetFilter (int level)
		{
			return asl_set_filter (Handle, level);
		}

		[DllImport (Constants.SystemLibrary)]
		extern static IntPtr asl_search (IntPtr handle, IntPtr msg);

		[DllImport (Constants.SystemLibrary)]
		extern static IntPtr aslresponse_next (IntPtr handle);

		[DllImport (Constants.SystemLibrary)]
		extern static void aslresponse_free (IntPtr handle);

		/// <param name="msg">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public IEnumerable<Message> Search (Message msg)
		{
			if (msg is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (msg));
			var search = asl_search (Handle, msg.Handle);
			GC.KeepAlive (msg);
			IntPtr mh;

			while ((mh = aslresponse_next (search)) != IntPtr.Zero)
				yield return new Message (mh, true);

			aslresponse_free (search);
		}
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	public class Message : DisposableObject {
		/// <summary>To be added.</summary>
		///     <remarks>To be added.</remarks>
		public enum Kind {
			/// <summary>To be added.</summary>
			Message,
			/// <summary>To be added.</summary>
			Query,
		}

		/// <summary>To be added.</summary>
		///     <remarks>To be added.</remarks>
		[Flags]
		public enum Op {
			/// <summary>To be added.</summary>
			CaseFold = 0x10,
			/// <summary>To be added.</summary>
			Prefix = 0x20,
			/// <summary>To be added.</summary>
			Suffix = 0x40,
			/// <summary>To be added.</summary>
			Substring = 0x60,
			/// <summary>To be added.</summary>
			Numeric = 0x80,
			/// <summary>To be added.</summary>
			Regex = 0x100,
			/// <summary>To be added.</summary>
			Equal = 1,
			/// <summary>To be added.</summary>
			Greater = 2,
			/// <summary>To be added.</summary>
			GreaterEqual = 3,
			/// <summary>To be added.</summary>
			Less = 4,
			/// <summary>To be added.</summary>
			LessEqual = 5,
			/// <summary>To be added.</summary>
			NotEqual = 6,
			/// <summary>To be added.</summary>
			True = 7,
		}

		[Preserve (Conditional = true)]
		internal Message (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <param name="kind">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public Message (Kind kind)
			: base (asl_new (kind), true)
		{
		}

		[DllImport (Constants.SystemLibrary)]
		extern static void asl_free (IntPtr handle);

		/// <param name="disposing">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		protected override void Dispose (bool disposing)
		{
			if (Handle != IntPtr.Zero && Owns)
				asl_free (Handle);
			base.Dispose (disposing);
		}

		[DllImport (Constants.SystemLibrary)]
		extern static IntPtr asl_new (Kind kind);

		[DllImport (Constants.SystemLibrary)]
		extern static IntPtr asl_get (IntPtr handle, IntPtr key);

		[DllImport (Constants.SystemLibrary)]
		extern static int asl_set (IntPtr handle, IntPtr key, IntPtr value);

		public string this [string key] {
			get {
				if (key is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (key));
				using var keyStr = new TransientString (key);
				return Marshal.PtrToStringAuto (asl_get (Handle, keyStr))!;
			}
			set {
				if (key is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (key));
				using var keyStr = new TransientString (key);
				using var valueStr = new TransientString (value);
				asl_set (Handle, keyStr, valueStr);
			}
		}

		[DllImport (Constants.SystemLibrary)]
		extern static int asl_unset (IntPtr handle, IntPtr key);

		/// <param name="key">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void Remove (string key)
		{
			if (key is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (key));
			using var keyStr = new TransientString (key);
			asl_unset (Handle, keyStr);
		}

		[DllImport (Constants.SystemLibrary)]
		extern static IntPtr asl_key (IntPtr handle, int /* uint32_t */ key);

		public string this [int key] {
			get {
				return Marshal.PtrToStringAuto (asl_key (Handle, key))!;
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string Time {
			get { return this ["Time"]; }
			set { this ["Time"] = value; }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string Host {
			get { return this ["Host"]; }
			set { this ["Host"] = value; }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string Sender {
			get { return this ["Sender"]; }
			set { this ["Sender"] = value; }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string Facility {
			get { return this ["Facility"]; }
			set { this ["Facility"] = value; }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string PID {
			get { return this ["PID"]; }
			set { this ["PID"] = value; }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string UID {
			get { return this ["UID"]; }
			set { this ["UID"] = value; }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string GID {
			get { return this ["GID"]; }
			set { this ["GID"] = value; }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string Level {
			get { return this ["Level"]; }
			set { this ["Level"] = value; }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string Msg {
			get { return this ["Message"]; }
			set { this ["Message"] = value; }
		}

		[DllImport (Constants.SystemLibrary)]
		extern static int asl_set_query (IntPtr handle, IntPtr key, IntPtr value, int /* uint32_t */ op);

		/// <param name="key">To be added.</param>
		///         <param name="op">To be added.</param>
		///         <param name="value">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool SetQuery (string key, Op op, string value)
		{
			using var keyStr = new TransientString (key);
			using var valueStr = new TransientString (value);
			return asl_set_query (Handle, keyStr, valueStr, (int) op) == 0;
		}
	}
}

#endif
