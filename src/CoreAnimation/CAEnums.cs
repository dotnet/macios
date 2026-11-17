//
// CAEnums.cs: definitions used for CoreAnimation
// 
// Authors:
//   Geoff Norton
//   Miguel de Icaza
//
// Copyright 2009-2010, Novell, Inc.
// Copyright 2014 Xamarin Inc.
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
using CoreGraphics;

#nullable enable

namespace CoreAnimation {

	// untyped enum -> CALayer.h
	// note: edgeAntialiasingMask is an `unsigned int` @property
	/// <summary>Flags used to determine what side of a layer should be antialiased.</summary>
	[Flags]
	public enum CAEdgeAntialiasingMask : uint {
		/// <summary>Left side of the layer is antialiased.</summary>
		LeftEdge = 1 << 0,
		/// <summary>Right side of the layer is antialiased.</summary>
		RightEdge = 1 << 1,
		/// <summary>Bottom side of the layer is antialiased.</summary>
		BottomEdge = 1 << 2,
		/// <summary>Top side of the layer is antialiased.</summary>
		TopEdge = 1 << 3,
		/// <summary>All sides of the layer are antialiased.</summary>
		All = LeftEdge | RightEdge | BottomEdge | TopEdge,
		/// <summary>Left and right sides of the layer are antialiased.</summary>
		LeftRightEdges = LeftEdge | RightEdge,
		/// <summary>Top and bottom side of the layer are antialiased.</summary>
		TopBottomEdges = TopEdge | BottomEdge,
	}

	/// <summary>Specifies which corners of a <see cref="CALayer" /> are rounded.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum CACornerMask : ulong {
		/// <summary>Rounds the corner at the minimum x- and minimum y-coordinates.</summary>
		MinXMinYCorner = 1 << 0,
		/// <summary>Rounds the corner at the maximum x- and minimum y-coordinates.</summary>
		MaxXMinYCorner = 1 << 1,
		/// <summary>Rounds the corner at the minimum x- and maximum y-coordinates.</summary>
		MinXMaxYCorner = 1 << 2,
		/// <summary>Rounds the corner at the maximum x- and maximum y-coordinates.</summary>
		MaxXMaxYCorner = 1 << 3,
	}

	// untyped enum -> CALayer.h (only on OSX headers)
	// note: autoresizingMask is an `unsigned int` @property
	/// <summary>Specifies how a layer adjusts its frame when its superlayer's bounds change.</summary>
	[Flags]
	[NoiOS]
	[NoTV]
	[MacCatalyst (13, 1)]
	public enum CAAutoresizingMask : uint {
		/// <summary>Does not automatically resize the layer or adjust its margins.</summary>
		NotSizable = 0,
		/// <summary>Allows the minimum x-coordinate margin to resize.</summary>
		MinXMargin = 1 << 0,
		/// <summary>Allows the layer's width to resize.</summary>
		WidthSizable = 1 << 1,
		/// <summary>Allows the maximum x-coordinate margin to resize.</summary>
		MaxXMargin = 1 << 2,
		/// <summary>Allows the minimum y-coordinate margin to resize.</summary>
		MinYMargin = 1 << 3,
		/// <summary>Allows the layer's height to resize.</summary>
		HeightSizable = 1 << 4,
		/// <summary>Allows the maximum y-coordinate margin to resize.</summary>
		MaxYMargin = 1 << 5,
	}

	// typedef int -> CAConstraintLayoutManager.h
	/// <summary>Specifies the geometric attribute of a layer used in a <see cref="CAConstraint" />.</summary>
	[NoiOS]
	[NoTV]
	[MacCatalyst (13, 1)]
	public enum CAConstraintAttribute {
		/// <summary>Specifies the minimum x-coordinate of a layer's frame.</summary>
		MinX,
		/// <summary>Specifies the midpoint of a layer's frame along the x-axis.</summary>
		MidX,
		/// <summary>Specifies the maximum x-coordinate of a layer's frame.</summary>
		MaxX,
		/// <summary>Specifies the width of a layer's frame.</summary>
		Width,
		/// <summary>Specifies the minimum y-coordinate of a layer's frame.</summary>
		MinY,
		/// <summary>Specifies the midpoint of a layer's frame along the y-axis.</summary>
		MidY,
		/// <summary>Specifies the maximum y-coordinate of a layer's frame.</summary>
		MaxY,
		/// <summary>Specifies the height of a layer's frame.</summary>
		Height,
	};
}
