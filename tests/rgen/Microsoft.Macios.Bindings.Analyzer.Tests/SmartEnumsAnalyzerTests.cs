// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Xamarin.Tests;
using Xamarin.Utils;
using Xunit;

namespace Microsoft.Macios.Bindings.Analyzer.Tests;

public class SmartEnumsAnalyzerTests : BaseGeneratorWithAnalyzerTestClass {

	const string emptyIdentifier = @"
using Foundation;
using ObjCRuntime;
using ObjCBindings;

namespace AVFoundation;

[BindingType<SmartEnum>]
public enum AVCaptureSystemPressureExampleLevel {
	[Field<EnumValue> (""       "")]
	Shutdown,
}";

	const string notValidIdentifierQuotes = @"
using Foundation;
using ObjCRuntime;
using ObjCBindings;

namespace AVFoundation;

[BindingType<SmartEnum>]
public enum AVCaptureSystemPressureExampleLevel {
	// empty field, this should be an error
	[Field<EnumValue> (""Weird\""Name"")]
	Shutdown,
}";

	const string notValidIdentifierKeyword = @"
using Foundation;
using ObjCRuntime;
using ObjCBindings;

namespace AVFoundation;

[BindingType<SmartEnum>]
public enum AVCaptureSystemPressureExampleLevel {
	// empty field, this should be an error
	[Field<EnumValue> (""class"")]
	Shutdown,
}";

	const string notValidIdentifierStartNumber = @"
using Foundation;
using ObjCRuntime;
using ObjCBindings;

namespace AVFoundation;

[BindingType<SmartEnum>]
public enum AVCaptureSystemPressureExampleLevel {
	// empty field, this should be an error
	[Field<EnumValue> (""42Values"")]
	Shutdown,
}";

	const string notValidIdentifierNewLines = @"
using Foundation;
using ObjCRuntime;
using ObjCBindings;

namespace AVFoundation;

[BindingType<SmartEnum>]
public enum AVCaptureSystemPressureExampleLevel {
	// empty field, this should be an error
	[Field<EnumValue> (""The\nValues"")]
	Shutdown,
}";

	[Theory]
	[AllSupportedPlatforms (emptyIdentifier, "       ")]
	[AllSupportedPlatforms (notValidIdentifierQuotes, "Weird\"Name")]
	[AllSupportedPlatforms (notValidIdentifierKeyword, "class")]
	[AllSupportedPlatforms (notValidIdentifierStartNumber, "42Values")]
	[AllSupportedPlatforms (notValidIdentifierNewLines, "The\nValues")]
	public async Task SmartEnumSymbolMustBeValidIdentifier (ApplePlatform platform, string inputText, string fieldValue)
	{
		var (compilation, _) = CreateCompilation (platform, sources: inputText);
		var diagnostics = await RunAnalyzer (new BindingTypeSemanticAnalyzer (), compilation);
		var analyzerDiagnotics = diagnostics
			.Where (d => d.Id == "RBI0010").ToArray ();
		Assert.Single (analyzerDiagnotics);
		VerifyDiagnosticMessage (analyzerDiagnotics [0], "RBI0010",
			DiagnosticSeverity.Error,
			$"The enum value 'AVFoundation.AVCaptureSystemPressureExampleLevel.Shutdown' backing field '{fieldValue}' is not a valid identifier");
	}

	const string appleFrameworkLib = @"
using Foundation;
using ObjCRuntime;
using ObjCBindings;

namespace AVFoundation;

[BindingType<SmartEnum>]
public enum AVCaptureSystemPressureExampleLevel {
	[Field<EnumValue> (""AVCaptureSystemPressureLevelNominal"")]
	Nominal,

	[Field<EnumValue> (""AVCaptureSystemPressureLevelFair"")]
	Fair,

	[Field<EnumValue> (""AVCaptureSystemPressureLevelSerious"")]
	Serious,

	[Field<EnumValue> (""AVCaptureSystemPressureLevelCritical"")]
	Critical,

	// do not do this with apple frameworks
	[Field<EnumValue> (""AVCaptureSystemPressureLevelShutdown"", ""/path/to/not/needed/lib"")]
	Shutdown,
}";

	const string appleFrameworkLibNamedParameter = @"
using Foundation;
using ObjCRuntime;
using ObjCBindings;

namespace AVFoundation;

[BindingType<SmartEnum>]
public enum AVCaptureSystemPressureExampleLevel {
	[Field<EnumValue> (""AVCaptureSystemPressureLevelNominal"")]
	Nominal,

	[Field<EnumValue> (""AVCaptureSystemPressureLevelFair"")]
	Fair,

	[Field<EnumValue> (""AVCaptureSystemPressureLevelSerious"")]
	Serious,

	[Field<EnumValue> (""AVCaptureSystemPressureLevelCritical"")]
	Critical,

	// do not do this with apple frameworks
	[Field<EnumValue> (""AVCaptureSystemPressureLevelShutdown"", LibraryPath= ""/path/to/not/needed/lib"")]
	Shutdown,
}";

	[Theory]
	[AllSupportedPlatforms (appleFrameworkLib)]
	[AllSupportedPlatforms (appleFrameworkLibNamedParameter)]
	public async Task SmartEnumAppleFrameworkNotLibrary (ApplePlatform platform, string inputText)
	{
		var (compilation, _) = CreateCompilation (platform, sources: inputText);
		var diagnostics = await RunAnalyzer (new BindingTypeSemanticAnalyzer (), compilation);

		var analyzerDiagnotics = diagnostics
			.Where (d => d.Id == "RBI0012").ToArray ();
		Assert.Single (analyzerDiagnotics);
		VerifyDiagnosticMessage (analyzerDiagnotics [0], "RBI0012",
			DiagnosticSeverity.Warning,
			"The Field attribute for the enum value 'AVFoundation.AVCaptureSystemPressureExampleLevel.Shutdown' must not provide a value for 'LibraryName'");
	}

	const string customLibraryMissingLibraryName = @"
using System;
using System.Runtime.Versioning;
using Foundation;
using ObjCRuntime;
using ObjCBindings;

namespace CustomLibrary;

[BindingType<SmartEnum>]
public enum CustomLibraryEnum {
	[Field<EnumValue> (""None"", ""/path/to/customlibrary.framework"")]
	None,
	[Field<EnumValue> (""Medium"", ""/path/to/customlibrary.framework"")]
	Medium,
	// missing lib, this is an error
	[Field<EnumValue> (""High"")]
	High,
}
";

	const string customLibraryEmptyLibraryName = @"
using System;
using System.Runtime.Versioning;
using Foundation;
using ObjCRuntime;
using ObjCBindings;

namespace CustomLibrary;

[BindingType<SmartEnum>]
public enum CustomLibraryEnum {
	[Field<EnumValue> (""None"", ""/path/to/customlibrary.framework"")]
	None,
	[Field<EnumValue> (""Medium"", ""/path/to/customlibrary.framework"")]
	Medium,
	// empty lib, this is an error
	[Field<EnumValue> (""High"", ""   "")]
	High,
}
";

	const string customLibraryEmptyLibraryNameParameter = @"
using System;
using System.Runtime.Versioning;
using Foundation;
using ObjCRuntime;
using ObjCBindings;

namespace CustomLibrary;

[BindingType<SmartEnum>]
public enum CustomLibraryEnum {
	[Field<EnumValue> (""None"", ""/path/to/customlibrary.framework"")]
	None,
	[Field<EnumValue> (""Medium"", ""/path/to/customlibrary.framework"")]
	Medium,
	// empty lib, this is an error
	[Field<EnumValue> (""High"", LibraryPath = ""   "")]
	High,
}
";

	[Theory]
	[AllSupportedPlatforms (customLibraryMissingLibraryName)]
	[AllSupportedPlatforms (customLibraryEmptyLibraryName)]
	[AllSupportedPlatforms (customLibraryEmptyLibraryNameParameter)]
	public async Task SmartEnumThirdPartyLibrary (ApplePlatform platform, string inputText)
	{
		var (compilation, _) = CreateCompilation (platform, sources: inputText);
		var diagnostics = await RunAnalyzer (new BindingTypeSemanticAnalyzer (), compilation);
		var analyzerDiagnotics = diagnostics
			.Where (d => d.Id == "RBI0011").ToArray ();
		Assert.Single (analyzerDiagnotics);
		VerifyDiagnosticMessage (analyzerDiagnotics [0], "RBI0011",
			DiagnosticSeverity.Error,
			"The field attribute for the enum value 'CustomLibrary.CustomLibraryEnum.High' must set the property 'LibraryName'");
	}

	[Theory]
	[AllSupportedPlatforms]
	public async Task SmartEnumNoFieldAttributes (ApplePlatform platform)
	{
		const string inputText = @"
using System;
using System.Runtime.Versioning;
using Foundation;
using ObjCRuntime;
using ObjCBindings;

namespace CustomLibrary;

[BindingType<SmartEnum>]
public enum CustomLibraryEnum {
	None,
	Medium,
	High,
}
";
		var (compilation, _) = CreateCompilation (platform, sources: inputText);
		var diagnostics = await RunAnalyzer (new BindingTypeSemanticAnalyzer (), compilation);
		var analyzerDiagnotics = diagnostics
			.Where (d => d.Id == "RBI0008").ToArray ();
		// we should have a diagnostic for each enum value
		Assert.Single (analyzerDiagnotics);
		VerifyDiagnosticMessage (analyzerDiagnotics [0], "RBI0008",
			DiagnosticSeverity.Error,
			"The enum 'CustomLibrary.CustomLibraryEnum' must have at least one member tagged with a Field<EnumValue> attribute");
	}

	[Theory]
	[AllSupportedPlatforms]
	public async Task SmartEnumFieldNotDuplicated (ApplePlatform platform)
	{
		const string inputText = @"
using Foundation;
using ObjCRuntime;
using ObjCBindings;

namespace AVFoundation;

[BindingType<SmartEnum>]
public enum AVCaptureSystemPressureExampleLevel {
	[Field<EnumValue> (""AVCaptureSystemPressureLevelNominal"")]
	Nominal,

	// duplicated, this should be an error
	[Field<EnumValue> (""AVCaptureSystemPressureLevelNominal"")]
	Fair,

	[Field<EnumValue> (""AVCaptureSystemPressureLevelSerious"")]
	Serious,

	[Field<EnumValue> (""AVCaptureSystemPressureLevelCritical"")]
	Critical,

	[Field<EnumValue> (""AVCaptureSystemPressureLevelShutdown"")]
	Shutdown,
}";

		var (compilation, _) = CreateCompilation (platform, sources: inputText);
		var diagnostics = await RunAnalyzer (new BindingTypeSemanticAnalyzer (), compilation);
		var analyzerDiagnotics = diagnostics
			.Where (d => d.Id == "RBI0009").ToArray ();
		Assert.Single (analyzerDiagnotics);
		VerifyDiagnosticMessage (analyzerDiagnotics [0], "RBI0009",
			DiagnosticSeverity.Error,
			"The backing field 'AVFoundation.AVCaptureSystemPressureLevelNominal' for the enum value 'AVCaptureSystemPressureExampleLevel.Fair' is already in use for the enum value 'AVCaptureSystemPressureExampleLevel.Nominal'");
	}

	[Theory]
	[AllSupportedPlatforms]
	public async Task UnsupportedPlatformSmartEnum (ApplePlatform platform)
	{
		string inputText = $@"
using Foundation;
using ObjCRuntime;
using ObjCBindings;
using System.Runtime.Versioning;

namespace AVFoundation;

[BindingType<SmartEnum>]
[UnsupportedOSPlatform (""{platform.AsString ().ToLower ()}"")]
public enum AVCaptureSystemPressureExampleLevel {{
	[Field<EnumValue> (""AVCaptureSystemPressureLevelNominal"")]
	Nominal,

	[Field<EnumValue> (""AVCaptureSystemPressureLevelFair"")]
	Fair,

	[Field<EnumValue> (""AVCaptureSystemPressureLevelSerious"")]
	Serious,

	[Field<EnumValue> (""AVCaptureSystemPressureLevelCritical"")]
	Critical,

	[Field<EnumValue> (""AVCaptureSystemPressureLevelShutdown"")]
	Shutdown,
}}";

		var (compilation, _) = CreateCompilation (platform, sources: inputText);
		var diagnostics = await RunAnalyzer (new BindingTypeSemanticAnalyzer (), compilation);
		var analyzerDiagnotics = diagnostics
			.Where (d => d.Id == "RBI0027").ToArray ();
		Assert.Single (analyzerDiagnotics);
		var str = analyzerDiagnotics [0].GetMessage ();
		VerifyDiagnosticMessage (analyzerDiagnotics [0], "RBI0027",
			DiagnosticSeverity.Error,
			$"The symbol 'AVFoundation.AVCaptureSystemPressureExampleLevel' is accessible on platform '{platform}' when it was marked otherwise");
	}

	[Theory]
	[AllSupportedPlatforms]
	public async Task UnsupportedPlatformSmartEnumValue (ApplePlatform platform)
	{
		string inputText = $@"
using Foundation;
using ObjCRuntime;
using ObjCBindings;
using System.Runtime.Versioning;

namespace AVFoundation;

[SupportedOSPlatform (""macos"")]
[SupportedOSPlatform (""ios"")]
[SupportedOSPlatform (""tvos"")]
[SupportedOSPlatform (""maccatalyst13.1"")]
[BindingType<SmartEnum>]
public enum AVCaptureSystemPressureExampleLevel {{
	[Field<EnumValue> (""AVCaptureSystemPressureLevelNominal"")]
	Nominal,

	[UnsupportedOSPlatform (""{platform.AsString ().ToLower ()}"")]
	[Field<EnumValue> (""AVCaptureSystemPressureLevelFair"")]
	Fair,

	[Field<EnumValue> (""AVCaptureSystemPressureLevelSerious"")]
	Serious,

	[Field<EnumValue> (""AVCaptureSystemPressureLevelCritical"")]
	Critical,

	[Field<EnumValue> (""AVCaptureSystemPressureLevelShutdown"")]
	Shutdown,
}}";

		var (compilation, _) = CreateCompilation (platform, sources: inputText);
		var diagnostics = await RunAnalyzer (new BindingTypeSemanticAnalyzer (), compilation);
		var analyzerDiagnotics = diagnostics
			.Where (d => d.Id == "RBI0027").ToArray ();
		Assert.Single (analyzerDiagnotics);
		var str = analyzerDiagnotics [0].GetMessage ();
		VerifyDiagnosticMessage (analyzerDiagnotics [0], "RBI0027",
			DiagnosticSeverity.Error,
			$"The symbol 'AVFoundation.AVCaptureSystemPressureExampleLevel.Fair' is accessible on platform '{platform}' when it was marked otherwise");
	}

	[Theory]
	[AllSupportedPlatforms]
	public async Task UnsupportedPlatformSmartEnumValueIgnored (ApplePlatform platform)
	{
		Dictionary<ApplePlatform, string> platformDefines = new () {
			{ ApplePlatform.iOS , "__IOS__" },
			{ ApplePlatform.TVOS, "__TVOS__" },
			{ ApplePlatform.MacOSX, "__MACOS__" },
			{ ApplePlatform.MacCatalyst, "__MACCATALYST__" },
		};
		string inputText = $@"
using Foundation;
using ObjCRuntime;
using ObjCBindings;
using System.Runtime.Versioning;

namespace AVFoundation;

[SupportedOSPlatform (""macos"")]
[SupportedOSPlatform (""ios"")]
[SupportedOSPlatform (""tvos"")]
[SupportedOSPlatform (""maccatalyst13.1"")]
[BindingType<SmartEnum>]
public enum AVCaptureSystemPressureExampleLevel {{
	[Field<EnumValue> (""AVCaptureSystemPressureLevelNominal"")]
	Nominal,

#if !{platformDefines [platform]}
	[UnsupportedOSPlatform (""{platform.AsString ().ToLower ()}"")]
	[Field<EnumValue> (""AVCaptureSystemPressureLevelFair"")]
	Fair,
#endif

	[Field<EnumValue> (""AVCaptureSystemPressureLevelSerious"")]
	Serious,

	[Field<EnumValue> (""AVCaptureSystemPressureLevelCritical"")]
	Critical,

	[Field<EnumValue> (""AVCaptureSystemPressureLevelShutdown"")]
	Shutdown,
}}";

		var (compilation, _) = CreateCompilation (platform, sources: inputText);
		var diagnostics = await RunAnalyzer (new BindingTypeSemanticAnalyzer (), compilation);
		var analyzerDiagnotics = diagnostics
			.Where (d => d.Id == "RBI0027").ToArray ();
		Assert.Empty (analyzerDiagnotics);
	}
}
