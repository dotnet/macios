// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;

using CoreFoundation;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using Metal;
using MetalKit;
using GameController;
using UIKit;

namespace TouchController {

	[NoMacCatalyst, iOS (26, 0)]
	[Native]
	public enum TCThrottleOrientation : long {
		Vertical,
		Horizontal,
	}

	[NoMacCatalyst, iOS (26, 0)]
	[Native]
	public enum TCColliderShape : long {
		Circle,
		Rect,
		LeftSide,
		RightSide,
	}

	[NoMacCatalyst, iOS (26, 0)]
	[Native]
	public enum TCControlContentsButtonShape : long {
		Circle,
		Rect,
	}

	[NoMacCatalyst, iOS (26, 0)]
	[Native]
	public enum TCControlContentsDpadDirection : long {
		Up,
		Down,
		Left,
		Right,
	}

	[NoMacCatalyst, iOS (26, 0)]
	[Native]
	public enum TCControlContentsDpadElementStyle : long {
		Circle,
		Pentagon,
	}

	[NoMacCatalyst, iOS (26, 0)]
	[Native]
	public enum TCControlLabelRole : long {
		Button,
		DirectionPad,
	}

	[NoMacCatalyst, iOS (26, 0)]
	[Native]
	public enum TCControlLayoutAnchor : long {
		TopLeft,
		TopCenter,
		TopRight,
		CenterLeft,
		Center,
		CenterRight,
		BottomLeft,
		BottomCenter,
		BottomRight,
	}

	[NoMacCatalyst, iOS (26, 0)]
	[Native]
	public enum TCControlLayoutAnchorCoordinateSystem : long {
		Relative,
		Absolute,
	}

	interface ITCControl { }

	[NoMacCatalyst, iOS (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface TCControl : TCControlLayout {

		[Abstract]
		[Export ("label", ArgumentSemantic.Strong)]
		TCControlLabel Label { get; }

		[Abstract]
		[Export ("colliderShape", ArgumentSemantic.Assign)]
		TCColliderShape ColliderShape { get; }

		[Abstract]
		[Export ("pressed")]
		bool Pressed { [Bind ("isPressed")] get; }

		[Abstract]
		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }

		[Abstract]
		[Export ("handleTouchBeganAtPoint:")]
		void HandleTouchBegan (CGPoint point);

		[Abstract]
		[Export ("handleTouchMovedAtPoint:")]
		void HandleTouchMoved (CGPoint point);

		[Abstract]
		[Export ("handleTouchEndedAtPoint:")]
		void HandleTouchEnded (CGPoint point);

		[Export ("highlightDuration")]
		double HighlightDuration { get; set; }
	}

	interface ITCControlLayout { }

	[NoMacCatalyst, iOS (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface TCControlLayout {

		[Abstract]
		[Export ("anchor", ArgumentSemantic.Assign)]
		TCControlLayoutAnchor Anchor { get; set; }

		[Abstract]
		[Export ("anchorCoordinateSystem", ArgumentSemantic.Assign)]
		TCControlLayoutAnchorCoordinateSystem AnchorCoordinateSystem { get; set; }

		[Abstract]
		[Export ("offset", ArgumentSemantic.Assign)]
		CGPoint Offset { get; set; }

		[Abstract]
		[Export ("zIndex")]
		nint ZIndex { get; set; }

		[Abstract]
		[Export ("size", ArgumentSemantic.Assign)]
		CGSize Size { get; set; }

		[Abstract]
		[Export ("position", ArgumentSemantic.Assign)]
		CGPoint Position { get; }
	}

	[NoMacCatalyst, iOS (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface TCButton : TCControl, TCControlLayout {

		[NullAllowed, Export ("contents", ArgumentSemantic.Strong)]
		TCControlContents Contents { get; set; }
	}

	[NoMacCatalyst, iOS (26, 0)]
	[BaseType (typeof (NSObject))]
	interface TCButtonDescriptor {

		[Export ("label", ArgumentSemantic.Strong)]
		TCControlLabel Label { get; set; }

		[NullAllowed, Export ("contents", ArgumentSemantic.Strong)]
		TCControlContents Contents { get; set; }

		[Export ("anchor", ArgumentSemantic.Assign)]
		TCControlLayoutAnchor Anchor { get; set; }

		[Export ("anchorCoordinateSystem", ArgumentSemantic.Assign)]
		TCControlLayoutAnchorCoordinateSystem AnchorCoordinateSystem { get; set; }

		[Export ("offset", ArgumentSemantic.Assign)]
		CGPoint Offset { get; set; }

		[Export ("zIndex")]
		nint ZIndex { get; set; }

		[Export ("size", ArgumentSemantic.Assign)]
		CGSize Size { get; set; }

		[Export ("colliderShape", ArgumentSemantic.Assign)]
		TCColliderShape ColliderShape { get; set; }

		[Export ("highlightDuration")]
		double HighlightDuration { get; set; }
	}

	[NoMacCatalyst, iOS (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface TCControlLabel {

		[Static]
		[Export ("buttonA", ArgumentSemantic.Strong)]
		TCControlLabel ButtonA { get; }

		[Static]
		[Export ("buttonB", ArgumentSemantic.Strong)]
		TCControlLabel ButtonB { get; }

		[Static]
		[Export ("buttonX", ArgumentSemantic.Strong)]
		TCControlLabel ButtonX { get; }

		[Static]
		[Export ("buttonY", ArgumentSemantic.Strong)]
		TCControlLabel ButtonY { get; }

		[Static]
		[Export ("buttonMenu", ArgumentSemantic.Strong)]
		TCControlLabel ButtonMenu { get; }

		[Static]
		[Export ("buttonOptions", ArgumentSemantic.Strong)]
		TCControlLabel ButtonOptions { get; }

		[Static]
		[Export ("buttonLeftShoulder", ArgumentSemantic.Strong)]
		TCControlLabel ButtonLeftShoulder { get; }

		[Static]
		[Export ("buttonLeftTrigger", ArgumentSemantic.Strong)]
		TCControlLabel ButtonLeftTrigger { get; }

		[Static]
		[Export ("buttonRightShoulder", ArgumentSemantic.Strong)]
		TCControlLabel ButtonRightShoulder { get; }

		[Static]
		[Export ("buttonRightTrigger", ArgumentSemantic.Strong)]
		TCControlLabel ButtonRightTrigger { get; }

		[Static]
		[Export ("leftThumbstick", ArgumentSemantic.Strong)]
		TCControlLabel LeftThumbstick { get; }

		[Static]
		[Export ("leftThumbstickButton", ArgumentSemantic.Strong)]
		TCControlLabel LeftThumbstickButton { get; }

		[Static]
		[Export ("rightThumbstick", ArgumentSemantic.Strong)]
		TCControlLabel RightThumbstick { get; }

		[Static]
		[Export ("rightThumbstickButton", ArgumentSemantic.Strong)]
		TCControlLabel RightThumbstickButton { get; }

		[Static]
		[Export ("directionPad", ArgumentSemantic.Strong)]
		TCControlLabel DirectionPad { get; }

		[Export ("initWithName:role:")]
		NativeHandle Constructor (string name, TCControlLabelRole role);

		[Export ("name", ArgumentSemantic.Retain)]
		string Name { get; }

		[Export ("role", ArgumentSemantic.Assign)]
		TCControlLabelRole Role { get; }
	}

	[NoMacCatalyst, iOS (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface TCDirectionPad : TCControl, TCControlLayout {

		[NullAllowed, Export ("compositeLabel", ArgumentSemantic.Strong)]
		TCControlLabel CompositeLabel { get; set; }

		[NullAllowed, Export ("upLabel", ArgumentSemantic.Strong)]
		TCControlLabel UpLabel { get; set; }

		[NullAllowed, Export ("downLabel", ArgumentSemantic.Strong)]
		TCControlLabel DownLabel { get; set; }

		[NullAllowed, Export ("leftLabel", ArgumentSemantic.Strong)]
		TCControlLabel LeftLabel { get; set; }

		[NullAllowed, Export ("rightLabel", ArgumentSemantic.Strong)]
		TCControlLabel RightLabel { get; set; }

		[NullAllowed, Export ("upContents", ArgumentSemantic.Strong)]
		TCControlContents UpContents { get; set; }

		[NullAllowed, Export ("downContents", ArgumentSemantic.Strong)]
		TCControlContents DownContents { get; set; }

		[NullAllowed, Export ("leftContents", ArgumentSemantic.Strong)]
		TCControlContents LeftContents { get; set; }

		[NullAllowed, Export ("rightContents", ArgumentSemantic.Strong)]
		TCControlContents RightContents { get; set; }

		[Export ("radial")]
		bool Radial { [Bind ("isRadial")] get; set; }

		[Export ("digital")]
		bool Digital { [Bind ("isDigital")] get; set; }

		[Export ("mutuallyExclusiveInput")]
		bool MutuallyExclusiveInput { [Bind ("inputIsMutuallyExclusive")] get; set; }
	}

	[NoMacCatalyst, iOS (26, 0)]
	[BaseType (typeof (NSObject))]
	interface TCDirectionPadDescriptor {

		[NullAllowed, Export ("compositeLabel", ArgumentSemantic.Strong)]
		TCControlLabel CompositeLabel { get; set; }

		[NullAllowed, Export ("upLabel", ArgumentSemantic.Strong)]
		TCControlLabel UpLabel { get; set; }

		[NullAllowed, Export ("downLabel", ArgumentSemantic.Strong)]
		TCControlLabel DownLabel { get; set; }

		[NullAllowed, Export ("leftLabel", ArgumentSemantic.Strong)]
		TCControlLabel LeftLabel { get; set; }

		[NullAllowed, Export ("rightLabel", ArgumentSemantic.Strong)]
		TCControlLabel RightLabel { get; set; }

		[NullAllowed, Export ("upContents", ArgumentSemantic.Strong)]
		TCControlContents UpContents { get; set; }

		[NullAllowed, Export ("downContents", ArgumentSemantic.Strong)]
		TCControlContents DownContents { get; set; }

		[NullAllowed, Export ("leftContents", ArgumentSemantic.Strong)]
		TCControlContents LeftContents { get; set; }

		[NullAllowed, Export ("rightContents", ArgumentSemantic.Strong)]
		TCControlContents RightContents { get; set; }

		[Export ("anchor", ArgumentSemantic.Assign)]
		TCControlLayoutAnchor Anchor { get; set; }

		[Export ("anchorCoordinateSystem", ArgumentSemantic.Assign)]
		TCControlLayoutAnchorCoordinateSystem AnchorCoordinateSystem { get; set; }

		[Export ("offset", ArgumentSemantic.Assign)]
		CGPoint Offset { get; set; }

		[Export ("zIndex")]
		nint ZIndex { get; set; }

		[Export ("size", ArgumentSemantic.Assign)]
		CGSize Size { get; set; }

		[Export ("colliderShape", ArgumentSemantic.Assign)]
		TCColliderShape ColliderShape { get; set; }

		[Export ("highlightDuration")]
		double HighlightDuration { get; set; }

		[Export ("radial")]
		bool Radial { [Bind ("isRadial")] get; set; }

		[Export ("digital")]
		bool Digital { [Bind ("isDigital")] get; set; }

		[Export ("mutuallyExclusiveInput")]
		bool MutuallyExclusiveInput { [Bind ("inputIsMutuallyExclusive")] get; set; }
	}

	[NoMacCatalyst, iOS (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface TCThrottle : TCControl, TCControlLayout {

		[NullAllowed, Export ("backgroundContents", ArgumentSemantic.Strong)]
		TCControlContents BackgroundContents { get; set; }

		[NullAllowed, Export ("indicatorContents", ArgumentSemantic.Strong)]
		TCControlContents IndicatorContents { get; set; }

		[Export ("orientation", ArgumentSemantic.Assign)]
		TCThrottleOrientation Orientation { get; }

		[Export ("snapsToBaseValue")]
		bool SnapsToBaseValue { get; set; }

		[Export ("baseValue")]
		nfloat BaseValue { get; set; }

		[Export ("indicatorSize", ArgumentSemantic.Assign)]
		CGSize IndicatorSize { get; set; }

		[Export ("throttleSize", ArgumentSemantic.Assign)]
		CGSize ThrottleSize { get; set; }
	}

	[NoMacCatalyst, iOS (26, 0)]
	[BaseType (typeof (NSObject))]
	interface TCThrottleDescriptor {

		[Export ("label", ArgumentSemantic.Strong)]
		TCControlLabel Label { get; set; }

		[NullAllowed, Export ("backgroundContents", ArgumentSemantic.Strong)]
		TCControlContents BackgroundContents { get; set; }

		[NullAllowed, Export ("indicatorContents", ArgumentSemantic.Strong)]
		TCControlContents IndicatorContents { get; set; }

		[Export ("size", ArgumentSemantic.Assign)]
		CGSize Size { get; set; }

		[Export ("indicatorSize", ArgumentSemantic.Assign)]
		CGSize IndicatorSize { get; set; }

		[Export ("throttleSize", ArgumentSemantic.Assign)]
		CGSize ThrottleSize { get; set; }

		[Export ("orientation", ArgumentSemantic.Assign)]
		TCThrottleOrientation Orientation { get; set; }

		[Export ("snapsToBaseValue")]
		bool SnapsToBaseValue { get; set; }

		[Export ("baseValue")]
		nfloat BaseValue { get; set; }

		[Export ("anchor", ArgumentSemantic.Assign)]
		TCControlLayoutAnchor Anchor { get; set; }

		[Export ("anchorCoordinateSystem", ArgumentSemantic.Assign)]
		TCControlLayoutAnchorCoordinateSystem AnchorCoordinateSystem { get; set; }

		[Export ("offset", ArgumentSemantic.Assign)]
		CGPoint Offset { get; set; }

		[Export ("zIndex")]
		nint ZIndex { get; set; }

		[Export ("colliderShape", ArgumentSemantic.Assign)]
		TCColliderShape ColliderShape { get; set; }

		[Export ("highlightDuration")]
		double HighlightDuration { get; set; }
	}

	[NoMacCatalyst, iOS (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface TCThumbstick : TCControl, TCControlLayout {

		[NullAllowed, Export ("backgroundContents", ArgumentSemantic.Strong)]
		TCControlContents BackgroundContents { get; set; }

		[NullAllowed, Export ("stickContents", ArgumentSemantic.Strong)]
		TCControlContents StickContents { get; set; }

		[Export ("hidesWhenNotPressed")]
		bool HidesWhenNotPressed { get; set; }

		[Export ("stickSize", ArgumentSemantic.Assign)]
		CGSize StickSize { get; set; }
	}

	[NoMacCatalyst, iOS (26, 0)]
	[BaseType (typeof (NSObject))]
	interface TCThumbstickDescriptor {

		[Export ("label", ArgumentSemantic.Strong)]
		TCControlLabel Label { get; set; }

		[NullAllowed, Export ("backgroundContents", ArgumentSemantic.Strong)]
		TCControlContents BackgroundContents { get; set; }

		[NullAllowed, Export ("stickContents", ArgumentSemantic.Strong)]
		TCControlContents StickContents { get; set; }

		[Export ("hidesWhenNotPressed")]
		bool HidesWhenNotPressed { get; set; }

		[Export ("stickSize", ArgumentSemantic.Assign)]
		CGSize StickSize { get; set; }

		[Export ("size", ArgumentSemantic.Assign)]
		CGSize Size { get; set; }

		[Export ("anchor", ArgumentSemantic.Assign)]
		TCControlLayoutAnchor Anchor { get; set; }

		[Export ("anchorCoordinateSystem", ArgumentSemantic.Assign)]
		TCControlLayoutAnchorCoordinateSystem AnchorCoordinateSystem { get; set; }

		[Export ("offset", ArgumentSemantic.Assign)]
		CGPoint Offset { get; set; }

		[Export ("zIndex")]
		nint ZIndex { get; set; }

		[Export ("colliderShape", ArgumentSemantic.Assign)]
		TCColliderShape ColliderShape { get; set; }

		[Export ("highlightDuration")]
		double HighlightDuration { get; set; }
	}

	[NoMacCatalyst, iOS (26, 0)]
	[BaseType (typeof (NSObject))]
	interface TCTouchController {

		[Field ("TCGameControllerProductCategoryTouchController")]
		NSString ProductCategoryTouchController { get; }

		[Static]
		[Export ("supported")]
		bool Supported { [Bind ("isSupported")] get; }

		[Export ("device")]
		IMTLDevice Device { get; }

		[Export ("controls")]
		ITCControl [] Controls { get; }

		[Export ("buttons")]
		TCButton [] Buttons { get; }

		[Export ("switches")]
		TCSwitch [] Switches { get; }

		[Export ("thumbsticks")]
		TCThumbstick [] Thumbsticks { get; }

		[Export ("directionPads")]
		TCDirectionPad [] DirectionPads { get; }

		[Export ("throttles")]
		TCThrottle [] Throttles { get; }

		[Export ("touchpads")]
		TCTouchpad [] Touchpads { get; }

		[Export ("size", ArgumentSemantic.Assign)]
		CGSize Size { get; set; }

		[Export ("drawableSize", ArgumentSemantic.Assign)]
		CGSize DrawableSize { get; set; }

		[Export ("initWithDescriptor:")]
		NativeHandle Constructor (TCTouchControllerDescriptor descriptor);

		[Export ("automaticallyLayoutControlsForLabels:")]
		void AutomaticallyLayoutControlsForLabels (TCControlLabel [] labels);

		[Export ("addButtonWithDescriptor:")]
		TCButton AddButton (TCButtonDescriptor descriptor);

		[Export ("addSwitchWithDescriptor:")]
		TCSwitch AddSwitch (TCSwitchDescriptor descriptor);

		[Export ("addThumbstickWithDescriptor:")]
		TCThumbstick AddThumbstick (TCThumbstickDescriptor descriptor);

		[Export ("addDirectionPadWithDescriptor:")]
		TCDirectionPad AddDirectionPad (TCDirectionPadDescriptor descriptor);

		[Export ("addThrottleWithDescriptor:")]
		TCThrottle AddThrottle (TCThrottleDescriptor descriptor);

		[Export ("addTouchpadWithDescriptor:")]
		TCTouchpad AddTouchpad (TCTouchpadDescriptor descriptor);

		[Export ("removeAllControls")]
		void RemoveAllControls ();

		[Export ("removeControl:")]
		void RemoveControl (ITCControl control);

		[Export ("controlAtPoint:")]
		[return: NullAllowed]
		ITCControl GetControl (CGPoint point);

		[Export ("handleTouchBeganAtPoint:index:")]
		bool HandleTouchBegan (CGPoint point, nint index);

		[Export ("handleTouchMovedAtPoint:index:")]
		bool HandleTouchMoved (CGPoint point, nint index);

		[Export ("handleTouchEndedAtPoint:index:")]
		bool HandleTouchEnded (CGPoint point, nint index);

		[Export ("renderUsingRenderCommandEncoder:")]
		void Render (IMTLRenderCommandEncoder encoder);

		[Export ("connected")]
		bool Connected { [Bind ("isConnected")] get; }

		[Export ("connect")]
		void Connect ();

		[Export ("disconnect")]
		void Disconnect ();

		[Export ("controller", ArgumentSemantic.Strong)]
		GCController Controller { get; }
	}

	[NoMacCatalyst, iOS (26, 0)]
	[BaseType (typeof (NSObject))]
	[DesignatedDefaultCtor]
	interface TCTouchControllerDescriptor {

		[Export ("device", ArgumentSemantic.Strong)]
		IMTLDevice Device { get; set; }

		[Export ("size", ArgumentSemantic.Assign)]
		CGSize Size { get; set; }

		[Export ("drawableSize", ArgumentSemantic.Assign)]
		CGSize DrawableSize { get; set; }

		[Export ("colorPixelFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat ColorPixelFormat { get; set; }

		[Export ("depthAttachmentPixelFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat DepthAttachmentPixelFormat { get; set; }

		[Export ("stencilAttachmentPixelFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat StencilAttachmentPixelFormat { get; set; }

		[Export ("sampleCount")]
		nuint SampleCount { get; set; }

		[Export ("initWithMTKView:")]
		NativeHandle Constructor (MTKView mtkView);
	}

	[NoMacCatalyst, iOS (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface TCTouchpad : TCControl, TCControlLayout {

		[NullAllowed, Export ("contents", ArgumentSemantic.Strong)]
		TCControlContents Contents { get; set; }

		[Export ("reportsRelativeValues")]
		bool ReportsRelativeValues { get; set; }
	}

	[NoMacCatalyst, iOS (26, 0)]
	[BaseType (typeof (NSObject))]
	interface TCTouchpadDescriptor {

		[Export ("label", ArgumentSemantic.Strong)]
		TCControlLabel Label { get; set; }

		[NullAllowed, Export ("contents", ArgumentSemantic.Strong)]
		TCControlContents Contents { get; set; }

		[Export ("anchor", ArgumentSemantic.Assign)]
		TCControlLayoutAnchor Anchor { get; set; }

		[Export ("anchorCoordinateSystem", ArgumentSemantic.Assign)]
		TCControlLayoutAnchorCoordinateSystem AnchorCoordinateSystem { get; set; }

		[Export ("offset", ArgumentSemantic.Assign)]
		CGPoint Offset { get; set; }

		[Export ("zIndex")]
		nint ZIndex { get; set; }

		[Export ("size", ArgumentSemantic.Assign)]
		CGSize Size { get; set; }

		[Export ("colliderShape", ArgumentSemantic.Assign)]
		TCColliderShape ColliderShape { get; set; }

		[Export ("highlightDuration")]
		double HighlightDuration { get; set; }

		[Export ("reportsRelativeValues")]
		bool ReportsRelativeValues { get; set; }
	}

	[NoMacCatalyst, iOS (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface TCControlContents {

		[Static]
		[Export ("contentsWithImages:")]
		TCControlContents CreateContents (TCControlImage [] images);

		[Static]
		[Export ("buttonContentsForSystemImageNamed:size:shape:controller:")]
		TCControlContents CreateButtonContents (string systemImageName, CGSize size, TCControlContentsButtonShape shape, TCTouchController controller);

		[Static]
		[Export ("switchedOnContentsForSystemImageNamed:size:shape:controller:")]
		TCControlContents CreateSwitchedOnContents (string systemImageName, CGSize size, TCControlContentsButtonShape shape, TCTouchController controller);

		[Static]
		[Export ("thumbstickStickContentsOfSize:controller:")]
		TCControlContents CreateThumbstickStickContents (CGSize size, TCTouchController controller);

		[Static]
		[Export ("thumbstickBackgroundContentsOfSize:controller:")]
		TCControlContents CreateThumbstickBackgroundContents (CGSize size, TCTouchController controller);

		[Static]
		[Export ("throttleIndicatorContentsOfSize:controller:")]
		TCControlContents CreateThrottleIndicatorContents (CGSize size, TCTouchController controller);

		[Static]
		[Export ("throttleBackgroundContentsOfSize:controller:")]
		TCControlContents CreateThrottleBackgroundContents (CGSize size, TCTouchController controller);

		[Static]
		[Export ("directionPadContentsForLabel:size:style:direction:controller:")]
		TCControlContents CreateDirectionPadContents (TCControlLabel label, CGSize size, TCControlContentsDpadElementStyle style, TCControlContentsDpadDirection direction, TCTouchController controller);

		[Export ("images", ArgumentSemantic.Strong)]
		TCControlImage [] Images { get; }
	}

	[NoMacCatalyst, iOS (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface TCControlImage {

		[Export ("initWithTexture:size:")]
		NativeHandle Constructor (IMTLTexture texture, CGSize size);

		[Export ("initWithTexture:size:highlightTexture:offset:tintColor:")]
		[DesignatedInitializer]
		NativeHandle Constructor (IMTLTexture texture, CGSize size, [NullAllowed] IMTLTexture highlightTexture, CGPoint offset, CGColor tintColor);

		[Export ("initWithCGImage:size:device:")]
		NativeHandle Constructor (CGImage cgImage, CGSize size, IMTLDevice device);

		[Export ("initWithUIImage:size:device:")]
		NativeHandle Constructor (UIImage uiImage, CGSize size, IMTLDevice device);

		[Export ("texture", ArgumentSemantic.Strong)]
		IMTLTexture Texture { get; set; }

		[NullAllowed, Export ("highlightTexture", ArgumentSemantic.Strong)]
		IMTLTexture HighlightTexture { get; set; }

		[Export ("size", ArgumentSemantic.Assign)]
		CGSize Size { get; set; }

		[Export ("offset", ArgumentSemantic.Assign)]
		CGPoint Offset { get; set; }

		[Export ("tintColor", ArgumentSemantic.Assign)]
		CGColor TintColor { get; set; }
	}

	[NoMacCatalyst, iOS (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface TCSwitch : TCControl, TCControlLayout {

		[NullAllowed, Export ("contents", ArgumentSemantic.Strong)]
		TCControlContents Contents { get; set; }

		[NullAllowed, Export ("switchedOnContents", ArgumentSemantic.Strong)]
		TCControlContents SwitchedOnContents { get; set; }

		[Export ("switchedOn")]
		bool SwitchedOn { [Bind ("isSwitchedOn")] get; }
	}

	[NoMacCatalyst, iOS (26, 0)]
	[BaseType (typeof (NSObject))]
	interface TCSwitchDescriptor {

		[Export ("label", ArgumentSemantic.Strong)]
		TCControlLabel Label { get; set; }

		[NullAllowed, Export ("contents", ArgumentSemantic.Strong)]
		TCControlContents Contents { get; set; }

		[NullAllowed, Export ("switchedOnContents", ArgumentSemantic.Strong)]
		TCControlContents SwitchedOnContents { get; set; }

		[Export ("anchor", ArgumentSemantic.Assign)]
		TCControlLayoutAnchor Anchor { get; set; }

		[Export ("anchorCoordinateSystem", ArgumentSemantic.Assign)]
		TCControlLayoutAnchorCoordinateSystem AnchorCoordinateSystem { get; set; }

		[Export ("offset", ArgumentSemantic.Assign)]
		CGPoint Offset { get; set; }

		[Export ("zIndex")]
		nint ZIndex { get; set; }

		[Export ("size", ArgumentSemantic.Assign)]
		CGSize Size { get; set; }

		[Export ("colliderShape", ArgumentSemantic.Assign)]
		TCColliderShape ColliderShape { get; set; }

		[Export ("highlightDuration")]
		double HighlightDuration { get; set; }
	}
}
