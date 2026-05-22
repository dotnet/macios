//
// Unit tests for CTFontCollection
//
// Author:
//	Vincent Dondain <vidondai@microsoft.com>
//
// Copyright 2018 Microsoft. All rights reserved.
//

using System.Linq;
using CoreText;

namespace MonoTouchFixtures.CoreText {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CTFontCollectionTest {
		[Test]
		public void GetMatchingFontDescriptorsTest ()
		{
			var collection = new CTFontCollection (null);
			var sortIsCalled = false;
			var descList = collection.GetMatchingFontDescriptors ((CTFontDescriptor x, CTFontDescriptor y) => {
				sortIsCalled = true;
				return 0;
			});

			Assert.That (sortIsCalled, Is.True, "GetMatchingFontDescriptors delegate is called");

			// Native crash (can't assert on it) if https://github.com/dotnet/macios/pull/3871 fix not present.
			descList.First ().GetAttributes ();
		}

		[Test]
		public void GetMatchingFontDescriptorsCollectionOptionsTest ()
		{
			TestRuntime.AssertXcodeVersion (10, 0);
			using (var collection = new CTFontCollection (null)) {
				var fd1 = collection.GetMatchingFontDescriptors ();
				var fd2 = collection.GetMatchingFontDescriptors (options: null); // documented to return the same thing as the parameterless if null
				Assert.That (fd1, Is.Not.Null, "fd1");
				Assert.That (fd2, Is.Not.Null, "fd2");
				Assert.That (fd2.Length, Is.EqualTo (fd1.Length), "equal collections");

				var fd3 = collection.GetMatchingFontDescriptors (new CTFontCollectionOptions { RemoveDuplicates = true });
				Assert.That (fd3, Is.Not.Null, "fd3");
			}
		}
	}
}
