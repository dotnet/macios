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
			Assert.That (view, Is.Not.Null, "NSStackViewCreateWithEmptyConstructor - Failed to create view");
		}

		[Test]
		public void NSStackViewShouldCreateWithViews ()
		{
			view = NSStackView.FromViews (new [] { first, second });

			Assert.That (view, Is.Not.Null, "NSStackViewCreateWithViews - Failed to create view");
			Assert.That (view.Views.Length == 2, Is.True, "NSStackViewShouldCreateWithViews - StackView does not have 2 views");
		}

		[Test]
		public void NSStackViewShouldAddView ()
		{
			view.AddView (new NSView (), NSStackViewGravity.Bottom);

			Assert.That (view.Views.Length == 1, Is.True, "NSStackViewShouldAddView - Failed to add view - length was 0");
		}

		[Test]
		public void NSStackViewShouldInsertView ()
		{
			view.AddView (first, NSStackViewGravity.Trailing);
			view.AddView (second, NSStackViewGravity.Trailing);

			view.InsertView (third, 1, NSStackViewGravity.Trailing);

			Assert.That (view.Views.Length == 3, Is.True, "NSStackViewShouldInsertView - Wrong number of views");
			Assert.That (view.Views [1] == third, Is.True, "NSStackViewShouldInsertView - New view not inserted at the correct location");
		}

		[Test]
		public void NSStackViewShouldRemoveView ()
		{
			view = NSStackView.FromViews (new [] { first, second });

			view.RemoveView (second);

			Assert.That (view.Views.Length == 1, Is.True, "NSStackViewShouldRemoveView - Failed to remove view");
		}

		[Test]
		public void NSStackViewShouldSetViews ()
		{
			view.SetViews (new [] { first, second }, NSStackViewGravity.Leading);

			Assert.That (view.Views.Length == 2, Is.True, "NSStackViewShouldSetViews - Views length was not 0");
			Assert.That (view.ViewsInGravity (NSStackViewGravity.Leading).Length == 2, Is.True, "NSStackViewShouldSetViews - ViewsInGravity Leading was not 2");
			Assert.That (view.ViewsInGravity (NSStackViewGravity.Trailing).Length == 0, Is.True, "NSStackViewShouldSetViews - ViewsInGravity Trailing was not 0");
		}

		[Test]
		public void NSStackViewShouldChangeAlignment ()
		{
			var alignment = view.Alignment;
			view.Alignment = NSLayoutAttribute.Right;

			Assert.That (view.Alignment == alignment, Is.False, "NSStackViewShouldChangeAlignment - Failed to change Alignment property");
		}

		[Test]
		public void NSStackViewShouldChangeOrientation ()
		{
			var orientation = view.Orientation;
			view.Orientation = NSUserInterfaceLayoutOrientation.Vertical;

			Assert.That (view.Orientation == orientation, Is.False, "NSStackViewShouldChangeOrientation - Failed to change Orientation property");
		}

		[Test]
		public void NSStackViewShouldChangeSpacing ()
		{
			var spacing = view.Spacing;
			view.Spacing = spacing + 3;

			Assert.That (view.Spacing == spacing, Is.False, "NSStackViewShouldChangeSpacing - Failed to change Spacing property");
		}

		[Test]
		public void NSStackViewShouldChangeEdgeInsets ()
		{
			var edgeInsets = view.EdgeInsets;
			view.EdgeInsets = new NSEdgeInsets (20, 20, 20, 20);

			Assert.That (view.EdgeInsets.Left == edgeInsets.Left, Is.False, "NSStackViewShouldChangeEdgeInsets - Failed to change EdgeInsets property");
			Assert.That (view.EdgeInsets.Right == edgeInsets.Right, Is.False, "NSStackViewShouldChangeEdgeInsets - Failed to change EdgeInsets property");
			Assert.That (view.EdgeInsets.Top == edgeInsets.Top, Is.False, "NSStackViewShouldChangeEdgeInsets - Failed to change EdgeInsets property");
			Assert.That (view.EdgeInsets.Bottom == edgeInsets.Bottom, Is.False, "NSStackViewShouldChangeEdgeInsets - Failed to change EdgeInsets property");
		}

		[Test]
		public void NSStackViewShouldChangeHasEqualSpacing ()
		{
			var hasEqualSpacing = view.HasEqualSpacing;
			view.HasEqualSpacing = !hasEqualSpacing;

			Assert.That (view.HasEqualSpacing == hasEqualSpacing, Is.False, "NSStackViewShouldChangeHasEqualSpacing - Failed to change HasEqualSpacing property");
		}

		//		[Test]
		//		public void NSStackViewShouldSetDelegate ()
		//		{
		//			var view = new NSStackView ();
		//			view.Delegate = new NSStackViewDelegate ();
		//
		//			Assert.That (view.Delegate, Is.Not.Null, "NSStackViewShouldSetDelegate - Delegate property returned null");
		//		}

		[Test]
		public void NSStackViewShouldChangeClippingResistance ()
		{
			var clippingResistance = view.ClippingResistancePriorityForOrientation (NSLayoutConstraintOrientation.Vertical);
			view.SetClippingResistancePriority (clippingResistance + 3, NSLayoutConstraintOrientation.Vertical);

			Assert.That (view.ClippingResistancePriorityForOrientation (NSLayoutConstraintOrientation.Vertical) == clippingResistance, Is.False, "NSStackViewShouldChangeClippingResistance - Failed to set ClippingResistance");
		}

		[Test]
		public void NSStackViewShouldChangeHuggingPriority ()
		{
			var huggingPriority = view.HuggingPriority (NSLayoutConstraintOrientation.Horizontal);
			view.SetHuggingPriority (huggingPriority + 10, NSLayoutConstraintOrientation.Horizontal);

			Assert.That (view.HuggingPriority (NSLayoutConstraintOrientation.Horizontal) == huggingPriority, Is.False, "NSStackViewShouldChangeHuggingPriority - Failed to set HuggingPriority");
		}

		[Test]
		public void NSStackViewShouldChangeCustomSpacing ()
		{
			view.AddView (first, NSStackViewGravity.Trailing);
			view.AddView (second, NSStackViewGravity.Trailing);

			var customSpacing = view.CustomSpacingAfterView (first);
			view.SetCustomSpacing (10, first);

			Assert.That (view.CustomSpacingAfterView (first) == customSpacing, Is.False, "NSStackViewShouldChangeCustomSpacing - Failed to set CustomSpacing");
		}

		[Test]
		public void NSStackViewShouldChangeVisibilityPriority ()
		{
			view.AddView (first, NSStackViewGravity.Trailing);
			view.AddView (second, NSStackViewGravity.Trailing);

			var visibilityPriority = view.VisibilityPriority (first);
			view.SetVisibilityPriority (10, first);

			Assert.That (view.VisibilityPriority (first) == visibilityPriority, Is.False, "NSStackViewShouldChangeVisibilityPriority - Failed to set VisibilityPriority");
		}
	}
}

#endif // __MACOS__
