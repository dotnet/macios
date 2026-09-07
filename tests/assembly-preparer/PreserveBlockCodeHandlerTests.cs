// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Mono.Cecil.Rocks;

namespace AssemblyPreparerTests;

public class PreserveBlockCodeHandlerTests : BaseClass {
	[Test]
	[TestCase (ApplePlatform.MacCatalyst, false)]
	[TestCase (ApplePlatform.iOS, false)]
	[TestCase (ApplePlatform.TVOS, false)]
	[TestCase (ApplePlatform.MacOSX, true)]
	public void MarkedTest (ApplePlatform platform, bool isCoreCLR)
	{
		var code = @"
		using System;
		using ObjCRuntime;
		namespace ObjCRuntime;
		class Trampolines {
			static internal class SDInnerBlock {
				// this field is not preserved by other means, but it must not be linked away
				static internal readonly DInnerBlock Handler = Invoke;

				[MonoPInvokeCallback (typeof (DInnerBlock))]
				static internal void Invoke (IntPtr block, int magic_number)
				{
				}

				public delegate void DInnerBlock (IntPtr block, int magic_number);
			}
		}";

		AssertPrepare (platform, isCoreCLR, code, out var assemblyDefinition);

		var type = assemblyDefinition.MainModule.Types.Single (v => v.Name == "Trampolines").NestedTypes.Single (v => v.Name == "SDInnerBlock");
		var cctor = type.GetStaticConstructor ();
		var attribs = cctor.CustomAttributes?.OrderBy (v => string.Join (", ", v.ConstructorArguments.Select (v => v.Value?.ToString ()))).ToArray ();
		Assert.That (attribs, Is.Not.Null, "Attributes");
		Assert.That (attribs.Count, Is.EqualTo (2), "Attribute count");
		Assert.That (attribs.All (v => v.AttributeType.Name == "DynamicDependencyAttribute"), Is.True, "Attribute name");
		// Handler field: DDA(string memberSignature, string typeName, string assemblyName)
		Assert.That (attribs [0].ConstructorArguments.Count, Is.EqualTo (3), "First attribute's argument count");
		Assert.That ((string) attribs [0].ConstructorArguments [0].Value, Is.EqualTo ("Handler"), "First attribute's first argument");
		Assert.That ((string) attribs [0].ConstructorArguments [1].Value, Is.EqualTo ("ObjCRuntime.Trampolines.SDInnerBlock"), "First attribute's second argument");
		Assert.That ((string) attribs [0].ConstructorArguments [2].Value, Is.EqualTo ("Test"), "First attribute's third argument");

		// Invoke method: DDA(string memberSignature) - same declaring type, so simpler constructor
		Assert.That (attribs [1].ConstructorArguments.Count, Is.EqualTo (1), "Second attribute's argument count");
		Assert.That ((string) attribs [1].ConstructorArguments [0].Value, Is.EqualTo ("Invoke(System.IntPtr,System.Int32)"), "Second attribute's first argument");
	}
}
