// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text;
using Xamarin.Linker;

namespace AssemblyPreparerTests;

public class GenerateMainTests {
	[TestCase (XamarinRuntime.CoreCLR, false, false)]
	[TestCase (XamarinRuntime.CoreCLR, true, false)]
	[TestCase (XamarinRuntime.CoreCLR, true, true)]
	[TestCase (XamarinRuntime.MonoVM, true, false)]
	[TestCase (XamarinRuntime.MonoVM, true, true)]
	[TestCase (XamarinRuntime.NativeAOT, true, false)]
	[TestCase (XamarinRuntime.NativeAOT, true, true)]
	public void ExtensionEntryPoint (XamarinRuntime runtime, bool isExtension, bool isXpcService)
	{
		var directory = Xamarin.Cache.CreateTemporaryDirectory ();
		var configuration = new LinkerConfiguration (ConsoleLog.Instance, [
			"AssemblyName=Service.dll",
			"AreAnyAssembliesTrimmed=false",
			"Platform=macOS",
			$"TargetFramework={TargetFramework.GetTargetFramework (ApplePlatform.MacOSX)}",
			"DeploymentTarget=14.0",
			"LibMonoLinkMode=dylib",
			$"XamarinRuntime={runtime}",
			$"IsAppExtension={isExtension}",
			$"IsXPCService={isXpcService}",
		], Path.Combine (directory, "config.txt")) {
			AssemblyInfos = [],
		};
		var app = configuration.Application;
		app.RootAssemblies.Add ("Service.dll");
		var source = new StringBuilder ();
		app.GenerateMain (source, ApplePlatform.MacOSX, Abi.ARM64, Path.Combine (directory, "main.mm"), []);

		var nativeExtension = isExtension && !isXpcService;
		var launchMode = nativeExtension ? "XamarinLaunchModeExtension" : "XamarinLaunchModeApp";
		Assert.That (source.ToString (), Does.Contain ($"xamarin_main (argc, argv, {launchMode})"), "Launch mode");
		var placeholderMain = "extern \"C\" int __managed__Main";
		if (nativeExtension && runtime == XamarinRuntime.NativeAOT)
			Assert.That (source.ToString (), Does.Contain (placeholderMain), "Class library entry point");
		else
			Assert.That (source.ToString (), Does.Not.Contain (placeholderMain), "Managed entry point must not be replaced");

		var saved = new List<string> ();
		configuration.Save (saved);
		Assert.That (saved.Contains ("IsXPCService=true"), Is.EqualTo (isXpcService), "XPC flag survives configuration serialization");
	}
}
