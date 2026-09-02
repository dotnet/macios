// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN macosx: -sdk macosx

@import CoreGraphics;

typedef CGPoint NSPoint;
typedef CGSize NSSize;
typedef CGRect NSRect;

@interface NSGeometryTest
-(NSPoint)getPoint;
-(NSSize)getSize;
-(NSRect)getRect;
@end
