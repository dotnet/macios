#nullable enable

using System.Threading.Tasks;

namespace HomeKit {

	public partial class HMService {

#if !TVOS
		/// <summary>Updates the type of device associated with the service.</summary>
		/// <param name="serviceType">The associated service type.</param>
		/// <param name="completion">The handler to invoke after the update completes.</param>
		public void UpdateAssociatedServiceType (HMServiceType serviceType, Action<NSError> completion)
		{
			UpdateAssociatedServiceType (serviceType.GetConstant (), completion);
		}

		/// <summary>Asynchronously updates the type of device associated with the service.</summary>
		/// <param name="serviceType">The associated service type.</param>
		/// <returns>A task that represents the update operation.</returns>
		public Task UpdateAssociatedServiceTypeAsync (HMServiceType serviceType)
		{
			return UpdateAssociatedServiceTypeAsync (serviceType.GetConstant ());
		}
#endif
	}
}
