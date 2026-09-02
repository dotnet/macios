using System.Collections;

#nullable enable

namespace GeneratorTests {
	public class StringExtensionTests {

		public class KeywordsDataSource : IEnumerable {
			static string [] keywords = {
				"abstract","event","new","struct","as","explicit","null","switch","base","extern",
				"this","false","operator","throw","break","finally","out","true",
				"fixed","override","try","case","params","typeof","catch","for",
				"private","foreach","protected","checked","goto","public",
				"unchecked","class","if","readonly","unsafe","const","implicit","ref",
				"continue","in","return","using","virtual","default",
				"interface","sealed","volatile","delegate","internal","do","is",
				"sizeof","while","lock","stackalloc","else","static","enum",
				"namespace",
				"object","bool","byte","float","uint","char","ulong","ushort",
				"decimal","int","sbyte","short","double","long","string","void",
				"partial", "yield", "where"
			};

			public IEnumerator GetEnumerator ()
			{
				foreach (var key in keywords) {
					yield return key;
				}
			}
		}

		[TestCaseSource (typeof (KeywordsDataSource))]
		public void KeywordsTest (string keyword)
			=> Assert.That (keyword.GetSafeParamName (), Is.Not.EqualTo (keyword), "keyword");

		[TestCase ("😀OhOh")]
		[TestCase (" OhOh")]
		[TestCase ("Oh Oh")]
		public void NotFixableIllegalChar (string illegal)
			=> Assert.That (illegal.GetSafeParamName (), Is.Null, "paramName is null");

		[TestCase ("1param")]
		public void StartsWithFixableIllegalChar (string illegal)
		{
			var legal = illegal.GetSafeParamName ();
			Assert.That (legal, Is.Not.Null, "legal is not null");
			Assert.That (legal, Is.EqualTo ("@" + illegal), "legal");
		}

		[Test]
		public void QuoteNullString ()
		{
			string? str = null;
			Assert.That (str.Quote (), Is.EqualTo (string.Empty));
		}

		[Test]
		public void QuoteEmptyString ()
		{
			string str = String.Empty;
			Assert.That (str.Quote (), Is.EqualTo (@""""""));
		}

		[TestCase ("No quotes", "@\"No quotes\"")]
		[TestCase ("\"quotes\"", "@\"\"\"quotes\"\"\"")]
		public void QuoteString (string input, string output)
		{
			Assert.That (input.Quote (), Is.EqualTo (output));
		}

		[Test]
		public void CamelCaseTest ()
		{
			var str = "pascalCaseExample";
			Assert.That (str.CamelCase (), Is.EqualTo ("PascalCaseExample"));
		}

		[Test]
		public void PascalCaseTest ()
		{
			var str = "CamelCaseExample";
			Assert.That (str.PascalCase (), Is.EqualTo ("camelCaseExample"));
		}

		[TestCase ("@thisIsNotCapitalized", "ThisIsNotCapitalized")]
		[TestCase ("thisIsNotCapitalized", "ThisIsNotCapitalized")]
		[TestCase ("t", "T")]
		public void CapitalizeTest (string input, string output)
			=> Assert.That (input.Capitalize (), Is.EqualTo (output));

		[TestCase ("ArityTest", "ArityTest")]
		[TestCase ("Arity`Test", "Arity")]
		[TestCase ("Arity`", "Arity")]
		[TestCase ("`Arity", "`Arity")]
		[TestCase ("A`rity", "A")]
		[TestCase ("`", "`")]
		[TestCase (null, null)]
		public void RemoveArityTest (string input, string output)
			=> Assert.That (input.RemoveArity (), Is.EqualTo (output));
	}
}
