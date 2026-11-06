// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN macosx: -sdk macosx -x objective-c
// RUN iphoneos: -sdk iphoneos -x objective-c

#import <Foundation/Foundation.h>

void *_void_ptr ();
int *_int_ptr ();
int **_int_ptr_ptr ();
int ***_int_ptr_ptr_ptr ();

@interface ObjCObjectOutPointers
-(void)getError:(NSError **)error;
@end

typedef struct {
	double latitude;
	double longitude;
} CLLocationCoordinate2D;

@interface MAPolygon : NSObject
+(instancetype)polygonWithCoordinates:(CLLocationCoordinate2D *)coords count:(NSUInteger)count;
-(void)getCoordinates:(CLLocationCoordinate2D *)coords count:(NSUInteger)count;
-(void)setCoordinates:(const CLLocationCoordinate2D *)coords count:(NSUInteger)count;
-(CLLocationCoordinate2D *)coordinates;
-(void)getCoordinatesPointer:(CLLocationCoordinate2D **)coords;
@end
