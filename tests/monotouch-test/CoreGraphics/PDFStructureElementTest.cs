// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#if !__TVOS__

using CoreGraphics;

namespace MonoTouchFixtures.CoreGraphics {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class PDFStructureElementTest {

		[Test]
		public void SettersAndChildren ()
		{
			TestRuntime.AssertXcodeVersion (27, 0);

			using var root = new CGPDFStructureElement (CGPdfTagType.Document);
			using var child = new CGPDFStructureElement (CGPdfTagType.Paragraph);

			child.SetTitle ("Title");
			child.SetLanguageIdentifier ("en");
			child.SetAlternativeText ("Alternative");
			child.SetExpansionText ("Expansion");
			child.SetActualText ("Actual");

			Assert.That (root.AddChild (child), Is.Zero, "AddChild");

			Assert.Throws<ArgumentNullException> (() => child.SetTitle (null), "SetTitle");
			Assert.Throws<ArgumentNullException> (() => child.SetLanguageIdentifier (null), "SetLanguageIdentifier");
			Assert.Throws<ArgumentNullException> (() => child.SetAlternativeText (null), "SetAlternativeText");
			Assert.Throws<ArgumentNullException> (() => child.SetExpansionText (null), "SetExpansionText");
			Assert.Throws<ArgumentNullException> (() => child.SetActualText (null), "SetActualText");
			Assert.Throws<ArgumentNullException> (() => root.AddChild (null), "AddChild null");
			Assert.Throws<ArgumentNullException> (() => root.AddMarkedContentItem (null), "AddMarkedContentItem null");
		}

		[Test]
		public void TaggedPdfAuthoring ()
		{
			TestRuntime.AssertXcodeVersion (27, 0);

			var mediaBox = new CGRect (0, 0, 100, 100);
			using var data = new NSMutableData ();
			using var consumer = new CGDataConsumer (data);
			using var context = new CGContextPDF (consumer, mediaBox);
			using var root = new CGPDFStructureElement (CGPdfTagType.Document);
			using var paragraph = new CGPDFStructureElement (CGPdfTagType.Paragraph);
			using var objectElement = new CGPDFStructureElement (CGPdfTagType.Object);

			Assert.Throws<ArgumentNullException> (() => context.AddStructureTreeRootChild (null), "Add root null");
			Assert.That (root.AddChild (paragraph), Is.Zero, "Add paragraph");
			Assert.That (root.AddChild (objectElement), Is.Zero, "Add object");
			Assert.That (context.AddStructureTreeRootChild (root), Is.Zero, "Add root");

			context.BeginPage (null);
			try {
				using var markedContentItem = context.BeginMarkedContentSequence (CGPdfTagType.Paragraph);
				Assert.That (markedContentItem, Is.Not.Null, "Marked content");
				if (markedContentItem is not null) {
					try {
						context.FillRect (new CGRect (10, 10, 20, 20));
					} finally {
						context.EndMarkedContentSequence ();
					}
					Assert.That (paragraph.AddMarkedContentItem (markedContentItem), Is.Zero, "Add marked content");
				}

				context.BeginNonStructuralMarkedContentSequence (CGPdfTagType.Artifact);
				try {
					context.FillRect (new CGRect (40, 10, 20, 20));
				} finally {
					context.EndMarkedContentSequence ();
				}

				byte [] imageData = new byte [4];
				using var colorSpace = CGColorSpace.CreateDeviceRGB ();
				using var bitmapContext = new CGBitmapContext (imageData, 1, 1, 8, 4, colorSpace, CGBitmapFlags.PremultipliedLast);
				using var image = bitmapContext.ToImage ();
				Assert.That (image, Is.Not.Null, "Image");

				using var objectReference = context.BeginObjectReference ();
				Assert.That (objectReference, Is.Not.Null, "Object reference");
				if (objectReference is not null) {
					try {
						context.DrawImage (new CGRect (10, 40, 20, 20), image);
					} finally {
						context.EndObjectReference ();
					}
					Assert.That (objectElement.AddMarkedContentItem (objectReference), Is.Zero, "Add object reference");
				}
			} finally {
				context.EndPage ();
			}

			context.Close ();
			Assert.That (data.Length, Is.GreaterThan ((nuint) 0), "PDF data");

			using var provider = new CGDataProvider (data);
			using var document = new CGPDFDocument (provider);
			Assert.That (document.Pages, Is.EqualTo ((nint) 1), "Pages");
			Assert.That (document.GetCatalog ().GetDictionary ("StructTreeRoot", out _), Is.True, "StructTreeRoot");
		}
	}
}

#endif // !__TVOS__
