// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;
using System.Linq;

using NUnit.Framework;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class SayHelloTaskTests : TestBase {
		[Test]
		public void ReadPasswordFile ()
		{
			const string password = "placeholder-password with spaces\n";
			var path = CreateTempFile ("password");
			File.WriteAllText (path, password);
			var task = CreateTask<Messaging.SayHello> ();
			task.ServerPassword = "previous-password";
			task.ServerPasswordFile = path;
			task.ContinueOnDisconnected = false;

			ExecuteTask (task, expectedErrorCount: 1);

			Assert.That (task.ServerPassword, Is.EqualTo (password), "Password");
			Assert.That (string.Join ("\n", Engine.Logger.AllEvents.Select (v => v.Message)), Does.Not.Contain (password), "Logged password");
		}

		[Test]
		[TestCase (false)]
		[TestCase (true)]
		public void InvalidPasswordFile (bool createEmptyFile)
		{
			var directory = Cache.CreateTemporaryDirectory ();
			var path = Path.Combine (directory, "password");
			if (createEmptyFile)
				File.WriteAllText (path, "");
			var task = CreateTask<Messaging.SayHello> ();
			task.ServerPasswordFile = path;

			ExecuteTask (task, expectedErrorCount: 1);

			Assert.That (Engine.Logger.ErrorEvents.Single ().Message, Does.Contain (path), "Error identifies password file");
			if (createEmptyFile)
				Assert.That (Engine.Logger.ErrorEvents.Single ().Message, Does.Contain ("is empty"), "Empty file error");
		}
	}
}
