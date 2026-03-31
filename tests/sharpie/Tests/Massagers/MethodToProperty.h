// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN enabled: -x objective-c
// RUN disabled: -x objective-c -massage=-MethodToProperty

typedef _Bool bool;

@interface MethodToProperty
// should become a readonly property
-(bool)isEnabled;

// should remain a method
-(void)enable;

// should become a readwrite property
-(bool)visible;
-(void)setVisible:(bool)visible;

// should remain a method
-(bool)getState:(int)foo;
@end
