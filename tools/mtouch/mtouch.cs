// Copyright 2009, Novell, Inc.
// Copyright 2011-2013 Xamarin Inc. All rights reserved.
//
// Authors:
//   Miguel de Icaza
//   Geoff Norton
//   Jb Evain
//   Sebastien Pouliot
//

using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;

using Mono.Options;

using ObjCRuntime;

using Xamarin.Utils;

public enum OutputFormat {
	Default,
	Xml,
}

namespace Xamarin.Bundler {
	public partial class Driver {
		internal const string NAME = "mtouch";

		public static void ShowHelp (OptionSet os)
		{
			Console.WriteLine ("mtouch - Mono Compiler for iOS");
			Console.WriteLine ("Copyright 2009-2011, Novell, Inc.");
			Console.WriteLine ("Copyright 2011-2016, Xamarin Inc.");
			Console.WriteLine ("Usage is: mtouch [options]");

			os.WriteOptionDescriptions (Console.Out);
		}

		enum Action {
			None,
			Help,
			Version,
			RunRegistrar,
		}

		static int Main2 (string [] args)
		{
			var action = Action.None;
			var app = new Application (args);
			var os = new OptionSet ();
			if (ParseOptions (app, os, args, ref action))
				return 0;

			ValidateXcode (app, false, false);

			if (action != Action.RunRegistrar)
				throw ErrorHelper.CreateError (99, Errors.MX0099, "Only --runregistrar is supported.");

			app.RunRegistrar ();

			return 0;
		}
	}
}
