// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Threading;

namespace WebKit {
	public partial class WKWebView {
#if !COREBUILD
		const int nativeMemoryPressure = 32 * 1024 * 1024;
		int memoryPressure = AddMemoryPressure ();

		static int AddMemoryPressure ()
		{
			if (AppContext.TryGetSwitch ("WebKit.WKWebView.EnableMemoryPressure", out var enabled) && !enabled)
				return 0;

			GC.AddMemoryPressure (nativeMemoryPressure);
			return nativeMemoryPressure;
		}

		void RemoveMemoryPressure ()
		{
			var pressure = Interlocked.Exchange (ref memoryPressure, 0);
			if (pressure != 0)
				GC.RemoveMemoryPressure (pressure);
		}
#endif
	}
}
