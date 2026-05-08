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
				ClassicAssert.IsTrue (desc.Dictionary.ContainsKey (argKey));
				var argDict = desc.Dictionary [argKey] as NSDictionary;
				ClassicAssert.IsNotNull (argDict);
				ClassicAssert.IsTrue (argDict.ContainsKey (nsName));
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
				ClassicAssert.IsTrue (desc.Dictionary.ContainsKey (argKey));
				var argDict = desc.Dictionary [argKey] as NSDictionary;
				ClassicAssert.IsNotNull (argDict);
				ClassicAssert.IsTrue (argDict.ContainsKey (nsName));
			}
		}

		[Test]
		public void TestRemoveNoArguments ()
		{
			var arg = new NSScriptCommandArgumentDescription () { AppleEventCode = "frgt", Type = "text", Name = "Foo" };
			var desc = new NSScriptCommandDescriptionDictionary ();
			// no exception should happen
			ClassicAssert.IsFalse (desc.Remove (arg));
		}

		[Test]
		public void TestRemoveMissingArgument ()
		{
			var arg = new NSScriptCommandArgumentDescription () { AppleEventCode = "frgt", Type = "text", Name = "Foo" };
			var desc = new NSScriptCommandDescriptionDictionary () { Arguments = new NSMutableDictionary () };
			// no exception should happen
			ClassicAssert.IsFalse (desc.Remove (arg));
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
				ClassicAssert.IsTrue (desc.Dictionary.ContainsKey (argKey));
				var argDict = desc.Dictionary [argKey] as NSDictionary;
				ClassicAssert.IsNotNull (argDict);
				ClassicAssert.IsTrue (argDict.ContainsKey (nsName));
			}
			desc.Remove (arg);
			using (var argKey = new NSString ("Arguments"))
			using (var nsName = new NSString (arg.Name)) {
				ClassicAssert.IsTrue (desc.Dictionary.ContainsKey (argKey));
				var argDict = desc.Dictionary [argKey] as NSDictionary;
				ClassicAssert.IsNotNull (argDict);
				ClassicAssert.IsFalse (argDict.ContainsKey (nsName));
			}
		}
	}
}
#endif // __MACOS__
