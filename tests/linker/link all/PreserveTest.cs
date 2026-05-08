//
// Preserve tests
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013-2016 Xamarin Inc. All rights reserved.
//

using System.Reflection;

// this will preserve the specified type (only)
[assembly: Preserve (typeof (LinkAll.Attributes.TypeWithoutMembers))]

// this will preserve the specified type with all it's members
[assembly: Preserve (typeof (LinkAll.Attributes.TypeWithMembers), AllMembers = true)]

// as the preserved field is an attribute this means that [Obfuscation] becomes like [Preserve]
// IOW preserving the attribute does not do much good if what it decorates gets removed
[assembly: Preserve (typeof (ObfuscationAttribute))]

namespace LinkAll.Attributes {

	// type and members preserved by assembly-level attribute above
	class TypeWithMembers {

		public string Present { get; set; } = "";
	}

	// type (only, not members) preserved by assembly-level attribute above
	class TypeWithoutMembers {

		public string Absent { get; set; } = "";
	}

	class MemberWithCustomAttribute {

		// since [Obfuscation] was manually preserved then we'll preserve everything that's decorated with the attribute
		[Obfuscation]
		public string Custom { get; set; } = "";
	}

	[TestFixture]
	// we want the tests to be available because we use the linker
	[Preserve (AllMembers = true)]
	public class PreserveTest {

#if DEBUG
		const bool Debug = true;
#else
		const bool Debug = false;
#endif
		string AssemblyName = typeof (NSObject).Assembly.ToString ();
		string WorkAroundLinkerHeuristics { get { return ""; } }

		[Test]
		public void PreserveTypeWithMembers ()
		{
			var t = Type.GetType ("LinkAll.Attributes.TypeWithMembers" + WorkAroundLinkerHeuristics)!;
			// both type and members are preserved
			Assert.That (t, Is.Not.Null, "type");
			Assert.That (t.GetProperty ("Present"), Is.Not.Null, "members");
		}

		[Test]
		public void PreserveTypeWithoutMembers ()
		{
			var t = Type.GetType ("LinkAll.Attributes.TypeWithoutMembers" + WorkAroundLinkerHeuristics)!;
			// type is preserved
			Assert.That (t, Is.Not.Null, "type");
			// but we did not ask the linker to preserve it's members
			Assert.That (t.GetProperty ("Absent"), Is.Null, "members");
		}

		[Test]
		public void Runtime_RegisterEntryAssembly ()
		{
			TestRuntime.AssertSimulator ("https://github.com/dotnet/macios/issues/10457");

			var klass = Type.GetType ("ObjCRuntime.Runtime, " + AssemblyName)!;
			Assert.That (klass, Is.Not.Null, "Runtime");
			// RegisterEntryAssembly is only needed for the simulator (not on devices) so it's only preserved for sim builds
			var method = klass.GetMethod ("RegisterEntryAssembly", BindingFlags.NonPublic | BindingFlags.Static, null, new [] { typeof (Assembly) }, null);
#if __MACOS__
			var expectedNull = true;
#else
			var expectedNull = TestRuntime.IsDevice;
#endif
			Assert.That (method is null, Is.EqualTo (expectedNull), "RegisterEntryAssembly");
		}

		[Test]
		public void MonoTouchException_Unconditional ()
		{
			const string klassName = "ObjCRuntime.ObjCException";
			var klass = Type.GetType (klassName + ", " + AssemblyName);
			Assert.That (klass, Is.Not.Null, klassName);
		}

		[Test]
		public void Class_Unconditional ()
		{
			var klass = Type.GetType ("ObjCRuntime.Class, " + AssemblyName)!;
			Assert.That (klass, Is.Not.Null, "Class");
			// handle is unconditionally preserved
			var field = klass.GetField ("handle", BindingFlags.NonPublic | BindingFlags.Instance);
			Assert.That (field, Is.Not.Null, "handle");
		}

		[Test]
		public void Runtime_Unconditional ()
		{
			var klass = Type.GetType ("ObjCRuntime.Runtime, " + AssemblyName)!;
			Assert.That (klass, Is.Not.Null, "Runtime");
			// Initialize and a few other methods are unconditionally preserved
			var method = klass.GetMethod ("Initialize", BindingFlags.NonPublic | BindingFlags.Static);
			Assert.That (method, Is.Not.Null, "Initialize");
			method = klass.GetMethod ("RegisterNSObject", BindingFlags.NonPublic | BindingFlags.Static, null, new Type [] { typeof (NSObject), typeof (IntPtr) }, null);
			Assert.That (method, Is.Not.Null, "RegisterNSObject");
		}

		[Test]
		public void Selector_Unconditional ()
		{
			var klass = Type.GetType ("ObjCRuntime.Selector, " + AssemblyName)!;
			Assert.That (klass, Is.Not.Null, "Selector");
			// handle and is unconditionally preserved
			var field = klass.GetField ("handle", BindingFlags.NonPublic | BindingFlags.Instance);
			Assert.That (field, Is.Not.Null, "handle");
			var method = klass.GetMethod ("GetHandle", BindingFlags.Public | BindingFlags.Static);
			Assert.That (method, Is.Not.Null, "GetHandle");
		}

		[Test]
		public void SmartEnumTest ()
		{
			var consumer = GetType ().Assembly.GetType ("LinkAll.Attributes.SmartConsumer" + WorkAroundLinkerHeuristics)!;
			Assert.That (consumer, Is.Not.Null, "SmartConsumer");
			Assert.That (consumer.GetMethod ("GetSmartEnumValue"), Is.Not.Null, "GetSmartEnumValue");
			Assert.That (consumer.GetMethod ("SetSmartEnumValue"), Is.Not.Null, "SetSmartEnumValue");
			var smartEnum = GetType ().Assembly.GetType ("LinkAll.Attributes.SmartEnum")!;
			Assert.That (smartEnum, Is.Not.Null, "SmartEnum");
			var smartExtensions = GetType ().Assembly.GetType ("LinkAll.Attributes.SmartEnumExtensions" + WorkAroundLinkerHeuristics)!;
			Assert.That (smartExtensions, Is.Not.Null, "SmartEnumExtensions");
			Assert.That (smartExtensions.GetMethod ("GetConstant"), Is.Not.Null, "GetConstant");
			Assert.That (smartExtensions.GetMethod ("GetValue"), Is.Not.Null, "GetValue");

			// Unused smart enums and their extensions should be linked away
			Assert.That (typeof (NSObject).Assembly.GetType ("AVFoundation.AVMediaTypes"), Is.Null, "AVMediaTypes");
			Assert.That (typeof (NSObject).Assembly.GetType ("AVFoundation.AVMediaTypesExtensions"), Is.Null, "AVMediaTypesExtensions");
		}

		[Test]
		public void PreserveAllExcludesNestedTypes ()
		{
			var parentClass = GetType ().Assembly.GetType ("LinkAll.Attributes.ParentClass" + WorkAroundLinkerHeuristics);
			Assert.That (parentClass, Is.Not.Null, "ParentClass");
			var nestedEnum = GetType ().Assembly.GetType ("LinkAll.Attributes.ParentClass.NestedEnum" + WorkAroundLinkerHeuristics);
			Assert.That (nestedEnum, Is.Null, "NestedEnum");
			var nestedStruct = GetType ().Assembly.GetType ("LinkAll.Attributes.ParentClass.NestedStruct" + WorkAroundLinkerHeuristics);
			Assert.That (nestedStruct, Is.Null, "NestedStruct");
			var nestedClass = GetType ().Assembly.GetType ("LinkAll.Attributes.ParentClass.NestedClass" + WorkAroundLinkerHeuristics);
			Assert.That (nestedClass, Is.Null, "NestedClass");
		}

		[Test]
		public void PreserveAllKeepsEnumValues ()
		{
			var enumType = GetType ().Assembly.GetType ("LinkAll.Attributes.MyEnum" + WorkAroundLinkerHeuristics)!;
			Assert.That (enumType, Is.Not.Null, "MyEnum");
			Assert.That (enumType.GetFields (BindingFlags.Public | BindingFlags.Static).Length, Is.EqualTo (3), "fields");
			AssertHasStaticField ("A", 1);
			AssertHasStaticField ("B", 2);
			AssertHasStaticField ("C", 4);

			void AssertHasStaticField (string name, int value)
			{
				var field = enumType.GetField (name, BindingFlags.Public | BindingFlags.Static)!;
				Assert.That (field, Is.Not.Null, name);
				Assert.That ((int) field.GetValue (null)!, Is.EqualTo (value), $"{name} == {value}");
			}
		}
	}

	[Preserve (AllMembers = true)]
	class SmartConsumer : NSObject {
		// The Smart Get/Set methods should not be linked away, and neither should the Smart enums + extensions
		[Export ("getSmartEnumValue")]
		[return: BindAs (typeof (SmartEnum), OriginalType = typeof (NSString))]
		public SmartEnum GetSmartEnumValue ()
		{
			return SmartEnum.Smart;
		}

		[Export ("setSmartEnumValue:")]
		public void SetSmartEnumValue ([BindAs (typeof (SmartEnum), OriginalType = typeof (NSString))] SmartEnum value)
		{
		}
	}

	public enum SmartEnum : int {
		Smart = 0,
	}

	public static class SmartEnumExtensions {
		public static NSString GetConstant (this SmartEnum self)
		{
			return (NSString) "Smart";
		}

		public static SmartEnum GetValue (NSString constant)
		{
			return SmartEnum.Smart;
		}
	}

	[Preserve (AllMembers = true)]
	public class ParentClass {
		public enum NestedEnum { A, B };
		public class NestedClass { }
		public struct NestedStruct { }
	}

	[Preserve (AllMembers = true)]
	public enum MyEnum { A = 1, B = 2, C = 4 }
}
