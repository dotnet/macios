// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN iphoneos: -sdk iphoneos -x objective-c
// RUN macosx: -sdk macosx -x objective-c

#import <Foundation/Foundation.h>
#import <CoreFoundation/CoreFoundation.h>
#import <CoreGraphics/CoreGraphics.h>
#import <ImageIO/ImageIO.h>
#import <CoreMIDI/CoreMIDI.h>
#import <CoreMedia/CoreMedia.h>
#import <CoreVideo/CoreVideo.h>
#import <Security/Security.h>
#import <MetalPerformanceShaders/MetalPerformanceShaders.h>
#import <Metal/Metal.h>

typedef id <MTLTexture> __nonnull NS_RETURNS_RETAINED (^AHorribleType)( MPSKernel * __nonnull filter, id <MTLCommandBuffer> __nonnull commandBuffer, id <MTLTexture> __nonnull sourceTexture);

@interface TestObject : NSObject

@property(assign) CGColorRef color;
@property(assign) CGPathRef path;
@property(assign) CGGradientRef gradient;
@property(assign) CGContextRef context;
@property(assign) CGImageRef image;
@property(assign) CGImageSourceRef imageSource;
@property(assign) CGColorSpaceRef colorSpace;
@property(assign) CGPDFDocumentRef pdfDocument;
@property(assign) CGPDFPageRef pdfPage;

@property(assign) dispatch_data_t dispatchDataObject;
@property(assign) sec_identity_t identity2;
@property(assign) sec_trust_t trust2;
@property(assign) sec_protocol_options_t protocolOptions;
@property(assign) sec_protocol_metadata_t protocolMetadata;

@property(assign) CFRunLoopRef runLoop;
@property(assign) MIDIEndpointRef endpoint;
@property(assign) CMTimebaseRef timebase;
@property(assign) CMClockRef clock;
@property(assign) CMSampleBufferRef sampleBuffer;
@property(assign) CVImageBufferRef imageBuffer;
@property(assign) CVPixelBufferRef pixelBuffer;
@property(assign) CGLayerRef layer;
@property(assign) CMFormatDescriptionRef formatDescription;
@property(assign) CMAudioFormatDescriptionRef audioFormatDescription;
@property(assign) CMVideoFormatDescriptionRef videoFormatDescription;
@property(assign) SecIdentityRef identity;
@property(assign) SecTrustRef trust;
@property(assign) SecAccessControlRef accessControl;

@end
