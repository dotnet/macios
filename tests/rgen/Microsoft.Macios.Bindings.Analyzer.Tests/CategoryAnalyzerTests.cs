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

public class CategoryAnalyzerTests : BaseGeneratorWithAnalyzerTestClass {

	class TestDataCategoryAnalyzerWarnings : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			// not partial category 
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
[BindingType<Category> (typeof (NSObject))]
public static class TestClass{
}",
				"RBI0001",
				DiagnosticSeverity.Error,
				"The binding type 'TestNamespace.TestClass' must be declared partial"
			];

			// not static category 
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
[BindingType<Category> (typeof (NSObject))]
public partial class TestClass{
}",
				"RBI0004",
				DiagnosticSeverity.Error,
				"BindingType<Category> can only be used to decorate a static class but was found on 'TestNamespace.TestClass' which is not static"
			];

			// static method, not partial method
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
[BindingType<Category> (typeof (NSObject))]
public static partial class TestClass{

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Method> (""valueForKey:"", Flags = Method.MarshalNativeExceptions)]
	public static unsafe NSObject ValueForKey (this NSObject self, NSString key);
}",
				"RBI0042",
				DiagnosticSeverity.Error,
				"The method 'ValueForKey' must me partial"
			];

			// static method, not an extension method
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
[BindingType<Category> (typeof (NSObject))]
public static partial class TestClass{

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Method> (""valueForKey:"", Flags = Method.MarshalNativeExceptions)]
	public static unsafe partial NSObject ValueForKey (NSObject self, NSString key);
}",
				"RBI0043",
				DiagnosticSeverity.Error,
				"The method 'ValueForKey' in category 'TestClass' has to be an extension method for 'Foundation.NSObject'"
			];

			// static method, an extension method for the wrong type
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
[BindingType<Category> (typeof (NSObject))]
public static partial class TestClass{

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Method> (""valueForKey:"", Flags = Method.MarshalNativeExceptions)]
	public static unsafe partial NSObject ValueForKey (this NSValue self, NSString key);
}",
				"RBI0044",
				DiagnosticSeverity.Error,
				"Extension method 'ValueForKey' in category 'TestClass' must have the first parameter type match the category's extended type 'Foundation.NSObject' found 'Foundation.NSValue'"
			];
			
			// category with constructors
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
[BindingType<Category> (typeof (NSObject))]
public partial class TestClass{

	[Export<Constructor> (""initWithScheme:host:path:"")]
	public TestClass (string scheme, string host, string path);

}",
				"RBI0046",
				DiagnosticSeverity.Error,
				"Category 'TestClass' has constructors (found 1), but constructors are not supported on categories"
			];
			
			// category with properties 
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
[BindingType<Category> (typeof (NSObject))]
public partial class TestClass{

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Property> (""count"")]
	public virtual partial nuint Count { get; set; }
}",
				"RBI0047",
				DiagnosticSeverity.Error,
				"Category 'TestClass' has properties (found 1), but properties are not supported on categories"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataCategoryAnalyzerWarnings>]
	public async Task CategoryAnalyzerWarnings (ApplePlatform platform, string inputText, string diagnosticId, DiagnosticSeverity severity, string diagnosticMessage)
	{
		var (compilation, _) = CreateCompilation (platform, sources: inputText);
		var diagnostics = await RunAnalyzer (new BindingTypeSemanticAnalyzer (), compilation);

		var analyzerDiagnotics = diagnostics
			.Where (d => d.Id == diagnosticId).ToArray ();
		Assert.Single (analyzerDiagnotics);
		VerifyDiagnosticMessage (analyzerDiagnotics [0], diagnosticId,
			severity, diagnosticMessage);
	}

}
