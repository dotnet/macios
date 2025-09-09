#if __MACOS__
using System;
using System.Threading.Tasks;
using NUnit.Framework;

using AppKit;
using AudioUnit;
using AudioToolbox;
using Foundation;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSTextFinderTests {
		[Test]
		public void NSTextFinderConstructor ()
		{
			NSTextFinder f = new NSTextFinder ();
			Assert.IsNotNull (f);

			FinderClient client = new FinderClient ();
			f.Client = client;
		}

		class FinderClient : NSObject, INSTextFinderClient {
			public bool AllowsMultipleSelection { get { return true; } }

			public bool Editable { get { return true; } }

			public string String { get { return "Testing One Two Three"; } }

			public NSRange FirstSelectedRange { get { return new NSRange (); } }

			public NSArray SelectedRanges { get; set; }

			public NSArray VisibleCharacterRanges { get { return new NSArray (); } }

			public bool Selectable { get { return true; } }

			public string GetString (nuint characterIndex, out NSRange outRange, bool outFlag)
			{
				outRange = default (NSRange);
				return String;
			}

			public nuint StringLength {
				get {
					return (nuint) String.Length;
				}
			}

			public void ScrollRangeToVisible (NSRange range)
			{
			}

			public bool ShouldReplaceCharacters (NSArray ranges, NSArray strings)
			{
				return false;
			}

			public void ReplaceCharacters (NSRange range, string str)
			{
			}

			public void DidReplaceCharacters ()
			{
			}

			public NSView GetContentView (nuint index, out NSRange outRange)
			{
				outRange = default (NSRange);
				return null;
			}

			public NSArray GetRects (NSRange characterRange)
			{
				return null;
			}

			public void DrawCharacters (NSRange range, NSView view)
			{
			}
		}
	}
}

#endif // __MACOS__
