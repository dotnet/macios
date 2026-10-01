#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using AppKit;
using Foundation;
using ObjCRuntime;

namespace SecurityInterface {

	[Register ("__MonoMac_SecurityInterfaceSheetDidEndDispatcher")]
	internal sealed class SecurityInterfaceSheetDidEndDispatcher : NSObject {
		const string selectorName = "securityInterfaceSheetDidEnd:returnCode:contextInfo:";
		internal static readonly Selector Selector = new Selector (selectorName);

		GCHandle root;
		Action<NSModalResponse>? action;
		bool completed;

		SecurityInterfaceSheetDidEndDispatcher ()
		{
			IsDirectBinding = false;
		}

		[DynamicDependency (nameof (DidEnd))]
		internal static SecurityInterfaceSheetDidEndDispatcher Create (Action<NSModalResponse> action)
		{
			ArgumentNullException.ThrowIfNull (action);
			var dispatcher = new SecurityInterfaceSheetDidEndDispatcher {
				action = action,
			};
			dispatcher.root = GCHandle.Alloc (dispatcher);
			return dispatcher;
		}

		[Export (selectorName)]
		public void DidEnd (NSWindow sheet, nint returnCode, IntPtr contextInfo)
		{
			if (completed)
				return;

			completed = true;
			var callback = action;
			action = null;
			try {
				callback?.Invoke ((NSModalResponse) returnCode);
			} finally {
				Release ();
			}
		}

		internal void Cancel ()
		{
			if (completed)
				return;
			completed = true;
			action = null;
			Release ();
		}

		void Release ()
		{
			if (root.IsAllocated)
				root.Free ();
			Dispose ();
		}
	}
}
