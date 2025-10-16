//
// Unit tests for CGPDFOperatorTable
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

using CoreGraphics;

namespace MonoTouchFixtures.CoreGraphics {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class PDFOperatorTableTest {

		[Test]
		public void Default ()
		{
			using (var table = new CGPDFOperatorTable ()) {
				Assert.That (table.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");
			}
		}
	}
}
