// 
// CATransform3D.cs: Implements the managed CATransform3D
//
// Authors:
//   Miguel de Icaza
//     
// Copyright 2009 Novell, Inc
// Copyright 2014 Xamarin Inc
//
using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using ObjCRuntime;
using CoreGraphics;

#nullable enable

namespace CoreAnimation {

	// CATransform3D.h
	/// <summary>3D transformation.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct CATransform3D {
		public nfloat M11;
		public nfloat M12;
		public nfloat M13;
		public nfloat M14;
		public nfloat M21;
		public nfloat M22;
		public nfloat M23;
		public nfloat M24;
		public nfloat M31;
		public nfloat M32;
		public nfloat M33;
		public nfloat M34;
		public nfloat M41;
		public nfloat M42;
		public nfloat M43;
		public nfloat M44;

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		static public readonly CATransform3D Identity;

		static CATransform3D ()
		{
			Identity = new CATransform3D ();
			Identity.M11 = Identity.M22 = Identity.M33 = Identity.M44 = 1f;
		}

		[DllImport (Constants.QuartzLibrary)]
		extern static byte CATransform3DIsIdentity (CATransform3D t);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsIdentity {
			get {
				return CATransform3DIsIdentity (this) != 0;
			}
		}

		[DllImport (Constants.QuartzLibrary)]
		extern static byte CATransform3DEqualToTransform (CATransform3D a, CATransform3D b);

		/// <param name="other">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Equals (CATransform3D other)
		{
			return CATransform3DEqualToTransform (this, other) != 0;
		}

		/// <param name="other">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override bool Equals (object? other)
		{
			if (!(other is CATransform3D))
				return false;
			return CATransform3DEqualToTransform (this, (CATransform3D) other) != 0;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override int GetHashCode ()
		{
			var hash = new HashCode ();
			hash.Add (M11);
			hash.Add (M12);
			hash.Add (M13);
			hash.Add (M14);
			hash.Add (M21);
			hash.Add (M22);
			hash.Add (M23);
			hash.Add (M24);
			hash.Add (M31);
			hash.Add (M32);
			hash.Add (M33);
			hash.Add (M34);
			hash.Add (M41);
			hash.Add (M42);
			hash.Add (M43);
			hash.Add (M44);
			return hash.ToHashCode ();
		}

		// Transform matrix =  [1 0 0 0; 0 1 0 0; 0 0 1 0; tx ty tz 1]
		//[DllImport(Constants.QuartzLibrary)]
		//extern static CATransform3D CATransform3DMakeTranslation (float tx, float ty, float tz);
		public static CATransform3D MakeTranslation (nfloat tx, nfloat ty, nfloat tz)
		{
			//return CATransform3DMakeTranslation (tx, ty, tz);
			CATransform3D r = Identity;
			r.M41 = tx;
			r.M42 = ty;
			r.M43 = tz;

			return r;
		}

		// Scales matrix = [sx 0 0 0; 0 sy 0 0; 0 0 sz 0; 0 0 0 1]
		//[DllImport(Constants.QuartzLibrary)]
		//extern static CATransform3D CATransform3DMakeScale (float sx, float sy, float sz);
		public static CATransform3D MakeScale (nfloat sx, nfloat sy, nfloat sz)
		{
			CATransform3D r = Identity;
			r.M11 = sx;
			r.M22 = sy;
			r.M33 = sz;

			return r;
		}

		[DllImport (Constants.QuartzLibrary, EntryPoint = "CATransform3DMakeRotation")]
		public extern static CATransform3D MakeRotation (nfloat angle, nfloat x, nfloat y, nfloat z);

		[DllImport (Constants.QuartzLibrary)]
		extern static CATransform3D CATransform3DTranslate (CATransform3D t, nfloat tx, nfloat ty, nfloat tz);

		public CATransform3D Translate (nfloat tx, nfloat ty, nfloat tz)
		{
			return CATransform3DTranslate (this, tx, ty, tz);
		}

		[DllImport (Constants.QuartzLibrary)]
		extern static CATransform3D CATransform3DScale (CATransform3D t, nfloat sx, nfloat sy, nfloat sz);

		public CATransform3D Scale (nfloat sx, nfloat sy, nfloat sz)
		{
			return CATransform3DScale (this, sx, sy, sz);
		}
		public CATransform3D Scale (nfloat s)
		{
			return CATransform3DScale (this, s, s, s);
		}

		[DllImport (Constants.QuartzLibrary)]
		extern static CATransform3D CATransform3DRotate (CATransform3D t, nfloat angle, nfloat x, nfloat y, nfloat z);

		public CATransform3D Rotate (nfloat angle, nfloat x, nfloat y, nfloat z)
		{
			return CATransform3DRotate (this, angle, x, y, z);
		}

		[DllImport (Constants.QuartzLibrary)]
		extern static CATransform3D CATransform3DConcat (CATransform3D a, CATransform3D b);

		/// <param name="b">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CATransform3D Concat (CATransform3D b)
		{
			return CATransform3DConcat (this, b);
		}

		[DllImport (Constants.QuartzLibrary)]
		extern static CATransform3D CATransform3DInvert (CATransform3D t);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CATransform3D Invert ()
		{
			return CATransform3DInvert (this);
		}

		/// <param name="m">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[DllImport (Constants.QuartzLibrary, EntryPoint = "CATransform3DMakeAffineTransform")]
		public extern static CATransform3D MakeFromAffine (CGAffineTransform m);


		[DllImport (Constants.QuartzLibrary)]
		extern static byte CATransform3DIsAffine (CATransform3D t);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsAffine {
			get {
				return CATransform3DIsAffine (this) != 0;
			}
		}

		/// <param name="t">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[DllImport (Constants.QuartzLibrary, EntryPoint = "CATransform3DGetAffineTransform")]
		public extern static CGAffineTransform GetAffine (CATransform3D t);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			return String.Format ("[{0} {1} {2} {3}; {4} {5} {6} {7}; {8} {9} {10} {11}; {12} {13} {14} {15}]",
						  M11, M12, M13, M14,
						  M21, M22, M23, M24,
						  M31, M32, M33, M34,
						  M41, M42, M43, M44);
		}
	}
}
