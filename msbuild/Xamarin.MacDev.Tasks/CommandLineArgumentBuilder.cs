using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Microsoft.Build.Utilities;
using Xamarin.Utils;

#nullable enable

namespace Xamarin.MacDev {
	/// <summary>
	/// Builds a process argument string.
	/// </summary>
	public static class CommandLineArgumentBuilder {
		public static List<string> CreateResponseFile (Task task, string responseFilePath, IList<string> responseArguments, IList<string>? nonResponseArguments)
		{
			// Generate a response file
			var responseFile = Path.GetFullPath (responseFilePath);

			if (File.Exists (responseFile))
				File.Delete (responseFile);

			try {
				File.WriteAllLines (responseFile, StringUtils.Quote (responseArguments.ToArray ()));
			} catch (Exception ex) {
				task.Log.LogWarning ("Failed to create response file '{0}': {1}", responseFile, ex);
			}

			// Some arguments can not safely go in the response file and are 
			// added separately. They must go _after_ the response file
			// as they may override options passed in the response file
			var actualArgs = new List<string> ();

			actualArgs.Add ($"@{responseFile}");
			if (nonResponseArguments is not null)
				actualArgs.AddRange (nonResponseArguments);

			// Generate the command line
			return actualArgs;
		}
	}
}
