// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System;
using System.Collections.Generic;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using Xamarin.MacDev;

namespace Xamarin.MacDev.Tasks {
	static class PListItemGroup {
		public static void Merge (
			TaskLoggingHelper log,
			PDictionary dictionary,
			IEnumerable<ITaskItem>? items,
			Func<PString, string, PString> transformString,
			string invalidRemoveValueMessage,
			string invalidBooleanValueMessage,
			string unknownTypeMessage)
		{
			if (items is null)
				return;

			foreach (var item in items) {
				var key = item.ItemSpec;
				var type = item.GetMetadata ("Type");
				var value = item.GetMetadata ("Value");

				switch (type.ToLowerInvariant ()) {
				case "remove":
					if (!string.IsNullOrEmpty (value))
						log.LogError (invalidRemoveValueMessage, value, key, type);
					dictionary.Remove (key);
					break;
				case "boolean":
					if (!TryParseBooleanStrict (value, out var booleanValue)) {
						log.LogError (invalidBooleanValueMessage, value, key, type);
						continue;
					}
					dictionary [key] = new PBoolean (booleanValue);
					break;
				case "string":
					dictionary [key] = transformString (new PString (value), key);
					break;
				case "stringarray":
					var arraySeparator = item.GetMetadata ("ArraySeparator");
					if (string.IsNullOrEmpty (arraySeparator))
						arraySeparator = ";";
					var array = new PArray ();
					foreach (var element in value.Split (new [] { arraySeparator }, StringSplitOptions.None))
						array.Add (transformString (new PString (element), key));
					dictionary [key] = array;
					break;
				default:
					log.LogError (unknownTypeMessage, type, key);
					break;
				}
			}
		}

		public static bool TryParseBooleanStrict (string value, out bool result)
		{
			if (string.Equals (value, "true", StringComparison.OrdinalIgnoreCase)) {
				result = true;
				return true;
			}

			if (string.Equals (value, "false", StringComparison.OrdinalIgnoreCase)) {
				result = false;
				return true;
			}

			result = false;
			return false;
		}
	}
}
