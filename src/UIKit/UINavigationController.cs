
namespace UIKit {
	public partial class UINavigationController {
		static IntPtr LookupClass (Type t)
		{
			return Class.GetHandle (t);
		}

		/// <param name="navigationBarType">The type of navigation bar to use.</param>
		///         <param name="toolbarType">The type of toolbar to use.</param>
		///         <summary>To be added.</summary>
		public UINavigationController (Type navigationBarType, Type toolbarType) : this (LookupClass (navigationBarType), LookupClass (toolbarType))
		{
		}

	}
}
