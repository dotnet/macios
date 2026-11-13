// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.Emitters;
using Microsoft.Macios.Generator.Formatters;
using Xunit;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.Macios.Generator.Emitters.BindingSyntaxFactory;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

#pragma warning disable APL0003

namespace Microsoft.Macios.Generator.Tests.Emitters;

public class NativeObjectFactoryTests {
	const string factorySource = """
		#nullable enable
		namespace ObjCRuntime {
			public struct NativeHandle {
				public static NativeHandle Zero => default;
				public static implicit operator System.IntPtr (NativeHandle handle) => default;
			}
			public interface INativeObject { }
			public interface ICreatableNativeObject<T> : INativeObject
				where T : INativeObject, ICreatableNativeObject<T> {
				static abstract T Create (System.IntPtr handle, bool owns);
			}
			public static class Runtime {
				public static NativeHandle RetainAndAutoreleaseNativeObject (INativeObject? value) => default;
				public static NativeHandle RetainAndAutoreleaseNSObject (Foundation.NSObject? value) => default;
				public static T? GetCreatableNativeObject<T> (System.IntPtr handle, bool owns)
					where T : INativeObject, ICreatableNativeObject<T>
					=> handle == System.IntPtr.Zero ? default : T.Create (handle, owns);
				public static T? GetINativeObject<T> (System.IntPtr handle, bool owns = false)
					where T : INativeObject => default;
				public static T? GetINativeObject<T> (System.IntPtr handle, bool forcedType, bool owns)
					where T : INativeObject => default;
				public static T? GetNSObject<T> (System.IntPtr handle)
					where T : Foundation.NSObject => default;
			}
		}
		namespace Foundation {
			public class NSObject : ObjCRuntime.INativeObject { }
			public class NSArray : NSObject {
				public static NSArray FromNSObjects (params ObjCRuntime.INativeObject[] values) => new NSArray ();
				public static T?[]? ArrayFromHandle<T> (System.IntPtr handle,
					System.Converter<System.IntPtr, T> creator, bool releaseHandle = false) => default;
			}
		}
		""";

	const string wrapperSource = """
		using ObjCRuntime;
		namespace ThirdParty {
			public class Native : ICreatableNativeObject<Native> {
				static Native ICreatableNativeObject<Native>.Create (System.IntPtr handle, bool owns) => new Native ();
			}
			public class Inherited : Native { }
			public class SelfTyped : Native, ICreatableNativeObject<SelfTyped> {
				static SelfTyped ICreatableNativeObject<SelfTyped>.Create (System.IntPtr handle, bool owns) => new SelfTyped ();
			}
			public class Plain : INativeObject { }
			public abstract class Abstract : ICreatableNativeObject<Abstract> {
				static Abstract ICreatableNativeObject<Abstract>.Create (System.IntPtr handle, bool owns) => throw new System.NotImplementedException ();
			}
			public class ObjectWrapper : Foundation.NSObject, ICreatableNativeObject<ObjectWrapper> {
				static ObjectWrapper ICreatableNativeObject<ObjectWrapper>.Create (System.IntPtr handle, bool owns) => new ObjectWrapper ();
			}
			public interface IProtocol : INativeObject { }
			public interface IFactoryProtocol : ICreatableNativeObject<IFactoryProtocol> {
				static IFactoryProtocol ICreatableNativeObject<IFactoryProtocol>.Create (System.IntPtr handle, bool owns)
					=> throw new System.NotImplementedException ();
			}
			public class Generic<T> : ICreatableNativeObject<Generic<T>> {
				static Generic<T> ICreatableNativeObject<Generic<T>>.Create (System.IntPtr handle, bool owns) => new Generic<T> ();
			}
		}
		namespace Other {
			public interface ICreatableNativeObject<T> { }
			public class Impostor : INativeObject, ICreatableNativeObject<Impostor> { }
		}
		""";

	static readonly Lazy<Compilation> compilation = new (() => {
		var runtime = CreateCompilation ("FactoryRuntime", factorySource);
		var runtimeReference = EmitReference (runtime);
		var wrappers = CreateCompilation ("ThirdParty", wrapperSource, runtimeReference);
		return CreateCompilation ("Consumer", """
			public class Consumer<T> {
				public ThirdParty.Native Native;
				public ThirdParty.Native[] Array;
				public ThirdParty.Generic<int> Closed;
				public ThirdParty.Generic<T> Open;
				public ThirdParty.Generic<T[]> OpenArrayArgument;
				public T Parameter;
			}
			public class Constrained<T> where T : ObjCRuntime.INativeObject, ObjCRuntime.ICreatableNativeObject<T> { }
			""", runtimeReference, EmitReference (wrappers));
	});

	static CSharpCompilation CreateCompilation (string name, string source, params MetadataReference [] references)
		=> CSharpCompilation.Create (name, [CSharpSyntaxTree.ParseText (source, new CSharpParseOptions (LanguageVersion.Preview))],
			references.Prepend (MetadataReference.CreateFromFile (typeof (object).Assembly.Location)),
			new CSharpCompilationOptions (OutputKind.DynamicallyLinkedLibrary,
				allowUnsafe: true, nullableContextOptions: NullableContextOptions.Enable));

	static MetadataReference EmitReference (Compilation source)
	{
		using var stream = new MemoryStream ();
		var result = source.Emit (stream);
		Assert.True (result.Success, string.Join ("\n", result.Diagnostics));
		return MetadataReference.CreateFromImage (stream.ToArray ());
	}

	static TypeInfo GetTypeInfo (string name = "ThirdParty.Native")
		=> new (compilation.Value.GetTypeByMetadataName (name) ?? throw new InvalidOperationException (name));

	static TypeInfo GetFieldTypeInfo (string name)
		=> new (((IFieldSymbol) compilation.Value.GetTypeByMetadataName ("Consumer`1")!.GetMembers (name).Single ()).Type);

	static string Text (SyntaxNode node)
		=> node.NormalizeWhitespace ().ToFullString ();

	static Property GetProperty (TypeInfo type)
		=> new ("Value", type, default, [], [], []) {
			ExportStrongPropertyData = new ExportData<ObjCBindings.StrongDictionaryProperty> ("Key"),
		};

	[Theory]
	[InlineData ("ThirdParty.Native", true)]
	[InlineData ("ThirdParty.Inherited", false)]
	[InlineData ("ThirdParty.SelfTyped", true)]
	[InlineData ("ThirdParty.Plain", false)]
	[InlineData ("ThirdParty.Abstract", false)]
	[InlineData ("ThirdParty.ObjectWrapper", false)]
	[InlineData ("ThirdParty.IProtocol", false)]
	[InlineData ("ThirdParty.IFactoryProtocol", false)]
	[InlineData ("ThirdParty.Generic`1", false)]
	[InlineData ("Other.Impostor", false)]
	public void SelectsOnlyConcreteSelfTypedFactories (string name, bool expected)
		=> Assert.Equal (expected, GetTypeInfo (name).IsCreatableNativeObject);

	[Theory]
	[InlineData ("Closed", true)]
	[InlineData ("Open", false)]
	[InlineData ("OpenArrayArgument", false)]
	[InlineData ("Parameter", false)]
	public void RequiresClosedConcreteType (string field, bool expected)
		=> Assert.Equal (expected, GetFieldTypeInfo (field).IsCreatableNativeObject);

	[Fact]
	public void RejectsFactoryConstrainedTypeParameters ()
	{
		var type = compilation.Value.GetTypeByMetadataName ("Constrained`1")!.TypeParameters [0];
		Assert.False (new TypeInfo (type).IsCreatableNativeObject);
	}

	[Fact]
	public void RecognizesSourceFactoryImplementation ()
	{
		var source = CreateCompilation ("SourceWrapper", wrapperSource,
			EmitReference (CreateCompilation ("FactoryRuntime", factorySource)));
		Assert.True (new TypeInfo (source.GetTypeByMetadataName ("ThirdParty.Native")!).IsCreatableNativeObject);
	}

	[Fact]
	public void PreservesArrayElementFactory ()
	{
		var array = GetFieldTypeInfo ("Array");
		Assert.False (array.IsCreatableNativeObject);
		Assert.True (array.ArrayElementIsCreatableNativeObject);
		Assert.True (array.ToArrayElementType ().IsCreatableNativeObject);
	}

	[Theory]
	[InlineData (false, false)]
	[InlineData (true, false)]
	[InlineData (false, true)]
	[InlineData (true, true)]
	public void ConvertsReturns (bool nullable, bool owns)
	{
		var info = new ReturnInfo { Type = GetTypeInfo ().WithNullable (nullable), ReleaseHandle = owns };
		var expression = ConvertToManaged (info, IdentifierName ("ptr"));
		var expected = $"global::ObjCRuntime.Runtime.GetCreatableNativeObject<global::ThirdParty.Native>(ptr, {owns.ToString ().ToLowerInvariant ()})";
		Assert.Equal (expected + (nullable ? "" : "!"), Text (expression!));
		AssertGeneratedExpressionCompiles (expression!);
	}

	[Theory]
	[InlineData ("ThirdParty.Inherited", "GetINativeObject")]
	[InlineData ("ThirdParty.Plain", "GetINativeObject")]
	[InlineData ("ThirdParty.IProtocol", "GetINativeObject")]
	[InlineData ("ThirdParty.ObjectWrapper", "GetNSObject")]
	public void PreservesFallback (string type, string method)
	{
		var expression = ConvertToManaged (new ReturnInfo { Type = GetTypeInfo (type), ReleaseHandle = false }, IdentifierName ("ptr"));
		Assert.Contains ($".{method}<", Text (expression!));
		Assert.DoesNotContain ("GetCreatableNativeObject", Text (expression!));
	}

	[Theory]
	[InlineData (false, false)]
	[InlineData (false, true)]
	[InlineData (true, false)]
	[InlineData (true, true)]
	public void ConvertsCallbackAndForcedOwnership (bool nullable, bool owns)
	{
		var parameter = new DelegateParameter (0, GetTypeInfo ().WithNullable (nullable), "ptr") {
			ForcedType = new ForcedTypeData (owns),
		};
		var argument = GetTrampolineInvokeArgument ("Callback", parameter);
		Assert.Equal ($"global::ObjCRuntime.Runtime.GetCreatableNativeObject<global::ThirdParty.Native>(ptr, {owns.ToString ().ToLowerInvariant ()})!",
			Text (argument));
		AssertGeneratedExpressionCompiles (argument.Expression);
	}

	[Theory]
	[InlineData (false)]
	[InlineData (true)]
	public void ConvertsRefOutNativeArguments (bool array)
	{
		var parameter = new ArgumentInfo {
			Name = "value",
			Type = array ? GetFieldTypeInfo ("Array") : GetTypeInfo (),
			IsByRef = true,
			ReferenceKind = ReferenceKind.Out,
		};
		var conversion = Assert.Single (GetPostNativeInvokeArgumentConversions (parameter));
		Assert.Contains ("GetCreatableNativeObject<global::ThirdParty.Native>", Text (conversion));
		if (array)
			Assert.Contains ("NSArray.ArrayFromHandle", Text (conversion));
	}

	[Theory]
	[InlineData (false)]
	[InlineData (true)]
	public void ConvertsFactoryArraysWithNullAwareConverter (bool nullable)
	{
		var type = GetFieldTypeInfo ("Array").WithNullable (nullable);
		var conversion = ConvertToManaged (new ReturnInfo { Type = type }, IdentifierName ("ptr"));
		Assert.Equal ("global::Foundation.NSArray.ArrayFromHandle<global::ThirdParty.Native>(ptr, __xamarin_nativeHandle => global::ObjCRuntime.Runtime.GetCreatableNativeObject<global::ThirdParty.Native>(__xamarin_nativeHandle, false)!)" +
			(nullable ? "" : "!"), Text (conversion!));
		AssertGeneratedExpressionCompiles (conversion!);
		var parameter = new DelegateParameter (0, type, "ptr");
		Assert.Contains ("NSArray.ArrayFromHandle", Text (GetTrampolineInvokeArgument ("Callback", parameter)));
	}

	[Fact]
	public void ConvertsClosedGenericFactoryArrays ()
	{
		var closedType = ((IFieldSymbol) compilation.Value.GetTypeByMetadataName ("Consumer`1")!.GetMembers ("Closed").Single ()).Type;
		var type = new TypeInfo (compilation.Value.CreateArrayTypeSymbol (closedType));
		Assert.True (type.ArrayElementIsCreatableNativeObject);
		var conversion = ConvertToManaged (new ReturnInfo { Type = type }, IdentifierName ("ptr"));
		Assert.Contains ("GetCreatableNativeObject<global::ThirdParty.Generic<int>>", Text (conversion!));
		AssertGeneratedExpressionCompiles (conversion!);
	}

	[Theory]
	[InlineData (false)]
	[InlineData (true)]
	public void PreservesNullableCallbackContract (bool nullable)
	{
		var argument = GetTrampolineInvokeArgument ("Callback",
			new DelegateParameter (0, GetTypeInfo ().WithNullable (nullable), "ptr"));
		Assert.Equal ($"global::ObjCRuntime.Runtime.GetCreatableNativeObject<global::ThirdParty.Native>(ptr, false){(nullable ? "" : "!")}",
			Text (argument));
		AssertGeneratedExpressionCompiles (argument.Expression);
	}

	[Theory]
	[InlineData (false)]
	[InlineData (true)]
	public void ProtocolsKeepRuntimeFallback (bool owns)
	{
		var parameter = new DelegateParameter (0, GetTypeInfo ("ThirdParty.IProtocol") with { IsProtocol = true }, "ptr") {
			ForcedType = new ForcedTypeData (owns),
		};
		Assert.Equal ("global::ObjCRuntime.Runtime.GetINativeObject<global::ThirdParty.IProtocol>(ptr, false)!",
			Text (GetTrampolineInvokeArgument ("Callback", parameter)));
	}

	[Theory]
	[InlineData ("ThirdParty.Inherited")]
	[InlineData ("ThirdParty.Plain")]
	public void NonFactoriesKeepForcedTypeAndOwnership (string type)
	{
		var parameter = new DelegateParameter (0, GetTypeInfo (type), "ptr") {
			ForcedType = new ForcedTypeData (true),
		};
		var argument = GetTrampolineInvokeArgument ("Callback", parameter);
		Assert.Equal ($"global::ObjCRuntime.Runtime.GetINativeObject<global::{type}>(ptr, true, true)!",
			Text (argument));
		AssertGeneratedExpressionCompiles (argument.Expression);
	}

	[Theory]
	[InlineData (false)]
	[InlineData (true)]
	public void ArrayConverterPreservesReleaseArgument (bool release)
	{
		var conversion = GetCFArrayFromHandle (GetTypeInfo (), [Argument (IdentifierName ("ptr")), BoolArgument (release)]);
		Assert.EndsWith ($"!, {release.ToString ().ToLowerInvariant ()})", Text (conversion));
		AssertGeneratedExpressionCompiles (conversion);
	}

	[Theory]
	[InlineData (false, false)]
	[InlineData (true, false)]
	[InlineData (false, true)]
	[InlineData (true, true)]
	public void ConvertsByRefCallbackInputs (bool nullable, bool array)
	{
		var parameter = new DelegateParameter (0,
			(array ? GetFieldTypeInfo ("Array") : GetTypeInfo ()).WithNullable (nullable), "ptr") {
			ReferenceKind = ReferenceKind.Ref,
		};
		var conversions = GetTrampolinePreInvokeByRefArgument (parameter);
		Assert.Contains ("GetCreatableNativeObject<global::ThirdParty.Native>", Text (conversions [1]));
		Assert.Equal ("global::ObjCRuntime.NativeHandle*", Text (GetTrampolineInvokeParameter (parameter).ParameterType));
		var post = Text (Assert.Single (GetTrampolinePostInvokeByRefArgument ("Callback", parameter)));
		Assert.DoesNotContain ("HasValue", post);
		Assert.Contains (array ? "RetainAndAutoreleaseNSObject" : "RetainAndAutoreleaseNativeObject", post);
		if (array)
			Assert.Contains ("is null ? global::ObjCRuntime.NativeHandle.Zero", post);
		var declarationType = GetTrampolineInvokeParameter (parameter).ParameterType;
		var argument = GetTrampolineInvokeArgument ("Callback", parameter);
		var source = $$"""
			public static class Generated {
				static void Callback (ref {{parameter.Type.GetIdentifierSyntax ()}} value) { }
				public static unsafe void Convert ({{declarationType}} ptr) {
					{{string.Join ("\n", conversions.Select (Text))}}
					Callback ({{argument}});
					{{post}}
				}
			}
			""";
		AssertGeneratedSourceCompiles (source);
	}

	[Fact]
	public void DoesNotReadOutCallbackInput ()
	{
		var parameter = new DelegateParameter (0, GetTypeInfo (), "ptr") { ReferenceKind = ReferenceKind.Out };
		Assert.Single (GetTrampolinePreInvokeByRefArgument (parameter));
	}

	[Fact]
	public void ConvertsStrongDictionaryValues ()
	{
		var scalar = GetStrongDictionaryInvocations (GetProperty (GetTypeInfo ())).Getter;
		Assert.Equal ("TryGetNativeValue(Key, out var __xamarin_nativeValue) ? global::ObjCRuntime.Runtime.GetCreatableNativeObject<global::ThirdParty.Native>(__xamarin_nativeValue, false) : null",
			Text (scalar));
		var array = GetStrongDictionaryInvocations (GetProperty (GetFieldTypeInfo ("Array"))).Getter;
		Assert.Contains ("GetArray<global::ThirdParty.Native>(Key, __xamarin_nativeHandle => global::ObjCRuntime.Runtime.GetCreatableNativeObject", Text (array));
	}

	[Fact]
	public void ConvertsNativeFields ()
	{
		var property = GetProperty (GetTypeInfo ()) with {
			ExportFieldData = new (new FieldData<ObjCBindings.Property> ("NativeValue"), "NativeLibrary"),
		};
		var expression = FieldConstantGetter (property);
		Assert.Contains ("GetCreatableNativeObject<global::ThirdParty.Native>", Text (expression));
		Assert.Contains ("Dlfcn.GetIndirect", Text (expression));
		Assert.EndsWith (", false)!", Text (expression));
		Assert.Contains ("SetIntPtr", Text (FieldConstantSetter (property, "value")));
	}

	[Fact]
	public void FactoryEligibilityParticipatesInEquality ()
	{
		var type = GetTypeInfo ();
		Assert.NotEqual (type, type with { IsCreatableNativeObject = false });
		var array = GetFieldTypeInfo ("Array");
		Assert.NotEqual (array, array with { ArrayElementIsCreatableNativeObject = false });
	}

	static void AssertGeneratedExpressionCompiles (SyntaxNode expression)
	{
		var source = $"public static class Generated {{ public static object? Convert (System.IntPtr ptr) => {expression}; }}";
		AssertGeneratedSourceCompiles (source);
	}

	static void AssertGeneratedSourceCompiles (string source)
	{
		var generated = CreateCompilation ("Generated", source,
			compilation.Value.References.Where (r => r.Display != typeof (object).Assembly.Location).ToArray ());
		Assert.Empty (generated.GetDiagnostics ().Where (d => d.Severity == DiagnosticSeverity.Error));
	}
}
