using System.Reflection;

namespace GeneratorTests {

	public class ReflectionTest {

		protected FieldInfo GetField (string fieldName, Type testType)
		{
			var fieldInfo = testType.GetField (fieldName);
			Assert.That (fieldInfo, Is.Not.Null, "fieldInfo is not null");
			return fieldInfo!;
		}

		protected PropertyInfo GetProperty (string propertyName, Type testType)
		{
			var propertyInfo = testType.GetProperty (propertyName);
			Assert.That (propertyInfo, Is.Not.Null, "propertyInto is not null");
			return propertyInfo!;
		}

		protected MethodInfo GetMethod (string methodName, Type testType)
		{
			var memberInfo = testType.GetMethod (methodName);
			Assert.That (memberInfo, Is.Not.Null, "memberInfo is not null");
			return memberInfo!;
		}
	}
}
