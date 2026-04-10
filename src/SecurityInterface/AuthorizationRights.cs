#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using ObjCRuntime;

namespace SecurityInterface {

	/// <summary>Represents a single authorization right with a name, optional value, and flags.</summary>
	[SupportedOSPlatform ("macos")]
	public readonly struct AuthorizationRight {
		readonly byte []? value;

		/// <summary>Gets the name of the authorization right.</summary>
		public string Name { get; }

		/// <summary>Gets the flags associated with this right.</summary>
		public uint Flags { get; }

		/// <summary>Gets a copy of the value data, or <see langword="null" /> if no value is set.</summary>
		public byte []? Value => value is null ? null : (byte []) value.Clone ();

		/// <summary>Creates a new authorization right with the specified name, optional value, and flags.</summary>
		/// <param name="name">The authorization right name.</param>
		/// <param name="value">The optional value data.</param>
		/// <param name="flags">The flags for this right.</param>
		public AuthorizationRight (string name, byte []? value = null, uint flags = 0)
		{
			if (name is null)
				ThrowHelper.ThrowArgumentNullException (nameof (name));
			Name = name;
			this.value = value is null ? null : (byte []) value.Clone ();
			Flags = flags;
		}

		internal byte []? GetRawValue () => value;
	}

	[StructLayout (LayoutKind.Sequential)]
	unsafe struct AuthorizationItemNative {
		public IntPtr Name;
		public nuint ValueLength;
		public IntPtr Value;
		public uint Flags;
	}

	[StructLayout (LayoutKind.Sequential)]
	unsafe struct AuthorizationRightsNative {
		public uint Count;
		public AuthorizationItemNative* Items;
	}

	/// <summary>Represents a set of authorization rights used to configure an <see cref="SFAuthorizationView" />.</summary>
	[SupportedOSPlatform ("macos")]
	public unsafe sealed class AuthorizationRights : IDisposable, INativeObject, IReadOnlyList<AuthorizationRight> {
		NativeHandle handle;
		readonly AuthorizationRight [] items;

		/// <summary>Creates a new authorization rights set from the specified rights.</summary>
		/// <param name="items">The authorization rights to include.</param>
		public AuthorizationRights (params AuthorizationRight [] items)
			: this ((IEnumerable<AuthorizationRight>) items)
		{
		}

		/// <summary>Creates a new authorization rights set from the specified right names.</summary>
		/// <param name="rights">The authorization right names.</param>
		public AuthorizationRights (params string [] rights)
		{
			if (rights is null)
				ThrowHelper.ThrowArgumentNullException (nameof (rights));
			items = new AuthorizationRight [rights.Length];
			for (int i = 0; i < rights.Length; i++)
				items [i] = new AuthorizationRight (rights [i]);
			AllocateNative ();
		}

		/// <summary>Creates a new authorization rights set from the specified rights.</summary>
		/// <param name="items">The authorization rights to include.</param>
		public AuthorizationRights (IEnumerable<AuthorizationRight> items)
		{
			if (items is null)
				ThrowHelper.ThrowArgumentNullException (nameof (items));
			var list = new List<AuthorizationRight> ();
			foreach (var item in items)
				list.Add (new AuthorizationRight (item.Name, item.GetRawValue (), item.Flags));
			this.items = list.ToArray ();
			AllocateNative ();
		}

		AuthorizationRights (AuthorizationRight [] items, bool noCopy)
		{
			this.items = items;
			AllocateNative ();
		}

		~AuthorizationRights ()
		{
			Dispose (false);
		}

		/// <summary>Gets the native handle to the AuthorizationRights structure.</summary>
		public NativeHandle Handle => handle;

		/// <summary>Gets the number of rights in this set.</summary>
		public int Count => items.Length;

		/// <summary>Gets the authorization right at the specified index.</summary>
		/// <param name="index">The zero-based index.</param>
		public AuthorizationRight this [int index] => items [index];

		/// <summary>Returns an enumerator that iterates through the authorization rights.</summary>
		public IEnumerator<AuthorizationRight> GetEnumerator () => ((IEnumerable<AuthorizationRight>) items).GetEnumerator ();

		IEnumerator IEnumerable.GetEnumerator () => items.GetEnumerator ();

		/// <summary>Creates an <see cref="AuthorizationRights" /> by reading from a native AuthorizationRights pointer.</summary>
		/// <param name="handle">The pointer to the native AuthorizationRights structure, or zero for <see langword="null" />.</param>
		/// <returns>A new managed rights set cloned from the native data, or <see langword="null" /> if the handle is zero.</returns>
		public static AuthorizationRights? FromHandle (NativeHandle handle)
		{
			if (handle == NativeHandle.Zero)
				return null;

			var native = (AuthorizationRightsNative*) handle;
			var managedItems = new AuthorizationRight [native->Count];

			for (int i = 0; i < native->Count; i++) {
				var item = native->Items [i];
				var name = Marshal.PtrToStringUTF8 (item.Name)!;
				byte []? value = null;

				if (item.ValueLength != 0) {
					var length = checked((int) item.ValueLength);
					value = new byte [length];
					Marshal.Copy (item.Value, value, 0, length);
				}

				managedItems [i] = new AuthorizationRight (name, value, item.Flags);
			}

			return new AuthorizationRights (managedItems, noCopy: true);
		}

		void AllocateNative ()
		{
			handle = Marshal.AllocHGlobal (sizeof (AuthorizationRightsNative));
			var native = (AuthorizationRightsNative*) handle;
			native->Count = (uint) items.Length;

			if (items.Length == 0) {
				native->Items = null;
				return;
			}

			native->Items = (AuthorizationItemNative*) Marshal.AllocHGlobal (sizeof (AuthorizationItemNative) * items.Length);

			for (int i = 0; i < items.Length; i++) {
				var item = items [i];
				var value = item.GetRawValue ();
				native->Items [i] = new AuthorizationItemNative {
					Name = StringToUtf8 (item.Name),
					ValueLength = value is null ? 0 : (nuint) value.Length,
					Value = value is null || value.Length == 0 ? IntPtr.Zero : BytesToHGlobal (value),
					Flags = item.Flags,
				};
			}
		}

		static IntPtr StringToUtf8 (string value)
		{
			var bytes = Encoding.UTF8.GetBytes (value);
			var ptr = Marshal.AllocHGlobal (bytes.Length + 1);
			Marshal.Copy (bytes, 0, ptr, bytes.Length);
			Marshal.WriteByte (ptr, bytes.Length, 0);
			return ptr;
		}

		static IntPtr BytesToHGlobal (byte [] value)
		{
			var ptr = Marshal.AllocHGlobal (value.Length);
			Marshal.Copy (value, 0, ptr, value.Length);
			return ptr;
		}

		/// <summary>Releases all unmanaged memory associated with this rights set.</summary>
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		void Dispose (bool disposing)
		{
			if (handle == NativeHandle.Zero)
				return;

			var native = (AuthorizationRightsNative*) handle;
			if (native->Items is not null) {
				for (int i = 0; i < native->Count; i++) {
					if (native->Items [i].Name != IntPtr.Zero)
						Marshal.FreeHGlobal (native->Items [i].Name);
					if (native->Items [i].Value != IntPtr.Zero)
						Marshal.FreeHGlobal (native->Items [i].Value);
				}
				Marshal.FreeHGlobal ((IntPtr) native->Items);
			}

			Marshal.FreeHGlobal (handle);
			handle = NativeHandle.Zero;
		}
	}
}
