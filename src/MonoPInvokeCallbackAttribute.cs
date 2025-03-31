//
// MonoPInvokeCallbackAttribute.cs: necessary for AOT ports of Mono
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
using System;

namespace ObjCRuntime {
	[AttributeUsage (AttributeTargets.Method)]
	public sealed class MonoPInvokeCallbackAttribute : Attribute {
		/// <param name="t">The type of the delegate that will be calling us back.</param>
		///         <summary>Constructor for the MonoPInvokeCallbackAttribute.</summary>
		///         <remarks>
		///           <para>
		/// 	  You must specify the type of the delegate that this code
		/// 	  will be called as.  The following example shows the scenario
		/// 	  in which this is used:
		///
		/// 	 </para>
		///           <code lang="csharp lang-csharp"><![CDATA[using System;
		/// 	delegate void DrawPatternCallback (IntPtr voidptr, IntPtr cgcontextref);
		///
		/// 	º [MonoPInvokeCallback (typeof (DrawPatternCallback))]
		/// 	static void DrawCallback (IntPtr voidptr, IntPtr cgcontextptr)
		/// 	{
		/// 		// This method is called from the C library
		/// 	}
		/// ]]></code>
		///         </remarks>
		public MonoPInvokeCallbackAttribute (Type t)
		{
			DelegateType = t;
		}

		/// <summary>The type of the delegate that will be calling us back.</summary>
		///         <value>The type of the delegate that will be calling us back.</value>
		///         <remarks>
		///         </remarks>
		public Type DelegateType { get; set; }
	}
}
