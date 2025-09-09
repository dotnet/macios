// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Xamarin.Tests;
using Xamarin.Utils;
using Xunit;

namespace Microsoft.Macios.Bindings.Analyzer.Tests;

public class ClassAnalyzerTests : BaseGeneratorWithAnalyzerTestClass {

	class TestDataClassAnalyzerWarnings : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			// not partial class
			/*
			yield return [
@"
#pragma warning disable APL0003

using System;
using System.Runtime.Versioning;
using AVFoundation;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using nfloat = System.Runtime.InteropServices.NFloat;

namespace TestNamespace;

[SupportedOSPlatform (""macos"")]
[SupportedOSPlatform (""ios"")]
[SupportedOSPlatform (""tvos"")]
[SupportedOSPlatform (""maccatalyst13.1"")]
[BindingType<Class>]
public class TestClass{
}",
				"RBI0001",
				DiagnosticSeverity.Error,
				"The binding type 'TestNamespace.TestClass' must be declared partial"
			];
			*/

			// duplicate selector, 2 properties
			yield return [
				@"
#pragma warning disable APL0003

using System;
using System.Runtime.Versioning;
using AVFoundation;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using nfloat = System.Runtime.InteropServices.NFloat;

namespace TestNamespace;

[SupportedOSPlatform (""macos"")]
[SupportedOSPlatform (""ios"")]
[SupportedOSPlatform (""tvos"")]
[SupportedOSPlatform (""maccatalyst13.1"")]
[BindingType<Class>]
public partial class TestClass{

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Property> (""count"")]
	public virtual partial nuint Count { get; set; }

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Property> (""count"")]
	public virtual partial nuint SecondCount { get; set; }
}",
				"RBI0034",
				DiagnosticSeverity.Warning,
				"The selector 'count' used by 'SecondCount' is already used by 'Count'"
			];

			// duplicate selector, 2 static properties
			yield return [
				@"
#pragma warning disable APL0003

using System;
using System.Runtime.Versioning;
using AVFoundation;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using nfloat = System.Runtime.InteropServices.NFloat;

namespace TestNamespace;

[SupportedOSPlatform (""macos"")]
[SupportedOSPlatform (""ios"")]
[SupportedOSPlatform (""tvos"")]
[SupportedOSPlatform (""maccatalyst13.1"")]
[BindingType<Class>]
public partial class TestClass{

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Property> (""count"")]
	public static virtual partial nuint Count { get; set; }

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Property> (""count"")]
	public static virtual partial nuint SecondCount { get; set; }
}",
				"RBI0034",
				DiagnosticSeverity.Warning,
				"The selector 'count' used by 'SecondCount' is already used by 'Count'"
			];

			// duplicate selector, property and method
			yield return [
				@"
#pragma warning disable APL0003

using System;
using System.Runtime.Versioning;
using AVFoundation;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using nfloat = System.Runtime.InteropServices.NFloat;

namespace TestNamespace;

[SupportedOSPlatform (""macos"")]
[SupportedOSPlatform (""ios"")]
[SupportedOSPlatform (""tvos"")]
[SupportedOSPlatform (""maccatalyst13.1"")]
[BindingType<Class>]
public partial class TestClass{

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Property> (""count"")]
	public virtual partial nuint Count { get; set; }

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Method> (""count"")]
	public virtual partial nuint GetCount ();
}",
				"RBI0034",
				DiagnosticSeverity.Warning,
				"The selector 'count' used by 'GetCount' is already used by 'Count'"
			];

			// duplicate selector, 2 methods
			yield return [
				@"
#pragma warning disable APL0003

using System;
using System.Runtime.Versioning;
using AVFoundation;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using nfloat = System.Runtime.InteropServices.NFloat;

namespace TestNamespace;

[SupportedOSPlatform (""macos"")]
[SupportedOSPlatform (""ios"")]
[SupportedOSPlatform (""tvos"")]
[SupportedOSPlatform (""maccatalyst13.1"")]
[BindingType<Class>]
public partial class TestClass{

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Method> (""count"")]
	public virtual partial nuint GetCount ();

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Method> (""count"")]
	public virtual partial nuint SecondGetCount ();
}",
				"RBI0034",
				DiagnosticSeverity.Warning,
				"The selector 'count' used by 'SecondGetCount' is already used by 'GetCount'"
			];

			// duplicate selector, 2 static methods
			yield return [
				@"
#pragma warning disable APL0003

using System;
using System.Runtime.Versioning;
using AVFoundation;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using nfloat = System.Runtime.InteropServices.NFloat;

namespace TestNamespace;

[SupportedOSPlatform (""macos"")]
[SupportedOSPlatform (""ios"")]
[SupportedOSPlatform (""tvos"")]
[SupportedOSPlatform (""maccatalyst13.1"")]
[BindingType<Class>]
public partial class TestClass{

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Method> (""count"")]
	public static virtual partial nuint GetCount ();

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Method> (""count"")]
	public static virtual partial nuint SecondGetCount ();
}",
				"RBI0034",
				DiagnosticSeverity.Warning,
				"The selector 'count' used by 'SecondGetCount' is already used by 'GetCount'"
			];

			// duplicate strong delegate from removing Weak and setting the strong name
			yield return [
				@"
#pragma warning disable APL0003

using System;
using System.Runtime.Versioning;
using AVFoundation;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using nfloat = System.Runtime.InteropServices.NFloat;

namespace TestNamespace;

[SupportedOSPlatform (""macos"")]
[SupportedOSPlatform (""ios"")]
[SupportedOSPlatform (""tvos"")]
[SupportedOSPlatform (""maccatalyst13.1"")]
[BindingType<Class>]
public partial class TestClass{

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Property> (""delegate"",
		ArgumentSemantic.Weak,
		Flags = Property.WeakDelegate,
		StrongDelegateType = typeof (INSUserActivityDelegate))]
	public virtual partial NSObject? WeakDelegate { get; set; }

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Property> (""delegateSecond"",
		ArgumentSemantic.Weak,
		Flags = Property.WeakDelegate,
		StrongDelegateType = typeof (INSUserActivityDelegate),
		StrongDelegateName = ""Delegate""
	)]
	public virtual partial NSObject? WeakSecondDelegate { get; set; }
}",
				"RBI0033",
				DiagnosticSeverity.Error,
				"The weak delegate 'WeakSecondDelegate' strong delegate 'Delegate' is already used by 'WeakDelegate'"
			];

			// duplicate strong delegate from both setting the strong name
			yield return [
				@"
#pragma warning disable APL0003

using System;
using System.Runtime.Versioning;
using AVFoundation;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using nfloat = System.Runtime.InteropServices.NFloat;

namespace TestNamespace;

[SupportedOSPlatform (""macos"")]
[SupportedOSPlatform (""ios"")]
[SupportedOSPlatform (""tvos"")]
[SupportedOSPlatform (""maccatalyst13.1"")]
[BindingType<Class>]
public partial class TestClass{

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Property> (""delegate"",
		ArgumentSemantic.Weak,
		Flags = Property.WeakDelegate,
		StrongDelegateType = typeof (INSUserActivityDelegate)
		StrongDelegateName = ""Delegate""
	)]
	public virtual partial NSObject? OtherWeakDelegate { get; set; }

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Property> (""delegateSecond"",
		ArgumentSemantic.Weak,
		Flags = Property.WeakDelegate,
		StrongDelegateType = typeof (INSUserActivityDelegate),
		StrongDelegateName = ""Delegate""
	)]
	public virtual partial NSObject? WeakSecondDelegate { get; set; }
}",
				"RBI0033",
				DiagnosticSeverity.Error,
				"The weak delegate 'WeakSecondDelegate' strong delegate 'Delegate' is already used by 'OtherWeakDelegate'"
			];

			// empty field selector
			yield return [
				@"
#pragma warning disable APL0003

using System;
using System.Runtime.Versioning;
using AVFoundation;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using nfloat = System.Runtime.InteropServices.NFloat;

namespace TestNamespace;

[SupportedOSPlatform (""macos"")]
[SupportedOSPlatform (""ios"")]
[SupportedOSPlatform (""tvos"")]
[SupportedOSPlatform (""maccatalyst13.1"")]
[BindingType<Class>]
public partial class TestClass{
	[Field<Property> ("""")]
	public static partial int FormatRGBA16Int { get; }
}",
				"RBI0018",
				DiagnosticSeverity.Error,
				"An export property selector must not contain any whitespace"
			];

			// field selector with space
			yield return [
				@"
#pragma warning disable APL0003

using System;
using System.Runtime.Versioning;
using AVFoundation;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using nfloat = System.Runtime.InteropServices.NFloat;

namespace TestNamespace;

[SupportedOSPlatform (""macos"")]
[SupportedOSPlatform (""ios"")]
[SupportedOSPlatform (""tvos"")]
[SupportedOSPlatform (""maccatalyst13.1"")]
[BindingType<Class>]
public partial class TestClass{
	[Field<Property> (""my field"")]
	public static partial int FormatRGBA16Int { get; }
}",
				"RBI0019",
				DiagnosticSeverity.Error,
				"An export property selector must not contain any whitespace"
			];

			// not static field
			yield return [
				@"
#pragma warning disable APL0003

using System;
using System.Runtime.Versioning;
using AVFoundation;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using nfloat = System.Runtime.InteropServices.NFloat;

namespace TestNamespace;

[SupportedOSPlatform (""macos"")]
[SupportedOSPlatform (""ios"")]
[SupportedOSPlatform (""tvos"")]
[SupportedOSPlatform (""maccatalyst13.1"")]
[BindingType<Class>]
public partial class TestClass{
	[Field<Property> (""FormatRGBA16Int"")]
	public partial int FormatRGBA16Int { get; }
}",
				"RBI0030",
				DiagnosticSeverity.Error,
				"Field properties must be declared static"
			];

			// not static field
			yield return [
				@"
#pragma warning disable APL0003

using System;
using System.Runtime.Versioning;
using AVFoundation;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using nfloat = System.Runtime.InteropServices.NFloat;

namespace TestNamespace;

[SupportedOSPlatform (""macos"")]
[SupportedOSPlatform (""ios"")]
[SupportedOSPlatform (""tvos"")]
[SupportedOSPlatform (""maccatalyst13.1"")]
[BindingType<Class>]
public partial class TestClass{
	[Field<Property> (""FormatRGBA16Int"")]
	public static int FormatRGBA16Int { get; }
}",
				"RBI0031",
				DiagnosticSeverity.Error,
				"Exported properties must be declared partial"
			];

			// not partial property
			yield return [
				@"
#pragma warning disable APL0003

using System;
using System.Runtime.Versioning;
using AVFoundation;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using nfloat = System.Runtime.InteropServices.NFloat;

namespace TestNamespace;

[SupportedOSPlatform (""macos"")]
[SupportedOSPlatform (""ios"")]
[SupportedOSPlatform (""tvos"")]
[SupportedOSPlatform (""maccatalyst13.1"")]
[BindingType<Class>]
public partial class TestClass{

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Property> (""attributedStringByInflectingString"")]
	public virtual NSAttributedString AttributedStringByInflectingString { get; set; }
}",
				"RBI0031",
				DiagnosticSeverity.Error,
				"Exported properties must be declared partial"
			];

			// property invalid selector empty
			yield return [
				@"
#pragma warning disable APL0003

using System;
using System.Runtime.Versioning;
using AVFoundation;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using nfloat = System.Runtime.InteropServices.NFloat;

namespace TestNamespace;

[SupportedOSPlatform (""macos"")]
[SupportedOSPlatform (""ios"")]
[SupportedOSPlatform (""tvos"")]
[SupportedOSPlatform (""maccatalyst13.1"")]
[BindingType<Class>]
public partial class TestClass{

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Property> ("""")]
	public virtual partial NSAttributedString AttributedStringByInflectingString { get; set; }
}",
				"RBI0018",
				DiagnosticSeverity.Error,
				"An export property selector must not contain any whitespace"
			];

			// property invalid selector has space 
			yield return [
				@"
#pragma warning disable APL0003

using System;
using System.Runtime.Versioning;
using AVFoundation;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using nfloat = System.Runtime.InteropServices.NFloat;

namespace TestNamespace;

[SupportedOSPlatform (""macos"")]
[SupportedOSPlatform (""ios"")]
[SupportedOSPlatform (""tvos"")]
[SupportedOSPlatform (""maccatalyst13.1"")]
[BindingType<Class>]
public partial class TestClass{

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Property> (""attributedStringByInflectingString "")]
	public virtual partial NSAttributedString AttributedStringByInflectingString { get; set; }
}",
				"RBI0019",
				DiagnosticSeverity.Error,
				"An export property selector must not contain any whitespace"
			];

			// property invalid selector has extra ':' 
			yield return [
				@"
#pragma warning disable APL0003

using System;
using System.Runtime.Versioning;
using AVFoundation;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using nfloat = System.Runtime.InteropServices.NFloat;

namespace TestNamespace;

[SupportedOSPlatform (""macos"")]
[SupportedOSPlatform (""ios"")]
[SupportedOSPlatform (""tvos"")]
[SupportedOSPlatform (""maccatalyst13.1"")]
[BindingType<Class>]
public partial class TestClass{

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Property> (""attributedStringByInflectingString"")]
	public virtual partial NSAttributedString AttributedStringByInflectingString { 
		[Export<Property> (""isAttributedStringByInflectingString:"")]
		get; 
		set; 
	}
}",
				"RBI0029",
				DiagnosticSeverity.Error,
				"There is a mismatch between the arguments of 'AttributedStringByInflectingString' (found 0) and the selector 'isAttributedStringByInflectingString:' (found 1)"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataClassAnalyzerWarnings>]
	public async Task ClassAnalyzerWarnings (ApplePlatform platform, string inputText, string diagnosticId, DiagnosticSeverity severity, string diagnosticMessage)
	{
		var (compilation, _) = CreateCompilation (platform, sources: inputText);
		var diagnostics = await RunAnalyzer (new BindingTypeSemanticAnalyzer (), compilation);

		var analyzerDiagnotics = diagnostics
			.Where (d => d.Id == diagnosticId).ToArray ();
		Assert.Single (analyzerDiagnotics);
		VerifyDiagnosticMessage (analyzerDiagnotics [0], diagnosticId,
			severity, diagnosticMessage);
	}

	class TestDataClassAnalyzerSuccess : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			// duplicate selector, 2 properties, one is skipped from registration
			yield return [
				@"
#pragma warning disable APL0003

using System;
using System.Runtime.Versioning;
using AVFoundation;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using nfloat = System.Runtime.InteropServices.NFloat;

namespace TestNamespace;

[SupportedOSPlatform (""macos"")]
[SupportedOSPlatform (""ios"")]
[SupportedOSPlatform (""tvos"")]
[SupportedOSPlatform (""maccatalyst13.1"")]
[BindingType<Class>]
public partial class TestClass{

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Property> (""count"")]
	public virtual partial nuint Count { get; set; }

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Property> (""count"", Flags = Property.SkipRegistration)]
	public virtual partial nuint SecondCount { get; set; }
}",
				"RBI0034",
			];

			// duplicate selector, 2 properties, one is static the other is instance
			yield return [
				@"
#pragma warning disable APL0003

using System;
using System.Runtime.Versioning;
using AVFoundation;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using nfloat = System.Runtime.InteropServices.NFloat;

namespace TestNamespace;

[SupportedOSPlatform (""macos"")]
[SupportedOSPlatform (""ios"")]
[SupportedOSPlatform (""tvos"")]
[SupportedOSPlatform (""maccatalyst13.1"")]
[BindingType<Class>]
public partial class TestClass{

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Property> (""count"")]
	public static virtual partial nuint Count { get; set; }

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Property> (""count"", Flags = Property.SkipRegistration)]
	public virtual partial nuint SecondCount { get; set; }
}",
				"RBI0034",
			];

			// duplicate selector, 2 methods one is static the other is instance
			yield return [
				@"
#pragma warning disable APL0003

using System;
using System.Runtime.Versioning;
using AVFoundation;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using nfloat = System.Runtime.InteropServices.NFloat;

namespace TestNamespace;

[SupportedOSPlatform (""macos"")]
[SupportedOSPlatform (""ios"")]
[SupportedOSPlatform (""tvos"")]
[SupportedOSPlatform (""maccatalyst13.1"")]
[BindingType<Class>]
public partial class TestClass{

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Method> (""count"")]
	public static virtual partial nuint GetCount ();

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Method> (""count"")]
	public virtual partial nuint SecondGetCount ();
}",
				"RBI0034",
			];

			// duplicate selector, 2 methods one is skipped
			yield return [
				@"
#pragma warning disable APL0003

using System;
using System.Runtime.Versioning;
using AVFoundation;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using nfloat = System.Runtime.InteropServices.NFloat;

namespace TestNamespace;

[SupportedOSPlatform (""macos"")]
[SupportedOSPlatform (""ios"")]
[SupportedOSPlatform (""tvos"")]
[SupportedOSPlatform (""maccatalyst13.1"")]
[BindingType<Class>]
public partial class TestClass{

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Method> (""count"", Flags = Method.SkipRegistration)]
	public virtual partial nuint GetCount ();

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Method> (""count"")]
	public virtual partial nuint SecondGetCount ();
}",
				"RBI0034",
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataClassAnalyzerSuccess>]
	public async Task ClassAnalyzerSuccess (ApplePlatform platform, string inputText, string diagnosticId)
	{
		var (compilation, _) = CreateCompilation (platform, sources: inputText);
		var diagnostics = await RunAnalyzer (new BindingTypeSemanticAnalyzer (), compilation);

		var analyzerDiagnotics = diagnostics
			.Where (d => d.Id == diagnosticId).ToArray ();
		// ensure that the error is not present
		Assert.Empty (analyzerDiagnotics);
	}
}
