// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Linq;

using Mono.Cecil;
using Xamarin.Tests;

namespace ObjCRuntime {
	interface ICreatableNativeObject<T> { }
}

namespace GeneratorTests {
	[TestFixture]
	public class NativeObjectFactoryTests : BGenBase {
		class Wrapper : ObjCRuntime.ICreatableNativeObject<Wrapper> { }
		class InheritedWrapper : Wrapper { }
		class DerivedWrapper : Wrapper, ObjCRuntime.ICreatableNativeObject<DerivedWrapper> { }
		class GenericWrapper<T> : ObjCRuntime.ICreatableNativeObject<GenericWrapper<T>> { }
		abstract class AbstractWrapper : ObjCRuntime.ICreatableNativeObject<AbstractWrapper> { }

		[TestCase (typeof (Wrapper), true)]
		[TestCase (typeof (InheritedWrapper), false)]
		[TestCase (typeof (DerivedWrapper), true)]
		[TestCase (typeof (GenericWrapper<int>), true)]
		[TestCase (typeof (AbstractWrapper), false)]
		[TestCase (typeof (IDisposable), false)]
		[TestCase (typeof (object), false)]
		public void RequiresSelfTypedFactory (Type type, bool expected)
		{
			Assert.That (TypeManager.HasNativeObjectFactory (type), Is.EqualTo (expected));
		}

		[Test]
		public void GeneratedConversions ()
		{
			var bgen = BuildFile (Profile.iOS, v => {
				v.Sources.Add (v.GetFullPathRelativeToGeneratorDirectory ("native-object-factories-sources.cs"));
				v.AddExtraSourcesRelativeToGeneratorDirectory ("native-object-factories-partial.cs");
				v.NoWarn = "0436"; // The partial wrapper deliberately shadows its framework type.
			}, "native-object-factories.cs");
			bgen.AssertApiCallsMethod ("NativeFactories", "Consumer", "get_Path", "GetCreatableNativeObject", "Return value");
			bgen.AssertApiCallsMethod ("NativeFactories", "Consumer", "UpdatePath", "GetCreatableNativeObject", "Reference parameter");
			bgen.AssertApiCallsMethod ("NativeFactories", "Consumer", "get_Paths", "ArrayFromHandle", "Array return value");
			bgen.AssertApiCallsMethod ("NativeFactories", "Consumer", "UpdatePaths", "ArrayFromHandle", "Reference array");
			bgen.AssertApiCallsMethod ("NativeFactories", "Consumer", "get_Foreign", "GetCreatableNativeObject", "Third-party factory");
			bgen.AssertApiCallsMethod ("NativeFactories", "Consumer", "get_Inherited", "GetINativeObject", "Inherited non-self-typed interface");
			bgen.AssertApiCallsMethod ("NativeFactories", "Options", "get_Path", "GetCreatableNativeObject", "Strong dictionary value");
			bgen.AssertApiCallsMethod ("NativeFactories", "Keys", "get_NativePath", "GetCreatableNativeObject", "Native field");
			bgen.AssertApiCallsMethod ("NativeFactories", "Consumer", "get_Data", "GetCreatableNativeObject", "Partial binding return value");
			bgen.AssertApiCallsMethod ("NativeFactories", "Consumer", "UpdateData", "GetCreatableNativeObject", "Partial binding reference parameter");
			bgen.AssertApiCallsMethod ("NativeFactories", "Options", "get_Data", "GetCreatableNativeObject", "Partial binding strong dictionary value");

			var trampoline = bgen.ApiAssembly.MainModule.GetType ("ObjCRuntime", "Trampolines").NestedTypes.Single (v => v.Name == "SDFactoryCallback");
			bgen.AssertApiCallsMethod (trampoline.Methods.Single (v => v.Name == "Invoke"), "GetCreatableNativeObject", "Callback parameters");

			var calls = bgen.ApiAssembly.MainModule.GetTypes ().SelectMany (v => v.Methods)
				.Where (v => v.HasBody && v.Name != "get_Inherited").SelectMany (v => v.Body.Instructions)
				.Select (v => v.Operand).OfType<MethodReference> ()
				.Where (v => v.DeclaringType.FullName == "ObjCRuntime.Runtime");
			Assert.That (calls.Select (v => v.Name), Does.Not.Contain ("GetINativeObject"), "No reflective native-object conversions");
		}
	}
}
