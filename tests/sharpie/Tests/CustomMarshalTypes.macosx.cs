using CoreFoundation;
using CoreGraphics;
using CoreMedia;
using CoreMidi;
using CoreVideo;
using Foundation;
using ImageIO;
using Metal;
using MetalPerformanceShaders;
using ObjCRuntime;
using Security;

// typedef NS_RETURNS_RETAINED NS_RETURNS_RETAINED id<MTLTexture> ((^))(MPSKernel * _Nonnull, id<MTLCommandBuffer> _Nonnull, id<MTLTexture> _Nonnull) AHorribleType;
delegate IMTLTexture AHorribleType (MPSKernel arg0, IMTLCommandBuffer arg1, IMTLTexture arg2);

// @interface TestObject : NSObject
[BaseType (typeof (NSObject))]
interface TestObject {
	// @property (assign) CGColorRef color;
	[Export ("color", ArgumentSemantic.Assign)]
	CGColor Color { get; set; }

	// @property (assign) CGPathRef path;
	[Export ("path", ArgumentSemantic.Assign)]
	CGPath Path { get; set; }

	// @property (assign) CGGradientRef gradient;
	[Export ("gradient", ArgumentSemantic.Assign)]
	CGGradient Gradient { get; set; }

	// @property (assign) CGContextRef context;
	[Export ("context", ArgumentSemantic.Assign)]
	CGContext Context { get; set; }

	// @property (assign) CGImageRef image;
	[Export ("image", ArgumentSemantic.Assign)]
	CGImage Image { get; set; }

	// @property (assign) CGImageSourceRef imageSource;
	[Export ("imageSource", ArgumentSemantic.Assign)]
	CGImageSource ImageSource { get; set; }

	// @property (assign) CGColorSpaceRef colorSpace;
	[Export ("colorSpace", ArgumentSemantic.Assign)]
	CGColorSpace ColorSpace { get; set; }

	// @property (assign) CGPDFDocumentRef pdfDocument;
	[Export ("pdfDocument", ArgumentSemantic.Assign)]
	CGPDFDocument PdfDocument { get; set; }

	// @property (assign) CGPDFPageRef pdfPage;
	[Export ("pdfPage", ArgumentSemantic.Assign)]
	CGPDFPage PdfPage { get; set; }

	// @property (assign) dispatch_data_t dispatchDataObject;
	[Export ("dispatchDataObject", ArgumentSemantic.Assign)]
	DispatchData DispatchDataObject { get; set; }

	// @property (assign) sec_identity_t identity2;
	[Export ("identity2", ArgumentSemantic.Assign)]
	SecIdentity2 Identity2 { get; set; }

	// @property (assign) sec_trust_t trust2;
	[Export ("trust2", ArgumentSemantic.Assign)]
	SecTrust2 Trust2 { get; set; }

	// @property (assign) sec_protocol_options_t protocolOptions;
	[Export ("protocolOptions", ArgumentSemantic.Assign)]
	SecProtocolOptions ProtocolOptions { get; set; }

	// @property (assign) sec_protocol_metadata_t protocolMetadata;
	[Export ("protocolMetadata", ArgumentSemantic.Assign)]
	SecProtocolMetadata ProtocolMetadata { get; set; }

	// @property (assign) CFRunLoopRef runLoop;
	[Export ("runLoop", ArgumentSemantic.Assign)]
	CFRunLoop RunLoop { get; set; }

	// @property (assign) MIDIEndpointRef endpoint;
	[Export ("endpoint", ArgumentSemantic.Assign)]
	MidiEndpoint Endpoint { get; set; }

	// @property (assign) CMTimebaseRef timebase;
	[Export ("timebase", ArgumentSemantic.Assign)]
	CMTimebase Timebase { get; set; }

	// @property (assign) CMClockRef clock;
	[Export ("clock", ArgumentSemantic.Assign)]
	CMClock Clock { get; set; }

	// @property (assign) CMSampleBufferRef sampleBuffer;
	[Export ("sampleBuffer", ArgumentSemantic.Assign)]
	CMSampleBuffer SampleBuffer { get; set; }

	// @property (assign) CVImageBufferRef imageBuffer;
	[Export ("imageBuffer", ArgumentSemantic.Assign)]
	CVImageBuffer ImageBuffer { get; set; }

	// @property (assign) CVPixelBufferRef pixelBuffer;
	[Export ("pixelBuffer", ArgumentSemantic.Assign)]
	CVPixelBuffer PixelBuffer { get; set; }

	// @property (assign) CGLayerRef layer;
	[Export ("layer", ArgumentSemantic.Assign)]
	CGLayer Layer { get; set; }

	// @property (assign) CMFormatDescriptionRef formatDescription;
	[Export ("formatDescription", ArgumentSemantic.Assign)]
	CMFormatDescription FormatDescription { get; set; }

	// @property (assign) CMAudioFormatDescriptionRef audioFormatDescription;
	[Export ("audioFormatDescription", ArgumentSemantic.Assign)]
	CMAudioFormatDescription AudioFormatDescription { get; set; }

	// @property (assign) CMVideoFormatDescriptionRef videoFormatDescription;
	[Export ("videoFormatDescription", ArgumentSemantic.Assign)]
	CMVideoFormatDescription VideoFormatDescription { get; set; }

	// @property (assign) SecIdentityRef identity;
	[Export ("identity", ArgumentSemantic.Assign)]
	SecIdentity Identity { get; set; }

	// @property (assign) SecTrustRef trust;
	[Export ("trust", ArgumentSemantic.Assign)]
	SecTrust Trust { get; set; }

	// @property (assign) SecAccessControlRef accessControl;
	[Export ("accessControl", ArgumentSemantic.Assign)]
	SecAccessControl AccessControl { get; set; }
}
