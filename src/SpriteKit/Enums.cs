//
// Enums.cs: enums for SpriteKit
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)
//
// Copyright 2013-2014 Xamarin Inc

using ObjCRuntime;

#nullable enable

namespace SpriteKit {

	// NSInteger -> SKKeyframeSequence.h
	/// <summary>An enumeration whose values specify the interpolation mode of a <see cref="T:SpriteKit.SKKeyframeSequence" />.</summary>
	[Native]
	public enum SKInterpolationMode : long {
		/// <summary>Interpolate values linearly.</summary>
		Linear = 1,
		/// <summary>Interpolate values with a B-spline.</summary>
		Spline = 2,
		/// <summary>Do not interpolate values. (Use the value at the latest time that is before the current time.)</summary>
		Step = 3,
	}

	// NSInteger -> SKKeyframeSequence.h
	/// <summary>An enumeration whose values specify whether the time value of a <see cref="T:SpriteKit.SKKeyframeSequence" /> should cycle.</summary>
	[Native]
	public enum SKRepeatMode : long {
		/// <summary>After the last keyframe's time sequence is past, the last keyframe's value is returned.</summary>
		Clamp = 1,
		/// <summary>After the last keyframe's time sequence is past, the sequence loops back to the first keyframe.</summary>
		Loop = 2,
	}

	// NSInteger -> SKAction.h
	/// <summary>An enumeration whose values specify the time-varying behavior of a <see cref="T:SpriteKit.SKAction" />. Used with <see cref="P:SpriteKit.SKAction.TimingMode" />.</summary>
	[Native]
	public enum SKActionTimingMode : long {
		/// <summary>Render the animation with no easing.</summary>
		Linear = 0,
		/// <summary>Ease the animation in.</summary>
		EaseIn = 1,
		/// <summary>Ease the animation out.</summary>
		EaseOut = 2,
		/// <summary>Ease the animation in and out.</summary>
		EaseInEaseOut = 3,
	}

	// NSInteger -> SKLabelNode.h
	/// <summary>
	///  An enumeration whose values specify vertical alignment of a <see cref="T:SpriteKit.SKLabelNode" />. Used with <see cref="P:SpriteKit.SKLabelNode.VerticalAlignmentMode" /></summary>
	[Native]
	public enum SKLabelVerticalAlignmentMode : long {
		/// <summary>Place the baseline at the node's origin.</summary>
		Baseline = 0,
		/// <summary>Place the text at the vertical center of the node.</summary>
		Center = 1,
		/// <summary>Place the top of the text at the node's origin.</summary>
		Top = 2,
		/// <summary>Place the bottom of the text at the node's origin.</summary>
		Bottom = 3,
	}

	// NSInteger -> SKLabelNode.h
	/// <summary>An enumeration whose values specify horizontal alignment of a <see cref="T:SpriteKit.SKLabelNode" />. Used with <see cref="P:SpriteKit.SKLabelNode.HorizontalAlignmentMode" /></summary>
	[Native]
	public enum SKLabelHorizontalAlignmentMode : long {
		/// <summary>Place the text in the horizontal center of the node.</summary>
		Center = 0,
		/// <summary>Place the left edge of the text at the node's origin.</summary>
		Left = 1,
		/// <summary>Place the right edge of the text at the node's origin.</summary>
		Right = 2,
	}

	// NSInteger -> SKNode.h
	/// <summary>An enumeration whose values specify options for blending of visual <see cref="T:SpriteKit.SKNode" />s or particles.</summary>
	[Native]
	public enum SKBlendMode : long {
		/// <summary>The alpha value of the source color is used to blend the colors.</summary>
		Alpha = 0,
		/// <summary>The colors are added.</summary>
		Add = 1,
		/// <summary>The source color is subtracted from the destination.</summary>
		Subtract = 2,
		/// <summary>The colors are multiplied.</summary>
		Multiply = 3,
		/// <summary>The colors are multiplied and then doubled.</summary>
		MultiplyX2 = 4,
		/// <summary>The inverted source color is multiplied by the destination color, and the source color is added to the result..</summary>
		Screen = 5,
		/// <summary>The source color is used.</summary>
		Replace = 6,
		/// <summary>To be added.</summary>
		MultiplyAlpha = 7,
	}

	// NSInteger -> SKScene.h
	/// <summary>An enumeration whose values specify the way in which a <see cref="T:SpriteKit.SKScene" /> scales to the view in which it is being displayed.</summary>
	[Native]
	public enum SKSceneScaleMode : long {
		/// <summary>Stretch the scene to fill the view.</summary>
		Fill = 0,
		/// <summary>Scale the scene to fill the view, even if the scene must be cropped.</summary>
		AspectFill = 1,
		/// <summary>Scale the scene to fit the view, possibly letterboxing the scene.</summary>
		AspectFit = 2,
		/// <summary>Resize the scene so that it is the same size and shape as the view.</summary>
		ResizeFill = 3,
	}

	// NSInteger -> SKTexture.h
	/// <summary>An enumeration whose values specify how a <see cref="T:SpriteKit.SKTexture" /> is rendered on a <see cref="T:SpriteKit.SKSpriteNode" /> of a different size.</summary>
	[Native]
	public enum SKTextureFilteringMode : long {
		/// <summary>The pixel is calculated using the nearest point in the texture. Faster, lower quality.</summary>
		Nearest = 0,
		/// <summary>The pixel is calculated using a linear filter of the nearby texture pixels. Slower, higher quality.</summary>
		Linear = 1,
	}

	// NSInteger -> SKTransition.h
	/// <summary>An enumeration of directions for use with <see cref="T:SpriteKit.SKTransition" />s.</summary>
	[Native]
	public enum SKTransitionDirection : long {
		Up = 0,
		Down = 1,
		Right = 2,
		Left = 3,
	}

	/// <summary>Contains values that describe the data with which an <see cref="T:SpriteKit.SKUniform" /> was initialized.</summary>
	[Native]
	public enum SKUniformType : long {
		/// <summary>The <see cref="T:SpriteKit.SKUniform" /> has not been initialized.</summary>
		None,
		/// <summary>The <see cref="T:SpriteKit.SKUniform" /> was initialized with a float.</summary>
		Float,
		/// <summary>The <see cref="T:SpriteKit.SKUniform" /> was initialized with a vector that contains 2 floats.</summary>
		FloatVector2,
		/// <summary>The <see cref="T:SpriteKit.SKUniform" /> was initialized with a vector that contains 3 floats.</summary>
		FloatVector3,
		/// <summary>The <see cref="T:SpriteKit.SKUniform" /> was initialized with a vector that contains 4 floats.</summary>
		FloatVector4,
		/// <summary>The <see cref="T:SpriteKit.SKUniform" /> was initialized with a 2x2 array of floats.</summary>
		FloatMatrix2,
		/// <summary>The <see cref="T:SpriteKit.SKUniform" /> was initialized with a 3x3 array of floats.</summary>
		FloatMatrix3,
		/// <summary>The <see cref="T:SpriteKit.SKUniform" /> was initialized with a 4x4 array of floats.</summary>
		FloatMatrix4,
		/// <summary>The <see cref="T:SpriteKit.SKUniform" /> contains texture data.</summary>
		Texture,
	}

	/// <summary>Enumerates values used with <see cref="P:SpriteKit.SKEmitterNode.ParticleRenderOrder" />.</summary>
	[Native]
	public enum SKParticleRenderOrder : ulong {
		/// <summary>The oldest particles are rendered last.</summary>
		OldestLast,
		/// <summary>The oldest particles are rendered first.</summary>
		OldestFirst,
		/// <summary>Sprite Kit may render the particles in whichever order is best for performance.</summary>
		DontCare,
	}

	/// <summary>Enumeration of valid types for <see cref="T:SpriteKit.SKAttribute" /> values.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SKAttributeType : long {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Float = 1,
		/// <summary>To be added.</summary>
		VectorFloat2 = 2,
		/// <summary>To be added.</summary>
		VectorFloat3 = 3,
		/// <summary>To be added.</summary>
		VectorFloat4 = 4,
		/// <summary>To be added.</summary>
		HalfFloat = 5,
		/// <summary>To be added.</summary>
		VectorHalfFloat2 = 6,
		/// <summary>To be added.</summary>
		VectorHalfFloat3 = 7,
		/// <summary>To be added.</summary>
		VectorHalfFloat4 = 8,
	}

	/// <summary>Enumerates how a <see cref="T:SpriteKit.SKTileDefinition" /> kind may be rotated.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SKTileDefinitionRotation : ulong {
		/// <summary>To be added.</summary>
		Angle0 = 0,
		/// <summary>To be added.</summary>
		Angle90,
		Angle180,
		Angle270,
	}

	/// <summary>Enumerates supported tiling schemes.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SKTileSetType : ulong {
		Grid,
		Isometric,
		HexagonalFlat,
		HexagonalPointy,
	}

	/// <summary>Enumerates how neighboring tiles may be automatically placed.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SKTileAdjacencyMask : ulong {
		/// <summary>To be added.</summary>
		Up = 1 << 0,
		/// <summary>To be added.</summary>
		UpperRight = 1 << 1,
		/// <summary>To be added.</summary>
		Right = 1 << 2,
		/// <summary>To be added.</summary>
		LowerRight = 1 << 3,
		/// <summary>To be added.</summary>
		Down = 1 << 4,
		/// <summary>To be added.</summary>
		LowerLeft = 1 << 5,
		/// <summary>To be added.</summary>
		Left = 1 << 6,
		/// <summary>To be added.</summary>
		UpperLeft = 1 << 7,
		/// <summary>To be added.</summary>
		All = Up | UpperRight | Right | LowerRight | Down | LowerLeft | Left | UpperLeft,
		/// <summary>To be added.</summary>
		HexFlatUp = 1 << 0,
		/// <summary>To be added.</summary>
		HexFlatUpperRight = 1 << 1,
		/// <summary>To be added.</summary>
		HexFlatLowerRight = 1 << 2,
		/// <summary>To be added.</summary>
		HexFlatDown = 1 << 3,
		/// <summary>To be added.</summary>
		HexFlatLowerLeft = 1 << 4,
		/// <summary>To be added.</summary>
		HexFlatUpperLeft = 1 << 5,
		/// <summary>To be added.</summary>
		HexFlatAll = HexFlatUp | HexFlatUpperRight | HexFlatLowerRight | HexFlatDown | HexFlatLowerLeft | HexFlatUpperLeft,
		/// <summary>To be added.</summary>
		HexPointyUpperLeft = 1 << 0,
		/// <summary>To be added.</summary>
		HexPointyUpperRight = 1 << 1,
		/// <summary>To be added.</summary>
		HexPointyRight = 1 << 2,
		/// <summary>To be added.</summary>
		HexPointyLowerRight = 1 << 3,
		/// <summary>To be added.</summary>
		HexPointyLowerLeft = 1 << 4,
		/// <summary>To be added.</summary>
		HexPointyLeft = 1 << 5,
		/// <summary>To be added.</summary>
		HexPointyAll = HexPointyUpperLeft | HexPointyUpperRight | HexPointyRight | HexPointyLowerRight | HexPointyLowerLeft | HexPointyLeft,
		/// <summary>To be added.</summary>
		UpEdge = Right | LowerRight | Down | LowerLeft | Left,
		/// <summary>To be added.</summary>
		UpperRightEdge = Down | LowerLeft | Left,
		/// <summary>To be added.</summary>
		RightEdge = Down | LowerLeft | Left | UpperLeft | Up,
		/// <summary>To be added.</summary>
		LowerRightEdge = Left | UpperLeft | Up,
		/// <summary>To be added.</summary>
		DownEdge = Up | UpperRight | Right | Left | UpperLeft,
		/// <summary>To be added.</summary>
		LowerLeftEdge = Up | UpperRight | Right,
		/// <summary>To be added.</summary>
		LeftEdge = Up | UpperRight | Right | LowerRight | Down,
		/// <summary>To be added.</summary>
		UpperLeftEdge = Right | LowerRight | Down,
		/// <summary>To be added.</summary>
		UpperRightCorner = Up | UpperRight | Right | LowerRight | Down | Left | UpperLeft,
		/// <summary>To be added.</summary>
		LowerRightCorner = Up | UpperRight | Right | LowerRight | Down | LowerLeft | Left,
		/// <summary>To be added.</summary>
		LowerLeftCorner = Up | Right | LowerRight | Down | LowerLeft | Left | UpperLeft,
		/// <summary>To be added.</summary>
		UpperLeftCorner = Up | UpperRight | Right | Down | LowerLeft | Left | UpperLeft,
	}

	/// <summary>Enumerates the various ways a <see cref="T:SpriteKit.SKNode" /> may be focusable.</summary>
	[NoMac]
	[MacCatalyst (13, 1)]
	[Native]
	public enum SKNodeFocusBehavior : long {
		None = 0,
		Occluding,
		Focusable,
	}
}
