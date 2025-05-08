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
				"global::System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<System.Action> (callbackParameter)"
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
				"(IntPtr) (long) enumParameter",
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
				"boolParameter ? (byte) 1 : (byte) 0",
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
				"global::CoreFoundation.CFArray.ArrayFromHandle<Foundation.NSObject> (nsObjectArray)!",
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
				"global::Foundation.NSArray.ArrayFromHandle<CoreMedia.CMTimebase> (inativeArray)!",
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
				"global::ObjCRuntime.Runtime.GetINativeObject<Foundation.INSUrlConnectionDataDelegate> (protocolParameter, false)!",
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
				"global::ObjCRuntime.Runtime.GetINativeObject<Foundation.INSUrlConnectionDataDelegate> (forcedParameter, true, false)!",
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
				"global::ObjCRuntime.Runtime.GetINativeObject<Foundation.INSUrlConnectionDataDelegate> (forcedParameter, true, true)!",
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
				"global::ObjCRuntime.Runtime.GetNSObject<Foundation.NSObject> (nsObjectParameter)!",
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
				"global::ObjCRuntime.Runtime.GetINativeObject<CoreMedia.CMTimebase> (inativeParameter, false)!",
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
				"new AudioToolbox.AudioBuffers (audioBuffer)",
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
				"__xamarin_nullified__0",
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
				"__xamarin_bool__0",
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
				"__xamarin_pref0",
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

}
