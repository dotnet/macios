// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;
using System.Threading;

using Xamarin.Bundler;
using Xamarin.Utils;

namespace GeneratorTests {
	[TestFixture]
	public class LoggingTests {
		[Test]
		[NonParallelizable]
		public void ConsoleLog ()
		{
			var output = new StringWriter ();
			var error = new StringWriter ();
			var previousOutput = Console.Out;
			var previousError = Console.Error;

			try {
				Console.SetOut (output);
				Console.SetError (error);

				var log = new ConsoleLog ();
				log.Log ("normal output");
				log.LogError ("plain error");
				log.LogError (new BindingException (86, true));
				log.LogWarning (new BindingException (1027, false));
				ErrorHelper.Show (log, new InvalidOperationException ("unexpected"), false);
			} finally {
				Console.SetOut (previousOutput);
				Console.SetError (previousError);
			}

			Assert.That (output.ToString (), Does.Contain ("normal output"));
			Assert.That (output.ToString (), Does.Contain ("error BI0086:"));
			Assert.That (output.ToString (), Does.Contain ("warning BI1027:"));
			Assert.That (output.ToString (), Does.Contain ("error BI0000: Unexpected error"));
			Assert.That (output.ToString (), Does.Contain ("System.InvalidOperationException: unexpected"));
			Assert.That (error.ToString (), Is.EqualTo ($"plain error{Environment.NewLine}"));
		}

		[Test]
		public void InjectedLog ()
		{
			var log = new TestLog ();

			var exitCode = BindingTouch.Run (new [] { "--use-zero-copy", "--help" }, log, CancellationToken.None);

			Assert.That (exitCode, Is.EqualTo (0));
			Assert.That (log.Messages, Has.Some.Contains ("Mono Objective-C API binder"));
			Assert.That (log.Warnings, Has.Count.EqualTo (1));
			Assert.That (log.Warnings [0].Code, Is.EqualTo (1027));
			Assert.That (log.Errors, Is.Empty);
		}

		[Test]
		public void InjectedLogError ()
		{
			var log = new TestLog ();

			var exitCode = BindingTouch.Run (new [] { "api.cs" }, log, CancellationToken.None);

			Assert.That (exitCode, Is.EqualTo (1));
			Assert.That (log.Errors, Has.Count.EqualTo (1));
			Assert.That (log.Errors [0].Code, Is.EqualTo (86));
		}

		[Test]
		public void InjectedLogUnexpectedError ()
		{
			var log = new TestLog ();

			ErrorHelper.Show (log, new InvalidOperationException ("unexpected"), false);

			Assert.That (log.Messages, Has.Some.StartsWith ("error BI0000: Unexpected error"));
			Assert.That (log.Exceptions, Has.Count.EqualTo (1));
			Assert.That (log.Exceptions [0].Message, Is.EqualTo ("unexpected"));
		}

		[Test]
		public void Cancellation ()
		{
			var cancellationToken = new CancellationToken (true);

			Assert.Throws<OperationCanceledException> (() => BindingTouch.Run ([], new TestLog (), cancellationToken));
		}

		class TestLog : IToolLog {
			public int Verbosity => 0;
			public ApplePlatform Platform => ApplePlatform.None;
			public List<string> Messages { get; } = new ();
			public List<BindingException> Errors { get; } = new ();
			public List<BindingException> Warnings { get; } = new ();
			public List<Exception> Exceptions { get; } = new ();

			public void Log (string message)
			{
				Messages.Add (message);
			}

			public void LogError (string message)
			{
				Messages.Add (message);
			}

			public void LogError (BindingException exception)
			{
				Errors.Add (exception);
			}

			public void LogWarning (BindingException exception)
			{
				Warnings.Add (exception);
			}

			public void LogException (Exception exception)
			{
				Exceptions.Add (exception);
			}
		}
	}
}
