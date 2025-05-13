//
// Mostly notifications for keyboard events
//

using Foundation;
using ObjCRuntime;
using CoreGraphics;

using System;

namespace UIKit {
	public partial class UIKeyboard {
		static CGRect RectangleFFrom (NSObject key, NSNotification n)
		{
			if (n is null || n.UserInfo is null)
				throw new ArgumentNullException ("n");
			var val = n.UserInfo [key] as NSValue;
			if (val is not null)
				return val.CGRectValue;
			return CGRect.Empty;
		}

#if !TVOS && !__MACCATALYST__
		/// <param name="n">notification and its payload.</param>
		///         <summary>Deprecated: helper method to extract the animation duration from a notification.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>Use the <see cref="UIKit.UIKeyboard.Notifications" /> strongly typed methods instead.</remarks>
		public static double AnimationDurationFromNotification (NSNotification n)
		{
			if (n is null || n.UserInfo is null)
				throw new ArgumentNullException ("n");
			var val = n.UserInfo [AnimationDurationUserInfoKey] as NSNumber;
			if (val is null)
				return 0;
			return val.DoubleValue;
		}

		/// <param name="n">notification and its payload.</param>
		///         <summary>Deprecated: helper method to extract the animation curve from a notification.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>Use the <see cref="UIKit.UIKeyboard.Notifications" /> strongly typed methods instead.</remarks>
		public static uint AnimationCurveFromNotification (NSNotification n)
		{
			if (n is null || n.UserInfo is null)
				throw new ArgumentNullException ("n");
			var val = n.UserInfo [AnimationCurveUserInfoKey] as NSNumber;
			if (val is null)
				return 0;
			return val.UInt32Value;
		}
#endif

		static CGPoint PointFFrom (NSObject key, NSNotification n)
		{
			if (n is null || n.UserInfo is null)
				throw new ArgumentNullException ("n");
			var val = n.UserInfo [key] as NSValue;
			if (val is null)
				return CGPoint.Empty;
			return val.CGPointValue;
		}

#if !TVOS && !__MACCATALYST__
		/// <param name="n">notification and its payload.</param>
		///         <summary>Deprecated: helper method to extract the keyboard's starting frame from a notification</summary>
		///         <returns>
		///         </returns>
		///         <remarks>Use the <see cref="UIKit.UIKeyboard.Notifications" /> strongly typed methods instead.</remarks>
		static public CGRect FrameBeginFromNotification (NSNotification n)
		{
			return RectangleFFrom (FrameBeginUserInfoKey, n);
		}

		/// <param name="n">notification and its payload.</param>
		///         <summary>Deprecated: helper method to extract the keyboard's ending frame from a notification</summary>
		///         <returns>
		///         </returns>
		///         <remarks>Use the <see cref="UIKit.UIKeyboard.Notifications" /> strongly typed methods instead.</remarks>
		static public CGRect FrameEndFromNotification (NSNotification n)
		{
			return RectangleFFrom (FrameEndUserInfoKey, n);
		}
#endif
	}
}
