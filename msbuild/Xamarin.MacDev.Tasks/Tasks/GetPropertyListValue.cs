using System;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using Xamarin.MacDev;
using Xamarin.Localization.MSBuild;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	public class GetPropertyListValue : Task {
		#region Inputs

		[Required]
		public string PropertyListFile { get; set; } = "";

		[Required]
		public string Property { get; set; } = "";

		#endregion Inputs

		#region Outputs

		[Output]
		public string Value { get; set; } = "";

		#endregion Outputs

		public override bool Execute ()
		{
			PArray? array = null;
			PDictionary? dict;
			PObject? value;
			string [] path;
			int i = 0;

			path = Property.TrimStart (':').Split (new [] { ':' });

			if (path.Length == 0) {
				Log.LogError (152, null, MSBStrings.E0152); // No property specified.
				return false;
			}

			try {
				value = dict = PDictionary.OpenFile (PropertyListFile);
			} catch (Exception ex) {
				Log.LogError (10, null, MSBStrings.E0010, PropertyListFile, ex.Message); // Error loading '{0}': {1}
				return false;
			}

			do {
				if (dict is not null) {
					if (!dict.TryGetValue (path [i], out value)) {
						var item = i > 0 ? string.Join ("/", path, 0, i - 1) : PropertyListFile;
						Log.LogError (153, null, MSBStrings.E0153, item, path [i]); // The dictionary at '{0}' did not contain the key: {1}
						return false;
					}
				} else if (array is not null) {
					int arrayIndex;

					if (!int.TryParse (path [i], out arrayIndex)) {
						Log.LogError (145, null, MSBStrings.E0145, path [i]); // No {0} signing identities match the specified provisioning profile '{1}'.
						return false;
					}

					if (arrayIndex < 0 || arrayIndex >= array.Count) {
						var item = i > 0 ? string.Join ("/", path, 0, i - 1) : PropertyListFile;
						Log.LogError (155, null, MSBStrings.E0155, item); // Array index out of range for item '{0}'
						return false;
					}

					value = array [arrayIndex];
				} else {
					Log.LogError (156, null, MSBStrings.E0156, value?.Type); // {0} values do not support child properties.
					return false;
				}

				dict = value as PDictionary;
				array = value as PArray;
				i++;
			} while (i < path.Length);

			if (array is not null || dict is not null || value.Type is PObjectType.Data) {
				Log.LogError (157, null, MSBStrings.E0157, value.Type.ToString ().ToLowerInvariant ()); // Getting {0} values is not supported.
				return false;
			}

			if (value is IPValueObject pvalue) {
				Value = pvalue.Value.ToString () ?? string.Empty;
			} else {
				Value = value.ToString () ?? string.Empty;
			}

			return !Log.HasLoggedErrors;
		}
	}
}
