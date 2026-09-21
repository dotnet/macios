//
// Copyright 2016, Xamarin, Inc.
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

namespace MediaLibrary {
	/// <summary>A bitmask specifying the kinds of media provided by a media source.</summary>
	[Native]
	public enum MLMediaSourceType : ulong {
		/// <summary>The source provides audio media.</summary>
		Audio = 1 << 0,
		/// <summary>The source provides image media.</summary>
		Image = 1 << 1,
		/// <summary>The source provides movie media.</summary>
		Movie = 1 << 2,
	}

	/// <summary>A bitmask specifying one or more kinds of media objects.</summary>
	[Native]
	public enum MLMediaType : ulong {
		/// <summary>Audio media.</summary>
		Audio = 1 << 0,
		/// <summary>Image media.</summary>
		Image = 1 << 1,
		/// <summary>Movie media.</summary>
		Movie = 1 << 2,
	}
}
