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
using static Microsoft.Macios.Generator.Emitters.BindingSyntaxFactory;

namespace Microsoft.Macios.Generator.Tests.Emitters;

public class BindingSyntaxFactoryTrampolineTests : BaseGeneratorTestClass {

	class TestDataGetTrampolineInvokeReturnType : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			const string arrayNSObjectResult = @"
using System;
using Foundation;

namespace NS {

	public delegate NSString [] Callback ();
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				arrayNSObjectResult,
				"NSArray.FromNSObjects (auxVariable).GetHandle ()"
			];

			const string nsObjectResult = @"
using System;
using Foundation;

namespace NS {

	public delegate NSString Callback ();
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nsObjectResult,
				"auxVariable.GetHandle ()"
			];

			const string systemStringResult = @"
using System;

namespace NS {

	public delegate string Callback ();
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				systemStringResult,
				"NFString.CreateNative (auxVariable, true)"
			];

			const string nullableSystemStringResult = @"
using System;

namespace NS {

	public delegate string? Callback ();
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nullableSystemStringResult,
				"NFString.CreateNative (auxVariable, true)"
			];

			const string boolReturnType = @"
using System;

namespace NS {

	public delegate bool Callback ();
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				boolReturnType,
				"auxVariable ? (byte) 1 : (byte) 0"
			];

			const string nativeEnum = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {

	[Native (""GKErrorCode"")]
	[BindingType<SmartEnum> (Flags = SmartEnum.ErrorCode, ErrorDomain = ""GKErrorDomain"")]
	public enum NativeSampleEnum : long {
		None = 0,
		Unknown = 1,
	}

	public delegate NativeSampleEnum Callback()
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nativeEnum,
				"(IntPtr) (long) auxVariable",
			];

			const string unsignedNativeEnum = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {

	[Native (""GKErrorCode"")]
	[BindingType<SmartEnum> (Flags = SmartEnum.ErrorCode, ErrorDomain = ""GKErrorDomain"")]
	public enum NativeSampleEnum : ulong {
		None = 0,
		Unknown = 1,
	}

	public delegate NativeSampleEnum Callback()
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				unsignedNativeEnum,
				"(UIntPtr) (ulong) auxVariable",
			];

			const string intReturnType = @"
using System;

namespace NS {
	public delegate int Callback()
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				intReturnType,
				null!
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataGetTrampolineInvokeReturnType>]
	void GetTrampolineInvokeReturnTypeTests (ApplePlatform platform, string inputText, string? expectedExpression)
	{
		var (compilation, syntaxTrees) = CreateCompilation (platform, sources: inputText);
		Assert.Single (syntaxTrees);
		var semanticModel = compilation.GetSemanticModel (syntaxTrees [0]);
		var declaration = syntaxTrees [0].GetRoot ()
			.DescendantNodes ().OfType<MethodDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (declaration);
		Assert.True (Method.TryCreate (declaration, semanticModel, out var changes));
		Assert.NotNull (changes);
		// we know the first parameter of the method is the delegate
		Assert.Single (changes.Value.Parameters);
		var parameter = changes.Value.Parameters [0];
		var auxVariableName = "auxVariable";
		var expression = GetTrampolineInvokeReturnType (parameter.Type, auxVariableName);
		Assert.Equal (expectedExpression, expression?.ToString ());
	}
}
