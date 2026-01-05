#nullable enable

using System.Linq;
using System.Collections.Generic;

namespace NaturalLanguage {

	public partial class NLLanguageExtensions {

		static internal Dictionary<NLLanguage, double> Convert (NSDictionary<NSString, NSNumber> dict)
		{
			var result = new Dictionary<NLLanguage, double> ((int) dict.Count);
			var e = (IEnumerable<KeyValuePair<NSString, NSNumber>>) dict;
			foreach (var kvp in e) {
				result [NLLanguageExtensions.GetValue (kvp.Key)] = kvp.Value.DoubleValue;
			}
			return result;
		}
	}
}
