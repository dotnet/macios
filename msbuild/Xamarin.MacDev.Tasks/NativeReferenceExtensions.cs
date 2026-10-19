// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;

using Microsoft.Build.Framework;

namespace Xamarin.MacDev.Tasks {
	static class NativeReferenceExtensions {
		public static bool IsSystemFramework (this ITaskItem item)
		{
			var path = item.ItemSpec.Replace ('\\', '/').TrimEnd ('/');
			return item.GetMetadata ("Kind") == "Framework"
				&& path.StartsWith ("/System/Library/Frameworks/", StringComparison.Ordinal)
				&& path.EndsWith (".framework", StringComparison.Ordinal);
		}
	}
}
