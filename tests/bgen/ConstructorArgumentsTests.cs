#nullable enable

using System.Collections;

namespace GeneratorTests {

	[TestFixture]
	[Parallelizable (ParallelScope.All)]
	public class ConstructorArgumentsTests {

		[Test]
		public void GetCtorValuesNullVersion ()
		{
			var args = new AttributeFactory.ConstructorArguments (PlatformName.iOS, "test");
			var values = args.GetCtorValues ();
			Assert.That (values.Length, Is.EqualTo (2), "Length");
			Assert.That (values [0], Is.EqualTo ((byte) PlatformName.iOS), "Platform");
			Assert.That (values [1], Is.EqualTo ("test"), "Message");
		}

		[Test]
		public void GetCtorValuesNullBuild ()
		{
			var args = new AttributeFactory.ConstructorArguments (PlatformName.iOS, 13, 0, "test");
			var values = args.GetCtorValues ();
			Assert.That (values.Length, Is.EqualTo (4), "Length");
			Assert.That (values [0], Is.EqualTo ((byte) PlatformName.iOS), "Platform");
			Assert.That (values [1], Is.EqualTo (13), "Major");
			Assert.That (values [2], Is.EqualTo (0), "Minor");
			Assert.That (values [3], Is.EqualTo ("test"), "Message");
		}

		[Test]
		public void GetCtorValuesFullVersion ()
		{
			var args = new AttributeFactory.ConstructorArguments (PlatformName.iOS, 13, 0, 1, "test");
			var values = args.GetCtorValues ();
			Assert.That (values.Length, Is.EqualTo (5), "Length");
			Assert.That (values [0], Is.EqualTo ((byte) PlatformName.iOS), "Platform");
			Assert.That (values [1], Is.EqualTo (13), "Major");
			Assert.That (values [2], Is.EqualTo (0), "Minor");
			Assert.That (values [3], Is.EqualTo (1), "Build");
			Assert.That (values [4], Is.EqualTo ("test"), "Message");
		}

		[Test]
		public void GetCtorTypesNullVersion ()
		{
			var args = new AttributeFactory.ConstructorArguments (PlatformName.iOS, "test");
			var types = args.GetCtorTypes ();
			Assert.That (types.Length, Is.EqualTo (2), "Length");
			Assert.That (types [0], Is.EqualTo (typeof (PlatformName)), "Platform");
			Assert.That (types [1], Is.EqualTo (typeof (string)), "Message");
		}

		[Test]
		public void GetCtorTypesNullBuild ()
		{
			var args = new AttributeFactory.ConstructorArguments (PlatformName.iOS, 13, 0, "test");
			var types = args.GetCtorTypes ();
			Assert.That (types.Length, Is.EqualTo (4), "Length");
			Assert.That (types [0], Is.EqualTo (typeof (PlatformName)), "Platform");
			Assert.That (types [1], Is.EqualTo (typeof (int)), "Major");
			Assert.That (types [2], Is.EqualTo (typeof (int)), "Minor");
			Assert.That (types [3], Is.EqualTo (typeof (string)), "Message");
		}

		[Test]
		public void GetCtorTypesFullVersion ()
		{
			var args = new AttributeFactory.ConstructorArguments (PlatformName.iOS, 13, 0, 1, "test");
			var types = args.GetCtorTypes ();
			Assert.That (types.Length, Is.EqualTo (5), "Length");
			Assert.That (types [0], Is.EqualTo (typeof (PlatformName)), "Platform");
			Assert.That (types [1], Is.EqualTo (typeof (int)), "Major");
			Assert.That (types [2], Is.EqualTo (typeof (int)), "Minor");
			Assert.That (types [3], Is.EqualTo (typeof (int)), "Build");
			Assert.That (types [4], Is.EqualTo (typeof (string)), "Message");
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
			Assert.That (success, Is.True, "success");
			Assert.That (actualValues!.Length, Is.EqualTo (expectedValues!.Length), "Values Length");
			for (int index = 0; index < expectedValues.Length; index++) {
				Assert.That (actualValues [index], Is.EqualTo (expectedValues [index]), $"Values [{index}]");
			}
			Assert.That (actualTypes!.Length, Is.EqualTo (expectedTypes!.Length), "Types Length");
			for (int index = 0; index < expectedTypes.Length; index++) {
				Assert.That (actualTypes [index], Is.EqualTo (expectedTypes [index]), $"Types [{index}]");
			}
		}

		[Test]
		public void TryGetCtorArgumentsFail ()
		{
			var success = AttributeFactory.ConstructorArguments.TryGetCtorArguments (Array.Empty<object> (), PlatformName.iOS,
				out var actualValues, out var actualTypes);
			Assert.That (success, Is.False, "success");
			Assert.That (actualValues, Is.Null, "values");
			Assert.That (actualTypes, Is.Null, "type");
		}
	}
}
