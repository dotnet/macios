#if __MACOS__
#nullable enable

using System;
using NUnit.Framework;
using AppKit;
using Foundation;
using Security;
using SecurityInterface;

namespace MonoTouchFixtures.SecurityInterface {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SFChooseIdentityTableCellViewTest {

		[Test]
		public void Constructor ()
		{
			using var cellView = new SFChooseIdentityTableCellView (new global::CoreGraphics.CGRect (0, 0, 200, 44));
			Assert.That (cellView.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");
		}

		[Test]
		public void IssuerTextField_DefaultNull ()
		{
			using var cellView = new SFChooseIdentityTableCellView (new global::CoreGraphics.CGRect (0, 0, 200, 44));
			Assert.That (cellView.IssuerTextField, Is.Null, "IssuerTextField should initially be null");
		}

		[Test]
		public void IssuerTextField_SetAndGet ()
		{
			using var cellView = new SFChooseIdentityTableCellView (new global::CoreGraphics.CGRect (0, 0, 200, 44));
			using var textField = new NSTextField ();
			cellView.IssuerTextField = textField;
			Assert.That (cellView.IssuerTextField, Is.Not.Null, "IssuerTextField should be set");
		}
	}
}
#endif // __MACOS__
