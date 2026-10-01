// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using MonoTouch.NUnit.UI;

namespace Xamarin.Tests {
	[TestFixture]
	[NonParallelizable]
	public class ConsoleTextWriterTests {
		[Test]
		public void UsesCurrentConsoleWriter ()
		{
			var originalWriter = Console.Out;
			using var firstWriter = new StringWriter ();
			using var secondWriter = new StringWriter ();

			try {
				var writer = new ConsoleTextWriter ();

				Console.SetOut (firstWriter);
				writer.WriteLine ("first");
				Console.SetOut (secondWriter);
				writer.WriteLine ("second");
			} finally {
				Console.SetOut (originalWriter);
			}

			Assert.That (firstWriter.ToString (), Is.EqualTo ($"first{Environment.NewLine}"));
			Assert.That (secondWriter.ToString (), Is.EqualTo ($"second{Environment.NewLine}"));
		}
	}
}
