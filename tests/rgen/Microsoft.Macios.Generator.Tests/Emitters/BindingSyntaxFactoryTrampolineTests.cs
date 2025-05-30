// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.IO;
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

	class TestDataGetTrampolineInvokeArgument : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			var pointerParameter = @"
using System;

namespace NS {
	public delegate void Callback (int* pointerParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				pointerParameter,
				"pointerParameter" // uses the parameter name
			];

			var ccallbackParameter = @"
using System;
using ObjCRuntime;

namespace NS {
	public delegate void Callback ([CCallback] Action callbackParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				ccallbackParameter,
				$"global::System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<{Global ("System.Action")}> (callbackParameter)"
			];

			var blockParameter = @"
using System;
using ObjCRuntime;

namespace NS {
	public delegate void Callback ([BlockCallback] Action callbackParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				blockParameter,
				"NIDsomeTrampolineName.Create (callbackParameter)!",
			];

			var nativeEnumParameter = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {

        [Native (""""GKErrorCode"""")]
        [BindingType<SmartEnum> (Flags = SmartEnum.ErrorCode, ErrorDomain = """"GKErrorDomain"""")]
        public enum NativeSampleEnum : long {
                None = 0,
                Unknown = 1,
        }

        public delegate void Callback (NativeSampleEnum enumParameter);
        public class MyClass {
                public void MyMethod (Callback cb) {}
        }
}
";

			yield return [
				"someTrampolineName",
				nativeEnumParameter,
				$"({Global ("NS.NativeSampleEnum")}) (long) enumParameter",
			];

			var boolParameter = @"
using System;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				boolParameter,
				"boolParameter != 0",
			];

			var nsObjectArray = @"
using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (NSObject[] nsObjectArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				nsObjectArray,
				$"global::CoreFoundation.CFArray.ArrayFromHandle<{Global ("Foundation.NSObject")}> (nsObjectArray)!",
			];

			var iNativeObjectArray = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (CMTimebase[] inativeArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				iNativeObjectArray,
				$"global::Foundation.NSArray.ArrayFromHandle<{Global ("CoreMedia.CMTimebase")}> (inativeArray)!",
			];

			var stringArray = @"
using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (string [] stringArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				stringArray,
				"global::CoreFoundation.CFArray.StringArrayFromHandle (stringArray)!",
			];

			var stringParameter = @"
using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (string stringParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				stringParameter,
				"global::CoreFoundation.CFString.FromHandle (stringParameter)!",
			];

			var protocolParameter = @"
using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (INSUrlConnectionDataDelegate protocolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				protocolParameter,
				$"global::ObjCRuntime.Runtime.GetINativeObject<{Global ("Foundation.INSUrlConnectionDataDelegate")}> (protocolParameter, false)!",
			];

			var forcedParameterOwnsFalse = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback ([ForcedType (false)]INSUrlConnectionDataDelegate forcedParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				forcedParameterOwnsFalse,
				$"global::ObjCRuntime.Runtime.GetINativeObject<{Global ("Foundation.INSUrlConnectionDataDelegate")}> (forcedParameter, true, false)!",
			];

			var forcedParameterOwnsTrue = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback ([ForcedType (true)]INSUrlConnectionDataDelegate forcedParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				forcedParameterOwnsTrue,
				$"global::ObjCRuntime.Runtime.GetINativeObject<{Global ("Foundation.INSUrlConnectionDataDelegate")}> (forcedParameter, true, true)!",
			];

			var nsObjectParameter = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (NSObject nsObjectParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				nsObjectParameter,
				$"global::ObjCRuntime.Runtime.GetNSObject<{Global ("Foundation.NSObject")}> (nsObjectParameter)!",
			];

			var nullableNSObjectParameter = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (NSObject? nsObjectParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				nullableNSObjectParameter,
				$"global::ObjCRuntime.Runtime.GetNSObject<{Global ("Foundation.NSObject")}> (nsObjectParameter)",
			];

			var iNativeParameter = @"
using System;
using CoreMedia;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (CMTimebase inativeParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				iNativeParameter,
				$"global::ObjCRuntime.Runtime.GetINativeObject<{Global ("CoreMedia.CMTimebase")}> (inativeParameter, false)!",
			];

			var nullableINativeParameter = @"
using System;
using CoreMedia;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (CMTimebase? inativeParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				nullableINativeParameter,
				$"global::ObjCRuntime.Runtime.GetINativeObject<{Global ("CoreMedia.CMTimebase")}> (inativeParameter, false)",
			];

			var cmSampleBuffer = @"
using System;
using CoreMedia;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (CMSampleBuffer cmSampleBuffer);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				cmSampleBuffer,
				"cmSampleBuffer == IntPtr.Zero ? null! : new global::CoreMedia.CMSampleBuffer (cmSampleBuffer, false)",
			];

			var audioBuffer = @"
using System;
using AudioToolbox;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (AudioBuffers audioBuffer);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				audioBuffer,
				$"new {Global ("AudioToolbox.AudioBuffers")} (audioBuffer)",
			];

			var outNullableInt = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (out int? outNullableInt);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				outNullableInt,
				"out __xamarin_nullified__0",
			];

			var refNullableInt = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (ref int? outNullableInt);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				refNullableInt,
				"ref __xamarin_nullified__0",
			];

			var outBoolean = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (out bool outBool);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				outBoolean,
				"out __xamarin_bool__0",
			];

			var refBoolean = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (ref bool outBool);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				refBoolean,
				"ref __xamarin_bool__0",
			];

			var outInt = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (out int outInt);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				outInt,
				$"out {Global ("System.Runtime.CompilerServices.Unsafe")}.AsRef<int> (outInt)",
			];

			var refInt = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (ref int outInt);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				refInt,
				$"ref {Global ("System.Runtime.CompilerServices.Unsafe")}.AsRef<int> (outInt)",
			];

			var outNSObject = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (out NSObject outNSObject);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				outNSObject,
				"out __xamarin_pref0",
			];

			var valueType = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (int valueType);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				valueType,
				"valueType",
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataGetTrampolineInvokeArgument>]
	void GetTrampolineInvokeArgumentTests (ApplePlatform platform, string trampolineName, string inputText, string expectedExpression)
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
		// assert it is indeed a delegate
		Assert.NotNull (parameter.Type.Delegate);
		var expression = GetTrampolineInvokeArgument (trampolineName, parameter.Type.Delegate!.Parameters [0]);
		Assert.Equal (expectedExpression, expression.ToString ());
	}


	class TestDataGetTrampolinePreInvokeArgumentConversions : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			var pointerParameter = @"
using System;

namespace NS {
	public delegate void Callback (int* pointerParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				pointerParameter,
				string.Empty,
			];

			var ccallbackParameter = @"
using System;
using ObjCRuntime;

namespace NS {
	public delegate void Callback ([CCallback] Action callbackParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				ccallbackParameter,
				string.Empty,
			];

			var blockParameter = @"
using System;
using ObjCRuntime;

namespace NS {
	public delegate void Callback ([BlockCallback] Action callbackParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				blockParameter,
				string.Empty,
			];

			var nativeEnumParameter = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {

        [Native (""""GKErrorCode"""")]
        [BindingType<SmartEnum> (Flags = SmartEnum.ErrorCode, ErrorDomain = """"GKErrorDomain"""")]
        public enum NativeSampleEnum : long {
                None = 0,
                Unknown = 1,
        }

        public delegate void Callback (NativeSampleEnum enumParameter);
        public class MyClass {
                public void MyMethod (Callback cb) {}
        }
}
";

			yield return [
				"someTrampolineName",
				nativeEnumParameter,
				string.Empty,
			];

			var boolParameter = @"
using System;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				boolParameter,
				string.Empty,
			];

			var nsObjectArray = @"
using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (NSObject[] nsObjectArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				nsObjectArray,
				string.Empty,
			];

			var iNativeObjectArray = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (CMTimebase[] inativeArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				iNativeObjectArray,
				string.Empty,
			];

			var stringArray = @"
using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (string [] stringArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				stringArray,
				string.Empty,
			];

			var stringParameter = @"
using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (string stringParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				stringParameter,
				string.Empty,
			];

			var protocolParameter = @"
using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (INSUrlConnectionDataDelegate protocolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				protocolParameter,
				string.Empty,
			];

			var forcedParameterOwnsFalse = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback ([ForcedType (false)]INSUrlConnectionDataDelegate forcedParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				forcedParameterOwnsFalse,
				string.Empty,
			];

			var forcedParameterOwnsTrue = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback ([ForcedType (true)]INSUrlConnectionDataDelegate forcedParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				forcedParameterOwnsTrue,
				string.Empty,
			];

			var nsObjectParameter = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (NSObject nsObjectParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				nsObjectParameter,
				string.Empty,
			];

			var iNativeParameter = @"
using System;
using CoreMedia;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (CMTimebase inativeParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				iNativeParameter,
				string.Empty,
			];

			var cmSampleBuffer = @"
using System;
using CoreMedia;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (CMSampleBuffer cmSampleBuffer);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				cmSampleBuffer,
				string.Empty,
			];

			var audioBuffer = @"
using System;
using AudioToolbox;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (AudioBuffers audioBuffer);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				audioBuffer,
				string.Empty,
			];

			var outNullableInt = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (out int? outNullableInt);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				outNullableInt,
				@"int? __xamarin_nullified__0 = null;
if (outNullableInt is not null)
	__xamarin_nullified__0 = *outNullableInt;
",
			];

			var outBoolean = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (out bool outBool);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				outBoolean,
				"bool __xamarin_bool__0 = *outBool != 0;\n",
			];

			var outNSObject = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (out NSObject outNSObject);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				outNSObject,
				string.Empty,
			];

			var valueType = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (int valueType);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				valueType,
				string.Empty,
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataGetTrampolinePreInvokeArgumentConversions>]
	void GetTrampolinePreInvokeArgumentConversionsTests (ApplePlatform platform, string trampolineName, string inputText, string expectedExpression)
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
		// assert it is indeed a delegate
		Assert.NotNull (parameter.Type.Delegate);
		var conversions = GetTrampolinePreInvokeArgumentConversions (trampolineName, parameter.Type.Delegate!.Parameters [0]);
		// uses a tabbeb string builder to get the conversion string and test
		var sb = new TabbedStringBuilder (new ());
		sb.Write (conversions);
		Assert.Equal (expectedExpression, sb.ToCode ());
	}

	class TestDataGetTrampolinePostInvokeArgumentConversions : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			var pointerParameter = @"
using System;

namespace NS {
	public delegate void Callback (int* pointerParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				pointerParameter,
				string.Empty,
			];

			var ccallbackParameter = @"
using System;
using ObjCRuntime;

namespace NS {
	public delegate void Callback ([CCallback] Action callbackParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				ccallbackParameter,
				string.Empty,
			];

			var blockParameter = @"
using System;
using ObjCRuntime;

namespace NS {
	public delegate void Callback ([BlockCallback] Action callbackParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				blockParameter,
				string.Empty,
			];

			var nativeEnumParameter = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {

        [Native (""""GKErrorCode"""")]
        [BindingType<SmartEnum> (Flags = SmartEnum.ErrorCode, ErrorDomain = """"GKErrorDomain"""")]
        public enum NativeSampleEnum : long {
                None = 0,
                Unknown = 1,
        }

        public delegate void Callback (NativeSampleEnum enumParameter);
        public class MyClass {
                public void MyMethod (Callback cb) {}
        }
}
";

			yield return [
				"someTrampolineName",
				nativeEnumParameter,
				string.Empty,
			];

			var boolParameter = @"
using System;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				boolParameter,
				string.Empty,
			];

			var nsObjectArray = @"
using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (NSObject[] nsObjectArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				nsObjectArray,
				string.Empty,
			];

			var iNativeObjectArray = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (CMTimebase[] inativeArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				iNativeObjectArray,
				string.Empty,
			];

			var stringArray = @"
using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (string [] stringArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				stringArray,
				string.Empty,
			];

			var stringParameter = @"
using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (string stringParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				stringParameter,
				string.Empty,
			];

			var protocolParameter = @"
using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (INSUrlConnectionDataDelegate protocolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				protocolParameter,
				string.Empty,
			];

			var forcedParameterOwnsFalse = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback ([ForcedType (false)]INSUrlConnectionDataDelegate forcedParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				forcedParameterOwnsFalse,
				string.Empty,
			];

			var forcedParameterOwnsTrue = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback ([ForcedType (true)]INSUrlConnectionDataDelegate forcedParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				forcedParameterOwnsTrue,
				string.Empty,
			];

			var nsObjectParameter = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (NSObject nsObjectParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				nsObjectParameter,
				string.Empty,
			];

			var iNativeParameter = @"
using System;
using CoreMedia;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (CMTimebase inativeParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				iNativeParameter,
				string.Empty,
			];

			var cmSampleBuffer = @"
using System;
using CoreMedia;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (CMSampleBuffer cmSampleBuffer);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				cmSampleBuffer,
				string.Empty,
			];

			var audioBuffer = @"
using System;
using AudioToolbox;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (AudioBuffers audioBuffer);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				audioBuffer,
				string.Empty,
			];

			var outNullableInt = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (out int? outNullableInt);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				outNullableInt,
				@"if (outNullableInt is not null && __xamarin_nullified__0.HasValue)
	*outNullableInt = __xamarin_nullified__0.Value;
",
			];

			var outBoolean = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (out bool outBool);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				outBoolean,
				"*outBool = __xamarin_bool__0 ? (byte)1 : (byte)0;\n",
			];

			var outNSObject = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (out NSObject outNSObject);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				outNSObject,
				@"if (outNSObject is not null)
	*outNSObject = Runtime.RetainAndAutoreleaseNativeObject(__xamarin_pref0);
",
			];

			var valueType = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (int valueType);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				valueType,
				string.Empty,
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataGetTrampolinePostInvokeArgumentConversions>]
	void GetTrampolinePostInvokeArgumentConversionsTests (ApplePlatform platform, string trampolineName, string inputText, string expectedExpression)
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
		// assert it is indeed a delegate
		Assert.NotNull (parameter.Type.Delegate);
		var conversions = GetTrampolinePostInvokeArgumentConversions (trampolineName, parameter.Type.Delegate!.Parameters [0]);
		// uses a tabbeb string builder to get the conversion string and test
		var sb = new TabbedStringBuilder (new ());
		sb.Write (conversions);
		Assert.Equal (expectedExpression, sb.ToCode ());
	}

	class TestDataGetTrampolineDelegateDeclaration : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			var pointerParameter = @"
using System;

namespace NS {
	public delegate void Callback (int* pointerParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				pointerParameter,
				"DCallback",
				"unsafe internal delegate void DCallback (global::System.IntPtr block_ptr, int* pointerParameter);",
			];

			var ccallbackParameter = @"
using System;
using ObjCRuntime;

namespace NS {
	public delegate void Callback ([CCallback] Action callbackParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				ccallbackParameter,
				"DCallback",
				"unsafe internal delegate void DCallback (global::System.IntPtr block_ptr, global::System.IntPtr callbackParameter);",
			];

			var nativeEnumParameter = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {

        [Native (""""GKErrorCode"""")]
        [BindingType<SmartEnum> (Flags = SmartEnum.ErrorCode, ErrorDomain = """"GKErrorDomain"""")]
        public enum NativeSampleEnum : long {
                None = 0,
                Unknown = 1,
        }

        public delegate void Callback (NativeSampleEnum enumParameter);
        public class MyClass {
                public void MyMethod (Callback cb) {}
        }
}
";

			yield return [
				nativeEnumParameter,
				"DCallback",
				$"unsafe internal delegate void DCallback (global::System.IntPtr block_ptr, {Global ("System.IntPtr")} enumParameter);",
			];

			var boolParameter = @"
using System;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				boolParameter,
				"DCallback",
				"unsafe internal delegate void DCallback (global::System.IntPtr block_ptr, byte boolParameter);",
			];

			var nsObjectArray = @"
using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (NSObject[] nsObjectArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nsObjectArray,
				"DCallback",
				"unsafe internal delegate void DCallback (global::System.IntPtr block_ptr, global::ObjCRuntime.NativeHandle nsObjectArray);",
			];

			var iNativeObjectArray = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (CMTimebase[] inativeArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				iNativeObjectArray,
				"DCallback",
				"unsafe internal delegate void DCallback (global::System.IntPtr block_ptr, global::ObjCRuntime.NativeHandle inativeArray);"
			];


			var stringArray = @"
using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (string [] stringArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				stringArray,
				"DCallback",
				"unsafe internal delegate void DCallback (global::System.IntPtr block_ptr, global::ObjCRuntime.NativeHandle stringArray);",
			];

			var protocolParameter = @"
using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (INSUrlConnectionDataDelegate protocolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				protocolParameter,
				"DCallback",
				"unsafe internal delegate void DCallback (global::System.IntPtr block_ptr, global::ObjCRuntime.NativeHandle protocolParameter);"
			];

			var outNullableInt = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (out int? outNullableInt);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				outNullableInt,
				"DCallback",
				"unsafe internal delegate void DCallback (global::System.IntPtr block_ptr, int* outNullableInt);"
			];

			var outInt = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (out int? outNullableInt);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				outInt,
				"DCallback",
				"unsafe internal delegate void DCallback (global::System.IntPtr block_ptr, int* outNullableInt);"
			];

			var outBoolean = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (out bool outBool);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				outBoolean,
				"DCallback",
				"unsafe internal delegate void DCallback (global::System.IntPtr block_ptr, byte* outBool);",
			];


			var outNSObject = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (ref NSObject outNSObject);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				outNSObject,
				"DCallback",
				"unsafe internal delegate void DCallback (global::System.IntPtr block_ptr, global::ObjCRuntime.NativeHandle* outNSObject);"
			];

			var valueReturnboolParameter = @"
using System;
using ObjCRuntime;

namespace NS {
	public delegate int Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				valueReturnboolParameter,
				"DCallback",
				"unsafe internal delegate int DCallback (global::System.IntPtr block_ptr, byte boolParameter);",
			];

			var nsObjectReturnBoolParameter = @"
using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	public delegate NSObject Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nsObjectReturnBoolParameter,
				"DCallback",
				"unsafe internal delegate global::ObjCRuntime.NativeHandle DCallback (global::System.IntPtr block_ptr, byte boolParameter);",
			];

			var nullableNSObjectReturnBoolParameter = @"
using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	public delegate NSObject? Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nullableNSObjectReturnBoolParameter,
				"DCallback",
				"unsafe internal delegate global::ObjCRuntime.NativeHandle DCallback (global::System.IntPtr block_ptr, byte boolParameter);",
			];

			var arrayNSObjectReturnBoolParameter = @"
using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	public delegate NSObject[] Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				arrayNSObjectReturnBoolParameter,
				"DCallback",
				"unsafe internal delegate global::ObjCRuntime.NativeHandle DCallback (global::System.IntPtr block_ptr, byte boolParameter);",
			];

			var nullableArrayNSObjectReturnBoolParameter = @"
using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	public delegate NSObject[]? Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nullableArrayNSObjectReturnBoolParameter,
				"DCallback",
				"unsafe internal delegate global::ObjCRuntime.NativeHandle DCallback (global::System.IntPtr block_ptr, byte boolParameter);",
			];

			var intPtrParameters = @"
using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	public delegate int Callback (IntPtr timestamp, uint frameCount, IntPtr inputData);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				intPtrParameters,
				"DCallback",
				"unsafe internal delegate int DCallback (global::System.IntPtr block_ptr, global::System.IntPtr timestamp, uint frameCount, global::System.IntPtr inputData);",
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataGetTrampolineDelegateDeclaration>]
	void GetTrampolineDelegateDeclarationTests (ApplePlatform platform, string inputText, string expectedDelegateName, string expectedExpression)
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
		// assert it is indeed a delegate
		Assert.NotNull (parameter.Type.Delegate);
		var delegateDeclaration = GetTrampolineDelegateDeclaration (parameter.Type, expectedDelegateName);
		Assert.Equal (expectedExpression, delegateDeclaration.ToString ());
	}

	class TestDataCallTrampolineDelegate : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			var pointerParameter = @"
using System;

namespace NS {
	public delegate void Callback (int* pointerParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				pointerParameter,
				"del (pointerParameter);",
			];

			var pointerParameterWithReturn = @"
using System;

namespace NS {
	public delegate int Callback (int* pointerParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				pointerParameterWithReturn,
				"var ret = del (pointerParameter);",
			];
			var ccallbackParameter = @"
using System;
using ObjCRuntime;

namespace NS {
	public delegate void Callback ([CCallback] Action callbackParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				ccallbackParameter,
				$"del (global::System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<{Global ("System.Action")}> (callbackParameter));",
			];

			var severalParametersConversion = @"
using System;
using ObjCRuntime;

namespace NS {
	public delegate void Callback ([CCallback] Action callbackParameter, [BlockCallback] Action callbackParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				severalParametersConversion,
				$"del (global::System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<{Global ("System.Action")}> (callbackParameter), NIDsomeTrampolineName.Create (callbackParameter)!);",
			];

			var severalParametersConversionReturn = @"
using System;
using ObjCRuntime;

namespace NS {
	public delegate int Callback ([CCallback] Action callbackParameter, [BlockCallback] Action callbackParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				severalParametersConversionReturn,
				$"var ret = del (global::System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<{Global ("System.Action")}> (callbackParameter), NIDsomeTrampolineName.Create (callbackParameter)!);",
			];

		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataCallTrampolineDelegate>]
	void CallTrampolineDelegateTests (ApplePlatform platform, string trampolineName, string inputText, string expectedExpression)
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
		// assert it is indeed a delegate
		Assert.NotNull (parameter.Type.Delegate);
		var argumentSyntax = GetTrampolineInvokeArguments (trampolineName, parameter.Type.Delegate);
		var invocation = CallTrampolineDelegate (parameter.Type.Delegate, argumentSyntax);
		Assert.Equal (expectedExpression, invocation.ToFullString ());
	}

	class TestDataGetTrampolineInvokeParameter : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			var pointerParameter = @"
using System;

namespace NS {
	public delegate void Callback (int* pointerParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				pointerParameter,
				"int* pointerParameter"
			];


			var ccallbackParameter = @"
using System;
using ObjCRuntime;

namespace NS {
	public delegate void Callback ([CCallback] Action callbackParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				ccallbackParameter,
				"global::System.IntPtr callbackParameter"
			];

			var blockParameter = @"
using System;
using ObjCRuntime;

namespace NS {
	public delegate void Callback ([BlockCallback] Action callbackParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				blockParameter,
				"global::System.IntPtr callbackParameter",
			];

			var nativeEnumParameter = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {

        [Native (""""GKErrorCode"""")]
        [BindingType<SmartEnum> (Flags = SmartEnum.ErrorCode, ErrorDomain = """"GKErrorDomain"""")]
        public enum NativeSampleEnum : long {
                None = 0,
                Unknown = 1,
        }

        public delegate void Callback (NativeSampleEnum enumParameter);
        public class MyClass {
                public void MyMethod (Callback cb) {}
        }
}
";

			yield return [
				nativeEnumParameter,
				$"{Global ("System.IntPtr")} enumParameter",
			];

			var boolParameter = @"
using System;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				boolParameter,
				"byte boolParameter",
			];

			var nsObjectArray = @"
using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (NSObject[] nsObjectArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nsObjectArray,
				"global::ObjCRuntime.NativeHandle nsObjectArray",
			];



			var iNativeObjectArray = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (CMTimebase[] inativeArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				iNativeObjectArray,
				"global::ObjCRuntime.NativeHandle inativeArray",
			];


			var stringArray = @"
using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (string [] stringArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				stringArray,
				"global::ObjCRuntime.NativeHandle stringArray",
			];

			var stringParameter = @"
using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (string stringParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				stringParameter,
				"global::ObjCRuntime.NativeHandle stringParameter",
			];

			var protocolParameter = @"
using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (INSUrlConnectionDataDelegate protocolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				protocolParameter,
				"global::ObjCRuntime.NativeHandle protocolParameter",
			];


			var forcedParameterOwnsFalse = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback ([ForcedType (false)]INSUrlConnectionDataDelegate forcedParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				forcedParameterOwnsFalse,
				"global::ObjCRuntime.NativeHandle forcedParameter",
			];

			var nsObjectParameter = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (NSObject nsObjectParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nsObjectParameter,
				"global::ObjCRuntime.NativeHandle nsObjectParameter",
			];

			var iNativeParameter = @"
using System;
using CoreMedia;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (CMTimebase inativeParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				iNativeParameter,
				"global::ObjCRuntime.NativeHandle inativeParameter",
			];

			var cmSampleBuffer = @"
using System;
using CoreMedia;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (CMSampleBuffer cmSampleBuffer);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				cmSampleBuffer,
				"global::ObjCRuntime.NativeHandle cmSampleBuffer",
			];

			var audioBuffer = @"
using System;
using AudioToolbox;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (AudioBuffers audioBuffer);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				audioBuffer,
				"global::ObjCRuntime.NativeHandle audioBuffer",
			];

			var outNullableInt = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (out int? outNullableInt);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				outNullableInt,
				"int* outNullableInt",
			];

			var outBoolean = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (out bool outBool);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				outBoolean,
				"byte* outBool",
			];

			var outNSObject = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (out NSObject outNSObject);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				outNSObject,
				"global::ObjCRuntime.NativeHandle* outNSObject",
			];

			var valueType = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (int valueType);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				valueType,
				"int valueType",
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataGetTrampolineInvokeParameter>]
	void GetTrampolineInvokeParameterTests (ApplePlatform platform, string inputText, string expectedExpression)
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
		// assert it is indeed a delegate
		Assert.NotNull (parameter.Type.Delegate);
		var parameterInfo = GetTrampolineInvokeParameter (parameter.Type.Delegate!.Parameters [0]);
		var expression = Parameter (parameterInfo.ParameterName)
			.WithType (parameterInfo.ParameterType)
			.NormalizeWhitespace ();
		Assert.Equal (expectedExpression, expression.ToString ());
	}

	class TestDataGetTrampolineInvokeDeclaration : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			var pointerParameter = @"
using System;
namespace NS {
	public delegate void Callback (int* pointerParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				pointerParameter,
				"internal static unsafe void Invoke (global::System.IntPtr block_ptr, int* pointerParameter)",
			];

			var ccallbackParameter = @"
using System;
using ObjCRuntime;
namespace NS {
	public delegate void Callback ([CCallback] Action callbackParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				ccallbackParameter,
				"internal static unsafe void Invoke (global::System.IntPtr block_ptr, global::System.IntPtr callbackParameter)",
			];

			var nativeEnumParameter = @"
using System;
using ObjCBindings;
using ObjCRuntime;
namespace NS {
        [Native (""""GKErrorCode"""")]
        [BindingType<SmartEnum> (Flags = SmartEnum.ErrorCode, ErrorDomain = """"GKErrorDomain"""")]
        public enum NativeSampleEnum : long {
                None = 0,
                Unknown = 1,
        }
        public delegate void Callback (NativeSampleEnum enumParameter);
        public class MyClass {
                public void MyMethod (Callback cb) {}
        }
}
";

			yield return [
				nativeEnumParameter,
				$"internal static unsafe void Invoke ({Global ("System.IntPtr")} block_ptr, {Global ("System.IntPtr")} enumParameter)",
			];

			var boolParameter = @"
using System;
using ObjCRuntime;
namespace NS {
	public delegate void Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				boolParameter,
				"internal static unsafe void Invoke (global::System.IntPtr block_ptr, byte boolParameter)",
			];

			var nsObjectArray = @"
using System;
using Foundation;
using ObjCRuntime;
namespace NS {
	public delegate void Callback (NSObject[] nsObjectArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nsObjectArray,
				"internal static unsafe void Invoke (global::System.IntPtr block_ptr, global::ObjCRuntime.NativeHandle nsObjectArray)",
			];

			var iNativeObjectArray = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;
namespace NS {
	public delegate void Callback (CMTimebase[] inativeArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				iNativeObjectArray,
				"internal static unsafe void Invoke (global::System.IntPtr block_ptr, global::ObjCRuntime.NativeHandle inativeArray)"
			];


			var stringArray = @"
using System;
using Foundation;
using ObjCRuntime;
namespace NS {
	public delegate void Callback (string [] stringArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				stringArray,
				"internal static unsafe void Invoke (global::System.IntPtr block_ptr, global::ObjCRuntime.NativeHandle stringArray)",
			];

			var protocolParameter = @"
using System;
using Foundation;
using ObjCRuntime;
namespace NS {
	public delegate void Callback (INSUrlConnectionDataDelegate protocolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				protocolParameter,
				"internal static unsafe void Invoke (global::System.IntPtr block_ptr, global::ObjCRuntime.NativeHandle protocolParameter)"
			];

			var outNullableInt = @"
using System;
using Foundation;
using ObjCBindings;
namespace NS {
	public delegate void Callback (out int? outNullableInt);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				outNullableInt,
				"internal static unsafe void Invoke (global::System.IntPtr block_ptr, int* outNullableInt)"
			];

			var outInt = @"
using System;
using Foundation;
using ObjCBindings;
namespace NS {
	public delegate void Callback (out int? outNullableInt);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				outInt,
				"internal static unsafe void Invoke (global::System.IntPtr block_ptr, int* outNullableInt)"
			];

			var outBoolean = @"
using System;
using Foundation;
using ObjCBindings;
namespace NS {
	public delegate void Callback (out bool outBool);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				outBoolean,
				"internal static unsafe void Invoke (global::System.IntPtr block_ptr, byte* outBool)",
			];


			var outNSObject = @"
using System;
using Foundation;
using ObjCBindings;
namespace NS {
	public delegate void Callback (ref NSObject outNSObject);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				outNSObject,
				"internal static unsafe void Invoke (global::System.IntPtr block_ptr, global::ObjCRuntime.NativeHandle* outNSObject)"
			];

			var valueReturnboolParameter = @"
using System;
using ObjCRuntime;
namespace NS {
	public delegate int Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				valueReturnboolParameter,
				"internal static unsafe int Invoke (global::System.IntPtr block_ptr, byte boolParameter)",
			];

			var nsObjectReturnBoolParameter = @"
using System;
using Foundation;
using ObjCRuntime;
namespace NS {
	public delegate NSObject Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nsObjectReturnBoolParameter,
				"internal static unsafe global::ObjCRuntime.NativeHandle Invoke (global::System.IntPtr block_ptr, byte boolParameter)",
			];

			var nullableNSObjectReturnBoolParameter = @"
using System;
using Foundation;
using ObjCRuntime;
namespace NS {
	public delegate NSObject? Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nullableNSObjectReturnBoolParameter,
				"internal static unsafe global::ObjCRuntime.NativeHandle Invoke (global::System.IntPtr block_ptr, byte boolParameter)",
			];

			var arrayNSObjectReturnBoolParameter = @"
using System;
using Foundation;
using ObjCRuntime;
namespace NS {
	public delegate NSObject[] Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				arrayNSObjectReturnBoolParameter,
				"internal static unsafe global::ObjCRuntime.NativeHandle Invoke (global::System.IntPtr block_ptr, byte boolParameter)",
			];

			var nullableArrayNSObjectReturnBoolParameter = @"
using System;
using Foundation;
using ObjCRuntime;
namespace NS {
	public delegate NSObject[]? Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nullableArrayNSObjectReturnBoolParameter,
				"internal static unsafe global::ObjCRuntime.NativeHandle Invoke (global::System.IntPtr block_ptr, byte boolParameter)",
			];

			var blockNamedParameter = @"
using System;
using Foundation;
using ObjCRuntime;
namespace NS {
	public delegate NSObject[]? Callback (bool block_ptr);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				blockNamedParameter,
				"internal static unsafe global::ObjCRuntime.NativeHandle Invoke (global::System.IntPtr block_ptr_0, byte block_ptr)",
			];

			var doubleBlockNamedParameter = @"
using System;
using Foundation;
using ObjCRuntime;
namespace NS {
	public delegate NSObject[]? Callback (bool block_ptr, bool block_ptr_0);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				doubleBlockNamedParameter,
				"internal static unsafe global::ObjCRuntime.NativeHandle Invoke (global::System.IntPtr block_ptr_1, byte block_ptr, byte block_ptr_0)",
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataGetTrampolineInvokeDeclaration>]
	void GetTrampolineInvokeDeclarationTests (ApplePlatform platform, string inputText, string expectedExpression)
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
		// assert it is indeed a delegate
		Assert.NotNull (parameter.Type.Delegate);
		var delegateDeclaration = GetTrampolineInvokeSignature (parameter.Type);
		Assert.Equal (expectedExpression, delegateDeclaration.ToString ());
	}

	class TestDataGetTrampolineDelegatePointer : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			var pointerParameter = @"
using System;
namespace NS {
	public delegate void Callback (int* pointerParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				pointerParameter,
				"delegate* unmanaged<global::System.IntPtr, int*, void> trampoline = &Invoke;",
			];

			var ccallbackParameter = @"
using System;
using ObjCRuntime;
namespace NS {
	public delegate void Callback ([CCallback] Action callbackParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				ccallbackParameter,
				"delegate* unmanaged<global::System.IntPtr, global::System.IntPtr, void> trampoline = &Invoke;",
			];

			var nativeEnumParameter = @"
using System;
using ObjCBindings;
using ObjCRuntime;
namespace NS {
        [Native (""""GKErrorCode"""")]
        [BindingType<SmartEnum> (Flags = SmartEnum.ErrorCode, ErrorDomain = """"GKErrorDomain"""")]
        public enum NativeSampleEnum : long {
                None = 0,
                Unknown = 1,
        }
        public delegate void Callback (NativeSampleEnum enumParameter);
        public class MyClass {
                public void MyMethod (Callback cb) {}
        }
}
";

			yield return [
				nativeEnumParameter,
				$"delegate* unmanaged<global::System.IntPtr, {Global ("System.IntPtr")}, void> trampoline = &Invoke;",
			];

			var boolParameter = @"
using System;
using ObjCRuntime;
namespace NS {
	public delegate void Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				boolParameter,
				"delegate* unmanaged<global::System.IntPtr, byte, void> trampoline = &Invoke;",
			];

			var nsObjectArray = @"
using System;
using Foundation;
using ObjCRuntime;
namespace NS {
	public delegate void Callback (NSObject[] nsObjectArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nsObjectArray,
				"delegate* unmanaged<global::System.IntPtr, global::ObjCRuntime.NativeHandle, void> trampoline = &Invoke;",
			];

			var iNativeObjectArray = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;
namespace NS {
	public delegate void Callback (CMTimebase[] inativeArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				iNativeObjectArray,
				"delegate* unmanaged<global::System.IntPtr, global::ObjCRuntime.NativeHandle, void> trampoline = &Invoke;",
			];

			var stringArray = @"
using System;
using Foundation;
using ObjCRuntime;
namespace NS {
	public delegate void Callback (string [] stringArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				stringArray,
				"delegate* unmanaged<global::System.IntPtr, global::ObjCRuntime.NativeHandle, void> trampoline = &Invoke;",
			];

			var protocolParameter = @"
using System;
using Foundation;
using ObjCRuntime;
namespace NS {
	public delegate void Callback (INSUrlConnectionDataDelegate protocolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				protocolParameter,
				"delegate* unmanaged<global::System.IntPtr, global::ObjCRuntime.NativeHandle, void> trampoline = &Invoke;",
			];
			var outNullableInt = @"
using System;
using Foundation;
using ObjCBindings;
namespace NS {
	public delegate void Callback (out int? outNullableInt);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				outNullableInt,
				"delegate* unmanaged<global::System.IntPtr, int*, void> trampoline = &Invoke;",
			];

			var outInt = @"
using System;
using Foundation;
using ObjCBindings;
namespace NS {
	public delegate void Callback (out int outNullableInt);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				outInt,
				"delegate* unmanaged<global::System.IntPtr, int*, void> trampoline = &Invoke;",
			];

			var outBoolean = @"
using System;
using Foundation;
using ObjCBindings;
namespace NS {
	public delegate void Callback (out bool outBool);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				outBoolean,
				"delegate* unmanaged<global::System.IntPtr, byte*, void> trampoline = &Invoke;",
			];

			var outNSObject = @"
using System;
using Foundation;
using ObjCBindings;
namespace NS {
	public delegate void Callback (ref NSObject outNSObject);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				outNSObject,
				"delegate* unmanaged<global::System.IntPtr, global::ObjCRuntime.NativeHandle*, void> trampoline = &Invoke;",
			];

			var valueReturnboolParameter = @"
using System;
using ObjCRuntime;
namespace NS {
	public delegate int Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				valueReturnboolParameter,
				"delegate* unmanaged<global::System.IntPtr, byte, int> trampoline = &Invoke;",
			];

			var nsObjectReturnBoolParameter = @"
using System;
using Foundation;
using ObjCRuntime;
namespace NS {
	public delegate NSObject Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nsObjectReturnBoolParameter,
				"delegate* unmanaged<global::System.IntPtr, byte, global::ObjCRuntime.NativeHandle> trampoline = &Invoke;",
			];

			var nullableNSObjectReturnBoolParameter = @"
using System;
using Foundation;
using ObjCRuntime;
namespace NS {
	public delegate NSObject? Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nullableNSObjectReturnBoolParameter,
				"delegate* unmanaged<global::System.IntPtr, byte, global::ObjCRuntime.NativeHandle> trampoline = &Invoke;",
			];

			var arrayNSObjectReturnBoolParameter = @"
using System;
using Foundation;
using ObjCRuntime;
namespace NS {
	public delegate NSObject[] Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				arrayNSObjectReturnBoolParameter,
				"delegate* unmanaged<global::System.IntPtr, byte, global::ObjCRuntime.NativeHandle> trampoline = &Invoke;",
			];

			var nullableArrayNSObjectReturnBoolParameter = @"
using System;
using Foundation;
using ObjCRuntime;
namespace NS {
	public delegate NSObject[]? Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nullableArrayNSObjectReturnBoolParameter,
				"delegate* unmanaged<global::System.IntPtr, byte, global::ObjCRuntime.NativeHandle> trampoline = &Invoke;",
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataGetTrampolineDelegatePointer>]
	void GetTrampolineDelegatePointerTests (ApplePlatform platform, string inputText, string expectedExpression)
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
		// assert it is indeed a delegate
		Assert.NotNull (parameter.Type.Delegate);
		var delegateDeclaration = GetTrampolineDelegatePointer (parameter.Type);
		Assert.Equal (expectedExpression, delegateDeclaration.ToString ());
	}

	class TestDataGetTrampolineInvokeArgumentInitializations : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			var outNullableInt = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (out int? outNullableInt);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				outNullableInt,
				"*outNullableInt = default;\n",
			];

			var outBoolean = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (out bool outBool);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				outBoolean,
				"*outBool = default;\n",
			];

			var outNSObject = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback (out NSObject outNSObject);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				outNSObject,
				"*outNSObject = default;\n",
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataGetTrampolineInvokeArgumentInitializations>]
	void GetTrampolineInvokeArgumentInitializationsTests (ApplePlatform platform, string trampolineName, string inputText, string expectedExpression)
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
		// assert it is indeed a delegate
		Assert.NotNull (parameter.Type.Delegate);
		var conversions = GetTrampolineInvokeArgumentInitializations (trampolineName, parameter.Type.Delegate!.Parameters [0]);
		// uses a tabbeb string builder to get the conversion string and test
		var sb = new TabbedStringBuilder (new ());
		sb.Write (conversions);
		Assert.Equal (expectedExpression, sb.ToCode ());
	}

	class TestDataGetTrampolineNativeInvokeSignature : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			var pointerParameter = @"
using System;
namespace NS {
	public delegate void Callback (int* pointerParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				pointerParameter,
				"unsafe void Invoke (int* pointerParameter)",
			];

			var ccallbackParameter = @"
using System;
using ObjCRuntime;
namespace NS {
	public delegate void Callback ([CCallback] Action callbackParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				ccallbackParameter,
				$"unsafe void Invoke ({Global ("System.Action")} callbackParameter)",
			];

			var nativeEnumParameter = @"
using System;
using ObjCBindings;
using ObjCRuntime;
namespace NS {
        [Native (""""GKErrorCode"""")]
        [BindingType<SmartEnum> (Flags = SmartEnum.ErrorCode, ErrorDomain = """"GKErrorDomain"""")]
        public enum NativeSampleEnum : long {
                None = 0,
                Unknown = 1,
        }
        public delegate void Callback (NativeSampleEnum enumParameter);
        public class MyClass {
                public void MyMethod (Callback cb) {}
        }
}
";

			yield return [
				nativeEnumParameter,
				$"unsafe void Invoke ({Global ("NS.NativeSampleEnum")} enumParameter)",
			];

			var boolParameter = @"
using System;
using ObjCRuntime;
namespace NS {
	public delegate void Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				boolParameter,
				"unsafe void Invoke (bool boolParameter)",
			];

			var nsObjectArray = @"
using System;
using Foundation;
using ObjCRuntime;
namespace NS {
	public delegate void Callback (NSObject[] nsObjectArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nsObjectArray,
				$"unsafe void Invoke ({Global ("Foundation.NSObject")}[] nsObjectArray)",
			];

			var iNativeObjectArray = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;
namespace NS {
	public delegate void Callback (CMTimebase[] inativeArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				iNativeObjectArray,
				$"unsafe void Invoke ({Global ("CoreMedia.CMTimebase")}[] inativeArray)"
			];

			var stringArray = @"
using System;
using Foundation;
using ObjCRuntime;
namespace NS {
	public delegate void Callback (string [] stringArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				stringArray,
				"unsafe void Invoke (string[] stringArray)",
			];

			var protocolParameter = @"
using System;
using Foundation;
using ObjCRuntime;
namespace NS {
	public delegate void Callback (INSUrlConnectionDataDelegate protocolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				protocolParameter,
				$"unsafe void Invoke ({Global ("Foundation.INSUrlConnectionDataDelegate")} protocolParameter)"
			];

			var outNullableInt = @"
using System;
using Foundation;
using ObjCBindings;
namespace NS {
	public delegate void Callback (out int? outNullableInt);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				outNullableInt,
				"unsafe void Invoke (out int? outNullableInt)"
			];

			var outInt = @"
using System;
using Foundation;
using ObjCBindings;
namespace NS {
	public delegate void Callback (out int outNullableInt);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				outInt,
				"unsafe void Invoke (out int outNullableInt)"
			];

			var outBoolean = @"
using System;
using Foundation;
using ObjCBindings;
namespace NS {
	public delegate void Callback (out bool outBool);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				outBoolean,
				"unsafe void Invoke (out bool outBool)",
			];


			var outNSObject = @"
using System;
using Foundation;
using ObjCBindings;
namespace NS {
	public delegate void Callback (ref NSObject outNSObject);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				outNSObject,
				$"unsafe void Invoke (ref {Global ("Foundation.NSObject")} outNSObject)"
			];

			var valueReturnboolParameter = @"
using System;
using ObjCRuntime;
namespace NS {
	public delegate int Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				valueReturnboolParameter,
				"unsafe int Invoke (bool boolParameter)",
			];

			var nsObjectReturnBoolParameter = @"
using System;
using Foundation;
using ObjCRuntime;
namespace NS {
	public delegate NSObject Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nsObjectReturnBoolParameter,
				$"unsafe {Global ("Foundation.NSObject")} Invoke (bool boolParameter)",
			];

			var nullableNSObjectReturnBoolParameter = @"
using System;
using Foundation;
using ObjCRuntime;
namespace NS {
	public delegate NSObject? Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nullableNSObjectReturnBoolParameter,
				$"unsafe {Global ("Foundation.NSObject")}? Invoke (bool boolParameter)",
			];

			var arrayNSObjectReturnBoolParameter = @"
using System;
using Foundation;
using ObjCRuntime;
namespace NS {
	public delegate NSObject[] Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				arrayNSObjectReturnBoolParameter,
				$"unsafe {Global ("Foundation.NSObject")}[] Invoke (bool boolParameter)",
			];

			var nullableArrayNSObjectReturnBoolParameter = @"
using System;
using Foundation;
using ObjCRuntime;
namespace NS {
	public delegate NSObject[]? Callback (bool boolParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nullableArrayNSObjectReturnBoolParameter,
				$"unsafe {Global ("Foundation.NSObject")}[]? Invoke (bool boolParameter)",
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataGetTrampolineNativeInvokeSignature>]
	void GetTrampolineNativeInvokeSignatureTests (ApplePlatform platform, string inputText, string expectedExpression)
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
		// assert it is indeed a delegate
		Assert.NotNull (parameter.Type.Delegate);
		var delegateDeclaration = GetTrampolineNativeInvokeSignature (parameter.Type);
		Assert.Equal (expectedExpression, delegateDeclaration.ToString ());
	}

}
