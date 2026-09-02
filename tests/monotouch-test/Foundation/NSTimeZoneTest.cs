//
// Unit tests for NSTimeZone
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2011 Xamarin Inc. All rights reserved.
//

using System.IO;

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSTimeZoneTest {

		[Test]
		public void KnownTimeZoneNames ()
		{
			Assert.That (NSTimeZone.KnownTimeZoneNames.Count, Is.GreaterThan (400), "KnownTimeZoneNames"); // 416 on iOS5
		}

		[Test]
		public void AbbreviationsTest ()
		{
			Assert.That (NSTimeZone.Abbreviations.ContainsKey (new NSString ("CST")));
		}

		[Test]
		public void AbbreviationTest ()
		{
			var timezone = NSTimeZone.LocalTimeZone;
			Assert.That (timezone.Abbreviation (), Is.Not.Null);
		}

		[Test]
		public void All_28300 ()
		{
			foreach (var name in NSTimeZone.KnownTimeZoneNames) {
				// simulator uses OSX to get timezones which might have some holes,
				// e.g. @"Pacific/Bougainville" does not seems to be available in Mavericks
#if !MONOMAC && !__MACCATALYST__
				if (Runtime.Arch == Arch.SIMULATOR) {
					if (!File.Exists (Path.Combine ("/usr/share/zoneinfo/", name)))
						continue;
				}
#endif
				TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById (name);
				Assert.That (tzi.GetUtcOffset (DateTime.Now), Is.Not.Null, name);
			}

			Assert.That (TimeZoneInfo.Local.GetUtcOffset (DateTime.Now), Is.Not.Null, "Local");
		}
	}
}
