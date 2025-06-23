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
				@$"var block_callbackParameter = {Global ("ObjCRuntime.Trampolines.")}{Nomenclator.GetTrampolineClassName ("Action", Nomenclator.TrampolineClassType.StaticBridgeClass)}.CreateNullableBlock (callbackParameter);
{Global ("ObjCRuntime")}.BlockLiteral* block_ptr_callbackParameter = callbackParameter is not null ? &block_callbackParameter : null;
",
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
				outNSObject,
				"*outNSObject = default;\n",
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
