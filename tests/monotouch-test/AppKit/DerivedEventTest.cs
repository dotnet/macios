#if __MACOS__
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using AppKit;
using CoreGraphics;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class DerivedEventTest {
		[Test]
		public void DerivedEvents_DontStompEachOther ()
		{
			NSComboBox b = new NSComboBox (new CGRect (0, 0, 200, 25));

			b.SelectionChanged += (sender, e) => Console.WriteLine ("Change");
			TestDelegates (b);
			b.EditingEnded += (sender, e) => Console.WriteLine ("Edit");
			TestDelegates (b);
			b.Delegate = null;

			b.EditingEnded += (sender, e) => Console.WriteLine ("Edit");
			TestDelegates (b);
			b.SelectionChanged += (sender, e) => Console.WriteLine ("Change");
			TestDelegates (b);
		}

		void TestDelegates (NSComboBox b)
		{
			NSTextField f = (NSTextField) b;
			Assert.That (b.Delegate, Is.Not.Null, "NSComboBox delegate null");
			Assert.That (f.Delegate, Is.Not.Null, "NSTextField delegate null");
			Assert.That (f.Delegate.GetHashCode (), Is.EqualTo (b.Delegate.GetHashCode ()), "Delegates are not equal");
		}

		[Test]
		public void DerivedEvents_OverwriteThrows ()
		{
#if RELEASE
			var checkTrimmedAway = TestRuntime.IsLinkAny;
#else
			var checkTrimmedAway = false;
#endif
			TestOverrideThrow (false, !checkTrimmedAway);
			TestOverrideThrow (true, !checkTrimmedAway);
#if MONOMAC
			NSApplication.CheckForEventAndDelegateMismatches = false;
#else
			UIApplication.CheckForEventAndDelegateMismatches = false;
#endif
			TestOverrideThrow (false, false);
			TestOverrideThrow (true, false);
		}

		void TestOverrideThrow (bool eventFirst, bool shouldThrow)
		{
			NSComboBox b = new NSComboBox (new CGRect (0, 0, 200, 25));
			if (eventFirst)
				b.SelectionChanged += (sender, e) => Console.WriteLine ("Change");
			else
				b.Delegate = new NSComboBoxDelegate ();

			bool didThrow = false;
			try {
				if (eventFirst)
					b.Delegate = new NSComboBoxDelegate ();
				else
					b.SelectionChanged += (sender, e) => Console.WriteLine ("Change");
			} catch (System.InvalidOperationException) {
				didThrow = true;
			}
			if (shouldThrow != didThrow)
				Assert.Fail ($"TestOverrideThrow ({eventFirst}, {shouldThrow}) did not have expected thrown status");
		}
	}
}
#endif // __MACOS__
