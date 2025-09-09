#nullable enable

using System;
using System.Threading.Tasks;

using ObjCRuntime;
using Foundation;

namespace HomeKit {

	public partial class HMService {

#if !TVOS
		/// <param name="serviceType">To be added.</param>
		///         <param name="completion">To be added.</param>
		///         <summary>Specifies that the device attached to a switch or outlet is of type <paramref name="serviceType" />. After the system sets the association, the system runs <paramref name="completion" />.</summary>
		///         <remarks>To be added.</remarks>
		public void UpdateAssociatedServiceType (HMServiceType serviceType, Action<NSError> completion)
		{
			UpdateAssociatedServiceType (serviceType.GetConstant (), completion);
		}

		/// <param name="serviceType">To be added.</param>
		///         <summary>Asynchronously updates the associated service type to  <paramref name="serviceType" /></summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public Task UpdateAssociatedServiceTypeAsync (HMServiceType serviceType)
		{
			return UpdateAssociatedServiceTypeAsync (serviceType.GetConstant ());
		}
#endif
	}
}
