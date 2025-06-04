// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
using Xamarin.Tests;
using Xamarin.Utils;
using Xunit;
using static Microsoft.Macios.Generator.Tests.TestDataFactory;

namespace Microsoft.Macios.Generator.Tests;

public class NomenclatorTests : BaseGeneratorTestClass {

	[Theory]
	[InlineData ("AVCaptureDeviceType", "AVCaptureDeviceTypeExtensions")]
	[InlineData ("GKError", "GKErrorExtensions")]
	public void GetSmartEnumExtensionClassNameTests (string enumName, string expectedName)
		=> Assert.Equal (Nomenclator.GetSmartEnumExtensionClassName (enumName), expectedName);

	[Theory]
	[AllSupportedPlatforms]
	public void GetTrampolineNameGeneric (ApplePlatform platform)
	{
		// write a sample code to retrieve the roslyn symbol and type info so that 
		// we can test the nomenclator.
		var code = @"
using System;
using System.Collections.Generic;

namespace Test;

public class GenericTrampoline<T> where T : class {
	void DidAccelerateSeveral (object accelerometer, object second, object last);
}

public class Example {
	public 	GenericTrampoline<string> Trampoline { get; set; }
}
";

		var (compilation, syntaxTrees) = CreateCompilation (platform, sources: code);

		Assert.Single (syntaxTrees);
		var declaration = syntaxTrees [0].GetRoot ()
			.DescendantNodes ()
			.OfType<PropertyDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (declaration);
		var semanticModel = compilation.GetSemanticModel (syntaxTrees [0]);
		Assert.NotNull (semanticModel);
		Assert.True (Property.TryCreate (declaration, semanticModel, out var property));
		Assert.NotNull (property);
		var type = property.Value.ReturnType;

		// should always return the same name for the trampoline, even if called multiple times
		var name1 = Nomenclator.GetTrampolineName (type);
		var name2 = Nomenclator.GetTrampolineName (type);
		// compare names and ensure that the correct number is used
		Assert.Equal ("GenericTrampolineArity1string", name1);
		Assert.Equal (name1, name2);
	}

	[Theory]
	[AllSupportedPlatforms]
	public void GetTrampolineNestedClass (ApplePlatform platform)
	{
		// write a sample code to retrieve the roslyn symbol and type info so that 
		// we can test the nomenclator.
		var code = @"
using System;
using System.Collections.Generic;

namespace Test;

public class Example {
	public class GenericTrampoline<T> where T : class {
		void DidAccelerateSeveral (object accelerometer, object second, object last);
	}

	public 	GenericTrampoline<string> Trampoline { get; set; }
}
";

		var (compilation, syntaxTrees) = CreateCompilation (platform, sources: code);

		Assert.Single (syntaxTrees);
		var declaration = syntaxTrees [0].GetRoot ()
			.DescendantNodes ()
			.OfType<PropertyDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (declaration);
		var semanticModel = compilation.GetSemanticModel (syntaxTrees [0]);
		Assert.NotNull (semanticModel);
		Assert.True (Property.TryCreate (declaration, semanticModel, out var property));
		Assert.NotNull (property);
		var type = property.Value.ReturnType;

		var name1 = Nomenclator.GetTrampolineName (type);
		var name2 = Nomenclator.GetTrampolineName (type);
		// compare names and ensure that the correct number is used
		Assert.Equal ("Example_GenericTrampolineArity1string", name1);
		Assert.Equal (name1, name2);
	}

	[Theory]
	[AllSupportedPlatforms]
	public void GetTrampolineClassNameTest (ApplePlatform platform)
	{
		// write a sample code to retrieve the roslyn symbol and type info so that 
		// we can test the nomenclator.
		var code = @"
using System;
using System.Collections.Generic;

namespace Test;

public class GenericTrampoline<T> where T : class {
	void DidAccelerateSeveral (object accelerometer, object second, object last);
}

public class Example {
	public 	GenericTrampoline<string> Trampoline { get; set; }
}
";

		var (compilation, syntaxTrees) = CreateCompilation (platform, sources: code);

		Assert.Single (syntaxTrees);
		var declaration = syntaxTrees [0].GetRoot ()
			.DescendantNodes ()
			.OfType<PropertyDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (declaration);
		var semanticModel = compilation.GetSemanticModel (syntaxTrees [0]);
		Assert.NotNull (semanticModel);
		Assert.True (Property.TryCreate (declaration, semanticModel, out var property));
		Assert.NotNull (property);
		var type = property.Value.ReturnType;

		var trampolineName = Nomenclator.GetTrampolineName (type);
		// get the class name for each of the types and ensure that the correct value is used
		Assert.Equal ($"D{trampolineName}", Nomenclator.GetTrampolineClassName (trampolineName, Nomenclator.TrampolineClassType.DelegateType));
		Assert.Equal ($"SD{trampolineName}", Nomenclator.GetTrampolineClassName (trampolineName, Nomenclator.TrampolineClassType.StaticBridgeClass));
		Assert.Equal ($"NID{trampolineName}", Nomenclator.GetTrampolineClassName (trampolineName, Nomenclator.TrampolineClassType.NativeInvocationClass));
	}

	class TestDataGetVariableName : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			var exampleParameter = new Parameter (0, ReturnTypeForBool (), "firstParameter");
			yield return [exampleParameter, Nomenclator.VariableType.Handle, $"{exampleParameter.Name}__handle__"];
			yield return [exampleParameter, Nomenclator.VariableType.BlockLiteral, $"block_ptr_{exampleParameter.Name}"];
			yield return [exampleParameter, Nomenclator.VariableType.PrimitivePointer, $"converted_{exampleParameter.Name}"];
			yield return [exampleParameter, Nomenclator.VariableType.NSArray, $"nsa_{exampleParameter.Name}"];
			yield return [exampleParameter, Nomenclator.VariableType.NSString, $"ns{exampleParameter.Name}"];
			yield return [exampleParameter, Nomenclator.VariableType.NSStringStruct, $"_s{exampleParameter.Name}"];
			yield return [exampleParameter, Nomenclator.VariableType.BindFrom, $"nsb_{exampleParameter.Name}"];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataGetVariableName))]
	void GetNameForVariableTypeTests (Parameter parameter, Nomenclator.VariableType variableType, string expectedName)
		=> Assert.Equal (expectedName, Nomenclator.GetNameForVariableType (parameter.Name, variableType));

	class TestDataGetNameForTempTrampolineVariable : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			var exampleParameter = new DelegateParameter (0, ReturnTypeForBool (), "firstParameter");
			// bool, diff pos
			yield return [exampleParameter, $"__xamarin_bool__{exampleParameter.Position}"];
			exampleParameter = new (4, ReturnTypeForBool (), "firstParameter");
			yield return [exampleParameter, $"__xamarin_bool__{exampleParameter.Position}"];

			// nullable value type, diff pos
			exampleParameter = new (0, ReturnTypeForInt (isNullable: true), "firstParameter");
			yield return [exampleParameter, $"__xamarin_nullified__{exampleParameter.Position}"];
			exampleParameter = new (4, ReturnTypeForInt (isNullable: true), "firstParameter");
			yield return [exampleParameter, $"__xamarin_nullified__{exampleParameter.Position}"];

			exampleParameter = new (0, ReturnTypeForNSObject ("MyNSObject"), "firstParameter");
			yield return [exampleParameter, $"__xamarin_pref{exampleParameter.Position}"];
			exampleParameter = new (4, ReturnTypeForNSObject ("MyNSObject"), "firstParameter");
			yield return [exampleParameter, $"__xamarin_pref{exampleParameter.Position}"];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataGetNameForTempTrampolineVariable))]
	void GetNameForTempTrampolineVariable (DelegateParameter parameter, string expectedName)
		=> Assert.Equal (expectedName, Nomenclator.GetNameForTempTrampolineVariable (parameter));
}

