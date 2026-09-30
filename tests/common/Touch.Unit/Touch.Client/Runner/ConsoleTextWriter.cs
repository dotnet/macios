// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Text;

namespace MonoTouch.NUnit.UI {
	class ConsoleTextWriter : TextWriter {
		public override Encoding Encoding {
			get {
				return Console.Out.Encoding;
			}
		}

		public override void Write (char value)
		{
			Console.Out.Write (value);
		}

		public override void Write (char []? buffer)
		{
			Console.Out.Write (buffer);
		}

		public override void WriteLine (string? value)
		{
			Console.Out.WriteLine (value);
		}
	}
}
