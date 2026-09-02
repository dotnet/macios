using System.Linq;
using System.Reflection;
using bgen;

#nullable enable

namespace GeneratorTests {

	[TestFixture]
	[Parallelizable (ParallelScope.None)] // dotnet implementation is not thread safe..
	public class NullabilityContextTests : ReflectionTest {

		interface IInterfaceTest { }

		class ObjectTest : IInterfaceTest {
			public int NotNullableValueTypeField = 0;
			public string NotNullableRefTypeField = string.Empty;
			public int? NullableValueTypeField = 0;
			public string? NullableRefTypeField = string.Empty;

			public int NotNullableValueTypeProperty { get; set; }
			public string NotNullableRefProperty { get; set; } = string.Empty;
			public int? NullableValueTypeProperty { get; set; }
			public string? NullableRefProperty { get; set; }

			public int? this [int i] {
				get => i;
				set { }
			}

			public string? this [string? i] {
				get => i;
				set { }
			}

			public string this [double? i] {
				get => string.Empty;
				set { }
			}

			public void NotNullableValueTypeParameter (int myParam) { }
			public void NotNullableOutValueTypeParameter (out int myParam) { myParam = 1; }
			public void NotNullableRefValueTypeParameter (ref int myParam) { myParam = 1; }
			public void NotNullableRefParameter (string myParam) { }
			public void NotNullableOutRefParameter (out string myParam) { myParam = string.Empty; }
			public void NotNullableRefRefParameter (ref string myParam) { myParam = string.Empty; }

			public void NullableValueTypeParameter (int? myParam) { }
			public void NullableOutValueTypeParameter (out int? myParam) { myParam = null; }
			public void NullableRefValueTypeParameter (ref int? myParam) { myParam = null; }
			public void NullableRefTypeParameter (string? myParam) { }
			public void NullableOutRefTypeParameter (out string? myParam) { myParam = null; }
			public void NullableRefRefTypeParameter (out string? myParam) { myParam = null; }

			public int NotNullableValueTypeReturn () => 0;
			public string NotNullableRefTypeReturn () => string.Empty;

			public int? NullableValueTypeReturn () => null;
			public string? NullableRefTypeReturn () => null;

			// array return type tests
			public int [] NotNullableValueTypeArray () => Array.Empty<int> ();
			public int? [] NotNullableNullableValueTypeArray () => Array.Empty<int?> ();
			public int []? NullableValueTypeArray () => null;
			public int? []? NullableNullableValueTypeArray () => null;

			public string [] NotNullableRefTypeArray () => Array.Empty<string> ();
			public string? [] NotNullableNullableRefTypeArray () => Array.Empty<string?> ();
			public string []? NullableRefTypeArray () => null;
			public string? []? NullableNullableRefTypeArray () => null;

			public int [] [] NotNullableNestedArrayNotNullableValueType () => Array.Empty<int []> ();
			public int? [] [] NotNullableNestedArrayNullableValueType () => Array.Empty<int? []> ();
			public int? []? [] NotNullableNestedNullableArrayNullableValueType () => Array.Empty<int? []?> ();
			public int? []? []? NullableNestedNullableArrayNullableValueType () => null;


			public string [] [] NotNullableNestedArrayNotNullableRefType () => Array.Empty<string []> ();
			public string? [] [] NotNullableNestedArrayNullableRefType () => Array.Empty<string? []> ();
			public string? []? [] NotNullableNestedNullableArrayNullableRefType () => Array.Empty<string? []?> ();
			public string? []? []? NullableNestedNullableArrayNullableRefType () => null;

			public List<int> NotNullableValueTypeList () => new ();
			public List<int?> NullableValueTypeList () => new ();
			public List<int?>? NullableListNullableValueType () => null;

			public List<string> NotNullableRefTypeList () => new ();
			public List<string?> NullableRefTypeList () => new ();
			public List<string?>? NullableListNullableRefType () => null;

			public List<HashSet<int>> NestedGenericNotNullableValueType () => new ();
			public List<HashSet<int?>> NestedGenericNullableValueType () => new ();
			public List<HashSet<int?>?> NestedNullableGenericNullableValueType () => new ();
			public List<HashSet<int?>?>? NullableNestedNullableGenericNullableValueType () => new ();

			public List<HashSet<string>> NestedGenericNotNullableRefType () => new ();
			public List<HashSet<string?>> NestedGenericNullableRefType () => new ();
			public List<HashSet<string?>?> NestedNullableGenericNullableRefType () => new ();
			public List<HashSet<string?>?>? NullableNestedNullableGenericNullableRefType () => new ();

			public Dictionary<string, int> DictionaryStringInt () => new ();
			public Dictionary<string, int?> DictionaryStringNullableInt () => new ();
			public Dictionary<string, int?> DictionaryNullableStringNullableInt () => new ();
			public Dictionary<string, int?>? NullableDictionaryNullableStringNullableInt () => new ();

			public T GenericNotNullConstrain<T> () where T : notnull => default!;
			public T? GenericNullableClassConstrain<T> () where T : class? => default;
			public T? GenericNullableRefTypeConstrain<T> () where T : ObjectTest => null;
			public T GenericNotNullableRefTypeConstrain<T> () where T : ObjectTest => default!;
			public T? GenericNullableInterface<T> () where T : IInterfaceTest => default;

			public void GenericNotNullParameterConstrain<T> (T param) where T : notnull { }
			public void GenericNullableClassParameterConstrain<T> (T param) where T : class? { }
			public void GenericNullableRefTypeParameterConstrain<T> (T? param) where T : ObjectTest { }
			public void GenericNullableInterfaceParameterConstrain<T> (T? param) where T : IInterfaceTest { }
		}

		public static PropertyInfo GetIndexer (Type type, params Type [] arguments) => type.GetProperties ().First (
			x => x.GetIndexParameters ().Select (y => y.ParameterType).SequenceEqual (arguments));

		Type testType = typeof (object);
		NullabilityInfoContext context = new ();

		[SetUp]
		public void SetUp ()
		{
			testType = typeof (ObjectTest);
			context = new ();
		}


		[TestCase ("NotNullableValueTypeField", typeof (int))]
		[TestCase ("NotNullableRefTypeField", typeof (string))]
		public void NotNullableFieldTest (string fieldName, Type expectedType)
		{
			var fieldInfo = GetField (fieldName, testType);
			var info = context.Create (fieldInfo);
			Assert.That (info.IsNullable (), Is.False, "isNullable");
			Assert.That (info.Type, Is.EqualTo (expectedType));
		}

		[TestCase ("NullableValueTypeField", typeof (Nullable<int>))]
		[TestCase ("NullableRefTypeField", typeof (string))]
		public void NullableFieldTest (string fieldName, Type expectedType)
		{
			var fieldInfo = GetField (fieldName, testType);
			var info = context.Create (fieldInfo);
			Assert.That (info.IsNullable (), Is.True, "isNullable");
			Assert.That (info.Type, Is.EqualTo (expectedType));
		}

		[TestCase ("NotNullableValueTypeProperty", typeof (int))]
		[TestCase ("NotNullableRefProperty", typeof (string))]
		public void NotNullablePropertyTest (string propertyName, Type expectedType)
		{
			var propertyInfo = GetProperty (propertyName, testType);
			var info = context.Create (propertyInfo);
			Assert.That (info.IsNullable (), Is.False, "isNullable");
			Assert.That (info.Type, Is.EqualTo (expectedType));
		}

		// public int? this [int i] {
		[TestCase (typeof (int), typeof (Nullable<int>), true, typeof (int), false)]
		// public string? this [string? i] 
		[TestCase (typeof (string), typeof (string), true, typeof (string), true)]
		// public string this [double? i] {
		[TestCase (typeof (double?), typeof (string), false, typeof (Nullable<double>), true)]
		public void IndexersTests (Type indexersTypes, Type resultType, bool isNullable, Type indexParameter, bool isIndexParameterNull)
		{
			var propertyInfo = GetIndexer (testType, indexersTypes);
			Assert.That (propertyInfo, Is.Not.Null, "propertyInto is not null");
			var info = context.Create (propertyInfo.GetMethod!.ReturnParameter!);
			Assert.That (info.IsNullable (), Is.EqualTo (isNullable), "info.IsNullable");
			Assert.That (info.Type, Is.EqualTo (resultType), "info.Type");
			var parameters = propertyInfo.SetMethod!.GetParameters ();
			var paramInfo = context.Create (parameters [0]);
			Assert.That (paramInfo.IsNullable (), Is.EqualTo (isIndexParameterNull), "paramInfo.IsNullable");
			Assert.That (paramInfo.Type, Is.EqualTo (indexParameter), "paramInfo.Type");
		}

		[TestCase ("NullableValueTypeProperty", typeof (Nullable<int>))]
		[TestCase ("NullableRefProperty", typeof (string))]
		public void NullablePropertyTest (string propertyName, Type expectedType)
		{
			var propertyInfo = GetProperty (propertyName, testType);
			var info = context.Create (propertyInfo);
			Assert.That (info.IsNullable (), Is.True, "isNullable");
			Assert.That (info.Type, Is.EqualTo (expectedType));
		}

		[TestCase ("NotNullableValueTypeParameter", typeof (int))]
		[TestCase ("NotNullableRefParameter", typeof (string))]
		public void NotNullableParameterTest (string methodName, Type expectedType)
		{
			var memberInfo = GetMethod (methodName, testType);
			var paramInfo = memberInfo.GetParameters () [0];
			var info = context.Create (paramInfo);
			Assert.That (info.IsNullable (), Is.False, "isNullable");
			Assert.That (info.Type, Is.EqualTo (expectedType));
		}

		[TestCase ("NotNullableOutValueTypeParameter", typeof (int))]
		[TestCase ("NotNullableRefValueTypeParameter", typeof (int))]
		[TestCase ("NotNullableOutRefParameter", typeof (string))]
		[TestCase ("NotNullableRefRefParameter", typeof (string))]
		public void NotNullableRefParameterTest (string methodName, Type expectedType)
		{
			var memberInfo = GetMethod (methodName, testType);
			var paramInfo = memberInfo.GetParameters () [0];
			var info = context.Create (paramInfo);
			Assert.That (info.IsNullable (), Is.False, "isNullable");
			Assert.That (info.Type, Is.EqualTo (expectedType.MakeByRefType ()));
		}

		[TestCase ("NullableValueTypeParameter", typeof (Nullable<int>))]
		[TestCase ("NullableRefTypeParameter", typeof (string))]
		public void NullableParameterTest (string methdName, Type expectedType)
		{
			var memberInfo = GetMethod (methdName, testType);
			var paramInfo = memberInfo.GetParameters () [0];
			var info = context.Create (paramInfo);
			Assert.That (info.IsNullable (), Is.True, "isNullable");
			Assert.That (info.Type, Is.EqualTo (expectedType));
		}

		[TestCase ("NullableOutValueTypeParameter", typeof (Nullable<int>))]
		[TestCase ("NullableRefValueTypeParameter", typeof (Nullable<int>))]
		[TestCase ("NullableOutRefTypeParameter", typeof (string))]
		[TestCase ("NullableRefRefTypeParameter", typeof (string))]
		public void NullableRefParameterTest (string methdName, Type expectedType)
		{
			var memberInfo = GetMethod (methdName, testType);
			var paramInfo = memberInfo.GetParameters () [0];
			var info = context.Create (paramInfo);
			Assert.That (info.IsNullable (), Is.True, "isNullable");
			Assert.That (info.Type, Is.EqualTo (expectedType.MakeByRefType ()));
		}

		[TestCase ("NotNullableValueTypeReturn", typeof (int))]
		[TestCase ("NotNullableRefTypeReturn", typeof (string))]
		public void NotNullableReturnTypeTest (string methodName, Type expectedType)
		{
			var memberInfo = GetMethod (methodName, testType);
			var info = context.Create (memberInfo.ReturnParameter!);
			Assert.That (info.IsNullable (), Is.False, "isNullable");
			Assert.That (info.Type, Is.EqualTo (expectedType));
		}

		[TestCase ("NullableValueTypeReturn", typeof (Nullable<int>))]
		[TestCase ("NullableRefTypeReturn", typeof (string))]
		public void NullableReturnTypeTest (string methodName, Type expectedType)
		{
			var memberInfo = GetMethod (methodName, testType);
			var info = context.Create (memberInfo.ReturnParameter!);
			Assert.That (info.IsNullable (), Is.True, "isNullable");
			Assert.That (info.Type, Is.EqualTo (expectedType));
		}

		[TestCase ("NotNullableValueTypeArray", typeof (int []), false, typeof (int), false)]
		[TestCase ("NotNullableNullableValueTypeArray", typeof (int []), false, typeof (Nullable<int>), true)]
		[TestCase ("NullableValueTypeArray", typeof (int []), true, typeof (int), false)]
		[TestCase ("NullableNullableValueTypeArray", typeof (int []), true, typeof (Nullable<int>), true)]
		[TestCase ("NotNullableRefTypeArray", typeof (string []), false, typeof (string), false)]
		[TestCase ("NotNullableNullableRefTypeArray", typeof (string []), false, typeof (string), true)]
		public void ReturnTypeArrayTests (string methodName, Type? expectedType, bool arrayIsNullable, Type expectedElementType, bool elementTypeIsNullable)
		{
			var memberInfo = GetMethod (methodName, testType);
			var info = context.Create (memberInfo.ReturnParameter!);
			Assert.That (info.IsNullable (), Is.EqualTo (arrayIsNullable), "info.IsNullable");
			Assert.That (info.ElementType!.Type, Is.EqualTo (expectedElementType), "info.ElementType.Type");
			Assert.That (info.ElementType.IsNullable (), Is.EqualTo (elementTypeIsNullable), "info.ElementTyps.IsNullable");
		}

		[TestCase ("NotNullableNestedArrayNotNullableValueType", typeof (int [] []), false, false, typeof (int), false)]
		[TestCase ("NotNullableNestedArrayNullableValueType", typeof (int? [] []), false, false, typeof (Nullable<int>), true)]
		[TestCase ("NotNullableNestedNullableArrayNullableValueType", typeof (int? [] []), false, true, typeof (Nullable<int>), true)]
		[TestCase ("NullableNestedNullableArrayNullableValueType", typeof (int? [] []), true, true, typeof (Nullable<int>), true)]
		[TestCase ("NotNullableNestedArrayNotNullableRefType", typeof (string [] []), false, false, typeof (string), false)]
		[TestCase ("NotNullableNestedArrayNullableRefType", typeof (string? [] []), false, false, typeof (string), true)]
		[TestCase ("NotNullableNestedNullableArrayNullableRefType", typeof (string? [] []), false, true, typeof (string), true)]
		[TestCase ("NullableNestedNullableArrayNullableRefType", typeof (string? [] []), true, true, typeof (string), true)]
		public void ReturnNestedArrayTests (string methodName, Type? expectedType, bool isArrayNullable,
			bool isNestedArrayNullable, Type nestedArrayType, bool isNestedArrayElementNullable)
		{
			var memberInfo = GetMethod (methodName, testType);
			var info = context.Create (memberInfo.ReturnParameter!);
			Assert.That (info.IsNullable (), Is.EqualTo (isArrayNullable), "isArrayNullable");
			Assert.That (info.ElementType!.IsNullable (), Is.EqualTo (isNestedArrayNullable), "isNestedArrayNullable");
			Assert.That (info.ElementType!.ElementType!.Type, Is.EqualTo (nestedArrayType), "nestedArrayType");
			Assert.That (info.ElementType.ElementType.IsNullable (), Is.EqualTo (isNestedArrayElementNullable), "isNestedArrayElementNullable");
		}

		[TestCase ("NotNullableValueTypeList", typeof (List<int>), false, typeof (int), false)]
		[TestCase ("NullableValueTypeList", typeof (List<Nullable<int>>), false, typeof (Nullable<int>), true)]
		[TestCase ("NullableListNullableValueType", typeof (List<Nullable<int>>), true, typeof (Nullable<int>), true)]
		[TestCase ("NotNullableRefTypeList", typeof (List<string>), false, typeof (string), false)]
		[TestCase ("NullableRefTypeList", typeof (List<string>), false, typeof (string), true)]
		[TestCase ("NullableListNullableRefType", typeof (List<string>), true, typeof (string), true)]
		public void ReturnSimpleGenericType (string methodName, Type? expectedType, bool isGenericTypeNullable,
			Type? genericParameterType, bool isGenericParameterNull)
		{
			var memberInfo = GetMethod (methodName, testType);
			var info = context.Create (memberInfo.ReturnParameter!);
			Assert.That (info.Type, Is.EqualTo (expectedType), "info.Type");
			Assert.That (info.IsNullable (), Is.EqualTo (isGenericTypeNullable), "info.IsNullable");
			Assert.That (info.GenericTypeArguments! [0].Type, Is.EqualTo (genericParameterType), "genericParameterType");
			Assert.That (info.GenericTypeArguments [0].IsNullable (), Is.EqualTo (isGenericParameterNull), "isGenericParameterNull");
		}

		[TestCase ("NestedGenericNotNullableValueType", typeof (List<HashSet<int>>), false, typeof (HashSet<int>), false, typeof (int), false)]
		[TestCase ("NestedGenericNullableValueType", typeof (List<HashSet<Nullable<int>>>), false, typeof (HashSet<Nullable<int>>), false, typeof (Nullable<int>), true)]
		[TestCase ("NestedNullableGenericNullableValueType", typeof (List<HashSet<Nullable<int>>>), false, typeof (HashSet<Nullable<int>>), true, typeof (Nullable<int>), true)]
		[TestCase ("NullableNestedNullableGenericNullableValueType", typeof (List<HashSet<Nullable<int>>>), true, typeof (HashSet<Nullable<int>>), true, typeof (Nullable<int>), true)]
		[TestCase ("NestedGenericNotNullableRefType", typeof (List<HashSet<string>>), false, typeof (HashSet<string>), false, typeof (string), false)]
		[TestCase ("NestedGenericNullableRefType", typeof (List<HashSet<string>>), false, typeof (HashSet<string>), false, typeof (string), true)]
		[TestCase ("NestedNullableGenericNullableRefType", typeof (List<HashSet<string>>), false, typeof (HashSet<string>), true, typeof (string), true)]
		[TestCase ("NullableNestedNullableGenericNullableRefType", typeof (List<HashSet<string>>), true, typeof (HashSet<string>), true, typeof (string), true)]
		public void ReturnNestedGeneric (string methodName, Type? expectedType, bool isGenericNullable,
			Type? nestedGenericType, bool isNestedGenericNullable, Type? nestedGenericArgumentType,
			bool isNullableNestedGenericArgument)
		{
			var memberInfo = GetMethod (methodName, testType);
			var info = context.Create (memberInfo.ReturnParameter!);
			Assert.That (info.Type, Is.EqualTo (expectedType), "info.Type");
			Assert.That (info.IsNullable (), Is.EqualTo (isGenericNullable), "info.IsNullable");
			Assert.That (info.GenericTypeArguments! [0].Type, Is.EqualTo (nestedGenericType), "nestedGenericType");
			Assert.That (info.GenericTypeArguments [0].IsNullable (), Is.EqualTo (isNestedGenericNullable), "isNestedGenericNullable");
			Assert.That (info.GenericTypeArguments [0].GenericTypeArguments! [0].Type, Is.EqualTo (nestedGenericArgumentType), "nestedGenericArgumentType");
			Assert.That (info.GenericTypeArguments [0].GenericTypeArguments! [0].IsNullable (), Is.EqualTo (isNullableNestedGenericArgument), "isNullableNestedGenericArgument");
		}

		[TestCase ("DictionaryStringInt", typeof (Dictionary<string, int>), false, typeof (string), false, typeof (int), false)]
		[TestCase ("DictionaryStringNullableInt", typeof (Dictionary<string, Nullable<int>>), false, typeof (string), false, typeof (Nullable<int>), true)]
		[TestCase ("DictionaryNullableStringNullableInt", typeof (Dictionary<string, Nullable<int>>), false, typeof (string), false, typeof (Nullable<int>), true)]
		[TestCase ("NullableDictionaryNullableStringNullableInt", typeof (Dictionary<string, Nullable<int>>), true, typeof (string), false, typeof (Nullable<int>), true)]
		public void ReturnDictionary (string methodName, Type? dictionaryType, bool isDictionaryNullable, Type? keyType,
			bool isKeyNullable, Type? valueType, bool isValueNullable)
		{
			var memberInfo = GetMethod (methodName, testType);
			var info = context.Create (memberInfo.ReturnParameter!);
			Assert.That (info.Type, Is.EqualTo (dictionaryType), "info.Type");
			Assert.That (info.IsNullable (), Is.EqualTo (isDictionaryNullable), "info.IsNullable");
			Assert.That (info.GenericTypeArguments! [0].Type, Is.EqualTo (keyType), "keyType");
			Assert.That (info.GenericTypeArguments [0].IsNullable (), Is.EqualTo (isKeyNullable), "isKeyNullable");
			Assert.That (info.GenericTypeArguments [1].Type, Is.EqualTo (valueType), "valueType");
			Assert.That (info.GenericTypeArguments [1].IsNullable (), Is.EqualTo (isValueNullable), "isValueNullable");
		}

		[TestCase ("GenericNotNullConstrain", false)]
		[TestCase ("GenericNullableClassConstrain", true)]
		[TestCase ("GenericNullableRefTypeConstrain", true)]
		[TestCase ("GenericNullableInterface", true)] // limitation in the lang ? is ignore
		public void GenericReturnConstrainTests (string methodName, bool returnTypeIsNull)
		{
			var memberInfo = GetMethod (methodName, testType);
			var info = context.Create (memberInfo.ReturnParameter!);
			Assert.That (info.IsNullable (), Is.EqualTo (returnTypeIsNull));
		}

		[TestCase ("GenericNotNullParameterConstrain", false)]
		[TestCase ("GenericNullableClassParameterConstrain", true)]
		[TestCase ("GenericNullableRefTypeParameterConstrain", true)]
		[TestCase ("GenericNullableInterfaceParameterConstrain", true)] // limitation in the lang ? is ignore
		public void GenericParamConstrainTests (string methodName, bool returnTypeIsNull)
		{
			var memberInfo = GetMethod (methodName, testType);
			var info = context.Create (memberInfo.GetParameters () [0]);
			Assert.That (info.IsNullable (), Is.EqualTo (returnTypeIsNull));
		}
	}
}
