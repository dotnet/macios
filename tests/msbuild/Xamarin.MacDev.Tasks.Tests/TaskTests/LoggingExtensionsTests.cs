// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using NUnit.Framework;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class LoggingExtensionsTests : TestBase {
		[TestCase (137, "MT0137")]
		[TestCase (7194, "MT7194")]
		public void DiagnosticCodes (int code, string expectedCode)
		{
			var task = CreateTask<GenerateBundleName> ();
			task.Log.LogWarning (code, null, "Test warning");
			task.Log.LogWarning (code, null, 42, "Test warning with location");
			task.Log.LogError (code, null, "Test error");
			task.Log.LogError (code, null, 42, "Test error with location");

			Assert.That (Engine.Logger.WarningsEvents, Has.Count.EqualTo (2), "Warnings");
			Assert.That (Engine.Logger.ErrorEvents, Has.Count.EqualTo (2), "Errors");
			foreach (var warning in Engine.Logger.WarningsEvents)
				Assert.That (warning.Code, Is.EqualTo (expectedCode), "Warning code");
			foreach (var error in Engine.Logger.ErrorEvents)
				Assert.That (error.Code, Is.EqualTo (expectedCode), "Error code");
			Assert.That (Engine.Logger.WarningsEvents [1].LineNumber, Is.EqualTo (42), "Warning location");
			Assert.That (Engine.Logger.ErrorEvents [1].LineNumber, Is.EqualTo (42), "Error location");
		}
	}
}
