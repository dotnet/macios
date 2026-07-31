
namespace UIKit {
	public partial class UINavigationController {
		static IntPtr LookupClass (Type t)
		{
			return Class.GetHandle (t);
		}

		/// <param name="navigationBarType">The navigation bar type.</param>
		///         <param name="toolbarType">The toolbar type.</param>
		///         <summary>To be added.</summary>
		public UINavigationController (Type navigationBarType, Type toolbarType) : this (LookupClass (navigationBarType), LookupClass (toolbarType))
		{
		}

	}
}
