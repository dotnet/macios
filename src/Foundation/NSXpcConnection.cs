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

namespace Foundation {
	public partial class NSXpcConnection : NSObject {
		/// <summary>
		/// Creates a proxy object for the remote object exposed by the connection.
		/// </summary>
		/// <typeparam name="TProtocol">The protocol interface type that the remote object conforms to.</typeparam>
		/// <returns>A proxy object that conforms to the specified protocol.</returns>
		public TProtocol CreateRemoteObjectProxy<TProtocol> ()
			where TProtocol : class, INativeObject
		{
			IntPtr nativeProxyPtr = _CreateRemoteObjectProxy ();
			return Runtime.GetINativeObject<TProtocol> (nativeProxyPtr, true)!;
		}

		/// <summary>
		/// Creates a proxy object for the remote object exposed by the connection with an error handler.
		/// </summary>
		/// <typeparam name="TProtocol">The protocol interface type that the remote object conforms to.</typeparam>
		/// <param name="errorHandler">A handler that is called when an error occurs during the communication with the remote object.</param>
		/// <returns>A proxy object that conforms to the specified protocol.</returns>
		public TProtocol CreateRemoteObjectProxy<TProtocol> (Action<NSError> errorHandler)
			where TProtocol : class, INativeObject
		{
			IntPtr nativeProxyPtr = _CreateRemoteObjectProxy (errorHandler);
			return Runtime.GetINativeObject<TProtocol> (nativeProxyPtr, true)!;
		}

		/// <summary>
		/// Creates a synchronous proxy object for the remote object exposed by the connection with an error handler.
		/// </summary>
		/// <typeparam name="TProtocol">The protocol interface type that the remote object conforms to.</typeparam>
		/// <param name="errorHandler">A handler that is called when an error occurs during the communication with the remote object.</param>
		/// <returns>A synchronous proxy object that conforms to the specified protocol.</returns>
		public TProtocol CreateSynchronousRemoteObjectProxy<TProtocol> (Action<NSError> errorHandler)
			where TProtocol : class, INativeObject
		{
			IntPtr nativeProxyPtr = _CreateSynchronousRemoteObjectProxy (errorHandler);
			return Runtime.GetINativeObject<TProtocol> (nativeProxyPtr, true)!;
		}
	}
}
