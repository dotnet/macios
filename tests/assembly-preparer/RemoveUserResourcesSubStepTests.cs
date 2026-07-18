// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text;

using MonoTouch.Tuner;

using Xamarin.Linker;

namespace AssemblyPreparerTests;

public class RemoveUserResourcesSubStepTests : BaseClass {
	static string GetContentPrefix (ApplePlatform platform)
	{
		switch (platform) {
		case ApplePlatform.iOS:
		case ApplePlatform.TVOS:
		case ApplePlatform.MacCatalyst:
			return "__monotouch_content_";
		case ApplePlatform.MacOSX:
			return "__xammac_content_";
		default:
			throw new NotImplementedException (platform.ToString ());
		}
	}

	// Builds a user assembly with an embedded MonoTouch/XamMac content resource, then runs the
	// RemoveUserResourcesSubStep with the provided value of HotReloadCompatibleBuild and returns the
	// resource names still present in the (in-memory) assembly after the step ran.
	List<string> GetResourcesAfterStep (ApplePlatform platform, bool isCoreCLR, bool hotReloadCompatibleBuild)
	{
		var prefix = GetContentPrefix (platform);
		var resourceName = prefix + "TestResource.bin";
		var content = Encoding.UTF8.GetBytes ("This is an embedded user resource.");

		var extraCsproj = $@"
	<ItemGroup>
		<EmbeddedResource Include=""TestResource.bin"">
			<LogicalName>{resourceName}</LogicalName>
		</EmbeddedResource>
	</ItemGroup>";

		var code = @"
		using Foundation;
		class MyClass : NSObject {
		}";

		var extraConfig = $"HotReloadCompatibleBuild={(hotReloadCompatibleBuild ? "true" : "false")}";

		using var preparer = CreatePreparer (platform, isCoreCLR, p => p.Registrar = RegistrarMode.Dynamic, code, out var testInfo, extraCsproj: extraCsproj, extraConfig: extraConfig, extraFiles: new [] { ("TestResource.bin", content) });

		var context = preparer.Configuration.DerivedLinkContext;
		new LoadAssembliesStep ().Process (context);
		new RemoveUserResourcesSubStep ().Process (context);

		var testAssembly = context.GetAssemblies ().Single (v => v.Name.Name == "Test");
		return testAssembly.MainModule.Resources.Select (v => v.Name).ToList ();
	}

	[Test]
	[TestCase (ApplePlatform.iOS, false)]
	[TestCase (ApplePlatform.TVOS, false)]
	[TestCase (ApplePlatform.MacCatalyst, false)]
	[TestCase (ApplePlatform.MacOSX, true)]
	public void ResourceRemovedByDefault (ApplePlatform platform, bool isCoreCLR)
	{
		var prefix = GetContentPrefix (platform);
		var resources = GetResourcesAfterStep (platform, isCoreCLR, hotReloadCompatibleBuild: false);
		Assert.That (resources, Has.None.StartsWith (prefix), "The user resource should be stripped when HotReloadCompatibleBuild is disabled.");
	}

	[Test]
	[TestCase (ApplePlatform.iOS, false)]
	[TestCase (ApplePlatform.TVOS, false)]
	[TestCase (ApplePlatform.MacCatalyst, false)]
	[TestCase (ApplePlatform.MacOSX, true)]
	public void ResourceKeptForHotReload (ApplePlatform platform, bool isCoreCLR)
	{
		var prefix = GetContentPrefix (platform);
		var resourceName = prefix + "TestResource.bin";
		var resources = GetResourcesAfterStep (platform, isCoreCLR, hotReloadCompatibleBuild: true);

		// The resource must be left untouched: the step must not remove it (and thus not upgrade the
		// user assembly to AssemblyAction.Save, which is what would break Hot Reload).
		Assert.That (resources, Has.Some.EqualTo (resourceName), "The user resource must be kept when HotReloadCompatibleBuild is enabled.");
	}
}
