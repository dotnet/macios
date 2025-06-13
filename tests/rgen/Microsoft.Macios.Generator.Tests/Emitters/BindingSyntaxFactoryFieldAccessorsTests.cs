// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;
using Xamarin.Tests;
using Xamarin.Utils;
using Xunit;
using static Microsoft.Macios.Generator.Emitters.BindingSyntaxFactory;

namespace Microsoft.Macios.Generator.Tests.Emitters;

public class BindingSyntaxFactoryFieldAccessorsTests : BaseGeneratorTestClass {

	class TestDataFieldConstantGetter : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			const string nsStringFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial NSString GenericGray { get; }

	}
}
";

			yield return [nsStringFieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetStringConstant ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")!"];

			const string byteFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial byte GenericGray { get; }

	}
}
";

			yield return [byteFieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetByte ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"];

			const string otherByteFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial Byte GenericGray { get; }

	}
}
";
			yield return [otherByteFieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetByte ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"];

			const string sbyteFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial sbyte GenericGray { get; }

	}
}
";

			yield return [sbyteFieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetSByte ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"];

			const string otherSbyteFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial SByte GenericGray { get; }

	}
}
";

			yield return [otherSbyteFieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetSByte ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"];

			const string int16FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial short GenericGray { get; }

	}
}
";

			yield return [int16FieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetInt16 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"];

			const string otherInt16FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial Int16 GenericGray { get; }

	}
}
";

			yield return [otherInt16FieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetInt16 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"];

			const string uint16FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial ushort GenericGray { get; }

	}
}
";

			yield return [uint16FieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetUInt16 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"];

			const string otherUint16FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial UInt16 GenericGray { get; }

	}
}
";

			yield return [otherUint16FieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetUInt16 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"];

			const string int32FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial int GenericGray { get; }

	}
}
";

			yield return [int32FieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetInt32 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"];

			const string otherInt32FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial Int32 GenericGray { get; }

	}
}
";

			yield return [otherInt32FieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetInt32 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"];

			const string uint32FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial uint GenericGray { get; }

	}
}
";

			yield return [uint32FieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetUInt32 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"];

			const string otherUint32FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial UInt32 GenericGray { get; }

	}
}
";

			yield return [otherUint32FieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetUInt32 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"];

			const string doubleFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial double GenericGray { get; }

	}
}
";

			yield return [doubleFieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetDouble ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"];

			const string otherDoubleFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial Double GenericGray { get; }

	}
}
";

			yield return [otherDoubleFieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetDouble ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"];

			const string floatFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial float GenericGray { get; }

	}
}
";

			yield return [floatFieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetFloat ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"];

			const string otherFloatFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial Single GenericGray { get; }

	}
}
";

			yield return [otherFloatFieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetFloat ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"];

			const string intPtrFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial IntPtr GenericGray { get; }

	}
}
";

			yield return [intPtrFieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetIntPtr ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"];

			const string uintPtrFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial UIntPtr GenericGray { get; }

	}
}
";

			yield return [uintPtrFieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetUIntPtr ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"];

			const string nintFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial nint GenericGray { get; }

	}
}
";

			yield return [nintFieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetIntPtr ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"];

			const string nuintFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial nuint GenericGray { get; }

	}
}
";

			yield return [nuintFieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetUIntPtr ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"];

			const string nfloatFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial nfloat GenericGray { get; }

	}
}
";

			yield return [nfloatFieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetNFloat ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"];

			const string cgsizeFieldProperty = @"
using System;
using Foundation;
using CoreGraphics;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGSize GenericGray { get; }

	}
}
";

			yield return [cgsizeFieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetCGSize ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"];

			const string cmtagFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CMTag GenericGray { get; }

	}
}
";

			yield return [cmtagFieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetStruct<{Global ("CoreMedia.CMTag")}> ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"];

			const string nsArrayFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial NSArray GenericGray { get; }

	}
}
";

			yield return [nsArrayFieldProperty,
				$"{Global ("ObjCRuntime.Runtime")}.GetNSObject<{Global ("Foundation.NSArray")}> ({Global ("ObjCRuntime.Dlfcn")}.GetIndirect ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\"))!"];

			const string nsNumberFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial NSNumber GenericGray { get; }

	}
}
";

			yield return [nsNumberFieldProperty,
				$"{Global ("ObjCRuntime.Runtime")}.GetNSObject<{Global ("Foundation.NSNumber")}> ({Global ("ObjCRuntime.Dlfcn")}.GetIndirect ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\"))!"];

			const string sbyteEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : sbyte{
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				sbyteEnumFieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetSByte ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"
			];

			const string byteEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : byte{
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				byteEnumFieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetByte ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"
			];

			const string shortEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : short {
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				shortEnumFieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetInt16 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"
			];

			const string ushortEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : ushort {
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				ushortEnumFieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetUInt16 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"
			];

			const string intEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : int {
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				intEnumFieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetInt32 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"
			];

			const string uintEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : uint {
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				uintEnumFieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetUInt32 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"
			];

			const string longEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : long {
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				longEnumFieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetInt64 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"
			];

			const string ulongEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : ulong {
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				ulongEnumFieldProperty,
				$"{Global ("ObjCRuntime.Dlfcn")}.GetUInt64 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\")"
			];

			const string cmTimeFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CMTime GenericGray { get; }

	}
}
";

			yield return [
				cmTimeFieldProperty,
				$"*(({Global ("CoreMedia.CMTime")}*) {Global ("ObjCRuntime.Dlfcn")}.dlsym ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\"))"
			];

			const string whiteFieldProperty = @"
using System;
using AVFoundation;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial AVCaptureWhiteBalanceGains GenericGray { get; }

	}
}
";

			yield return [
				whiteFieldProperty,
				$"*(({Global ("AVFoundation.AVCaptureWhiteBalanceGains")}*) {Global ("ObjCRuntime.Dlfcn")}.dlsym ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\"))"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataFieldConstantGetter>]
	void FieldConstantGetterTests (ApplePlatform platform, string inputText, string expectedCall)
	{
		var (compilation, sourceTrees) =
			CreateCompilation (platform, sources: inputText);
		Assert.Single (sourceTrees);
		// get the declarations we want to work with and the semantic model
		var node = sourceTrees [0].GetRoot ()
			.DescendantNodes ()
			.OfType<PropertyDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (node);
		var semanticModel = compilation.GetSemanticModel (sourceTrees [0]);
		var context = new RootContext (semanticModel);
		Assert.True (Property.TryCreate (node, context, out var property));
		var compilationUnit = FieldConstantGetter (property.Value).ToString ();
		var str = compilationUnit.ToString ();
		Assert.Equal (expectedCall, FieldConstantGetter (property.Value).ToString ());
	}

	class TestDataFieldConstantSetter : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			const string nsStringFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial NSString GenericGray { get; }

	}
}
";

			yield return [nsStringFieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetString ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string byteFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial byte GenericGray { get; }

	}
}
";

			yield return [byteFieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetByte ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string otherByteFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial Byte GenericGray { get; }

	}
}
";
			yield return [otherByteFieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetByte ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string sbyteFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial sbyte GenericGray { get; }

	}
}
";

			yield return [sbyteFieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetSByte ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string otherSbyteFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial SByte GenericGray { get; }

	}
}
";

			yield return [otherSbyteFieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetSByte ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string int16FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial short GenericGray { get; }

	}
}
";

			yield return [int16FieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetInt16 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string otherInt16FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial Int16 GenericGray { get; }

	}
}
";

			yield return [otherInt16FieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetInt16 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string uint16FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial ushort GenericGray { get; }

	}
}
";

			yield return [uint16FieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetUInt16 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string otherUint16FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial UInt16 GenericGray { get; }

	}
}
";

			yield return [otherUint16FieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetUInt16 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string int32FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial int GenericGray { get; }

	}
}
";

			yield return [int32FieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetInt32 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string otherInt32FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial Int32 GenericGray { get; }

	}
}
";

			yield return [otherInt32FieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetInt32 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string uint32FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial uint GenericGray { get; }

	}
}
";

			yield return [uint32FieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetUInt32 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string otherUint32FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial UInt32 GenericGray { get; }

	}
}
";

			yield return [otherUint32FieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetUInt32 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string doubleFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial double GenericGray { get; }

	}
}
";

			yield return [doubleFieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetDouble ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string otherDoubleFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial Double GenericGray { get; }

	}
}
";

			yield return [otherDoubleFieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetDouble ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string floatFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial float GenericGray { get; }

	}
}
";

			yield return [floatFieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetFloat ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string otherFloatFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial Single GenericGray { get; }

	}
}
";

			yield return [otherFloatFieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetFloat ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string intPtrFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial IntPtr GenericGray { get; }

	}
}
";

			yield return [intPtrFieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetIntPtr ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string uintPtrFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial UIntPtr GenericGray { get; }

	}
}
";

			yield return [uintPtrFieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetUIntPtr ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string nintFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial nint GenericGray { get; }

	}
}
";

			yield return [nintFieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetIntPtr ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string nuintFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial nuint GenericGray { get; }

	}
}
";

			yield return [nuintFieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetUIntPtr ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string nfloatFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial nfloat GenericGray { get; }

	}
}
";

			yield return [nfloatFieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetNFloat ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string cgsizeFieldProperty = @"
using System;
using Foundation;
using CoreGraphics;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGSize GenericGray { get; }

	}
}
";

			yield return [cgsizeFieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetCGSize ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string cmtagFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CMTag GenericGray { get; }

	}
}
";

			yield return [
				cmtagFieldProperty,
				"value",
				"throw new NotSupportedException(\"Setting fields of type 'CoreMedia.CMTag' is not supported.\")"];

			const string nsArrayFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial NSArray GenericGray { get; }

	}
}
";

			yield return [nsArrayFieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetArray ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string nsNumberFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial NSNumber GenericGray { get; }

	}
}
";

			yield return [nsNumberFieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetObject ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string sbyteEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : sbyte{
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				sbyteEnumFieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetSByte ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", (sbyte)value)"
			];

			const string byteEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : byte{
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				byteEnumFieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetByte ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", (byte)value)"
			];

			const string shortEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : short {
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				shortEnumFieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetInt16 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", (short)value)"
			];

			const string ushortEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : ushort {
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				ushortEnumFieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetUInt16 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", (ushort)value)"
			];

			const string intEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : int {
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				intEnumFieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetInt32 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", (int)value)"
			];

			const string uintEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : uint {
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				uintEnumFieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetUInt32 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", (uint)value)"
			];

			const string longEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : long {
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				longEnumFieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetInt64 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", (long)value)"
			];

			const string ulongEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : ulong {
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				ulongEnumFieldProperty,
				"value",
				$"{Global ("ObjCRuntime.Dlfcn")}.SetUInt64 ({Global ("ObjCRuntime.Libraries.CoreGraphics")}.Handle, \"kCGColorSpaceGenericGray\", (ulong)value)"
			];

			const string cmTimeFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CMTime GenericGray { get; }

	}
}
";

			yield return [
				cmTimeFieldProperty,
				"value",
				"throw new NotSupportedException(\"Setting fields of type 'CoreMedia.CMTime' is not supported.\")"];

			const string whiteFieldProperty = @"
using System;
using AVFoundation;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial AVCaptureWhiteBalanceGains GenericGray { get; }

	}
}
";

			yield return [
				whiteFieldProperty,
				"value",
				"throw new NotSupportedException(\"Setting fields of type 'AVFoundation.AVCaptureWhiteBalanceGains' is not supported.\")"];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataFieldConstantSetter>]
	void FieldConstantSetterTests (ApplePlatform platform, string inputText, string variableName, string expectedCall)
	{
		var (compilation, sourceTrees) =
			CreateCompilation (platform, sources: inputText);
		Assert.Single (sourceTrees);
		// get the declarations we want to work with and the semantic model
		var node = sourceTrees [0].GetRoot ()
			.DescendantNodes ()
			.OfType<PropertyDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (node);
		var semanticModel = compilation.GetSemanticModel (sourceTrees [0]);
		var context = new RootContext (semanticModel);
		Assert.True (Property.TryCreate (node, context, out var property));
		Assert.Equal (expectedCall, FieldConstantSetter (property.Value, variableName).ToString ());
	}
}
