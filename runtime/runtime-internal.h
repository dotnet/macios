/* -*- Mode: C; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * runtime-internal.h: This header is not shipped with Xamarin.iOS/Mac.
 *
 *  Authors: Rolf Bjarne Kvinge
 *
 *  Copyright (C) 2014 Xamarin Inc. (www.xamarin.com)
 *
 */

#ifndef __RUNTIME_INTERNAL_H__
#define __RUNTIME_INTERNAL_H__

#include "xamarin/xamarin.h"

#define LOG(...) do { if (xamarin_log_level > 0) NSLog (@ __VA_ARGS__); } while (0);

// #define DEBUG_LAUNCH_TIME

#ifdef DEBUG_LAUNCH_TIME
#define DEBUG_LAUNCH_TIME_PRINT(msg) \
	debug_launch_time_print (msg);
#else
#define DEBUG_LAUNCH_TIME_PRINT(...)
#endif

void *xamarin_marshal_return_value (MonoType *mtype, const char *type, MonoObject *retval, bool retain, MonoMethod *method);
MonoAssembly * xamarin_open_assembly (const char *name);

#endif /* __RUNTIME_INTERNAL_H__ */
