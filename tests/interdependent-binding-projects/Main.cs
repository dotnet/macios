using System.Linq;
using System.Reflection;

#nullable enable

public static partial class TestLoader {
	static partial void AddTestAssembliesImpl (HashSet<Assembly> assemblies)
	{
		assemblies.Add (typeof (Xamarin.BindingTests2.BindingTest).Assembly);
		assemblies.Add (typeof (Xamarin.BindingTests.ProtocolTest).Assembly);
	}
}

[TestFixture]
public class LoaderTest {
	public void TestAssemblyCount ()
	{
		Assert.That (TestLoader.GetTestAssemblies ().Count (), Is.EqualTo (3), "Test assembly count");
	}
}
