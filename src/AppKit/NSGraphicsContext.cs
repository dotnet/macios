//
// Copyright 2012-2013 Xamarin Inc.
//
// Authors:
//   Miguel de Icaza (miguel@gnome.org)
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
//

#if !__MACCATALYST__

#nullable enable

using CoreGraphics;

#nullable enable

namespace AppKit {
	public partial class NSGraphicsContext {
		/// <summary>Creates a graphics context for the specified Core Graphics context.</summary>
		/// <param name="context">The Core Graphics context to draw into.</param>
		/// <param name="initialFlippedState">Whether the coordinate system of the new graphics context is flipped.</param>
		/// <returns>A new graphics context that draws into <paramref name="context" />.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context" /> is <see langword="null" />.</exception>
		/// <remarks>
		///   <para>This method is obsolete. Use <see cref="FromCGContext(CGContext,bool)" /> instead.</para>
		/// </remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.14", "Use 'FromCGContext' instead.")]
		public static NSGraphicsContext FromGraphicsPort (CGContext context, bool initialFlippedState)
		{
			if (context is null)
				throw new ArgumentNullException (nameof (context));
			NSGraphicsContext result = FromGraphicsPort (context.Handle, initialFlippedState);
			GC.KeepAlive (context);
			return result;
		}

		/// <summary>Gets the underlying Core Graphics context.</summary>
		/// <value>The Core Graphics context this graphics context draws into.</value>
		/// <remarks>
		///   <para>This property is obsolete. Use <see cref="CGContext" /> instead.</para>
		/// </remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.14", "Use 'CGContext' instead.")]
		public virtual CGContext GraphicsPort {
			get { return new CGContext (GraphicsPortHandle, false); }
		}
	}
}
#endif // !__MACCATALYST__
