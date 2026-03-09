// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

//
// Unit tests for CTFont.GetAvailableTables
//

using CoreText;

namespace MonoTouchFixtures.CoreText {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CTFontGetAvailableTablesTest {

		[Test]
		public void GetAvailableTables ()
		{
			using var font = new CTFont ("Helvetica", 12);
			var tables = font.GetAvailableTables (CTFontTableOptions.None);
			Assert.IsNotNull (tables, "tables");
			Assert.That (tables.Length, Is.GreaterThan (0), "tables/length");
		}
	}
}
