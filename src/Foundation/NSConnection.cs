//
// NSConnection.cs
//
// Author:
//   Alex Corrado <corrado@xamarin.com>
//
// Copyright 2013 Xamarin Inc. (http://xamarin.com)
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

#if MONOMAC

using System.Diagnostics.CodeAnalysis;

#nullable enable

namespace Foundation {
	public partial class NSConnection {

		/// <summary>Gets the root proxy object for this connection.</summary>
		/// <typeparam name="TProxy">The type of the proxy object to return.</typeparam>
		/// <returns>The root proxy object, cast to the specified type.</returns>
		/// <remarks>This method retrieves the root object provided by the receiver's peer. The returned object is a proxy that represents the root object on the remote side of the connection.</remarks>
		public TProxy GetRootProxy<TProxy> () where TProxy : NSObject
		{
			return GetRootProxy<TProxy> (_GetRootProxy ());
		}

		/// <summary>Gets the root proxy object for the connection identified by name and host.</summary>
		/// <typeparam name="TProxy">The type of the proxy object to return.</typeparam>
		/// <param name="name">The name of the connection.</param>
		/// <param name="hostName">The name of the host on which the connection is registered.</param>
		/// <returns>The root proxy object for the specified connection, cast to the specified type.</returns>
		/// <remarks>This method retrieves the root object from a connection identified by <paramref name="name" /> on the specified <paramref name="hostName" />.</remarks>
		public static TProxy GetRootProxy<TProxy> (string name, string? hostName) where TProxy : NSObject
		{
			return GetRootProxy<TProxy> (_GetRootProxy (name, hostName));
		}

		/// <summary>Gets the root proxy object for the connection identified by name, host, and port name server.</summary>
		/// <typeparam name="TProxy">The type of the proxy object to return.</typeparam>
		/// <param name="name">The name of the connection.</param>
		/// <param name="hostName">The name of the host on which the connection is registered.</param>
		/// <param name="server">The <see cref="NSPortNameServer" /> to use for looking up the connection.</param>
		/// <returns>The root proxy object for the specified connection, cast to the specified type.</returns>
		/// <remarks>This method retrieves the root object from a connection identified by <paramref name="name" /> on the specified <paramref name="hostName" /> using the given port name server.</remarks>
		public static TProxy GetRootProxy<TProxy> (string name, string? hostName, NSPortNameServer server) where TProxy : NSObject
		{
			return GetRootProxy<TProxy> (_GetRootProxy (name, hostName, server));
		}

		static TProxy GetRootProxy<[DynamicallyAccessedMembers (DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] TProxy> (IntPtr handle) where TProxy : NSObject
		{
			var result = Runtime.TryGetNSObject (handle) as TProxy;

			if (result is null)
				result = (TProxy) Activator.CreateInstance (typeof (TProxy), new object [] { handle })!;

			return result;
		}
	}
}

#endif // MONOMAC
