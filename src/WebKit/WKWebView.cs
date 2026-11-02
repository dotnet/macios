// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Threading;

namespace WebKit {
	public partial class WKWebView {
#if !COREBUILD
		const int nativeMemoryPressure = 32 * 1024 * 1024;
		bool memoryPressureInitialized;
		int memoryPressure;

		internal override void InitializeNativeMemoryPressure ()
		{
			if (memoryPressureInitialized)
				return;

			memoryPressureInitialized = true;
			if (AppContext.TryGetSwitch ("WebKit.WKWebView.EnableMemoryPressure", out var enabled) && !enabled)
				return;

			GC.AddMemoryPressure (nativeMemoryPressure);
			memoryPressure = nativeMemoryPressure;
		}

		internal override void ReleaseNativeMemoryPressure ()
		{
			var pressure = Interlocked.Exchange (ref memoryPressure, 0);
			if (pressure != 0)
				GC.RemoveMemoryPressure (pressure);
		}
#endif
	}
}
