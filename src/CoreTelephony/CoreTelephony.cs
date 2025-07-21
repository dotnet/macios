#nullable enable

using Foundation;
using ObjCRuntime;
using System;

namespace CoreTelephony {

	public partial class CTCall {
#if !COREBUILD
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? StateDialing {
			get {
				return Dlfcn.SlowGetStringConstant (Constants.CoreTelephonyLibrary, "CTCallStateDialing");
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? StateIncoming {
			get {
				return Dlfcn.SlowGetStringConstant (Constants.CoreTelephonyLibrary, "CTCallStateIncoming");
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? StateConnected {
			get {
				return Dlfcn.SlowGetStringConstant (Constants.CoreTelephonyLibrary, "CTCallStateConnected");
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? StateDisconnected {
			get {
				return Dlfcn.SlowGetStringConstant (Constants.CoreTelephonyLibrary, "CTCallStateDisconnected");
			}
		}
#endif // !COREBUILD
	}
}
