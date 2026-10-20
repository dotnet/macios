// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;
using System.Linq;

using Mono.Cecil;
using Mono.Cecil.Cil;

using Xamarin.Tests;

namespace GeneratorTests {
	[TestFixture]
	[Parallelizable (ParallelScope.All)]
	public class PointerParameterTests : BGenBase {
		[TestCase (Profile.iOS)]
		[TestCase (Profile.tvOS)]
		[TestCase (Profile.macOSMobile)]
		[TestCase (Profile.MacCatalyst)]
		public void PointerParameters (Profile profile)
		{
			var bgen = BuildFile (profile, "pointer-parameters.cs");
			var type = bgen.ApiAssembly.MainModule.GetType ("PointerParameters.PointerBindings");
			foreach (var name in new [] { "Points", "Ranges", "Prefix", "Unsigned", "Three", "Result", "Point", "Empty", "Nullable", "Member", "StaticMember", "ConstantMember", "TwoArrays", "Keyword" }) {
				var method = type.Methods.Single (method => method.Name == name);
				Assert.That (method.IsVirtual, Is.False, name + ": outgoing only");
				Assert.That (method.CustomAttributes.Any (attribute => attribute.AttributeType.Name == "ExportAttribute"), Is.False, name + ": no callback export");
				Assert.That (method.Body.Variables.Any (variable => variable.VariableType.IsPinned), Is.True, name + ": pinned array");
				Assert.That (method.Body.Instructions.Any (instruction => instruction.OpCode == OpCodes.Newobj &&
					instruction.Operand is MethodReference ctor && ctor.DeclaringType.FullName == "System.ArgumentOutOfRangeException"), Is.True, name + ": length check");
				Assert.That (method.Body.Instructions.Where (instruction => instruction.Operand is MethodReference)
					.Select (instruction => ((MethodReference) instruction.Operand).DeclaringType.FullName), Does.Not.Contain ("Foundation.NSArray"), name + ": not NSArray");
			}

			Assert.That (type.Methods.Single (method => method.Name == "Ranges").IsStatic, Is.True, "static array method");
			Assert.That (type.Methods.Single (method => method.Name == "Pointer").Parameters [0].ParameterType.IsPointer, Is.True, "raw pointer");
			Assert.That (type.Methods.Single (method => method.Name == "Reference").Parameters [0].ParameterType.IsByReference, Is.True, "ref struct");
			Assert.That (type.Methods.Single (method => method.Name == "Output").Parameters [0].IsOut, Is.True, "out struct");
			var objects = type.Methods.Single (method => method.Name == "Objects");
			Assert.That (objects.IsVirtual, Is.True, "ordinary arrays remain overridable");
			Assert.That (objects.Body.Instructions.Any (instruction => instruction.Operand is MethodReference called &&
				called.DeclaringType.FullName == "Foundation.NSArray"), Is.True, "ordinary NSArray marshalling");

			var generated = string.Join ("\n", Directory.GetFiles (bgen.TmpDirectory ?? throw new InvalidOperationException ("Missing generated source directory."),
				"*.cs", SearchOption.AllDirectories).Select (File.ReadAllText));
			Assert.That (generated, Does.Contain ("var __pointer_array_length_0 = this.Count;"), "member length evaluated once");
			Assert.That (generated, Does.Contain ("(ulong) __pointer_array_length_0 > (ulong) (values?.LongLength ?? 0)"), "prefix bound");
			Assert.That (generated, Does.Contain ("__pointer_array_length_0 < 0 ||"), "negative signed count");
			Assert.That (generated, Does.Contain ("fixed (CGPoint* @class__pointer = @class)"), "escaped parameter");
		}

		[TestCase ("[PointerAsStruct] CGPoint value", "PointerAsStruct requires")]
		[TestCase ("[PointerAsStruct] int* value", "blittable struct")]
		[TestCase ("[PointerAsStruct] CGPoint** value", "blittable struct")]
		[TestCase ("[PointerAsStruct] in CGPoint value", "in parameters")]
		[TestCase ("[PointerAsArray (Length = 1)] ref CGPoint [] value", "one-dimensional array")]
		[TestCase ("[PointerAsArray (Length = 1)] CGPoint [,] value", "one-dimensional array")]
		[TestCase ("[PointerAsArray (Length = 1)] string [] value", "blittable struct")]
		[TestCase ("[PointerAsArray (Length = 1)] int [] value", "blittable struct")]
		[TestCase ("[PointerAsArray (Length = 1)] CGPoint? [] value", "blittable struct")]
		[TestCase ("[PointerAsArray (Length = 1)] ManagedStruct [] value", "blittable struct")]
		[TestCase ("[PointerAsArray (Length = 1)] BooleanStruct [] value", "blittable struct")]
		[TestCase ("[PointerAsArray (Length = 1)] AutoStruct [] value", "blittable struct")]
		[TestCase ("[PointerAsArray (Length = 1)] MarshalStruct [] value", "blittable struct")]
		[TestCase ("[PointerAsStruct, PointerAsArray (Length = 1)] CGPoint* value", "cannot be combined")]
		[TestCase ("[PointerAsArray] CGPoint [] value", "exactly one")]
		[TestCase ("[PointerAsArray (Length = -1)] CGPoint [] value", "exactly one")]
		[TestCase ("[PointerAsArray (Length = -2)] CGPoint [] value", "exactly one")]
		[TestCase ("[PointerAsArray (Length = -1, LengthFromMember = \"Count\")] CGPoint [] value", "exactly one")]
		[TestCase ("[PointerAsArray (Length = 1, LengthFromMember = \"Count\")] CGPoint [] value", "exactly one")]
		[TestCase ("[PointerAsArray (LengthFromParameter = \"missing\")] CGPoint [] value", "integral parameter")]
		[TestCase ("[PointerAsArray (LengthFromParameter = \"value\")] CGPoint [] value", "integral type")]
		[TestCase ("[PointerAsArray (LengthFromMember = \"missing\")] CGPoint [] value", "readable integral")]
		[TestCase ("[PointerAsArray (LengthFromMember = \"Count\")] CGPoint [] value", "integral type")]
		public void InvalidPointerParameter (string parameter, string diagnostic)
		{
			Configuration.IgnoreIfIgnoredPlatform (Profile.macOSMobile.AsPlatform ());
			var bgen = new BGenTool { Profile = Profile.macOSMobile };
			bgen.CreateTemporaryBinding ($$"""
				using CoreGraphics;
				using Foundation;
				using ObjCRuntime;
				using System.Runtime.InteropServices;
				namespace PointerErrors {
					struct ManagedStruct { public string Text; }
					struct BooleanStruct { public bool Value; }
					[StructLayout (LayoutKind.Auto)]
					struct AutoStruct { public int Value; }
					struct MarshalStruct { [MarshalAs (UnmanagedType.I4)] public bool Value; }
					[BaseType (typeof (NSObject))]
					interface Binding {
						[Export ("method:")]
						unsafe void Method ({{parameter}});
						[Export ("count")]
						string Count { get; }
					}
				}
				""");
			bgen.AssertExecuteError ("invalid pointer parameter");
			bgen.AssertErrorPattern (1130, $".*{diagnostic}.*");
		}

		[TestCase ("[Protocol]", "", "void Method ([PointerAsArray (Length = 1)] CGPoint [] value);", "protocols")]
		[TestCase ("[Model]", "", "void Method ([PointerAsArray (Length = 1)] CGPoint [] value);", "models")]
		[TestCase ("", "[Abstract]", "void Method ([PointerAsArray (Length = 1)] CGPoint [] value);", "non-overridable")]
		[TestCase ("", "[Override]", "void Method ([PointerAsArray (Length = 1)] CGPoint [] value);", "non-overridable")]
		[TestCase ("", "[Static]", "void Method ([PointerAsArray (LengthFromMember = \"Count\")] CGPoint [] value);", "static length member")]
		[TestCase ("", "", "NativeHandle Constructor ([PointerAsArray (LengthFromMember = \"Count\")] CGPoint [] value);", "static length member")]
		[TestCase ("", "", "void Method ([PointerAsArray (LengthFromParameter = \"count\")] CGPoint [] value, ref int count);", "integral parameter")]
		[TestCase ("", "", "void Method ([PointerAsArray (LengthFromParameter = \"count\")] CGPoint [] value, out int count);", "integral parameter")]
		[TestCase ("", "", "void Method ([PointerAsArray (LengthFromParameter = \"count\")] CGPoint [] value, float count);", "integral type")]
		[TestCase ("", "", "void Method ([PointerAsArray (LengthFromParameter = \"count\")] CGPoint [] value, bool count);", "integral type")]
		public void InvalidArrayContext (string typeAttributes, string methodAttributes, string method, string diagnostic)
		{
			Configuration.IgnoreIfIgnoredPlatform (Profile.macOSMobile.AsPlatform ());
			var bgen = new BGenTool { Profile = Profile.macOSMobile };
			bgen.CreateTemporaryBinding ($$"""
				using CoreGraphics;
				using Foundation;
				using ObjCRuntime;
				namespace PointerErrors {
					{{typeAttributes}}
					[BaseType (typeof (NSObject))]
					interface Binding {
						[Export ("method:")]
						{{methodAttributes}}
						{{method}}
						[Export ("count")]
						int Count { get; }
					}
				}
				""");
			bgen.AssertExecuteError ("invalid array context");
			bgen.AssertErrorPattern (1130, $".*{diagnostic}.*");
		}

		[Test]
		public void InvalidArrayCallback ()
		{
			Configuration.IgnoreIfIgnoredPlatform (Profile.macOSMobile.AsPlatform ());
			var bgen = new BGenTool { Profile = Profile.macOSMobile };
			bgen.CreateTemporaryBinding ("""
				using CoreGraphics;
				using Foundation;
				namespace PointerErrors {
					delegate void Callback ([PointerAsArray (Length = 1)] CGPoint [] value);
					[BaseType (typeof (NSObject))]
					interface Binding {
						[Export ("method:")]
						void Method (Callback callback);
					}
				}
				""");
			bgen.AssertExecuteError ("invalid array callback");
			bgen.AssertErrorPattern (1130, ".*callbacks.*");
		}
	}
}
