// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace AssemblyPreparerTests;

public class DisposeTests : BaseClass {
	[Test]
	public void PreTrimAssemblyResolver ()
	{
		var resolver = new TestResolver ();
		var assembly = AssemblyDefinition.CreateAssembly (new AssemblyNameDefinition ("Test", new Version (1, 0)), "Test", ModuleKind.Dll);
		resolver.CacheAssembly (assembly);

		var preparer = CreatePreparer (ApplePlatform.iOS, false, p => p.Configuration.Application.PreTrimAssemblyResolver = resolver, "public class C {}", out _);

		Assert.That (resolver.IsDisposed, Is.False, "Resolver disposed before preparer");
		preparer.Dispose ();
		Assert.That (resolver.IsDisposed, Is.True, "Resolver disposed by preparer");
		Assert.That (resolver.ResolverCache, Is.Empty, "Resolver cache");
	}

	sealed class TestResolver : CoreResolver {
		public bool IsDisposed { get; private set; }

		public override AssemblyDefinition Resolve (AssemblyNameReference name, ReaderParameters parameters)
		{
			throw new AssemblyResolutionException (name);
		}

		public override void Dispose ()
		{
			base.Dispose ();
			IsDisposed = true;
		}
	}
}
