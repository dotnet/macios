// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN macosx: -sdk macosx -x objective-c++
// RUN iphoneos: -sdk iphoneos -x objective-c++

/*namespace std {
	class string { };
}

@interface NSString
@end

@interface NSString (CxxStringAdditions)
+(NSString *)stringWithCxxStdString:(const std::string&)string;
@end*/

#import <Foundation/Foundation.h>

namespace dp
{
    class String
    {

    };
}

@interface ForceNSStringRMServicesAdditionsToLoad : NSObject
@end

@interface NSString (RMServicesAdditions)

#ifdef __cplusplus
    +(NSString*)stringWithDPString:(const dp::String&)string;
#endif

@end
