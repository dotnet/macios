//
// SCNJavaScript.cs: JSC bridge
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

using System;
using System.Runtime.InteropServices;

using ObjCRuntime;
using JavaScriptCore;

#nullable enable

namespace SceneKit {
#if NET
	/// <summary>Static class that contains a method to export JavaScript modules.</summary>
	///     <remarks>To be added.</remarks>
	///     <!-- TODO: Probably https://developer.apple.com/library/prerelease/mac/documentation/SceneKit/Reference/SCNAction_Class/index.html#//apple_ref/occ/clm/SCNAction/javaScriptActionWithScript:duration: -->
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
#endif
	public static class SCNJavaScript {
		[DllImport (Constants.SceneKitLibrary)]
		static extern void SCNExportJavaScriptModule (IntPtr context);

		/// <param name="context">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static void ExportModule (JSContext context)
		{
			if (context is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (context));

			SCNExportJavaScriptModule (context.Handle);
			GC.KeepAlive (context);
		}
	}
}
