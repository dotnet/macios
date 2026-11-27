// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Linq;
using System.Runtime.CompilerServices;

namespace Introspection {
	[TestFixture]
	public partial class ApiStructSizeTest {
		[DllImport ("__Internal")]
		static extern int x_get_struct_size (string nativeName);

		static void CheckSize<T> (string nativeName) where T : struct
		{
			var nativeSize = x_get_struct_size (nativeName);
			Assert.That (nativeSize, Is.GreaterThan (0), $"No native size for {nativeName}");
			Assert.That (Marshal.SizeOf<T> (), Is.EqualTo (nativeSize), $"{typeof (T).FullName} / {nativeName}");
		}

		static void CheckSize (string managedTypeName, string nativeName)
		{
			var type = typeof (NSObject).Assembly.GetType (managedTypeName, throwOnError: true)!;
			var nativeSize = x_get_struct_size (nativeName);
			Assert.That (nativeSize, Is.GreaterThan (0), $"No native size for {nativeName}");
			Assert.That (Marshal.SizeOf (type), Is.EqualTo (nativeSize), $"{type.FullName} / {nativeName}");
		}

		static bool IsCompilerGenerated (Type type)
		{
			return type.IsDefined (typeof (CompilerGeneratedAttribute), false) ||
				(type.DeclaringType is not null && IsCompilerGenerated (type.DeclaringType));
		}

		static bool IsStructTypeForTesting (Type type)
		{
			return type.IsValueType && !type.IsPrimitive && !type.IsEnum && !IsCompilerGenerated (type);
		}

		static Type [] GetUnlistedStructs (IEnumerable<Type> types)
		{
			var testedTypes = testedStructs.Concat (testedInternalStructs.Select (name => typeof (NSObject).Assembly.GetType (name, throwOnError: true)!)).ToHashSet ();
			return types.Where (IsStructTypeForTesting)
				.Where (type => !testedTypes.Contains (type) && !excludedStructs.ContainsKey (type.FullName ?? type.Name))
				.Where (type => !unavailableStructs.Contains (type.FullName ?? type.Name))
				.OrderBy (type => type.FullName, StringComparer.Ordinal)
				.ToArray ();
		}

		[Test]
		public void AllStructsAreListed ()
		{
			Assert.That (excludedStructs.Values, Is.All.Not.Empty, "Every excluded struct must have a reason.");
			var testedNames = testedStructs.Select (type => type.FullName).Concat (testedInternalStructs).ToArray ();
			Assert.That (testedNames.Intersect (excludedStructs.Keys.Concat (unavailableStructs)), Is.Empty,
				"A struct must not be both tested and excluded or unavailable.");
			foreach (var name in testedInternalStructs) {
				var type = typeof (NSObject).Assembly.GetType (name, throwOnError: true)!;
				Assert.That (IsStructTypeForTesting (type), Is.True, $"{name} is not a testable struct.");
			}
			var unlisted = GetUnlistedStructs (typeof (NSObject).Assembly.GetTypes ());
			Assert.That (unlisted, Is.Empty, "Add these structs to WriteStructSizeTests in tests/test-libraries/testgenerator.cs:\n" +
				string.Join ("\n", unlisted.Select (type => type.FullName)));
		}

		struct UnlistedStruct {
		}

		[CompilerGenerated]
		struct GeneratedStruct {
			public struct NestedStruct {
			}
		}

		[Test]
		public void UnlistedStructIsDetected ()
		{
			Assert.That (GetUnlistedStructs ([
				typeof (UnlistedStruct), typeof (GeneratedStruct), typeof (GeneratedStruct.NestedStruct),
				typeof (NSRange), typeof (ObjCRuntime.NativeHandle), typeof (int), typeof (string), typeof (DayOfWeek),
				typeof (AVFoundation.AVCaptureWhiteBalanceGains),
			]),
				Is.EqualTo (new [] { typeof (UnlistedStruct) }));
		}

		[Test]
		public void UnknownStructSize ()
		{
			Assert.That (x_get_struct_size ("NotAnAppleStruct"), Is.EqualTo (-1));
			Assert.Throws<AssertionException> (() => CheckSize<int> ("NotAnAppleStruct"));
		}

		[Test]
		public void IncorrectStructSize ()
		{
			Assert.Throws<AssertionException> (() => CheckSize<int> ("MTLQuadTessellationFactorsHalf"));
			Assert.Throws<AssertionException> (() => CheckSize ("Foundation.NSRange", "MTLQuadTessellationFactorsHalf"));
		}
	}
}
