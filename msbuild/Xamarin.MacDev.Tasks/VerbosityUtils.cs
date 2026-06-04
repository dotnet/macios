// Copyright (c) Microsoft Corp

using System;
using Microsoft.Build.Framework;
using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {

	// Helper code for Task delegating work to external tools but that still
	// needs to be verbosity-aware
	public static class VerbosityUtils {

		public static void RenderVerbosity (IList<string> arguments, int taskVerbosity)
		{
			if (taskVerbosity == 0)
				return;

			for (var i = 0; i < Math.Abs (taskVerbosity); i++)
				arguments.Add (taskVerbosity < 0 ? "-q" : "-v");
		}

		//
		// This is an hack, since there can be multiple loggers.
		// However it should cover most use cases.
		//
		// msbuild argument format
		//	-verbosity:<level> Display this amount of information in the event log.
		//		The available verbosity levels are: q[uiet], m[inimal],
		//		n[ormal], d[etailed], and diag[nostic]. (Short form: -v)
		//
		public static int GetVerbosityLevel (string commandLine)
		{
			if (!StringUtils.TryParseArguments (commandLine, out var args, out _))
				return 0;

			var hasBinaryLog = false;
			foreach (var arg in args) {
				// the minimum length we're looking for is `/bl`
				if (arg.Length < 2)
					continue;
				// msbuild accepts two types of argument separator
				if (arg [0] != '/' && arg [0] != '-')
					continue;


				var colon = arg.IndexOf (':');
				var name = arg.Substring (1, colon == -1 ? arg.Length - 1 : colon - 1);
				var value = colon == -1 ? string.Empty : arg.Substring (colon + 1);

				// the argument is not case sensitive
				switch (name.ToLowerInvariant ()) {
				case "v":
				case "verbosity":
					var verbosity = value;
					// case sensitive
					switch (verbosity) {
					case "q":
					case "quiet":
						return GetVerbosityLevel (LoggerVerbosity.Quiet);
					case "m":
					case "minimal":
						return GetVerbosityLevel (LoggerVerbosity.Minimal);
					case "n":
					case "normal":
					default:
						return GetVerbosityLevel (LoggerVerbosity.Normal);
					case "d":
					case "detailed":
						return GetVerbosityLevel (LoggerVerbosity.Detailed);
					case "diag":
					case "diagnostic":
						return GetVerbosityLevel (LoggerVerbosity.Diagnostic);
					}
				case "bl":
				case "binarylogger":
					hasBinaryLog = true;
					break;
				}
			}

			// A binary log was requested, and verbosity wasn't specified, so default to diagnostic.
			if (hasBinaryLog)
				return GetVerbosityLevel (LoggerVerbosity.Diagnostic);

			// nothing is normal
			return GetVerbosityLevel (LoggerVerbosity.Normal);
		}

		// The values here come from: https://github.com/mono/monodevelop/blob/143f9b6617123a0841a5cc5a2a4e13b309535792/main/src/core/MonoDevelop.Projects.Formats.MSBuild/MonoDevelop.Projects.MSBuild.Shared/RemoteBuildEngineMessages.cs#L186
		// Assume 'Normal (2)' is the default verbosity (no change), and the other values follow from there.
		public static int GetVerbosityLevel (LoggerVerbosity v)
		{
			switch ((LoggerVerbosity) v) {
			case LoggerVerbosity.Quiet:
				return -4;
			case LoggerVerbosity.Minimal:
				return -2;
			case LoggerVerbosity.Normal:
			default:
				return 0;
			case LoggerVerbosity.Detailed:
				return 2;
			case LoggerVerbosity.Diagnostic:
				return 4;
			}
		}
	}
}
