// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
#pragma warning disable APL0003

using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.Availability;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.IO;
using ObjCRuntime;
using Xunit;
using static Microsoft.Macios.Generator.Tests.TestDataFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Generator.Tests.IO;

public class TabbedStringBuilderTests {
	StringBuilder sb;

	public TabbedStringBuilderTests ()
	{
		sb = new ();
	}

	[Theory]
	[InlineData (0, "")]
	[InlineData (1, "\t")]
	[InlineData (5, "\t\t\t\t\t")]
	public void ConstructorNotBlockTest (int tabCount, string expectedTabs)
	{
		string result;
		using (var block = new TabbedStringBuilder (sb, tabCount)) {
			block.WriteLine ("Test");
			result = block.ToCode ();
		}

		Assert.Equal ($"{expectedTabs}Test\n", result);
	}

	[Theory]
	[InlineData (0, "")]
	[InlineData (1, "\t")]
	[InlineData (5, "\t\t\t\t\t")]
	public void ConstructorBlockTest (int tabCount, string expectedTabs)
	{
		string result;
		using (var block = new TabbedStringBuilder (sb, tabCount, true)) {
			block.WriteLine ("Test");
			result = block.ToCode ();
		}

		Assert.Equal ($"{expectedTabs}{{\n{expectedTabs}\tTest\n{expectedTabs}}}\n", result);
	}

	[Theory]
	[InlineData (0)]
	[InlineData (1)]
	[InlineData (5)]
	public void AppendLineTest (int tabCount)
	{
		string result;
		using (var block = new TabbedStringBuilder (sb, tabCount)) {
			block.WriteLine ();
			result = block.ToCode ();
		}

		// an empty line should have not tabs
		Assert.Equal ("\n", result);
	}

	[Theory]
	[InlineData (0)]
	[InlineData (1)]
	[InlineData (5)]
	public async Task AppendLineTestAsync (int tabCount)
	{
		string result;
		using (var block = new TabbedStringBuilder (sb, tabCount)) {
			await block.WriteLineAsync ();
			result = block.ToCode ();
		}

		// an empty line should have not tabs
		Assert.Equal ("\n", result);
	}

	[Theory]
	[InlineData ("// test comment", 0, "")]
	[InlineData ("var t = 1;", 1, "\t")]
	[InlineData ("Console.WriteLine (\"1\");", 5, "\t\t\t\t\t")]
	public void AppendLineStringTest (string line, int tabCount, string expectedTabs)
	{
		string result;
		using (var block = new TabbedStringBuilder (sb, tabCount, true)) {
			block.WriteLine (line);
			result = block.ToCode ();
		}

		Assert.Equal ($"{expectedTabs}{{\n{expectedTabs}\t{line}\n{expectedTabs}}}\n", result);
	}

	[Theory]
	[InlineData ("// test comment", 0, "")]
	[InlineData ("var t = 1;", 1, "\t")]
	[InlineData ("Console.WriteLine (\"1\");", 5, "\t\t\t\t\t")]
	public async Task AppendLineStringTestAsync (string line, int tabCount, string expectedTabs)
	{
		string result;
		using (var block = new TabbedStringBuilder (sb, tabCount, true)) {
			await block.WriteLineAsync (line);
			result = block.ToCode ();
		}

		Assert.Equal ($"{expectedTabs}{{\n{expectedTabs}\t{line}\n{expectedTabs}}}\n", result);
	}

	[Theory]
	[InlineData (0, "")]
	[InlineData (1, "\t")]
	[InlineData (5, "\t\t\t\t\t")]
	public void AppendInterpolatedLineTest (int tabCount, string expectedTabs)
	{
		string result;
		var val1 = "Hello";
		var val2 = "World";
		var val3 = '!';
		var line = "Hello World!";
		var expected = $"{expectedTabs}{{\n{expectedTabs}\t{line}\n{expectedTabs}}}\n";
		using (var block = new TabbedStringBuilder (sb, tabCount, true)) {
			block.WriteLine ($"{val1} {val2}{val3}");
			result = block.ToCode ();
		}

		Assert.Equal (expected, result);
	}


	[Theory]
	[InlineData (0, "")]
	[InlineData (1, "\t")]
	[InlineData (5, "\t\t\t\t\t")]
	public void AppendRawTest (int tabCount, string expectedTabs)
	{
		var input = @"
## Raw string
Because we are using a raw string  we expected:
  1. The string to be split in lines
  2. All lines should have the right indentation
     - This means nested one
  3. And all lines should have the correct tabs
";
		var expected = $@"
{expectedTabs}## Raw string
{expectedTabs}Because we are using a raw string  we expected:
{expectedTabs}  1. The string to be split in lines
{expectedTabs}  2. All lines should have the right indentation
{expectedTabs}     - This means nested one
{expectedTabs}  3. And all lines should have the correct tabs
";
		string result;
		using (var block = new TabbedStringBuilder (sb, tabCount)) {
			block.WriteRaw (input);
			result = block.ToCode ();
		}

		Assert.Equal (expected, result);
	}

	[Theory]
	[InlineData (0, "")]
	[InlineData (1, "\t")]
	[InlineData (5, "\t\t\t\t\t")]
	public async Task AppendRawTestAsync (int tabCount, string expectedTabs)
	{
		var input = @"
## Raw string
Because we are using a raw string  we expected:
  1. The string to be split in lines
  2. All lines should have the right indentation
     - This means nested one
  3. And all lines should have the correct tabs
";
		var expected = $@"
{expectedTabs}## Raw string
{expectedTabs}Because we are using a raw string  we expected:
{expectedTabs}  1. The string to be split in lines
{expectedTabs}  2. All lines should have the right indentation
{expectedTabs}     - This means nested one
{expectedTabs}  3. And all lines should have the correct tabs
";
		string result;
		using (var block = new TabbedStringBuilder (sb, tabCount)) {
			await block.WriteRawAsync (input);
			result = block.ToCode ();
		}

		Assert.Equal (expected, result);
	}

	[Theory]
	[InlineData (0, "")]
	[InlineData (1, "\t")]
	[InlineData (5, "\t\t\t\t\t")]
	public void AppendGeneratedCodeAttributeTest (int tabCount, string expectedTabs)
	{
		var expected = $"{expectedTabs}[BindingImpl (BindingImplOptions.GeneratedCode)]\n";
		string result;
		using (var block = new TabbedStringBuilder (sb, tabCount)) {
			block.AppendGeneratedCodeAttribute (false);
			result = block.ToCode ();
		}

		Assert.Equal (expected, result);
	}

	[Theory]
	[InlineData (0, "")]
	[InlineData (1, "\t")]
	[InlineData (5, "\t\t\t\t\t")]
	public void AppendGeneratedCodeAttributeOptimizableTest (int tabCount, string expectedTabs)
	{
		var expected =
			$"{expectedTabs}[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]\n";
		string result;
		using (var block = new TabbedStringBuilder (sb, tabCount)) {
			block.AppendGeneratedCodeAttribute ();
			result = block.ToCode ();
		}

		Assert.Equal (expected, result);
	}

	[Theory]
	[InlineData (EditorBrowsableState.Advanced, 0, "")]
	[InlineData (EditorBrowsableState.Never, 1, "\t")]
	[InlineData (EditorBrowsableState.Always, 5, "\t\t\t\t\t")]
	public void AppendEditorBrowsableAttributeTest (EditorBrowsableState state, int tabCount, string expectedTabs)
	{
		var expected = $"{expectedTabs}[EditorBrowsable (EditorBrowsableState.{state})]\n";
		string result;
		using (var block = new TabbedStringBuilder (sb, tabCount)) {
			block.AppendEditorBrowsableAttribute (state);
			result = block.ToCode ();
		}

		Assert.Equal (expected, result);
	}

	[Theory]
	[InlineData (0, "")]
	[InlineData (1, "\t")]
	[InlineData (5, "\t\t\t\t\t")]
	public void AppendNotificationAdviceTests (int tabCount, string expectedTabs)
	{
		var className = "TestClass";
		var notificationName = "DidWriteAttribute";
		var expected = $"{expectedTabs}[Advice (\"Use '{className}.Notifications.{notificationName}' helper method instead.\")]\n";
		string result;
		using (var block = new TabbedStringBuilder (sb, tabCount)) {
			block.AppendNotificationAdvice (className, notificationName);
			result = block.ToCode ();
		}

		Assert.Equal (expected, result);
	}

	[Theory]
	[InlineData (0, "")]
	[InlineData (1, "\t")]
	[InlineData (5, "\t\t\t\t\t")]
	public void AppendDesignatedInitializer (int tabCount, string expectedTabs)
	{
		var expected = $"{expectedTabs}[DesignatedInitializer]\n";
		string result;
		using (var block = new TabbedStringBuilder (sb, tabCount)) {
			block.AppendDesignatedInitializer ();
			result = block.ToCode ();
		}

		Assert.Equal (expected, result);
	}

	[Theory]
	[InlineData (true, 0, "")]
	[InlineData (true, 1, "\t")]
	[InlineData (true, 5, "\t\t\t\t\t")]
	[InlineData (false, 0, "")]
	[InlineData (false, 1, "\t")]
	[InlineData (false, 5, "\t\t\t\t\t")]
	public void AppendPreserveAttribute (bool conditional, int tabCount, string expectedTabs)
	{
		var expected = $"{expectedTabs}[Preserve (Conditional = {conditional.ToString ().ToLower ()})]\n";
		string result;
		using (var block = new TabbedStringBuilder (sb, tabCount)) {
			block.AppendPreserveAttribute (conditional);
			result = block.ToCode ();
		}

		Assert.Equal (expected, result);
	}

	[Theory]
	[InlineData ("\"MyMember\"", 0, "")]
	[InlineData ("\"MyOtherMember\"", 1, "\t")]
	[InlineData ("nameof(MyProperty)", 5, "\t\t\t\t\t")]
	public void AppendDynamicDependencyAttributeTests (string member, int tabCount, string expectedTabs)
	{
		var expected = $"{expectedTabs}[DynamicDependency (\"{member}\")]\n";
		string result;
		using (var block = new TabbedStringBuilder (sb, tabCount)) {
			block.AppendDynamicDependencyAttribute (member);
			result = block.ToCode ();
		}

		Assert.Equal (expected, result);
	}

	[Theory]
	[InlineData ("MyProtocol", "MyProtocolWrapper", 0, "")]
	[InlineData ("AnotherProtocol", "AnotherProtocolWrapper", 1, "\t")]
	[InlineData ("ThirdProtocol", "ThirdProtocolWrapper", 5, "\t\t\t\t\t")]
	public void AppendProtocolAttributeTests (string name, string wrapperName, int tabCount, string expectedTabs)
	{
		var expected = $"{expectedTabs}[Protocol (Name = \"{name}\", WrapperType = typeof ({wrapperName}))]\n";
		string result;
		using (var block = new TabbedStringBuilder (sb, tabCount)) {
			block.AppendProtocolAttribute (name, wrapperName);
			result = block.ToCode ();
		}

		Assert.Equal (expected, result);
	}

	[Theory]
	[InlineData (0, "")]
	[InlineData (1, "\t")]
	[InlineData (5, "\t\t\t\t\t")]
	public void AppendRequiredMemberAttributeTests (int tabCount, string expectedTabs)
	{
		var expected = $"{expectedTabs}[global::Foundation.RequiredMember]\n";
		string result;
		using (var block = new TabbedStringBuilder (sb, tabCount)) {
			block.AppendRequiredMemberAttribute ();
			result = block.ToCode ();
		}

		Assert.Equal (expected, result);
	}

	[Theory]
	[InlineData (0, "")]
	[InlineData (1, "\t")]
	[InlineData (5, "\t\t\t\t\t")]
	public void CreateEmptyBlockTest (int tabCount, string expectedTabs)
	{
		var blockContent = "// the test";
		var expected = $@"{expectedTabs}{{
{expectedTabs}{"\t"}{blockContent}
{expectedTabs}}}
";
		string result;
		using (var block = new TabbedStringBuilder (sb, tabCount)) {
			using (var nested = block.CreateBlock (true)) {
				nested.WriteLine (blockContent);
			}

			result = block.ToCode ();
		}

		Assert.Equal (expected, result);
	}

	[Theory]
	[InlineData (0, "", "if (true)")]
	[InlineData (1, "\t", "using (var t = new StringBuilder)")]
	[InlineData (5, "\t\t\t\t\t", "fixed (*foo)")]
	public void CreateBlockTest (int tabCount, string expectedTabs, string blockType)
	{
		var blockContent = "// the test";
		var expected = $@"{expectedTabs}{blockType}
{expectedTabs}{{
{expectedTabs}{"\t"}{blockContent}
{expectedTabs}}}
";
		string result;
		using (var block = new TabbedStringBuilder (sb, tabCount)) {
			using (var nested = block.CreateBlock (blockType, true)) {
				nested.WriteLine (blockContent);
			}

			result = block.ToCode ();
		}

		Assert.Equal (expected, result);
	}

	[Theory]
	[InlineData (0, "")]
	[InlineData (1, "\t")]
	[InlineData (5, "\t\t\t\t\t")]
	public void WriteHeaderTest (int tabCount, string expectedTabs)
	{
		var expected = $@"{expectedTabs}// <auto-generated />

{expectedTabs}#nullable enable

";
		string result;
		using (var block = new TabbedStringBuilder (sb, tabCount)) {
			block.WriteHeader ();
			result = block.ToCode ();
		}

		Assert.Equal (expected, result);
	}


	public static IEnumerable<object []> AppendMemberAvailabilityTestData {
		get {
			var builder = SymbolAvailability.CreateBuilder ();

			// single platform, available no version
			builder.Add (new SupportedOSPlatformData ("ios"));
			yield return [builder.ToImmutable (), "[SupportedOSPlatform (\"ios\")]\n"];
			builder.Clear ();

			// single platform available with version
			builder.Add (new SupportedOSPlatformData ("macos13.0"));
			yield return [builder.ToImmutable (), "[SupportedOSPlatform (\"macos13.0\")]\n"];
			builder.Clear ();

			// single platform available with version, unavailable with version
			builder.Add (new SupportedOSPlatformData ("ios"));
			builder.Add (new UnsupportedOSPlatformData ("ios13.0"));
			yield return [builder.ToImmutable (), "[SupportedOSPlatform (\"ios\")]\n[UnsupportedOSPlatform (\"ios13.0\")]\n"];
			builder.Clear ();

			// several platforms available no version
			builder.Add (new SupportedOSPlatformData ("ios"));
			builder.Add (new SupportedOSPlatformData ("tvos"));
			builder.Add (new SupportedOSPlatformData ("macos"));
			yield return [builder.ToImmutable (), "[SupportedOSPlatform (\"macos\")]\n[SupportedOSPlatform (\"ios\")]\n[SupportedOSPlatform (\"tvos\")]\n"];
			builder.Clear ();

			// several platforms available with version 
			builder.Add (new SupportedOSPlatformData ("ios12.0"));
			builder.Add (new SupportedOSPlatformData ("tvos12.0"));
			builder.Add (new SupportedOSPlatformData ("macos10.0"));
			yield return [builder.ToImmutable (), "[SupportedOSPlatform (\"macos10.0\")]\n[SupportedOSPlatform (\"ios12.0\")]\n[SupportedOSPlatform (\"tvos12.0\")]\n"];
			builder.Clear ();

			// several platforms unsupported
			// several platforms available with version 
			builder.Add (new UnsupportedOSPlatformData ("ios12.0"));
			builder.Add (new UnsupportedOSPlatformData ("tvos12.0"));
			builder.Add (new UnsupportedOSPlatformData ("macos"));
			yield return [builder.ToImmutable (), "[UnsupportedOSPlatform (\"macos\")]\n[UnsupportedOSPlatform (\"ios12.0\")]\n[UnsupportedOSPlatform (\"tvos12.0\")]\n"];
			builder.Clear ();
		}
	}

	[Theory]
	[MemberData (nameof (AppendMemberAvailabilityTestData))]
	void AppendMemberAvailabilityTest (SymbolAvailability availability, string expectedString)
	{
		var block = new TabbedStringBuilder (sb);
		block.AppendMemberAvailability (availability);
		var result = block.ToCode ();
		Assert.Equal (expectedString, result);
	}

	public static IEnumerable<object []> AppendProtocolMemberDataTestData {
		get {
			yield return [
				new ProtocolMemberData (
					isRequired: true,
					isProperty: true,
					isStatic: false,
					name: "Name",
					selector: "name"
				) {
					PropertyType = ReturnTypeForString (),
					GetterSelector = "name",
					SetterSelector = null,
					ArgumentSemantic = ArgumentSemantic.None,
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [],
				},
				"[ProtocolMember (IsRequired = true, IsProperty = true, IsStatic = false, Name = \"Name\", Selector = \"name\", PropertyType = typeof (string), GetterSelector = \"name\", SetterSelector = null, ArgumentSemantic = ArgumentSemantic.None)]\n"
			];

			yield return [
				new ProtocolMemberData (
					isRequired: false,
					isProperty: true,
					isStatic: false,
					name: "Name",
					selector: "name"
				) {
					PropertyType = ReturnTypeForString (),
					GetterSelector = "name",
					SetterSelector = null,
					ArgumentSemantic = ArgumentSemantic.None,
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [],
				},
				"[ProtocolMember (IsRequired = false, IsProperty = true, IsStatic = false, Name = \"Name\", Selector = \"name\", PropertyType = typeof (string), GetterSelector = \"name\", SetterSelector = null, ArgumentSemantic = ArgumentSemantic.None)]\n"
			];

			yield return [
				new ProtocolMemberData (
					isRequired: true,
					isProperty: true,
					isStatic: true,
					name: "Name",
					selector: "name"
				) {
					PropertyType = ReturnTypeForString (),
					GetterSelector = "name",
					SetterSelector = null,
					ArgumentSemantic = ArgumentSemantic.None,
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [],
				},
				"[ProtocolMember (IsRequired = true, IsProperty = true, IsStatic = true, Name = \"Name\", Selector = \"name\", PropertyType = typeof (string), GetterSelector = \"name\", SetterSelector = null, ArgumentSemantic = ArgumentSemantic.None)]\n"
			];

			yield return [
				new ProtocolMemberData (
					isRequired: true,
					isProperty: true,
					isStatic: false,
					name: "Name",
					selector: "name"
				) {
					PropertyType = ReturnTypeForString (),
					GetterSelector = "name",
					SetterSelector = "setName:",
					ArgumentSemantic = ArgumentSemantic.None,
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [],
				},
				"[ProtocolMember (IsRequired = true, IsProperty = true, IsStatic = false, Name = \"Name\", Selector = \"name\", PropertyType = typeof (string), GetterSelector = \"name\", SetterSelector = \"setName:\", ArgumentSemantic = ArgumentSemantic.None)]\n"
			];

			yield return [
				new ProtocolMemberData (
					isRequired: true,
					isProperty: true,
					isStatic: false,
					name: "Name",
					selector: "name"
				) {
					PropertyType = ReturnTypeForString (),
					GetterSelector = "myName",
					SetterSelector = "setMyName:",
					ArgumentSemantic = ArgumentSemantic.None,
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [],
				},
				"[ProtocolMember (IsRequired = true, IsProperty = true, IsStatic = false, Name = \"Name\", Selector = \"name\", PropertyType = typeof (string), GetterSelector = \"myName\", SetterSelector = \"setMyName:\", ArgumentSemantic = ArgumentSemantic.None)]\n"
			];

			yield return [
				new ProtocolMemberData (
					isRequired: true,
					isProperty: true,
					isStatic: false,
					name: "Name",
					selector: "name"
				) {
					PropertyType = ReturnTypeForString (),
					GetterSelector = "name",
					SetterSelector = "setName:",
					ArgumentSemantic = ArgumentSemantic.Copy,
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [],
				},
				"[ProtocolMember (IsRequired = true, IsProperty = true, IsStatic = false, Name = \"Name\", Selector = \"name\", PropertyType = typeof (string), GetterSelector = \"name\", SetterSelector = \"setName:\", ArgumentSemantic = ArgumentSemantic.Copy)]\n"
			];

			yield return [
				new ProtocolMemberData (
					isRequired: true,
					isProperty: true,
					isStatic: false,
					name: "Name",
					selector: "callback"
				) {
					PropertyType = ReturnTypeForAction (),
					GetterSelector = "callback",
					SetterSelector = "setCallback:",
					ArgumentSemantic = ArgumentSemantic.None,
					ReturnTypeDelegateProxy = TypeInfo.CreateDelegateProxy (ReturnTypeForAction ()),
					ParameterBlockProxy = [TypeInfo.CreateDelegateProxy (ReturnTypeForAction ())],
				},
				"[ProtocolMember (IsRequired = true, IsProperty = true, IsStatic = false, Name = \"Name\", Selector = \"callback\", PropertyType = typeof (global::System.Action), GetterSelector = \"callback\", SetterSelector = \"setCallback:\", ArgumentSemantic = ArgumentSemantic.None, ReturnTypeDelegateProxy = typeof (global::ObjCRuntime.Trampolines.SDAction), ParameterBlockProxy = new Type? [] { typeof (global::ObjCRuntime.Trampolines.SDAction) })]\n"
			];

			yield return [
				new ProtocolMemberData (
					isRequired: true,
					isProperty: true,
					isStatic: false,
					name: "Name",
					selector: "callback"
				) {
					PropertyType = ReturnTypeForAction (null, "string", "string"),
					GetterSelector = "callback",
					SetterSelector = "setCallback:",
					ArgumentSemantic = ArgumentSemantic.None,
					ReturnTypeDelegateProxy = TypeInfo.CreateDelegateProxy (ReturnTypeForAction (null, "string", "string")),
					ParameterBlockProxy = [TypeInfo.CreateDelegateProxy (ReturnTypeForAction (null, "string", "string"))],
				},
				"[ProtocolMember (IsRequired = true, IsProperty = true, IsStatic = false, Name = \"Name\", Selector = \"callback\", PropertyType = typeof (global::System.Action<string, string>), GetterSelector = \"callback\", SetterSelector = \"setCallback:\", ArgumentSemantic = ArgumentSemantic.None, ReturnTypeDelegateProxy = typeof (global::ObjCRuntime.Trampolines.SDActionArity2stringstring), ParameterBlockProxy = new Type? [] { typeof (global::ObjCRuntime.Trampolines.SDActionArity2stringstring) })]\n"
			];

			yield return [
				new ProtocolMemberData (
					isRequired: true,
					isProperty: false,
					isStatic: false,
					name: "MyMethod",
					selector: "completeRequestReturningItems:completionHandler:"
				) {
					ReturnType = TypeInfo.Void,
					ParameterType = [ReturnTypeForString ()],
					ParameterByRef = [false],
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [null],
				},
				"[ProtocolMember (IsRequired = true, IsProperty = false, IsStatic = false, Name = \"MyMethod\", Selector = \"completeRequestReturningItems:completionHandler:\", ReturnType = typeof (void), ParameterType = new Type [] { typeof (string) }, ParameterByRef = new bool [] { false }, ParameterBlockProxy = new Type? [] { null })]\n"
			];

			yield return [
				new ProtocolMemberData (
					isRequired: true,
					isProperty: false,
					isStatic: true,
					name: "MyMethod",
					selector: "completeRequestReturningItems:completionHandler:"
				) {
					ReturnType = TypeInfo.Void,
					ParameterType = [ReturnTypeForString ()],
					ParameterByRef = [false],
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [null],
				},
				"[ProtocolMember (IsRequired = true, IsProperty = false, IsStatic = true, Name = \"MyMethod\", Selector = \"completeRequestReturningItems:completionHandler:\", ReturnType = typeof (void), ParameterType = new Type [] { typeof (string) }, ParameterByRef = new bool [] { false }, ParameterBlockProxy = new Type? [] { null })]\n"
			];

			yield return [
				new ProtocolMemberData (
					isRequired: true,
					isProperty: false,
					isStatic: false,
					name: "MyMethod",
					selector: "completeRequestReturningItems:completionHandler:"
				) {
					ReturnType = ReturnTypeForInt (),
					ParameterType = [ReturnTypeForString ()],
					ParameterByRef = [false],
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [null],
				},
				"[ProtocolMember (IsRequired = true, IsProperty = false, IsStatic = false, Name = \"MyMethod\", Selector = \"completeRequestReturningItems:completionHandler:\", ReturnType = typeof (int), ParameterType = new Type [] { typeof (string) }, ParameterByRef = new bool [] { false }, ParameterBlockProxy = new Type? [] { null })]\n"
			];

			yield return [
				new ProtocolMemberData (
					isRequired: true,
					isProperty: false,
					isStatic: false,
					name: "MyMethod",
					selector: "completeRequestReturningItems:completionHandler:"
				) {
					ReturnType = ReturnTypeForInt (),
					ParameterType = [ReturnTypeForString (), ReturnTypeForString ()],
					ParameterByRef = [false, false],
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [null, null],
				},
				"[ProtocolMember (IsRequired = true, IsProperty = false, IsStatic = false, Name = \"MyMethod\", Selector = \"completeRequestReturningItems:completionHandler:\", ReturnType = typeof (int), ParameterType = new Type [] { typeof (string), typeof (string) }, ParameterByRef = new bool [] { false, false }, ParameterBlockProxy = new Type? [] { null, null })]\n"
			];

			yield return [
				new ProtocolMemberData (
					isRequired: true,
					isProperty: false,
					isStatic: false,
					name: "MyMethod",
					selector: "completeRequestReturningItems:completionHandler:"
				) {
					ReturnType = ReturnTypeForInt (),
					ParameterType = [ReturnTypeForString ()],
					ParameterByRef = [false],
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [null],
				},
				"[ProtocolMember (IsRequired = true, IsProperty = false, IsStatic = false, Name = \"MyMethod\", Selector = \"completeRequestReturningItems:completionHandler:\", ReturnType = typeof (int), ParameterType = new Type [] { typeof (string) }, ParameterByRef = new bool [] { false }, ParameterBlockProxy = new Type? [] { null })]\n"
			];

			yield return [
				new ProtocolMemberData (
					isRequired: true,
					isProperty: false,
					isStatic: false,
					name: "MyMethod",
					selector: "completeRequestReturningItems:completionHandler:"
				) {
					ReturnType = ReturnTypeForInt (),
					ParameterType = [ReturnTypeForString (), ReturnTypeForString ()],
					ParameterByRef = [false, true],
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [null, null],
				},
				"[ProtocolMember (IsRequired = true, IsProperty = false, IsStatic = false, Name = \"MyMethod\", Selector = \"completeRequestReturningItems:completionHandler:\", ReturnType = typeof (int), ParameterType = new Type [] { typeof (string), typeof (string) }, ParameterByRef = new bool [] { false, true }, ParameterBlockProxy = new Type? [] { null, null })]\n"
			];

			yield return [
				new ProtocolMemberData (
					isRequired: true,
					isProperty: false,
					isStatic: false,
					name: "MyMethod",
					selector: "completeRequestReturningItems:completionHandler:"
				) {
					ReturnType = TypeInfo.Void,
					ParameterType = [ReturnTypeForString ()],
					ParameterByRef = [false],
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [null],
					IsVariadic = true,
				},
				"[ProtocolMember (IsRequired = true, IsProperty = false, IsStatic = false, Name = \"MyMethod\", Selector = \"completeRequestReturningItems:completionHandler:\", ReturnType = typeof (void), ParameterType = new Type [] { typeof (string) }, ParameterByRef = new bool [] { false }, ParameterBlockProxy = new Type? [] { null })]\n"
			];

			yield return [
				new ProtocolMemberData (
					isRequired: true,
					isProperty: false,
					isStatic: false,
					name: "MyMethod",
					selector: "completeRequestReturningItems:completionHandler:"
				) {
					ReturnType = ReturnTypeForAction (),
					ParameterType = [ReturnTypeForString ()],
					ParameterByRef = [false],
					ReturnTypeDelegateProxy = TypeInfo.CreateDelegateProxy (ReturnTypeForAction ()),
					ParameterBlockProxy = [null],
				},
				"[ProtocolMember (IsRequired = true, IsProperty = false, IsStatic = false, Name = \"MyMethod\", Selector = \"completeRequestReturningItems:completionHandler:\", ReturnType = typeof (global::System.Action), ParameterType = new Type [] { typeof (string) }, ParameterByRef = new bool [] { false }, ReturnTypeDelegateProxy = typeof (global::ObjCRuntime.Trampolines.SDAction), ParameterBlockProxy = new Type? [] { null })]\n"
			];

			yield return [
				new ProtocolMemberData (
					isRequired: true,
					isProperty: false,
					isStatic: false,
					name: "MyMethod",
					selector: "completeRequestReturningItems:completionHandler:"
				) {
					ReturnType = ReturnTypeForInt (),
					ParameterType = [ReturnTypeForString (), ReturnTypeForAction ()],
					ParameterByRef = [false, false],
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [null, TypeInfo.CreateDelegateProxy (ReturnTypeForAction ())],
				},
				"[ProtocolMember (IsRequired = true, IsProperty = false, IsStatic = false, Name = \"MyMethod\", Selector = \"completeRequestReturningItems:completionHandler:\", ReturnType = typeof (int), ParameterType = new Type [] { typeof (string), typeof (global::System.Action) }, ParameterByRef = new bool [] { false, false }, ParameterBlockProxy = new Type? [] { null, typeof (global::ObjCRuntime.Trampolines.SDAction) })]\n"
			];

			yield return [
				new ProtocolMemberData (
					isRequired: false,
					isProperty: false,
					isStatic: false,
					name: "MyMethod",
					selector: "completeRequestReturningItems:completionHandler:"
				) {
					ReturnType = TypeInfo.Void,
					ParameterType = [ReturnTypeForString ()],
					ParameterByRef = [false],
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [null],
					IsVariadic = false,
				},
				"[ProtocolMember (IsRequired = false, IsProperty = false, IsStatic = false, Name = \"MyMethod\", Selector = \"completeRequestReturningItems:completionHandler:\", ReturnType = typeof (void), ParameterType = new Type [] { typeof (string) }, ParameterByRef = new bool [] { false }, ParameterBlockProxy = new Type? [] { null })]\n"
			];
		}
	}

	[Theory]
	[MemberData (nameof (AppendProtocolMemberDataTestData))]
	void AppendProtocolMemberDataTest (ProtocolMemberData protocolMemberData, string expectedString)
	{
		var block = new TabbedStringBuilder (sb);
		block.AppendProtocolMemberData (protocolMemberData);
		var result = block.ToCode ();
		Assert.Equal (expectedString, result);
	}

	public static IEnumerable<object []> AppendDynamicDependencyAttributeWithMethodTestData {
		get {
			// Method with no parameters
			var methodNoParams = new Method (
				type: "NS.MyClass",
				name: "MyMethod",
				returnType: ReturnTypeForVoid (),
				symbolAvailability: new (),
				exportMethodData: new ("selector", ArgumentSemantic.None, ObjCBindings.Method.Default),
				attributes: [],
				modifiers: [Token (SyntaxKind.PublicKeyword)],
				parameters: []
			);
			yield return [methodNoParams, "[DynamicDependency (\"MyMethod()\")]\n"];

			// Method with one parameter
			var methodOneParam = new Method (
				type: "NS.MyClass",
				name: "MyMethod",
				returnType: ReturnTypeForVoid (),
				symbolAvailability: new (),
				exportMethodData: new ("selector", ArgumentSemantic.None, ObjCBindings.Method.Default),
				attributes: [],
				modifiers: [Token (SyntaxKind.PublicKeyword)],
				parameters: [new Parameter (0, ReturnTypeForString (), "param1")]
			);
			yield return [methodOneParam, "[DynamicDependency (\"MyMethod(string)\")]\n"];

			// Method with multiple parameters
			var methodMultipleParams = new Method (
				type: "NS.MyClass",
				name: "MyMethod",
				returnType: ReturnTypeForVoid (),
				symbolAvailability: new (),
				exportMethodData: new ("selector", ArgumentSemantic.None, ObjCBindings.Method.Default),
				attributes: [],
				modifiers: [Token (SyntaxKind.PublicKeyword)],
				parameters: [
					new Parameter (0, ReturnTypeForString (), "param1"),
					new Parameter (1, ReturnTypeForInt (), "param2")
				]
			);
			yield return [methodMultipleParams, "[DynamicDependency (\"MyMethod(string,int)\")]\n"];

			// Extension method (with 'this' parameter)
			var extensionMethod = new Method (
				type: "NS.MyClass",
				name: "MyExtensionMethod",
				returnType: ReturnTypeForVoid (),
				symbolAvailability: new (),
				exportMethodData: new ("selector", ArgumentSemantic.None, ObjCBindings.Method.Default),
				attributes: [],
				modifiers: [SyntaxFactory.Token (SyntaxKind.PublicKeyword)],
				parameters: [
					new Parameter (0, ReturnTypeForString (), "self") { IsThis = true },
					new Parameter (1, ReturnTypeForInt (), "param1")
				]
			);
			yield return [extensionMethod, "[DynamicDependency (\"MyExtensionMethod(string,int)\")]\n"];
		}
	}

	[Theory]
	[MemberData (nameof (AppendDynamicDependencyAttributeWithMethodTestData))]
	void AppendDynamicDependencyAttributeWithMethodTests (Method method, string expectedString)
	{
		var block = new TabbedStringBuilder (sb);
		block.AppendDynamicDependencyAttribute (method);
		var result = block.ToCode ();
		Assert.Equal (expectedString, result);
	}

	public static IEnumerable<object []> AppendExportMethodAttributeTestData {
		get {
			// Simple selector
			yield return [new ExportData<ObjCBindings.Method> ("mySelector:"), "[Export<Method> (\"mySelector:\")]\n"];

			// Selector with multiple parts
			yield return [new ExportData<ObjCBindings.Method> ("mySelector:withObject:"), "[Export<Method> (\"mySelector:withObject:\")]\n"];

			// Selector with no parameters
			yield return [new ExportData<ObjCBindings.Method> ("mySelector"), "[Export<Method> (\"mySelector\")]\n"];

			// Null selector
			yield return [new ExportData<ObjCBindings.Method> (null), "[Export<Method> (\"\")]\n"];
		}
	}

	[Theory]
	[MemberData (nameof (AppendExportMethodAttributeTestData))]
	void AppendExportMethodAttributeTests (ExportData<ObjCBindings.Method> exportData, string expectedString)
	{
		var block = new TabbedStringBuilder (sb);
		block.AppendExportAttribute (exportData);
		var result = block.ToCode ();
		Assert.Equal (expectedString, result);
	}

	public static IEnumerable<object []> AppendExportPropertyAttributeTestData {
		get {
			// Simple selector
			yield return [new ExportData<ObjCBindings.Property> ("myProperty"), "[Export<Property> (\"myProperty\")]\n"];

			// Selector with underscore
			yield return [new ExportData<ObjCBindings.Property> ("_myProperty"), "[Export<Property> (\"_myProperty\")]\n"];

			// Null selector
			yield return [new ExportData<ObjCBindings.Property> (null), "[Export<Property> (\"\")]\n"];
		}
	}

	[Theory]
	[MemberData (nameof (AppendExportPropertyAttributeTestData))]
	void AppendExportPropertyAttributeTests (ExportData<ObjCBindings.Property> exportData, string expectedString)
	{
		var block = new TabbedStringBuilder (sb);
		block.AppendExportAttribute (exportData);
		var result = block.ToCode ();
		Assert.Equal (expectedString, result);
	}

	[Fact]
	public void ClearTests ()
	{
		var block = new TabbedStringBuilder (sb);
		var line = "My Line";
		block.Write (line);
		Assert.Equal (line, block.ToCode ());
		block.Clear ();
		Assert.Equal (string.Empty, block.ToCode ());
	}

	[Fact]
	public void CreateBlockStringArray ()
	{
		var expecteString =
@"using (var m1 = new MemoryStream())
using (var m2 = new MemoryStream())
using (var m3 = new MemoryStream())
{
	// this is an example with several usings
}
";
		var baseBlock = new TabbedStringBuilder (sb);
		// create a list of lines to get the new block
		var usingStatements = new [] {
			"using (var m1 = new MemoryStream())",
			"using (var m2 = new MemoryStream())",
			"using (var m3 = new MemoryStream())",
		};
		using (var usingBlock = baseBlock.CreateBlock (usingStatements, true)) {
			usingBlock.WriteLine ("// this is an example with several usings");
		}
		Assert.Equal (expecteString, baseBlock.ToCode ());
	}

	[Fact]
	public void WriteBlockForExpressions ()
	{
		// create an expression list and ensure that the final bloc is correct
		var expectedString =
@"public void Test ()
{
	Single? __xamarin_nullified__1 = null;
	if (value is not null)
		__xamarin_nullified__1 = *value;
}
";
		var baseBlock = new TabbedStringBuilder (sb);
		var members = new List<SyntaxNode> {
			LocalDeclarationStatement(
				VariableDeclaration(
						NullableType(
							IdentifierName("Single")))
					.WithVariables(
						SingletonSeparatedList<VariableDeclaratorSyntax>(
							VariableDeclarator(
									Identifier("__xamarin_nullified__1"))
								.WithInitializer(
									EqualsValueClause(
										LiteralExpression(
											SyntaxKind.NullLiteralExpression)))))),
			IfStatement(
				IsPatternExpression(
					IdentifierName("value"),
					UnaryPattern(
						ConstantPattern(
							LiteralExpression(
								SyntaxKind.NullLiteralExpression)))),
				ExpressionStatement(
					AssignmentExpression(
						SyntaxKind.SimpleAssignmentExpression,
						IdentifierName("__xamarin_nullified__1"),
						PrefixUnaryExpression(
							SyntaxKind.PointerIndirectionExpression,
							IdentifierName("value"))).WithLeadingTrivia (Whitespace ("\t"))))
			};

		using (var methodBlock = baseBlock.CreateBlock ("public void Test ()", true)) {
			methodBlock.Write (members);
		}

		Assert.Equal (expectedString, baseBlock.ToCode ());
	}


}
