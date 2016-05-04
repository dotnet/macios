﻿//
// Test fixture for class_ptr introspection tests
//
// Authors:
//	Alex Soto  <alex.soto@xamarin.com>
//
// Copyright 2012-2014 Xamarin Inc.
//
using System;
using System.Reflection;
#if XAMCORE_2_0
using Foundation;
using ObjCRuntime;
#else
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
#endif
using NUnit.Framework;

namespace Introspection {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class iOSApiClassPtrTest : ApiClassPtrTest {

		protected override bool Skip (Type type)
		{
			// While the following types are categories and contains a class_ptr
			// they are not used at all as extensions since they are just used to expose 
			// static properties.
			switch (type.Name) {
			case "NSUrlUtilities_NSCharacterSet":
			case "AVAssetTrackTrackAssociation":
				return true;
			}
			return base.Skip (type);
		}
	}
}

