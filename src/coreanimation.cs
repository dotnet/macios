//
// coreanimation.cs: API definition for CoreAnimation binding
//
// Authors:
//   Geoff Norton
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
// Copyright 2010, Novell, Inc.
// Copyright 2011, 2012, 2015 Xamarin Inc
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
using System.ComponentModel;
using System.Diagnostics;
#if MONOMAC
using AppKit;
using CoreVideo;
using OpenGL;
#else
using UIKit;
#endif
#if HAS_OPENGLES
using OpenGLES;
#endif
using Foundation;
using CoreImage;
using CoreGraphics;
using ObjCRuntime;
using Metal;
using SceneKit; // For SCNAnimationEvent

#if __TVOS__
using CAEdrMetadata = Foundation.NSObject;
#endif

#if !MONOMAC
using CGLPixelFormat = Foundation.NSObject;
using CVTimeStamp = Foundation.NSObject;
using CGLContext = System.IntPtr;
#endif

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace CoreAnimation {

	/// <summary>Provides a hierarchical timing system, with support for repetition and sequencing.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GraphicsImaging/Reference/CAMediaTiming_protocol/index.html">Apple documentation for <c>CAMediaTiming</c></related>
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface CAMediaTiming {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("beginTime")]
		double BeginTime { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("duration")]
		double Duration { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("speed")]
		float Speed { get; set; } /* float, not CGFloat */

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("timeOffset")]
		double TimeOffset { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("repeatCount")]
		float RepeatCount { get; set; } /* float, not CGFloat */

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("repeatDuration")]
		double RepeatDuration { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("autoreverses")]
		bool AutoReverses { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("fillMode", ArgumentSemantic.Copy)]
		string FillMode { get; set; }
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="CoreAnimation.CAMediaTiming" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="CoreAnimation.CAMediaTiming" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="CoreAnimation.CAMediaTiming" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="CoreAnimation.CAMediaTiming_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface ICAMediaTiming { }

	[NoiOS]
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CAConstraintLayoutManager : NSCoding {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("layoutManager")]
		CAConstraintLayoutManager LayoutManager { get; }
	}

	[NoiOS]
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CAConstraint : NSSecureCoding {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("attribute")]
		CAConstraintAttribute Attribute { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("sourceName")]
		string SourceName { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("sourceAttribute")]
		CAConstraintAttribute SourceAttribute { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("scale")]
		nfloat Scale { get; }

		/// <param name="attribute">To be added.</param>
		/// <param name="relativeToSource">To be added.</param>
		/// <param name="srcAttr">To be added.</param>
		/// <param name="scale">To be added.</param>
		/// <param name="offset">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Static]
		[Export ("constraintWithAttribute:relativeTo:attribute:scale:offset:")]
		CAConstraint Create (CAConstraintAttribute attribute, string relativeToSource, CAConstraintAttribute srcAttr, nfloat scale, nfloat offset);

		/// <param name="attribute">To be added.</param>
		/// <param name="relativeToSource">To be added.</param>
		/// <param name="srcAttr">To be added.</param>
		/// <param name="offset">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Static]
		[Export ("constraintWithAttribute:relativeTo:attribute:offset:")]
		CAConstraint Create (CAConstraintAttribute attribute, string relativeToSource, CAConstraintAttribute srcAttr, nfloat offset);

		/// <param name="attribute">To be added.</param>
		///         <param name="relativeToSource">To be added.</param>
		///         <param name="srcAttribute">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("constraintWithAttribute:relativeTo:attribute:")]
		CAConstraint Create (CAConstraintAttribute attribute, string relativeToSource, CAConstraintAttribute srcAttribute);

		/// <param name="attribute">To be added.</param>
		/// <param name="relativeToSource">To be added.</param>
		/// <param name="srcAttr">To be added.</param>
		/// <param name="scale">To be added.</param>
		/// <param name="offset">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithAttribute:relativeTo:attribute:scale:offset:")]
		NativeHandle Constructor (CAConstraintAttribute attribute, string relativeToSource, CAConstraintAttribute srcAttr, nfloat scale, nfloat offset);
	}

	/// <include file="../docs/api/CoreAnimation/CADisplayLink.xml" path="/Documentation/Docs[@DocId='T:CoreAnimation.CADisplayLink']/*" />
	[Mac (14, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CADisplayLink {
		/// <param name="target">Target object to invoke the selector on.</param>
		///         <param name="sel">Selector to invoke.</param>
		///         <summary>Objective-C style registeration of the method to be invoked every time the display is about to be updated.</summary>
		///         <returns>The DisplayLink object that will invoke the specified method on each screen update.</returns>
		///         <remarks>
		///           <para>
		/// 	    With C# you can use the Create overload that takes a NSAction as it can be used with lambdas.
		/// 	  </para>
		///           <para>
		/// 	    Once you create the display link, you must add the handler to the runloop.
		/// 	  </para>
		///         </remarks>
		[Export ("displayLinkWithTarget:selector:")]
		[Static]
		CADisplayLink Create (NSObject target, Selector sel);

		/// <param name="runloop">The runloop on which to run.</param>
		///         <param name="mode">Modes in which the timer will be invoked, one of the various NSString constants in <see cref="Foundation.NSRunLoop" />.<para tool="nullallowed">This parameter can be <see langword="null" />.</para></param>
		///         <summary>Trigger timer events on the specified runloop for the specified modes (weakly typed parameters).</summary>
		///         <remarks>
		///           <para>You should use the strongly typed version if possible, as it prevents common errors.</para>
		///           <para>
		/// 	    The mode parameter will determine when the event is sent.
		/// 	    The NSRunLoop.NSDefaultRunLoopMode is not delivered during
		/// 	    UI tracking events (like scrolling in a UIScrollbar).  For
		/// 	    getting those kinds of events use
		/// 	    NSRunLoop.UITrackingRunLoopMode.  Or use
		/// 	    NSRunLoop.NSRunLoopCommonModes which covers both cases.
		///
		/// 	  </para>
		///         </remarks>
		[Export ("addToRunLoop:forMode:")]
		void AddToRunLoop (NSRunLoop runloop, NSString mode);

		/// <param name="runloop">The runloop on which to run.</param>
		///         <param name="mode">Modes in which the timer will be invoked.</param>
		///         <summary>Trigger timer events on the specified runloop for the specified modes.</summary>
		///         <remarks>
		///           <para>The mode parameter will determine when the event is sent. The NSRunLoop.NSDefaultRunLoopMode is not delivered during UI tracking events (like scrolling in a UIScrollbar). For getting those kinds of events use NSRunLoop.UITrackingRunLoopMode. Or use NSRunLoop.NSRunLoopCommonModes which covers both cases.  	  </para>
		///         </remarks>
		[Wrap ("AddToRunLoop (runloop, mode.GetConstant ()!)")]
		void AddToRunLoop (NSRunLoop runloop, NSRunLoopMode mode);

		/// <param name="runloop">The run loop from which to remove the display link.</param>
		///         <param name="mode">
		///           <para>The mode of the run loop.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Removes the display link from the provided run loop when in the specified mode.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("removeFromRunLoop:forMode:")]
		void RemoveFromRunLoop (NSRunLoop runloop, NSString mode);

		/// <param name="runloop">The run loop from which to remove the display link.</param>
		///         <param name="mode">The mode of the run loop.</param>
		///         <summary>Removes the display link from the provided run loop when in the specified mode.</summary>
		///         <remarks>To be added.</remarks>
		[Wrap ("RemoveFromRunLoop (runloop, mode.GetConstant ()!)")]
		void RemoveFromRunLoop (NSRunLoop runloop, NSRunLoopMode mode);

		/// <summary>Terminates the connection between CoreAnimation and your code.     This removes the CADisplayLink from all run loops.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("invalidate")]
		void Invalidate ();

		/// <summary>Timestamp for the last frame displayed.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <para>Developers can this value to compute which date should be displayed next.</para>
		///         </remarks>
		[Export ("timestamp")]
		double Timestamp { get; }

		/// <summary>Whether the notifications between the animation loop and your code are suspended.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("paused")]
		bool Paused { [Bind ("isPaused")] get; set; }

		/// <summary>Developers should not use this deprecated property. Developers should use 'PreferredFramesPerSecond' property.</summary>
		///         <value>To be added.</value>
		///         <remarks>
		///           <para>
		/// 	    The default value is one, which means that your code is
		/// 	    invoked once each time that a screen refresh will take
		/// 	    place.  If you set this value to two, then your method will
		/// 	    only be invoked half the time (once every two frames) and so
		/// 	    on.
		/// 	  </para>
		///           <para>
		/// 	    In general, you should not skip frames.  If your software
		/// 	    can not keep up with the frame rate, it is better to
		/// 	    increase the frame interval and deliver a consistent set
		/// 	    of updates.
		/// 	  </para>
		///         </remarks>
		[Obsoleted (PlatformName.iOS, 10, 0, message: "Use 'PreferredFramesPerSecond' property.")]
		[Obsoleted (PlatformName.TvOS, 10, 0, message: "Use 'PreferredFramesPerSecond' property.")]
		[Obsoleted (PlatformName.MacCatalyst, 13, 1, message: "Use 'PreferredFramesPerSecond' property.")]
		[NoMac]
		[Export ("frameInterval")]
		nint FrameInterval { get; set; }

		/// <summary>The time between screen refreshes.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("duration")]
		double Duration { get; }

		/// <summary>Gets the time stamp for the desired time to start displaying the target of the display link.</summary>
		///         <value>The time stamp for the desired time to start displaying the target of the display link.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("targetTimestamp")]
		double TargetTimestamp { get; }

		/// <summary>Gets or sets the preferred display rate.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 15, 0, message: "Use 'PreferredFrameRateRange' property.")]
		[Deprecated (PlatformName.TvOS, 15, 0, message: "Use 'PreferredFrameRateRange' property.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 15, 0, message: "Use 'PreferredFrameRateRange' property.")]
		[NoMac]
		[Export ("preferredFramesPerSecond")]
		nint PreferredFramesPerSecond { get; set; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("preferredFrameRateRange", ArgumentSemantic.Assign)]
		CAFrameRateRange PreferredFrameRateRange { get; set; }
	}

	[MacCatalyst (13, 1)]
	enum CAContentsFormat {
		/// <summary>To be added.</summary>
		[Field ("kCAContentsFormatGray8Uint")]
		Gray8Uint,
		/// <summary>To be added.</summary>
		[Field ("kCAContentsFormatRGBA8Uint")]
		Rgba8Uint,
		/// <summary>To be added.</summary>
		[Field ("kCAContentsFormatRGBA16Float")]
		Rgba16Float,
	}


	/// <include file="../docs/api/CoreAnimation/CALayer.xml" path="/Documentation/Docs[@DocId='T:CoreAnimation.CALayer']/*" />
	[BaseType (typeof (NSObject))]
	[Dispose ("OnDispose ();", Optimizable = true)]
	interface CALayer : CAMediaTiming, NSSecureCoding {
		/// <summary>Factory method to create a new <see cref="CoreAnimation.CALayer" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("layer")]
		[Static]
		CALayer Create ();

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("presentationLayer")]
		[NullAllowed]
		CALayer PresentationLayer { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("modelLayer")]
		CALayer ModelLayer { get; }

		/// <param name="key">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("defaultValueForKey:")]
		[return: NullAllowed]
		NSObject DefaultValue (string key);

		/// <param name="key">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("needsDisplayForKey:")]
		bool NeedsDisplayForKey (string key);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("bounds")]
		CGRect Bounds { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("zPosition")]
		nfloat ZPosition { get; set; }

		/// <summary>The anchor point for the <see cref="CoreAnimation.CALayer.Bounds" /> rectangle.</summary>
		///         <value>The developer specifies the value for this property using the unit coordinate space. The default value for this property is (0.5, 0.5), representing the center of the bounds rectangle for this layer.</value>
		///         <remarks>To be added.</remarks>
		[Export ("anchorPoint")]
		CGPoint AnchorPoint { get; set; }

		/// <summary>The anchor point for the <see cref="CoreAnimation.CALayer.Bounds" />, defined along the Z axis.</summary>
		///         <value>The default value is 0.</value>
		///         <remarks>To be added.</remarks>
		[Export ("anchorPointZ")]
		nfloat AnchorPointZ { get; set; }

		/// <summary>The layer's position in the coordinate space of its <see cref="CoreAnimation.CALayer.SuperLayer" />.</summary>
		///         <value>The default is (0,0). This value is specified in points relative to the layer's <see cref="CoreAnimation.CALayer.AnchorPoint" />.</value>
		///         <remarks>To be added.</remarks>
		[Export ("position")]
		CGPoint Position { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("transform")]
		CATransform3D Transform { get; set; }

		/// <summary>The affine version of the layer's <see cref="CoreAnimation.CALayer.Transform" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("affineTransform")]
		CGAffineTransform AffineTransform { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("frame")]
		CGRect Frame { get; set; }

		/// <summary>Whether the layer is not displayed.</summary>
		///         <value>The default value is <see langword="false" />.</value>
		///         <remarks>To be added.</remarks>
		[Export ("hidden")] // Setter needs setHidden instead
		bool Hidden { [Bind ("isHidden")] get; set; }

		/// <summary>Whether the layer draws its content when facing away from the viewer.</summary>
		///         <value>The default value is <see langword="true" />.</value>
		///         <remarks>To be added.</remarks>
		[Export ("doubleSided")]  // Setter needs setDoubleSided
		bool DoubleSided { [Bind ("isDoubleSided")] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("geometryFlipped")]
		bool GeometryFlipped { [Bind ("isGeometryFlipped")] get; set; }

		/// <summary>Whether the layer's contents are implicitly flipped when the layer is rendered.</summary>
		///         <value>The default value is <see langword="false" />.</value>
		///         <remarks>To be added.</remarks>
		[Export ("contentsAreFlipped")]
		bool ContentsAreFlipped { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("superlayer")]
		[NullAllowed]
		CALayer SuperLayer { get; }

		/// <summary>Removes <c>this</c> from its <see cref="CoreAnimation.CALayer.SuperLayer" />.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("removeFromSuperlayer")]
		void RemoveFromSuperLayer ();

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("sublayers", ArgumentSemantic.Copy)]
		CALayer [] Sublayers { get; set; }

		/// <param name="layer">The layer being added.</param>
		///         <summary>Appends the <paramref name="layer" /> to this layer's <see cref="CoreAnimation.CALayer.Sublayers" />.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("addSublayer:")]
		[PostGet ("Sublayers")]
		void AddSublayer (CALayer layer);

		/// <param name="layer">The layer being inserted.</param>
		///         <param name="index">The index at which to insert the <paramref name="layer" />.</param>
		///         <summary>Inserts the specified layer into the <see cref="CoreAnimation.CALayer.Sublayers" /> array at the specified index.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("insertSublayer:atIndex:")]
		[PostGet ("Sublayers")]
		void InsertSublayer (CALayer layer, int index);

		/// <param name="layer">The layer being inserted.</param>
		///         <param name="sibling">The existing sublayer, which will subsequently appear in front of <paramref name="layer" />.</param>
		///         <summary>Inserts the specified layer into the <see cref="CoreAnimation.CALayer.Sublayers" /> array immediately prior to <paramref name="sibling" />.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("insertSublayer:below:")]
		[PostGet ("Sublayers")]
		void InsertSublayerBelow (CALayer layer, [NullAllowed] CALayer sibling);

		/// <param name="layer">The layer being inserted.</param>
		///         <param name="sibling">The existing sublayer, which will subsequently appear behind <paramref name="layer" />.</param>
		///         <summary>Inserts the specified layer into the <see cref="CoreAnimation.CALayer.Sublayers" /> array immediately after <paramref name="sibling" />.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("insertSublayer:above:")]
		[PostGet ("Sublayers")]
		void InsertSublayerAbove (CALayer layer, [NullAllowed] CALayer sibling);

		/// <param name="layer">To be added.</param>
		///         <param name="with">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("replaceSublayer:with:")]
		[PostGet ("Sublayers")]
		void ReplaceSublayer (CALayer layer, CALayer with);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("sublayerTransform")]
		CATransform3D SublayerTransform { get; set; }

		/// <summary>An optional layer, the alpha channel of which is used to mask the contents of this layer.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("mask", ArgumentSemantic.Strong)]
		[NullAllowed]
		CALayer Mask { get; set; }

		/// <summary>Whether sublayers are clipped to the <see cref="CoreAnimation.CALayer.Bounds" /> of this layer.</summary>
		///         <value>The default value is <see langword="false" />.</value>
		///         <remarks>If <see langword="true" />, the clipping mask matches the bounds of <c>this</c><see cref="CoreAnimation.CALayer" />, including rounded corners.</remarks>
		[Export ("masksToBounds")]
		bool MasksToBounds { get; set; }

		/// <param name="point">To be added.</param>
		///         <param name="layer">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("convertPoint:fromLayer:")]
		CGPoint ConvertPointFromLayer (CGPoint point, [NullAllowed] CALayer layer);

		/// <param name="point">To be added.</param>
		///         <param name="layer">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("convertPoint:toLayer:")]
		CGPoint ConvertPointToLayer (CGPoint point, [NullAllowed] CALayer layer);

		/// <param name="rect">To be added.</param>
		///         <param name="layer">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("convertRect:fromLayer:")]
		CGRect ConvertRectFromLayer (CGRect rect, [NullAllowed] CALayer layer);

		/// <param name="rect">To be added.</param>
		///         <param name="layer">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("convertRect:toLayer:")]
		CGRect ConvertRectToLayer (CGRect rect, [NullAllowed] CALayer layer);

		/// <param name="timeInterval">To be added.</param>
		///         <param name="layer">
		///           <para>They layer that will be used to convert the time from.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("convertTime:fromLayer:")]
		double ConvertTimeFromLayer (double timeInterval, [NullAllowed] CALayer layer);

		/// <param name="timeInterval">To be added.</param>
		///         <param name="layer">
		///           <para>The layer that will be used to convert the time to.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("convertTime:toLayer:")]
		double ConvertTimeToLayer (double timeInterval, [NullAllowed] CALayer layer);

		/// <param name="p">A point, in the coordinate system of this layer's <see cref="CoreAnimation.CALayer.SuperLayer" />.</param>
		///         <summary>The furthest descendant in this layer's hierarchy that contains the point <paramref name="p" />.</summary>
		///         <returns>The layer (possible <c>this</c>) that contains the point <paramref name="p" /> or <see langword="null" /> if <paramref name="p" /> lies outside the <see cref="CoreAnimation.CALayer.Bounds" /> rectangle of <c>this</c>.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("hitTest:")]
		[return: NullAllowed]
		CALayer HitTest (CGPoint p);

		/// <param name="p">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("containsPoint:")]
		bool Contains (CGPoint p);

		/// <summary>The contents of this layer, as a <see cref="CoreGraphics.CGImage" />.</summary>
		///         <value>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		[Export ("contents", ArgumentSemantic.Strong), NullAllowed]
		CGImage Contents { get; set; }

		[Export ("contents", ArgumentSemantic.Strong)]
		[Internal]
		[Sealed]
		IntPtr _Contents { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("layoutManager", ArgumentSemantic.Retain)]
		[NullAllowed]
		NSObject LayoutManager { get; set; }

		/// <summary>The scale factor applied to the layer.</summary>
		///         <value>The default value is 1.0.</value>
		///         <remarks>To be added.</remarks>
		[Export ("contentsScale")]
		nfloat ContentsScale { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("contentsRect")]
		CGRect ContentsRect { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("contentsGravity", ArgumentSemantic.Copy)]
		string ContentsGravity { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("contentsCenter")]
		CGRect ContentsCenter { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("minificationFilter", ArgumentSemantic.Copy)]
		string MinificationFilter { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("magnificationFilter", ArgumentSemantic.Copy)]
		string MagnificationFilter { get; set; }

		/// <summary>Whether the layer and its contents are completely opaque.</summary>
		///         <value>The default value is <see langword="false" />. If <see langword="true" />, the system can render the layer more quickly.</value>
		///         <remarks>To be added.</remarks>
		[Export ("opaque")]
		bool Opaque { [Bind ("isOpaque")] get; set; }

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("display")]
		void Display ();

		/// <summary>Whether the layer has been marked as requiring an update.</summary>
		///         <value>
		///           <see langword="true" /> if the layer has been marked as requiring an update.</value>
		///         <remarks>To be added.</remarks>
		[Export ("needsDisplay")]
		bool NeedsDisplay { get; }

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setNeedsDisplay")]
		void SetNeedsDisplay ();

		/// <param name="r">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setNeedsDisplayInRect:")]
		void SetNeedsDisplayInRect (CGRect r);

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("displayIfNeeded")]
		void DisplayIfNeeded ();

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("needsDisplayOnBoundsChange")]
		bool NeedsDisplayOnBoundsChange { get; set; }

		/// <param name="ctx">Prepared context to draw into.</param>
		///         <summary>Draws the layer on the specified context.</summary>
		///         <remarks>
		///           <para>
		/// 	    Developers override this method to provide custom
		/// 	    rendering of the contents of their instance of the
		/// 	    CALayer.  If this method is not overwritten, the CALayer
		/// 	    will invoke the <see cref="CoreAnimation.CALayerDelegate.DrawLayer(CoreAnimation.CALayer,CoreGraphics.CGContext)" />
		/// 	    method to render the contents.
		/// 	  </para>
		///           <para>
		/// 	    The provided context has been preconfigured for the target
		/// 	    surface as well as having a clipping region defined.
		/// 	  </para>
		///         </remarks>
		[Export ("drawInContext:")]
		void DrawInContext (CGContext ctx);

		/// <param name="ctx">The context in which the layer should be rendered.</param>
		///         <summary>Renders the layer into the specified <see cref="CoreGraphics.CGContext" />.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("renderInContext:")]
		void RenderInContext (CGContext ctx);

		/// <summary>The background color of the layer.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("backgroundColor")]
		CGColor BackgroundColor { get; set; }

		/// <summary>The radius used when drawing rounded corners.</summary>
		///         <value>The default value is 0.</value>
		///         <remarks>To be added.</remarks>
		[Export ("cornerRadius")]
		nfloat CornerRadius { get; set; }

		/// <summary>The width of the border of the layer.</summary>
		///         <value>The default value is 0.</value>
		///         <remarks>To be added.</remarks>
		[Export ("borderWidth")]
		nfloat BorderWidth { get; set; }

		/// <summary>The color of the border of the layer.</summary>
		///         <value>The default value is black.</value>
		///         <remarks>To be added.</remarks>
		[Export ("borderColor")]
		[NullAllowed]
		CGColor BorderColor { get; set; }

		/// <summary>The transparency of the layer, in the range 0 (transparent) to 1.0 (opaque).</summary>
		///         <value>The default value is 1.0.</value>
		///         <remarks>To be added.</remarks>
		[Export ("opacity")]
		float Opacity { get; set; } /* float, not CGFloat */

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("edgeAntialiasingMask")]
		CAEdgeAntialiasingMask EdgeAntialiasingMask { get; set; }

		// Layout methods

		/// <summary>The preferred size for this layer, in the coordinate of its <see cref="CoreAnimation.CALayer.SuperLayer" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("preferredFrameSize")]
		CGSize PreferredFrameSize ();

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setNeedsLayout")]
		void SetNeedsLayout ();

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("needsLayout")]
		bool NeedsLayout ();

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("layoutIfNeeded")]
		void LayoutIfNeeded ();

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("layoutSublayers")]
		void LayoutSublayers ();

		/// <param name="eventKey">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("defaultActionForKey:")]
		[return: NullAllowed]
		NSObject DefaultActionForKey (string eventKey);

		/// <param name="eventKey">Identifier of the action desired.</param>
		///         <summary>Returns the <see cref="CoreAnimation.CALayer.Actions" /> value associated with the specified key.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("actionForKey:")]
		[return: NullAllowed]
		NSObject ActionForKey (string eventKey);

		/// <summary>
		///           <see cref="Foundation.NSDictionary" /> containing the layer's actions.</summary>
		///         <value>
		///           <para>The default value is <see langword="null" />.</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("actions", ArgumentSemantic.Copy)]
		NSDictionary Actions { get; set; }

		/// <param name="animation">To be added.</param>
		///         <param name="key">
		///           <para>
		/// 	    An identifier for the animation.  If the key already exists in the layer, the previous animation is removed.   
		/// 	  </para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Adds the <paramref name="animation" /> to the render tree for the layer and associates it in <see cref="CoreAnimation.CALayer.Actions" /> with the key <paramref name="key" />.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("addAnimation:forKey:")]
		void AddAnimation (CAAnimation animation, [NullAllowed] string key);

		/// <summary>Removes all animations currently attached to the layer.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("removeAllAnimations")]
		void RemoveAllAnimations ();

		/// <param name="key">The animation's identifier.</param>
		///         <summary>Removes the specified animation from the layer.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("removeAnimationForKey:")]
		void RemoveAnimation (string key);

		/// <summary>Identifiers for the animations associated with this layer.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("animationKeys"), NullAllowed]
		string [] AnimationKeys { get; }

		/// <param name="key">The animation's identifier.</param>
		///         <summary>Returns the animation associated with the <paramref name="key" />.</summary>
		///         <returns>The <see cref="CoreAnimation.CAAnimation" /> associated with <paramref name="key" /> or <see langword="null" /> if there is no such animation.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("animationForKey:")]
		[return: NullAllowed]
		CAAnimation AnimationForKey (string key);

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("name", ArgumentSemantic.Copy)]
		string Name { get; set; }

		/// <summary>An object that can respond to the delegate protocol for this type</summary>
		///         <value>The instance that will respond to events and data requests.</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>   Methods must be decorated with the [Export ("selectorName")] attribute to respond to each method from the protocol.   Alternatively use the Delegate method which is strongly typed and does not require the [Export] attributes on methods.</para>
		///         </remarks>
		[Export ("delegate", ArgumentSemantic.Weak)]
		[NullAllowed]
		NSObject WeakDelegate { get; [PostSnippet (@"SetCALayerDelegate (value as CALayerDelegate);", Optimizable = true)] set; }

		/// <summary>An instance of the CoreAnimation.ICALayerDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the CoreAnimation.ICALayerDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		ICALayerDelegate Delegate { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("shadowColor")]
		[NullAllowed]
		CGColor ShadowColor { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("shadowOffset")]
		CGSize ShadowOffset { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("shadowOpacity")]
		float ShadowOpacity { get; set; } /* float, not CGFloat */

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("shadowRadius")]
		nfloat ShadowRadius { get; set; }

		/// <summary>Represents the value associated with the constant kCATransition</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCATransition")]
		NSString Transition { get; }

		/// <summary>Represents the value associated with the constant kCAGravityCenter</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAGravityCenter")]
		NSString GravityCenter { get; }

		/// <summary>Represents the value associated with the constant kCAGravityTop</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAGravityTop")]
		NSString GravityTop { get; }

		/// <summary>Represents the value associated with the constant kCAGravityBottom</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAGravityBottom")]
		NSString GravityBottom { get; }

		/// <summary>Represents the value associated with the constant kCAGravityLeft</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAGravityLeft")]
		NSString GravityLeft { get; }

		/// <summary>Represents the value associated with the constant kCAGravityRight</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAGravityRight")]
		NSString GravityRight { get; }

		/// <summary>Represents the value associated with the constant kCAGravityTopLeft</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAGravityTopLeft")]
		NSString GravityTopLeft { get; }

		/// <summary>Represents the value associated with the constant kCAGravityTopRight</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAGravityTopRight")]
		NSString GravityTopRight { get; }

		/// <summary>Represents the value associated with the constant kCAGravityBottomLeft</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAGravityBottomLeft")]
		NSString GravityBottomLeft { get; }

		/// <summary>Represents the value associated with the constant kCAGravityBottomRight</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAGravityBottomRight")]
		NSString GravityBottomRight { get; }

		/// <summary>Represents the value associated with the constant kCAGravityResize</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAGravityResize")]
		NSString GravityResize { get; }

		/// <summary>Represents the value associated with the constant kCAGravityResizeAspect</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAGravityResizeAspect")]
		NSString GravityResizeAspect { get; }

		/// <summary>Represents the value associated with the constant kCAGravityResizeAspectFill</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAGravityResizeAspectFill")]
		NSString GravityResizeAspectFill { get; }

		/// <summary>Represents the value associated with the constant kCAFilterNearest</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAFilterNearest")]
		NSString FilterNearest { get; }

		/// <summary>Represents the value associated with the constant kCAFilterLinear</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAFilterLinear")]
		NSString FilterLinear { get; }

		/// <summary>Represents the value associated with the constant kCAFilterTrilinear</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAFilterTrilinear")]
		NSString FilterTrilinear { get; }

		/// <summary>Represents the value associated with the constant kCAOnOrderIn</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAOnOrderIn")]
		NSString OnOrderIn { get; }

		/// <summary>Represents the value associated with the constant kCAOnOrderOut</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAOnOrderOut")]
		NSString OnOrderOut { get; }

		[MacCatalyst (13, 1)]
		[Internal]
		[Export ("contentsFormat")]
		NSString _ContentsFormat { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("visibleRect")]
		CGRect VisibleRect { get; }

		/// <param name="p">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("scrollPoint:")]
		void ScrollPoint (CGPoint p);

		/// <param name="r">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("scrollRectToVisible:")]
		void ScrollRectToVisible (CGRect r);

		/// <summary>An array of Core Image filters applied to the content of this layer and its sublayers.</summary>
		///         <value>
		///           <para>The default value is <see langword="null" />.</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("filters", ArgumentSemantic.Copy)]
		CIFilter [] Filters { get; set; }

		/// <summary>An array of Core Image filters that are applied to the content behind this layer.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("backgroundFilters", ArgumentSemantic.Copy)]
		CIFilter [] BackgroundFilters { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("style", ArgumentSemantic.Copy), NullAllowed]
		NSDictionary Style { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("minificationFilterBias")]
		float MinificationFilterBias { get; set; } /* float, not CGFloat */

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("autoresizingMask")]
		CAAutoresizingMask AutoresizingMask { get; set; }

		/// <param name="oldSize">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("resizeSublayersWithOldSize:")]
		void ResizeSublayers (CGSize oldSize);

		/// <param name="oldSuperlayerSize">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("resizeWithOldSuperlayerSize:")]
		void Resize (CGSize oldSuperlayerSize);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("constraints")]
		[NullAllowed]
		CAConstraint [] Constraints { get; set; }

		/// <param name="c">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("addConstraint:")]
		void AddConstraint (CAConstraint c);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("shouldRasterize")]
		bool ShouldRasterize { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("shadowPath")]
		CGPath ShadowPath { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("rasterizationScale")]
		nfloat RasterizationScale { get; set; }

		/// <summary>Whether drawing commands are deferred and processed asynchronously on a background thread.</summary>
		///         <value>The default value is <see langword="false" />.</value>
		///         <remarks>To be added.</remarks>
		[Export ("drawsAsynchronously")]
		bool DrawsAsynchronously { get; set; }

		/// <summary>Whether edge antialiasing is allowed.</summary>
		///         <value>
		///           <see langword="true" /> if edge antialisting is allowed. By default, antialiasing is allowed for all edges.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("allowsEdgeAntialiasing")]
		bool AllowsEdgeAntialiasing { get; set; }

		/// <summary>Whether group opacity is allowed.</summary>
		///         <value>
		///           <see langword="true" /> if group opacity is allowed. By default, opacity is allowed for all groups.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("allowsGroupOpacity")]
		bool AllowsGroupOpacity { get; set; }

		/// <summary>A Core Image filter that is used for compositing the layer and the content behind it.</summary>
		///         <value>
		///           <para>The default value is <see langword="null" />, causing
		/// 	  the layer to use source-over compositing.</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("compositingFilter", ArgumentSemantic.Strong)]
		NSObject CompositingFilter { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("maskedCorners", ArgumentSemantic.Assign)]
		CACornerMask MaskedCorners { get; set; }

		[BindAs (typeof (CACornerCurve))]
		[TV (13, 0)]
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("cornerCurve")]
		NSString CornerCurve { get; set; }

		[TV (13, 0)]
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("cornerCurveExpansionFactor:")]
		nfloat GetCornerCurveExpansionFactor ([BindAs (typeof (CACornerCurve))] NSString curve);

		[NoTV]
		[iOS (17, 0)]
		[MacCatalyst (17, 0)]
		[Mac (14, 0)]
		[Export ("wantsExtendedDynamicRangeContent")]
		bool WantsExtendedDynamicRangeContent { get; set; }

		[Mac (15, 0), iOS (18, 0), TV (18, 0), MacCatalyst (18, 0)]
		[Export ("toneMapMode")]
		[BindAs (typeof (CAToneMapMode))]
		NSString ToneMapMode { get; set; }
	}

	[TV (13, 0)]
	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	enum CACornerCurve {
		[DefaultEnumValue]
		[Field ("kCACornerCurveCircular")]
		Circular,
		[Field ("kCACornerCurveContinuous")]
		Continuous,
	}

	[Mac (15, 0), iOS (18, 0), TV (18, 0), MacCatalyst (18, 0)]
	enum CAToneMapMode {
		[DefaultEnumValue]
		[Field ("CAToneMapModeAutomatic")]
		Automatic,
		[Field ("CAToneMapModeNever")]
		Never,
		[Field ("CAToneMapModeIfSupported")]
		IfSupported,
	}

	interface ICAMetalDrawable { }

	/// <summary>Interface that defines a protocol for a display buffer at the metal layer.</summary>
	[Protocol]
	[MacCatalyst (13, 1)]
	interface CAMetalDrawable : MTLDrawable {
		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("texture")]
		IMTLTexture Texture { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("layer")]
		CAMetalLayer Layer { get; }
	}

	/// <summary>A <see cref="CoreAnimation.CALayer" /> that is rendered using Metal functions.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Animation/Reference/CAMetalLayer_Ref/index.html">Apple documentation for <c>CAMetalLayer</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CALayer))]
	interface CAMetalLayer {
		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("device")]
		IMTLDevice Device { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("pixelFormat")]
		MTLPixelFormat PixelFormat { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("framebufferOnly")]
		bool FramebufferOnly { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("drawableSize")]
		CGSize DrawableSize { get; set; }

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("nextDrawable")]
		[return: NullAllowed]
		ICAMetalDrawable NextDrawable ();

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("presentsWithTransaction")]
		bool PresentsWithTransaction { [Bind ("presentsWithTransaction")] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[NoiOS]
		[MacCatalyst (13, 1)]
		[Export ("displaySyncEnabled")]
		bool DisplaySyncEnabled { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("allowsNextDrawableTimeout")]
		bool AllowsNextDrawableTimeout { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("maximumDrawableCount")]
		nuint MaximumDrawableCount { get; set; }

		[TV (13, 0)]
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("colorspace", ArgumentSemantic.Assign)]
		CGColorSpace ColorSpace { get; set; }

		[TV (13, 0)]
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("preferredDevice")]
		IMTLDevice PreferredDevice { get; }

		[iOS (16, 0)]
		[NoTV]
		[MacCatalyst (16, 0)]
		[NullAllowed, Export ("EDRMetadata", ArgumentSemantic.Strong)]
		CAEdrMetadata EdrMetadata { get; set; }

		[NoTV]
		[iOS (16, 0)]
		[MacCatalyst (16, 0)]
		[Export ("wantsExtendedDynamicRangeContent")]
		bool WantsExtendedDynamicRangeContent { get; set; }

		[TV (16, 0)]
		[iOS (16, 0)]
		[MacCatalyst (16, 0)]
		[Mac (13, 0)]
		[Export ("developerHUDProperties", ArgumentSemantic.Copy)]
		[NullAllowed]
		// There's no documentation about which values are valid in this dictionary, so we can't create any strong bindings for it.
		NSDictionary DeveloperHudProperties { get; set; }
	}

	/// <summary>Layer whose content can be provided asynchronously, and with multiple levels of detail.</summary>
	///     <remarks>
	///       <para>
	///    When you want to use one of the CALayer subclasses as your UIView's backing layer, you need to add the following code snippet to your class:
	/// </para>
	///       <example>
	///         <code lang="csharp lang-csharp"><![CDATA[
	/// class MyView : UIView {
	///     //
	///     // This instructs the runtime that whenever a MyView is created
	///     // that it should instantiate a CATiledLayer and assign that to the
	///     // UIView.Layer property
	///     //
	///     [Export ("layerClass")]
	///     public static Class LayerClass () {
	///         return new Class (typeof (CATilerLayer));
	///     }
	/// }
	///   ]]></code>
	///       </example>
	///     </remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GraphicsImaging/Reference/CATiledLayer_class/index.html">Apple documentation for <c>CATiledLayer</c></related>
	[BaseType (typeof (CALayer))]
	interface CATiledLayer {
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("layer"), New, Static]
		CALayer Create ();

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("fadeDuration")]
		double FadeDuration { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("levelsOfDetail")]
		nint LevelsOfDetail { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("levelsOfDetailBias")]
		nint LevelsOfDetailBias { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("tileSize")]
		CGSize TileSize { get; set; }
	}

	/// <summary>A layer that replicates an existing layer, with some attributes (color, transform) altered.</summary>
	///     <remarks>
	///       <para>
	///    When you want to use one of the CALayer subclasses as your UIView's backing layer, you need to add the following code snippet to your class:
	/// </para>
	///       <example>
	///         <code lang="csharp lang-csharp"><![CDATA[
	/// class MyView : UIView {
	///     //
	///     // This instructs the runtime that whenever a MyView is created
	///     // that it should instantiate a CAReplicatorLayer and assign that to the
	///     // UIView.Layer property
	///     //
	///     [Export ("layerClass")]
	///     public static Class LayerClass () {
	///         return new Class (typeof (CAReplicatorLayer));
	///     }
	/// }
	///   ]]></code>
	///       </example>
	///     </remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GraphicsImaging/Reference/CAReplicatorLayer_class/index.html">Apple documentation for <c>CAReplicatorLayer</c></related>
	[BaseType (typeof (CALayer))]
	interface CAReplicatorLayer {
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("layer"), New, Static]
		CALayer Create ();

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("instanceCount")]
		nint InstanceCount { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("instanceDelay")]
		double InstanceDelay { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("instanceTransform")]
		CATransform3D InstanceTransform { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("preservesDepth")]
		bool PreservesDepth { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("instanceColor")]
		[NullAllowed]
		CGColor InstanceColor { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("instanceRedOffset")]
		float InstanceRedOffset { get; set; } /* float, not CGFloat */

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("instanceGreenOffset")]
		float InstanceGreenOffset { get; set; } /* float, not CGFloat */

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("instanceBlueOffset")]
		float InstanceBlueOffset { get; set; } /* float, not CGFloat */

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("instanceAlphaOffset")]
		float InstanceAlphaOffset { get; set; } /* float, not CGFloat */
	}


	/// <summary>Layer used to show portions of another layer.</summary>
	///     <remarks>
	///       <para>
	///    When you want to use one of the CALayer subclasses as your UIView's backing layer, you need to add the following code snippet to your class:
	/// </para>
	///       <example>
	///         <code lang="csharp lang-csharp"><![CDATA[
	/// class MyView : UIView {
	///     //
	///     // This instructs the runtime that whenever a MyView is created
	///     // that it should instantiate a CAScrollLayer and assign that to the
	///     // UIView.Layer property
	///     //
	///     [Export ("layerClass")]
	///     public static Class LayerClass () {
	///         return new Class (typeof (CAScrollLayer));
	///     }
	/// }
	///   ]]></code>
	///       </example>
	///     </remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GraphicsImaging/Reference/CAScrollLayer_class/index.html">Apple documentation for <c>CAScrollLayer</c></related>
	[BaseType (typeof (CALayer))]
	interface CAScrollLayer {
		/// <summary>Creates a new sroll layer with default values.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("layer"), New, Static]
		CALayer Create ();

#if NET
		[Protected]
		[Export ("scrollMode", ArgumentSemantic.Copy)]
		NSString WeakScrollMode { get; set; }

		/// <summary>Gets or sets a string that describes the scroll mode.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		CAScroll ScrollMode {
			[Wrap ("CAScrollExtensions.GetValue (WeakScrollMode)")]
			get;
			[Wrap ("WeakScrollMode = value.GetConstant ()!")]
			set;
		}
#else
		[Export ("scrollMode", ArgumentSemantic.Copy)]
		NSString ScrollMode { get; set; }
#endif

		/// <param name="p">To be added.</param>
		///         <summary>Scrolls the scroll layer to the supplied point.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("scrollToPoint:")]
		void ScrollToPoint (CGPoint p);

		/// <param name="r">To be added.</param>
		///         <summary>Scrolls the scroll layer to include the specified rectangle.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("scrollToRect:")]
		void ScrollToRect (CGRect r);
	}

	/// <summary>Enumerates scrolling directions.</summary>
	enum CAScroll {

		/// <summary>Neither vertical nor horizontal scrolling.</summary>
		[Field ("kCAScrollNone")]
		None,

		/// <summary>Vertical scrolling.</summary>
		[Field ("kCAScrollVertically")]
		Vertically,

		/// <summary>Horizontal scrolling.</summary>
		[Field ("kCAScrollHorizontally")]
		Horizontally,

		/// <summary>Both vertical and horizontal scrolling.</summary>
		[Field ("kCAScrollBoth")]
		Both,
	}

	/// <summary>Draws a bezier curve and composes the result with its first sublayer.</summary>
	///     <remarks>
	///       <para>
	///    When you want to use one of the CALayer subclasses as your UIView's backing layer, you need to add the following code snippet to your class:
	/// </para>
	///       <example>
	///         <code lang="csharp lang-csharp"><![CDATA[
	/// class MyView : UIView {
	///     //
	///     // This instructs the runtime that whenever a MyView is created
	///     // that it should instantiate a CAShapeLayer and assign that to the
	///     // UIView.Layer property
	///     //
	///     [Export ("layerClass")]
	///     public static Class LayerClass () {
	///         return new Class (typeof (CAShapeLayer));
	///     }
	/// }
	///   ]]></code>
	///       </example>
	///     </remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GraphicsImaging/Reference/CAShapeLayer_class/index.html">Apple documentation for <c>CAShapeLayer</c></related>
	[BaseType (typeof (CALayer))]
	interface CAShapeLayer {
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("layer"), New, Static]
		CALayer Create ();

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("path")]
		[NullAllowed]
		CGPath Path { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("fillColor")]
		[NullAllowed]
		CGColor FillColor { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("fillRule", ArgumentSemantic.Copy)]
		NSString FillRule { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("lineCap", ArgumentSemantic.Copy)]
		NSString LineCap { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("lineDashPattern", ArgumentSemantic.Copy)]
		[NullAllowed]
		NSNumber [] LineDashPattern { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("lineDashPhase")]
		nfloat LineDashPhase { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("lineJoin", ArgumentSemantic.Copy)]
		NSString LineJoin { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("lineWidth")]
		nfloat LineWidth { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("miterLimit")]
		nfloat MiterLimit { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("strokeColor")]
		[NullAllowed]
		CGColor StrokeColor { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("strokeStart")]
		nfloat StrokeStart { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("strokeEnd")]
		nfloat StrokeEnd { get; set; }

		/// <summary>Represents the value associated with the constant kCALineJoinMiter</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCALineJoinMiter")]
		NSString JoinMiter { get; }

		/// <summary>Represents the value associated with the constant kCALineJoinRound</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCALineJoinRound")]
		NSString JoinRound { get; }

		/// <summary>Represents the value associated with the constant kCALineJoinBevel</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCALineJoinBevel")]
		NSString JoinBevel { get; }

		/// <summary>Represents the value associated with the constant kCALineCapButt</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCALineCapButt")]
		NSString CapButt { get; }

		/// <summary>Represents the value associated with the constant kCALineCapRound</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCALineCapRound")]
		NSString CapRound { get; }

		/// <summary>Represents the value associated with the constant kCALineCapSquare</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCALineCapSquare")]
		NSString CapSquare { get; }

		/// <summary>Represents the value associated with the constant kCAFillRuleNonZero</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAFillRuleNonZero")]
		NSString FillRuleNonZero { get; }

		/// <summary>Represents the value associated with the constant kCAFillRuleEvenOdd</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAFillRuleEvenOdd")]
		NSString FillRuleEvenOdd { get; }
	}

	/// <summary>3D compositing layer.</summary>
	///     <remarks>
	///       <para>
	///    When you want to use one of the CALayer subclasses as your UIView's backing layer, you need to add the following code snippet to your class:
	/// </para>
	///       <example>
	///         <code lang="csharp lang-csharp"><![CDATA[
	/// class MyView : UIView {
	///     //
	///     // This instructs the runtime that whenever a MyView is created
	///     // that it should instantiate a CATransformLayer and assign that to the
	///     // UIView.Layer property
	///     //
	///     [Export ("layerClass")]
	///     public static Class LayerClass () {
	///         return new Class (typeof (CATransformLayer));
	///     }
	/// }
	///   ]]></code>
	///       </example>
	///     </remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GraphicsImaging/Reference/CATransformLayer_class/index.html">Apple documentation for <c>CATransformLayer</c></related>
	[BaseType (typeof (CALayer))]
	interface CATransformLayer {
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("layer"), New, Static]
		CALayer Create ();

		/// <param name="thePoint">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("hitTest:")]
		CALayer HitTest (CGPoint thePoint);
	}

	enum CATextLayerTruncationMode {
		/// <summary>To be added.</summary>
		[Field ("kCATruncationNone")]
		None,

		/// <summary>To be added.</summary>
		[Field ("kCATruncationStart")]
		Start,

		/// <summary>To be added.</summary>
		[Field ("kCATruncationMiddle")]
		Middle,

		/// <summary>To be added.</summary>
		[Field ("kCATruncationEnd")]
		End,
	}

	enum CATextLayerAlignmentMode {
		/// <summary>To be added.</summary>
		[Field ("kCAAlignmentLeft")]
		Left,

		/// <summary>To be added.</summary>
		[Field ("kCAAlignmentRight")]
		Right,

		/// <summary>To be added.</summary>
		[Field ("kCAAlignmentCenter")]
		Center,

		/// <summary>To be added.</summary>
		[Field ("kCAAlignmentJustified")]
		Justified,

		/// <summary>To be added.</summary>
		[Field ("kCAAlignmentNatural")]
		Natural,
	}

	/// <summary>Simple text layour and rendering of regular or attributed text.</summary>
	///     <remarks>
	///       <para>
	///    When you want to use one of the CALayer subclasses as your UIView's backing layer, you need to add the following code snippet to your class:
	/// </para>
	///       <example>
	///         <code lang="csharp lang-csharp"><![CDATA[
	/// class MyView : UIView {
	///     //
	///     // This instructs the runtime that whenever a MyView is created
	///     // that it should instantiate a CATextLayer and assign that to the
	///     // UIView.Layer property
	///     //
	///     [Export ("layerClass")]
	///     public static Class LayerClass () {
	///         return new Class (typeof (CATextLayer));
	///     }
	/// }
	///   ]]></code>
	///       </example>
	///     </remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GraphicsImaging/Reference/CATextLayer_class/index.html">Apple documentation for <c>CATextLayer</c></related>
	[BaseType (typeof (CALayer))]
	interface CATextLayer {
		/// <summary>Creates and returns a new <see cref="CoreAnimation.CALayer" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("layer"), New, Static]
		CALayer Create ();

		/// <summary>Gets the text that will be rendered.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("string", ArgumentSemantic.Copy)]
		string String { get; set; }

		[Sealed]
		[Internal]
		[NullAllowed] // by default this property is null
		[Export ("string", ArgumentSemantic.Copy)]
		IntPtr _AttributedString { get; set; }

		/// <summary>Gets or sets the font size.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("fontSize")]
		nfloat FontSize { get; set; }

		[Export ("font"), Internal]
		IntPtr _Font { get; set; }

		/// <summary>Gets or sets the foreground color.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("foregroundColor")]
		[NullAllowed]
		CGColor ForegroundColor { get; set; }

		/// <summary>Gets or sets a Boolean value that controls whether to wrap text to the bounds of the layer.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("wrapped")]
		bool Wrapped { [Bind ("isWrapped")] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Protected]
		[Export ("truncationMode", ArgumentSemantic.Copy)]
		NSString WeakTruncationMode { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Protected]
		[Export ("alignmentMode", ArgumentSemantic.Copy)]
		NSString WeakAlignmentMode { get; set; }

#if !NET // Use smart enums instead, CATruncationMode and CATextLayerAlignmentMode.
		[Obsolete ("Use 'CATextLayerTruncationMode.None.GetConstant ()' instead.")]
		[Static]
		[Wrap ("CATextLayerTruncationMode.None.GetConstant ()")]
		NSString TruncationNone { get; }

		[Obsolete ("Use 'CATextLayerTruncationMode.Start.GetConstant ()' instead.")]
		[Static]
		[Wrap ("CATextLayerTruncationMode.Start.GetConstant ()")]
		NSString TruncantionStart { get; }

		[Obsolete ("Use 'CATextLayerTruncationMode.End.GetConstant ()' instead.")]
		[Static]
		[Wrap ("CATextLayerTruncationMode.End.GetConstant ()")]
		NSString TruncantionEnd { get; }

		[Obsolete ("Use 'CATextLayerTruncationMode.Middle.GetConstant ()' instead.")]
		[Static]
		[Wrap ("CATextLayerTruncationMode.Middle.GetConstant ()")]
		NSString TruncantionMiddle { get; }

		[Obsolete ("Use 'CATextLayerAlignmentMode.Natural.GetConstant ()' instead.")]
		[Static]
		[Wrap ("CATextLayerAlignmentMode.Natural.GetConstant ()")]
		NSString AlignmentNatural { get; }

		[Obsolete ("Use 'CATextLayerAlignmentMode.Left.GetConstant ()' instead.")]
		[Static]
		[Wrap ("CATextLayerAlignmentMode.Left.GetConstant ()")]
		NSString AlignmentLeft { get; }

		[Obsolete ("Use 'CATextLayerAlignmentMode.Right.GetConstant ()' instead.")]
		[Static]
		[Wrap ("CATextLayerAlignmentMode.Right.GetConstant ()")]
		NSString AlignmentRight { get; }

		[Obsolete ("Use 'CATextLayerAlignmentMode.Center.GetConstant ()' instead.")]
		[Static]
		[Wrap ("CATextLayerAlignmentMode.Center.GetConstant ()")]
		NSString AlignmentCenter { get; }

		[Obsolete ("Use 'CATextLayerAlignmentMode.Justified.GetConstant ()' instead.")]
		[Static]
		[Wrap ("CATextLayerAlignmentMode.Justified.GetConstant ()")]
		NSString AlignmentJustified { get; }
#endif // !NET

		/// <summary>Gets or sets a Boolean value that controls whether subpixel quantization is allowed.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("allowsFontSubpixelQuantization")]
		bool AllowsFontSubpixelQuantization { get; set; }
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="CoreAnimation.CALayerDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="CoreAnimation.CALayerDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="CoreAnimation.CALayerDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="CoreAnimation.CALayerDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface ICALayerDelegate { }

	/// <summary>Delegate class for the CALayer.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CoreAnimation/CALayerDelegate">Apple documentation for <c>CALayerDelegate</c></related>
	[BaseType (typeof (NSObject))]
	[Model]
#if IOS || TVOS
	[Protocol (FormalSince = "10.0")]
#elif MONOMAC
	[Protocol (FormalSince = "10.12")]
#else
	[Protocol]
#endif
	interface CALayerDelegate {
		/// <param name="layer">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("displayLayer:")]
		void DisplayLayer (CALayer layer);

		/// <param name="layer">To be added.</param>
		///         <param name="context">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("drawLayer:inContext:"), EventArgs ("CALayerDrawEventArgs")]
		void DrawLayer (CALayer layer, CGContext context);

		/// <param name="layer">The layer that will be redrawn.</param>
		///         <summary>Method that is called when <paramref name="layer" /> is about to be drawn.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("layerWillDraw:")]
		void WillDrawLayer (CALayer layer);

		/// <param name="layer">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("layoutSublayersOfLayer:")]
		void LayoutSublayersOfLayer (CALayer layer);

		/// <param name="layer">To be added.</param>
		///         <param name="eventKey">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("actionForLayer:forKey:"), EventArgs ("CALayerDelegateAction"), DefaultValue (null)]
		[return: NullAllowed]
		NSObject ActionForLayer (CALayer layer, string eventKey);
	}

#if HAS_OPENGLES
	/// <summary>Layer
	///  used to render OpenGL content.</summary>
	///     <remarks>
	///       <para>
	///    When you want to use one of the CALayer subclasses as your UIView's backing layer, you need to add the following code snippet to your class:
	/// </para>
	///       <example>
	///         <code lang="csharp lang-csharp"><![CDATA[
	/// class MyView : UIView {
	///     //
	///     // This instructs the runtime that whenever a MyView is created
	///     // that it should instantiate a CAEAGLLayer and assign that to the
	///     // UIView.Layer property
	///     //
	///     [Export ("layerClass")]
	///     public static Class LayerClass () {
	///         return new Class (typeof (CAEAGLLayer));
	///     }
	/// }
	///   ]]></code>
	///       </example>
	///     </remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/QuartzCore/Reference/CAEAGLLayer_Class/index.html">Apple documentation for <c>CAEAGLLayer</c></related>
	[NoMac]
	[NoMacCatalyst]
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'CAMetalLayer' instead.")]
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'CAMetalLayer' instead.")]
	[BaseType (typeof (CALayer))]
	interface CAEAGLLayer : EAGLDrawable {
		/// <summary>Creates and returns a new <see cref="CoreAnimation.CALayer" />.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("layer"), New, Static]
		CALayer Create ();

		/// <summary>Gets or sets a Boolean value that controls whether the layer is presented synchronously or asynchronously. <see langword="true" /> indicates synchronous presentation. Default is <see langword="false" /></summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("presentsWithTransaction")]
		bool PresentsWithTransaction { get; set; }
	}
#endif

	/// <summary>An interface implemented by objects that participate in animations coordinated by a CALayer.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GraphicsImaging/Reference/CAAction_protocol/index.html">Apple documentation for <c>CAAction</c></related>
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	[DisableDefaultCtor]
	interface CAAction {
		/// <param name="eventKey">To be added.</param>
		///         <param name="obj">To be added.</param>
		///         <param name="arguments">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("runActionForKey:object:arguments:")]
		void RunAction (string eventKey, NSObject obj, [NullAllowed] NSDictionary arguments);
	}

	/// <include file="../docs/api/CoreAnimation/CAAnimation.xml" path="/Documentation/Docs[@DocId='T:CoreAnimation.CAAnimation']/*" />
	[BaseType (typeof (NSObject)
#if NET
		, Delegates = new string [] { "WeakDelegate" }, Events = new Type [] { typeof (CAAnimationDelegate) }
#endif
	)]
	interface CAAnimation : CAAction, CAMediaTiming, NSSecureCoding, NSMutableCopying, SCNAnimationProtocol {
		/// <summary>Creates a new animation, you will use the derived classes static method instead.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("animation"), Static]
		CAAnimation CreateAnimation ();

		/// <param name="key">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>The default value used for the given object.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("defaultValueForKey:")]
		[return: NullAllowed]
		NSObject DefaultValue (string key);

		/// <summary>The timing function used to compute the animation.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("timingFunction", ArgumentSemantic.Strong)]
		CAMediaTimingFunction TimingFunction { get; set; }

#if NET
		// before that we need to be wrap this manually to avoid the BI1110 error
		/// <summary>An instance of the CoreAnimation.CAAnimationDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the CoreAnimation.CAAnimationDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		ICAAnimationDelegate Delegate { get; set; }
#endif

		/// <summary>An object that can respond to the delegate protocol for this type</summary>
		///         <value>The instance that will respond to events and data requests.</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>   Methods must be decorated with the [Export ("selectorName")] attribute to respond to each method from the protocol.   Alternatively use the Delegate method which is strongly typed and does not require the [Export] attributes on methods.</para>
		///         </remarks>
		[Export ("delegate", ArgumentSemantic.Strong)]
		[NullAllowed]
		NSObject WeakDelegate { get; set; }

		/// <summary>Whether the animation is removed from the layer on completion.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("removedOnCompletion")]
		bool RemovedOnCompletion { [Bind ("isRemovedOnCompletion")] get; set; }

		/// <param name="key">To be added.</param>
		///         <summary>With key-value observing, indicates that the value associated with <paramref name="key" /> is about to change.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("willChangeValueForKey:")]
		void WillChangeValueForKey (string key);

		/// <param name="key">To be added.</param>
		///         <summary>As part of key-value observing, indicates that the value represented by <paramref name="key" /> has changed.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("didChangeValueForKey:")]
		void DidChangeValueForKey (string key);

		/// <param name="key">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Whether the value for the given key should be archived.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("shouldArchiveValueForKey:")]
		bool ShouldArchiveValueForKey (string key);

		/// <summary>Represents the value associated with the constant kCATransitionFade</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCATransitionFade")]
		NSString TransitionFade { get; }

		/// <summary>Represents the value associated with the constant kCATransitionMoveIn</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCATransitionMoveIn")]
		NSString TransitionMoveIn { get; }

		/// <summary>Represents the value associated with the constant kCATransitionPush</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCATransitionPush")]
		NSString TransitionPush { get; }

		/// <summary>Represents the value associated with the constant kCATransitionReveal</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCATransitionReveal")]
		NSString TransitionReveal { get; }

		/// <summary>Represents the value associated with the constant kCATransitionFromRight</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCATransitionFromRight")]
		NSString TransitionFromRight { get; }

		/// <summary>Represents the value associated with the constant kCATransitionFromLeft</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCATransitionFromLeft")]
		NSString TransitionFromLeft { get; }

		/// <summary>Represents the value associated with the constant kCATransitionFromTop</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCATransitionFromTop")]
		NSString TransitionFromTop { get; }

		/// <summary>Represents the value associated with the constant kCATransitionFromBottom</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCATransitionFromBottom")]
		NSString TransitionFromBottom { get; }

		/* 'calculationMode' strings. */
		/// <summary>Represents the value associated with the constant kCAAnimationLinear</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAAnimationLinear")]
		NSString AnimationLinear { get; }

#if !NET
		[Field ("kCAAnimationDiscrete")]
		[Obsolete ("The name has been fixed, use 'AnimationDiscrete' instead.")]
		NSString AnimationDescrete { get; }
#endif
		/// <summary>Represents the value associated with the constant kCAAnimationDiscrete</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAAnimationDiscrete")]
		NSString AnimationDiscrete { get; }

		/// <summary>Represents the value associated with the constant kCAAnimationPaced</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAAnimationPaced")]
		NSString AnimationPaced { get; }

		/// <summary>Represents the value associated with the constant kCAAnimationCubic</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAAnimationCubic")]
		NSString AnimationCubic { get; }

		/// <summary>Represents the value associated with the constant kCAAnimationCubicPaced</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAAnimationCubicPaced")]
		NSString AnimationCubicPaced { get; }

		/* 'rotationMode' strings. */
		/// <summary>Represents the value associated with the constant kCAAnimationRotateAuto</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAAnimationRotateAuto")]
		NSString RotateModeAuto { get; }

		/// <summary>Represents the value associated with the constant kCAAnimationRotateAutoReverse</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAAnimationRotateAutoReverse")]
		NSString RotateModeAutoReverse { get; }

		#region SceneKitAdditions

		/// <param name="animation">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("animationWithSCNAnimation:")]
		CAAnimation FromSCNAnimation (SCNAnimation animation);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("usesSceneTimeBase")]
		bool UsesSceneTimeBase { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("fadeInDuration")]
		nfloat FadeInDuration { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("fadeOutDuration")]
		nfloat FadeOutDuration { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed] // by default this property is null
		[Export ("animationEvents", ArgumentSemantic.Retain)]
		SCNAnimationEvent [] AnimationEvents { get; set; }

		#endregion

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("preferredFrameRateRange", ArgumentSemantic.Assign)]
		CAFrameRateRange PreferredFrameRateRange { get; set; }
	}

	interface ICAAnimationDelegate { }

	/// <summary>Delegate for responding to animation lifecycle events.</summary>
	[BaseType (typeof (NSObject))]
#if IOS || TVOS
	[Protocol (FormalSince = "10.0")]
#elif MONOMAC
	[Protocol (FormalSince = "10.12")]
#else
	[Synthetic]
#endif
	[Model]
	interface CAAnimationDelegate {
		/// <param name="anim">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>The animation has started.</summary>
		///         <remarks>To be added.</remarks>
		[EventArgs ("", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		[Export ("animationDidStart:")]
		void AnimationStarted (CAAnimation anim);

		/// <param name="anim">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="finished">To be added.</param>
		///         <summary>The animation has stopped.  Use the bool value to determine if this is a temporary pause, or the end of the animation.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("animationDidStop:finished:"), EventArgs ("CAAnimationState", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void AnimationStopped (CAAnimation anim, bool finished);

	}

	/// <summary>An animation that can animate object properties.</summary>
	///     <remarks>For a list of common properties to animate, see the documentation for <see cref="CoreAnimation.CAPropertyAnimation.KeyPath" /></remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GraphicsImaging/Reference/CAPropertyAnimation_class/index.html">Apple documentation for <c>CAPropertyAnimation</c></related>
	[BaseType (typeof (CAAnimation))]
	interface CAPropertyAnimation {
		/// <include file="../docs/api/CoreAnimation/CAPropertyAnimation.xml" path="/Documentation/Docs[@DocId='M:CoreAnimation.CAPropertyAnimation.FromKeyPath(System.String)']/*" />
		[Static]
		[Export ("animationWithKeyPath:")]
		CAPropertyAnimation FromKeyPath ([NullAllowed] string path);

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("keyPath", ArgumentSemantic.Copy)]
		string KeyPath { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("additive")]
		bool Additive { [Bind ("isAdditive")] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("cumulative")]
		bool Cumulative { [Bind ("isCumulative")] get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("valueFunction", ArgumentSemantic.Strong)]
		CAValueFunction ValueFunction { get; set; }
	}

	/// <include file="../docs/api/CoreAnimation/CABasicAnimation.xml" path="/Documentation/Docs[@DocId='T:CoreAnimation.CABasicAnimation']/*" />
	[BaseType (typeof (CAPropertyAnimation))]
	interface CABasicAnimation {
		/// <param name="path">
		///           <para>A string representing the keypath for the animation.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Creates an animation from the given key path.</summary>
		///         <returns>The new animation.</returns>
		///         <remarks>See the base class FromKeyPath for information on the values for the key path.</remarks>
		[Static, New, Export ("animationWithKeyPath:")]
		CABasicAnimation FromKeyPath ([NullAllowed] string path);

		[Export ("fromValue", ArgumentSemantic.Strong)]
		[Internal]
		[Sealed]
		IntPtr _From { get; set; }

		/// <summary>Initial value for the property to animate.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("fromValue", ArgumentSemantic.Strong)]
		[NullAllowed]
		NSObject From { get; set; }

		[Export ("toValue", ArgumentSemantic.Strong)]
		[Internal]
		[Sealed]
		IntPtr _To { get; set; }

		/// <summary>Destination value for the property to animate.</summary>
		///         <value>
		///           <para>Final value that the property will have.</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Export ("toValue", ArgumentSemantic.Strong)]
		[NullAllowed]
		NSObject To { get; set; }

		[Export ("byValue", ArgumentSemantic.Strong)]
		[Internal]
		[Sealed]
		IntPtr _By { get; set; }

		/// <summary>Value to increment by.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("byValue", ArgumentSemantic.Strong)]
		[NullAllowed]
		NSObject By { get; set; }
	}

	/// <summary>A spring animation with stiffness, mass, and damping.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CoreAnimation/CASpringAnimation">Apple documentation for <c>CASpringAnimation</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CABasicAnimation))]
	interface CASpringAnimation {
		/// <param name="path">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Creates a new animation from the specified key path.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static, New, Export ("animationWithKeyPath:")]
		CABasicAnimation FromKeyPath ([NullAllowed] string path);

		/// <summary>Gets or sets the mass for the animation.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("mass")]
		nfloat Mass { get; set; }

		/// <summary>Gets or sets the stiffness of the spring for the animation.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("stiffness")]
		nfloat Stiffness { get; set; }

		/// <summary>Gets or sets the damping for the spring animation.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("damping")]
		nfloat Damping { get; set; }

		/// <summary>Gets or sets the initial velocity of the animation.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("initialVelocity")]
		nfloat InitialVelocity { get; set; }

		/// <summary>Gets the settling time for the animation.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[iOS (17, 0), TV (17, 0), MacCatalyst (17, 0), Mac (14, 0)]
		[Export ("settlingDuration")]
		double /* CFTimeInterval */ SettlingDuration { get; }

		[iOS (17, 0), TV (17, 0), MacCatalyst (17, 0), Mac (14, 0)]
		[Export ("allowsOverdamping")]
		bool AllowsOverdamping { get; set; }

		[iOS (17, 0), TV (17, 0), MacCatalyst (17, 0), Mac (14, 0)]
		[Export ("initWithPerceptualDuration:bounce:")]
		NativeHandle Constructor (double /* CFTimeInterval */ perceptualDuration, nfloat bounce);

		[iOS (17, 0), TV (17, 0), MacCatalyst (17, 0), Mac (14, 0)]
		[Export ("perceptualDuration")]
		double /* CFTimeInterval */ PerceptualDuration { get; }

		[iOS (17, 0), TV (17, 0), MacCatalyst (17, 0), Mac (14, 0)]
		[Export ("bounce")]
		nfloat Bounce { get; }
	}

	/// <summary>Keyframe-based animation support.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GraphicsImaging/Reference/CAKeyframeAnimation_class/index.html">Apple documentation for <c>CAKeyframeAnimation</c></related>
	[BaseType (typeof (CAPropertyAnimation), Name = "CAKeyframeAnimation")]
	interface CAKeyFrameAnimation {
		/// <param name="path">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static, Export ("animationWithKeyPath:")]
		CAKeyFrameAnimation FromKeyPath ([NullAllowed] string path);

		/// <summary>Sets the key frame animation values.</summary>
		///         <value>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>
		///           <para>
		/// 	    To pass number, create instances of <see cref="Foundation.NSNumber" /> with the value,
		/// 	    to pass other values, use <see cref="Foundation.NSValue" />.
		///
		/// 	  </para>
		///           <para>
		/// 	    If you need to pass CoreGraphics objects, or retrieve
		/// 	    elements from the key frame that might contain CoreGraphics
		/// 	    objects, you should use the <see cref="CoreAnimation.CAKeyFrameAnimation.SetValues" /> and
		/// 	    <see cref="CoreAnimation.CAKeyFrameAnimation.GetValuesAs{T}" /> methods instead.
		///
		/// 	  </para>
		///         </remarks>
		[NullAllowed] // by default this property is null
		[Export ("values", ArgumentSemantic.Copy)]
		NSObject [] Values { get; set; }

		[Export ("values", ArgumentSemantic.Strong)]
		[Internal]
		[Sealed]
		NSArray _Values { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("path")]
		CGPath Path { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("keyTimes", ArgumentSemantic.Copy)]
		[NullAllowed]
		NSNumber [] KeyTimes { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("timingFunctions", ArgumentSemantic.Copy)]
		CAMediaTimingFunction [] TimingFunctions { get; set; }

		[Export ("calculationMode", ArgumentSemantic.Copy)]
		[Internal]
		NSString _CalculationMode { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("rotationMode", ArgumentSemantic.Copy)]
		[NullAllowed]
		string RotationMode { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("tensionValues", ArgumentSemantic.Copy)]
		NSNumber [] TensionValues { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("continuityValues", ArgumentSemantic.Copy)]
		NSNumber [] ContinuityValues { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("biasValues", ArgumentSemantic.Copy)]
		NSNumber [] BiasValues { get; set; }
	}

	/// <summary>Transition animations for a layer.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GraphicsImaging/Reference/CATransition_class/index.html">Apple documentation for <c>CATransition</c></related>
	[BaseType (typeof (CAAnimation))]
	interface CATransition {
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("animation"), Static, New]
		CATransition CreateAnimation ();

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("type", ArgumentSemantic.Copy)]
		string Type { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("subtype", ArgumentSemantic.Copy)]
		string Subtype { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("startProgress")]
		float StartProgress { get; set; } /* float, not CGFloat */

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("endProgress")]
		float EndProgress { get; set; } /* float, not CGFloat */

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("filter", ArgumentSemantic.Strong)]
		[NullAllowed]
		NSObject Filter { get; set; }
	}

	/// <summary>Constants used for the FillMode property in CAAnimation and CALayer, used to control the behavior of objects once the animation has completed.</summary>
	///     <remarks>These are used in the FillMode property of CAAnimation and CALayer.</remarks>
	[Static]
	interface CAFillMode {
		/// <summary>Represents the value associated with the constant kCAFillModeForwards</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAFillModeForwards")]
		NSString Forwards { get; }

		/// <summary>Represents the value associated with the constant kCAFillModeBackwards</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAFillModeBackwards")]
		NSString Backwards { get; }

		/// <summary>Represents the value associated with the constant kCAFillModeBoth</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAFillModeBoth")]
		NSString Both { get; }

		/// <summary>Represents the value associated with the constant kCAFillModeRemoved</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAFillModeRemoved")]
		NSString Removed { get; }
	}

	/// <summary>Framework to synchronize multiple transformation operations.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GraphicsImaging/Reference/CATransaction_class/index.html">Apple documentation for <c>CATransaction</c></related>
	[BaseType (typeof (NSObject))]
	interface CATransaction {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("begin")]
		void Begin ();

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("commit")]
		void Commit ();

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("flush")]
		void Flush ();

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("lock")]
		void Lock ();

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("unlock")]
		void Unlock ();

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("animationDuration")]
		double AnimationDuration { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Static, NullAllowed]
		[Export ("animationTimingFunction")]
		CAMediaTimingFunction AnimationTimingFunction { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("disableActions")]
		bool DisableActions { get; set; }

		/// <param name="key">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("valueForKey:")]
		[return: NullAllowed]
		NSObject ValueForKey (NSString key);

		/// <param name="anObject">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="key">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("setValue:forKey:")]
		void SetValueForKey ([NullAllowed] NSObject anObject, NSString key);

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Static, Export ("completionBlock"), NullAllowed]
		Action CompletionBlock { get; set; }

		/// <summary>Represents the value associated with the constant kCATransactionAnimationDuration</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCATransactionAnimationDuration")]
		NSString AnimationDurationKey { get; }

		/// <summary>Represents the value associated with the constant kCATransactionDisableActions</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCATransactionDisableActions")]
		NSString DisableActionsKey { get; }

		/// <summary>Represents the value associated with the constant kCATransactionAnimationTimingFunction</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCATransactionAnimationTimingFunction")]
		NSString TimingFunctionKey { get; }

		/// <summary>Represents the value associated with the constant kCATransactionCompletionBlock</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCATransactionCompletionBlock")]
		NSString CompletionBlockKey { get; }
	}

	/// <summary>Groups and orchestrates multiple animations.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GraphicsImaging/Reference/CAAnimationGroup_class/index.html">Apple documentation for <c>CAAnimationGroup</c></related>
	[BaseType (typeof (CAAnimation))]
	interface CAAnimationGroup {
		/// <summary>An array of the animations in this group.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("animations", ArgumentSemantic.Copy)]
		CAAnimation [] Animations { get; set; }

		/// <summary>Factory method that creates a new CAAnimationGroup.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("animation"), Static, New]
		CAAnimationGroup CreateAnimation ();
	}

	/// <summary>Layer that renders a gradient over its background.</summary>
	///     <remarks>
	///       <para>
	///    When you want to use one of the CALayer subclasses as your UIView's backing layer, you need to add the following code snippet to your class:
	/// </para>
	///       <example>
	///         <code lang="csharp lang-csharp"><![CDATA[
	/// class MyView : UIView {
	///     //
	///     // This instructs the runtime that whenever a MyView is created
	///     // that it should instantiate a CAGradientLayer and assign that to the
	///     // UIView.Layer property
	///     //
	///     [Export ("layerClass")]
	///     public static Class LayerClass () {
	///         return new Class (typeof (CAGradientLayer));
	///     }
	/// }
	///   ]]></code>
	///       </example>
	///     </remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GraphicsImaging/Reference/CAGradientLayer_class/index.html">Apple documentation for <c>CAGradientLayer</c></related>
	[BaseType (typeof (CALayer))]
	interface CAGradientLayer {
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("layer"), New, Static]
		CALayer Create ();

		[NullAllowed] // by default this property is null
		[Export ("colors", ArgumentSemantic.Copy)]
		[Internal]
		IntPtr _Colors { get; set; }

		/// <summary>Optional array of NSNumbers that describe the position of each gradient stop.   This property can be animated.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("locations", ArgumentSemantic.Copy)]
		NSNumber [] Locations { get; set; }

		/// <summary>Starting point for the gradient.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("startPoint")]
		CGPoint StartPoint { get; set; }

		/// <summary>The end point for the gradient.   This property can be animated.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("endPoint")]
		CGPoint EndPoint { get; set; }

#if NET
		/// <summary>The gradient type displayed.</summary>
		CAGradientLayerType LayerType {
			[Wrap ("CAGradientLayerTypeExtensions.GetValue (WeakLayerType)")]
			get;
			[Wrap ("WeakLayerType = value.GetConstant ()!")]
			set;
		}

		[Export ("type", ArgumentSemantic.Copy)]
		NSString WeakLayerType { get; set; }
#else
		CAGradientLayerType LayerType {
			[Wrap ("CAGradientLayerTypeExtensions.GetValue ((NSString) Type)")]
			get;
			[Wrap ("Type = value.GetConstant ()")]
			set;
		}

		[Obsolete ("Use 'LayerType' property instead.")]
		[Export ("type", ArgumentSemantic.Copy)]
		string Type { get; set; }

		[Obsolete ("Use 'CAGradientLayerType.Axial' enum instead.")]
		[Field ("kCAGradientLayerAxial")]
		NSString GradientLayerAxial { get; }
#endif
	}

	enum CAGradientLayerType {
		/// <summary>To be added.</summary>
		[Field ("kCAGradientLayerAxial")]
		Axial,

		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		[Field ("kCAGradientLayerRadial")]
		Radial,

		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		[Field ("kCAGradientLayerConic")]
		Conic,
	}

	/// <summary>Defines the pacing of an animation.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Cocoa/Reference/CAMediaTimingFunction_class/index.html">Apple documentation for <c>CAMediaTimingFunction</c></related>
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CAMediaTimingFunction : NSSecureCoding {
		/// <param name="name">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("functionWithName:")]
		[Static]
		CAMediaTimingFunction FromName (NSString name);

		/// <param name="c1x">To be added.</param>
		///         <param name="c1y">To be added.</param>
		///         <param name="c2x">To be added.</param>
		///         <param name="c2y">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("functionWithControlPoints::::")]
		CAMediaTimingFunction FromControlPoints (float c1x, float c1y, float c2x, float c2y); /* all float, not CGFloat */

		/// <param name="c1x">To be added.</param>
		/// <param name="c1y">To be added.</param>
		/// <param name="c2x">To be added.</param>
		/// <param name="c2y">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithControlPoints::::")]
		NativeHandle Constructor (float c1x, float c1y, float c2x, float c2y); /* all float, not CGFloat */

		[Export ("getControlPointAtIndex:values:"), Internal]
		void GetControlPointAtIndex (nint idx, IntPtr /* float[2] */ point);

		/// <summary>Represents the value associated with the constant kCAMediaTimingFunctionLinear</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAMediaTimingFunctionLinear")]
		NSString Linear { get; }

		/// <summary>Represents the value associated with the constant kCAMediaTimingFunctionEaseIn</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAMediaTimingFunctionEaseIn")]
		NSString EaseIn { get; }

		/// <summary>Represents the value associated with the constant kCAMediaTimingFunctionEaseOut</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAMediaTimingFunctionEaseOut")]
		NSString EaseOut { get; }

		/// <summary>Represents the value associated with the constant kCAMediaTimingFunctionEaseInEaseOut</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAMediaTimingFunctionEaseInEaseOut")]
		NSString EaseInEaseOut { get; }

		/// <summary>Represents the value associated with the constant kCAMediaTimingFunctionDefault</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAMediaTimingFunctionDefault")]
		NSString Default { get; }
	}

	/// <summary>Class used to apply functions to property values during an animation.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GraphicsImaging/Reference/CAValueFunction_class/index.html">Apple documentation for <c>CAValueFunction</c></related>
	[BaseType (typeof (NSObject))]
	interface CAValueFunction : NSSecureCoding {
		/// <param name="name">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("functionWithName:"), Static]
		[return: NullAllowed]
		CAValueFunction FromName (string name);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("name")]
		string Name { get; }

		/// <summary>Represents the value associated with the constant kCAValueFunctionRotateX</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAValueFunctionRotateX")]
		NSString RotateX { get; }

		/// <summary>Represents the value associated with the constant kCAValueFunctionRotateY</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAValueFunctionRotateY")]
		NSString RotateY { get; }

		/// <summary>Represents the value associated with the constant kCAValueFunctionRotateZ</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAValueFunctionRotateZ")]
		NSString RotateZ { get; }

		/// <summary>Represents the value associated with the constant kCAValueFunctionScale</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAValueFunctionScale")]
		NSString Scale { get; }

		/// <summary>Represents the value associated with the constant kCAValueFunctionScaleX</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAValueFunctionScaleX")]
		NSString ScaleX { get; }

		/// <summary>Represents the value associated with the constant kCAValueFunctionScaleY</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAValueFunctionScaleY")]
		NSString ScaleY { get; }

		/// <summary>Represents the value associated with the constant kCAValueFunctionScaleZ</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAValueFunctionScaleZ")]
		NSString ScaleZ { get; }

		/// <summary>Represents the value associated with the constant kCAValueFunctionTranslate</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAValueFunctionTranslate")]
		NSString Translate { get; }

		/// <summary>Represents the value associated with the constant kCAValueFunctionTranslateX</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAValueFunctionTranslateX")]
		NSString TranslateX { get; }

		/// <summary>Represents the value associated with the constant kCAValueFunctionTranslateY</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAValueFunctionTranslateY")]
		NSString TranslateY { get; }

		/// <summary>Represents the value associated with the constant kCAValueFunctionTranslateZ</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAValueFunctionTranslateZ")]
		NSString TranslateZ { get; }

	}

	[NoiOS]
	[NoTV]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'CAMetalLayer' instead.")]
	[NoMacCatalyst]
	[BaseType (typeof (CALayer))]
	interface CAOpenGLLayer {
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("layer"), New, Static]
		CALayer Create ();

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("asynchronous")]
		bool Asynchronous { [Bind ("isAsynchronous")] get; set; }

		/// <param name="glContext">To be added.</param>
		///         <param name="pixelFormat">To be added.</param>
		///         <param name="timeInterval">To be added.</param>
		///         <param name="timeStamp">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("canDrawInCGLContext:pixelFormat:forLayerTime:displayTime:")]
		bool CanDrawInCGLContext (CGLContext glContext, CGLPixelFormat pixelFormat, double timeInterval, ref CVTimeStamp timeStamp);

		/// <param name="glContext">To be added.</param>
		///         <param name="pixelFormat">To be added.</param>
		///         <param name="timeInterval">To be added.</param>
		///         <param name="timeStamp">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("drawInCGLContext:pixelFormat:forLayerTime:displayTime:")]
		void DrawInCGLContext (CGLContext glContext, CGLPixelFormat pixelFormat, double timeInterval, ref CVTimeStamp timeStamp);

		/// <param name="mask">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("copyCGLPixelFormatForDisplayMask:")]
		CGLPixelFormat CopyCGLPixelFormatForDisplayMask (UInt32 mask);

		/// <param name="pixelFormat">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("releaseCGLPixelFormat:")]
		void Release (CGLPixelFormat pixelFormat);

		/// <param name="pixelFormat">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("copyCGLContextForPixelFormat:")]
		CGLContext CopyContext (CGLPixelFormat pixelFormat);

		/// <param name="glContext">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("releaseCGLContext:")]
		void Release (CGLContext glContext);
	}

	/// <summary>A source of particles emitted by a <see cref="CoreAnimation.CAEmitterLayer" /> instance.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GraphicsImaging/Reference/CAEmitterCell_class/index.html">Apple documentation for <c>CAEmitterCell</c></related>
	[BaseType (typeof (NSObject))]
	interface CAEmitterCell : CAMediaTiming, NSSecureCoding {
		/// <summary>Gets or sets a name for the cell that can be used in animation keypaths.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("name", ArgumentSemantic.Copy)]
		string Name { get; set; }

		/// <summary>Gets or sets a Boolean value that controls whether emitted cells are rendered.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }

		/// <summary>Gets or sets the frequeny, in Hz, at which particles are emitted.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("birthRate")]
		float BirthRate { get; set; } /* float, not CGFloat */

		/// <summary>Gets or sets the average particle lifetime.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("lifetime")]
		float LifeTime { get; set; } /* float, not CGFloat */

		/// <summary>Gets or sets the average amount of variation in the particle lifetime.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("lifetimeRange")]
		float LifetimeRange { get; set; } /* float, not CGFloat */

		/// <summary>Gets the angle from the Z axis at which particles are emitted.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("emissionLatitude")]
		nfloat EmissionLatitude { get; set; }

		/// <summary>Gets or sets the azimuth angle of the emitted particles.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("emissionLongitude")]
		nfloat EmissionLongitude { get; set; }

		/// <summary>Gets or sets the average amount of variation in the particle emission rate.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("emissionRange")]
		nfloat EmissionRange { get; set; }

		/// <summary>Gets or sets the initial velocity of the emitter cell.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("velocity")]
		nfloat Velocity { get; set; }

		/// <summary>Gets or sets the average value by which the velocity can differ from its nominal value.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("velocityRange")]
		nfloat VelocityRange { get; set; }

		/// <summary>Gets or sets the X component of the cell's acceleration.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("xAcceleration")]
		nfloat AccelerationX { get; set; }

		/// <summary>Gets or sets the Y component of the cell's acceleration.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("yAcceleration")]
		nfloat AccelerationY { get; set; }

		/// <summary>Gets or sets the Z component of the cell's acceleration.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("zAcceleration")]
		nfloat AccelerationZ { get; set; }

		/// <summary>Gets or sets the scale factor to apply to the emitter cell.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("scale")]
		nfloat Scale { get; set; }

		/// <summary>Gets or sets the average value by which the scale factor can differ from its nominal value.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("scaleRange")]
		nfloat ScaleRange { get; set; }

		/// <summary>Gets or sets the speed at which the scale factor changes.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("scaleSpeed")]
		nfloat ScaleSpeed { get; set; }

		/// <summary>Gets or sets the rotational speed in radians per second.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("spin")]
		nfloat Spin { get; set; }

		/// <summary>Gets or sets the average value by which the spin can differ from its nominal value.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("spinRange")]
		nfloat SpinRange { get; set; }

		/// <summary>Gets or sets the color or average color of the emitted objects.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("color")]
		[NullAllowed]
		CGColor Color { get; set; }

		/// <summary>Gets or sets the speed at which the red value changes.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("redSpeed")]
		float RedSpeed { get; set; } /* float, not CGFloat */

		/// <summary>Gets or sets the speed at which the green value changes.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("greenSpeed")]
		float GreenSpeed { get; set; } /* float, not CGFloat */

		/// <summary>Gets or sets the speed at which the blue value changes.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("blueSpeed")]
		float BlueSpeed { get; set; } /* float, not CGFloat */

		/// <summary>Gets or sets the speed at which the alpha value changes.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("alphaSpeed")]
		float AlphaSpeed { get; set; } /* float, not CGFloat */

		/// <summary>Gets or sets a weak reference to the image contents for the layer.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("contents", ArgumentSemantic.Strong)]
		NSObject WeakContents { get; set; }

		/// <summary>Gets or sets the image content of the layer.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // just like it's weak property
		[Sealed]
		[Export ("contents", ArgumentSemantic.Strong)]
		CGImage Contents { get; set; }

		/// <summary>Gets or sets the rectangular portion of the image contents to draw.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("contentsRect")]
		CGRect ContentsRect { get; set; }

		/// <summary>TGets or sets the filter to use when shrinking content.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("minificationFilter", ArgumentSemantic.Copy)]
		string MinificationFilter { get; set; }

		/// <summary>Gets or sets the filter to use when enlarging content.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("magnificationFilter", ArgumentSemantic.Copy)]
		string MagnificationFilter { get; set; }

		/// <summary>Gets or sets a numerical value that controls detail presentation for a trilinear minification filter.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("minificationFilterBias")]
		float MinificationFilterBias { get; set; } /* float, not CGFloat */

		/// <summary>Gets or sets an optional array of subcells.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("emitterCells", ArgumentSemantic.Copy)]
		CAEmitterCell [] Cells { get; set; }

		/// <summary>Gets or sets an optional dictionary of style values.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("style", ArgumentSemantic.Copy)]
		NSDictionary Style { get; set; }

		/// <summary>Creates and returns a new <see cref="CoreAnimation.CAEmitterCell" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("emitterCell")]
		CAEmitterCell EmitterCell ();

		/// <param name="key">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Returns the default value for the property that is indexed by the specified <paramref name="key" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("defaultValueForKey:")]
		[return: NullAllowed]
		NSObject DefaultValueForKey (string key);

		/// <param name="key">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Returns a Boolean value that tells if the value for <paramref name="key" /> should be archived.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("shouldArchiveValueForKey:")]
		bool ShouldArchiveValueForKey (string key);

		/// <summary>Gets or sets the average deviation of the blue color component.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("redRange")]
		float RedRange { get; set; } /* float, not CGFloat */

		/// <summary>Gets or sets the average deviation of the green color component.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("greenRange")]
		float GreenRange { get; set; } /* float, not CGFloat */

		/// <summary>Gets or sets the average deviation of the blue color component.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("blueRange")]
		float BlueRange { get; set; } /* float, not CGFloat */

		/// <summary>Gets or sets the average amount of the alpha variation.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("alphaRange")]
		float AlphaRange { get; set; } /* float, not CGFloat */

		/// <summary>Gets or sets the scale at which to draw the image contents.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("contentsScale")]
		nfloat ContentsScale { get; set; }
	}

	/// <summary>A particle-system emitter. Particle types are defined by <see cref="CoreAnimation.CAEmitterCell" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GraphicsImaging/Reference/CAEmitterLayer_class/index.html">Apple documentation for <c>CAEmitterLayer</c></related>
	[BaseType (typeof (CALayer))]
	interface CAEmitterLayer {
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("layer"), New, Static]
		CALayer Create ();

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("emitterCells", ArgumentSemantic.Copy)]
		CAEmitterCell [] Cells { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("birthRate")]
		float BirthRate { get; set; } /* float, not CGFloat */

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("lifetime")]
		float LifeTime { get; set; } /* float, not CGFloat */

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("emitterPosition")]
		CGPoint Position { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("emitterZPosition")]
		nfloat ZPosition { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("emitterSize")]
		CGSize Size { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("emitterDepth")]
		nfloat Depth { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("emitterShape", ArgumentSemantic.Copy)]
		string Shape { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("emitterMode", ArgumentSemantic.Copy)]
		string Mode { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("renderMode", ArgumentSemantic.Copy)]
		string RenderMode { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("preservesDepth")]
		bool PreservesDepth { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("velocity")]
		float Velocity { get; set; } /* float, not CGFloat */

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("scale")]
		float Scale { get; set; } /* float, not CGFloat */

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("spin")]
		float Spin { get; set; } /* float, not CGFloat */

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("seed")]
		int Seed { get; set; } // unsigned int

		/* `emitterShape' values. */
		/// <summary>Represents the value associated with the constant kCAEmitterLayerPoint</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAEmitterLayerPoint")]
		NSString ShapePoint { get; }

		/// <summary>Represents the value associated with the constant kCAEmitterLayerLine</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAEmitterLayerLine")]
		NSString ShapeLine { get; }

		/// <summary>Represents the value associated with the constant kCAEmitterLayerRectangle</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAEmitterLayerRectangle")]
		NSString ShapeRectangle { get; }

		/// <summary>Represents the value associated with the constant kCAEmitterLayerCuboid</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAEmitterLayerCuboid")]
		NSString ShapeCuboid { get; }

		/// <summary>Represents the value associated with the constant kCAEmitterLayerCircle</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAEmitterLayerCircle")]
		NSString ShapeCircle { get; }

		/// <summary>Represents the value associated with the constant kCAEmitterLayerSphere</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAEmitterLayerSphere")]
		NSString ShapeSphere { get; }

		/* `emitterMode' values. */
		/// <summary>Represents the value associated with the constant kCAEmitterLayerPoints</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAEmitterLayerPoints")]
		NSString ModePoints { get; }

		/// <summary>Represents the value associated with the constant kCAEmitterLayerOutline</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAEmitterLayerOutline")]
		NSString ModeOutline { get; }

		/// <summary>Represents the value associated with the constant kCAEmitterLayerSurface</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAEmitterLayerSurface")]
		NSString ModeSurface { get; }

		/// <summary>Represents the value associated with the constant kCAEmitterLayerVolume</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAEmitterLayerVolume")]
		NSString ModeVolume { get; }

		/*  `renderOrder' values. */
		/// <summary>Represents the value associated with the constant kCAEmitterLayerUnordered</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAEmitterLayerUnordered")]
		NSString RenderUnordered { get; }

		/// <summary>Represents the value associated with the constant kCAEmitterLayerOldestFirst</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAEmitterLayerOldestFirst")]
		NSString RenderOldestFirst { get; }

		/// <summary>Represents the value associated with the constant kCAEmitterLayerOldestLast</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAEmitterLayerOldestLast")]
		NSString RenderOldestLast { get; }

		/// <summary>Represents the value associated with the constant kCAEmitterLayerBackToFront</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAEmitterLayerBackToFront")]
		NSString RenderBackToFront { get; }

		/// <summary>Represents the value associated with the constant kCAEmitterLayerAdditive</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kCAEmitterLayerAdditive")]
		NSString RenderAdditive { get; }
	}

	// Corresponding headers were removed in Xcode 9 without any explanation
	// rdar #33590997 was filled - no news
	// 'initWithType:', 'behaviorWithType:' and 'behaviorTypes' API now cause rejection
	// https://trello.com/c/J8BDDUV9/86-33590997-coreanimation-quartzcore-api-removals
#if !NET
	[BaseType (typeof (NSObject))]
	interface CAEmitterBehavior : NSSecureCoding {

		// [Export ("initWithType:")]
		// NativeHandle Constructor (NSString type);

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }

		[NullAllowed] // by default this property is null
		[Export ("name")]
		string Name { get; set; }

		[Export ("type")]
		string Type { get; }

		// [Static][Export ("behaviorWithType:")]
		// CAEmitterBehavior Create (NSString type);

		[Field ("kCAEmitterBehaviorAlignToMotion")]
		NSString AlignToMotion { get; }

		[Field ("kCAEmitterBehaviorAttractor")]
		NSString Attractor { get; }

		[Field ("kCAEmitterBehaviorSimpleAttractor")]
		NSString SimpleAttractor { get; }

		[Field ("kCAEmitterBehaviorColorOverLife")]
		NSString ColorOverLife { get; }

		[Field ("kCAEmitterBehaviorDrag")]
		NSString Drag { get; }

		[Field ("kCAEmitterBehaviorLight")]
		NSString Light { get; }

		[Field ("kCAEmitterBehaviorValueOverLife")]
		NSString ValueOverLife { get; }

		[Field ("kCAEmitterBehaviorWave")]
		NSString Wave { get; }
	}
#endif

	[Internal]
	[Static]
	partial interface CARendererOptionKeys {
		[Field ("kCARendererColorSpace")]
		NSString ColorSpace { get; }

		[Field ("kCARendererMetalCommandQueue")]
		NSString MetalCommandQueue { get; }
	}

	[StrongDictionary ("CARendererOptionKeys")]
	interface CARendererOptions {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ColorSpace")]
		CGColorSpace ColorSpace { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoMacCatalyst]
		[Export ("MetalCommandQueue")]
		IMTLCommandQueue MetalCommandQueue { get; set; }
	}

	[BaseType (typeof (NSObject))]
	interface CARenderer {
		/// <param name="tex">To be added.</param>
		///         <param name="dict">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("rendererWithMTLTexture:options:")]
		CARenderer Create (IMTLTexture tex, [NullAllowed] NSDictionary dict);

		/// <param name="tex">To be added.</param>
		///         <param name="options">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Wrap ("Create (tex, options.GetDictionary ())")]
		CARenderer Create (IMTLTexture tex, [NullAllowed] CARendererOptions options);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("layer", ArgumentSemantic.Strong)]
		CALayer Layer { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("bounds", ArgumentSemantic.Assign)]
		CGRect Bounds { get; set; }

		/// <param name="timeInSeconds">To be added.</param>
		///         <param name="ts">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("beginFrameAtTime:timeStamp:")]
		void BeginFrame (double timeInSeconds, ref CVTimeStamp ts);

		[Sealed]
		[Internal] // since the timestamp is nullable
		[Export ("beginFrameAtTime:timeStamp:")]
		void BeginFrame (double timeInSeconds, IntPtr ts);

		/// <param name="timeInSeconds">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Wrap ("BeginFrame (timeInSeconds, IntPtr.Zero)")]
		void BeginFrame (double timeInSeconds);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("updateBounds")]
		CGRect UpdateBounds ();

		/// <param name="r">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("addUpdateRect:")]
		void AddUpdate (CGRect r);

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("render")]
		void Render ();

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("nextFrameTime")]
		double /* CFTimeInterval */ GetNextFrameTime ();

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("endFrame")]
		void EndFrame ();

		/// <param name="tex">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setDestination:")]
		void SetDestination (IMTLTexture tex);
	}

	[iOS (16, 0)]
	[NoTV]
	[MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject), Name = "CAEDRMetadata")]
	[DisableDefaultCtor]
	interface CAEdrMetadata : NSCopying, NSSecureCoding {

		[Static]
		[Export ("HDR10MetadataWithDisplayInfo:contentInfo:opticalOutputScale:")]
		CAEdrMetadata GetHdr10Metadata ([NullAllowed] NSData displayData, [NullAllowed] NSData contentData, float scale);

		[Static]
		[Export ("HDR10MetadataWithMinLuminance:maxLuminance:opticalOutputScale:")]
		CAEdrMetadata GetHdr10Metadata (float minNits, float maxNits, float scale);

		[Static]
		[Export ("HLGMetadata", ArgumentSemantic.Retain)]
		CAEdrMetadata HlgMetadata { get; }

		[Mac (13, 0)]
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("available")]
		bool Available { [Bind ("isAvailable")] get; }

		[Static]
		[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("HLGMetadataWithAmbientViewingEnvironment:")]
		CAEdrMetadata GetHlgMetadata (NSData ambientViewingEnvironmentData);
	}

	[BaseType (typeof (NSObject))]
	[iOS (17, 0), TV (17, 0), Mac (14, 0), MacCatalyst (17, 0)]
	[DisableDefaultCtor]
	interface CAMetalDisplayLinkUpdate {
		[Export ("drawable")]
		ICAMetalDrawable Drawable { get; }

		[Export ("targetTimestamp")]
		double /* CFTimeInterval */ TargetTimestamp { get; }

		[Export ("targetPresentationTimestamp")]
		double /* CFTimeInterval */ TargetPresentationTimestamp { get; }
	}

	[Protocol (BackwardsCompatibleCodeGeneration = false), Model]
	[BaseType (typeof (NSObject))]
	[iOS (17, 0), TV (17, 0), Mac (14, 0), MacCatalyst (17, 0)]
	interface CAMetalDisplayLinkDelegate {
		[Abstract]
		[Export ("metalDisplayLink:needsUpdate:")]
		void NeedsUpdate (CAMetalDisplayLink link, CAMetalDisplayLinkUpdate update);
	}

	interface ICAMetalDisplayLinkDelegate { }

	[BaseType (typeof (NSObject))]
	[iOS (17, 0), TV (17, 0), Mac (14, 0), MacCatalyst (17, 0)]
	[DisableDefaultCtor]
	interface CAMetalDisplayLink {
		[Export ("initWithMetalLayer:")]
		NativeHandle Constructor (CAMetalLayer layer);

		[Export ("addToRunLoop:forMode:")]
		void AddToRunLoop (NSRunLoop runloop, NSRunLoopMode mode);

		[Export ("removeFromRunLoop:forMode:")]
		void RemoveFromRunLoop (NSRunLoop runloop, NSRunLoopMode mode);

		[Export ("invalidate")]
		void Invalidate ();

		[Export ("delegate", ArgumentSemantic.Weak), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate"), NullAllowed]
		ICAMetalDisplayLinkDelegate Delegate { get; set; }

		[Export ("preferredFrameLatency")]
		float PreferredFrameLatency { get; set; }

		[Export ("preferredFrameRateRange")]
		CAFrameRateRange PreferredFrameRateRange { get; set; }

		[Export ("paused")]
		bool Paused { [Bind ("isPaused")] get; set; }
	}
}
