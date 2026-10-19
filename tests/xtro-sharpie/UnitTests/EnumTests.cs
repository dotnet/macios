// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ClangSharp;
using ClangSharp.Interop;
using Extrospection;
using Mono.Cecil;
using Sharpie.Bind;

namespace Xamarin.Tests;

[TestFixture]
[NonParallelizable]
public class EnumTests {
	const string Framework = "EnumTests";

	static void AddAttribute (ModuleDefinition module, Mono.Cecil.ICustomAttributeProvider provider, string name, string? argument = null)
	{
		var attributeType = new TypeReference (name == "ObsoleteAttribute" ? "System" : "Tests", name, module, module);
		var constructor = new MethodReference (".ctor", module.TypeSystem.Void, attributeType) { HasThis = true };
		var attribute = new CustomAttribute (constructor);
		if (argument is not null) {
			constructor.Parameters.Add (new ParameterDefinition (module.TypeSystem.String));
			attribute.ConstructorArguments.Add (new CustomAttributeArgument (module.TypeSystem.String, argument));
		}
		provider.CustomAttributes.Add (attribute);
	}

	static string [] Check (string declarations, (string Name, object Value) [] values, bool unsigned = false, bool native = false, Action<ModuleDefinition, TypeDefinition>? prepare = null)
	{
		var directory = Cache.CreateTemporaryDirectory ();
		var headers = Path.Combine (directory, Framework + ".framework", "Headers");
		Directory.CreateDirectory (headers);
		var header = Path.Combine (headers, Framework + ".h");
		File.WriteAllText (header, declarations);

		using var module = ModuleDefinition.CreateModule (Framework, ModuleKind.Dll);
		var type = new TypeDefinition (Framework, "TestEnum", TypeAttributes.Public | TypeAttributes.Sealed,
			new TypeReference ("System", "Enum", module, module.TypeSystem.CoreLibrary));
		module.Types.Add (type);
		var underlyingType = native
			? (unsigned ? module.TypeSystem.UInt64 : module.TypeSystem.Int64)
			: (unsigned ? module.TypeSystem.UInt32 : module.TypeSystem.Int32);
		type.Fields.Add (new FieldDefinition ("value__", FieldAttributes.Public | FieldAttributes.SpecialName | FieldAttributes.RTSpecialName, underlyingType));
		foreach (var value in values)
			type.Fields.Add (new FieldDefinition (value.Name, FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.HasDefault, type) { Constant = value.Value });
		if (native)
			AddAttribute (module, type, "NativeAttribute");
		prepare?.Invoke (module, type);

		var previousPlatform = Helpers.Platform;
		Helpers.Platform = Platforms.macOS;
		Log.On (Framework).Clear ();
		try {
			using var index = CXIndex.Create ();
			var result = CXTranslationUnit.TryParse (index, header, ["-x", "objective-c"], [],
				CXTranslationUnit_Flags.CXTranslationUnit_IncludeAttributedTypes | CXTranslationUnit_Flags.CXTranslationUnit_VisitImplicitAttributes, out var handle);
			Assert.That (result, Is.EqualTo (CXErrorCode.CXError_Success), "Parsing synthetic header");
			using var unit = TranslationUnit.GetOrCreate (handle);
			Assert.That (unit, Is.Not.Null, "Translation unit");
			Assert.That (unit.Handle.NumDiagnostics, Is.Zero, "Synthetic header diagnostics");
			var check = new EnumCheck (new BindingResult ());
			check.VisitManagedType (type);
			check.Visit (unit.TranslationUnitDecl);
			check.EndVisit ();
			return Log.On (Framework).Distinct ().ToArray ();
		} finally {
			Log.On (Framework).Clear ();
			Helpers.Platform = previousPlatform;
			Directory.Delete (directory, true);
		}
	}

	static string Wrong (string member, object managed, object native) =>
		$"!wrong-enum-value! TestEnum.{member} managed value {managed} does not match native value TestEnum{member} = {native}";

	[Test]
	public void ReorderedImplicitValues ()
	{
		var messages = Check ("""
			enum TestEnum {
				TestEnumPassive,
				TestEnumActive,
				TestEnumTimeSensitive,
				TestEnumCritical
			};
			""", [("Active", 0), ("Critical", 1), ("Passive", 2), ("TimeSensitive", 3)]);
		Assert.That (messages, Is.EquivalentTo (new [] {
			Wrong ("Active", 0, 1),
			Wrong ("Critical", 1, 3),
			Wrong ("Passive", 2, 0),
			Wrong ("TimeSensitive", 3, 2),
		}));
	}

	[Test]
	public void CorrectValuesAndRenamedMembers ()
	{
		var messages = Check ("""
			enum TestEnum {
				TestEnumPassive,
				TestEnumActive,
				TestEnumTimeSensitive,
				TestEnumCritical,
				TestEnumHTTP = 10
			};
			""", [("Passive", 0), ("Active", 1), ("TimeSensitive", 2), ("Urgent", 3), ("Http", 10)]);
		Assert.That (messages, Is.Empty);
	}

	[Test]
	public void ExplicitAndFollowingImplicitValues ()
	{
		var messages = Check ("""
			enum TestEnum {
				TestEnumNegative = -2,
				TestEnumNext,
				TestEnumShift = 1 << 4,
				TestEnumLast
			};
			""", [("Negative", -1), ("Next", -2), ("Shift", 17), ("Last", 16)]);
		Assert.That (messages, Is.EquivalentTo (new [] {
			Wrong ("Negative", -1, -2),
			Wrong ("Next", -2, -1),
			Wrong ("Shift", 17, 16),
			Wrong ("Last", 16, 17),
		}));
	}

	[Test]
	public void AliasesAreCheckedIndividually ()
	{
		var messages = Check ("""
			enum TestEnum {
				TestEnumFirst = 0,
				TestEnumAlias = 0,
				TestEnumSecond = 1
			};
			""", [("First", 1), ("Alias", 0), ("Second", 1)]);
		Assert.That (messages, Is.EqualTo (new [] { Wrong ("First", 1, 0) }));
	}

	[Test]
	public void NativeAliasesAreNotDiscarded ()
	{
		var messages = Check ("""
			enum TestEnum {
				TestEnumFirst = 0,
				TestEnumAlias = 0,
				TestEnumSecond = 1
			};
			""", [("First", 0), ("Alias", 1), ("Second", 1)]);
		Assert.That (messages, Is.EqualTo (new [] { Wrong ("Alias", 1, 0) }));
	}

	[Test]
	public void UnsignedNativeValues ()
	{
		var messages = Check ("""
			typedef unsigned long NSUInteger;
			enum TestEnum : NSUInteger {
				TestEnumLow = 1,
				TestEnumHigh = 1UL << 63
			};
			""", [("Low", 1UL << 63), ("High", 1UL)], unsigned: true, native: true);
		Assert.That (messages, Is.EquivalentTo (new [] {
			Wrong ("Low", 1UL << 63, 1UL),
			Wrong ("High", 1UL, 1UL << 63),
		}));
	}

	[Test]
	public void Unsigned32BitValues ()
	{
		var messages = Check ("""
			enum TestEnum : unsigned int {
				TestEnumLow = 1,
				TestEnumHigh = 1U << 31
			};
			""", [("Low", 1U << 31), ("High", 1U)], unsigned: true);
		Assert.That (messages, Is.EquivalentTo (new [] {
			Wrong ("Low", 1U << 31, 1U),
			Wrong ("High", 1U, 1U << 31),
		}));
	}

	[Test]
	public void NativeAllBitsSetCompatibility ()
	{
		var messages = Check ("""
			typedef unsigned long NSUInteger;
			enum TestEnum : NSUInteger {
				TestEnumAll = 0xffffffff
			};
			""", [("All", ulong.MaxValue)], unsigned: true, native: true);
		Assert.That (messages, Is.Empty);
	}

	[TestCase ("availability(macos,unavailable)")]
	[TestCase ("availability(macos,introduced=10.0,deprecated=10.1)")]
	public void UnavailableOrDeprecatedNativeMember (string attribute)
	{
		var messages = Check ($$"""
			enum TestEnum {
				TestEnumFirst __attribute__(({{attribute}})) = 0,
				TestEnumSecond = 1
			};
			""", [("First", 1), ("Second", 0)], prepare: (module, type) => {
			AddAttribute (module, type.Fields.Single (v => v.Name == "Second"), "UnsupportedOSPlatformAttribute", "macos");
		});
		Assert.That (messages, Is.Empty);
	}

	[TestCase ("ObsoleteAttribute", null)]
	[TestCase ("UnsupportedOSPlatformAttribute", "macos")]
	[TestCase ("ObsoletedOSPlatformAttribute", "macos10.0")]
	public void UnavailableOrObsoleteManagedMember (string attribute, string? argument)
	{
		var messages = Check ("""
			enum TestEnum {
				TestEnumFirst = 0,
				TestEnumSecond = 1
			};
			""", [("First", 1), ("Second", 1)], prepare: (module, type) => {
			AddAttribute (module, type.Fields.Single (v => v.Name == "First"), attribute, argument);
		});
		Assert.That (messages, Is.EqualTo (new [] {
			"!missing-enum-value! TestEnum native value TestEnumFirst = 0 not bound",
		}));
	}

	[Test]
	public void DeprecatedNativeEnum ()
	{
		var messages = Check ("""
			enum __attribute__((availability(macos,introduced=10.0,deprecated=10.1))) TestEnum {
				TestEnumFirst = 0,
				TestEnumSecond = 1
			};
			""", [("First", 1), ("Second", 0)]);
		Assert.That (messages, Is.Empty);
	}

	[Test]
	public void ExistingMissingAndExtraValueDiagnostics ()
	{
		var messages = Check ("""
			enum TestEnum {
				TestEnumFirst = 0,
				TestEnumSecond = 1
			};
			""", [("First", 0), ("Renamed", 2)]);
		Assert.That (messages, Is.EquivalentTo (new [] {
			"!missing-enum-value! TestEnum native value TestEnumSecond = 1 not bound",
			"!extra-enum-value! Managed value 2 for TestEnum.Renamed not found in native headers",
		}));
	}
}
