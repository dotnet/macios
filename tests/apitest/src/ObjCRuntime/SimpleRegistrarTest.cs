﻿using System;
using System.Runtime.InteropServices;
using NUnit.Framework;

#if !XAMCORE_2_0
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;
#else
using Foundation;
using ObjCRuntime;

#endif

namespace Xamarin.Mac.Tests
{
	[Register ("RegistrarTestClass")]
	class RegistrarTestClass : NSObject 
	{
		public virtual string Value {
			[Export ("value")]
			get {
				return "RegistrarTestClass";
			}
		}
	}

	[Register ("RegistrarTestDerivedClass")]
	class RegistrarTestDerivedClass : RegistrarTestClass 
	{
		public override string Value {
			get {
				return "RegistrarTestDerivedClass";
			}
		}
	}
	
	[TestFixture]
	public class SimpleRegistrarTest
	{
		[DllImport ("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
		public extern static IntPtr IntPtr_objc_msgSend (IntPtr receiver, IntPtr selector);

		[Test]
		public void SimpleRegistrarSmokeTest ()
		{
			RegistrarTestClass obj = new RegistrarTestClass ();
			IntPtr receiver = obj.Handle;

			RegistrarTestDerivedClass derivedObj = new RegistrarTestDerivedClass ();
			IntPtr derivedReceiver = derivedObj.Handle;

			Assert.AreEqual (Runtime.GetNSObject<NSString> (IntPtr_objc_msgSend (receiver, Selector.GetHandle ("value"))), (NSString)"RegistrarTestClass");

			Assert.AreEqual (Runtime.GetNSObject<NSString> (IntPtr_objc_msgSend (derivedReceiver, Selector.GetHandle ("value"))), (NSString)"RegistrarTestDerivedClass");
		}

		[Test]
		public void SimpleRegistrar_XamarinMacRegistered ()
		{
			// __NSObject_Disposer is registered by src/Foundation/NSObject2.cs and should exist
			// This will throw is for some reason it is not
			Class c = new Class ("__NSObject_Disposer");
		}
	}
}
