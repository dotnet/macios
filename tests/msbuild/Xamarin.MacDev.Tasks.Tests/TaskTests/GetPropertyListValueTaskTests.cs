using System;
using System.IO;
using NUnit.Framework;

using Xamarin.MacDev;
using Xamarin.MacDev.Tasks;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class GetPropertyListValueTaskTests : TestBase {

		void TestExecuteTask (string property, string expected)
		{
			var path = Path.Combine (Path.GetDirectoryName (GetType ().Assembly.Location)!, "Resources", "PropertyList.plist");
			var task = CreateTask<GetPropertyListValue> ();
			task.PropertyListFile = path;
			task.Property = property;

			if (expected is null) {
				ExecuteTask (task, 1);
				return;
			}

			ExecuteTask (task);

			Assert.AreEqual (expected, task.Value, "Task produced the incorrect plist output.");
		}

		[Test]
		public void TestWithColonPrefix ()
		{
			const string property = "KeyString";
			const string expected = "MarLoe";

			TestExecuteTask (":" + property, expected);
		}


		[Test]
		public void TestWithoutColonPrefix ()
		{
			const string property = "KeyString";
			const string expected = "MarLoe";

			TestExecuteTask (property, expected);
		}

		[Test]
		public void TestReturnArrayNotSupported ()
		{
			const string property = "KeyArray";

			TestExecuteTask (property, null);
		}

		[Test]
		public void TestReturnDictionaryNotSupported ()
		{
			const string property = "KeyDictionary";

			TestExecuteTask (property, null);
		}

		[Test]
		public void TestReturnDataNotSupported ()
		{
			const string property = "KeyData";

			TestExecuteTask (property, null);
		}

		[Test]
		public void TestKeyNotFound ()
		{
			const string property = "ThisKeyDoesNotExist";

			TestExecuteTask (property, null);
		}

		[Test]
		public void TestGetStringProperty ()
		{
			const string property = "KeyString";
			const string expected = "MarLoe";

			TestExecuteTask (property, expected);
		}

		[Test]
		public void TestGetIntegerProperty ()
		{
			const string property = "KeyNumberInt";
			const string expected = "1013";

			TestExecuteTask (property, expected);
		}

		[Test]
		public void TestGetDoubleProperty ()
		{
			const string property = "KeyNumberDouble";
			const string expected = "10.13";

			TestExecuteTask (property, expected);
		}

		[Test]
		public void TestGetBooleanTrueProperty ()
		{
			const string property = "KeyBooleanTrue";
			const string expected = "True";

			TestExecuteTask (property, expected);
		}

		[Test]
		public void TestGetBooleanFalseProperty ()
		{
			const string property = "KeyBooleanFalse";
			const string expected = "False";

			TestExecuteTask (property, expected);
		}

		[Test]
		public void TestGetDateProperty ()
		{
			const string property = "KeyDate";
			const string expected = "11/19/1974 10:20:30 AM";

			TestExecuteTask (property, expected);
		}

		[Test]
		public void TestGetArrayIndexOutOfBounds ()
		{
			var property = $"KeyArray:99";

			TestExecuteTask (property, null);
		}

		[Test]
		[TestCase (0)]
		[TestCase (1)]
		[TestCase (2)]
		public void TestGetArrayIndexProperty (int index)
		{
			var property = $"KeyArray:{index}";
			var expected = $"Array Item {index}";

			TestExecuteTask (property, expected);
		}

		[Test]
		[TestCase (0)]
		[TestCase (1)]
		[TestCase (2)]
		public void TestGetArrayArrayIndexProperty (int index)
		{
			var property = $"KeyArray:3:{index}";
			var expected = $"Array Array Item 3.{index}";

			TestExecuteTask (property, expected);
		}

		[Test]
		[TestCase (0)]
		[TestCase (1)]
		[TestCase (2)]
		public void TestGetArrayDictionaryKeyProperty (int index)
		{
			var property = $"KeyArray:4:Item{index}";
			var expected = $"Array Dictionary Item 4.{index}";

			TestExecuteTask (property, expected);
		}

		[Test]
		public void TestGetDictionaryKeyNotFound ()
		{
			var property = $"KeyDictionary:ItemKeyNotFound";

			TestExecuteTask (property, null);
		}

		[Test]
		[TestCase (0)]
		[TestCase (1)]
		[TestCase (2)]
		public void TestGetDictionaryKeyProperty (int index)
		{
			var property = $"KeyDictionary:Item{index}";
			var expected = $"Dictionary Item {index}";

			TestExecuteTask (property, expected);
		}

		[Test]
		[TestCase (0)]
		[TestCase (1)]
		[TestCase (2)]
		public void TestGetDictionaryDictionaryKeyProperty (int index)
		{
			var property = $"KeyDictionary:Item3:Item{index}";
			var expected = $"Dictionary Dictionary Item 3.{index}";

			TestExecuteTask (property, expected);
		}

		[Test]
		[TestCase (0)]
		[TestCase (1)]
		[TestCase (2)]
		public void TestGetDictionaryArrayKeyProperty (int index)
		{
			var property = $"KeyDictionary:Item4:{index}";
			var expected = $"Dictionary Array Item 4.{index}";

			TestExecuteTask (property, expected);
		}

	}
}
