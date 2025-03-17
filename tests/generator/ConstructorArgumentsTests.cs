#nullable enable

using System;
using System.Collections;
using NUnit.Framework;
using ObjCRuntime;

namespace GeneratorTests {

	[TestFixture]
	[Parallelizable (ParallelScope.All)]
	public class ConstructorArgumentsTests {

		[Test]
		public void GetCtorValuesNullVersion ()
		{
			var args = new AttributeFactory.ConstructorArguments (PlatformName.iOS, "test");
			var values = args.GetCtorValues ();
			Assert.AreEqual (2, values.Length, "Length");
			Assert.AreEqual ((byte) PlatformName.iOS, values [0], "Platform");
			Assert.AreEqual ("test", values [1], "Message");
		}

		[Test]
		public void GetCtorValuesNullBuild ()
		{
			var args = new AttributeFactory.ConstructorArguments (PlatformName.iOS, 13, 0, "test");
			var values = args.GetCtorValues ();
			Assert.AreEqual (4, values.Length, "Length");
			Assert.AreEqual ((byte) PlatformName.iOS, values [0], "Platform");
			Assert.AreEqual (13, values [1], "Major");
			Assert.AreEqual (0, values [2], "Minor");
			Assert.AreEqual ("test", values [3], "Message");
		}

		[Test]
		public void GetCtorValuesFullVersion ()
		{
			var args = new AttributeFactory.ConstructorArguments (PlatformName.iOS, 13, 0, 1, "test");
			var values = args.GetCtorValues ();
			Assert.AreEqual (5, values.Length, "Length");
			Assert.AreEqual ((byte) PlatformName.iOS, values [0], "Platform");
			Assert.AreEqual (13, values [1], "Major");
			Assert.AreEqual (0, values [2], "Minor");
			Assert.AreEqual (1, values [3], "Build");
			Assert.AreEqual ("test", values [4], "Message");
		}

		[Test]
		public void GetCtorTypesNullVersion ()
		{
			var args = new AttributeFactory.ConstructorArguments (PlatformName.iOS, "test");
			var types = args.GetCtorTypes ();
			Assert.AreEqual (2, types.Length, "Length");
			Assert.AreEqual (typeof (PlatformName), types [0], "Platform");
			Assert.AreEqual (typeof (string), types [1], "Message");
		}

		[Test]
		public void GetCtorTypesNullBuild ()
		{
			var args = new AttributeFactory.ConstructorArguments (PlatformName.iOS, 13, 0, "test");
			var types = args.GetCtorTypes ();
			Assert.AreEqual (4, types.Length, "Length");
			Assert.AreEqual (typeof (PlatformName), types [0], "Platform");
			Assert.AreEqual (typeof (int), types [1], "Major");
			Assert.AreEqual (typeof (int), types [2], "Minor");
			Assert.AreEqual (typeof (string), types [3], "Message");
		}

		[Test]
		public void GetCtorTypesFullVersion ()
		{
			var args = new AttributeFactory.ConstructorArguments (PlatformName.iOS, 13, 0, 1, "test");
			var types = args.GetCtorTypes ();
			Assert.AreEqual (5, types.Length, "Length");
			Assert.AreEqual (typeof (PlatformName), types [0], "Platform");
			Assert.AreEqual (typeof (int), types [1], "Major");
			Assert.AreEqual (typeof (int), types [2], "Minor");
			Assert.AreEqual (typeof (int), types [3], "Build");
			Assert.AreEqual (typeof (string), types [4], "Message");
		}

		class TryGetArgumentsData : IEnumerable {
			public IEnumerator GetEnumerator ()
			{
				yield return new TestCaseData (
					new object [] { (byte) 13, (byte) 0 },
					PlatformName.iOS,
					new object? [] { (byte) PlatformName.iOS, (int) (byte) 13, (int) (byte) 0, null },
					new [] { typeof (PlatformName), typeof (int), typeof (int), typeof (string) }
				);

				yield return new TestCaseData (
					new object [] { (byte) 13, (byte) 0, (byte) 1 },
					PlatformName.iOS,
					new object? [] { (byte) PlatformName.iOS, (int) (byte) 13, (int) (byte) 0, (int) (byte) 1, null },
					new [] { typeof (PlatformName), typeof (int), typeof (int), typeof (int), typeof (string) }
				);
			}
		}

		[TestCaseSource (typeof (TryGetArgumentsData))]
		public void TryGetCtorArguments (object [] arguments, PlatformName platformName, object [] expectedValues,
			Type [] expectedTypes)
		{
			var success = AttributeFactory.ConstructorArguments.TryGetCtorArguments (arguments, platformName,
				out var actualValues, out var actualTypes);
			Assert.True (success, "success");
			Assert.AreEqual (expectedValues!.Length, actualValues!.Length, "Values Length");
			for (int index = 0; index < expectedValues.Length; index++) {
				Assert.AreEqual (expectedValues [index], actualValues [index], $"Values [{index}]");
			}
			Assert.AreEqual (expectedTypes!.Length, actualTypes!.Length, "Types Length");
			for (int index = 0; index < expectedTypes.Length; index++) {
				Assert.AreEqual (expectedTypes [index], actualTypes [index], $"Types [{index}]");
			}
		}

		[Test]
		public void TryGetCtorArgumentsFail ()
		{
			var success = AttributeFactory.ConstructorArguments.TryGetCtorArguments (Array.Empty<object> (), PlatformName.iOS,
				out var actualValues, out var actualTypes);
			Assert.False (success, "success");
			Assert.Null (actualValues, "values");
			Assert.Null (actualTypes, "type");
		}
	}
}
