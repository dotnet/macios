﻿using System;
using NUnit.Framework;

#if !XAMCORE_2_0
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;
#else
using AppKit;
using ObjCRuntime;
using Foundation;
#endif

namespace Xamarin.Mac.Tests
{
	public class NSUserDefaultsControllerTests
	{
		NSUserDefaultsController controller;

		[Test]
		public void NSUserDefaultsControllerShouldGetSharedController ()
		{
			controller = NSUserDefaultsController.SharedUserDefaultsController;

			Assert.IsNotNull (controller, "NSUserDefaultsControllerShouldGetDefaultController - SharedUserDefaultsController returned null");
		}

		[Test]
		public void NSUserDefaultsControllerShouldCreateNewControllerWithDefaultConstructor ()
		{
			controller = new NSUserDefaultsController ();

			Assert.IsNotNull (controller, "NSUserDefaultsControllerShouldCreateNewControllerWithDefaultConstructor - Constructor returned null");
		}

		[Test]
		public void NSUserDefaultsControllerShouldCreateNewControllerWithNullParameters ()
		{
			controller = new NSUserDefaultsController (null, null);

			Assert.IsTrue (controller.Defaults == NSUserDefaults.StandardUserDefaults);
			Assert.IsTrue (controller.InitialValues == null);
			Assert.IsNotNull (controller, "NSUserDefaultsControllerShouldCreateNewControllerWithNullParameters - Constructor returned null");
		}

		[Test]
		public void NSUserDefaultsControllerShouldCreateNewControllerWithParameters ()
		{
			var initialValues = new NSDictionary ();
			controller = new NSUserDefaultsController (NSUserDefaults.StandardUserDefaults, initialValues);

			Assert.IsTrue (controller.Defaults == NSUserDefaults.StandardUserDefaults);
			Assert.IsTrue (controller.InitialValues == initialValues);
			Assert.IsNotNull (controller, "NSUserDefaultsControllerShouldCreateNewControllerWithParameters - Constructor returned null");
		}

		[Test]
		public void NSUserDefaultsControllerShouldChangeInitialValues ()
		{
			controller = new NSUserDefaultsController (NSUserDefaults.StandardUserDefaults, null);
			var initialValues = controller.InitialValues;
			controller.InitialValues = new NSDictionary ();

			Assert.IsFalse (controller.InitialValues == initialValues, "NSUserDefaultsControllerShouldChangeInitialValues - Failed to set the InitialValues property");
		}

		[Test]
		public void NSUserDefaultsControllerShouldChangeAppliesImmediately ()
		{
			controller = new NSUserDefaultsController (NSUserDefaults.StandardUserDefaults, null);
			var appliesImmediately = controller.AppliesImmediately;
			controller.AppliesImmediately = !appliesImmediately;

			Assert.IsFalse (controller.AppliesImmediately == appliesImmediately, "NSUserDefaultsControllerShouldChangeAppliesImmediately - Failed to set the AppliesImmediately property");
		}
	}
}