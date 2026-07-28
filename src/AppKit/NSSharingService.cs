#if !__MACCATALYST__

#nullable enable

namespace AppKit {
	public partial class NSSharingService {
		/// <summary>Returns the sharing service for the specified service name.</summary>
		/// <param name="service">The sharing service to retrieve.</param>
		/// <returns>The sharing service, or <see langword="null" /> if the service is not available.</returns>
		public static NSSharingService? GetSharingService (NSSharingServiceName service)
		{
			var constant = service.GetConstant ();
			if (constant is null)
				return null;
			return NSSharingService.GetSharingService (constant);
		}
	}
}
#endif // !__MACCATALYST__
