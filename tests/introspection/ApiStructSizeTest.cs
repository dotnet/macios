// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

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
