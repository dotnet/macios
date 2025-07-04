// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.Emitters;
using Xamarin.Tests;
using Xamarin.Utils;
using Xunit;

namespace Microsoft.Macios.Generator.Tests.Emitters;

public class BindingSyntaxFactoryMethodTests : BaseGeneratorTestClass {

	class TestDataGetInvocationsTests : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			const string voidMethodNoParamsExportData = @"
using System;
using ObjCBindings;

namespace NS {
	public class MyClass {
		[Export<Method>(""myMethod"")]
		public void MyMethod () {}
	}
}
";

			yield return [
				voidMethodNoParamsExportData,
				$"{Global ("ObjCRuntime")}.Messaging.void_objc_msgSend (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod\"))",
				$"{Global ("ObjCRuntime")}.Messaging.void_objc_msgSendSuper (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod\"))"
			];

			const string asyncVoidMethodNoParamsExportDataMisingFlag = @"
using System;
using ObjCBindings;

namespace NS {
	public class MyClass {
		[Export<Method>(""myMethod"", Flags = Method.Default | Method.Async)]
		public void MyMethod (Action<int> completionHandler) {}
	}
}
";

			yield return [
				asyncVoidMethodNoParamsExportDataMisingFlag,
				$"{Global ("ObjCRuntime")}.Messaging.void_objc_msgSend_NativeHandle (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod\"), ({Global ("ObjCRuntime")}.NativeHandle) block_ptr_completionHandler)",
				$"{Global ("ObjCRuntime")}.Messaging.void_objc_msgSendSuper_NativeHandle (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod\"), ({Global ("ObjCRuntime")}.NativeHandle) block_ptr_completionHandler)",
			];

			const string asyncVoidMethodNoParamsExportDataAsyncFlag = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public class MyClass {
		[Export<Method>(""myMethod"", Flags = Method.Default | Method.Async,
			ResultTypeName = ""MyAsyncResult"")]
		public void MyMethod (Action<bool, string, NSError?> completionHandler) {}
	}
}
";

			yield return [
				asyncVoidMethodNoParamsExportDataAsyncFlag,
				$"{Global ("ObjCRuntime")}.Messaging.void_objc_msgSend_NativeHandle (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod\"), ({Global ("ObjCRuntime")}.NativeHandle) block_ptr_completionHandler)",
				$"{Global ("ObjCRuntime")}.Messaging.void_objc_msgSendSuper_NativeHandle (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod\"), ({Global ("ObjCRuntime")}.NativeHandle) block_ptr_completionHandler)"
			];

			const string stringMethodNoParams = @"
using System;
using ObjCBindings;

namespace NS {
	public class MyClass {

		[Export<Method>(""myMethod"", Flags = Method.Default)]
		public string MyMethod () => string.Empty;
	}
}
";

			yield return [
				stringMethodNoParams,
				$"ret = {Global ("CoreFoundation")}.CFString.FromHandle ({Global ("ObjCRuntime")}.Messaging.NativeHandle_objc_msgSend (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod\")), false)!",
				$"ret = {Global ("CoreFoundation")}.CFString.FromHandle ({Global ("ObjCRuntime")}.Messaging.NativeHandle_objc_msgSendSuper (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod\")), false)!"
			];

			const string customTypeNoParams = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public class CustomType : NSObject {
	}

	public class MyClass {

		[Export<Method>(""myMethod"", Flags = Method.Default)]
		public CustomType MyMethod () => new ();
	}
}
";

			yield return [
				customTypeNoParams,
				$"ret = {Global ("ObjCRuntime")}.Runtime.GetNSObject<{Global ("NS")}.CustomType> ({Global ("ObjCRuntime")}.Messaging.NativeHandle_objc_msgSend (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod\")))!",
				$"ret = {Global ("ObjCRuntime")}.Runtime.GetNSObject<{Global ("NS")}.CustomType> ({Global ("ObjCRuntime")}.Messaging.NativeHandle_objc_msgSendSuper (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod\")))!"
			];

			const string singleParameterMethod = @"
using System;
using ObjCBindings;

namespace NS {
	public class MyClass {
		[Export<Method>(""myMethod:"", Flags = Method.Default)]
		public string MyMethod (string input) => $""{input}_test"";
	}
}
";

			yield return [
				singleParameterMethod,
				$"ret = {Global ("CoreFoundation")}.CFString.FromHandle ({Global ("ObjCRuntime")}.Messaging.NativeHandle_objc_msgSend_NativeHandle (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod:\"), nsinput), false)!",
				$"ret = {Global ("CoreFoundation")}.CFString.FromHandle ({Global ("ObjCRuntime")}.Messaging.NativeHandle_objc_msgSendSuper_NativeHandle (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod:\"), nsinput), false)!"
			];

			const string singleParameterKeywordNameMethod = @"
using System;
using ObjCBindings;

namespace NS {
	public class MyClass {
		[Export<Method>(""myMethod:"", Flags = Method.Default)]
		public string MyMethod (string @event) => $""{@event}_test"";
	}
}
";

			yield return [
				singleParameterKeywordNameMethod,
				$"ret = {Global ("CoreFoundation")}.CFString.FromHandle ({Global ("ObjCRuntime")}.Messaging.NativeHandle_objc_msgSend_NativeHandle (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod:\"), nsevent), false)!",
				$"ret = {Global ("CoreFoundation")}.CFString.FromHandle ({Global ("ObjCRuntime")}.Messaging.NativeHandle_objc_msgSendSuper_NativeHandle (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod:\"), nsevent), false)!"
			];

			const string singleArrayParameterMethod = @"
using System;
using ObjCBindings;

namespace NS {
	public class MyClass {
		[Export<Method>(""myMethod:"", Flags = Method.Default)]
		public string MyMethod (string[] input) => $""{input}_test"";
	}
}
";

			yield return [
				singleArrayParameterMethod,
				$"ret = {Global ("CoreFoundation")}.CFString.FromHandle ({Global ("ObjCRuntime")}.Messaging.NativeHandle_objc_msgSend_NativeHandle (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod:\"), nsa_input.Handle), false)!",
				$"ret = {Global ("CoreFoundation")}.CFString.FromHandle ({Global ("ObjCRuntime")}.Messaging.NativeHandle_objc_msgSendSuper_NativeHandle (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod:\"), nsa_input.Handle), false)!"
			];

			const string nullableSingleArrayParameterMethod = @"
using System;
using ObjCBindings;

namespace NS {
	public class MyClass {
		[Export<Method>(""myMethod:"", Flags = Method.Default)]
		public string MyMethod (string[]? input) => $""{input}_test"";
	}
}
";

			yield return [
				nullableSingleArrayParameterMethod,
				$"ret = {Global ("CoreFoundation")}.CFString.FromHandle ({Global ("ObjCRuntime")}.Messaging.NativeHandle_objc_msgSend_NativeHandle (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod:\"), nsa_input.Handle), false)!",
				$"ret = {Global ("CoreFoundation")}.CFString.FromHandle ({Global ("ObjCRuntime")}.Messaging.NativeHandle_objc_msgSendSuper_NativeHandle (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod:\"), nsa_input.Handle), false)!"
			];

			const string customTypeParameter = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public class CustomType : NSObject {
	}

	public class MyClass {
		[Export<Method>(""myMethod:"", Flags = Method.Default)]
		public void MyMethod (CustomType input) {}
	}
}
";

			yield return [
				customTypeParameter,
				$"{Global ("ObjCRuntime")}.Messaging.void_objc_msgSend_NativeHandle (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod:\"), input__handle__)",
				$"{Global ("ObjCRuntime")}.Messaging.void_objc_msgSendSuper_NativeHandle (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod:\"), input__handle__)"
			];

			const string severalParametersMethod = @"
using System;
using ObjCBindings;

namespace NS {
	public class MyClass {
		[Export<Method>(""myMethod:withOption:"", Flags = Method.Default)]
		public string MyMethod (string input, string? second) => $""{input}_test{second}"";
	}
}
";

			yield return [
				severalParametersMethod,
				$"ret = {Global ("CoreFoundation")}.CFString.FromHandle ({Global ("ObjCRuntime")}.Messaging.NativeHandle_objc_msgSend_NativeHandle_NativeHandle (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod:withOption:\"), nsinput, nssecond), false)!",
				$"ret = {Global ("CoreFoundation")}.CFString.FromHandle ({Global ("ObjCRuntime")}.Messaging.NativeHandle_objc_msgSendSuper_NativeHandle_NativeHandle (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod:withOption:\"), nsinput, nssecond), false)!"
			];

			const string outParameterMethod = @"
using System;
using ObjCBindings;

namespace NS {
	public class MyClass {

		[Export<Method>(""tryGet:"", Flags = Method.Default)]
		public bool TryGetString (out string? example) {
			example = null;
			return false;
		}
	}
}
";
			yield return [
				outParameterMethod,
				$"ret = {Global ("ObjCRuntime")}.Messaging.bool_objc_msgSend_NativeHandle (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"tryGet:\"), &nsexample) != 0",
				$"ret = {Global ("ObjCRuntime")}.Messaging.bool_objc_msgSendSuper_NativeHandle (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"tryGet:\"), &nsexample) != 0"
			];

			const string outStructParameter = @"
using System;
using ObjCBindings;

namespace NS {
	public readonly struct MyStruct {
		public string Name { get; }
		public MyStruct(string name) {
			Name = name;
		}
	}
	public class MyClass {
		[Export<Method>(""myMethod:"", Flags = Method.Default)]
		public void MyMethod (out MyStruct data) {
			data = new MyStruct (""test"");
		}
	}
}
";

			yield return [
				outStructParameter,
				$"{Global ("ObjCRuntime")}.Messaging.void_objc_msgSend_MyStruct (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod:\"), ({Global ("NS")}.MyStruct*) {Global ("System")}.Runtime.CompilerServices.Unsafe.AsPointer<{Global ("NS")}.MyStruct> (ref data))",
				$"{Global ("ObjCRuntime")}.Messaging.void_objc_msgSendSuper_MyStruct (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod:\"), ({Global ("NS")}.MyStruct*) {Global ("System")}.Runtime.CompilerServices.Unsafe.AsPointer<{Global ("NS")}.MyStruct> (ref data))"
			];

			const string outNSErrorMethod = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public class MyClass {

		[Export<Method>(""tryGet:withError:"", Flags = Method.Default)]
		public bool TryGetString (string data, out NSError? example) {
			example = null;
			return false;
		}
	}
}
";
			yield return [
				outNSErrorMethod,
				$"ret = {Global ("ObjCRuntime")}.Messaging.bool_objc_msgSend_NativeHandle_NativeHandle (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"tryGet:withError:\"), nsdata, &example__handle__) != 0",
				$"ret = {Global ("ObjCRuntime")}.Messaging.bool_objc_msgSendSuper_NativeHandle_NativeHandle (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"tryGet:withError:\"), nsdata, &example__handle__) != 0",
			];

			const string returnTypeBindFromAttribute = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public class MyClass {
		[Export<Method>(""myMethod"", Flags = Method.Default)]
		[return: BindFrom (typeof(NSNumber))]
		public int MyMethod () {}
	}
}
";

			yield return [
				returnTypeBindFromAttribute,
				$"ret = {Global ("Foundation")}.NSNumber.ToInt32 ({Global ("ObjCRuntime")}.Messaging.NativeHandle_objc_msgSend (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod\")))",
				$"ret = {Global ("Foundation")}.NSNumber.ToInt32 ({Global ("ObjCRuntime")}.Messaging.NativeHandle_objc_msgSendSuper (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod\")))",
			];

			const string parameterBindFromAttr = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public class MyClass {
		[Export<Method>(""myMethod:"", Flags = Method.Default)]
		public void MyMethod ([BindFrom (typeof(NSNumber))] int value) {}
	}
}
";

			yield return [
				parameterBindFromAttr,
				$"{Global ("ObjCRuntime")}.Messaging.void_objc_msgSend_int (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod:\"), nsb_value__handle__)",
				$"{Global ("ObjCRuntime")}.Messaging.void_objc_msgSendSuper_int (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"myMethod:\"), nsb_value__handle__)"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataGetInvocationsTests>]
	void GetInvocationsTests (ApplePlatform platform, string inputText, string expectedSend, string expectedSendSuper)
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
		var invocations = BindingSyntaxFactory.GetInvocations (changes.Value);
		Assert.Equal (expectedSend, invocations.Send.ToString ());
		Assert.Equal (expectedSendSuper, invocations.SendSuper.ToString ());
	}

}
