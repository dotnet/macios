//
// Copyright 2009-2010, Novell, Inc.
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

namespace Foundation {
	/// <summary>An convenience exception used in Model method implementations.</summary>
	///     <remarks>To be added.</remarks>
	public class ModelNotImplementedException : Exception {
		/// <summary>Default constructor.</summary>
		///         <remarks>To be added.</remarks>
		public ModelNotImplementedException () { }
	}

	/// <summary>This class exits purely as a warning to future generations.   You called a method using “base”, but this was not required.</summary>
	///     <remarks>To be added.</remarks>
	public class You_Should_Not_Call_base_In_This_Method : Exception {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public You_Should_Not_Call_base_In_This_Method () { }
	}
}
