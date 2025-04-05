using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using Metal;
using ModelIO;
using ObjCRuntime;

#if MONOMAC
using AppKit;
#endif

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace MetalKit {

#if !MONOMAC
	// stubs to ease compilation using [NoiOS], [NoTV]
	interface NSDisplayGamut { }
#endif

	/// <summary>Static class that contains MetalKit constants related to MetalKit Model errors.</summary>
	[MacCatalyst (13, 1)]
	[Static]
	interface MTKModel {

		/// <summary>Gets the error domain where mesh initialization errors are reported.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MTKModelErrorDomain")]
		NSString ErrorDomain { get; }

		/// <summary>Gets the key that is used to retrieve error information from the <see cref="P:Foundation.NSError.UserInfo" /> property of a MetalKit initialization error.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MTKModelErrorKey")]
		NSString ErrorKey { get; }
	}

	/// <summary>A <see cref="T:UIKit.UIView" /> that is Metal-aware.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MetalKit/Reference/MTKView_ClassReference/index.html">Apple documentation for <c>MTKView</c></related>
	[MacCatalyst (13, 1)]
#if MONOMAC
	[BaseType (typeof (AppKit.NSView))]
#else
	[BaseType (typeof (UIKit.UIView))]
#endif
	interface MTKView : NSCoding, CALayerDelegate {
		[Export ("initWithFrame:device:")]
		[DesignatedInitializer]
		NativeHandle Constructor (CGRect frameRect, [NullAllowed] IMTLDevice device);

		/// <summary>An instance of the MetalKit.IMTKViewDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the MetalKit.IMTKViewDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IMTKViewDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[NullAllowed, Export ("device", ArgumentSemantic.Retain)]
		IMTLDevice Device { get; set; }

		[NullAllowed, Export ("currentDrawable")]
		ICAMetalDrawable CurrentDrawable { get; }

		[Export ("framebufferOnly")]
		bool FramebufferOnly { get; set; }

		[Export ("presentsWithTransaction")]
		bool PresentsWithTransaction { get; set; }

		[Export ("colorPixelFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat ColorPixelFormat { get; set; }

		[Export ("depthStencilPixelFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat DepthStencilPixelFormat { get; set; }

		[Export ("sampleCount", ArgumentSemantic.Assign)]
		nuint SampleCount { get; set; }

		[Export ("clearColor", ArgumentSemantic.Assign)]
		MTLClearColor ClearColor { get; set; }

		[Export ("clearDepth")]
		double ClearDepth { get; set; }

		[Export ("clearStencil")]
		uint ClearStencil { get; set; }

		[NullAllowed, Export ("depthStencilTexture")]
		IMTLTexture DepthStencilTexture { get; }

		[NullAllowed, Export ("multisampleColorTexture")]
		IMTLTexture MultisampleColorTexture { get; }

		[Export ("releaseDrawables")]
		void ReleaseDrawables ();

		[NullAllowed, Export ("currentRenderPassDescriptor")]
		MTLRenderPassDescriptor CurrentRenderPassDescriptor { get; }

		[Export ("preferredFramesPerSecond", ArgumentSemantic.Assign)]
		nint PreferredFramesPerSecond { get; set; }

		[Export ("enableSetNeedsDisplay")]
		bool EnableSetNeedsDisplay { get; set; }

		[Export ("autoResizeDrawable")]
		bool AutoResizeDrawable { get; set; }

		[Export ("drawableSize", ArgumentSemantic.Assign)]
		CGSize DrawableSize { get; set; }

		/// <summary>Gets or sets a Boolean value that controls whether the draw loop is paused.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("paused")]
		bool Paused { [Bind ("isPaused")] get; set; }

		[Export ("draw")]
		void Draw ();

		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		[NullAllowed, Export ("colorspace", ArgumentSemantic.Assign)]
		CGColorSpace ColorSpace { get; set; }

		[iOS (13, 0)]
		[TV (13, 0)]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("preferredDevice")]
		IMTLDevice PreferredDevice { get; }

		[iOS (13, 0)]
		[TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("preferredDrawableSize")]
		CGSize PreferredDrawableSize { get; }

		[iOS (13, 0)]
		[TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("depthStencilAttachmentTextureUsage", ArgumentSemantic.Assign)]
		MTLTextureUsage DepthStencilAttachmentTextureUsage { get; set; }

		[iOS (13, 0)]
		[TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("multisampleColorAttachmentTextureUsage", ArgumentSemantic.Assign)]
		MTLTextureUsage MultisampleColorAttachmentTextureUsage { get; set; }

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Export ("depthStencilStorageMode", ArgumentSemantic.Assign)]
		MTLStorageMode DepthStencilStorageMode { get; set; }
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="T:MetalKit.MTKViewDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="T:MetalKit.MTKViewDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="T:MetalKit.MTKViewDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="T:MetalKit.MTKViewDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IMTKViewDelegate { }

	/// <summary>Delegate object for <see cref="T:MetalKit.MTKView" /> objects. Provides methods called when the view is drawn and laid out.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MetalKit/Reference/MTKViewDelegate_ProtocolReference/index.html">Apple documentation for <c>MTKViewDelegate</c></related>
	[MacCatalyst (13, 1)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface MTKViewDelegate {
		[Abstract]
		[Export ("mtkView:drawableSizeWillChange:")]
		void DrawableSizeWillChange (MTKView view, CGSize size);

		[Abstract]
		[Export ("drawInMTKView:")]
		void Draw (MTKView view);
	}

	/// <summary>Key and domain strings for working with texture loading errors.</summary>
	[MacCatalyst (13, 1)]
	[Static]
	interface MTKTextureLoaderError {

		/// <summary>Gets the error domain where texture initialization errors are reported.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MTKTextureLoaderErrorDomain")]
		NSString Domain { get; }

		/// <summary>Gets the key that is used to retrieve error information from the <see cref="P:Foundation.NSError.UserInfo" /> property of a texture initialization error.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MTKTextureLoaderErrorKey")]
		NSString Key { get; }
	}

	[MacCatalyst (13, 1)]
	[Static, Internal]
	interface MTKTextureLoaderKeys {

		[Field ("MTKTextureLoaderOptionAllocateMipmaps")]
		NSString AllocateMipmapsKey { get; }

		[MacCatalyst (13, 1)]
		[Field ("MTKTextureLoaderOptionGenerateMipmaps")]
		NSString GenerateMipmapsKey { get; }

		[Field ("MTKTextureLoaderOptionSRGB")]
		NSString SrgbKey { get; }

		[Field ("MTKTextureLoaderOptionTextureUsage")]
		NSString TextureUsageKey { get; }

		[Field ("MTKTextureLoaderOptionTextureCPUCacheMode")]
		NSString TextureCpuCacheModeKey { get; }

		[MacCatalyst (13, 1)]
		[Field ("MTKTextureLoaderOptionTextureStorageMode")]
		NSString TextureStorageModeKey { get; }

		[MacCatalyst (13, 1)]
		[Field ("MTKTextureLoaderOptionCubeLayout")]
		NSString CubeLayoutKey { get; }

		[MacCatalyst (13, 1)]
		[Field ("MTKTextureLoaderOptionOrigin")]
		NSString OriginKey { get; }

		[iOS (17, 0), Mac (14, 0), TV (17, 0), MacCatalyst (17, 0)]
		[Field ("MTKTextureLoaderOptionLoadAsArray")]
		NSString LoadAsArrayKey { get; }

	}

	[MacCatalyst (13, 1)]
	enum MTKTextureLoaderCubeLayout {
		/// <summary>To be added.</summary>
		[Field ("MTKTextureLoaderCubeLayoutVertical")]
		Vertical,
	}

	[MacCatalyst (13, 1)]
	enum MTKTextureLoaderOrigin {
		/// <summary>To be added.</summary>
		[Field ("MTKTextureLoaderOriginTopLeft")]
		TopLeft,
		/// <summary>To be added.</summary>
		[Field ("MTKTextureLoaderOriginBottomLeft")]
		BottomLeft,
		/// <summary>To be added.</summary>
		[Field ("MTKTextureLoaderOriginFlippedVertically")]
		FlippedVertically,
	}

	/// <summary>Contains a dictionary of texture loading options and exposes them as discoverably named properties.</summary>
	[StrongDictionary ("MTKTextureLoaderKeys")]
	interface MTKTextureLoaderOptions {
		/// <summary>Gets a Boolean value that tells whether to allocate mipmap memory when loading the texture.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		bool AllocateMipmaps { get; set; }

		/// <summary>Gets a value that tells whether mipmaps will be created for the new texture.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		bool GenerateMipmaps { get; set; }

		/// <summary>Gets a Boolean value that tells whether the texture should be loaded from the sRGB color space.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		bool Srgb { get; set; }

		[iOS (17, 0), Mac (14, 0), TV (17, 0), MacCatalyst (17, 0)]
		bool LoadAsArray { get; set; }
	}

	/// <summary>Completion handler for textures loaded with <see cref="T:MetalKit.MTKTextureLoader" />.</summary>
	[MacCatalyst (13, 1)]
	delegate void MTKTextureLoaderCallback ([NullAllowed] IMTLTexture texture, [NullAllowed] NSError error);

	[MacCatalyst (13, 1)]
	delegate void MTKTextureLoaderArrayCallback (IMTLTexture [] textures, [NullAllowed] NSError error);

	/// <summary>Creates <see cref="T:Metal.IMTLTexture" /> objects from common image formats.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MetalKit/Reference/MTKTextureLoader_ClassReference/index.html">Apple documentation for <c>MTKTextureLoader</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MTKTextureLoader {
		[Export ("device")]
		IMTLDevice Device { get; }

		[Export ("initWithDevice:")]
		NativeHandle Constructor (IMTLDevice device);

		[Export ("newTextureWithContentsOfURL:options:completionHandler:"), Internal]
		void FromUrl (NSUrl url, [NullAllowed] NSDictionary options, MTKTextureLoaderCallback completionHandler);

		/// <param name="url">The location of the image data to load.</param>
		///         <param name="options">Options for loading the texture data.</param>
		///         <param name="completionHandler">A handler to run after the texture is loaded.</param>
		///         <summary>Creates a new Metal texture from the resource at the specified <paramref name="url" />.</summary>
		///         <remarks>To be added.</remarks>
		[Wrap ("FromUrl (url, options.GetDictionary (), completionHandler)")]
		[Async]
		void FromUrl (NSUrl url, [NullAllowed] MTKTextureLoaderOptions options, MTKTextureLoaderCallback completionHandler);

		[Export ("newTextureWithData:options:completionHandler:"), Internal]
		void FromData (NSData data, [NullAllowed] NSDictionary options, MTKTextureLoaderCallback completionHandler);

		/// <param name="data">The texture data.</param>
		///         <param name="options">Options for loading the texture data.</param>
		///         <param name="completionHandler">A handler to run after the texture is loaded.</param>
		///         <summary>Creates and returns a Metal texture from the specified image data and options, and runs a completion handler when it completes.</summary>
		///         <remarks>To be added.</remarks>
		[Wrap ("FromData (data, options.GetDictionary (), completionHandler)")]
		[Async]
		void FromData (NSData data, [NullAllowed] MTKTextureLoaderOptions options, MTKTextureLoaderCallback completionHandler);

		[Export ("newTextureWithCGImage:options:completionHandler:"), Internal]
		void FromCGImage (CGImage cgImage, [NullAllowed] NSDictionary options, MTKTextureLoaderCallback completionHandler);

		/// <param name="cgImage">A Core Graphics image.</param>
		///         <param name="options">Options for loading the texture data.</param>
		///         <param name="completionHandler">A handler to run after the texture is loaded.</param>
		///         <summary>Creates and returns a Metal texture from the specified Core Graphics image and options, and runs a completion handler when it completes.</summary>
		///         <remarks>To be added.</remarks>
		[Wrap ("FromCGImage (cgImage, options.GetDictionary (), completionHandler)")]
		[Async]
		void FromCGImage (CGImage cgImage, [NullAllowed] MTKTextureLoaderOptions options, MTKTextureLoaderCallback completionHandler);

		[Export ("newTextureWithContentsOfURL:options:error:"), Internal]
		[return: NullAllowed]
		IMTLTexture FromUrl (NSUrl url, [NullAllowed] NSDictionary options, out NSError error);

		/// <param name="url">The location of the image data to load.</param>
		///         <param name="options">Options for loading the texture data.</param>
		///         <param name="error">Contains the error, if one occurred.</param>
		///         <summary>Creates a new Metal texture from the resource at the specified <paramref name="url" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Wrap ("FromUrl (url, options.GetDictionary (), out error)")]
		[return: NullAllowed]
		IMTLTexture FromUrl (NSUrl url, [NullAllowed] MTKTextureLoaderOptions options, out NSError error);

		[MacCatalyst (13, 1)]
		[Export ("newTexturesWithContentsOfURLs:options:completionHandler:")]
		[Async]
		void FromUrls (NSUrl [] urls, [NullAllowed] NSDictionary options, MTKTextureLoaderArrayCallback completionHandler);

		/// <param name="urls">The locations of the image data to load.</param>
		///         <param name="options">Options for loading the texture data.</param>
		///         <param name="completionHandler">A handler to run after the texture is loaded.</param>
		///         <summary>Creates an array of new Metal textures from the resources at the specified <paramref name="urls" />.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Wrap ("FromUrls (urls, options.GetDictionary (), completionHandler)")]
		[Async]
		void FromUrls (NSUrl [] urls, [NullAllowed] MTKTextureLoaderOptions options, MTKTextureLoaderArrayCallback completionHandler);

		[MacCatalyst (13, 1)]
		[Export ("newTexturesWithContentsOfURLs:options:error:")]
		IMTLTexture [] FromUrls (NSUrl [] urls, [NullAllowed] NSDictionary options, out NSError error);

		/// <param name="urls">The locations of the image data to load.</param>
		///         <param name="options">Options for loading the texture data.</param>
		///         <param name="error">Contains the error, if one occurred.</param>
		///         <summary>Creates an array of new Metal textures from the resources at the specified <paramref name="urls" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Wrap ("FromUrls (urls, options.GetDictionary (), out error)")]
		IMTLTexture [] FromUrls (NSUrl [] urls, [NullAllowed] MTKTextureLoaderOptions options, out NSError error);

		[Export ("newTextureWithData:options:error:"), Internal]
		[return: NullAllowed]
		IMTLTexture FromData (NSData data, [NullAllowed] NSDictionary options, out NSError error);

		/// <param name="data">The texture data.</param>
		///         <param name="options">Options for loading the texture data.</param>
		///         <param name="error">Contains the error, if one occurred.</param>
		///         <summary>Creates and returns a Metal texture from the specified image data and options.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Wrap ("FromData (data, options.GetDictionary (), out error)")]
		[return: NullAllowed]
		IMTLTexture FromData (NSData data, [NullAllowed] MTKTextureLoaderOptions options, out NSError error);

		[Export ("newTextureWithCGImage:options:error:"), Internal]
		[return: NullAllowed]
		IMTLTexture FromCGImage (CGImage cgImage, [NullAllowed] NSDictionary options, out NSError error);

		/// <param name="cgImage">A Core Graphics image.</param>
		///         <param name="options">Options for loading the texture data.</param>
		///         <param name="error">Contains the error, if one occurred.</param>
		///         <summary>Creates and returns a Metal texture from the specified Core Graphics image and options.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Wrap ("FromCGImage (cgImage, options.GetDictionary (), out error)")]
		[return: NullAllowed]
		IMTLTexture FromCGImage (CGImage cgImage, [NullAllowed] MTKTextureLoaderOptions options, out NSError error);

		[MacCatalyst (13, 1)]
		[Export ("newTextureWithName:scaleFactor:bundle:options:completionHandler:")]
		[Async]
		void FromName (string name, nfloat scaleFactor, [NullAllowed] NSBundle bundle, [NullAllowed] NSDictionary options, MTKTextureLoaderCallback completionHandler);

		[MacCatalyst (13, 1)]
		[Wrap ("FromName (name, scaleFactor, bundle, options.GetDictionary (), completionHandler)")]
		[Async]
		void FromName (string name, nfloat scaleFactor, [NullAllowed] NSBundle bundle, [NullAllowed] MTKTextureLoaderOptions options, MTKTextureLoaderCallback completionHandler);

		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		[Export ("newTextureWithName:scaleFactor:displayGamut:bundle:options:completionHandler:")]
		[Async]
		void FromName (string name, nfloat scaleFactor, NSDisplayGamut displayGamut, [NullAllowed] NSBundle bundle, [NullAllowed] NSDictionary options, MTKTextureLoaderCallback completionHandler);

		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		[Wrap ("FromName (name, scaleFactor, displayGamut, bundle, options.GetDictionary (), completionHandler)")]
		[Async]
		void FromName (string name, nfloat scaleFactor, NSDisplayGamut displayGamut, [NullAllowed] NSBundle bundle, [NullAllowed] MTKTextureLoaderOptions options, MTKTextureLoaderCallback completionHandler);

		[MacCatalyst (13, 1)]
		[Export ("newTexturesWithNames:scaleFactor:bundle:options:completionHandler:")]
		[Async]
		void FromNames (string [] names, nfloat scaleFactor, [NullAllowed] NSBundle bundle, [NullAllowed] NSDictionary options, MTKTextureLoaderArrayCallback completionHandler);

		[MacCatalyst (13, 1)]
		[Wrap ("FromNames (names, scaleFactor, bundle, options.GetDictionary (), completionHandler)")]
		[Async]
		void FromNames (string [] names, nfloat scaleFactor, [NullAllowed] NSBundle bundle, [NullAllowed] MTKTextureLoaderOptions options, MTKTextureLoaderArrayCallback completionHandler);

		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		[Async]
		[Export ("newTexturesWithNames:scaleFactor:displayGamut:bundle:options:completionHandler:")]
		void FromNames (string [] names, nfloat scaleFactor, NSDisplayGamut displayGamut, [NullAllowed] NSBundle bundle, [NullAllowed] NSDictionary options, MTKTextureLoaderArrayCallback completionHandler);

		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		[Wrap ("FromNames (names, scaleFactor, displayGamut, bundle, options.GetDictionary (), completionHandler)")]
		[Async]
		void FromNames (string [] names, nfloat scaleFactor, NSDisplayGamut displayGamut, [NullAllowed] NSBundle bundle, [NullAllowed] MTKTextureLoaderOptions options, MTKTextureLoaderArrayCallback completionHandler);

		[MacCatalyst (13, 1)]
		[Export ("newTextureWithMDLTexture:options:completionHandler:")]
		[Async]
		void FromTexture (MDLTexture texture, [NullAllowed] NSDictionary options, MTKTextureLoaderCallback completionHandler);

		/// <param name="texture">The texture to load.</param>
		///         <param name="options">Options for loading the texture data.</param>
		///         <param name="completionHandler">A handler to run after the texture is loaded.</param>
		///         <summary>Creates a new Metal texture from the specified <paramref name="texture" />.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Wrap ("FromTexture (texture, options.GetDictionary (), completionHandler)")]
		[Async]
		void FromTexture (MDLTexture texture, [NullAllowed] MTKTextureLoaderOptions options, MTKTextureLoaderCallback completionHandler);

		[MacCatalyst (13, 1)]
		[Export ("newTextureWithMDLTexture:options:error:")]
		[return: NullAllowed]
		IMTLTexture FromTexture (MDLTexture texture, [NullAllowed] NSDictionary options, out NSError error);

		/// <param name="texture">The texture to load.</param>
		///         <param name="options">Options for loading the texture data.</param>
		///         <param name="error">Contains the error, if one occurred.</param>
		///         <summary>Creates a new Metal texture from the specified <paramref name="texture" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Wrap ("FromTexture (texture, options.GetDictionary (), out error)")]
		[return: NullAllowed]
		IMTLTexture FromTexture (MDLTexture texture, [NullAllowed] MTKTextureLoaderOptions options, out NSError error);

		[MacCatalyst (13, 1)]
		[Export ("newTextureWithName:scaleFactor:bundle:options:error:")]
		[return: NullAllowed]
		IMTLTexture FromName (string name, nfloat scaleFactor, [NullAllowed] NSBundle bundle, [NullAllowed] NSDictionary options, out NSError error);

		[MacCatalyst (13, 1)]
		[Wrap ("FromName (name, scaleFactor, bundle, options.GetDictionary (), out error)")]
		[return: NullAllowed]
		IMTLTexture FromName (string name, nfloat scaleFactor, [NullAllowed] NSBundle bundle, [NullAllowed] MTKTextureLoaderOptions options, out NSError error);

		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		[Export ("newTextureWithName:scaleFactor:displayGamut:bundle:options:error:")]
		[return: NullAllowed]
		IMTLTexture FromName (string name, nfloat scaleFactor, NSDisplayGamut displayGamut, [NullAllowed] NSBundle bundle, [NullAllowed] NSDictionary options, [NullAllowed] out NSError error);

		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		[Wrap ("FromName (name, scaleFactor, displayGamut, bundle, options.GetDictionary (), out error)")]
		[return: NullAllowed]
		IMTLTexture FromName (string name, nfloat scaleFactor, NSDisplayGamut displayGamut, [NullAllowed] NSBundle bundle, [NullAllowed] MTKTextureLoaderOptions options, [NullAllowed] out NSError error);
	}

	/// <summary>An <see cref="T:ModelIO.IMDLMeshBufferAllocator" /> that allocates a <see cref="T:MetalKit.MTKMeshBuffer" /> suitable for use with <see cref="T:ModelIO.MDLAsset" /> objects.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MetalKit/Reference/MTKMeshBufferAllocator_ClassReference/index.html">Apple documentation for <c>MTKMeshBufferAllocator</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // init is NS_UNAVAILABLE
	interface MTKMeshBufferAllocator : MDLMeshBufferAllocator {
		[Export ("initWithDevice:")]
		NativeHandle Constructor (IMTLDevice device);

		[Export ("device")]
		IMTLDevice Device { get; }
	}

	/// <summary>A Metal buffer for holding vertex and index data for ModelIO meshes and submeshes.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MetalKit/Reference/MTKMeshBuffer_ClassReference/index.html">Apple documentation for <c>MTKMeshBuffer</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MTKMeshBuffer : MDLMeshBuffer, MDLNamed {
		[Export ("buffer")]
		IMTLBuffer Buffer { get; }

		[Export ("offset")]
		nuint Offset { get; }
	}

	/// <summary>A segment of mesh data that can be rendered in a single draw call. Submesh vertex data is held in the parent <see cref="T:MetalKit.MTKMesh" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MetalKit/Reference/MTKSubmesh_ClassReference/index.html">Apple documentation for <c>MTKSubmesh</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MTKSubmesh {
		[Export ("primitiveType")]
		MTLPrimitiveType PrimitiveType { get; }

		[Export ("indexType")]
		MTLIndexType IndexType { get; }

		[Export ("indexBuffer")]
		MTKMeshBuffer IndexBuffer { get; }

		[Export ("indexCount")]
		nuint IndexCount { get; }

		[NullAllowed, Export ("mesh", ArgumentSemantic.Weak)]
		MTKMesh Mesh { get; }

		[Export ("name")]
		string Name { get; set; }
	}

	/// <summary>Holds vertex data for rendering with Metal.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MetalKit/Reference/MTKMesh_ClassReference/index.html">Apple documentation for <c>MTKMesh</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // init NS_UNAVAILABLE
	interface MTKMesh {
		[Export ("initWithMesh:device:error:")]
		NativeHandle Constructor (MDLMesh mesh, IMTLDevice device, out NSError error);

		// generator does not like `out []` -> https://trello.com/c/sZYNalbB/524-generator-support-out
		[Internal] // there's another, manual, public API exposed
		[Static]
		[Export ("newMeshesFromAsset:device:sourceMeshes:error:")]
		[return: NullAllowed]
		MTKMesh [] FromAsset (MDLAsset asset, IMTLDevice device, out NSArray sourceMeshes, out NSError error);

		[Export ("vertexBuffers")]
		MTKMeshBuffer [] VertexBuffers { get; }

		[Export ("vertexDescriptor")]
		MDLVertexDescriptor VertexDescriptor { get; }

		[Export ("submeshes")]
		MTKSubmesh [] Submeshes { get; }

		[Export ("vertexCount")]
		nuint VertexCount { get; }

		[Export ("name")]
		string Name { get; set; }
	}
}
