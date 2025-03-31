using System;

using Foundation;
using ObjCRuntime;
using CoreGraphics;

#nullable enable

namespace CoreAnimation {
	public partial class CAKeyFrameAnimation {
		/// <typeparam name="T">Generic type to get teh values as.</typeparam>
		///         <summary>Returns the elements of the key frame animation as an
		/// 	array of strongly typed values of NSObject or CoreGraphics objects.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		public T [] GetValuesAs<T> () where T : class, INativeObject
		{
			return NSArray.FromArrayNative<T> (_Values);
		}

		/// <param name="value">Values to set, they can be CoreGraphics
		/// 	objects, or subclasses of an NSObject.</param>
		///         <summary>Sets the values fo the key frame animation to the
		/// 	values specified in the array.</summary>
		///         <remarks>
		///           <para>
		/// 	    To pass number, create instances of <see cref="T:Foundation.NSNumber" /> with the value,
		/// 	    to pass other values, use <see cref="T:Foundation.NSValue" />, or pass the
		/// 	    CoreGraphics data types directly to it.
		/// 	  </para>
		///         </remarks>
		public void SetValues (INativeObject [] value)
		{
			_Values = NSArray.FromNSObjects (value);
		}

		// The underlying objective-c API appears to be doing pointer comparisions
		// or some other trickery. Our NSString -> C# string -> NSString conversions
		// were breaking this on the Mac. We look up the equivilant NSString and pass that
		// along to "fix" this
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual string CalculationMode {
			get {
				return _CalculationMode;
			}
			set {
				NSString result;
				if (value == CAAnimation.AnimationLinear)
					result = CAAnimation.AnimationLinear;
				else if (value == CAAnimation.AnimationDiscrete)
					result = CAAnimation.AnimationDiscrete;
				else if (value == CAAnimation.AnimationPaced)
					result = CAAnimation.AnimationPaced;
				else if (value == CAAnimation.AnimationCubic)
					result = CAAnimation.AnimationCubic;
				else if (value == CAAnimation.AnimationCubicPaced)
					result = CAAnimation.AnimationCubicPaced;
				else
					result = (NSString) value;

				_CalculationMode = result;
			}
		}
	}
}
