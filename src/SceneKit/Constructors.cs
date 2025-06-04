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
using System;

using Foundation;
using ObjCRuntime;

#nullable enable

namespace SceneKit {

	public partial class SCNText {
		/// <param name="str">To be added.</param>
		/// <param name="extrusionDepth">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public static SCNText Create (string str, nfloat extrusionDepth)
		{
			using (var tmp = new NSString (str))
				return Create ((NSObject) tmp, extrusionDepth);
		}

		/// <param name="attributedString">To be added.</param>
		/// <param name="extrusionDepth">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public static SCNText Create (NSAttributedString attributedString, nfloat extrusionDepth)
		{
			return Create ((NSObject) attributedString, extrusionDepth);
		}
	}
}
