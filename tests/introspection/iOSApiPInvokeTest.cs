//
// Test the existing of p/invoked symbols
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2014-2015 Xamarin Inc. All rights reserved.
//

using System.Linq;
using System.Reflection;
using UIKit;
using Xamarin.Utils;

namespace Introspection {

	[TestFixture]
	// we want the tests to be available because we use the linker
	[Preserve (AllMembers = true)]
	public class iOSApiPInvokeTest : ApiPInvokeTest {
		protected override bool SkipAssembly (Assembly a)
		{
			if (a == typeof (NSObject).Assembly) {
				// Don't test our product assembly in the simulator, because the
				// simulator might lack some libraries and symbols.
				if (TestRuntime.IsSimulator)
					return true;

				// Also we only want to check this on the OS version we're actually built for.
				var sdk = new Version (Constants.SdkVersion);
				if (!UIDevice.CurrentDevice.CheckSystemVersion (sdk.Major, sdk.Minor))
					return true;
			}

			return false;
		}

		[Test]
		public void MonoNativeFunctionWrapper ()
		{
			var nativeDelegates = from type in Assembly.GetTypes ()
								  where !Skip (type)
								  let attr = type.GetCustomAttribute<MonoNativeFunctionWrapperAttribute> ()
								  where attr is not null
								  select type;

			Errors = 0;
			int c = 0, n = 0;
			foreach (var t in nativeDelegates) {
				if (LogProgress)
					Console.WriteLine ("{0}. {1}", c++, t);

				foreach (var mi in t.GetMethods ()) {
					if (mi.DeclaringType == t)
						CheckSignature (mi);
				}
				n++;
			}
			AssertIfErrors ("{0} errors found in {1} native delegate validated", Errors, n);
		}

		[Test]
		public void MonoPInvokeCallback ()
		{
			Errors = 0;
			int c = 0, n = 0;
			foreach (var type in Assembly.GetTypes ()) {
				if (Skip (type))
					continue;
				foreach (var mi in type.GetMethods (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)) {
					if (mi.DeclaringType != type)
						continue;
					if (Skip (mi))
						continue;
					var attr = mi.GetCustomAttribute<MonoPInvokeCallbackAttribute> ();
					if (attr is null)
						continue;

					if (LogProgress)
						Console.WriteLine ("{0}. {1}", c++, mi);

					var at = attr.DelegateType;
					foreach (var m in at.GetMethods ()) {
						if (m.DeclaringType != at)
							continue;
						CheckSignature (m);
					}
					n++;
				}
			}
			AssertIfErrors ("{0} errors found in {1} native delegate validated", Errors, n);
		}
	}
}
