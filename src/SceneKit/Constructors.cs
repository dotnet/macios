//
// Constructors.cs: Provides various constructors, since SceneKit seems
// to be moving away from the standard instance method for constructors
// and instead uses static methods
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2012 Xamarin Inc
//

#nullable enable

namespace SceneKit {

	public partial class SCNText {
		/// <summary>Creates a new <see cref="SCNText" /> geometry from the specified string and extrusion depth.</summary>
		/// <param name="str">The text string to render.</param>
		/// <param name="extrusionDepth">The extrusion depth.</param>
		public static SCNText Create (string str, nfloat extrusionDepth)
		{
			using (var tmp = new NSString (str))
				return Create ((NSObject) tmp, extrusionDepth);
		}

		/// <summary>Creates a new <see cref="SCNText" /> geometry from the specified attributed string and extrusion depth.</summary>
		/// <param name="attributedString">The attributed string to render.</param>
		/// <param name="extrusionDepth">The extrusion depth.</param>
		public static SCNText Create (NSAttributedString attributedString, nfloat extrusionDepth)
		{
			return Create ((NSObject) attributedString, extrusionDepth);
		}
	}
}
