#nullable enable

using System.Collections.Generic;

namespace BusinessChat {
	/// <summary>Provides actions for opening Business Chat conversations.</summary>
	public partial class BCChatAction {
		/// <summary>Opens the transcript for the specified business with the supplied intent parameters.</summary>
		/// <param name="businessIdentifier">The identifier of the business whose transcript to open.</param>
		/// <param name="intentParameters">The parameters that describe the user's intent.</param>
		public static void OpenTranscript (string businessIdentifier, Dictionary<BCParameterName, string> intentParameters)
		{
			var keys = new NSString [intentParameters.Keys.Count];
			var values = new NSString [intentParameters.Keys.Count];
			var index = 0;
			foreach (var k in intentParameters.Keys) {
				if (k.GetConstant () is NSString s) {
					keys [index] = s;
					values [index] = new NSString (intentParameters [k]);
					index++;
				}
			}
			using (var dict = NSDictionary<NSString, NSString>.FromObjectsAndKeys (values, keys, keys.Length))
				OpenTranscript (businessIdentifier, dict);
		}
	}
}
