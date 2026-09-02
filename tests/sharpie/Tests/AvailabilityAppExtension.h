// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN iphoneos: -x objective-c -sdk iphoneos

__attribute__((availability(ios_app_extension,unavailable)))
@interface UnavailableiOSAppExtension
@end

__attribute__((availability(macosx_app_extension,unavailable)))
@interface UnavailableMacOSXAppExtension
@end

__attribute__((availability(tvos_app_extension,unavailable)))
@interface UnavailableTvOSAppExtension
@end

__attribute__((availability(watchos_app_extension,unavailable)))
@interface UnavailableWatchOSAppExtension
@end

__attribute__((availability(maccatalyst_app_extension,unavailable)))
@interface UnavailableMacCatalystAppExtension
@end

__attribute__((availability(visionos_app_extension,unavailable)))
@interface UnavailableVisionOSAppExtension
@end
