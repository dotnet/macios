
/*
 * Copyright 2011-2014 Xamarin Inc. All rights reserved.
 * Copyright 2010 Novell Inc.
 *
 * Authors:
 *   Sebastien Pouliot <sebastien@xamarin.com>
 *   Aaron Bockover <abock@xamarin.com>
 *   Rolf Bjarne Kvinge <rolf@xamarin.com>
 *   Geoff Norton <gnorton@novell.com>
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.IO;

using Mono.Options;

namespace Xamarin.Bundler {
	enum Action {
		None,
		Help,
		Version,
		RunRegistrar,
	}

	public static partial class Driver {
		internal const string NAME = "mmp";
		internal static Application App = new Application (Environment.GetCommandLineArgs ());
		static Target BuildTarget;
		static Action action;

		static void ShowHelp (OptionSet os)
		{
			Console.WriteLine ("mmp - Xamarin.Mac Packer");
			Console.WriteLine ("Copyright 2010 Novell Inc.");
			Console.WriteLine ("Copyright 2011-2016 Xamarin Inc.");
			Console.WriteLine ("Usage: mmp [options] application-exe");
			os.WriteOptionDescriptions (Console.Out);
		}

		internal static Action Action { get => action; }

		public static bool EnableDebug {
			get { return App.EnableDebug; }
		}

		static int Main2 (string [] args)
		{
			var os = new OptionSet ();
			if (ParseOptions (App, os, args, ref action))
				return 0;

			ValidateXcode (App, false, true);

			BuildTarget = new Target (App);
			App.Targets.Add (BuildTarget);
			App.InitializeCommon ();

			if (action != Action.RunRegistrar)
				throw ErrorHelper.CreateError (99, Errors.MX0099, "Only --runregistrar is supported.");

			App.Registrar = RegistrarMode.Static;
			App.RunRegistrar ();
			return 0;
		}
	}
}
