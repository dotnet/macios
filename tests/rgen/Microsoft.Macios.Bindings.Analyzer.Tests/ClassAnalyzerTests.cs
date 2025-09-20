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

			// async methods

			// async method not void
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
	[Export<Method> (""count"", 
		Flags = ObjCBindings.Method.Async)]
	public virtual partial nuint GetCount ();

}",
				"RBI0035",
				DiagnosticSeverity.Error,
				"The method 'GetCount' was marked as async but its return type is not void"
			];

			// void but no args
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
	[Export<Method> (""count"", 
		Flags = ObjCBindings.Method.Async)]
	public virtual partial void GetCount ();

}",
				"RBI0036",
				DiagnosticSeverity.Error,
				"The method 'GetCount' was marked as async but has 0 parameters when at least a single delegate parameter is required"
			];

			// void but with args but no delegate
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
	[Export<Method> (""count:"", 
		Flags = ObjCBindings.Method.Async)]
	public virtual partial void GetCount (int value);

}",
				"RBI0037",
				DiagnosticSeverity.Error,
				"The method 'GetCount' was marked as async but its last parameter is not a delegate",
			];

			// void but with delegate but with a duplicated async name
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
	[Export<Method> (""count:"", 
		Flags = ObjCBindings.Method.Async,
		MethodName = ""GetCountAsync"")]
	public virtual partial void GetCount (Action callback);

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Method> (""count_second:"", 
		Flags = ObjCBindings.Method.Async,
		MethodName = ""GetCountAsync"")]
	public virtual partial void GetCountSecond (Action callback);

}",
				"RBI0039",
				DiagnosticSeverity.Error,
				"The async name 'GetCountAsync' used by 'GetCountSecond' is already used by 'GetCount'",
			];

			// can async method but flag is missing
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
	[Export<Method> (""count:"", 
		Flags = ObjCBindings.Method.Default,
		MethodName = ""GetCountAsync"")]
	public virtual partial void GetCount (Action callback);

}",
				"RBI0038",
				DiagnosticSeverity.Warning,
				"The method 'GetCount' was not marked as async but it can be"
			];

			// correct async method but we are missing the return type
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
	[Export<Method> (""count:"", 
		Flags = ObjCBindings.Method.Async,
		MethodName = ""GetCountAsync"")]
	public virtual partial void GetCount (int first, int second, Action callback);

}",
				"RBI0040",
				DiagnosticSeverity.Warning,
				"The method 'GetCount' was marked as async and has multiple parameters but does not provide a return type name, a nameless tuple will be generated for the async method"
			];

			// constructor that hides a protocol constructor
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
[BindingType<Protocol>]
public partial interface IMyNSCoding {

	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Method> (""initWithCoder:"", Flags = Method.Factory)]
	public virtual partial IMyNSCoding CreateWithCoder (NSObject coder);
}

[SupportedOSPlatform (""macos"")]
[SupportedOSPlatform (""ios"")]
[SupportedOSPlatform (""tvos"")]
[SupportedOSPlatform (""maccatalyst13.1"")]
[BindingType<Class>]
public partial class TestClass : IMyNSCoding {

	// we are testing that the protocol constructor is not added and that we don't get a duplicate
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Constructor> (""initWithCoder:"")]
	public TestClass (NSObject coder);

}",
				"RBI0041",
				DiagnosticSeverity.Warning,
				"The class 'TestClass' contains a constructor with the selector 'initWithCoder:' that hides a inline constructor from protocol 'TestNamespace.IMyNSCoding'"
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

			// void but with delegate with a duplicated async name but with different args
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
	[Export<Method> (""count:"", 
		Flags = ObjCBindings.Method.Async,
		MethodName = ""GetCountAsync"")]
	public virtual partial void GetCount (Action callback);

	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[SupportedOSPlatform (""macos"")]
	[SupportedOSPlatform (""maccatalyst13.1"")]
	[Export<Method> (""count_second:"", 
		Flags = ObjCBindings.Method.Async,
		MethodName = ""GetCountAsync"")]
	public virtual partial void GetCountSecond (int second, Action callback);

}",
				"RBI0039",
			];

			// correct async method but with return type
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
	[Export<Method> (""count:"", 
		Flags = ObjCBindings.Method.Async,
		ResultType = typeof ((int First, int Second)),
		MethodName = ""GetCountAsync"")]
	public virtual partial void GetCount (int first, int second, Action callback);

}",
				"RBI0040",
			];

			// correct async method but with return type name
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
	[Export<Method> (""count:"", 
		Flags = ObjCBindings.Method.Async,
		ResultTypeName = ""AsyncResult""
		MethodName = ""GetCountAsync"")]
	public virtual partial void GetCount (int first, int second, Action callback);

}",
				"RBI0040",
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
