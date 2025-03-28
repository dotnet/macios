//
// ModelIO/MIEnums.cs: enumerations and definitions
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2015 Xamarin, Inc.
//
//
using System;
using System.Runtime.InteropServices;
using Foundation;
using CoreFoundation;
using CoreGraphics;
using Metal;
using ObjCRuntime;

#nullable enable

namespace ModelIO {
	/// <summary>Enumerates vertex data descriptions.</summary>
	[Native]
	public enum MDLVertexFormat : ulong {
		/// <summary>Indicates an invalid format.</summary>
		Invalid = 0,

		/// <summary>Indicates a packed vector format.</summary>
		PackedBits = 0x1000,
		/// <summary>Indicates unsigned two's complement 8-bit values.</summary>
		UCharBits = 0x10000,
		/// <summary>Indicate 8-bit signed integer values.</summary>
		CharBits = 0x20000,
		/// <summary>Indicates normalized unsigned two's complement 8-bit values.</summary>
		UCharNormalizedBits = 0x30000,
		/// <summary>Indicates a normalized 8-bit value.</summary>
		CharNormalizedBits = 0x40000,
		/// <summary>Indicates 16-bit unsigned two's complement value.</summary>
		UShortBits = 0x50000,
		/// <summary>Indicates 16-bit signed two's complement values.</summary>
		ShortBits = 0x60000,
		/// <summary>Indicates 16-bit normalized unsigned two's complement value.</summary>
		UShortNormalizedBits = 0x70000,
		/// <summary>Indicates 16-bit signed two's complement values.</summary>
		ShortNormalizedBits = 0x80000,
		/// <summary>Indicates 32-bit unsigned integer values.</summary>
		UIntBits = 0x90000,
		/// <summary>Indicates 32-bit two's complement values.</summary>
		IntBits = 0xA0000,
		/// <summary>Indicates half-precision floating point values.</summary>
		HalfBits = 0xB0000,
		/// <summary>Indicates single-precision floating point values.</summary>
		FloatBits = 0xC0000,

		/// <summary>Indicates one unsigned two's complement 8-bit value.</summary>
		UChar = UCharBits | 1,
		/// <summary>Indicates two unsigned two's complement 8-bit values.</summary>
		UChar2 = UCharBits | 2,
		/// <summary>Indicates three unsigned two's complement 8-bit values.</summary>
		UChar3 = UCharBits | 3,
		/// <summary>Indicates four unsigned two's complement 8-bit values.</summary>
		UChar4 = UCharBits | 4,

		/// <summary>Indicates a signed two's complement 8-bit value.</summary>
		Char = CharBits | 1,
		/// <summary>Indicates two signed two's complement 8-bit values.</summary>
		Char2 = CharBits | 2,
		/// <summary>Indicates three signed two's complement 8-bit values.</summary>
		Char3 = CharBits | 3,
		/// <summary>Indicates four signed two's complement 8-bit values.</summary>
		Char4 = CharBits | 4,

		/// <summary>Indicates one normalized unsigned two's complement 8-bit values.</summary>
		UCharNormalized = UCharNormalizedBits | 1,
		/// <summary>Indicates two normalized unsigned two's complement 8-bit values.</summary>
		UChar2Normalized = UCharNormalizedBits | 2,
		/// <summary>Indicates three normalized unsigned two's complement 8-bit values.</summary>
		UChar3Normalized = UCharNormalizedBits | 3,
		/// <summary>Indicates four normalized unsigned two's complement 8-bit values.</summary>
		UChar4Normalized = UCharNormalizedBits | 4,

		/// <summary>Indicates a normalized 8-bit value.</summary>
		CharNormalized = CharNormalizedBits | 1,
		/// <summary>Indicates two normalized 8-bit values.</summary>
		Char2Normalized = CharNormalizedBits | 2,
		/// <summary>Indicates three normalized 8-bit values.</summary>
		Char3Normalized = CharNormalizedBits | 3,
		/// <summary>Indicates four normalized 8-bit values.</summary>
		Char4Normalized = CharNormalizedBits | 4,

		/// <summary>Indicates one 16-bit signed two's complement values.</summary>
		UShort = UShortBits | 1,
		/// <summary>Indicates two 16-bit unsigned two's complement values.</summary>
		UShort2 = UShortBits | 2,
		/// <summary>Indicates three 16-bit unsigned two's complement values.</summary>
		UShort3 = UShortBits | 3,
		/// <summary>Indicates four 16-bit unsigned two's complement values.</summary>
		UShort4 = UShortBits | 4,

		/// <summary>Indicates one 16-bit signed two's complement value.</summary>
		Short = ShortBits | 1,
		/// <summary>Indicates two 16-bit signed two's complement values.</summary>
		Short2 = ShortBits | 2,
		/// <summary>Indicates three 16-bit signed two's complement values.</summary>
		Short3 = ShortBits | 3,
		/// <summary>Indicates four 16-bit signed two's complement values.</summary>
		Short4 = ShortBits | 4,

		/// <summary>Indicates one 16-bit normalized unsigned two's complement value.</summary>
		UShortNormalized = UShortNormalizedBits | 1,
		/// <summary>Indicates two 16-bit normalized unsigned two's complement values.</summary>
		UShort2Normalized = UShortNormalizedBits | 2,
		/// <summary>Indicates three 16-bit normalized unsigned two's complement values.</summary>
		UShort3Normalized = UShortNormalizedBits | 3,
		/// <summary>Indicates four 16-bit normalized unsigned two's complement values.</summary>
		UShort4Normalized = UShortNormalizedBits | 4,

		/// <summary>Indicates one normalized 16-bit signed two's complement value.</summary>
		ShortNormalized = ShortNormalizedBits | 1,
		/// <summary>Indicates two normalized 16-bit signed two's complement values.</summary>
		Short2Normalized = ShortNormalizedBits | 2,
		/// <summary>Indicates three normalized 16-bit signed two's complement values.</summary>
		Short3Normalized = ShortNormalizedBits | 3,
		/// <summary>Indicates four normalized 16-bit signed two's complement values.</summary>
		Short4Normalized = ShortNormalizedBits | 4,

		/// <summary>Indicates one 32-bit unsigned integer value.</summary>
		UInt = UIntBits | 1,
		/// <summary>Indicates two 32-bit unsigned integer values.</summary>
		UInt2 = UIntBits | 2,
		/// <summary>Indicates three 32-bit unsigned integer values.</summary>
		UInt3 = UIntBits | 3,
		/// <summary>Indicates four 32-bit unsigned integer values.</summary>
		UInt4 = UIntBits | 4,

		/// <summary>Indicates one 32-bit two's complement value.</summary>
		Int = IntBits | 1,
		/// <summary>Indicates two 32-bit two's complement values.</summary>
		Int2 = IntBits | 2,
		/// <summary>Indicates three 32-bit two's complement values.</summary>
		Int3 = IntBits | 3,
		/// <summary>Indicates four 32-bit two's complement values.</summary>
		Int4 = IntBits | 4,

		/// <summary>Indicates one half-precision floating point value.</summary>
		Half = HalfBits | 1,
		/// <summary>Indicates two half-precision floating point values.</summary>
		Half2 = HalfBits | 2,
		/// <summary>Indicates three half-precision floating point values.</summary>
		Half3 = HalfBits | 3,
		/// <summary>Indicates four half-precision floating point values.</summary>
		Half4 = HalfBits | 4,

		/// <summary>Indicates one single-precision floating point value.</summary>
		Float = FloatBits | 1,
		/// <summary>Indicates two single-precision floating point values.</summary>
		Float2 = FloatBits | 2,
		/// <summary>Indicates three single-precision floating point values.</summary>
		Float3 = FloatBits | 3,
		/// <summary>Indicates four single-precision floating point values.</summary>
		Float4 = FloatBits | 4,

		/// <summary>Indicates a packed 32-bit value with four signed two's complement integers arranged 10/10/10/2.</summary>
		Int1010102Normalized = IntBits | PackedBits | 4,
		/// <summary>Indicates a packed 32-bit value with four unsigned two's complement integers arranged 10/10/10/2.</summary>
		UInt1010102Normalized = UIntBits | PackedBits | 4,
	}

	/// <summary>Enumerates mesh buffer data types.</summary>
	[Native]
	public enum MDLMeshBufferType : ulong {
		/// <summary>Indicates a vertex buffer.</summary>
		Vertex = 1,
		/// <summary>Indicates an index buffer for vertex attributes.</summary>
		Index = 2,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		Custom = 3,
	}

	/// <summary>Enumerates the geometric primitives to use for rendering.</summary>
	[Native]
	public enum MDLGeometryType : long {
		/// <summary>Indicates that each index describes a point.</summary>
		Points = 0,
		/// <summary>Indicates that consecutive index pairs describe lines.</summary>
		Lines,
		/// <summary>Indicates that each 3-index stride in the buffer specifies a triangle.</summary>
		Triangles,
		/// <summary>Indicates that the first 3-index stride in the buffer specifies a triangle, and that successive points specify another triangle with the preceding two indices </summary>
		TriangleStrips,
		/// <summary>Indicates that each 4-index stride in the buffer specifies a quadrilateral.</summary>
		Quads,
		/// <summary>Indicates that the geometry is not uniform, and is described by its <see cref="P:ModelIO.MDLSubmesh.Topology" /> property.</summary>
		VariableTopology,
	}

	/// <summary>Enumerates bit depths for <see cref="T:ModelIO.MDLSubmesh" /> index buffers.</summary>
	[Native]
	public enum MDLIndexBitDepth : ulong {
		/// <summary>The index buffer has not been initialized.</summary>
		Invalid,
		/// <summary>Each index is an unsigned 8-bit integer.</summary>
		UInt8 = 8,
		/// <summary>Each index is an unsigned 16-bit integer.</summary>
		UInt16 = 16,
		/// <summary>Each index is an unsigned 32-bit integer.</summary>
		UInt32 = 32,
	}

	/// <summary>Enumerates the semantics of an <see cref="T:ModelIO.MDLMaterialProperty" />.</summary>
	[Native]
	public enum MDLMaterialSemantic : ulong {
		/// <summary>The property represents the base color of a surface.</summary>
		BaseColor = 0,
		/// <summary>The property represents the degree of subsurface light penetration.</summary>
		Subsurface,
		/// <summary>The property represents the degree that a surface appears metallic.</summary>
		Metallic,
		/// <summary>The property represents the intensity of specular highligths.</summary>
		Specular,
		/// <summary>The property represents the Blinn-Phong exponent.</summary>
		SpecularExponent,
		/// <summary>The property represents the balance between light color and surface color for specular highligts.</summary>
		SpecularTint,
		/// <summary>The property represents the roughness of the material.</summary>
		Roughness,
		/// <summary>The property represents the degree of elongation in the tangential direction.</summary>
		Anisotropic,
		/// <summary>The property represents the angle of anisotropy relative to the tangent direction. <c>[0.0, 1.0]</c> maps to <c>[0.0, 2*PI] radians</c>. </summary>
		AnisotropicRotation,
		/// <summary>The property represents the intensity of glancing highligths.</summary>
		Sheen,
		/// <summary>The property represents the tint of glancing highligths.</summary>
		SheenTint,
		/// <summary>The property represents the intensity of added specular highlights.</summary>
		Clearcoat,
		/// <summary>The property represents the spread of added specular highlights.</summary>
		ClearcoatGloss,
		/// <summary>The property represents the color of surface radiance.</summary>
		Emission,
		/// <summary>The property represents the magnitude of the bump perturbation of a surface.</summary>
		Bump,
		/// <summary>The property represents the opacity of the material.</summary>
		Opacity,
		/// <summary>The property represents the n1 term in Schlick's approximation of the Fresnel factor in specular relection.</summary>
		InterfaceIndexOfRefraction,
		/// <summary>The property represents the n2 term in Schlick's approximation of the Fresnel factor in specular relection.</summary>
		MaterialIndexOfRefraction,
		/// <summary>The property represents the variation in normal vectors for a material, in the tangent coordinate system.</summary>
		ObjectSpaceNormal,
		/// <summary>The property represents the variation in normal vectors for a material, in the tangent coordinate system.</summary>
		TangentSpaceNormal,
		/// <summary>The property represents the displacement of a surface material in the normal direction.</summary>
		Displacement,
		/// <summary>The property represents the magnitude of the displacement of a surface material in the normal direction.</summary>
		DisplacementScale,
		/// <summary>The property represents the reduction in ambient light due to neighboring geometry on a surface.</summary>
		AmbientOcclusion,
		/// <summary>The property represents the scaling factor for ambient occlusion.</summary>
		AmbientOcclusionScale,
		/// <summary>The property's semantics have not been set.</summary>
		None = 0x8000,
		/// <summary>The property represents a user-defined semantic.</summary>
		UserDefined = 0x8001,
	}

	/// <summary>Enumerates material property types.</summary>
	[Native]
	public enum MDLMaterialPropertyType : ulong {
		/// <summary>Indicates an uninitialized property.</summary>
		None,
		/// <summary>Indicates a property that contains a string.</summary>
		String,
		/// <summary>Indicates a property that contains a URL that typically addresses a texture.</summary>
		Url,
		/// <summary>Indicates a property that contains a texture.</summary>
		Texture,
		/// <summary>Indicates a property that specifies a color.</summary>
		Color,
		/// <summary>Indicates a property that contains a floating-point value.</summary>
		Float,
		/// <summary>Indicates a vector property that contains two floating-point values.</summary>
		Float2,
		/// <summary>Indicates a vector property that contains three floating-point values.</summary>
		Float3,
		/// <summary>Indicates a vector property that contains four floating-point values.</summary>
		Float4,
		/// <summary>Indicates a property that contains a 4x4 matrix of floating-point values.</summary>
		Matrix44,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		Buffer,
	}

	/// <summary>Enumerates procedures for handling texture coordinates outside of the range <c>[0.0,1.0]</c>.</summary>
	[Native]
	public enum MDLMaterialTextureWrapMode : ulong {
		/// <summary>Clamp coordinates outside the range <c>[0.0,1.0]</c> to <c>0.0</c> if low, or <c>1.0</c> if high.</summary>
		Clamp,
		/// <summary>Use only the fractional part of the coordinate.</summary>
		Repeat,
		/// <summary>Reflect coordinates outside the range <c>[0.0,1.0]</c> so that, for example, <c>1.3</c> maps to <c>0.7</c>, and <c>2.3</c> maps to <c>0.3</c>.</summary>
		Mirror,
	}

	/// <summary>Enumerates values that control how to sample between texels.</summary>
	[Native]
	public enum MDLMaterialTextureFilterMode : ulong {
		/// <summary>Indicates that the nearest neighbor's value should be used.</summary>
		Nearest,
		/// <summary>Indicates linear interopolation.</summary>
		Linear,
	}

	/// <summary>Enumerates values that control texture sampling between mipmap levels.</summary>
	[Native]
	public enum MDLMaterialMipMapFilterMode : ulong {
		/// <summary>Indicates selection of mipmap values nearest to the desired level.</summary>
		Nearest,
		/// <summary>Indicates linear interpolation between mipmap levels.</summary>
		Linear,
	}

	/// <summary>Enumerates values that specify data types and sizes for texel channels.</summary>
	[Native]
	public enum MDLTextureChannelEncoding : long {
		/// <summary>Indicates that each channel is an unsigned 8-bit integer.</summary>
		UInt8 = 1,
		/// <summary>Indicates that each channel is an unsigned 16-bit integer.</summary>
		UInt16 = 2,
		/// <summary>Indicates that each channel is an unsigned 24-bit integer.</summary>
		UInt24 = 3,
		/// <summary>Indicates that each channel is an unsigned 32-bit integer.</summary>
		UInt32 = 4,
		/// <summary>Indicates that each channel is a 16-bit floating-point number.</summary>
		Float16 = 258,
		/// <summary>To be added.</summary>
		Float16SR = 770,
		/// <summary>Indicates that each channel is a 32-bit floating-point number.</summary>
		Float32 = 260,
	}

	/// <summary>Enumerates the types of <see cref="T:ModelIO.MDLLight" />.</summary>
	[Native]
	public enum MDLLightType : ulong {
		/// <summary>An unknown light type, or an uninitialized light.</summary>
		Unknown = 0,
		/// <summary>A nondirectional, diffuse light that is not dependent on its location.</summary>
		Ambient,
		/// <summary>A directional light that is not dependent on its location.</summary>
		Directional,
		/// <summary>A light source that shines in a specific direction from a specific place.</summary>
		Spot,
		/// <summary>A point light that shines in all directions.</summary>
		Point,
		/// <summary>A line of light that shines in all directions.</summary>
		Linear,
		/// <summary>A disc-shaped light that shines in all directions.</summary>
		DiscArea,
		/// <summary>A rectangular light that shines in all directions.</summary>
		RectangularArea,
		/// <summary>A super-ellipse-shaped light that shines in all directions</summary>
		SuperElliptical,
		/// <summary>A light that is an instance of <see cref="T:ModelIO.MDLPhotometricLight" />.</summary>
		Photometric,
		/// <summary>A light that is shone through a cube map.</summary>
		Probe,
		/// <summary>A light that is generated by an environment light texture map.</summary>
		Environment,
	}

	/// <summary>Enumerates camera projections.</summary>
	[Native]
	public enum MDLCameraProjection : ulong {
		/// <summary>To be added.</summary>
		Perspective = 0,
		/// <summary>To be added.</summary>
		Orthographic = 1,
	}

	[Native]
	public enum MDLMaterialFace : ulong {
		/// <summary>To be added.</summary>
		Front = 0,
		/// <summary>To be added.</summary>
		Back,
		/// <summary>To be added.</summary>
		DoubleSided,
	}

	[Native]
	public enum MDLProbePlacement : long {
		/// <summary>To be added.</summary>
		UniformGrid = 0,
		/// <summary>To be added.</summary>
		IrradianceDistribution,
	}

	[MacCatalyst (13, 1)]
	public enum MDLNoiseTextureType {
		/// <summary>To be added.</summary>
		Vector,
		/// <summary>To be added.</summary>
		Cellular,
	}
}
