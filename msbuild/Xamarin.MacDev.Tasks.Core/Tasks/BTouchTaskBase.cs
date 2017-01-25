// Copyright (C) 2011,2012 Xamarin, Inc. All rights reserved.

using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Xamarin.MacDev.Tasks {
	public abstract class BTouchTaskBase : ToolTask {

		public string SessionId { get; set; }

		public string OutputPath { get; set; }

		[Required]
		public string BTouchToolPath { get; set; }

		[Required]
		public string BTouchToolExe { get; set; }

		public ITaskItem[] ObjectiveCLibraries { get; set; }

		public ITaskItem[] AdditionalLibPaths { get; set; }

		public bool AllowUnsafeBlocks { get; set; }

		public string CompilerPath { get; set; }

		public bool NoStdLib { get; set; }

		[Required]
		public string BaseLibDll { get; set; }

		[Required]
		public ITaskItem[] ApiDefinitions { get; set; }

		public ITaskItem[] CoreSources { get; set; }

		public string DefineConstants { get; set; }

		public bool EmitDebugInformation { get; set; }

		public string GeneratedSourcesDir { get; set; }

		public string GeneratedSourcesFileList { get; set; }

		public string Namespace { get; set; }

		public ITaskItem[] NativeLibraries { get; set; }

		public string OutputAssembly { get; set; }

		public bool ProcessEnums { get; set; }

		public ITaskItem[] References { get; set; }

		public ITaskItem[] Resources { get; set; }

		public ITaskItem[] Sources { get; set; }

		protected override string ToolName {
			get { return Path.GetFileNameWithoutExtension (ToolExe); }
		}

		protected override string GenerateFullPathToTool ()
		{
			return Path.Combine (ToolPath, ToolExe);
		}

		protected virtual void HandleReferences (CommandLineBuilder cmd)
		{
			if (References != null) {
				foreach (var item in References)
					cmd.AppendSwitchIfNotNull ("-r ", Path.GetFullPath (item.ItemSpec));
			}
		}

		protected override string GenerateCommandLineCommands ()
		{
			var cmd = new CommandLineBuilder ();
			#if DEBUG
			cmd.AppendSwitch ("/v");
			#endif
			if (NoStdLib)
				cmd.AppendSwitch ("/nostdlib");
			cmd.AppendSwitchIfNotNull ("/compiler:", CompilerPath);
			cmd.AppendSwitchIfNotNull ("/baselib:", BaseLibDll);
			cmd.AppendSwitchIfNotNull ("/out:", OutputAssembly);

			if (NoStdLib) {
				string dir;
				if (!string.IsNullOrEmpty (BaseLibDll))
					dir = Path.GetDirectoryName (BaseLibDll);
				else
					dir = null;
				cmd.AppendSwitchIfNotNull ("/lib:", dir);
				cmd.AppendSwitchIfNotNull ("/r:", Path.Combine (dir, "mscorlib.dll"));
			}

			if (ProcessEnums)
				cmd.AppendSwitch ("/process-enums");

			if (EmitDebugInformation)
				cmd.AppendSwitch ("/debug");

			if (AllowUnsafeBlocks)
				cmd.AppendSwitch ("/unsafe");

			cmd.AppendSwitchIfNotNull ("/ns:", Namespace);

			if (!string.IsNullOrEmpty (DefineConstants)) {
				var strv = DefineConstants.Split (new [] { ';' });
				var sanitized = new List<string> ();

				foreach (var str in strv) {
					if (str != string.Empty)
						sanitized.Add (str);
				}

				if (sanitized.Count > 0)
					cmd.AppendSwitchIfNotNull ("/d:", string.Join (";", sanitized.ToArray ()));
			}

			//cmd.AppendSwitch ("/e");

			foreach (var item in ApiDefinitions)
				cmd.AppendFileNameIfNotNull (Path.GetFullPath (item.ItemSpec));

			if (CoreSources != null) {
				foreach (var item in CoreSources)
					cmd.AppendSwitchIfNotNull ("/s:", Path.GetFullPath (item.ItemSpec));
			}

			if (Sources != null) {
				foreach (var item in Sources)
					cmd.AppendSwitchIfNotNull ("/x:", Path.GetFullPath (item.ItemSpec));
			}

			if (AdditionalLibPaths != null) {
				foreach (var item in AdditionalLibPaths)
					cmd.AppendSwitchIfNotNull ("/lib:", Path.GetFullPath (item.ItemSpec));
			}

			HandleReferences (cmd);

			if (Resources != null) {
				foreach (var item in Resources) {
					var args = new List<string> ();
					string id;

					args.Add (item.ToString ());
					id = item.GetMetadata ("LogicalName");
					if (!string.IsNullOrEmpty (id))
						args.Add (id);

					cmd.AppendSwitchIfNotNull ("/res:", args.ToArray (), ",");
				}
			}

			if (NativeLibraries != null) {
				foreach (var item in NativeLibraries) {
					var args = new List<string> ();
					string id;

					args.Add (item.ToString ());
					id = item.GetMetadata ("LogicalName");
					if (string.IsNullOrEmpty (id))
						id = Path.GetFileName (args[0]);
					args.Add (id);

					cmd.AppendSwitchIfNotNull ("/link-with:", args.ToArray (), ",");
				}
			}

			if (GeneratedSourcesDir != null)
				cmd.AppendSwitchIfNotNull ("/tmpdir:", Path.GetFullPath (GeneratedSourcesDir));

			if (GeneratedSourcesFileList != null)
				cmd.AppendSwitchIfNotNull ("/sourceonly:", Path.GetFullPath (GeneratedSourcesFileList));

			return cmd.ToString ();
		}

		public override bool Execute ()
		{
			ToolExe = BTouchToolExe;
			ToolPath = BTouchToolPath;

			if (!string.IsNullOrEmpty (SessionId) &&
			    !string.IsNullOrEmpty (GeneratedSourcesDir) &&
			    !Directory.Exists (GeneratedSourcesDir)) {
				Directory.CreateDirectory (GeneratedSourcesDir);
			}

			Log.LogTaskName ("BTouch");
			Log.LogTaskProperty ("BTouchToolPath", BTouchToolPath);
			Log.LogTaskProperty ("BTouchToolExe", BTouchToolExe);
			Log.LogTaskProperty ("AdditionalLibPaths", AdditionalLibPaths);
			Log.LogTaskProperty ("AllowUnsafeBlocks", AllowUnsafeBlocks);
			Log.LogTaskProperty ("ApiDefinitions", ApiDefinitions);
			Log.LogTaskProperty ("BaseLibDll", BaseLibDll);
			Log.LogTaskProperty ("CompilerPath", CompilerPath);
			Log.LogTaskProperty ("CoreSources", CoreSources);
			Log.LogTaskProperty ("DefineConstants", DefineConstants);
			Log.LogTaskProperty ("EmitDebugInformation", EmitDebugInformation);
			Log.LogTaskProperty ("GeneratedSourcesDir", GeneratedSourcesDir);
			Log.LogTaskProperty ("GeneratedSourcesFileList", GeneratedSourcesFileList);
			Log.LogTaskProperty ("Namespace", Namespace);
			Log.LogTaskProperty ("NativeLibraries", NativeLibraries);
			Log.LogTaskProperty ("NoStdLib", NoStdLib);
			Log.LogTaskProperty ("OutputAssembly", OutputAssembly);
			Log.LogTaskProperty ("ProcessEnums", ProcessEnums);
			Log.LogTaskProperty ("References", References);
			Log.LogTaskProperty ("Resources", Resources);
			Log.LogTaskProperty ("Sources", Sources);

			if (ApiDefinitions.Length == 0) {
				Log.LogError ("No API definition file specified.");
				return false;
			}

			return base.Execute ();
		}
	}
}
