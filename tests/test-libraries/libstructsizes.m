// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#include <stdint.h>
#include <string.h>
#include <simd/simd.h>
#import <Foundation/Foundation.h>
#import <CoreGraphics/CoreGraphics.h>
#import <QuartzCore/QuartzCore.h>
#import <CoreMedia/CoreMedia.h>
#import <CoreVideo/CoreVideo.h>
#import <CoreLocation/CoreLocation.h>
#if !TARGET_OS_TV
#import <CoreMotion/CoreMotion.h>
#endif
#import <MapKit/MapKit.h>
#import <Metal/Metal.h>
#import <MetalPerformanceShaders/MetalPerformanceShaders.h>
#import <SceneKit/SceneKit.h>
#import <GameController/GameController.h>
#import <AudioToolbox/AudioToolbox.h>

#if TARGET_OS_OSX
#import <AppKit/AppKit.h>
#else
#import <UIKit/UIKit.h>
#endif

#define CHECK_SIZE(type) \
	if (strcmp (native_name, #type) == 0) \
		return (int32_t) sizeof (type);

#ifdef __cplusplus
extern "C"
#endif
int32_t
x_get_struct_size (const char *native_name)
{
	if (native_name == NULL)
		return -1;

	// sizeof does not require the type's minimum OS version at runtime.
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wunguarded-availability-new"
#include "libstructsizes.generated.h"
#pragma clang diagnostic pop

	return -1;
}
