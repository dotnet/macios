//
// NSHost.cs: augment NSHost with C#isms and
// System.Net's IPAddress and IPHostEntry.
//
// Authors:
//   Aaron Bockover <abock@xamarin.com>
//
// Copyright 2013 Xamarin, Inc. All rights reserved.
//

#if MONOMAC

#nullable enable

using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;

namespace Foundation {
	public partial class NSHost : IEquatable<NSHost>, IEnumerable<IPAddress> {
		static NSHost? CheckNull (NSHost? host)
		{
			if (host is null)
				return null;

			var addrs = host._Addresses;
			if (addrs is null || addrs.Length == 0)
				return null;

			return host;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public static NSHost? Current {
			get { return CheckNull (_Current); }
		}

		/// <param name="address">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSHost? FromAddress (string? address)
		{
			if (address is null)
				return null;
			return CheckNull (_FromAddress (address));
		}

		/// <param name="name">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSHost? FromName (string? name)
		{
			if (name is null)
				return null;
			return CheckNull (_FromName (name));
		}

		public static explicit operator IPAddress? (NSHost? host)
		{
			if (host is null)
				return null;

			return host.Address;
		}

		public static explicit operator NSHost? (IPAddress? address)
		{
			return FromAddress (address);
		}

		public static explicit operator IPHostEntry? (NSHost host)
		{
			if (host is null)
				return null;

			return host.ToIPHostEntry ();
		}

		public static explicit operator NSHost? (IPHostEntry? hostEntry)
		{
			return FromIPHostEntry (hostEntry);
		}

		/// <param name="hostEntry">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSHost? FromIPHostEntry (IPHostEntry? hostEntry)
		{
			if (hostEntry is null)
				return null;

			if (hostEntry.AddressList is not null) {
				foreach (var addr in hostEntry.AddressList) {
					var host = FromAddress (addr);
					if (host is not null)
						return host;
				}
			}

			if (hostEntry.HostName is not null) {
				var host = FromName (hostEntry.HostName);
				if (host is not null)
					return host;
			}

			if (hostEntry.Aliases is not null) {
				foreach (var name in hostEntry.Aliases) {
					var host = FromName (name);
					if (host is not null)
						return host;
				}
			}

			return null;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public IPHostEntry ToIPHostEntry ()
		{
			return new IPHostEntry {
				HostName = Name!,
				AddressList = Addresses,
				Aliases = Names,
			};
		}

		/// <param name="address">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSHost? FromAddress (IPAddress? address)
		{
			if (address is null)
				return null;
			return FromAddress (address.ToString ());
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public IPAddress? Address {
			get {
				var address = _Address;
				if (address is null)
					return null;
				return IPAddress.Parse (address);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public IPAddress [] Addresses {
			get {
				var addrs = new IPAddress [_Addresses.Length];
				for (int i = 0; i < addrs.Length; i++)
					addrs [i] = IPAddress.Parse (_Addresses [i]);
				return addrs;
			}
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override int GetHashCode ()
		{
			return (int) _Hash;
		}

		/// <param name="obj">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override bool Equals (object? obj)
		{
			if (obj == this)
				return true;

			var host = obj as NSHost;
			if (host is not null)
				return Equals (host);

			return false;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public IEnumerator<IPAddress> GetEnumerator ()
		{
			foreach (var address in Addresses)
				yield return address;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

		bool IEquatable<NSHost>.Equals (NSHost? other)
		{
			return Equals ((object?) other);
		}
	}
}

#endif // MONOMAC
