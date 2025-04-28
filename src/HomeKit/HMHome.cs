#nullable enable

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ObjCRuntime;
using Foundation;

namespace HomeKit {

	public partial class HMHome {
		/// <param name="serviceTypes">To be added.</param>
		///         <summary>Returns services that accessories in the home provide that are of type <paramref name="serviceTypes" />. </summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public HMService []? GetServices (HMServiceType serviceTypes)
		{
			return GetServices (serviceTypes.ToArray ());
		}

#if !NET
		[NoTV]
#if TVOS
		[Obsolete ("This API is not available on this platform.")]
#endif // TVOS
		[Obsoleted (PlatformName.iOS, 9, 0, PlatformArchitecture.All, message: "This API in now prohibited on iOS. Use 'ManageUsers' instead.")]
		public virtual void RemoveUser (HMUser user, Action<NSError> completion)
		{
			throw new NotSupportedException ();
		}

		[NoTV]
#if TVOS
		[Obsolete ("This API is not available on this platform.")]
#endif // TVOS
		[Obsoleted (PlatformName.iOS, 9, 0, PlatformArchitecture.All, message: "This API in now prohibited on iOS. Use 'ManageUsers' instead.")]
		public virtual Task RemoveUserAsync (HMUser user)
		{
			throw new NotSupportedException ();
		}
#endif
	}
}
