using System.Reflection;

using Xamarin.Utils;

[assembly: Preserve (typeof (NSExpression), AllMembers = true)]

namespace MonoTouchFixtures.Foundation {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSExpressionTest {
		List<string> properties = new List<string> { "Block", "ConstantValue", "KeyPath",  "Function",
			"Variable", "Operand", "Arguments", "Collection", "Predicate", "LeftExpression",
			"RightExpression", "TrueExpression", "FalseExpression"};

		void TestProperties (NSExpression expression, List<string> availableProperties)
		{
			foreach (var pName in properties) {
				var type = typeof (NSExpression);
				var pInfo = type.GetProperty (pName);
				if (!availableProperties.Contains (pName)) {
					Assert.Throws<InvalidOperationException> (() => {
						// we want to make sure that is an invalid operation exception and not
						// a reflection one
						try {
							pInfo.GetValue (expression);
						} catch (TargetInvocationException e) {
							throw e.GetBaseException ();
						}
					}, $"Expressions of type '{expression.ExpressionType}' does not support the property '{pName}'");
				} else {
					Assert.DoesNotThrow (() => {
						pInfo.GetValue (expression);
					}, $"Expressions of type '{expression.ExpressionType}' do support the property '{pName}'");
				}
			}
		}

		[TestCase ("Foo", ExpectedResult = "Foo")]
		[TestCase (null, ExpectedResult = null)]
		public object FromConstant (object input)
		{
			NSObject value = null;

			switch (input) {
			case String stringValue:
				value = new NSString (stringValue);
				break;
			}

			using (var expression = NSExpression.FromConstant (value))
			using (var result = expression.EvaluateWith (null, null) as NSObject)
				return result?.ToString ();
		}

		[Test]
		public void FromKeyPath ()
		{
			using (var expression = NSExpression.FromKeyPath ("value"))
			using (var result = expression.EvaluateWith (null, null) as NSString)
				Assert.That (result, Is.Null);
		}

		[Test]
		public void FromFunctionTest ()
		{
			using (var expression = NSExpression.FromFunction ((o, e, c) => { return new NSString ("Foo"); }, new NSExpression [] { }))
			using (var result = expression.EvaluateWith (null, null) as NSString)
				Assert.That (result.ToString (), Is.EqualTo ("Foo"));
		}

		[Test]
		public void FromFormatWithArgsTest ()
		{
			using (var expression = NSExpression.FromFormat ("%f*%f", new NSObject [] { new NSNumber (2.0), new NSNumber (2.0) }))
			using (var result = expression.EvaluateWith (null, null) as NSNumber)
				Assert.That (result.DoubleValue, Is.EqualTo (4.0));
		}

		[Test]
		public void FromFormatWithNoArgsTest ()
		{
			using (var expression = NSExpression.FromFormat ("2*2"))
			using (var result = expression.EvaluateWith (null, null) as NSNumber)
				Assert.That (result.DoubleValue, Is.EqualTo (4.0));
		}

		[Test]
		public void FromFormatConstant ()
		{
			using (var expression = NSExpression.FromFormat ("2"))
			using (var result = expression.EvaluateWith (null, null) as NSNumber)
				Assert.That (result.DoubleValue, Is.EqualTo (2));
		}

		[Test]
		public void AggregatePropertiesTest ()
		{
			var availableProperties = new List<string> { "Collection" };
			using (var lower = NSExpression.FromConstant (new NSNumber (0)))
			using (var upper = NSExpression.FromConstant (new NSNumber (5)))
			using (var expression = NSExpression.FromAggregate (new NSExpression [] { lower, upper })) {
				Assert.That (expression.ExpressionType, Is.EqualTo (NSExpressionType.NSAggregate));
				TestProperties (expression, availableProperties);
			}
		}

		[Test]
		public void UnionSetPropertiesTest ()
		{
			var availableProperties = new List<string> { "LeftExpression", "RightExpression" };

			using (var llower = NSExpression.FromConstant (new NSNumber (0)))
			using (var lupper = NSExpression.FromConstant (new NSNumber (5)))
			using (var lh = NSExpression.FromAggregate (new NSExpression [] { llower, lupper }))
			using (var rlower = NSExpression.FromConstant (new NSNumber (10)))
			using (var rupper = NSExpression.FromConstant (new NSNumber (50)))
			using (var rh = NSExpression.FromAggregate (new NSExpression [] { rlower, rupper }))
			using (var expression = NSExpression.FromUnionSet (lh, rh)) {
				Assert.That (expression.ExpressionType, Is.EqualTo (NSExpressionType.UnionSet));
				TestProperties (expression, availableProperties);
			}
		}

		[Test]
		public void IntersectSetPropertiesTest ()
		{
			var availableProperties = new List<string> { "LeftExpression", "RightExpression" };

			using (var llower = NSExpression.FromConstant (new NSNumber (0)))
			using (var lupper = NSExpression.FromConstant (new NSNumber (5)))
			using (var lh = NSExpression.FromAggregate (new NSExpression [] { llower, lupper }))
			using (var rlower = NSExpression.FromConstant (new NSNumber (10)))
			using (var rupper = NSExpression.FromConstant (new NSNumber (50)))
			using (var rh = NSExpression.FromAggregate (new NSExpression [] { rlower, rupper }))
			using (var expression = NSExpression.FromIntersectSet (lh, rh)) {
				Assert.That (expression.ExpressionType, Is.EqualTo (NSExpressionType.IntersectSet));
				TestProperties (expression, availableProperties);
			}
		}

		[Test]
		public void MinusSetPropertiesTest ()
		{
			var availableProperties = new List<string> { "LeftExpression", "RightExpression" };

			using (var llower = NSExpression.FromConstant (new NSNumber (0)))
			using (var lupper = NSExpression.FromConstant (new NSNumber (5)))
			using (var lh = NSExpression.FromAggregate (new NSExpression [] { llower, lupper }))
			using (var rlower = NSExpression.FromConstant (new NSNumber (10)))
			using (var rupper = NSExpression.FromConstant (new NSNumber (50)))
			using (var rh = NSExpression.FromAggregate (new NSExpression [] { rlower, rupper }))
			using (var expression = NSExpression.FromMinusSet (lh, rh)) {
				Assert.That (expression.ExpressionType, Is.EqualTo (NSExpressionType.MinusSet));
				TestProperties (expression, availableProperties);
			}
		}

		[Test]
		public void ConstantPropertiesTest ()
		{
			var availableProperties = new List<string> { "ConstantValue" };
			using (var expression = NSExpression.FromFormat ("2")) {
				Assert.That (expression.ExpressionType, Is.EqualTo (NSExpressionType.ConstantValue));
				TestProperties (expression, availableProperties);
			}
		}

		[Test]
		public void VariablePropertiesTest ()
		{
			var availableProperties = new List<string> { "Variable" };
			using (var expression = NSExpression.FromVariable ("Variable")) {
				Assert.That (expression.ExpressionType, Is.EqualTo (NSExpressionType.Variable));
				TestProperties (expression, availableProperties);
			}
		}

		[Test]
		public void KeyPathPropertiesTest ()
		{
			var availableProperties = new List<string> { "KeyPath", "Operand", "Arguments" };
			using (var expression = NSExpression.FromKeyPath ("value")) {
				Assert.That (expression.ExpressionType, Is.EqualTo (NSExpressionType.KeyPath));
				TestProperties (expression, availableProperties);
			}
		}

		[Test]
		public void FunctionPropertiesTest ()
		{
			var availableProperties = new List<string> { "Function", "Operand", "Arguments" };
			using (var expression = NSExpression.FromFormat ("2*2")) {
				Assert.That (expression.ExpressionType, Is.EqualTo (NSExpressionType.Function));
				TestProperties (expression, availableProperties);
			}
		}

		[Test]
		public void BlockPropertiesTest ()
		{
			var availableProperties = new List<string> { "Block", "Arguments" };
			using (var expression = NSExpression.FromFunction ((o, e, c) => { return new NSString ("Foo"); }, new NSExpression [] { })) {
				Assert.That (expression.ExpressionType, Is.EqualTo (NSExpressionType.Block));
				TestProperties (expression, availableProperties);
			}
		}

		[Test]
		public void EvaluatedObjectPropertiesTest ()
		{
			var availableProperties = new List<string> { };
			var mySearchKey = new NSString ("James");
			using (var predicate = NSPredicate.FromFormat ("ANY employees.firstName like 'Matthew'") as NSComparisonPredicate)
			using (var expression = predicate.LeftExpression.Operand) { // NSExpressionType.EvaluatedObject;
				Assert.That (expression.ExpressionType, Is.EqualTo (NSExpressionType.EvaluatedObject));
				TestProperties (expression, availableProperties);
			}
		}

		[Test]
		public void AnyKeyPropertiesTest ()
		{
			TestRuntime.AssertXcodeVersion (5, 0);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 9, throwIfOtherPlatform: false);

			var availableProperties = new List<string> { };
			using (var expression = NSExpression.FromAnyKey ()) {
				Assert.That (expression.ExpressionType, Is.EqualTo (NSExpressionType.AnyKey));
				TestProperties (expression, availableProperties);
			}
		}
	}
}
