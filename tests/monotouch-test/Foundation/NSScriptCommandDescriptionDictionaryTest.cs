#if __MACOS__

// Copyright 2015 Xamarin, Inc.

using System.Linq;

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSScriptCommandDescriptionDictionaryTest {
		[Test]
		public void TestAddNullArgument ()
		{
			var arg = new NSScriptCommandArgumentDescription () { AppleEventCode = "frgt", Type = "text", Name = "Foo" };
			var desc = new NSScriptCommandDescriptionDictionary ();
			// no exception should happen
			desc.Add (arg);
			using (var argKey = new NSString ("Arguments"))
			using (var nsName = new NSString (arg.Name)) {
				Assert.That (desc.Dictionary.ContainsKey (argKey), Is.True);
				var argDict = desc.Dictionary [argKey] as NSDictionary;
				Assert.That (argDict, Is.Not.Null);
				Assert.That (argDict.ContainsKey (nsName), Is.True);
			}
		}

		[Test]
		public void TestAddArgument ()
		{
			var arg = new NSScriptCommandArgumentDescription () { AppleEventCode = "frgt", Type = "text", Name = "Foo" };
			var desc = new NSScriptCommandDescriptionDictionary () { Arguments = new NSMutableDictionary () };
			// no exception should happen
			desc.Add (arg);
			using (var argKey = new NSString ("Arguments"))
			using (var nsName = new NSString (arg.Name)) {
				Assert.That (desc.Dictionary.ContainsKey (argKey), Is.True);
				var argDict = desc.Dictionary [argKey] as NSDictionary;
				Assert.That (argDict, Is.Not.Null);
				Assert.That (argDict.ContainsKey (nsName), Is.True);
			}
		}

		[Test]
		public void TestRemoveNoArguments ()
		{
			var arg = new NSScriptCommandArgumentDescription () { AppleEventCode = "frgt", Type = "text", Name = "Foo" };
			var desc = new NSScriptCommandDescriptionDictionary ();
			// no exception should happen
			Assert.That (desc.Remove (arg), Is.False);
		}

		[Test]
		public void TestRemoveMissingArgument ()
		{
			var arg = new NSScriptCommandArgumentDescription () { AppleEventCode = "frgt", Type = "text", Name = "Foo" };
			var desc = new NSScriptCommandDescriptionDictionary () { Arguments = new NSMutableDictionary () };
			// no exception should happen
			Assert.That (desc.Remove (arg), Is.False);
		}

		[Test]
		public void RemoveArgument ()
		{
			var arg = new NSScriptCommandArgumentDescription () { AppleEventCode = "frgt", Type = "text", Name = "Foo" };
			var desc = new NSScriptCommandDescriptionDictionary () { Arguments = new NSMutableDictionary () };
			// no exception should happen
			desc.Add (arg);
			using (var argKey = new NSString ("Arguments"))
			using (var nsName = new NSString (arg.Name)) {
				Assert.That (desc.Dictionary.ContainsKey (argKey), Is.True);
				var argDict = desc.Dictionary [argKey] as NSDictionary;
				Assert.That (argDict, Is.Not.Null);
				Assert.That (argDict.ContainsKey (nsName), Is.True);
			}
			desc.Remove (arg);
			using (var argKey = new NSString ("Arguments"))
			using (var nsName = new NSString (arg.Name)) {
				Assert.That (desc.Dictionary.ContainsKey (argKey), Is.True);
				var argDict = desc.Dictionary [argKey] as NSDictionary;
				Assert.That (argDict, Is.Not.Null);
				Assert.That (argDict.ContainsKey (nsName), Is.False);
			}
		}
	}
}
#endif // __MACOS__
