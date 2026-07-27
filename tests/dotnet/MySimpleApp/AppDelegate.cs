using System;
using System.Reflection;
using System.Runtime.InteropServices;

using Foundation;
using ObjCRuntime;

namespace MySimpleApp {
#if EXPORT_ATTRIBUTE_REMOVAL
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
#endif

	public class Program {
#if EXPORT_ATTRIBUTE_REMOVAL_NSXPC
		static void UseNSXpcInterfaceMethodInfoOverload (NSXpcInterface value, MethodInfo method)
		{
			value.GetAllowedClasses (method, 0, false);
		}
#endif

		static int Main (string [] args)
		{
			GC.KeepAlive (typeof (NSObject)); // prevent linking away the platform assembly

#if EXPORT_ATTRIBUTE_REMOVAL
			GC.KeepAlive (typeof (INSUrlSessionDelegate));
			GC.KeepAlive (typeof (ExportMetadataApplicationType));
			Console.WriteLine (NSBundle.MainBundle.BundlePath);
			Console.WriteLine (new ExportMetadataApplicationType ().Description);
#endif

			Console.WriteLine (Environment.GetEnvironmentVariable ("MAGIC_WORD"));

			return args.Length;
		}
	}
}
