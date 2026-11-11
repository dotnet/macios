// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;
using System.Linq;

using NUnit.Framework;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class SayHelloTaskTests : TestBase {
		[TestCase (false)]
		[TestCase (true)]
		public void ReadCredentialFile (bool passPhrase)
		{
			const string credential = "placeholder-credential with spaces\n";
			var path = CreateTempFile ("credential");
			File.WriteAllText (path, credential);
			var task = CreateTask<Messaging.SayHello> ();
			if (passPhrase) {
				task.SSHPassPhrase = "previous-credential";
				task.SSHPassPhraseFile = path;
			} else {
				task.ServerPassword = "previous-credential";
				task.ServerPasswordFile = path;
			}
			task.ContinueOnDisconnected = false;

			ExecuteTask (task, expectedErrorCount: 1);

			Assert.That (passPhrase ? task.SSHPassPhrase : task.ServerPassword, Is.EqualTo (credential), "Credential");
			Assert.That (string.Join ("\n", Engine.Logger.AllEvents.Select (v => v.Message)), Does.Not.Contain (credential), "Logged credential");
		}

		[TestCase (false, false)]
		[TestCase (false, true)]
		[TestCase (true, false)]
		[TestCase (true, true)]
		public void InvalidCredentialFile (bool passPhrase, bool createEmptyFile)
		{
			var directory = Cache.CreateTemporaryDirectory ();
			var path = Path.Combine (directory, "credential");
			if (createEmptyFile)
				File.WriteAllText (path, "");
			var task = CreateTask<Messaging.SayHello> ();
			if (passPhrase)
				task.SSHPassPhraseFile = path;
			else
				task.ServerPasswordFile = path;

			ExecuteTask (task, expectedErrorCount: 1);

			Assert.That (Engine.Logger.ErrorEvents.Single ().Message, Does.Contain (path), "Error identifies credential file");
			if (createEmptyFile)
				Assert.That (Engine.Logger.ErrorEvents.Single ().Message, Does.Contain ("is empty"), "Empty file error");
		}
	}
}
