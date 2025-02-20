using System;
using Foundation;
using ObjCRuntime;

namespace UIKit {
	public partial class UINavigationController {
		static IntPtr LookupClass (Type t)
		{
			return t is null ? IntPtr.Zero : Class.GetHandle (t);
		}

		/// <param name="navigationBarType">To be added.</param>
		///         <param name="toolbarType">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public UINavigationController (Type navigationBarType, Type toolbarType) : this (LookupClass (navigationBarType), LookupClass (toolbarType))
		{
		}

	}
}
