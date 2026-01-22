using System;
using System.IO;
using NUnit.Framework;

using Xamarin.MacDev.Tasks;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class GetBundleNameTaskTests : TestBase {
		[Test]
		public void GetBundleName_MissingName ()
		{
			var task = CreateTask<GenerateBundleName> ();
			ExecuteTask (task, 1);
		}

		[Test]
		public void GetBundleName ()
		{
			var task = CreateTask<GenerateBundleName> ();
			task.ProjectName = "!@£///Hello_World%£";

			ExecuteTask (task);
			Assert.AreEqual ("Hello_World", task.BundleName, "#2");
		}
	}
}
