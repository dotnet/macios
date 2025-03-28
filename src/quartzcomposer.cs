//
// Copyright 2010, Kenneth Pouncey
//
// composer.cs: Definitions for binding QuartzComposer.
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
using Foundation;
using AppKit;
using ObjCRuntime;
using CoreGraphics;
using CoreAnimation;
using CoreImage;
using CoreVideo;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace QuartzComposer {

	[Deprecated (PlatformName.MacOSX, 10, 15)]
	[BaseType (typeof (NSObject))]
	interface QCComposition : NSCopying {
		[Static]
		[Export ("compositionWithFile:")]
		QCComposition GetComposition (string path);

		[Static]
		[Export ("compositionWithData:")]
		QCComposition GetComposition (NSData data);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("protocols")]
		string [] Protocols { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("attributes")]
		NSDictionary Attributes { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("inputKeys")]
		string [] InputKeys { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("outputKeys")]
		string [] OutputKeys { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("identifier")]
		string Identifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionAttributeNameKey")]
		NSString AttributeNameKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionAttributeDescriptionKey")]
		NSString AttributeDescriptionKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionAttributeCopyrightKey")]
		NSString AttributeCopyrightKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionAttributeBuiltInKey")]
		NSString AttributeBuiltInKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionAttributeIsTimeDependentKey")]
		NSString AttributeIsTimeDependentKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionAttributeHasConsumersKey")]
		NSString AttributeHasConsumersKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionAttributeCategoryKey")]
		NSString AttributeCategoryKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionCategoryDistortion")]
		NSString CategoryDistortion { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionCategoryStylize")]
		NSString CategoryStylize { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionCategoryUtility")]
		NSString CategoryUtility { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionInputImageKey")]
		NSString InputImageKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionInputSourceImageKey")]
		NSString InputSourceImageKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionInputDestinationImageKey")]
		NSString InputDestinationImageKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' instead.")]
		[Field ("QCCompositionInputRSSFeedURLKey")]
		NSString InputRSSFeedURLKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' instead.")]
		[Field ("QCCompositionInputRSSArticleDurationKey")]
		NSString InputRSSArticleDurationKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionInputPreviewModeKey")]
		NSString InputPreviewModeKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionInputXKey")]
		NSString InputXKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionInputYKey")]
		NSString InputYKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionInputScreenImageKey")]
		NSString InputScreenImageKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionInputAudioPeakKey")]
		NSString InputAudioPeakKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionInputAudioSpectrumKey")]
		NSString InputAudioSpectrumKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionInputTrackPositionKey")]
		NSString InputTrackPositionKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionInputTrackInfoKey")]
		NSString InputTrackInfoKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionInputTrackSignalKey")]
		NSString InputTrackSignalKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionInputPrimaryColorKey")]
		NSString InputPrimaryColorKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionInputSecondaryColorKey")]
		NSString InputSecondaryColorKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionInputPaceKey")]
		NSString InputPaceKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionOutputImageKey")]
		NSString OutputImageKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionOutputWebPageURLKey")]
		NSString OutputWebPageURLKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionProtocolGraphicAnimation")]
		NSString ProtocolGraphicAnimation { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionProtocolGraphicTransition")]
		NSString ProtocolGraphicTransition { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionProtocolImageFilter")]
		NSString ProtocolImageFilter { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionProtocolScreenSaver")]
		NSString ProtocolScreenSaver { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' instead.")]
		[Field ("QCCompositionProtocolRSSVisualizer")]
		NSString ProtocolRSSVisualizer { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("QCCompositionProtocolMusicVisualizer")]
		NSString ProtocolMusicVisualizer { get; }
	}


	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' instead.")]
	[BaseType (typeof (CAOpenGLLayer))]
	[DisableDefaultCtor] // return invalid handle
	interface QCCompositionLayer {

		[Static]
		[Export ("compositionLayerWithFile:")]
		QCCompositionLayer Create (string path);

		[Static]
		[Export ("compositionLayerWithComposition:")]
		QCCompositionLayer Create (QCComposition composition);

		[Export ("initWithFile:")]
		NativeHandle Constructor (string path);

		[Export ("initWithComposition:")]
		NativeHandle Constructor (QCComposition composition);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("composition")]
		QCComposition Composition { get; }

	}

	[Deprecated (PlatformName.MacOSX, 10, 15)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // crash when used (e.g. description) meant to be used thru sharedCompositionRepository
	interface QCCompositionRepository {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("sharedCompositionRepository")]
		QCCompositionRepository SharedCompositionRepository { get; }

		[Export ("compositionWithIdentifier:")]
		QCComposition GetComposition (string identifier);

		[Export ("compositionsWithProtocols:andAttributes:")]
		QCComposition [] GetCompositions (NSArray protocols, NSDictionary attributes);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("allCompositions")]
		QCComposition [] AllCompositions { get; }

	}

}
