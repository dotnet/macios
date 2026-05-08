#if __MACOS__

using AppKit;

namespace Xamarin.Mac.Tests {
	[Preserve (AllMembers = true)]
	public class NSStackViewTests {
		NSStackView view;
		NSView first;
		NSView second;
		NSView third;

		[SetUp]
		public void SetUp ()
		{
			view = new NSStackView ();

			first = new NSView ();
			second = new NSView ();
			third = new NSView ();
		}

		[Test]
		public void NSStackViewShouldCreateWithEmptyConstructor ()
		{
			ClassicAssert.IsNotNull (view, "NSStackViewCreateWithEmptyConstructor - Failed to create view");
		}

		[Test]
		public void NSStackViewShouldCreateWithViews ()
		{
			view = NSStackView.FromViews (new [] { first, second });

			ClassicAssert.IsNotNull (view, "NSStackViewCreateWithViews - Failed to create view");
			ClassicAssert.IsTrue (view.Views.Length == 2, "NSStackViewShouldCreateWithViews - StackView does not have 2 views");
		}

		[Test]
		public void NSStackViewShouldAddView ()
		{
			view.AddView (new NSView (), NSStackViewGravity.Bottom);

			ClassicAssert.IsTrue (view.Views.Length == 1, "NSStackViewShouldAddView - Failed to add view - length was 0");
		}

		[Test]
		public void NSStackViewShouldInsertView ()
		{
			view.AddView (first, NSStackViewGravity.Trailing);
			view.AddView (second, NSStackViewGravity.Trailing);

			view.InsertView (third, 1, NSStackViewGravity.Trailing);

			ClassicAssert.IsTrue (view.Views.Length == 3, "NSStackViewShouldInsertView - Wrong number of views");
			ClassicAssert.IsTrue (view.Views [1] == third, "NSStackViewShouldInsertView - New view not inserted at the correct location");
		}

		[Test]
		public void NSStackViewShouldRemoveView ()
		{
			view = NSStackView.FromViews (new [] { first, second });

			view.RemoveView (second);

			ClassicAssert.IsTrue (view.Views.Length == 1, "NSStackViewShouldRemoveView - Failed to remove view");
		}

		[Test]
		public void NSStackViewShouldSetViews ()
		{
			view.SetViews (new [] { first, second }, NSStackViewGravity.Leading);

			ClassicAssert.IsTrue (view.Views.Length == 2, "NSStackViewShouldSetViews - Views length was not 0");
			ClassicAssert.IsTrue (view.ViewsInGravity (NSStackViewGravity.Leading).Length == 2, "NSStackViewShouldSetViews - ViewsInGravity Leading was not 2");
			ClassicAssert.IsTrue (view.ViewsInGravity (NSStackViewGravity.Trailing).Length == 0, "NSStackViewShouldSetViews - ViewsInGravity Trailing was not 0");
		}

		[Test]
		public void NSStackViewShouldChangeAlignment ()
		{
			var alignment = view.Alignment;
			view.Alignment = NSLayoutAttribute.Right;

			ClassicAssert.IsFalse (view.Alignment == alignment, "NSStackViewShouldChangeAlignment - Failed to change Alignment property");
		}

		[Test]
		public void NSStackViewShouldChangeOrientation ()
		{
			var orientation = view.Orientation;
			view.Orientation = NSUserInterfaceLayoutOrientation.Vertical;

			ClassicAssert.IsFalse (view.Orientation == orientation, "NSStackViewShouldChangeOrientation - Failed to change Orientation property");
		}

		[Test]
		public void NSStackViewShouldChangeSpacing ()
		{
			var spacing = view.Spacing;
			view.Spacing = spacing + 3;

			ClassicAssert.IsFalse (view.Spacing == spacing, "NSStackViewShouldChangeSpacing - Failed to change Spacing property");
		}

		[Test]
		public void NSStackViewShouldChangeEdgeInsets ()
		{
			var edgeInsets = view.EdgeInsets;
			view.EdgeInsets = new NSEdgeInsets (20, 20, 20, 20);

			ClassicAssert.IsFalse (view.EdgeInsets.Left == edgeInsets.Left, "NSStackViewShouldChangeEdgeInsets - Failed to change EdgeInsets property");
			ClassicAssert.IsFalse (view.EdgeInsets.Right == edgeInsets.Right, "NSStackViewShouldChangeEdgeInsets - Failed to change EdgeInsets property");
			ClassicAssert.IsFalse (view.EdgeInsets.Top == edgeInsets.Top, "NSStackViewShouldChangeEdgeInsets - Failed to change EdgeInsets property");
			ClassicAssert.IsFalse (view.EdgeInsets.Bottom == edgeInsets.Bottom, "NSStackViewShouldChangeEdgeInsets - Failed to change EdgeInsets property");
		}

		[Test]
		public void NSStackViewShouldChangeHasEqualSpacing ()
		{
			var hasEqualSpacing = view.HasEqualSpacing;
			view.HasEqualSpacing = !hasEqualSpacing;

			ClassicAssert.IsFalse (view.HasEqualSpacing == hasEqualSpacing, "NSStackViewShouldChangeHasEqualSpacing - Failed to change HasEqualSpacing property");
		}

		//		[Test]
		//		public void NSStackViewShouldSetDelegate ()
		//		{
		//			var view = new NSStackView ();
		//			view.Delegate = new NSStackViewDelegate ();
		//
		//			ClassicAssert.IsNotNull (view.Delegate, "NSStackViewShouldSetDelegate - Delegate property returned null");
		//		}

		[Test]
		public void NSStackViewShouldChangeClippingResistance ()
		{
			var clippingResistance = view.ClippingResistancePriorityForOrientation (NSLayoutConstraintOrientation.Vertical);
			view.SetClippingResistancePriority (clippingResistance + 3, NSLayoutConstraintOrientation.Vertical);

			ClassicAssert.IsFalse (view.ClippingResistancePriorityForOrientation (NSLayoutConstraintOrientation.Vertical) == clippingResistance,
				"NSStackViewShouldChangeClippingResistance - Failed to set ClippingResistance");
		}

		[Test]
		public void NSStackViewShouldChangeHuggingPriority ()
		{
			var huggingPriority = view.HuggingPriority (NSLayoutConstraintOrientation.Horizontal);
			view.SetHuggingPriority (huggingPriority + 10, NSLayoutConstraintOrientation.Horizontal);

			ClassicAssert.IsFalse (view.HuggingPriority (NSLayoutConstraintOrientation.Horizontal) == huggingPriority,
				"NSStackViewShouldChangeHuggingPriority - Failed to set HuggingPriority");
		}

		[Test]
		public void NSStackViewShouldChangeCustomSpacing ()
		{
			view.AddView (first, NSStackViewGravity.Trailing);
			view.AddView (second, NSStackViewGravity.Trailing);

			var customSpacing = view.CustomSpacingAfterView (first);
			view.SetCustomSpacing (10, first);

			ClassicAssert.IsFalse (view.CustomSpacingAfterView (first) == customSpacing,
				"NSStackViewShouldChangeCustomSpacing - Failed to set CustomSpacing");
		}

		[Test]
		public void NSStackViewShouldChangeVisibilityPriority ()
		{
			view.AddView (first, NSStackViewGravity.Trailing);
			view.AddView (second, NSStackViewGravity.Trailing);

			var visibilityPriority = view.VisibilityPriority (first);
			view.SetVisibilityPriority (10, first);

			ClassicAssert.IsFalse (view.VisibilityPriority (first) == visibilityPriority,
				"NSStackViewShouldChangeVisibilityPriority - Failed to set VisibilityPriority");
		}
	}
}

#endif // __MACOS__
