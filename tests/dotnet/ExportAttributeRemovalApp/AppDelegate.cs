// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Reflection;

using Foundation;
using ObjCRuntime;

namespace ExportAttributeRemovalApp {
	[Register]
	class ExportMetadataApplicationType : NSObject {
		[Export ("applicationExport")]
		public void ApplicationExport ()
		{
		}

		[Action ("applicationAction:")]
		public void ApplicationAction (NSObject sender)
		{
		}

		[Outlet ("applicationOutlet")]
		public NSObject? ApplicationOutlet { get; set; }

		public override string Description => base.Description;
	}

	public class Program {
#if EXPORT_ATTRIBUTE_REMOVAL_NSXPC
		static void UseNSXpcInterfaceMethodInfoOverload (NSXpcInterface value, MethodInfo method)
		{
			value.GetAllowedClasses (method, 0, false);
		}
#endif

		static int Main (string [] args)
		{
			GC.KeepAlive (typeof (NSObject));
			GC.KeepAlive (typeof (INSUrlSessionDelegate));
			GC.KeepAlive (typeof (ExportMetadataApplicationType));
			Console.WriteLine (NSBundle.MainBundle.BundlePath);
			Console.WriteLine (new ExportMetadataApplicationType ().Description);

			return args.Length;
		}
	}
}
