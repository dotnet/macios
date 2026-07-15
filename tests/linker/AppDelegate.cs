using System.Reflection;
using System.Linq;

public static partial class TestLoader {
	static partial void AddTestAssembliesImpl (HashSet<Assembly> assemblies)
	{
		assemblies.Add (typeof (BundledResources.ResourcesTest).Assembly);
	}
}

[TestFixture]
public class LoaderTest {
	public void TestAssemblyCount ()
	{
		Assert.That (TestLoader.GetTestAssemblies ().Count (), Is.EqualTo (2), "Test assembly count");
	}
}
