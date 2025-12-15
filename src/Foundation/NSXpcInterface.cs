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

using System.Reflection;

#nullable enable

namespace Foundation {
	public partial class NSXpcInterface : NSObject {
		/// <summary>Creates an <see cref="NSXpcInterface" /> instance for the specified interface type.</summary>
		/// <param name="interfaceType">The interface type that defines the XPC protocol.</param>
		/// <returns>A new <see cref="NSXpcInterface" /> instance.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="interfaceType" /> is <see langword="null" />.</exception>
		public static NSXpcInterface Create (Type interfaceType)
		{
			ArgumentNullException.ThrowIfNull (interfaceType);
			return Create (new Protocol (interfaceType));
		}

		/// <summary>Gets the allowed classes for a specific method argument.</summary>
		/// <param name="method">the method for which to get the allowed classes.</param>
		/// <param name="argumentIndex">The zero-based index of the argument.</param>
		/// <param name="forReplyBlock">A value indicating whether the allowed classes are for a reply block.</param>
		/// <returns>A set of allowed <see cref="Class" /> objects for the specified method argument.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="method" /> is <see langword="null" />.</exception>
		/// <exception cref="ArgumentException">Thrown when the method is not exposed to Objective-C.</exception>
		public NSSet<Class> GetAllowedClasses (MethodInfo method, nuint argumentIndex, bool forReplyBlock)
		{
			ArgumentNullException.ThrowIfNull (method);

			var attribute = method.GetCustomAttribute<ExportAttribute> ();
			if (attribute is null)
				throw new ArgumentException ($"Method {method.Name} is not exposed to Objective-C", nameof (method));

			// The runtime ensures that the Selector property is non-null and a valid selector.
			var sel = new Selector (attribute.Selector!);
			return GetAllowedClasses (sel, argumentIndex, forReplyBlock);
		}

		/// <summary>Sets the allowed classes for a specific method argument.</summary>
		/// <param name="method">the method for which to set the allowed classes.</param>
		/// <param name="allowedClasses">A set of allowed <see cref="Class" /> objects.</param>
		/// <param name="argumentIndex">The zero-based index of the argument.</param>
		/// <param name="forReplyBlock">A value indicating whether the allowed classes are for a reply block.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="method" /> is <see langword="null" />.</exception>
		/// <exception cref="ArgumentException">Thrown when the method is not exposed to Objective-C.</exception>
		public void SetAllowedClasses (MethodInfo method, NSSet<Class> allowedClasses, nuint argumentIndex, bool forReplyBlock)
		{
			ArgumentNullException.ThrowIfNull (method);

			var attribute = method.GetCustomAttribute<ExportAttribute> ();
			if (attribute is null)
				throw new ArgumentException ($"Method {method.Name} is not exposed to Objective-C", nameof (method));

			// The runtime ensures that the Selector property is non-null and a valid selector.
			var sel = new Selector (attribute.Selector!);
			SetAllowedClasses (allowedClasses, sel, argumentIndex, forReplyBlock);
		}
	}
}
