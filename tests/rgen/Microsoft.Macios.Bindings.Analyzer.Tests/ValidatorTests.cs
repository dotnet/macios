// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
#pragma warning disable RS2008

using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Macios.Generator.Context;
using Xunit;

namespace Microsoft.Macios.Bindings.Analyzer.Tests;

public class ValidatorTests {

	readonly RootContext context;

	public ValidatorTests ()
	{
		// Create a dummy compilation to get a semantic model and RootContext
		var syntaxTree = CSharpSyntaxTree.ParseText ("namespace Test { }");
		var compilation = CSharpCompilation.Create (
			"TestAssembly",
			[syntaxTree],
			references: [],
			options: new CSharpCompilationOptions (OutputKind.DynamicallyLinkedLibrary)
		);
		var semanticModel = compilation.GetSemanticModel (syntaxTree);
		context = new RootContext (semanticModel);
	}

	internal static readonly DiagnosticDescriptor TST0001 = new (
		"TST0001",
		"Example Diagnostic Title",
		"Example Diagnostic Message Format: {0}",
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true
	);

	[Flags]
	public enum MyFlag {
		None = 0,
		OptionA = 1 << 0,
		OptionB = 1 << 1,
		OptionC = 1 << 2
	}

	[Flags]
	public enum OtherFlag {
		None = 0,
		OptionX = 1 << 0,
		OptionY = 1 << 1
	}

	public struct Address {
		public string? Street { get; set; }
		public string? City { get; set; }
	}

	public struct NonNullableAddress {
		public string Street { get; set; }
		public string City { get; set; }

		public static bool IsDefault (NonNullableAddress address) =>
			string.IsNullOrEmpty (address.Street) && string.IsNullOrEmpty (address.City);
	}

	public struct MyData {
		public MyFlag Flag { get; set; }
		public string? OptionalField1 { get; set; }
		public int? OptionalField2 { get; set; }
		public string? OptionalField3 { get; set; }
		public Address? HomeAddress { get; set; }
		public NonNullableAddress WorkAddress { get; set; }
	}

	public struct MyOtherData {
		public OtherFlag Flag { get; set; }
		public string? OptionalField1 { get; set; }
		public Address? HomeAddress { get; set; }
		public NonNullableAddress WorkAddress { get; set; }
	}

	[Theory]
	[InlineData (MyFlag.OptionA, null, true)] // Fails: flag set, field null
	[InlineData (MyFlag.OptionA, "value", false)] // Succeeds: flag set, field not null
	[InlineData (MyFlag.OptionB, null, false)] // Succeeds: flag not set, field null
	[InlineData (MyFlag.OptionB, "value", false)] // Succeeds: flag not set, field not null
	[InlineData (MyFlag.OptionA | MyFlag.OptionB, null, true)] // Fails: flag set, field null
	[InlineData (MyFlag.OptionA | MyFlag.OptionB, "value", false)] // Succeeds: flag set, field not null
	public void RequireWhenTests (MyFlag flag, string? optionalFieldValue, bool shouldFail)
	{
		var validator = new Validator<MyData> ();
		validator.RequireWhen (d => d.OptionalField1, d => d.Flag, requiredFlags: MyFlag.OptionA);

		var data = new MyData {
			Flag = flag,
			OptionalField1 = optionalFieldValue
		};

		var errors = validator.ValidateAll (data, context);

		if (shouldFail) {
			Assert.True (errors.ContainsKey (nameof (MyData.OptionalField1)));
			var diagnostics = errors [nameof (MyData.OptionalField1)];
			Assert.Single (diagnostics);
			Assert.Equal ("RBI0015", diagnostics [0].Id);
		} else {
			Assert.Empty (errors);
		}
	}

	[Theory]
	[InlineData (MyFlag.OptionA, null, true, "RBI0015")] // Fails: RequireWhen fails because field is null
	[InlineData (MyFlag.OptionA, "ab", true, "TST0001")] // Fails: Length check fails
	[InlineData (MyFlag.OptionA, "abcd", false)] // Succeeds: Both checks pass
	[InlineData (MyFlag.OptionB, null, false)] // Succeeds: RequireWhen is not triggered, length check is not triggered
	[InlineData (MyFlag.OptionB, "ab", true, "TST0001")] // Fails: Length check fails
	[InlineData (MyFlag.OptionB, "abcd", false)] // Succeeds: RequireWhen is not triggered, length check passes
	public void RequireWhenWithAdditionalValidationTests (MyFlag flag, string? optionalFieldValue, bool shouldFail, string? diagnosticId = null)
	{
		var validator = new Validator<MyData> ();
		validator.RequireWhen (d => d.OptionalField1, d => d.Flag, requiredFlags: MyFlag.OptionA);
		validator.AddStrategy (
			d => d.OptionalField1,
			TST0001,
			(string? data, RootContext _, out ImmutableArray<Diagnostic> diagnostics, Location? location) => {
				diagnostics = [];
				if (data is not null && data.Length <= 3) {
					diagnostics = [Diagnostic.Create (TST0001, location, "Field length must be > 3")];
					return false;
				}
				return true;
			});

		var data = new MyData {
			Flag = flag,
			OptionalField1 = optionalFieldValue
		};

		var errors = validator.ValidateAll (data, context);

		if (shouldFail) {
			Assert.True (errors.ContainsKey (nameof (MyData.OptionalField1)));
			var diagnostics = errors [nameof (MyData.OptionalField1)];
			Assert.Single (diagnostics);
			Assert.Equal (diagnosticId, diagnostics [0].Id);
		} else {
			Assert.Empty (errors);
		}
	}

	[Theory]
	[InlineData (MyFlag.OptionC, null, true, "HomeAddress", "RBI0015")] // Fails: RequireWhen fails because HomeAddress is null
	[InlineData (MyFlag.OptionC, "", true, "HomeAddress.Street", "TST0001")] // Fails: Nested validation fails for Street (null)
	[InlineData (MyFlag.OptionC, "abc", true, "HomeAddress.Street", "TST0001")] // Fails: Nested validation fails for Street (length)
	[InlineData (MyFlag.OptionC, "abcd", true, "HomeAddress.City", "TST0001")] // Fails: Nested validation fails for City (null)
	[InlineData (MyFlag.OptionC, "abcd,abc", true, "HomeAddress.City", "TST0001")] // Fails: Nested validation fails for City (length)
	[InlineData (MyFlag.OptionC, "abcd,efgh", false)] // Succeeds: All validations pass
	[InlineData (MyFlag.OptionB, null, false)] // Succeeds: RequireWhen not triggered, nested validator not run
	[InlineData (MyFlag.OptionB, "", true, "HomeAddress.Street", "TST0001")] // Fails: Nested validation fails for Street (null)
	public void RequireWhenWithNestedValidatorTests (MyFlag flag, string? addressParts, bool shouldFail, string? errorKey = null, string? diagnosticId = null)
	{
		var addressValidator = new Validator<Address> ();
		addressValidator.AddStrategy (a => a.Street, TST0001, (string? data, RootContext _, out ImmutableArray<Diagnostic> diagnostics, Location? location) => {
			diagnostics = [];
			if (data is null || data.Length <= 3) {
				diagnostics = [Diagnostic.Create (TST0001, location, "Street must be longer than 3 chars.")];
				return false;
			}
			return true;
		});
		addressValidator.AddStrategy (a => a.City, TST0001, (string? data, RootContext _, out ImmutableArray<Diagnostic> diagnostics, Location? location) => {
			diagnostics = [];
			if (data is null || data.Length <= 3) {
				diagnostics = [Diagnostic.Create (TST0001, location, "City must be longer than 3 chars.")];
				return false;
			}
			return true;
		});

		var validator = new Validator<MyData> ();
		validator.RequireWhen (d => d.HomeAddress, d => d.Flag, requiredFlags: MyFlag.OptionC);
		validator.AddNestedValidator (d => d.HomeAddress, addressValidator);

		Address? homeAddress = null;
		if (addressParts is not null) {
			var parts = addressParts.Split (',');
			homeAddress = new Address {
				Street = parts [0] == "" ? null : parts [0],
				City = parts.Length > 1 ? (parts [1] == "" ? null : parts [1]) : null
			};
		}

		var data = new MyData {
			Flag = flag,
			HomeAddress = homeAddress
		};

		var errors = validator.ValidateAll (data, context);

		if (shouldFail) {
			Assert.True (errors.ContainsKey (errorKey!));
			var diagnostics = errors [errorKey!];
			Assert.Single (diagnostics);
			Assert.Equal (diagnosticId, diagnostics [0].Id);
		} else {
			Assert.Empty (errors);
		}
	}

	[Theory]
	[InlineData ("field1", null, null, false, 2)] // Succeeds: OptionalField1 is set with others null
	[InlineData ("field1", "field2", null, true)] // Fails: OptionalField1 and OptionalField2 are both set
	[InlineData ("field1", null, "field3", true)] // Fails: OptionalField1 and OptionalField3 are both set
	[InlineData ("field1", "field2", "field3", true, 2)] // Fails: All fields are set
	[InlineData (null, "field2", null, false)] // Succeeds: Only OptionalField2 is set
	[InlineData (null, null, "field3", false)] // Succeeds: Only OptionalField3 is set
	[InlineData (null, "field2", "field3", false)] // Succeeds: OptionalField2 and OptionalField3 are set, OptionalField1 is not
	[InlineData (null, null, null, false)] // Succeeds: No fields are set
	public void MutuallyExclusiveTests (string? field1Value, string? field2Value, string? field3Value, bool shouldFail, int failureCount = 1)
	{
		var validator = new Validator<MyData> ();
		validator.MutuallyExclusive (
			exactlyOne: false,
			d => d.OptionalField1,
			d => d.OptionalField2
		);

		validator.MutuallyExclusive (
			exactlyOne: false,
			d => d.OptionalField1,
			d => d.OptionalField3
		);

		var data = new MyData {
			OptionalField1 = field1Value,
			OptionalField2 = field2Value == "field2" ? 42 : null,
			OptionalField3 = field3Value
		};

		var errors = validator.ValidateAll (data, context);

		if (shouldFail) {
			Assert.True (errors.ContainsKey (string.Empty)); // Global validation error
			var diagnostics = errors [string.Empty];
			Assert.Equal (failureCount, diagnostics.Count);
			foreach (var diagnostic in diagnostics) {
				Assert.Equal ("RBI0016", diagnostic.Id);
			}
		} else {
			Assert.Empty (errors);
		}
	}

	[Theory]
	[InlineData (null, true, 1, "RBI0015")] // Fails: null value (required validation)
	[InlineData ("ab", true, 1, "TST0001")] // Fails: length check only
	[InlineData ("1234", true, 1, "TST0001")] // Fails: no letter check only
	[InlineData ("12", true, 2)] // Fails: both length and no letter checks
	[InlineData ("abcd", false)] // Succeeds: all validations pass
	[InlineData ("ab1", false)] // Succeeds: has letter and length > 2
	public void MultipleValidationsOnSingleFieldTests (string? fieldValue, bool shouldFail, int expectedErrorCount = 0, string? firstDiagnosticId = null)
	{
		var validator = new Validator<MyData> ();

		validator.RequireWhen (d => d.OptionalField1, d => d.Flag, requiredFlags: MyFlag.OptionA);

		// Add length validation
		validator.AddStrategy (
			d => d.OptionalField1,
			TST0001,
			(string? data, RootContext _, out ImmutableArray<Diagnostic> diagnostics, Location? location) => {
				diagnostics = [];
				if (data is not null && data.Length <= 2) {
					diagnostics = [Diagnostic.Create (TST0001, location, "Field length must be > 2")];
					return false;
				}
				return true;
			});

		// Add letter validation
		validator.AddStrategy (
			d => d.OptionalField1,
			TST0001,
			(string? data, RootContext _, out ImmutableArray<Diagnostic> diagnostics, Location? location) => {
				diagnostics = [];
				if (data is not null && !data.Any (char.IsLetter)) {
					diagnostics = [Diagnostic.Create (TST0001, location, "Field must contain at least one letter")];
					return false;
				}
				return true;
			});

		var data = new MyData {
			Flag = MyFlag.OptionA, // Set flag to trigger RequireWhen
			OptionalField1 = fieldValue
		};

		var errors = validator.ValidateAll (data, context);

		if (shouldFail) {
			Assert.True (errors.ContainsKey (nameof (MyData.OptionalField1)));
			var diagnostics = errors [nameof (MyData.OptionalField1)];
			Assert.Equal (expectedErrorCount, diagnostics.Count);
			if (firstDiagnosticId is not null) {
				Assert.Equal (firstDiagnosticId, diagnostics [0].Id);
			}
		} else {
			Assert.Empty (errors);
		}
	}

	[Theory]
	[InlineData (MyFlag.OptionA, "ab", true)] // Fails: flag set, validation fails (length <= 3)
	[InlineData (MyFlag.OptionA, "abcd", false)] // Succeeds: flag set, validation passes (length > 3)
	[InlineData (MyFlag.OptionA, null, true)] // Fails: flag set, validation fails (null value)
	[InlineData (MyFlag.OptionB, "ab", false)] // Succeeds: flag not set, validation not executed
	[InlineData (MyFlag.OptionB, null, false)] // Succeeds: flag not set, validation not executed
	[InlineData (MyFlag.OptionA | MyFlag.OptionB, "ab", true)] // Fails: flag set, validation fails
	[InlineData (MyFlag.OptionA | MyFlag.OptionB, "abcd", false)] // Succeeds: flag set, validation passes
	public void AddConditionalStrategyTests (MyFlag flag, string? optionalFieldValue, bool shouldFail)
	{
		var validator = new Validator<MyData> ();
		validator.AddConditionalStrategy (
			d => d.OptionalField1,
			d => d.Flag,
			TST0001,
			(string? data, RootContext _, out ImmutableArray<Diagnostic> diagnostics, Location? location) => {
				diagnostics = [];
				if (data is null || data.Length <= 3) {
					diagnostics = [Diagnostic.Create (TST0001, location, "Field must be longer than 3 characters")];
					return false;
				}
				return true;
			},
			requireAllFlags: false,
			MyFlag.OptionA);

		var data = new MyData {
			Flag = flag,
			OptionalField1 = optionalFieldValue
		};

		var errors = validator.ValidateAll (data, context);

		if (shouldFail) {
			Assert.True (errors.ContainsKey (nameof (MyData.OptionalField1)));
			var diagnostics = errors [nameof (MyData.OptionalField1)];
			Assert.Single (diagnostics);
			Assert.Equal ("TST0001", diagnostics [0].Id);
		} else {
			Assert.Empty (errors);
		}
	}

	[Theory]
	[InlineData (MyFlag.OptionC, null, false)] // Succeeds: flag set, but HomeAddress is null (validation not executed)
	[InlineData (MyFlag.OptionC, "", true)] // Fails: flag set, validation fails (Street is null/empty)
	[InlineData (MyFlag.OptionC, "ab", true)] // Fails: flag set, validation fails (Street length <= 3)
	[InlineData (MyFlag.OptionC, "abcd", true)] // Fails: flag set, validation fails (City is null)
	[InlineData (MyFlag.OptionC, "abcd,ab", true)] // Fails: flag set, validation fails (City length <= 3)
	[InlineData (MyFlag.OptionC, "abcd,efgh", false)] // Succeeds: flag set, validation passes
	[InlineData (MyFlag.OptionB, "", false)] // Succeeds: flag not set, validation not executed
	[InlineData (MyFlag.OptionB, "ab", false)] // Succeeds: flag not set, validation not executed
	[InlineData (MyFlag.OptionA | MyFlag.OptionC, "abcd,efgh", false)] // Succeeds: flag set, validation passes
	public void AddConditionalStrategyNullableStructTests (MyFlag flag, string? addressParts, bool shouldFail)
	{
		var validator = new Validator<MyData> ();
		validator.AddConditionalStrategy (
			d => d.HomeAddress,
			d => d.Flag,
			TST0001,
			(Address? data, RootContext _, out ImmutableArray<Diagnostic> diagnostics, Location? location) => {
				diagnostics = [];
				if (data is null)
					return true; // null is valid when validation is triggered

				var address = data.Value;
				if (address.Street is null || address.Street.Length <= 3) {
					diagnostics = [Diagnostic.Create (TST0001, location, "Street must be longer than 3 characters")];
					return false;
				}
				if (address.City is null || address.City.Length <= 3) {
					diagnostics = [Diagnostic.Create (TST0001, location, "City must be longer than 3 characters")];
					return false;
				}
				return true;
			},
			requireAllFlags: false,
			MyFlag.OptionC);

		Address? homeAddress = null;
		if (addressParts is not null) {
			var parts = addressParts.Split (',');
			homeAddress = new Address {
				Street = parts [0] == "" ? null : parts [0],
				City = parts.Length > 1 ? (parts [1] == "" ? null : parts [1]) : null
			};
		}

		var data = new MyData {
			Flag = flag,
			HomeAddress = homeAddress
		};

		var errors = validator.ValidateAll (data, context);

		if (shouldFail) {
			Assert.True (errors.ContainsKey (nameof (MyData.HomeAddress)));
			var diagnostics = errors [nameof (MyData.HomeAddress)];
			Assert.Single (diagnostics);
			Assert.Equal ("TST0001", diagnostics [0].Id);
		} else {
			Assert.Empty (errors);
		}
	}

	[Theory]
	[InlineData (MyFlag.OptionA, "field1", "field2", null, true)] // Fails: flag set, OptionalField1 and OptionalField2 are both set
	[InlineData (MyFlag.OptionA, "field1", null, "field3", true)] // Fails: flag set, OptionalField1 and OptionalField3 are both set
	[InlineData (MyFlag.OptionA, "field1", null, null, false)] // Succeeds: flag set, only OptionalField1 is set
	[InlineData (MyFlag.OptionA, null, "field2", null, false)] // Succeeds: flag set, only OptionalField2 is set
	[InlineData (MyFlag.OptionA, null, null, null, false)] // Succeeds: flag set, no fields are set
	[InlineData (MyFlag.OptionB, "field1", "field2", null, false)] // Succeeds: flag not set, validation not executed
	[InlineData (MyFlag.OptionB, "field1", "field2", "field3", false)] // Succeeds: flag not set, validation not executed
	[InlineData (MyFlag.OptionA | MyFlag.OptionB, "field1", "field2", null, true)] // Fails: flag set, multiple fields are set
	public void AddConditionalMutuallyExclusiveTests (MyFlag flag, string? field1Value, string? field2Value, string? field3Value, bool shouldFail)
	{
		var validator = new Validator<MyData> ();
		validator.AddConditionalMutuallyExclusive (
			d => d.Flag,
			exactlyOne: false,
			requireAllFlags: false,
			requiredFlags: [MyFlag.OptionA],
			d => d.OptionalField1,
			d => d.OptionalField2,
			d => d.OptionalField3
		);

		var data = new MyData {
			Flag = flag,
			OptionalField1 = field1Value,
			OptionalField2 = field2Value == "field2" ? 42 : null,
			OptionalField3 = field3Value
		};

		var errors = validator.ValidateAll (data, context);

		if (shouldFail) {
			Assert.True (errors.ContainsKey (string.Empty)); // Global validation error
			var diagnostics = errors [string.Empty];
			Assert.Single (diagnostics);
			Assert.Equal ("RBI0016", diagnostics [0].Id);
		} else {
			Assert.Empty (errors);
		}
	}

	[Theory]
	[InlineData (MyFlag.OptionA, null, null, false)] // Succeeds: flag set, no nullable structs are set
	[InlineData (MyFlag.OptionA, "", null, false)] // Succeeds: flag set, only HomeAddress is set
	[InlineData (MyFlag.OptionA, null, 42, false)] // Succeeds: flag set, only OptionalField2 is set
	[InlineData (MyFlag.OptionA, "", 42, true)] // Fails: flag set, both nullable structs are set
	[InlineData (MyFlag.OptionB, "", 42, false)] // Succeeds: flag not set, validation not executed
	public void AddConditionalMutuallyExclusiveNullableStructTests (MyFlag flag, string? addressParts, int? optionalField2Value, bool shouldFail)
	{
		var validator = new Validator<MyData> ();
		validator.AddConditionalMutuallyExclusive (
			d => d.Flag,
			exactlyOne: false,
			requireAllFlags: false,
			requiredFlags: [MyFlag.OptionA],
			d => d.HomeAddress,
			d => d.OptionalField2
		);

		Address? homeAddress = null;
		if (addressParts is not null) {
			homeAddress = new Address {
				Street = addressParts == "" ? null : addressParts,
				City = null
			};
		}

		var data = new MyData {
			Flag = flag,
			HomeAddress = homeAddress,
			OptionalField2 = optionalField2Value
		};

		var errors = validator.ValidateAll (data, context);

		if (shouldFail) {
			Assert.True (errors.ContainsKey (string.Empty)); // Global validation error
			var diagnostics = errors [string.Empty];
			Assert.Single (diagnostics);
			Assert.Equal ("RBI0016", diagnostics [0].Id);
		} else {
			Assert.Empty (errors);
		}
	}

	[Fact]
	public void RestrictToFlagTypeTests ()
	{
		// Test with correct flag type - should succeed
		var validator1 = new Validator<MyData> ();
		validator1.RestrictToFlagType (
			d => d.OptionalField1,
			d => d.Flag,
			typeof (MyFlag)
		);

		var data1 = new MyData {
			Flag = MyFlag.OptionA,
			OptionalField1 = "test"
		};

		var errors1 = validator1.ValidateAll (data1, context);
		Assert.Empty (errors1);

		// Test with null field - should succeed
		var data2 = new MyData {
			Flag = MyFlag.OptionA,
			OptionalField1 = null
		};

		var errors2 = validator1.ValidateAll (data2, context);
		Assert.Empty (errors2);

		// Test with wrong flag type - should fail
		var validator2 = new Validator<MyOtherData> ();
		validator2.RestrictToFlagType (
			d => d.OptionalField1,
			d => d.Flag,
			typeof (MyFlag)
		);

		var data3 = new MyOtherData {
			Flag = OtherFlag.OptionX,
			OptionalField1 = "test"
		};

		var errors3 = validator2.ValidateAll (data3, context);
		Assert.True (errors3.ContainsKey (nameof (MyOtherData.OptionalField1)));
		var diagnostics3 = errors3 [nameof (MyOtherData.OptionalField1)];
		Assert.Single (diagnostics3);
		Assert.Equal ("RBI0017", diagnostics3 [0].Id);
	}

	[Fact]
	public void RestrictToFlagTypeNullableStructTests ()
	{
		// Test with correct flag type - should succeed
		var validator1 = new Validator<MyData> ();
		validator1.RestrictToFlagType (
			d => d.HomeAddress,
			d => d.Flag,
			typeof (MyFlag)
		);

		var data1 = new MyData {
			Flag = MyFlag.OptionA,
			HomeAddress = new Address { Street = "Test St", City = "Test City" }
		};

		var errors1 = validator1.ValidateAll (data1, context);
		Assert.Empty (errors1);

		// Test with wrong flag type - should fail
		var validator2 = new Validator<MyOtherData> ();
		validator2.RestrictToFlagType (
			d => d.HomeAddress,
			d => d.Flag,
			typeof (MyFlag)
		);

		var data2 = new MyOtherData {
			Flag = OtherFlag.OptionX,
			HomeAddress = new Address { Street = "Test St", City = "Test City" }
		};

		var errors2 = validator2.ValidateAll (data2, context);
		Assert.True (errors2.ContainsKey (nameof (MyOtherData.HomeAddress)));
		var diagnostics2 = errors2 [nameof (MyOtherData.HomeAddress)];
		Assert.Single (diagnostics2);
		Assert.Equal ("RBI0017", diagnostics2 [0].Id);
	}

	[Fact]
	public void RestrictToFlagTypeNonNullableStructTests ()
	{
		// Test with correct flag type and default struct - should succeed
		var validator1 = new Validator<MyData> ();
		validator1.RestrictToFlagType (
			d => d.WorkAddress,
			d => d.Flag,
			NonNullableAddress.IsDefault,
			typeof (MyFlag)
		);

		var data1 = new MyData {
			Flag = MyFlag.OptionA,
			WorkAddress = new NonNullableAddress () // Default struct
		};

		var errors1 = validator1.ValidateAll (data1, context);
		Assert.Empty (errors1);

		// Test with correct flag type and non-default struct - should succeed
		var data2 = new MyData {
			Flag = MyFlag.OptionA,
			WorkAddress = new NonNullableAddress { Street = "Test St", City = "Test City" }
		};

		var errors2 = validator1.ValidateAll (data2, context);
		Assert.Empty (errors2);

		// Test with wrong flag type and non-default struct - should fail
		var validator2 = new Validator<MyOtherData> ();
		validator2.RestrictToFlagType (
			d => d.WorkAddress,
			d => d.Flag,
			NonNullableAddress.IsDefault,
			typeof (MyFlag)
		);

		var data3 = new MyOtherData {
			Flag = OtherFlag.OptionX,
			WorkAddress = new NonNullableAddress { Street = "Test St", City = "Test City" }
		};

		var errors3 = validator2.ValidateAll (data3, context);
		Assert.True (errors3.ContainsKey (nameof (MyOtherData.WorkAddress)));
		var diagnostics3 = errors3 [nameof (MyOtherData.WorkAddress)];
		Assert.Single (diagnostics3);
		Assert.Equal ("RBI0017", diagnostics3 [0].Id);

		// Test with wrong flag type but default struct - should succeed
		var data4 = new MyOtherData {
			Flag = OtherFlag.OptionX,
			WorkAddress = new NonNullableAddress () // Default struct
		};

		var errors4 = validator2.ValidateAll (data4, context);
		Assert.Empty (errors4);
	}
}
