//
// Unit tests for UIPointerAccessory
//
// Authors:
//	Alex Soto <alexsoto@microsoft.com>
//
//
// Copyright Microsoft Corporation.
//

#if __IOS__
using UIKit;

namespace MonoTouchFixtures.UIKit {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class UIPointerAccessoryTest {

		[SetUp]
		public void Setup ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);
		}

		[Test]
		public void UIPointerAccessoryPositionTopTest ()
		{
			UIPointerAccessory acc = null;
			Assert.DoesNotThrow (() => acc = UIPointerAccessory.CreateArrow (UIPointerAccessoryPosition.Top), "Should not throw");
			Assert.That (acc, Is.Not.Null, $"{nameof (acc)} was null");
			Assert.That (UIPointerAccessoryPosition.Top.Offset, Is.EqualTo (acc.Position.Offset), "Offset");
			Assert.That (UIPointerAccessoryPosition.Top.Angle, Is.EqualTo (acc.Position.Angle), "Angle");
		}

		[Test]
		public void UIPointerAccessoryPositionTopRightTest ()
		{
			UIPointerAccessory acc = null;
			Assert.DoesNotThrow (() => acc = UIPointerAccessory.CreateArrow (UIPointerAccessoryPosition.TopRight), "Should not throw");
			Assert.That (acc, Is.Not.Null, $"{nameof (acc)} was null");
			Assert.That (UIPointerAccessoryPosition.TopRight.Offset, Is.EqualTo (acc.Position.Offset), "Offset");
			Assert.That (UIPointerAccessoryPosition.TopRight.Angle, Is.EqualTo (acc.Position.Angle), "Angle");
		}

		[Test]
		public void UIPointerAccessoryPositionRightTest ()
		{
			UIPointerAccessory acc = null;
			Assert.DoesNotThrow (() => acc = UIPointerAccessory.CreateArrow (UIPointerAccessoryPosition.Right), "Should not throw");
			Assert.That (acc, Is.Not.Null, $"{nameof (acc)} was null");
			Assert.That (UIPointerAccessoryPosition.Right.Offset, Is.EqualTo (acc.Position.Offset), "Offset");
			Assert.That (UIPointerAccessoryPosition.Right.Angle, Is.EqualTo (acc.Position.Angle), "Angle");
		}

		[Test]
		public void UIPointerAccessoryPositionBottomRightTest ()
		{
			UIPointerAccessory acc = null;
			Assert.DoesNotThrow (() => acc = UIPointerAccessory.CreateArrow (UIPointerAccessoryPosition.BottomRight), "Should not throw");
			Assert.That (acc, Is.Not.Null, $"{nameof (acc)} was null");
			Assert.That (UIPointerAccessoryPosition.BottomRight.Offset, Is.EqualTo (acc.Position.Offset), "Offset");
			Assert.That (UIPointerAccessoryPosition.BottomRight.Angle, Is.EqualTo (acc.Position.Angle), "Angle");
		}

		[Test]
		public void UIPointerAccessoryPositionBottomTest ()
		{
			UIPointerAccessory acc = null;
			Assert.DoesNotThrow (() => acc = UIPointerAccessory.CreateArrow (UIPointerAccessoryPosition.Bottom), "Should not throw");
			Assert.That (acc, Is.Not.Null, $"{nameof (acc)} was null");
			Assert.That (UIPointerAccessoryPosition.Bottom.Offset, Is.EqualTo (acc.Position.Offset), "Offset");
			Assert.That (UIPointerAccessoryPosition.Bottom.Angle, Is.EqualTo (acc.Position.Angle), "Angle");
		}

		[Test]
		public void UIPointerAccessoryPositionBottomLeftTest ()
		{
			UIPointerAccessory acc = null;
			Assert.DoesNotThrow (() => acc = UIPointerAccessory.CreateArrow (UIPointerAccessoryPosition.BottomLeft), "Should not throw");
			Assert.That (acc, Is.Not.Null, $"{nameof (acc)} was null");
			Assert.That (UIPointerAccessoryPosition.BottomLeft.Offset, Is.EqualTo (acc.Position.Offset), "Offset");
			Assert.That (UIPointerAccessoryPosition.BottomLeft.Angle, Is.EqualTo (acc.Position.Angle), "Angle");
		}

		[Test]
		public void UIPointerAccessoryPositionLeftTest ()
		{
			UIPointerAccessory acc = null;
			Assert.DoesNotThrow (() => acc = UIPointerAccessory.CreateArrow (UIPointerAccessoryPosition.Left), "Should not throw");
			Assert.That (acc, Is.Not.Null, $"{nameof (acc)} was null");
			Assert.That (UIPointerAccessoryPosition.Left.Offset, Is.EqualTo (acc.Position.Offset), "Offset");
			Assert.That (UIPointerAccessoryPosition.Left.Angle, Is.EqualTo (acc.Position.Angle), "Angle");
		}

		[Test]
		public void UIPointerAccessoryPositionTopLeftTest ()
		{
			UIPointerAccessory acc = null;
			Assert.DoesNotThrow (() => acc = UIPointerAccessory.CreateArrow (UIPointerAccessoryPosition.TopLeft), "Should not throw");
			Assert.That (acc, Is.Not.Null, $"{nameof (acc)} was null");
			Assert.That (UIPointerAccessoryPosition.TopLeft.Offset, Is.EqualTo (acc.Position.Offset), "Offset");
			Assert.That (UIPointerAccessoryPosition.TopLeft.Angle, Is.EqualTo (acc.Position.Angle), "Angle");
		}
	}
}
#endif // __IOS__
