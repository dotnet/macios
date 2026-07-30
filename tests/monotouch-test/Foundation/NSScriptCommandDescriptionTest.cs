#if __MACOS__
// Copyright 2015 Xamarin, Inc.

using System.Linq;

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSScriptCommandDescriptionTest {

		NSScriptCommandDescription scriptDescription = null;
		Dictionary<string, NSScriptCommandArgumentDescription> args;
		NSScriptCommandDescriptionDictionary dict = null;
		string suiteName, commandName, cmdClass, eventCode, eventClass, returnType, resultAppleEvent = null;

		[SetUp]
		public void Init ()
		{
			args = new Dictionary<string, NSScriptCommandArgumentDescription> {
				{"firstArg", new NSScriptCommandArgumentDescription {Name="firstArg", AppleEventCode="fArg", Type="integer", IsOptional=true}},
				{"secondArg", new NSScriptCommandArgumentDescription {Name="secondArg", AppleEventCode="sArg", Type="NSNumber"}},
				{"thirdArg", new NSScriptCommandArgumentDescription {Name="thirdArg", AppleEventCode="tArg", Type="integer"}}
			};

			suiteName = "Chromium Suite";
			commandName = "Exec Python";
			cmdClass = "NSScriptCommand";
			eventCode = "ExPy";
			eventClass = "CrSu";
			returnType = "NSString";
			resultAppleEvent = "text";
			dict = new NSScriptCommandDescriptionDictionary {
				CommandClass = cmdClass,
				AppleEventCode = eventCode,
				AppleEventClassCode = eventClass,
				Type = returnType,
				ResultAppleEventCode = resultAppleEvent
			};

			foreach (var arg in args.Values) {
				dict.Add (arg);
			}
			scriptDescription = NSScriptCommandDescription.Create (suiteName, commandName, dict);
		}

		[TearDown]
		public void Dispose ()
		{
			if (scriptDescription is not null)
				scriptDescription.Dispose ();
		}

		[Test]
		public void TestCreateWithDictWrongArgDescription ()
		{
			var description = new NSScriptCommandDescriptionDictionary ();
			Assert.Throws<ArgumentException> (() => NSScriptCommandDescription.Create (suiteName, commandName, description));
		}

		[TestCase ("")]
		[TestCase (null)]
		public void TestCreateWithDictNullOrEmptySuitName (string code)
		{
			var description = new NSScriptCommandDescriptionDictionary ();
			Assert.Throws<ArgumentException> (() => NSScriptCommandDescription.Create (code, commandName, description));
		}

		[TestCase ("")]
		[TestCase (null)]
		public void TestCreateWithDictNullOrEmptyCommandName (string code)
		{
			var description = new NSScriptCommandDescriptionDictionary ();
			Assert.Throws<ArgumentException> (() => NSScriptCommandDescription.Create (suiteName, code, description));
		}

		[Test]
		public void TestCreateWithDictNullDict ()
		{
			NSScriptCommandDescriptionDictionary dict = null;
			Assert.Throws<ArgumentNullException> (() => NSScriptCommandDescription.Create (suiteName, commandName, dict));
		}

		[TestCase ("")]
		[TestCase (null)]
		public void TestCreateSuiteNameNullOrEmpty (string code)
		{
			Assert.Throws<ArgumentException> (() => NSScriptCommandDescription.Create (code, commandName, dict));
		}

		[TestCase ("")]
		[TestCase (null)]
		public void TestCreateCommandNameNullOrEmpty (string code)
		{
			Assert.Throws<ArgumentException> (() => NSScriptCommandDescription.Create (suiteName, code, dict));
		}

		[TestCase ("")]
		[TestCase (null)]
		public void TestCreateCmdClassNullOrEmpty (string code)
		{
			dict.CommandClass = code;
			Assert.Throws<ArgumentException> (() => NSScriptCommandDescription.Create (suiteName, commandName, dict));
		}

		[TestCase ("")]
		[TestCase (null)]
		public void TestCreateEventCodeNullOrEmpty (string code)
		{
			dict.AppleEventCode = code;
			Assert.Throws<ArgumentException> (() => NSScriptCommandDescription.Create (suiteName, commandName, dict));
		}

		[TestCase ("TooLong")]
		[TestCase ("srt")]
		public void TestCreateEventCodeWrongLength (string code)
		{
			dict.AppleEventCode = code;
			Assert.Throws<ArgumentException> (() => NSScriptCommandDescription.Create (suiteName, commandName, dict));
		}

		[TestCase ("TooLong")]
		[TestCase ("srt")]
		public void TestCreateResultAppleEventWrongLength (string code)
		{
			dict.ResultAppleEventCode = code;
			Assert.Throws<ArgumentException> (() => NSScriptCommandDescription.Create (suiteName, commandName, dict));
		}

		[Test]
		public void TestClassName ()
		{
			Assert.That (scriptDescription.ClassName, Is.EqualTo (cmdClass));
		}

		[Test]
		public void TestName ()
		{
			Assert.That (scriptDescription.Name, Is.EqualTo (commandName));
		}

		[Test]
		public void TestSuiteName ()
		{
			Assert.That (scriptDescription.SuitName, Is.EqualTo (suiteName));
		}

		[Test]
		public void TestArgumentsNames ()
		{
			Assert.That (scriptDescription.ArgumentNames.Length, Is.EqualTo (args.Keys.Count));
			foreach (var argName in scriptDescription.ArgumentNames) {
				Assert.That (args.Keys.Contains (argName), Is.True, $"Arg {argName} is missing");
			}
		}

		[Test]
		public void TestAppleEventClassCode ()
		{
			Assert.That (scriptDescription.AppleEventClassCode, Is.EqualTo (eventClass));
		}

		[Test]
		public void TestAppleEventCode ()
		{
			Assert.That (scriptDescription.AppleEventCode, Is.EqualTo (eventCode));
		}

		[Test]
		public void TestIsOptionalArgument ()
		{
			foreach (KeyValuePair<string, NSScriptCommandArgumentDescription> kvp in args) {
				Assert.That (scriptDescription.IsOptionalArgument (kvp.Key), Is.EqualTo (kvp.Value.IsOptional), $"Wrong apple event code for arg {kvp.Key}");
			}
		}

		[Test]
		public void TestGetAppleEventCodeForArgument ()
		{
			foreach (KeyValuePair<string, NSScriptCommandArgumentDescription> kvp in args) {
				Assert.That (scriptDescription.GetAppleEventCodeForArgument (kvp.Key), Is.EqualTo (kvp.Value.AppleEventCode), $"Wrong apple event code for arg {kvp.Key}");
			}
		}

		[Test]
		public void TestReturnType ()
		{
			Assert.That (scriptDescription.ReturnType, Is.EqualTo (returnType));
		}
	}
}
#endif // __MACOS__
