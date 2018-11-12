﻿using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace BCLTestImporter {
	public static class RegisterTypeGenerator {

		static string UsingReplacement = "%USING%";
		static string KeysReplacement = "%KEY VALUES%";
		
		// Generates the code for the type registration using the give path to the template to use
		public static string GenerateCode (Dictionary <string, Type> typeRegistration, string templatePath)
		{
			var importStringBuilder = new StringBuilder ();
			var keyValuesStringBuilder = new StringBuilder ();
			var namespaces = new List<string> ();  // keep track of the namespaces to remove warnings
			foreach (var a in typeRegistration.Keys) {
				var t = typeRegistration [a];
				if (!string.IsNullOrEmpty (t.Namespace)) {
					if (!namespaces.Contains (t.Namespace)) {
						namespaces.Add (t.Namespace);
						importStringBuilder.AppendLine ($"using {t.Namespace};");
					}
					keyValuesStringBuilder.AppendLine ($"\t\t\t{{ \"{a}\", typeof ({t.FullName})}}, ");
				}
			}
			// got the lines we want to add, greab template and substitude
			using (StreamReader reader = new StreamReader(templatePath))
			{
				string result = reader.ReadToEnd();
				result = result.Replace (UsingReplacement, importStringBuilder.ToString ());
				result = result.Replace (KeysReplacement, keyValuesStringBuilder.ToString ());
				return result;
			}
		}
	}
}
