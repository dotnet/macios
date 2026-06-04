#nullable enable

using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeKit {

	public partial class HMHome {
		/// <param name="serviceTypes">The service types to filter by.</param>
		/// <summary>Returns services that accessories in the home provide that are of type <paramref name="serviceTypes" />.</summary>
		/// <returns>An array of services matching the specified types, or <see langword="null" /> if none are found.</returns>
		public HMService []? GetServices (HMServiceType serviceTypes)
		{
			return GetServices (serviceTypes.ToArray ());
		}
	}
}
