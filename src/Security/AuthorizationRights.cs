#if __MACOS__
#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using ObjCRuntime;

namespace Security {

	/// <summary>Represents a single authorization right with a name and an optional value.</summary>
	[SupportedOSPlatform ("macos")]
	public sealed class AuthorizationRight {
		readonly byte []? value;

		/// <summary>Gets the name of the authorization right.</summary>
		public string Name { get; }

		/// <summary>Gets a copy of the value data, or <see langword="null" /> if no value is set.</summary>
		public byte []? Value => value is null ? null : (byte []) value.Clone ();

		/// <summary>Creates a new authorization right with the specified name and optional value.</summary>
		public AuthorizationRight (string name, byte []? value = null)
			: this (name, value is null ? default : value.AsSpan ())
		{
		}

		/// <summary>Creates a new authorization right with the specified name and value.</summary>
		public AuthorizationRight (string name, ReadOnlySpan<byte> value)
		{
			ArgumentNullException.ThrowIfNull (name);
			Name = name;
			this.value = value.IsEmpty ? null : value.ToArray ();
		}

		internal ReadOnlySpan<byte> GetRawValue () => value;
	}

	[StructLayout (LayoutKind.Sequential)]
	struct AuthorizationItemNative {
		internal IntPtr Name;
		internal nuint ValueLength;
		internal IntPtr Value;
		internal uint Flags;
	}

	[StructLayout (LayoutKind.Sequential)]
	struct AuthorizationRightsNative {
		internal uint Count;
		internal unsafe AuthorizationItemNative* Items;
	}

	/// <summary>Represents a set of authorization rights used to configure an authorization view.</summary>
	[SupportedOSPlatform ("macos")]
	public sealed class AuthorizationRights : DisposableObject, IReadOnlyList<AuthorizationRight> {
		readonly IList<AuthorizationRight> items;

		/// <summary>Creates an empty authorization rights set.</summary>
		public AuthorizationRights ()
			: this ((IList<AuthorizationRight>) [])
		{
		}

		/// <summary>Creates a new authorization rights set from the specified right names.</summary>
		public AuthorizationRights (params string [] rights)
			: this (CreateItems (rights))
		{
		}

		/// <summary>Creates a new authorization rights set from the specified rights.</summary>
		/// <remarks>The supplied list must not be modified after this instance is created.</remarks>
		public unsafe AuthorizationRights (params IList<AuthorizationRight> items)
			: base ((IntPtr) AllocateNative (items), owns: true)
		{
			this.items = items;
		}

		/// <summary>Gets the number of rights in this set.</summary>
		public int Count => items.Count;

		/// <summary>Gets the authorization right at the specified index.</summary>
		public AuthorizationRight this [int index] => items [index];

		/// <summary>Returns an enumerator that iterates through the authorization rights.</summary>
		public IEnumerator<AuthorizationRight> GetEnumerator () => items.GetEnumerator ();

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();

		internal static unsafe AuthorizationRights? FromHandle (AuthorizationRightsNative* native)
		{
			if (native is null)
				return null;

			if (native->Count > 0 && native->Items is null)
				throw new InvalidOperationException ("The native authorization rights have a non-zero count and a null items pointer.");

			var count = checked((int) native->Count);
			var managedItems = new AuthorizationRight [count];
			for (var i = 0; i < count; i++) {
				var item = native->Items [i];
				if (item.Name == IntPtr.Zero)
					throw new InvalidOperationException ("A native authorization right has a null name pointer.");

				var name = Marshal.PtrToStringUTF8 (item.Name);
				if (name is null)
					throw new InvalidOperationException ("A native authorization right has an invalid UTF-8 name.");

				ReadOnlySpan<byte> value = default;
				if (item.ValueLength > 0) {
					if (item.Value == IntPtr.Zero)
						throw new InvalidOperationException ("A native authorization right has a non-zero value length and a null value pointer.");
					var length = checked((int) item.ValueLength);
					value = new ReadOnlySpan<byte> ((void*) item.Value, length);
				}

				managedItems [i] = new AuthorizationRight (name, value);
			}

			return new AuthorizationRights (managedItems);
		}

		static AuthorizationRight [] CreateItems (string [] rights)
		{
			ArgumentNullException.ThrowIfNull (rights);
			var result = new AuthorizationRight [rights.Length];
			for (var i = 0; i < rights.Length; i++)
				result [i] = new AuthorizationRight (rights [i]);
			return result;
		}

		static unsafe AuthorizationRightsNative* AllocateNative (IList<AuthorizationRight> items)
		{
			ArgumentNullException.ThrowIfNull (items);
			var count = items.Count;
			var native = (AuthorizationRightsNative*) Marshal.AllocHGlobal (sizeof (AuthorizationRightsNative));
			native->Count = 0;
			native->Items = null;

			try {
				if (count == 0)
					return native;

				native->Items = (AuthorizationItemNative*) Marshal.AllocHGlobal (sizeof (AuthorizationItemNative) * count);
				for (var i = 0; i < count; i++) {
					var item = items [i];
					ArgumentNullException.ThrowIfNull (item);
					var name = Marshal.StringToHGlobalAuto (item.Name);
					var value = item.GetRawValue ();
					var valuePointer = IntPtr.Zero;
					try {
						if (!value.IsEmpty) {
							valuePointer = Marshal.AllocHGlobal (value.Length);
							value.CopyTo (new Span<byte> ((void*) valuePointer, value.Length));
						}

						native->Items [i] = new AuthorizationItemNative {
							Name = name,
							ValueLength = (nuint) value.Length,
							Value = valuePointer,
							Flags = 0,
						};
						native->Count++;
					} catch {
						Marshal.FreeHGlobal (name);
						if (valuePointer != IntPtr.Zero)
							Marshal.FreeHGlobal (valuePointer);
						throw;
					}
				}

				return native;
			} catch {
				FreeNative (native);
				throw;
			}
		}

		static unsafe void FreeNative (AuthorizationRightsNative* native)
		{
			if (native is null)
				return;

			if (native->Items is not null) {
				for (var i = 0; i < native->Count; i++) {
					Marshal.FreeHGlobal (native->Items [i].Name);
					if (native->Items [i].Value != IntPtr.Zero)
						Marshal.FreeHGlobal (native->Items [i].Value);
				}
				Marshal.FreeHGlobal ((IntPtr) native->Items);
			}
			Marshal.FreeHGlobal ((IntPtr) native);
		}

		/// <inheritdoc />
		protected override unsafe void Dispose (bool disposing)
		{
			if (Owns)
				FreeNative ((AuthorizationRightsNative*) (IntPtr) Handle);
			base.Dispose (disposing);
		}
	}
}
#endif // __MACOS__
