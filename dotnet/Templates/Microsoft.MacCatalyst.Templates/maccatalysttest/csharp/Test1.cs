namespace MacCatalystTest1;

[TestClass]
public sealed class Test1 {
	[TestMethod]
	public void TestMethod1 ()
	{
	}

	[TestMethod]
	public void TestMethod2 ()
	{
		Assert.Fail ("This test is expected to fail");
	}

	[TestMethod]
	public void TestMethod3 ()
	{
		Assert.Inconclusive ("This test is expected to be skipped");
	}
}
