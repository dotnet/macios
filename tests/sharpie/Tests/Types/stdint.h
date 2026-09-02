// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN macosx: -sdk macosx
// RUN iphoneos: -sdk iphoneos

#include <stdint.h>

int8_t _int8_t ();
uint8_t _uint8_t ();
int16_t _int16_t ();
uint16_t _uint16_t ();
int32_t _int32_t ();
uint32_t _uint32_t ();
int64_t _int64_t ();
uint64_t _uint64_t ();

intptr_t _intptr_t ();
uintptr_t _uintptr_t ();
