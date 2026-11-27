using System;

namespace Microsoft.Build.Framework {
	[AttributeUsage (AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	internal sealed class MSBuildMultiThreadableTaskAttribute : Attribute {
	}
}
