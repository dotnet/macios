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
				$"{Global ("ObjCRuntime.Runtime")}.RetainAndAutoreleaseNSObject ({Global ("Foundation.NSArray")}.FromNSObjects (auxVariable))"
			];

			const string nullableArrayNSObjectResult = @"
using System;
using Foundation;

namespace NS {

	public delegate NSString []? Callback ();
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nullableArrayNSObjectResult,
				$"{Global ("ObjCRuntime.Runtime")}.RetainAndAutoreleaseNSObject ({Global ("Foundation.NSArray")}.FromNSObjects (auxVariable))"
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
				$"{Global ("ObjCRuntime.Runtime")}.RetainAndAutoreleaseNSObject (auxVariable)"
			];

			const string nullableNSObjectResult = @"
using System;
using Foundation;

namespace NS {

	public delegate NSString? Callback ();
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nullableNSObjectResult,
				$"{Global ("ObjCRuntime.Runtime")}.RetainAndAutoreleaseNSObject (auxVariable)"
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
				$"{Global ("ObjCRuntime.Runtime")}.RetainAndAutoreleaseNativeObject (auxVariable)"
			];

			const string nullableNativeObjectResult = @"
using System;
using Foundation;
using Security;

namespace NS {

	public delegate SecKeyChain? Callback ();
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nullableNativeObjectResult,
				$"{Global ("ObjCRuntime.Runtime")}.RetainAndAutoreleaseNativeObject (auxVariable)"
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
				$"{Global ("ObjCRuntime.Runtime")}.RetainAndAutoreleaseNSObject (auxVariable)"
			];

			const string nullableProtocolResult = @"
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
				nullableProtocolResult,
				$"{Global ("ObjCRuntime.Runtime")}.RetainAndAutoreleaseNSObject (auxVariable)"
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
				$"{Global ("Foundation.NSString")}.CreateNative (auxVariable, true)"
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
				$"{Global ("Foundation.NSString")}.CreateNative (auxVariable, true)"
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

			const string smartEnum = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {

	[BindingType<SmartEnum>]
	public enum MySmartEnum {

		[Field<EnumValue> (""AVCaptureDeviceTypeBuiltInMicrophone"")]
		BuiltInMicrophone,

		[Field<EnumValue> (""AVCaptureDeviceTypeBuiltInWideAngleCamera"")]
		BuiltInWideAngleCamera,

		[Field<EnumValue> (""AVCaptureDeviceTypeBuiltInTelephotoCamera"")]
		BuiltInTelephotoCamera,

		[Field<EnumValue> (""AVCaptureDeviceTypeBuiltInDuoCamera"")]
		BuiltInDuoCamera,

		[Field<EnumValue> (""AVCaptureDeviceTypeBuiltInDualCamera"")]
		BuiltInDualCamera,

		[Field<EnumValue> (""AVCaptureDeviceTypeBuiltInTrueDepthCamera"")]
		BuiltInTrueDepthCamera,

		[Field<EnumValue> (""AVCaptureDeviceTypeBuiltInUltraWideCamera"")]
		BuiltInUltraWideCamera,

		[Field<EnumValue> (""AVCaptureDeviceTypeBuiltInTripleCamera"")]
		BuiltInTripleCamera,

		[Field<EnumValue> (""AVCaptureDeviceTypeBuiltInDualWideCamera"")]
		BuiltInDualWideCamera,

		[Field<EnumValue> (""AVCaptureDeviceTypeExternalUnknown"")]
		ExternalUnknown,

		[Field<EnumValue> (""AVCaptureDeviceTypeBuiltInLiDARDepthCamera"")]
		BuiltInLiDarDepthCamera,
	}

	public delegate MySmartEnum Callback()
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				smartEnum,
				$"{Global ("ObjCRuntime.Runtime")}.RetainAndAutoreleaseNSObject (auxVariable)"
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

	class TestDataGetTrampolineNativeInvokeReturnType : IEnumerable<object []> {
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
				$"{Global ("CoreFoundation.CFArray")}.ArrayFromHandle<{Global ("Foundation.NSString")}> (auxVariable)!"
			];

			const string nullableArrayNSObjectResult = @"
using System;
using Foundation;

namespace NS {

	public delegate NSString []? Callback ();
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nullableArrayNSObjectResult,
				$"{Global ("CoreFoundation.CFArray")}.ArrayFromHandle<{Global ("Foundation.NSString")}> (auxVariable)"
			];

			const string arrayINativeResult = @"
using System;
using Foundation;
using Security;

namespace NS {

	public delegate SecKeyChain [] Callback ();
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				arrayINativeResult,
				$"{Global ("CoreFoundation.CFArray")}.ArrayFromHandle<{Global ("Security.SecKeyChain")}> (auxVariable)!"
			];

			const string nullableArrayINativeResult = @"
using System;
using Foundation;
using Security;

namespace NS {

	public delegate SecKeyChain []? Callback ();
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nullableArrayINativeResult,
				$"{Global ("CoreFoundation.CFArray")}.ArrayFromHandle<{Global ("Security.SecKeyChain")}> (auxVariable)"
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
				$"{Global ("ObjCRuntime.Runtime")}.GetNSObject<{Global ("Foundation.NSString")}> (auxVariable, false)!"
			];

			const string nullableNSObjectResult = @"
using System;
using Foundation;

namespace NS {

	public delegate NSString? Callback ();
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nullableNSObjectResult,
				$"{Global ("ObjCRuntime.Runtime")}.GetNSObject<{Global ("Foundation.NSString")}> (auxVariable, false)"
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
				$"{Global ("ObjCRuntime.Runtime")}.GetINativeObject<{Global ("Security.SecKeyChain")}> (auxVariable, false)!"
			];

			const string nullableNativeObjectResult = @"
using System;
using Foundation;
using Security;

namespace NS {

	public delegate SecKeyChain? Callback ();
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nullableNativeObjectResult,
				$"{Global ("ObjCRuntime.Runtime")}.GetINativeObject<{Global ("Security.SecKeyChain")}> (auxVariable, false)"
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
				$"{Global ("ObjCRuntime.Runtime")}.GetINativeObject<{Global ("Metal.IMTLTexture")}> (auxVariable, false)!"
			];

			const string nullableProtocolResult = @"
using System;
using Foundation;
using Metal;

namespace NS {

	public delegate IMTLTexture? Callback ();
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nullableProtocolResult,
				$"{Global ("ObjCRuntime.Runtime")}.GetINativeObject<{Global ("Metal.IMTLTexture")}> (auxVariable, false)"
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
				$"{Global ("CoreFoundation.CFString")}.FromHandle (auxVariable)!"
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
				$"{Global ("CoreFoundation.CFString")}.FromHandle (auxVariable)"
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
				"auxVariable != 0"
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
				"(global::NS.NativeSampleEnum) (long) auxVariable",
			];

			const string smartEnum = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {

	[BindingType<SmartEnum>]
	public enum MySmartEnum {

		[Field<EnumValue> (""AVCaptureDeviceTypeBuiltInMicrophone"")]
		BuiltInMicrophone,

		[Field<EnumValue> (""AVCaptureDeviceTypeBuiltInWideAngleCamera"")]
		BuiltInWideAngleCamera,

		[Field<EnumValue> (""AVCaptureDeviceTypeBuiltInTelephotoCamera"")]
		BuiltInTelephotoCamera,

		[Field<EnumValue> (""AVCaptureDeviceTypeBuiltInDuoCamera"")]
		BuiltInDuoCamera,

		[Field<EnumValue> (""AVCaptureDeviceTypeBuiltInDualCamera"")]
		BuiltInDualCamera,

		[Field<EnumValue> (""AVCaptureDeviceTypeBuiltInTrueDepthCamera"")]
		BuiltInTrueDepthCamera,

		[Field<EnumValue> (""AVCaptureDeviceTypeBuiltInUltraWideCamera"")]
		BuiltInUltraWideCamera,

		[Field<EnumValue> (""AVCaptureDeviceTypeBuiltInTripleCamera"")]
		BuiltInTripleCamera,

		[Field<EnumValue> (""AVCaptureDeviceTypeBuiltInDualWideCamera"")]
		BuiltInDualWideCamera,

		[Field<EnumValue> (""AVCaptureDeviceTypeExternalUnknown"")]
		ExternalUnknown,

		[Field<EnumValue> (""AVCaptureDeviceTypeBuiltInLiDARDepthCamera"")]
		BuiltInLiDarDepthCamera,
	}

	public delegate MySmartEnum Callback()
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				smartEnum,
				$"{Global ("NS.MySmartEnumExtensions")}.GetValue (auxVariable)",
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
				"(global::NS.NativeSampleEnum) (ulong) auxVariable",
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
	[AllSupportedPlatformsClassData<TestDataGetTrampolineNativeInvokeReturnType>]
	void GetTrampolineNativeInvokeReturnTypeTests (ApplePlatform platform, string inputText, string? expectedExpression)
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
		var expression = GetTrampolineNativeInvokeReturnType (parameter.Type, auxVariableName);
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
				$"{Global ("System.Runtime")}.InteropServices.Marshal.GetDelegateForFunctionPointer<{Global ("System.Action")}> (callbackParameter)"
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
				$"{Global ("CoreFoundation.CFArray")}.ArrayFromHandle<{Global ("Foundation.NSObject")}> (nsObjectArray)!",
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
				$"{Global ("Foundation.NSArray")}.ArrayFromHandle<{Global ("CoreMedia.CMTimebase")}> (inativeArray)!",
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
				$"{Global ("CoreFoundation.CFArray")}.StringArrayFromHandle (stringArray)!",
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
				$"{Global ("CoreFoundation.CFString")}.FromHandle (stringParameter)!",
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
				$"{Global ("ObjCRuntime.Runtime")}.GetINativeObject<{Global ("Foundation.INSUrlConnectionDataDelegate")}> (protocolParameter, false)!",
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
				$"{Global ("ObjCRuntime.Runtime")}.GetINativeObject<{Global ("Foundation.INSUrlConnectionDataDelegate")}> (forcedParameter, true, false)!",
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
				$"{Global ("ObjCRuntime.Runtime")}.GetINativeObject<{Global ("Foundation.INSUrlConnectionDataDelegate")}> (forcedParameter, true, true)!",
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
				$"{Global ("ObjCRuntime.Runtime")}.GetNSObject<{Global ("Foundation.NSObject")}> (nsObjectParameter)!",
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
				$"{Global ("ObjCRuntime.Runtime")}.GetNSObject<{Global ("Foundation.NSObject")}> (nsObjectParameter)",
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
				$"{Global ("ObjCRuntime.Runtime")}.GetINativeObject<{Global ("CoreMedia.CMTimebase")}> (inativeParameter, false)!",
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
				$"{Global ("ObjCRuntime.Runtime")}.GetINativeObject<{Global ("CoreMedia.CMTimebase")}> (inativeParameter, false)",
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
				$"cmSampleBuffer == IntPtr.Zero ? null! : new {Global ("CoreMedia.CMSampleBuffer")} (cmSampleBuffer, false)",
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
				$"unsafe internal delegate void DCallback ({Global ("System")}.IntPtr block_ptr, int* pointerParameter);",
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
				$"unsafe internal delegate void DCallback ({Global ("System")}.IntPtr block_ptr, {Global ("System")}.IntPtr callbackParameter);",
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
				$"unsafe internal delegate void DCallback ({Global ("System")}.IntPtr block_ptr, {Global ("System.IntPtr")} enumParameter);",
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
				$"unsafe internal delegate void DCallback ({Global ("System")}.IntPtr block_ptr, byte boolParameter);",
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
				$"unsafe internal delegate void DCallback ({Global ("System")}.IntPtr block_ptr, {Global ("ObjCRuntime")}.NativeHandle nsObjectArray);",
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
				$"unsafe internal delegate void DCallback ({Global ("System")}.IntPtr block_ptr, {Global ("ObjCRuntime")}.NativeHandle inativeArray);"
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
				$"unsafe internal delegate void DCallback ({Global ("System")}.IntPtr block_ptr, {Global ("ObjCRuntime")}.NativeHandle stringArray);",
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
				$"unsafe internal delegate void DCallback ({Global ("System")}.IntPtr block_ptr, {Global ("ObjCRuntime")}.NativeHandle protocolParameter);"
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
				$"unsafe internal delegate void DCallback ({Global ("System")}.IntPtr block_ptr, int* outNullableInt);"
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
				$"unsafe internal delegate void DCallback ({Global ("System")}.IntPtr block_ptr, int* outNullableInt);"
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
				$"unsafe internal delegate void DCallback ({Global ("System")}.IntPtr block_ptr, byte* outBool);",
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
				$"unsafe internal delegate void DCallback ({Global ("System")}.IntPtr block_ptr, {Global ("ObjCRuntime")}.NativeHandle* outNSObject);"
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
				$"unsafe internal delegate int DCallback ({Global ("System")}.IntPtr block_ptr, byte boolParameter);",
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
				$"unsafe internal delegate {Global ("ObjCRuntime")}.NativeHandle DCallback ({Global ("System")}.IntPtr block_ptr, byte boolParameter);",
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
				$"unsafe internal delegate {Global ("ObjCRuntime")}.NativeHandle DCallback ({Global ("System")}.IntPtr block_ptr, byte boolParameter);",
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
				$"unsafe internal delegate {Global ("ObjCRuntime")}.NativeHandle DCallback ({Global ("System")}.IntPtr block_ptr, byte boolParameter);",
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
				$"unsafe internal delegate {Global ("ObjCRuntime")}.NativeHandle DCallback ({Global ("System")}.IntPtr block_ptr, byte boolParameter);",
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
				$"unsafe internal delegate int DCallback ({Global ("System")}.IntPtr block_ptr, {Global ("System")}.IntPtr timestamp, uint frameCount, {Global ("System")}.IntPtr inputData);",
			];

			var nsNumberType = @"
using System;
using Foundation;
using ObjCBindings;
namespace NS {
	public delegate void Callback ([BindFrom (typeof(NSNumber))]int valueType);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nsNumberType,
				"DCallback",
				$"unsafe internal delegate void DCallback ({Global ("System.IntPtr")} block_ptr, {Global ("ObjCRuntime.NativeHandle")} valueType);",
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
				$"del ({Global ("System")}.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<{Global ("System.Action")}> (callbackParameter));",
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
				$"del ({Global ("System")}.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<{Global ("System.Action")}> (callbackParameter), NIDsomeTrampolineName.Create (callbackParameter)!);",
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
				$"var ret = del ({Global ("System")}.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<{Global ("System.Action")}> (callbackParameter), NIDsomeTrampolineName.Create (callbackParameter)!);",
			];

			var nsNumberParameterWithReturn = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate int Callback ([BindFrom (typeof(NSNumber))]int pointerParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				nsNumberParameterWithReturn,
				$"var ret = del ({Global ("ObjCRuntime.Runtime")}.GetNSObject<{Global ("Foundation.NSNumber")}> (pointerParameter)!.Int32Value);",
			];

			var nsNumberArrayParameterWithReturn = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate int Callback ([BindFrom (typeof(NSNumber))]int[] pointerParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				nsNumberArrayParameterWithReturn,
				$"var ret = del ({Global ("Foundation.NSArray")}.ArrayFromHandleFunc<int> (pointerParameter, {Global ("Foundation.NSNumber")}.ToInt32, false)!);"
			];

			var nsValueParameterWithReturn = @"
using System;
using Foundation;
using CoreGraphics;
using ObjCBindings;

namespace NS {
	public delegate int Callback ([BindFrom (typeof(NSValue))]CGSize size);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				nsValueParameterWithReturn,
				$"var ret = del ({Global ("ObjCRuntime.Runtime")}.GetNSObject<{Global ("Foundation.NSValue")}> (size)!.CGSizeValue);",
			];

			var nsValueArrayParameterWithReturn = @"
using System;
using Foundation;
using CoreGraphics;
using ObjCBindings;

namespace NS {
	public delegate int Callback ([BindFrom (typeof(NSValue))]CGSize[] size);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				nsValueArrayParameterWithReturn,
				$"var ret = del ({Global ("Foundation.NSArray")}.ArrayFromHandleFunc<{Global ("CoreGraphics.CGSize")}> (size, {Global ("Foundation.NSValue")}.ToCGSize, false)!);"
			];

			var smartEnumParameterWithReturn = @"
using System;
using Foundation;
using AVFoundation;
using ObjCBindings;

namespace NS {

	[BindingType<SmartEnum>]
	public enum CustomLibraryEnum {
		[Field<EnumValue> (""None"", ""/path/to/customlibrary.framework"")]
		None,
		[Field<EnumValue> (""Medium"", ""/path/to/customlibrary.framework"")]
		Medium,
		[Field<EnumValue> (""High"", ""/path/to/customlibrary.framework"")]
		High,
	}

	public delegate int Callback ([BindFrom (typeof(NSString))]CustomLibraryEnum level);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				smartEnumParameterWithReturn,
				$"var ret = del ({Global ("NS.CustomLibraryEnum")}.GetValue ({Global ("ObjCRuntime.Runtime")}.GetNSObject<{Global ("Foundation.NSString")}> (level)!));",
			];

			var smartEnumArrayParameterWithReturn = @"
using System;
using Foundation;
using AVFoundation;
using ObjCBindings;

namespace NS {

	[BindingType<SmartEnum>]
	public enum CustomLibraryEnum {
		[Field<EnumValue> (""None"", ""/path/to/customlibrary.framework"")]
		None,
		[Field<EnumValue> (""Medium"", ""/path/to/customlibrary.framework"")]
		Medium,
		[Field<EnumValue> (""High"", ""/path/to/customlibrary.framework"")]
		High,
	}

	public delegate int Callback ([BindFrom (typeof(NSString))]CustomLibraryEnum[] level);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				smartEnumArrayParameterWithReturn,
				$"var ret = del ({Global ("Foundation.NSArray")}.ArrayFromHandleFunc<{Global ("NS.CustomLibraryEnum")}> (level, {Global ("NS.CustomLibraryEnumExtensions")}.GetValue, false)!);"
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
		var x = invocation.ToFullString ();
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
				$"{Global ("System")}.IntPtr callbackParameter"
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
				$"{Global ("System")}.IntPtr callbackParameter",
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
				$"{Global ("ObjCRuntime")}.NativeHandle nsObjectArray",
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
				$"{Global ("ObjCRuntime")}.NativeHandle inativeArray",
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
				$"{Global ("ObjCRuntime")}.NativeHandle stringArray",
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
				$"{Global ("ObjCRuntime")}.NativeHandle stringParameter",
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
				$"{Global ("ObjCRuntime")}.NativeHandle protocolParameter",
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
				$"{Global ("ObjCRuntime")}.NativeHandle forcedParameter",
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
				$"{Global ("ObjCRuntime")}.NativeHandle nsObjectParameter",
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
				$"{Global ("ObjCRuntime")}.NativeHandle inativeParameter",
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
				$"{Global ("ObjCRuntime")}.NativeHandle cmSampleBuffer",
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
				$"{Global ("ObjCRuntime")}.NativeHandle audioBuffer",
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
				$"{Global ("ObjCRuntime")}.NativeHandle* outNSObject",
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

			var nsNumberType = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate void Callback ([BindFrom (typeof(NSNumber))]int valueType);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nsNumberType,
				$"{Global ("ObjCRuntime.NativeHandle")} valueType",
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
				$"internal static unsafe void Invoke ({Global ("System")}.IntPtr block_ptr, int* pointerParameter)",
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
				$"internal static unsafe void Invoke ({Global ("System")}.IntPtr block_ptr, {Global ("System")}.IntPtr callbackParameter)",
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
				$"internal static unsafe void Invoke ({Global ("System")}.IntPtr block_ptr, byte boolParameter)",
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
				$"internal static unsafe void Invoke ({Global ("System")}.IntPtr block_ptr, {Global ("ObjCRuntime")}.NativeHandle nsObjectArray)",
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
				$"internal static unsafe void Invoke ({Global ("System")}.IntPtr block_ptr, {Global ("ObjCRuntime")}.NativeHandle inativeArray)"
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
				$"internal static unsafe void Invoke ({Global ("System")}.IntPtr block_ptr, {Global ("ObjCRuntime")}.NativeHandle stringArray)",
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
				$"internal static unsafe void Invoke ({Global ("System")}.IntPtr block_ptr, {Global ("ObjCRuntime")}.NativeHandle protocolParameter)"
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
				$"internal static unsafe void Invoke ({Global ("System")}.IntPtr block_ptr, int* outNullableInt)"
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
				$"internal static unsafe void Invoke ({Global ("System")}.IntPtr block_ptr, int* outNullableInt)"
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
				$"internal static unsafe void Invoke ({Global ("System")}.IntPtr block_ptr, byte* outBool)",
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
				$"internal static unsafe void Invoke ({Global ("System")}.IntPtr block_ptr, {Global ("ObjCRuntime")}.NativeHandle* outNSObject)"
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
				$"internal static unsafe int Invoke ({Global ("System")}.IntPtr block_ptr, byte boolParameter)",
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
				$"internal static unsafe {Global ("ObjCRuntime")}.NativeHandle Invoke ({Global ("System")}.IntPtr block_ptr, byte boolParameter)",
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
				$"internal static unsafe {Global ("ObjCRuntime")}.NativeHandle Invoke ({Global ("System")}.IntPtr block_ptr, byte boolParameter)",
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
				$"internal static unsafe {Global ("ObjCRuntime")}.NativeHandle Invoke ({Global ("System")}.IntPtr block_ptr, byte boolParameter)",
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
				$"internal static unsafe {Global ("ObjCRuntime")}.NativeHandle Invoke ({Global ("System")}.IntPtr block_ptr, byte boolParameter)",
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
				$"internal static unsafe {Global ("ObjCRuntime")}.NativeHandle Invoke ({Global ("System")}.IntPtr block_ptr_0, byte block_ptr)",
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
				$"internal static unsafe {Global ("ObjCRuntime")}.NativeHandle Invoke ({Global ("System")}.IntPtr block_ptr_1, byte block_ptr, byte block_ptr_0)",
			];

			var nsNumberIntParameter = @"
using System;
using Foundation;
using ObjCBindings; 

namespace NS {
	public delegate void Callback ([BindFrom (typeof(NSNumber))] int pointerParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				nsNumberIntParameter,
				$"internal static unsafe void Invoke ({Global ("System.IntPtr")} block_ptr, {Global ("ObjCRuntime.NativeHandle")} pointerParameter)",
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
				$"delegate* unmanaged<{Global ("System")}.IntPtr, int*, void> trampoline = &Invoke;",
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
				$"delegate* unmanaged<{Global ("System")}.IntPtr, {Global ("System")}.IntPtr, void> trampoline = &Invoke;",
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
				$"delegate* unmanaged<{Global ("System")}.IntPtr, {Global ("System.IntPtr")}, void> trampoline = &Invoke;",
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
				$"delegate* unmanaged<{Global ("System")}.IntPtr, byte, void> trampoline = &Invoke;",
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
				$"delegate* unmanaged<{Global ("System")}.IntPtr, {Global ("ObjCRuntime")}.NativeHandle, void> trampoline = &Invoke;",
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
				$"delegate* unmanaged<{Global ("System")}.IntPtr, {Global ("ObjCRuntime")}.NativeHandle, void> trampoline = &Invoke;",
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
				$"delegate* unmanaged<{Global ("System")}.IntPtr, {Global ("ObjCRuntime")}.NativeHandle, void> trampoline = &Invoke;",
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
				$"delegate* unmanaged<{Global ("System")}.IntPtr, {Global ("ObjCRuntime")}.NativeHandle, void> trampoline = &Invoke;",
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
				$"delegate* unmanaged<{Global ("System")}.IntPtr, int*, void> trampoline = &Invoke;",
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
				$"delegate* unmanaged<{Global ("System")}.IntPtr, int*, void> trampoline = &Invoke;",
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
				$"delegate* unmanaged<{Global ("System")}.IntPtr, byte*, void> trampoline = &Invoke;",
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
				$"delegate* unmanaged<{Global ("System")}.IntPtr, {Global ("ObjCRuntime")}.NativeHandle*, void> trampoline = &Invoke;",
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
				$"delegate* unmanaged<{Global ("System")}.IntPtr, byte, int> trampoline = &Invoke;",
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
				$"delegate* unmanaged<{Global ("System")}.IntPtr, byte, {Global ("ObjCRuntime")}.NativeHandle> trampoline = &Invoke;",
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
				$"delegate* unmanaged<{Global ("System")}.IntPtr, byte, {Global ("ObjCRuntime")}.NativeHandle> trampoline = &Invoke;",
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
				$"delegate* unmanaged<{Global ("System")}.IntPtr, byte, {Global ("ObjCRuntime")}.NativeHandle> trampoline = &Invoke;",
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
				$"delegate* unmanaged<{Global ("System")}.IntPtr, byte, {Global ("ObjCRuntime")}.NativeHandle> trampoline = &Invoke;",
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

	class TestDataGetTrampolineNativeInvokeArgumentInitializations : IEnumerable<object []> {
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
				"outNullableInt = default;\n",
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
				"outBool = default;\n",
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
				"outNSObject = default;\n",
			];

			var refNSObject = @"
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
				"someTrampolineName",
				refNSObject,
				string.Empty,
			];

		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataGetTrampolineNativeInvokeArgumentInitializations>]
	void GetTrampolineInvokeArgumentNativeInitializationsTests (ApplePlatform platform, string trampolineName, string inputText, string expectedExpression)
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
		var conversions = GetTrampolineNativeInvokeArgumentInitializations (trampolineName, parameter.Type.Delegate!.Parameters [0]);
		// uses a tabbeb string builder to get the conversion string and test
		var sb = new TabbedStringBuilder (new ());
		sb.Write (conversions);
		Assert.Equal (expectedExpression, sb.ToCode ());
	}

	class TestDataGetTrampolinePreNativeInvokeArgumentConversions : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			// int value
			var intParameter = @"
using System;

namespace NS {
	public delegate void Callback (int intParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				intParameter,
				string.Empty,
			];

			// struct parameter
			var structParameter = @"
using System;

namespace NS {
	public struct MyStruct {
		public int Value;
	}

	public delegate void Callback (MyStruct structParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				structParameter,
				string.Empty,
			];

			var stringParameter = @"
using System;

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
				$@"if (stringParameter is null)
	{Global ("ObjCRuntime")}.ThrowHelper.ThrowArgumentNullException (nameof (stringParameter));
var nsstringParameter = {Global ("CoreFoundation")}.CFString.CreateNative (stringParameter);
",
			];

			var nullableStringParameter = @"
using System;

namespace NS {
	public delegate void Callback (string? stringParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				nullableStringParameter,
				$"var nsstringParameter = {Global ("CoreFoundation")}.CFString.CreateNative (stringParameter);\n",
			];

			var stringArrayParameter = @"
using System;

namespace NS {
	public delegate void Callback (string[] stringParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				stringArrayParameter,
				$@"if (stringParameter is null)
	{Global ("ObjCRuntime")}.ThrowHelper.ThrowArgumentNullException (nameof (stringParameter));
var nsa_stringParameter = {Global ("Foundation")}.NSArray.FromStrings (stringParameter);
",
			];

			var nullableStringArrayParameter = @"
using System;

namespace NS {
	public delegate void Callback (string[]? stringParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				nullableStringArrayParameter,
				$"var nsa_stringParameter = stringParameter is null ? null : {Global ("Foundation")}.NSArray.FromStrings (stringParameter);\n",
			];

			// smart enum parameter
			var smartEnumParameter = @"
using System;
using ObjCBindings;

namespace NS {

    [Native (""""GKErrorCode"""")]
	[BindingType<SmartEnum> (Flags = SmartEnum.ErrorCode, ErrorDomain = """"GKErrorDomain"""")]
	public enum NativeSampleEnum {
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
				smartEnumParameter,
				"var nsb_enumParameter = enumParameter.GetConstant ();\n"
			];


			// normal enum parameter
			var enumParameter = @"
using System;
using ObjCBindings;

namespace NS {

	public enum NativeSampleEnum {
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
				enumParameter,
				string.Empty,
			];


			// NSObject parameter

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
				"var nsObjectParameter__handle__ = nsObjectParameter!.GetNonNullHandle (nameof (nsObjectParameter));\n",
			];

			// nullable NSObject parameter

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
				"var nsObjectParameter__handle__ = nsObjectParameter?.GetHandle ();\n",
			];

			// NSObject array parameter

			var nsObjectArrayParameter = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {

    public delegate void Callback (NSObject[] nsObjectParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				nsObjectArrayParameter,
				$@"if (nsObjectParameter is null)
	{Global ("ObjCRuntime")}.ThrowHelper.ThrowArgumentNullException (nameof (nsObjectParameter));
var nsa_nsObjectParameter = {Global ("Foundation")}.NSArray.FromNSObjects (nsObjectParameter);
",
			];

			// nullable NSObject array parameter

			var nullableNSObjectArrayParameter = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {

    public delegate void Callback (NSObject[]? nsObjectParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				nullableNSObjectArrayParameter,
				$"var nsa_nsObjectParameter = nsObjectParameter is null ? null : {Global ("Foundation")}.NSArray.FromNSObjects (nsObjectParameter);\n",
			];

			// INativeObject parameter

			var iNativeParameter = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (CMTimebase inative);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				iNativeParameter,
				"var inative__handle__ = inative!.GetNonNullHandle (nameof (inative));\n"
			];

			// nullable INativeObject parameter

			var nullableINativeParameter = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (CMTimebase? inative);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				nullableINativeParameter,
				"var inative__handle__ = inative?.GetHandle ();\n",
			];

			// INativeObject array parameter

			var inativeArrayParameter = @"
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
				inativeArrayParameter,
				$@"if (inativeArray is null)
	{Global ("ObjCRuntime")}.ThrowHelper.ThrowArgumentNullException (nameof (inativeArray));
var nsa_inativeArray = {Global ("Foundation")}.NSArray.FromNSObjects (inativeArray);
",
			];

			// nullable INativeObject array parameter

			var nullableINativeArrayParameter = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (CMTimebase[]? inativeArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				nullableINativeArrayParameter,
				$"var nsa_inativeArray = inativeArray is null ? null : {Global ("Foundation")}.NSArray.FromNSObjects (inativeArray);\n"
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
				"var audioBuffer__handle__ = audioBuffer!.GetNonNullHandle (nameof (audioBuffer));\n",
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
				"var cmSampleBuffer__handle__ = cmSampleBuffer!.GetNonNullHandle (nameof (cmSampleBuffer));\n",
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataGetTrampolinePreNativeInvokeArgumentConversions>]
	void GetTrampolinePreNativeInvokeArgumentConversionsTests (ApplePlatform platform, string trampolineName, string inputText, string expectedExpression)
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
		var conversions = GetTrampolinePreNativeInvokeArgumentConversions (trampolineName, parameter.Type.Delegate!.Parameters [0]);
		// uses a tabbeb string builder to get the conversion string and test
		var sb = new TabbedStringBuilder (new ());
		sb.Write (conversions, false);
		Assert.Equal (expectedExpression, sb.ToCode ());
	}

	class TestDataGetTrampolinePostNativeInvokeArgumentConversions : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			// int value
			var intParameter = @"
using System;

namespace NS {
	public delegate void Callback (int intParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				intParameter,
				string.Empty,
			];

			// struct parameter
			var structParameter = @"
using System;

namespace NS {
	public struct MyStruct {
		public int Value;
	}

	public delegate void Callback (MyStruct structParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				structParameter,
				string.Empty,
			];

			var stringParameter = @"
using System;

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
				$"{Global ("CoreFoundation.CFString")}.ReleaseNative (nsstringParameter);\n"
			];

			var nullableStringParameter = @"
using System;

namespace NS {
	public delegate void Callback (string? stringParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				nullableStringParameter,
				$"{Global ("CoreFoundation.CFString")}.ReleaseNative (nsstringParameter);\n"
			];

			var stringArrayParameter = @"
using System;

namespace NS {
	public delegate void Callback (string[] stringParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				stringArrayParameter,
				$"{Global ("System.GC")}.KeepAlive (nsa_stringParameter);\n"
			];

			var nullableStringArrayParameter = @"
using System;

namespace NS {
	public delegate void Callback (string[]? stringParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				nullableStringArrayParameter,
				$"{Global ("System.GC")}.KeepAlive (nsa_stringParameter);\n"
			];

			// smart enum parameter
			var smartEnumParameter = @"
using System;
using ObjCBindings;

namespace NS {

    [Native (""""GKErrorCode"""")]
	[BindingType<SmartEnum> (Flags = SmartEnum.ErrorCode, ErrorDomain = """"GKErrorDomain"""")]
	public enum NativeSampleEnum {
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
				smartEnumParameter,
				$"{Global ("System.GC")}.KeepAlive (nsb_enumParameter);\n"
			];

			// normal enum parameter
			var enumParameter = @"
	using System;
	using ObjCBindings;

	namespace NS {

		public enum NativeSampleEnum {
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
				enumParameter,
				string.Empty,
			];

			// NSObject parameter

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
				$"{Global ("System.GC")}.KeepAlive (nsObjectParameter);\n"
			];

			// nullable NSObject parameter

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
				$"{Global ("System.GC")}.KeepAlive (nsObjectParameter);\n"
			];

			// NSObject array parameter

			var nsObjectArrayParameter = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {

    public delegate void Callback (NSObject[] nsObjectParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				nsObjectArrayParameter,
				$"{Global ("System.GC")}.KeepAlive (nsa_nsObjectParameter);\n"
			];

			// nullable NSObject array parameter

			var nullableNSObjectArrayParameter = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {

    public delegate void Callback (NSObject[]? nsObjectParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				nullableNSObjectArrayParameter,
				$"{Global ("System.GC")}.KeepAlive (nsa_nsObjectParameter);\n"
			];

			// INativeObject parameter

			var iNativeParameter = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (CMTimebase inative);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				iNativeParameter,
				$"{Global ("System.GC")}.KeepAlive (inative);\n"
			];

			// nullable INativeObject parameter

			var nullableINativeParameter = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (CMTimebase? inative);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				nullableINativeParameter,
				$"{Global ("System.GC")}.KeepAlive (inative);\n"
			];

			// INativeObject array parameter

			var inativeArrayParameter = @"
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
				inativeArrayParameter,
				$"{Global ("System.GC")}.KeepAlive (nsa_inativeArray);\n"
			];

			// nullable INativeObject array parameter

			var nullableINativeArrayParameter = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (CMTimebase[]? inativeArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				nullableINativeArrayParameter,
				$"{Global ("System.GC")}.KeepAlive (nsa_inativeArray);\n"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataGetTrampolinePostNativeInvokeArgumentConversions>]
	void GetTrampolinePostNativeInvokeArgumentConversionsTests (ApplePlatform platform, string trampolineName, string inputText, string expectedExpression)
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
		var conversions = GetTrampolinePostNativeInvokeArgumentConversions (trampolineName, parameter.Type.Delegate!.Parameters [0]);
		// uses a tabbeb string builder to get the conversion string and test
		var sb = new TabbedStringBuilder (new ());
		sb.Write (conversions, false);
		Assert.Equal (expectedExpression, sb.ToCode ());
	}

	class TestDataGetTrampolineNativeInvokeArguments : IEnumerable<object []> {
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
				"invoker (BlockLiteral, pointerParameter);",
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
				"var ret = invoker (BlockLiteral, pointerParameter);",
			];

			var nsNumberParameterWithReturn = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate int Callback ([BindFrom (typeof(NSNumber))]int pointerParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				nsNumberParameterWithReturn,
				"var ret = invoker (BlockLiteral, pointerParameter__handle__);",
			];

			var nsNumberArrayParameterWithReturn = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public delegate int Callback ([BindFrom (typeof(NSNumber))]int[] pointerParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				nsNumberArrayParameterWithReturn,
				"var ret = invoker (BlockLiteral, nsa_pointerParameter);",
			];

			var nsValueParameterWithReturn = @"
using System;
using Foundation;
using CoreGraphics;
using ObjCBindings;

namespace NS {
	public delegate int Callback ([BindFrom (typeof(NSValue))]CGSize size);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				nsValueParameterWithReturn,
				"var ret = invoker (BlockLiteral, size__handle__);"
			];

			var nsValueArrayParameterWithReturn = @"
using System;
using Foundation;
using CoreGraphics;
using ObjCBindings;

namespace NS {
	public delegate int Callback ([BindFrom (typeof(NSValue))]CGSize[] size);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				nsValueArrayParameterWithReturn,
				"var ret = invoker (BlockLiteral, nsa_size);",
			];

			var smartEnumParameterWithReturn = @"
using System;
using Foundation;
using AVFoundation;
using ObjCBindings;

namespace NS {

	[BindingType<SmartEnum>]
	public enum CustomLibraryEnum {
		[Field<EnumValue> (""None"", ""/path/to/customlibrary.framework"")]
		None,
		[Field<EnumValue> (""Medium"", ""/path/to/customlibrary.framework"")]
		Medium,
		[Field<EnumValue> (""High"", ""/path/to/customlibrary.framework"")]
		High,
	}

	public delegate int Callback ([BindFrom (typeof(NSString))]CustomLibraryEnum level);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				smartEnumParameterWithReturn,
				"var ret = invoker (BlockLiteral, nslevel);"
			];

			var smartEnumArrayParameterWithReturn = @"
using System;
using Foundation;
using AVFoundation;
using ObjCBindings;

namespace NS {

	[BindingType<SmartEnum>]
	public enum CustomLibraryEnum {
		[Field<EnumValue> (""None"", ""/path/to/customlibrary.framework"")]
		None,
		[Field<EnumValue> (""Medium"", ""/path/to/customlibrary.framework"")]
		Medium,
		[Field<EnumValue> (""High"", ""/path/to/customlibrary.framework"")]
		High,
	}

	public delegate int Callback ([BindFrom (typeof(NSString))]CustomLibraryEnum[] level);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				smartEnumArrayParameterWithReturn,
				"var ret = invoker (BlockLiteral, nsa_level);"
			];

			var refParameter = @"
using System;

namespace NS {
	public delegate int Callback (ref int pointerParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				"someTrampolineName",
				refParameter,
				$"var ret = invoker (BlockLiteral, (int*) {Global ("System.Runtime")}.CompilerServices.Unsafe.AsPointer<int> (ref pointerParameter));",
			];

			var refEnumParameter = @"
using System;
using AVFoundation;

namespace NS {
	public class MyClass {
		public void MyMethod (AVAudioConverterInputHandler cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				refEnumParameter,
				$"var ret = invoker (BlockLiteral, inNumberOfPackets, ({Global ("AVFoundation.AVAudioConverterInputStatus")}*) {Global ("System.Runtime")}.CompilerServices.Unsafe.AsPointer<{Global ("AVFoundation.AVAudioConverterInputStatus")}> (ref outStatus));",
			];

			var boolReferenceParameter = @"
using System;
using AVFoundation;

namespace NS {
	public class MyClass {
		public void MyMethod (AVAudioUnitComponentFilter cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				boolReferenceParameter,
				$"var ret = invoker (BlockLiteral, comp__handle__, (byte*) {Global ("System.Runtime")}.CompilerServices.Unsafe.AsPointer<bool> (ref stop));",
			];

			var doubleReferenceParameter = @"
using System;
using AVFoundation;

namespace NS {
	public class MyClass {
		public void MyMethod (AVMusicEventEnumerationBlock cb) {}
	}
}
";

			yield return [
				"someTrampolineName",
				doubleReferenceParameter,
				$"invoker (BlockLiteral, event__handle__, (double*) {Global ("System.Runtime")}.CompilerServices.Unsafe.AsPointer<double> (ref timeStamp), (byte*) global::System.Runtime.CompilerServices.Unsafe.AsPointer<bool> (ref removeEvent));",
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataGetTrampolineNativeInvokeArguments>]
	void CallNativeInvokerDelegateTests (ApplePlatform platform, string trampolineName, string inputText, string expectedExpression)
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
		var argumentSyntax = GetTrampolineNativeInvokeArguments (trampolineName, parameter.Type.Delegate);
		var invocation = CallNativeInvokerDelegate (parameter.Type.Delegate, argumentSyntax);
		var x = invocation.ToFullString ();
		Assert.Equal (expectedExpression, invocation.ToFullString ());
	}

}
