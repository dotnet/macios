// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
#pragma warning disable APL0003

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
using Xamarin.Tests;
using Xamarin.Utils;
using Xunit;
using static Microsoft.Macios.Generator.Emitters.BindingSyntaxFactory;

namespace Microsoft.Macios.Generator.Tests.Emitters;

public class BindingSyntaxFactoryStrongDictionaryTests : BaseGeneratorTestClass {

	class TestDataGetInvocations : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			const string stringGetter = @"
using System;
using Foundation;
using ObjCBindings;

namespace Test;

public class TestClass {

	public static class Keys {
		public static partial NSString ColorSpace { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.ColorSpace), StrongDictionaryKeyClass = typeof (Keys))]
	public string Name { get; set; }
}
";
			yield return [
				stringGetter,
				"GetStringValue (Test.TestClass.Keys.ColorSpace)",
				"SetStringValue (Test.TestClass.Keys.ColorSpace, value)",
			];

			const string nsstringGetter = @"
using System;
using Foundation;
using ObjCBindings;

namespace Test;

public class TestClass {

	public static class Keys {
		public static partial NSString ColorSpace { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.ColorSpace), StrongDictionaryKeyClass = typeof (Keys))]
	public NSString Name { get; set; }
}
";
			yield return [
				nsstringGetter,
				"GetNSStringValue (Test.TestClass.Keys.ColorSpace)",
				"SetStringValue (Test.TestClass.Keys.ColorSpace, value)",
			];

			const string stringArrayGetter = @"
using System;
using ObjCBindings;

namespace Test;

public class TestClass {

	public static class Keys {
		public static partial NSString Names { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.Names), StrongDictionaryKeyClass = typeof (Keys))]
	public string [] Names { get; set; }
}
";
			yield return [
				stringArrayGetter,
				$"GetArray<string> (Test.TestClass.Keys.Names, (ptr) => {Global ("CoreFoundation")}.CFString.FromHandle (ptr)!)",
				"SetArrayValue (Test.TestClass.Keys.Names, value)",
			];

			const string booleanGetter = @"
using System;
using ObjCBindings;

namespace Test;

public class TestClass {

	public static class Keys {
		public static partial NSString IsEnabled { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.IsEnabled), StrongDictionaryKeyClass = typeof (Keys))]
	public bool IsEnabled { get; set; }
}
";
			yield return [
				booleanGetter,
				"GetBoolValue (Test.TestClass.Keys.IsEnabled)",
				"SetBooleanValue (Test.TestClass.Keys.IsEnabled, value)",
			];

			const string int32Getter = @"
using System;
using ObjCBindings;

namespace Test;

public class TestClass {

	public static class Keys {
		public static partial NSString SomeNumber { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.SomeNumber), StrongDictionaryKeyClass = typeof (Keys))]
	public int SomeNumber { get; set; }
}
";
			yield return [
				int32Getter,
				"GetInt32Value (Test.TestClass.Keys.SomeNumber)",
				"SetNumberValue (Test.TestClass.Keys.SomeNumber, value)",
			];

			const string intPtrGetter = @"
using System;
using ObjCBindings;

namespace Test;

public class TestClass {

	public static class Keys {
		public static partial NSString SomePointer { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.SomePointer), StrongDictionaryKeyClass = typeof (Keys))]
	public IntPtr SomePointer { get; set; }
}
";
			yield return [
				intPtrGetter,
				"GetNIntValue (Test.TestClass.Keys.SomePointer)",
				"SetNumberValue (Test.TestClass.Keys.SomePointer, value)",
			];

			const string longGetter = @"
using System;
using ObjCBindings;

namespace Test;

public class TestClass {

	public static class Keys {
		public static partial NSString SomeLongNumber { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.SomeLongNumber), StrongDictionaryKeyClass = typeof (Keys))]
	public long SomeLongNumber { get; set; }
}
";
			yield return [
				longGetter,
				"GetLongValue (Test.TestClass.Keys.SomeLongNumber)",
				"SetNumberValue (Test.TestClass.Keys.SomeLongNumber, value)",
			];

			const string ulongGetter = @"
using System;
using ObjCBindings;

namespace Test;

public class TestClass {

	public static class Keys {
		public static partial NSString SomeULongNumber { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.SomeULongNumber), StrongDictionaryKeyClass = typeof (Keys))]
	public ulong SomeULongNumber { get; set; }
}
";
			yield return [
				ulongGetter,
				"GetULongValue (Test.TestClass.Keys.SomeULongNumber)",
				"SetNumberValue (Test.TestClass.Keys.SomeULongNumber, value)",
			];

			const string floatGetter = @"
using System;
using ObjCBindings;

namespace Test;

public class TestClass {

	public static class Keys {
		public static partial NSString SomeFloatNumber { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.SomeFloatNumber), StrongDictionaryKeyClass = typeof (Keys))]
	public float SomeFloatNumber { get; set; }
}
";
			yield return [
				floatGetter,
				"GetFloatValue (Test.TestClass.Keys.SomeFloatNumber)",
				"SetNumberValue (Test.TestClass.Keys.SomeFloatNumber, value)",
			];

			const string doubleGetter = @"
using System;
using ObjCBindings;

namespace Test;

public class TestClass {

	public static class Keys {
		public static partial NSString SomeDoubleNumber { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.SomeDoubleNumber), StrongDictionaryKeyClass = typeof (Keys))]
	public double SomeDoubleNumber { get; set; }
}
";
			yield return [
				doubleGetter,
				"GetDoubleValue (Test.TestClass.Keys.SomeDoubleNumber)",
				"SetNumberValue (Test.TestClass.Keys.SomeDoubleNumber, value)",
			];

			const string uint32Getter = @"
using System;
using ObjCBindings;

namespace Test;

public class TestClass {

	public static class Keys {
		public static partial NSString SomeUnsignedNumber { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.SomeUnsignedNumber), StrongDictionaryKeyClass = typeof (Keys))]
	public uint SomeUnsignedNumber { get; set; }
}
";
			yield return [
				uint32Getter,
				"GetUInt32Value (Test.TestClass.Keys.SomeUnsignedNumber)",
				"SetNumberValue (Test.TestClass.Keys.SomeUnsignedNumber, value)",
			];

			const string uIntPtrGetter = @"
using System;
using ObjCBindings;

namespace Test;

public class TestClass {

	public static class Keys {
		public static partial NSString SomeUnsignedPointer { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.SomeUnsignedPointer), StrongDictionaryKeyClass = typeof (Keys))]
	public UIntPtr SomeUnsignedPointer { get; set; }
}
";
			yield return [
				uIntPtrGetter,
				"GetNUIntValue (Test.TestClass.Keys.SomeUnsignedPointer)",
				"SetNumberValue (Test.TestClass.Keys.SomeUnsignedPointer, value)",
			];

			const string cgRectGetter = @"
using System;
using ObjCBindings;
using CoreGraphics;

namespace Test;

public class TestClass {

	public static class Keys {
		public static partial NSString SomeRect { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.SomeRect), StrongDictionaryKeyClass = typeof (Keys))]
	public CGRect SomeRect { get; set; }
}
";
			yield return [
				cgRectGetter,
				"GetCGRectValue (Test.TestClass.Keys.SomeRect)",
				"SetCGRectValue (Test.TestClass.Keys.SomeRect, value)",
			];

			const string cgSizeGetter = @"
using System;
using ObjCBindings;
using CoreGraphics;

namespace Test;

public class TestClass {

	public static class Keys {
		public static partial NSString SomeSize { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.SomeSize), StrongDictionaryKeyClass = typeof (Keys))]
	public CGSize SomeSize { get; set; }
}
";
			yield return [
				cgSizeGetter,
				"GetCGSizeValue (Test.TestClass.Keys.SomeSize)",
				"SetCGSizeValue (Test.TestClass.Keys.SomeSize, value)",
			];

			const string cgPointGetter = @"
using System;
using ObjCBindings;
using CoreGraphics;

namespace Test;

public class TestClass {

	public static class Keys {
		public static partial NSString SomePoint { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.SomePoint), StrongDictionaryKeyClass = typeof (Keys))]
	public CGPoint SomePoint { get; set; }
}
";
			yield return [
				cgPointGetter,
				"GetCGPointValue (Test.TestClass.Keys.SomePoint)",
				"SetCGPointValue (Test.TestClass.Keys.SomePoint, value)",
			];

			const string nativeEnumGetter = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace Test;

[Native (""""GKErrorCode"""")]
[BindingType<SmartEnum> (Flags = SmartEnum.ErrorCode, ErrorDomain = """"GKErrorDomain"""")]
public enum NativeSampleEnum : long {
		None = 0,
		Unknown = 1,
}

public class TestClass {

	public static class Keys {
		public static partial NSString SomeEnumValue { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.SomeEnumValue), StrongDictionaryKeyClass = typeof (Keys))]
	public NativeSampleEnum SomeEnumValue { get; set; }
}
";
			yield return [
				nativeEnumGetter,
				"GetNIntValue (Test.TestClass.Keys.SomeEnumValue)",
				"SetNumberValue (Test.TestClass.Keys.SomeEnumValue, value)",
			];

			const string nativeEnumUlongGetter = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace Test;

[Native (""""GKErrorCode"""")]
[BindingType<SmartEnum> (Flags = SmartEnum.ErrorCode, ErrorDomain = """"GKErrorDomain"""")]
public enum NativeSampleEnum : ulong {
		None = 0,
		Unknown = 1,
}

public class TestClass {

	public static class Keys {
		public static partial NSString SomeEnumValue { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.SomeEnumValue), StrongDictionaryKeyClass = typeof (Keys))]
	public NativeSampleEnum SomeEnumValue { get; set; }
}
";
			yield return [
				nativeEnumUlongGetter,
				"GetNUIntValue (Test.TestClass.Keys.SomeEnumValue)",
				"SetNumberValue (Test.TestClass.Keys.SomeEnumValue, value)",
			];

			const string nsDictionaryGetter = @"
using System;
using Foundation;
using ObjCBindings;

namespace Test;

public class TestClass {

	public static class Keys {
		public static partial NSString SomeDictionary { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.SomeDictionary), StrongDictionaryKeyClass = typeof (Keys))]
	public NSDictionary SomeDictionary { get; set; }
}
";
			yield return [
				nsDictionaryGetter,
				"GetNSDictionary (Test.TestClass.Keys.SomeDictionary)",
				"SetNativeValue (Test.TestClass.Keys.SomeDictionary, value)",
			];

			const string genericNsDictionaryGetter = @"
using System;
using Foundation;
using ObjCBindings;

namespace Test;

public class TestClass {

	public static class Keys {
		public static partial NSString SomeDictionary { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.SomeDictionary), StrongDictionaryKeyClass = typeof (Keys))]
	public NSDictionary<NSString, NSObject> SomeDictionary { get; set; }
}
";
			yield return [
				genericNsDictionaryGetter,
				"GetNSDictionary<global::Foundation.NSString, global::Foundation.NSObject> (Test.TestClass.Keys.SomeDictionary)",
				"SetNativeValue (Test.TestClass.Keys.SomeDictionary, value)",
			];

			const string strongDictionaryGetter = @"
using System;
using Foundation;
using ObjCBindings;

namespace Test;

[BindingType<StrongDictionary> ()]
public partial class MyStrongDictionary : DictionaryContainer {
}

public class TestClass {

	public static class Keys {
		public static partial NSString SomeStrongDictionary { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.SomeStrongDictionary), StrongDictionaryKeyClass = typeof (Keys))]
	public MyStrongDictionary SomeStrongDictionary { get; set; }
}
";
			yield return [
				strongDictionaryGetter,
				"GetStrongDictionary<global::Test.MyStrongDictionary> (Test.TestClass.Keys.SomeStrongDictionary, (dict) => new global::Test.MyStrongDictionary (dict))",
				"SetNativeValue (Test.TestClass.Keys.SomeStrongDictionary, value)",
			];

			const string nsValueGetter = @"
using System;
using Foundation;
using ObjCBindings;

namespace Test;

public class TestClass {

	public static class Keys {
		public static partial NSString SomeValue { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.SomeValue), StrongDictionaryKeyClass = typeof (Keys))]
	public NSValue SomeValue { get; set; }
}
";
			yield return [
				nsValueGetter,
				"Dictionary [Test.TestClass.Keys.SomeValue] as global::Foundation.NSValue",
				"SetNativeValue (Test.TestClass.Keys.SomeValue, value)",
			];

			const string imtlCommandQueueGetter = @"
using System;
using Foundation;
using Metal;
using ObjCBindings;

namespace Test;

public class TestClass {

	public static class Keys {
		public static partial NSString SomeQueue { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.SomeQueue), StrongDictionaryKeyClass = typeof (Keys))]
	public IMTLCommandQueue SomeQueue { get; set; }
}
";
			yield return [
				imtlCommandQueueGetter,
				"Dictionary [Test.TestClass.Keys.SomeQueue] as global::Metal.IMTLCommandQueue",
				"SetNativeValue (Test.TestClass.Keys.SomeQueue, value)",
			];

			const string cgColorSpaceGetter = @"
using System;
using CoreGraphics;
using Foundation;
using ObjCBindings;

namespace Test;

public class TestClass {

	public static class Keys {
		public static partial NSString SomeColorSpace { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.SomeColorSpace), StrongDictionaryKeyClass = typeof (Keys))]
	public CGColorSpace SomeColorSpace { get; set; }
}
";
			yield return [
				cgColorSpaceGetter,
				"GetNativeValue<global::CoreGraphics.CGColorSpace> (Test.TestClass.Keys.SomeColorSpace)",
				"SetNativeValue (Test.TestClass.Keys.SomeColorSpace, value)",
			];

			const string cgImageSourceGetter = @"
using System;
using ImageIO;
using Foundation;
using ObjCBindings;

namespace Test;

public class TestClass {

	public static class Keys {
		public static partial NSString SomeImageSource { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.SomeImageSource), StrongDictionaryKeyClass = typeof (Keys))]
	public CGImageSource SomeImageSource { get; set; }
}
";
			yield return [
				cgImageSourceGetter,
				"GetNativeValue<global::ImageIO.CGImageSource> (Test.TestClass.Keys.SomeImageSource)",
				"SetNativeValue (Test.TestClass.Keys.SomeImageSource, value)",
			];

			const string ctFontDescriptorGetter = @"
using System;
using CoreText;
using Foundation;
using ObjCBindings;

namespace Test;

public class TestClass {

	public static class Keys {
		public static partial NSString SomeFontDescriptor { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.SomeFontDescriptor), StrongDictionaryKeyClass = typeof (Keys))]
	public CTFontDescriptor SomeFontDescriptor { get; set; }
}
";
			yield return [
				ctFontDescriptorGetter,
				"GetNativeValue<global::CoreText.CTFontDescriptor> (Test.TestClass.Keys.SomeFontDescriptor)",
				"SetNativeValue (Test.TestClass.Keys.SomeFontDescriptor, value)",
			];

			const string nsValueArrayGetter = @"
using System;
using Foundation;
using ObjCBindings;

namespace Test;

public class TestClass {

	public static class Keys {
		public static partial NSString SomeValueArray { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.SomeValueArray), StrongDictionaryKeyClass = typeof (Keys))]
	public NSValue[] SomeValueArray { get; set; }
}
";
			yield return [
				nsValueArrayGetter,
				"GetArray<global::Foundation.NSValue> (Test.TestClass.Keys.SomeValueArray)",
				"SetArrayValue (Test.TestClass.Keys.SomeValueArray, value)",
			];

			const string imtlCommandQueueArrayGetter = @"
using System;
using Foundation;
using Metal;
using ObjCBindings;

namespace Test;

public class TestClass {

	public static class Keys {
		public static partial NSString SomeQueueArray { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.SomeQueueArray), StrongDictionaryKeyClass = typeof (Keys))]
	public IMTLCommandQueue[] SomeQueueArray { get; set; }
}
";
			yield return [
				imtlCommandQueueArrayGetter,
				"GetArray<global::Metal.IMTLCommandQueue> (Test.TestClass.Keys.SomeQueueArray)",
				"SetArrayValue (Test.TestClass.Keys.SomeQueueArray, value)",
			];

			const string ctFontDescriptorArrayGetter = @"
using System;
using CoreText;
using Foundation;
using ObjCBindings;

namespace Test;

public class TestClass {

	public static class Keys {
		public static partial NSString SomeFontDescriptorArray { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.SomeFontDescriptorArray), StrongDictionaryKeyClass = typeof (Keys))]
	public CTFontDescriptor[] SomeFontDescriptorArray { get; set; }
}
";
			yield return [
				ctFontDescriptorArrayGetter,
				"GetArray<global::CoreText.CTFontDescriptor> (Test.TestClass.Keys.SomeFontDescriptorArray, (ptr) => new global::CoreText.CTFontDescriptor (ptr, false))",
				"SetArrayValue (Test.TestClass.Keys.SomeFontDescriptorArray, value)",
			];

			const string enumArrayGetter = @"
using System;
using ObjCBindings;

namespace Test;

public enum MyEnum : long {
	One = 1,
	Two = 2,
}

public class TestClass {

	public static class Keys {
		public static partial NSString SomeEnumArray { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.SomeEnumArray), StrongDictionaryKeyClass = typeof (Keys))]
	public MyEnum[] SomeEnumArray { get; set; }
}
";
			yield return [
				enumArrayGetter,
				"GetArray<global::Test.MyEnum> (Test.TestClass.Keys.SomeEnumArray, (ptr) => { using var num = global::ObjCRuntime.Runtime.GetNSObject<global::Foundation.NSNumber> (ptr)!; return (global::Test.MyEnum) num.Int64Value; })",
				"SetArrayValue<global::Test.MyEnum[]> (Test.TestClass.Keys.SomeEnumArray, value)",
			];
		}

		IEnumerator IEnumerable.GetEnumerator ()
			=> GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataGetInvocations>]
	void GetInvocationsTests (ApplePlatform platform, string inputText, string expectedGetter, string expectedSetter)
	{
		var (compilation, syntaxTrees) = CreateCompilation (platform, sources: inputText);
		Assert.Single (syntaxTrees);
		var semanticModel = compilation.GetSemanticModel (syntaxTrees [0]);
		var declaration = syntaxTrees [0].GetRoot ()
			.DescendantNodes ().OfType<PropertyDeclarationSyntax> ()
			.LastOrDefault ();
		Assert.NotNull (declaration);
		Assert.True (Property.TryCreate (declaration, semanticModel, out var changes));
		Assert.NotNull (changes);
		var invocations = GetStrongDictionaryInvocations (changes.Value);
		var getter = invocations.Getter.ToString ();
		var setter = invocations.Setter.ToString ();
		Assert.Equal (expectedGetter, invocations.Getter.ToString ());
		Assert.Equal (expectedSetter, invocations.Setter.ToString ());
	}

	[Theory]
	[PlatformInlineData (ApplePlatform.iOS)]
	[PlatformInlineData (ApplePlatform.MacCatalyst)]
	[PlatformInlineData (ApplePlatform.TVOS)]
	void GetInvocationsUIKitTests (ApplePlatform platform)
	{

		const string uiEdgeInsetsGetter = @"
using System;
using ObjCBindings;
using UIKit;

namespace Test;

public class TestClass {

	public static class Keys {
		public static partial NSString SomeInsets { get; }
	}

	[Export<StrongDictionaryProperty> (nameof (Keys.SomeInsets), StrongDictionaryKeyClass = typeof (Keys))]
	public UIEdgeInsets SomeInsets { get; set; }
}
";
		var expectedGetter = "GetUIEdgeInsets (Test.TestClass.Keys.SomeInsets)";
		var expectedSetter = "SetUIEdgeInsets (Test.TestClass.Keys.SomeInsets, value)";
		var (compilation, syntaxTrees) = CreateCompilation (platform, sources: uiEdgeInsetsGetter);
		Assert.Single (syntaxTrees);
		var semanticModel = compilation.GetSemanticModel (syntaxTrees [0]);
		var declaration = syntaxTrees [0].GetRoot ()
			.DescendantNodes ().OfType<PropertyDeclarationSyntax> ()
			.LastOrDefault ();
		Assert.NotNull (declaration);
		Assert.True (Property.TryCreate (declaration, semanticModel, out var changes));
		Assert.NotNull (changes);
		var invocations = GetStrongDictionaryInvocations (changes.Value);
		var getter = invocations.Getter.ToString ();
		var setter = invocations.Setter.ToString ();
		Assert.Equal (expectedGetter, invocations.Getter.ToString ());
		Assert.Equal (expectedSetter, invocations.Setter.ToString ());
	}
}
