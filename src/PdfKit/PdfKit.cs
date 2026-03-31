
using CoreGraphics;

#nullable enable

namespace PdfKit {
	partial class PdfBorder {
		/// <summary>Gets or sets the dash pattern for the border.</summary>
		/// <value>An array of <see cref="nfloat" /> values defining the dash pattern, or <see langword="null" />.</value>
		public nfloat []? DashPattern {
			get {
				return WeakDashPattern?.ToArrayDropNullElements<nfloat, NSNumber> (v => v.NFloatValue);
			}
			set {
				WeakDashPattern = NSArray.FromNSObjects ((v) => new NSNumber (v), value);
			}
		}
	}

#if !IOS && !__TVOS__
	/// <summary>Represents a PDF markup annotation such as highlight, underline, or strikethrough.</summary>
	partial class PdfAnnotationMarkup {
		/// <summary>Gets or sets the points defining the quadrilateral bounds of the markup annotation.</summary>
		/// <value>An array of <see cref="CGPoint" /> values representing the quadrilateral vertices, or <see langword="null" />.</value>
		public CGPoint []? QuadrilateralPoints {
			get {
				return WeakQuadrilateralPoints?.ToArrayDropNullElements<CGPoint, NSValue> (v => v.CGPointValue);
			}
			set {
				using var arr = NSArray.FromNSObjects ((v) => NSValue.FromCGPoint (v), value);
				WeakQuadrilateralPoints = arr;
			}
		}
	}
#endif // !IOS && !__TVOS__
}
