// Copyright (c) Microsoft Corporation.
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

public class BindingSyntaxFactoryArgumentsTests : BaseGeneratorTestClass {

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
				nullableINativeArrayParameter,
				$"{Global ("System.GC")}.KeepAlive (nsa_inativeArray);\n"
			];

			// byref string parameter

			var byrefStringParam = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (out string outString);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				byrefStringParam,
				$"outString = {Global ("CoreFoundation")}.CFString.FromHandle (nsoutString, false)!;\n",
			];

			// byref string parameter nullable

			var nullableByrefStringParam = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (out string? outNullableString);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				nullableByrefStringParam,
				$"outNullableString = {Global ("CoreFoundation")}.CFString.FromHandle (nsoutNullableString, false);\n"
			];

			// byref num not smart

			var outNotSmartEnum = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public MyEnum {
		First = 0,
		Last = 1,
	}
	public delegate void Callback (out MyEnum outNullableString);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				outNotSmartEnum,
				string.Empty
			];

			// byref int

			var outInt = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (out int outNullableString);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				outInt,
				string.Empty
			];

			// byref struct

			var outStruct = @"
using System;
using AudioToolbox;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (out AudioTimeStamp outStruct);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				outStruct,
				string.Empty
			];

			// byref INativeObject

			var outINativeObject = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;
using Security;

namespace NS {
	public delegate void Callback (out SecKey outINativeObject);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				outINativeObject,
				$"outINativeObject = {Global ("ObjCRuntime")}.Runtime.GetINativeObject<global::Security.SecKey> (outINativeObject__handle__)!;\n"
			];

			// byref NSObject

			var outNSObject = @"
using System;
using AVFoundation;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (out NSObject outNSObject);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				outNSObject,
				$"outNSObject = {Global ("ObjCRuntime")}.Runtime.GetNSObject<{Global ("Foundation")}.NSObject> (outNSObject__handle__)!;\n"
			];

			// byref string array

			var outStringArray = @"
using System;
using AVFoundation;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (out string[] outStringArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				outStringArray,
				$"outStringArray = {Global ("CoreFoundation")}.CFArray.StringArrayFromHandle (nsa_outStringArray, false)!;\n"
			];

			// byref string array nullable

			var outNullStringArray = @"
using System;
using AVFoundation;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (out string[]? outNullStringArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				outNullStringArray,
				$"outNullStringArray = {Global ("CoreFoundation")}.CFArray.StringArrayFromHandle (nsa_outNullStringArray, false);\n"
			];

			// byref array of INativeObject

			var outINativeObjectArray = @"
using System;
using AVFoundation;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (out AVAsset[] outINativeObjectArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				outINativeObjectArray,
				$"outINativeObjectArray = {Global ("CoreFoundation")}.CFArray.ArrayFromHandle<{Global ("AVFoundation")}.AVAsset> (nsa_outINativeObjectArray)!;\n",
			];
			// byref array of NSObject

			var outNSObjectArray = @"
using System;
using AVFoundation;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (out NSObject[] outNSObject);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				outNSObjectArray,
				$"outNSObject = {Global ("CoreFoundation")}.CFArray.ArrayFromHandle<{Global ("Foundation")}.NSObject> (nsa_outNSObject)!;\n"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataGetTrampolinePostNativeInvokeArgumentConversions>]
	void GetTrampolinePostNativeInvokeArgumentConversionsTests (ApplePlatform platform, string inputText, string expectedExpression)
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
		var conversions = GetPostNativeInvokeArgumentConversions (parameter.Type.Delegate!.Parameters [0]);
		// uses a tabbeb string builder to get the conversion string and test
		var sb = new TabbedStringBuilder (new ());
		sb.Write (conversions, false);
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
				stringParameter,
				$"var nsstringParameter = {Global ("CoreFoundation")}.CFString.CreateNative (stringParameter);\n",
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
				stringArrayParameter,
				$"using var nsa_stringParameter = {Global ("Foundation")}.NSArray.FromStrings (stringParameter);\n",
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
				nullableStringArrayParameter,
				$"using var nsa_stringParameter = stringParameter is null ? null : {Global ("Foundation")}.NSArray.FromStrings (stringParameter);\n",
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
				smartEnumParameter,
				"using var nsb_enumParameter = enumParameter.GetConstant ();\n"
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
				nsObjectArrayParameter,
				$"using var nsa_nsObjectParameter = {Global ("Foundation")}.NSArray.FromNSObjects (nsObjectParameter);\n",
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
				nullableNSObjectArrayParameter,
				$"using var nsa_nsObjectParameter = nsObjectParameter is null ? null : {Global ("Foundation")}.NSArray.FromNSObjects (nsObjectParameter);\n",
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
				inativeArrayParameter,
				$"using var nsa_inativeArray = {Global ("Foundation")}.NSArray.FromNSObjects (inativeArray);\n",
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
				nullableINativeArrayParameter,
				$"using var nsa_inativeArray = inativeArray is null ? null : {Global ("Foundation")}.NSArray.FromNSObjects (inativeArray);\n"
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
				cmSampleBuffer,
				"var cmSampleBuffer__handle__ = cmSampleBuffer!.GetNonNullHandle (nameof (cmSampleBuffer));\n",
			];

			var blockParameter = @"
using System;
using ObjCRuntime;

namespace NS {
	public delegate void Callback ([BlockCallback] Action? callbackParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				blockParameter,
				@$"using var block_callbackParameter = {Global ("ObjCRuntime.Trampolines.")}{Nomenclator.GetTrampolineClassName ("Action", Nomenclator.TrampolineClassType.StaticBridgeClass)}.CreateNullableBlock (callbackParameter);
{Global ("ObjCRuntime")}.BlockLiteral* block_ptr_callbackParameter = callbackParameter is not null ? &block_callbackParameter : null;
",
			];

			var notDecoratedBlockParameter = @"
using System;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (Action? callbackParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				notDecoratedBlockParameter,
				@$"using var block_callbackParameter = {Global ("ObjCRuntime.Trampolines.")}{Nomenclator.GetTrampolineClassName ("Action", Nomenclator.TrampolineClassType.StaticBridgeClass)}.CreateNullableBlock (callbackParameter);
{Global ("ObjCRuntime")}.BlockLiteral* block_ptr_callbackParameter = callbackParameter is not null ? &block_callbackParameter : null;
",
			];

			// byref string parameter

			var byrefStringParam = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (out string outString);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				byrefStringParam,
				$"{Global ("ObjCRuntime")}.NativeHandle nsoutString;\n"
			];

			// byref string parameter nullable

			var nullableByrefStringParam = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (out string? outNullableString);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				nullableByrefStringParam,
				"global::ObjCRuntime.NativeHandle nsoutNullableString;\n"
			];

			// byref num not smart

			var outNotSmartEnum = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public MyEnum {
		First = 0,
		Last = 1,
	}
	public delegate void Callback (out MyEnum outNullableString);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				outNotSmartEnum,
				string.Empty
			];

			// byref int

			var outInt = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (out int outNullableString);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				outInt,
				string.Empty
			];

			// byref struct

			var outStruct = @"
using System;
using AudioToolbox;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (out AudioTimeStamp outStruct);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				outStruct,
				string.Empty
			];

			// byref INativeObject

			var outINativeObject = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;
using Security;

namespace NS {
	public delegate void Callback (out SecKey outINativeObject);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				outINativeObject,
				$"{Global ("ObjCRuntime")}.NativeHandle outINativeObject__handle__;\n",
			];

			// byref NSObject

			var outNSObject = @"
using System;
using AVFoundation;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (out NSObject outNSObject);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				outNSObject,
				$"{Global ("ObjCRuntime")}.NativeHandle outNSObject__handle__;\n",
			];

			// byref string array

			var outStringArray = @"
using System;
using AVFoundation;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (out string[] outStringArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				outStringArray,
				$"{Global ("ObjCRuntime")}.NativeHandle nsa_outStringArray;\n",
			];

			// byref string array nullable

			var outNullStringArray = @"
using System;
using AVFoundation;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (out string[]? outNullStringArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				outNullStringArray,
				$"{Global ("ObjCRuntime")}.NativeHandle nsa_outNullStringArray;\n",
			];

			// byref array of INativeObject

			var outINativeObjectArray = @"
using System;
using AVFoundation;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (out AVAsset[] outINativeObjectArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				outINativeObjectArray,
				"global::ObjCRuntime.NativeHandle nsa_outINativeObjectArray;\n"
			];

			// byref array of NSObject

			var outNSObjectArray = @"
using System;
using AVFoundation;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (out NSObject[] outNSObject);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				outNSObjectArray,
				$"{Global ("ObjCRuntime")}.NativeHandle nsa_outNSObject;\n",
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataGetTrampolinePreNativeInvokeArgumentConversions>]
	void GetTrampolinePreNativeInvokeArgumentConversionsTests (ApplePlatform platform, string inputText, string expectedExpression)
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
		var conversions = GetPreNativeInvokeArgumentConversions (parameter.Type.Delegate!.Parameters [0]);
		// uses a tabbeb string builder to get the conversion string and test
		var sb = new TabbedStringBuilder (new ());
		sb.Write (conversions, false);
		Assert.Equal (expectedExpression, sb.ToCode ());
	}

	class TestDataGetTrampolineValidationsArgumentConversionsTests : IEnumerable<object []> {
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
				stringParameter,
				$@"if (stringParameter is null)
	{Global ("ObjCRuntime")}.ThrowHelper.ThrowArgumentNullException (nameof (stringParameter));
",
			];

			var nullableStringParameter = @"
using System;

namespace NS {
	public delegate void Callback (string? nullableStringParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				nullableStringParameter,
				string.Empty,
			];

			var stringArrayParameter = @"
using System;

namespace NS {
	public delegate void Callback (string[] stringArrayParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				stringArrayParameter,
				$@"if (stringArrayParameter is null)
	{Global ("ObjCRuntime")}.ThrowHelper.ThrowArgumentNullException (nameof (stringArrayParameter));
",
			];

			var nullableStringArrayParameter = @"
using System;

namespace NS {
	public delegate void Callback (string[]? nullableStringArrayParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				nullableStringArrayParameter,
				string.Empty,
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
				smartEnumParameter,
				string.Empty,
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
				nsObjectParameter,
				string.Empty,
			];

			// nullable NSObject parameter

			var nullableNSObjectParameter = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {

    public delegate void Callback (NSObject? nullableNsObjectParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				nullableNSObjectParameter,
				string.Empty,
			];

			// NSObject array parameter

			var nsObjectArrayParameter = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {

    public delegate void Callback (NSObject[] nsObjectArrayParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				nsObjectArrayParameter,
				$@"if (nsObjectArrayParameter is null)
	{Global ("ObjCRuntime")}.ThrowHelper.ThrowArgumentNullException (nameof (nsObjectArrayParameter));
",
			];

			// nullable NSObject array parameter

			var nullableNSObjectArrayParameter = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {

    public delegate void Callback (NSObject[]? nullableNSObjectArrayParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				nullableNSObjectArrayParameter,
				string.Empty,
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
				iNativeParameter,
				string.Empty,
			];

			// nullable INativeObject parameter

			var nullableINativeParameter = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (CMTimebase? nullableINativeParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nullableINativeParameter,
				string.Empty,
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
				inativeArrayParameter,
				$@"if (inativeArray is null)
	{Global ("ObjCRuntime")}.ThrowHelper.ThrowArgumentNullException (nameof (inativeArray));
",
			];

			// nullable INativeObject array parameter

			var nullableINativeArrayParameter = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (CMTimebase[]? nullableINativeArrayParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				nullableINativeArrayParameter,
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
				audioBuffer,
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
				cmSampleBuffer,
				string.Empty,
			];

			var blockParameter = @"
using System;
using ObjCRuntime;

namespace NS {
	public delegate void Callback ([BlockCallback] Action? callbackParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				blockParameter,
				string.Empty,
			];

			var notDecoratedBlockParameter = @"
using System;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (Action? callbackParameter);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				notDecoratedBlockParameter,
				string.Empty,
			];

			// byref string parameter

			var byrefStringParam = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (out string outString);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				byrefStringParam,
				string.Empty,
			];

			// byref string parameter nullable

			var nullableByrefStringParam = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (out string? outNullableString);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				nullableByrefStringParam,
				string.Empty,
			];

			// byref num not smart

			var outNotSmartEnum = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public MyEnum {
		First = 0,
		Last = 1,
	}
	public delegate void Callback (out MyEnum outNullableString);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				outNotSmartEnum,
				string.Empty
			];

			// byref int

			var outInt = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (out int outNullableString);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				outInt,
				string.Empty
			];

			// byref struct

			var outStruct = @"
using System;
using AudioToolbox;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (out AudioTimeStamp outStruct);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				outStruct,
				string.Empty
			];

			// byref INativeObject

			var outINativeObject = @"
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;
using Security;

namespace NS {
	public delegate void Callback (out SecKey outINativeObject);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				outINativeObject,
				string.Empty,
			];

			// byref NSObject

			var outNSObject = @"
using System;
using AVFoundation;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (out NSObject outNSObject);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				outNSObject,
				string.Empty,
			];

			// byref string array

			var outStringArray = @"
using System;
using AVFoundation;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (out string[] outStringArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				outStringArray,
				string.Empty,
			];

			// byref string array nullable

			var outNullStringArray = @"
using System;
using AVFoundation;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (out string[]? outNullStringArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				outNullStringArray,
				string.Empty,
			];

			// byref array of INativeObject

			var outINativeObjectArray = @"
using System;
using AVFoundation;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (out AVAsset[] outINativeObjectArray);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				outINativeObjectArray,
				string.Empty,
			];

			// byref array of NSObject

			var outNSObjectArray = @"
using System;
using AVFoundation;
using Foundation;
using CoreMedia;
using ObjCRuntime;

namespace NS {
	public delegate void Callback (out NSObject[] outNSObject);
	public class MyClass {
		public void MyMethod (Callback cb) {}
	}
}
";
			yield return [
				outNSObjectArray,
				string.Empty,
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataGetTrampolineValidationsArgumentConversionsTests>]
	void GetTrampolineValidationsArgumentConversionsTests (ApplePlatform platform, string inputText, string expectedExpression)
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
		var conversions = GetNativeInvokeArgumentValidations (parameter.Type.Delegate!.Parameters [0]);
		// uses a tabbeb string builder to get the conversion string and test
		var sb = new TabbedStringBuilder (new ());
		sb.Write (conversions, false);
		Assert.Equal (expectedExpression, sb.ToCode ());
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
				outBoolean,
				string.Empty
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
				string.Empty,
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
				refNSObject,
				string.Empty,
			];

		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataGetTrampolineNativeInvokeArgumentInitializations>]
	void GetTrampolineInvokeArgumentNativeInitializationsTests (ApplePlatform platform, string inputText, string expectedExpression)
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
		var conversions = GetNativeInvokeArgumentInitializations (parameter.Type.Delegate!.Parameters [0]);
		// uses a tabbeb string builder to get the conversion string and test
		var sb = new TabbedStringBuilder (new ());
		sb.Write (conversions);
		Assert.Equal (expectedExpression, sb.ToCode ());
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
				outBoolean,
				string.Empty
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
				string.Empty
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataGetTrampolineInvokeArgumentInitializations>]
	void GetTrampolineInvokeArgumentInitializationsTests (ApplePlatform platform, string inputText, string expectedExpression)
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
		var conversions = GetTrampolineInvokeArgumentInitializations (parameter.Type.Delegate!.Parameters [0]);
		// uses a tabbeb string builder to get the conversion string and test
		var sb = new TabbedStringBuilder (new ());
		sb.Write (conversions);
		Assert.Equal (expectedExpression, sb.ToCode ());
	}
}
