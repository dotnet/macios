// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
using Xamarin.Tests;
using Xamarin.Utils;
using Xunit;
using static Microsoft.Macios.Generator.Emitters.BindingSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

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
				"global::ObjCRuntime.Runtime.RetainAndAutoreleaseNSObject (global::Foundation.NSArray.FromNSObjects (auxVariable))"
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
				"global::ObjCRuntime.Runtime.RetainAndAutoreleaseNSObject (auxVariable)"
			];

			const string nativeObjectResult = @"
using System;
using Foundation;
using Security;

namespace NS {

	public delegate SecKeyChain Callback ();
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nativeObjectResult,
				"global::ObjCRuntime.Runtime.RetainAndAutoreleaseNativeObject (auxVariable)"
			];

			const string protocolResult = @"
using System;
using Foundation;
using Metal;

namespace NS {

	public delegate IMTLTexture Callback ();
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				protocolResult,
				"global::ObjCRuntime.Runtime.RetainAndAutoreleaseNSObject (auxVariable)"
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
				"auxVariable"
			];

			const string voidReturnType = @"
using System;

namespace NS {
	public delegate void Callback()
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				voidReturnType,
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

	class TestDataCreateNativeClass : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return [
				"GetGeolocationCallback",
				ImmutableArray.Create (
					Argument (IdentifierName ("arg0"))
				),
				"NIDGetGeolocationCallback.Create (arg0)!"
			];

			yield return [
				"AVAssetImageGenerateAsynchronouslyForTimeCompletionHandler",
				ImmutableArray.Create (
					Argument (IdentifierName ("arg0")),
					Argument (IdentifierName ("arg1"))
				),
				"NIDAVAssetImageGenerateAsynchronouslyForTimeCompletionHandler.Create (arg0, arg1)!"
			];

			yield return [
				"AVAssetImageGeneratorCompletionHandler",
				ImmutableArray.Create<ArgumentSyntax> (),
				"NIDAVAssetImageGeneratorCompletionHandler.Create ()!"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataCreateNativeClass))]
	public void CreateTrampolineNativeInvocationClassTest (string trampolineName, ImmutableArray<ArgumentSyntax> arguments, string expectedExpression)
	{
		var expression = CreateTrampolineNativeInvocationClass (trampolineName, arguments);
		Assert.Equal (expectedExpression, expression.ToString ());
	}

}
