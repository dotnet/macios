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

		static bool IsCompilerGenerated (Type type)
		{
			return type.IsDefined (typeof (CompilerGeneratedAttribute), false) ||
				(type.DeclaringType is not null && IsCompilerGenerated (type.DeclaringType));
		}

		static Type [] GetUnlistedStructs (IEnumerable<Type> types)
		{
			return types.Where (type => type.IsValueType && !type.IsPrimitive && !type.IsEnum && !IsCompilerGenerated (type))
				.Where (type => !testedStructs.Contains (type) && !excludedStructs.ContainsKey (type.FullName ?? type.Name))
				.Where (type => !unavailableStructs.Contains (type.FullName ?? type.Name))
				.OrderBy (type => type.FullName, StringComparer.Ordinal)
				.ToArray ();
		}

		[Test]
		public void AllStructsAreListed ()
		{
			Assert.That (excludedStructs.Values, Is.All.Not.Empty, "Every excluded struct must have a reason.");
			Assert.That (testedStructs.Select (type => type.FullName).Intersect (excludedStructs.Keys.Concat (unavailableStructs)), Is.Empty,
				"A struct must not be both tested and excluded or unavailable.");
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
		}
	}
}
