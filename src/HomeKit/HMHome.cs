#nullable enable

using System.Collections.Generic;
using System.Threading.Tasks;

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
	}
}
