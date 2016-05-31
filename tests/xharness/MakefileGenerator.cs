﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace xharness
{
	public static class MakefileGenerator
	{
		static void WriteTarget (this StreamWriter writer, string target, string dependencies, params string [] arguments)
		{
			var t = string.Format (target, arguments);
			if (t.Contains ("\\ ")) {
				writer.Write (t.Replace ("\\ ", ""), arguments);
				writer.Write (" ");
			}
			writer.Write (t);
			if (!string.IsNullOrEmpty (dependencies)) {
				writer.Write (": ");
				writer.WriteLine (dependencies, arguments);
			} else {
				writer.WriteLine (":");
			}
		}

		enum MacTargetNameType { Build, Clean, Exec, Run }

		static string MakeMacTargetName (MacTarget target, MacTargetNameType type)
		{
			if (target is MacClassicTarget)
				return MakeMacClassicTargetName (target, type);
			else if (target is MacUnifiedTarget)
				return MakeMacUnifiedTargetName (target, type);
			else
				throw new NotImplementedException ();
		}

		static string MakeMacClassicTargetName (MacTarget target, MacTargetNameType type)
		{
			var make_escaped_suffix = "-" + target.Platform.Replace (" ", "\\ ");
			var make_escaped_name = target.SimplifiedName.Replace (" ", "\\ ");

			switch (type)
			{
			case MacTargetNameType.Build:
				return string.Format ("build{0}-classic-{1}", make_escaped_suffix, make_escaped_name);
			case MacTargetNameType.Clean:
				return string.Format ("clean{0}-classic-{1}", make_escaped_suffix, make_escaped_name);
			case MacTargetNameType.Exec:
				return string.Format ("exec{0}-classic-{1}", make_escaped_suffix, make_escaped_name);
			case MacTargetNameType.Run:
				return string.Format ("run{0}-classic-{1}", make_escaped_suffix, make_escaped_name);
			default:
				throw new NotImplementedException ();
			}
		}

		static string MakeMacUnifiedTargetName (MacTarget target, MacTargetNameType type)
		{
			var make_escaped_suffix = "-" + target.Platform.Replace (" ", "\\ ");
			var make_escaped_name = target.SimplifiedName.Replace (" ", "\\ ");

			switch (type)
			{
				case MacTargetNameType.Build:
					return string.Format ("build{0}-{2}-{1}", make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
				case MacTargetNameType.Clean:
					return string.Format ("clean{0}-{2}-{1}", make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
				case MacTargetNameType.Exec:
					return string.Format ("exec{0}-{2}-{1}", make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
				case MacTargetNameType.Run:
					return string.Format ("run{0}-{2}-{1}", make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
				default:
					throw new NotImplementedException ();
			}
		}

		public static void CreateMacMakefile (Harness harness, IEnumerable<MacTarget> targets)
		{
			var makefile = Path.Combine (harness.RootDirectory, "Makefile-mac.inc");
			using (var writer = new StreamWriter (makefile, false, new UTF8Encoding (false))) {
				writer.WriteLine (".stamp-configure-projects-mac: Makefile xharness/xharness.exe");
				writer.WriteLine ("\t$(Q) $(SYSTEM_MONO) --debug xharness/xharness.exe $(XHARNESS_VERBOSITY) --configure --autoconf --rootdir $(CURDIR)");
				writer.WriteLine ("\t$(Q) touch $@");
				writer.WriteLine ();

				var allTargets = new List<MacTarget> ();
				allTargets.AddRange (targets);

				List<string> allTargetNames = new List<string> (allTargets.Count);
				List<string> allTargetCleanNames = new List<string> (allTargets.Count);

				// build/[install/]run targets for specific test projects.
				foreach (var target in allTargets) {
					var make_escaped_simplified_name = target.SimplifiedName.Replace (" ", "\\ ");
					var make_escaped_name = target.Name.Replace (" ", "\\ ");

					writer.WriteLine ();
					if (target.ProjectPath != target.TemplateProjectPath) {
						writer.WriteLine ("# {0} for {1}", make_escaped_simplified_name, target.Suffix.Replace ("-", ""));
						writer.WriteLine (".stamp-configure-projects-mac: {0}", target.TemplateProjectPath.Replace (" ", "\\ "));
						writer.WriteLine ("{0}: .stamp-configure-projects-mac", target.ProjectPath.Replace (" ", "\\ "));
						writer.WriteLine ();
					}

					// build project target
					if (target.IsBCLProject) {
						throw new NotImplementedException ();
					}
					else if (target is MacClassicTarget) {
						allTargetNames.Add (MakeMacClassicTargetName (target, MacTargetNameType.Build));
						allTargetCleanNames.Add (MakeMacClassicTargetName (target, MacTargetNameType.Clean));

						writer.WriteTarget (MakeMacClassicTargetName (target, MacTargetNameType.Build), "$(GUI_UNIT_PATH)/bin/net_4_5/GuiUnit.exe");
						writer.WriteLine ("\t$(Q) $(MDTOOL) build {0}", target.ProjectPath);
						writer.WriteLine ();

						writer.WriteTarget (MakeMacClassicTargetName (target, MacTargetNameType.Clean), "");
						writer.WriteLine ("\t$(Q) $(MDTOOL) build -t:clean {0}", target.ProjectPath);
						writer.WriteLine ();

						writer.WriteTarget (MakeMacClassicTargetName (target, MacTargetNameType.Exec), "");
						writer.WriteLine ("\t$(Q) ./{0}/bin/x86/Debug/{0}.app/Contents/MacOS/{0}", make_escaped_name);
						writer.WriteLine ();

						writer.WriteTarget (MakeMacClassicTargetName (target, MacTargetNameType.Run), "");
						writer.WriteLine ("\t$(Q) $(MAKE) {0}", MakeMacClassicTargetName (target, MacTargetNameType.Build));
						writer.WriteLine ("\t$(Q) $(MAKE) {0}", MakeMacClassicTargetName (target, MacTargetNameType.Exec));
						writer.WriteLine ();
					}
					else {
						allTargetNames.Add (MakeMacUnifiedTargetName (target, MacTargetNameType.Build));
						allTargetCleanNames.Add (MakeMacUnifiedTargetName (target, MacTargetNameType.Clean));

						string guiUnitDependency = ((MacUnifiedTarget)target).Mobile ? "$(GUI_UNIT_PATH)/bin/xammac_mobile/GuiUnit.exe" : "$(GUI_UNIT_PATH)/bin/net_4_5/GuiUnit.exe";

						writer.WriteTarget (MakeMacUnifiedTargetName (target, MacTargetNameType.Build), "{0}", target.ProjectPath.Replace (" ", "\\ ") + " "  + guiUnitDependency);
						writer.WriteLine ("\t$(Q_XBUILD) $(SYSTEM_XBUILD) \"/property:Configuration=$(CONFIG)\" /t:Build $(XBUILD_VERBOSITY) \"{0}\"", target.ProjectPath);
						writer.WriteLine ();

						writer.WriteTarget (MakeMacUnifiedTargetName (target, MacTargetNameType.Clean), "");
						writer.WriteLine ("\t$(Q_XBUILD) $(SYSTEM_XBUILD) \"/property:Configuration=$(CONFIG)\" /t:Clean $(XBUILD_VERBOSITY) \"{0}\"", target.ProjectPath);
						writer.WriteLine ();

						writer.WriteTarget (MakeMacUnifiedTargetName (target, MacTargetNameType.Exec), "");
						writer.WriteLine ("\t$(Q) ./{0}/bin/x86/Debug{1}/{0}.app/Contents/MacOS/{0}", make_escaped_name, target.Suffix);
						writer.WriteLine ();

						writer.WriteTarget (MakeMacUnifiedTargetName (target, MacTargetNameType.Run), "");
						writer.WriteLine ("\t$(Q) $(MAKE) {0}", MakeMacUnifiedTargetName (target, MacTargetNameType.Build));
						writer.WriteLine ("\t$(Q) $(MAKE) {0}", MakeMacUnifiedTargetName (target, MacTargetNameType.Exec));
						writer.WriteLine ();
					}

					writer.WriteLine ();
				}
				writer.WriteLine ("# Env Variables to use local not system XM");
				writer.WriteLine ();

				var enviromentalVariables = new Dictionary<string,string> () { { "XBUILD_FRAMEWORK_FOLDERS_PATH", "$(MAC_DESTDIR)/Library/Frameworks/Mono.framework/External/xbuild-frameworks"},
					{ "MSBuildExtensionsPath", "$(MAC_DESTDIR)/Library/Frameworks/Mono.framework/External/xbuild"},
					{ "MD_APPLE_SDK_ROOT", "$(shell dirname `dirname $(XCODE_DEVELOPER_ROOT)`)"}
				};

				foreach (var key in enviromentalVariables) {
					writer.WriteLine ("{0}: export {1}:={2}", string.Join (" ", allTargetNames.ToArray ()), key.Key, key.Value);
					writer.WriteLine ("{0}: export {1}:={2}", string.Join (" ", allTargetCleanNames.ToArray ()), key.Key, key.Value);
				}

				writer.WriteLine ("# Container targets that run multiple test projects");
				writer.WriteLine ();

				var grouped = allTargets.GroupBy ((target) => target.SimplifiedName);
				foreach (MacTargetNameType action in Enum.GetValues (typeof (MacTargetNameType))) {
					var actionName = action.ToString ().ToLowerInvariant ();
					foreach (var group in grouped) {
						var targetName = group.Key;
						writer.WriteLine ("{0}-mac-{1}:", actionName, targetName);
						writer.WriteLine ("\t$(Q) rm -f \".$@-failure.stamp\"");
						foreach (var entry in group)
							writer.WriteLine ("\t$(Q) $(MAKE) {0} || echo \"{0} failed\" >> \".$@-failure.stamp\"", MakeMacTargetName (entry, action));
						writer.WriteLine ("\t$(Q) if test -e \".$@-failure.stamp\"; then cat \".$@-failure.stamp\"; rm \".$@-failure.stamp\"; exit 1; fi");
						writer.WriteLine ();
					}
				}

				writer.WriteLine ("mac-run run-mac:");
				writer.WriteLine ("\t$(Q) rm -rf \".$@-failure.stamp\"");
				foreach (var target in allTargets) {
					if (target is MacClassicTarget)
						writer.WriteLine ("\t$(Q) $(MAKE) \"{0}\" || echo \"{0} failed\" >> \".$@-failure.stamp\"", MakeMacClassicTargetName (target, MacTargetNameType.Run));
					else if (target is MacUnifiedTarget)
						writer.WriteLine ("\t$(Q) $(MAKE) \"{0}\" || echo \"{0} failed\" >> \".$@-failure.stamp\"", MakeMacUnifiedTargetName (target, MacTargetNameType.Run));
					else
						throw new NotImplementedException ();					
				}

				writer.WriteLine ("\t$(Q) if test -e \".$@-failure.stamp\"; then cat \".$@-failure.stamp\"; rm \".$@-failure.stamp\"; exit 1; fi");
				writer.WriteLine ();

				writer.WriteLine ("mac-build mac-build-all build-mac:"); // build everything
				writer.WriteLine ("\t$(Q) rm -rf \".$@-failure.stamp\"");
				foreach (var target in allTargets) {
					if (target is MacClassicTarget)
						writer.WriteLine ("\t$(Q) $(MAKE) \"{0}\" || echo \"{0} failed\" >> \".$@-failure.stamp\"", MakeMacClassicTargetName (target, MacTargetNameType.Build));
					else if (target is MacUnifiedTarget)
						writer.WriteLine ("\t$(Q) $(MAKE) \"{0}\" || echo \"{0} failed\" >> \".$@-failure.stamp\"", MakeMacUnifiedTargetName (target, MacTargetNameType.Build));
					else
						throw new NotImplementedException ();

				}
				writer.WriteLine ("\t$(Q) if test -e \".$@-failure.stamp\"; then cat \".$@-failure.stamp\"; rm \".$@-failure.stamp\"; exit 1; fi");
			}
		}

		static void WriteCollectionTarget (StreamWriter writer, string target, IEnumerable<Target> targets, string mode)
		{
			writer.WriteLine ();
			writer.Write (target);
			writer.WriteLine (":");
			foreach (var t in targets.Where ((v) => v.IsExe)) {
				if (t is ClassicTarget) {
					writer.WriteLine ("\t$(Q) $(MAKE) \"run{0}-{2}compat-{1}\" || echo \"run{0}-{2}compat-{1} failed\" >> \".$@-failure.stamp\"", "-ios", t.Name, mode);
				} else if (t is UnifiedTarget) {
					writer.WriteLine ("\t$(Q) $(MAKE) \"run{0}-{2}unified-{1}\" || echo \"run{0}-{2}unified-{1} failed\" >> \".$@-failure.stamp\"", "-ios", t.Name, mode);
				} else {
					writer.WriteLine ("\t$(Q) $(MAKE) \"run{0}-{2}-{1}\" || echo \"run{0}-{2}-{1} failed\" >> \".$@-failure.stamp\"", t.Suffix, t.Name, mode);
				}
			}
			writer.WriteLine ("\t$(Q) if test -e \".$@-failure.stamp\"; then cat \".$@-failure.stamp\"; rm \".$@-failure.stamp\"; exit 1; fi");
		}

		public static void CreateMakefile (Harness harness, IEnumerable<ClassicTarget> classic_targets, IEnumerable<UnifiedTarget> unified_targets, IEnumerable<TVOSTarget> tvos_targets, IEnumerable<WatchOSTarget> watchos_targets)
		{
			var makefile = Path.Combine (harness.RootDirectory, "Makefile.inc");
			using (var writer = new StreamWriter (makefile, false, new UTF8Encoding (false))) {
				writer.WriteLine (".stamp-configure-projects: Makefile xharness/xharness.exe");
				writer.WriteLine ("\t$(Q) $(SYSTEM_MONO) --debug xharness/xharness.exe $(XHARNESS_VERBOSITY) --configure --autoconf --rootdir $(CURDIR)");
				writer.WriteLine ("\t$(Q) touch $@");
				writer.WriteLine ();

				var allTargets = new List<Target> ();
				allTargets.AddRange (classic_targets);
				allTargets.AddRange (unified_targets);
				allTargets.AddRange (tvos_targets);
				allTargets.AddRange (watchos_targets);

				// build/[install/]run targets for specific test projects.
				foreach (var target in allTargets) {
					var make_escaped_suffix = "-" + target.Platform.Replace (" ", "\\ ");
					var make_escaped_name = target.Name.Replace (" ", "\\ ");

					writer.WriteLine ();
					if (target.ProjectPath != target.TemplateProjectPath) {
						writer.WriteLine ("# {0} for {1}", target.Name, target.Suffix.Replace ("-", ""));
						writer.WriteLine (".stamp-configure-projects: {0}", target.TemplateProjectPath.Replace (" ", "\\ "));
						writer.WriteLine ("{0}: .stamp-configure-projects", target.ProjectPath.Replace (" ", "\\ "), target.TemplateProjectPath.Replace (" ", "\\ "));
						writer.WriteLine ();
					}

					// build sim project target
					if (target.IsBCLProject) {
						writer.WriteTarget ("build{0}-bcl-sim{2}-{1}", "build{0}-sim{2}-{1}", make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
						writer.WriteLine ("\t$(Q) echo Build succeeded"); // This is important, otherwise we'll end up executing the catch-all build-% target
						writer.WriteLine ();
					}
					writer.WriteTarget ("build{0}-sim{3}-{1}", "{2}", make_escaped_suffix, make_escaped_name, target.ProjectPath.Replace (" ", "\\ "), target.MakefileWhereSuffix);
					writer.WriteLine ("\t$(Q_XBUILD) $(SYSTEM_XBUILD) \"/property:Configuration=$(CONFIG)\" \"/property:Platform=iPhoneSimulator\" /t:Build $(XBUILD_VERBOSITY) \"{0}\"", target.ProjectPath);
					writer.WriteLine ();

					if (target is ClassicTarget) {
						// target that builds both Classic+Unified
						writer.WriteTarget ("build{0}-sim-{1}", "build{0}-simclassic-{1} build{0}-simunified-{1}", make_escaped_suffix, make_escaped_name);
						writer.WriteLine ("\t$(Q) echo Build succeeded"); // This is important, otherwise we'll end up executing the catch-all build-% target
						writer.WriteLine ();
					}

					// clean sim project target
					if (target.IsBCLProject) {
						writer.WriteTarget ("clean{0}-bcl-sim{2}-{1}", "clean{0}-sim-{1}", make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
						writer.WriteLine ("\t$(Q) echo Clean succeeded"); // This is important, otherwise we'll end up executing the catch-all clean-% target
						writer.WriteLine ();
					}
					writer.WriteTarget ("clean{0}-sim{2}-{1}", string.Empty, make_escaped_suffix, make_escaped_name, target.ProjectPath.Replace (" ", "\\ "), target.MakefileWhereSuffix);
					writer.WriteLine ("\t$(Q_XBUILD) $(SYSTEM_XBUILD) \"/property:Configuration=$(CONFIG)\" \"/property:Platform=iPhoneSimulator\" /t:Clean $(XBUILD_VERBOSITY) \"{0}\"", target.ProjectPath);
					writer.WriteLine ();

					if (target.IsExe) {
						// run sim project target
						if (target.IsMultiArchitecture) {
							writer.WriteTarget ("run{0}-sim{2}-{1}", string.Empty, make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
							writer.WriteLine ("\t$(Q) rm -f \".$@-failure.stamp\"");
							writer.WriteLine ("\t$(Q) $(MAKE) build{0}-sim{2}-{1}", make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
							writer.WriteLine ("\t$(Q) $(MAKE) exec{0}-sim32-{1} || echo \"exec{0}-sim32-{1} failed\" >> \".$@-failure.stamp\"", make_escaped_suffix, make_escaped_name);
							writer.WriteLine ("\t$(Q) $(MAKE) exec{0}-sim64-{1} || echo \"exec{0}-sim64-{1} failed\" >> \".$@-failure.stamp\"", make_escaped_suffix, make_escaped_name);
							writer.WriteLine ("\t$(Q) if test -e \".$@-failure.stamp\"; then cat \".$@-failure.stamp\"; rm \".$@-failure.stamp\"; exit 1; fi");
							writer.WriteLine ();

							writer.WriteTarget ("run{0}-sim32-{1}", string.Empty, make_escaped_suffix, make_escaped_name);
							writer.WriteLine ("\t$(Q) $(MAKE) build{0}-sim{2}-{1}", make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
							writer.WriteLine ("\t$(Q) $(MAKE) exec{0}-sim32-{1}", make_escaped_suffix, make_escaped_name);
							writer.WriteLine ();

							writer.WriteTarget ("run{0}-sim64-{1}", string.Empty, make_escaped_suffix, make_escaped_name);
							writer.WriteLine ("\t$(Q) $(MAKE) build{0}-sim{2}-{1}", make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
							writer.WriteLine ("\t$(Q) $(MAKE) exec{0}-sim64-{1}", make_escaped_suffix, make_escaped_name);
							writer.WriteLine ();
						} else {
							writer.WriteTarget ("run{0}-sim{2}-{1}", string.Empty, make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
							writer.WriteLine ("\t$(Q) $(MAKE) build{0}-sim{2}-{1}", make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
							writer.WriteLine ("\t$(Q) $(MAKE) exec{0}-sim{2}-{1}", make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
							writer.WriteLine ();
						}

						if (target.IsBCLProject) {
							writer.WriteTarget ("run{0}-bcl-sim{2}-{1}", "run{0}-sim{2}-{1}", make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
							writer.WriteLine ("\t$(Q) echo Run succeeded"); // This is important, otherwise we'll end up executing the catch-all run-% target
							writer.WriteLine ();

							if (target.IsMultiArchitecture) {
								writer.WriteTarget ("run{0}-bcl-sim32-{1}", "run{0}-sim32-{1}", make_escaped_suffix, make_escaped_name);
								writer.WriteLine ("\t$(Q) echo Run succeeded"); // This is important, otherwise we'll end up executing the catch-all run-% target
								writer.WriteLine ();

								writer.WriteTarget ("run{0}-bcl-sim64-{1}", "run{0}-sim64-{1}", make_escaped_suffix, make_escaped_name);
								writer.WriteLine ("\t$(Q) echo Run succeeded"); // This is important, otherwise we'll end up executing the catch-all run-% target
								writer.WriteLine ();
							}
						}

						if (target is ClassicTarget) {
							// target that runs both Classic+Unified
							writer.WriteTarget ("run{0}-sim-{1}", string.Empty, make_escaped_suffix, make_escaped_name);
							writer.WriteLine ("\t$(Q) rm -f \".$@-failure.stamp\"");
							writer.WriteLine ("\t$(Q) $(MAKE) build{0}-simclassic-{1} build{0}-simunified-{1}", make_escaped_suffix, make_escaped_name);
							writer.WriteLine ("\t$(Q) $(MAKE) exec{0}-simclassic-{1} || echo \"exec{0}-simclassic-{1} failed\" >> \".$@-failure.stamp\"", make_escaped_suffix, make_escaped_name);
							writer.WriteLine ("\t$(Q) $(MAKE) exec{0}-sim32-{1} || echo \"exec{0}-sim32-{1} failed\" >> \".$@-failure.stamp\"", make_escaped_suffix, make_escaped_name);
							writer.WriteLine ("\t$(Q) $(MAKE) exec{0}-sim64-{1} || echo \"exec{0}-sim64-{1} failed\" >> \".$@-failure.stamp\"", make_escaped_suffix, make_escaped_name);
							writer.WriteLine ("\t$(Q) if test -e \".$@-failure.stamp\"; then cat \".$@-failure.stamp\"; rm \".$@-failure.stamp\"; exit 1; fi");
							writer.WriteLine ();

							// compat targets for finger memory
							writer.WriteTarget ("run-simcompat-{1}", "run-ios-simclassic-{1}", make_escaped_suffix, make_escaped_name);
							writer.WriteLine ("\t$(Q) echo Run succeeded"); // This is important, otherwise we'll end up executing the catch-all run-% target
							writer.WriteLine ();

							writer.WriteTarget ("run-simdual-{1}", "run-ios-simunified-{1}", make_escaped_suffix, make_escaped_name);
							writer.WriteLine ("\t$(Q) echo Run succeeded"); // This is important, otherwise we'll end up executing the catch-all run-% target
							writer.WriteLine ();
						}

						// exec sim project target
						if (target.IsMultiArchitecture) {
							writer.WriteTarget ("exec{0}-sim64-{1}", "$(UNIT_SERVER)", make_escaped_suffix, make_escaped_name);
							writer.WriteLine ("\t$(Q) $(SYSTEM_MONO) --debug xharness/xharness.exe $(XHARNESS_VERBOSITY) --run \"{0}\" --target {1}-simulator-64 --sdkroot $(XCODE_DEVELOPER_ROOT) --logfile \"$@.log\" --configuration $(CONFIG)", target.ProjectPath, target.Platform); 
							writer.WriteLine ();

							writer.WriteTarget ("exec{0}-sim32-{1}", "$(UNIT_SERVER)", make_escaped_suffix, make_escaped_name);
							writer.WriteLine ("\t$(Q) $(SYSTEM_MONO) --debug xharness/xharness.exe $(XHARNESS_VERBOSITY) --run \"{0}\" --target {1}-simulator-32 --sdkroot $(XCODE_DEVELOPER_ROOT) --logfile \"$@.log\" --configuration $(CONFIG)", target.ProjectPath, target.Platform); 
							writer.WriteLine ();
						} else {
							writer.WriteTarget ("exec{0}-sim{2}-{1}", "$(UNIT_SERVER)", make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
							writer.WriteLine ("\t$(Q) $(SYSTEM_MONO) --debug xharness/xharness.exe $(XHARNESS_VERBOSITY) --run \"{0}\" --target {1}-simulator --sdkroot $(XCODE_DEVELOPER_ROOT) --logfile \"$@.log\" --configuration $(CONFIG)", target.ProjectPath, target.Platform); 
							writer.WriteLine ();
						}

						if (target.IsBCLProject) {
							writer.WriteTarget ("exec{0}-bcl-sim{2}-{1}", "exec{0}-sim-{1}", make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
							writer.WriteLine ("\t$(Q) echo Exec succeeded"); // This is important, otherwise we'll end up executing the catch-all exec-% target
							writer.WriteLine ();

							if (target.IsMultiArchitecture) {
								writer.WriteTarget ("exec{0}-bcl-sim32-{1}", "exec{0}-sim32-{1}", make_escaped_suffix, make_escaped_name);
								writer.WriteLine ("\t$(Q) echo Exec succeeded"); // This is important, otherwise we'll end up executing the catch-all exec-% target
								writer.WriteLine ();

								writer.WriteTarget ("exec{0}-bcl-sim64-{1}", "exec{0}-sim64-{1}", make_escaped_suffix, make_escaped_name);
								writer.WriteLine ("\t$(Q) echo Exec succeeded"); // This is important, otherwise we'll end up executing the catch-all exec-% target
								writer.WriteLine ();
							}
						}
					}

					// build dev project target
					if (target.IsBCLProject) {
						writer.WriteTarget ("build{0}-bcl-dev{2}-{1}", "build{0}-dev-{1}", make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
						writer.WriteLine ("\t$(Q) echo Build succeeded"); // This is important, otherwise we'll end up executing the catch-all clean-% target
						writer.WriteLine ();
					}
					writer.WriteTarget ("build{0}-dev{3}-{1}", "{2} xharness/xharness.exe", make_escaped_suffix, make_escaped_name, target.ProjectPath.Replace (" ", "\\ "), target.MakefileWhereSuffix);
					writer.WriteLine ("\t$(Q_XBUILD) $(SYSTEM_XBUILD) \"/property:Configuration=$(CONFIG)\" \"/property:Platform=iPhone\" /t:Build $(XBUILD_VERBOSITY) \"{0}\"", target.ProjectPath);
					writer.WriteLine ();

					if (target is ClassicTarget) {
						// target that builds both Classic+Unified
						writer.WriteTarget ("build{0}-dev-{1}", "build{0}-devclassic-{1} build{0}-devunified-{1}", make_escaped_suffix, make_escaped_name);
						writer.WriteLine ("\t$(Q) echo Build succeeded"); // This is important, otherwise we'll end up executing the catch-all build-% target
						writer.WriteLine ();
					}

					// clean dev project target
					if (target.IsBCLProject) {
						writer.WriteTarget ("clean{0}-bcl-dev{2}-{1}", "clean{0}-dev-{1}", make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
						writer.WriteLine ("\t$(Q) echo Clean succeeded"); // This is important, otherwise we'll end up executing the catch-all clean-% target
						writer.WriteLine ();
					}
					writer.WriteTarget ("clean{0}-dev{2}-{1}", string.Empty, make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
					writer.WriteLine ("\t$(Q_XBUILD) $(SYSTEM_XBUILD) \"/property:Configuration=$(CONFIG)\" \"/property:Platform=iPhone\" /t:Clean $(XBUILD_VERBOSITY) \"{0}\"", target.ProjectPath);
					writer.WriteLine ();

					if (target.IsExe) {
						// install dev project target
						if (target.IsBCLProject) {
							writer.WriteTarget ("install{0}-bcl-dev{2}-{1}", "install{0}-dev-{1}", make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
							writer.WriteLine ("\t$(Q) echo Install succeeded"); // This is important, otherwise we'll end up executing the catch-all install-% target
							writer.WriteLine ();
						}
						writer.WriteTarget ("install{0}-dev{2}-{1}", "xharness/xharness.exe", make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
						writer.WriteLine ("\t$(Q) $(SYSTEM_MONO) --debug xharness/xharness.exe $(XHARNESS_VERBOSITY) --install \"{0}\" --target {1}-device --sdkroot $(XCODE_DEVELOPER_ROOT) --configuration $(CONFIG)", target.ProjectPath, target.Platform);
						writer.WriteLine ();

						// run dev project target
						if (target.IsBCLProject) {
							writer.WriteTarget ("run{0}-bcl-dev{2}-{1}", "run{0}-dev-{1}", make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
							writer.WriteLine ("\t$(Q) echo Run succeeded"); // This is important, otherwise we'll end up executing the catch-all run-% target
							writer.WriteLine ();
						}
						writer.WriteTarget ("run{0}-dev{2}-{1}", "xharness/xharness.exe", make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
						writer.WriteLine ("\t$(Q) $(MAKE) build{0}-dev{2}-{1}", make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
						writer.WriteLine ("\t$(Q) $(MAKE) install{0}-dev{2}-{1}", make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
						writer.WriteLine ("\t$(Q) $(MAKE) exec{0}-dev{2}-{1}", make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
						writer.WriteLine ();

						// exec dev project target
						if (target.IsBCLProject) {
							writer.WriteTarget ("exec{0}-bcl-dev{2}-{1}", "exec{0}-dev{2}-{1}", make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
							writer.WriteLine ("\t$(Q) echo Exec succeeded"); // This is important, otherwise we'll end up executing the catch-all run-% target
							writer.WriteLine ();
						}
						writer.WriteTarget ("exec{0}-dev{2}-{1}", "xharness/xharness.exe", make_escaped_suffix, make_escaped_name, target.MakefileWhereSuffix);
						writer.WriteLine ("\t$(Q) rm -f \"$@.log\"");
						writer.WriteLine ("\t$(Q) $(SYSTEM_MONO) --debug xharness/xharness.exe $(XHARNESS_VERBOSITY) --run \"{0}\" --target {1}-device --sdkroot $(XCODE_DEVELOPER_ROOT) --logfile \"$@.log\" --configuration $(CONFIG)", target.ProjectPath, target.Platform);
						writer.WriteLine ();

						if (target is ClassicTarget) {
							// target that runs both Classic+Unified
							writer.WriteTarget ("run{0}-dev-{1}", string.Empty, make_escaped_suffix, make_escaped_name);
							writer.WriteLine ("\t$(Q) rm -f \".$@-failure.stamp\"");
							writer.WriteLine ("\t$(Q) $(MAKE) build{0}-devclassic-{1} build{0}-devunified-{1}", make_escaped_suffix, make_escaped_name);
							writer.WriteLine ("\t$(Q) $(MAKE) install{0}-devclassic-{1}", make_escaped_suffix, make_escaped_name);
							writer.WriteLine ("\t$(Q) $(MAKE) exec{0}-devclassic-{1} || echo \"exec{0}-devclassic-{1} failed\" >> \".$@-failure.stamp\"", make_escaped_suffix, make_escaped_name);
							writer.WriteLine ("\t$(Q) $(MAKE) install{0}-devunified-{1}", make_escaped_suffix, make_escaped_name);
							writer.WriteLine ("\t$(Q) $(MAKE) exec{0}-devunified-{1} || echo \"exec{0}-devunified-{1} failed\" >> \".$@-failure.stamp\"", make_escaped_suffix, make_escaped_name);
							writer.WriteLine ("\t$(Q) if test -e \".$@-failure.stamp\"; then cat \".$@-failure.stamp\"; rm \".$@-failure.stamp\"; exit 1; fi");
							writer.WriteLine ();

							// compat targets for finger memory
							writer.WriteTarget ("run-devcompat-{1}", "run-ios-devclassic-{1}", make_escaped_suffix, make_escaped_name);
							writer.WriteLine ("\t$(Q) echo Run succeeded"); // This is important, otherwise we'll end up executing the catch-all run-% target
							writer.WriteLine ();

							writer.WriteTarget ("run-devdual-{1}", "run-ios-devunified-{1}", make_escaped_suffix, make_escaped_name);
							writer.WriteLine ("\t$(Q) echo Run succeeded"); // This is important, otherwise we'll end up executing the catch-all run-% target
							writer.WriteLine ();
						}
					}

					// targets that does both sim and device
					if (target.IsExe && !(target is UnifiedTarget) /* exclude Unified so that we don't end up duplicating these targets */) {
						// build target
						writer.WriteTarget ("build{0}-{1}", "build{0}-dev-{1} build{0}-sim-{1}", make_escaped_suffix, make_escaped_name);
						writer.WriteLine ("\t$(Q) echo Build succeeded"); // This is important, otherwise we'll end up executing the catch-all build-% target
						writer.WriteLine ();
						// run target
						writer.WriteTarget ("run{0}-{1}", "run{0}-dev-{1} run{0}-sim-{1}", make_escaped_suffix, make_escaped_name);
						writer.WriteLine ("\t$(Q) echo Run succeeded"); // This is important, otherwise we'll end up executing the catch-all run-% target
						writer.WriteLine ();
					}
				}

				writer.WriteLine ("# Container targets that run multiple test projects");
				writer.WriteLine ();
				writer.WriteLine ("run-local:"); // run every single test we have everywhere
				writer.WriteLine ("\t$(Q) rm -rf \".$@-failure.stamp\"");
				foreach (var target in allTargets) {
					if (!target.IsExe)
						continue;

					if (target is ClassicTarget) {
						writer.WriteLine ("\t$(Q) $(MAKE) \"run{0}-simclassic-{1}\" || echo \"run{0}-simclassic-{1} failed\" >> \".$@-failure.stamp\"", "-ios", target.Name);
						writer.WriteLine ("\t$(Q) $(MAKE) \"run{0}-devclassic-{1}\" || echo \"run{0}-devclassic-{1} failed\" >> \".$@-failure.stamp\"", "-ios", target.Name);
					} else if (target is UnifiedTarget) {
						writer.WriteLine ("\t$(Q) $(MAKE) \"run{0}-simunified-{1}\" || echo \"run{0}-simunified-{1} failed\" >> \".$@-failure.stamp\"", "-ios", target.Name);
						writer.WriteLine ("\t$(Q) $(MAKE) \"run{0}-devunified-{1}\" || echo \"run{0}-devunified-{1} failed\" >> \".$@-failure.stamp\"", "-ios", target.Name);
					} else {
						writer.WriteLine ("\t$(Q) $(MAKE) \"run{0}-sim-{1}\" || echo \"run{0}-sim-{1} failed\" >> \".$@-failure.stamp\"", target.Suffix, target.Name);
						writer.WriteLine ("\t$(Q) $(MAKE) \"run{0}-dev-{1}\" || echo \"run{0}-dev-{1} failed\" >> \".$@-failure.stamp\"", target.Suffix, target.Name);
					}
				}
				writer.WriteLine ("\t$(Q) if test -e \".$@-failure.stamp\"; then cat \".$@-failure.stamp\"; rm \".$@-failure.stamp\"; exit 1; fi");

				writer.WriteLine ();
				writer.WriteLine ("run-sim run-all-sim:"); // run every single test we have in the simulator
				writer.WriteLine ("\t$(Q) rm -rf \".$@-failure.stamp\"");
				foreach (var target in allTargets) {
					if (!target.IsExe)
						continue;

					if (target is ClassicTarget) {
						writer.WriteLine ("\t$(Q) $(MAKE) \"run{0}-simcompat-{1}\" || echo \"run{0}-simcompat-{1} failed\" >> \".$@-failure.stamp\"", "-ios", target.Name);
					} else if (target is UnifiedTarget) {
						writer.WriteLine ("\t$(Q) $(MAKE) \"run{0}-simunified-{1}\" || echo \"run{0}-simunified-{1} failed\" >> \".$@-failure.stamp\"", "-ios", target.Name);
					} else {
						writer.WriteLine ("\t$(Q) $(MAKE) \"run{0}-sim-{1}\" || echo \"run{0}-sim-{1} failed\" >> \".$@-failure.stamp\"", target.Suffix, target.Name);
					}
				}
				writer.WriteLine ("\t$(Q) if test -e \".$@-failure.stamp\"; then cat \".$@-failure.stamp\"; rm \".$@-failure.stamp\"; exit 1; fi");

				writer.WriteLine ();
				writer.WriteLine ("run-dev run-all-dev:"); // run every single test we have on device
				writer.WriteLine ("\t$(Q) rm -rf \".$@-failure.stamp\"");
				foreach (var target in allTargets) {
					if (!target.IsExe)
						continue;

					if (target is ClassicTarget) {
						writer.WriteLine ("\t$(Q) $(MAKE) \"run{0}-devcompat-{1}\" || echo \"run{0}-devcompat-{1} failed\" >> \".$@-failure.stamp\"", "-ios", target.Name);
					} else if (target is UnifiedTarget) {
						writer.WriteLine ("\t$(Q) $(MAKE) \"run{0}-devunified-{1}\" || echo \"run{0}-devunified-{1} failed\" >> \".$@-failure.stamp\"", "-ios", target.Name);
					} else {
						writer.WriteLine ("\t$(Q) $(MAKE) \"run{0}-dev-{1}\" || echo \"run{0}-dev-{1} failed\" >> \".$@-failure.stamp\"", target.Suffix, target.Name);
					}
				}
				writer.WriteLine ("\t$(Q) if test -e \".$@-failure.stamp\"; then cat \".$@-failure.stamp\"; rm \".$@-failure.stamp\"; exit 1; fi");

				foreach (var mode in new string [] { "sim", "dev" }) {
					WriteCollectionTarget (writer, "run-ios-" + mode, allTargets.Where ((v) => v.IsExe && (v is ClassicTarget || v is UnifiedTarget)), mode);
					WriteCollectionTarget (writer, "run-tvos-" + mode, tvos_targets.Where ((v) => v.IsExe), mode);
					WriteCollectionTarget (writer, "run-watchos-" + mode, watchos_targets.Where ((v) => v.IsExe), mode);
				}

				writer.WriteLine ();
				writer.WriteLine ("build build-all:"); // build everything
				writer.WriteLine ("\t$(Q) rm -rf \".$@-failure.stamp\"");
				foreach (var target in classic_targets) {
					if (!target.IsExe)
						continue;
					
					writer.WriteLine ("\t$(Q) $(MAKE) \"build-sim-{0}\" \"build-dev-{0}\" || echo \"build-{0} failed\" >> \".$@-failure.stamp\"", target.Name);
				}
				writer.WriteLine ("\t$(Q) if test -e \".$@-failure.stamp\"; then cat \".$@-failure.stamp\"; rm \".$@-failure.stamp\"; exit 1; fi");

				// targets that run all platforms
				writer.WriteLine ();
				foreach (var target in classic_targets) {
					var make_escaped_name = target.Name.Replace (" ", "\\ ");

					writer.WriteTarget ("build-sim-{0}", "build-ios-simclassic-{0} build-ios-simunified-{0} build-tvos-sim-{0} build-watchos-sim-{0}", make_escaped_name);
					writer.WriteLine ("\t$(Q) echo Simulator builds succeeded"); // This is important, otherwise we'll end up executing the catch-all build-% target
					writer.WriteLine ();

					writer.WriteTarget ("build-dev-{0}", "build-ios-devclassic-{0} build-ios-devunified-{0} build-tvos-dev-{0} build-watchos-dev-{0}", make_escaped_name);
					writer.WriteLine ("\t$(Q) echo Device builds succeeded"); // This is important, otherwise we'll end up executing the catch-all build-% target
					writer.WriteLine ();

					writer.WriteTarget ("build-{0}", "build-sim-{0} build-dev-{0}", make_escaped_name);
					writer.WriteLine ("\t$(Q) echo Build succeeded"); // This is important, otherwise we'll end up executing the catch-all build-% target
					writer.WriteLine ();

					writer.WriteTarget ("run-sim-{0}", "build-sim-{0}", make_escaped_name);
					writer.WriteLine ("\t$(Q) $(MAKE) \"exec-ios-simclassic-{0}\" || echo \"exec-sim-iosclassic-{0} failed\" >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) $(MAKE) \"exec-ios-sim32-{0}\"      || echo \"exec-ios-sim32-{0} failed\"      >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) $(MAKE) \"exec-ios-sim64-{0}\"      || echo \"exec-ios-sim64-{0} failed\"      >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) $(MAKE) \"exec-tvos-sim-{0}\"       || echo \"exec-tvos-sim-{0} failed\"       >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) $(MAKE) \"exec-watchos-sim-{0}\"    || echo \"exec-watchos-sim-{0} failed\"    >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) if test -e \".$@-failure.stamp\"; then cat \".$@-failure.stamp\"; rm \".$@-failure.stamp\"; exit 1; fi");
					writer.WriteLine ();

					writer.WriteTarget ("run-dev-{0}", "build-dev-{0}", make_escaped_name);
					writer.WriteLine ("\t$(Q) $(MAKE) \"install-ios-devclassic-{0}\" || echo \"install-ios-devclassic-{0} failed\" >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) $(MAKE) \"exec-ios-devclassic-{0}\"    || echo \"exec-ios-devclassic-{0} failed\"    >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) $(MAKE) \"install-ios-devunified-{0}\" || echo \"install-ios-devunified-{0} failed\" >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) $(MAKE) \"exec-ios-devunified-{0}\"    || echo \"exec-ios-devunified-{0} failed\"    >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) $(MAKE) \"install-tvos-dev-{0}\"       || echo \"install-tvos-dev-{0} failed\"       >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) $(MAKE) \"exec-tvos-dev-{0}\"          || echo \"exec-tvos-dev-{0} failed\"          >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) $(MAKE) \"install-watchos-dev-{0}\"    || echo \"install-watchos-dev-{0} failed\"    >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) $(MAKE) \"exec-watchos-dev-{0}\"       || echo \"exec-watchos-dev-{0} failed\"       >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) if test -e \".$@-failure.stamp\"; then cat \".$@-failure.stamp\"; rm \".$@-failure.stamp\"; exit 1; fi");
					writer.WriteLine ();

					writer.WriteTarget ("run-{0}", "build-{0}", make_escaped_name);
					// sim
					writer.WriteLine ("\t$(Q) $(MAKE) \"exec-ios-simclassic-{0}\" || echo \"exec-sim-iosclassic-{0} failed\" >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) $(MAKE) \"exec-ios-sim32-{0}\"      || echo \"exec-ios-sim32-{0} failed\"      >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) $(MAKE) \"exec-ios-sim64-{0}\"      || echo \"exec-ios-sim64-{0} failed\"      >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) $(MAKE) \"exec-tvos-sim-{0}\"       || echo \"exec-tvos-sim-{0} failed\"       >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) $(MAKE) \"exec-watchos-sim-{0}\"    || echo \"exec-watchos-sim-{0} failed\"    >> \".$@-failure.stamp\"", target.Name);
					// dev
					writer.WriteLine ("\t$(Q) $(MAKE) \"install-ios-devclassic-{0}\" || echo \"install-ios-devclassic-{0} failed\" >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) $(MAKE) \"exec-ios-devclassic-{0}\"    || echo \"exec-ios-devclassic-{0} failed\"    >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) $(MAKE) \"install-ios-devunified-{0}\" || echo \"install-ios-devunified-{0} failed\" >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) $(MAKE) \"exec-ios-devunified-{0}\"    || echo \"exec-ios-devunified-{0} failed\"    >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) $(MAKE) \"install-tvos-dev-{0}\"       || echo \"install-tvos-dev-{0} failed\"       >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) $(MAKE) \"exec-tvos-dev-{0}\"          || echo \"exec-tvos-dev-{0} failed\"          >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) $(MAKE) \"install-watchos-dev-{0}\"    || echo \"install-watchos-dev-{0} failed\"    >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) $(MAKE) \"exec-watchos-dev-{0}\"       || echo \"exec-watchos-dev-{0} failed\"       >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) if test -e \".$@-failure.stamp\"; then cat \".$@-failure.stamp\"; rm \".$@-failure.stamp\"; exit 1; fi");
					writer.WriteLine ();

					// Wrench needs a slightly different approach, because we want to run some tests, even if other tests failed to build
					// (the default is to build all, then run all if everything built). The problem is that there's no way (that I've found)
					// to make the build parallelizable and support the wrench mode at the same time.

					writer.WriteLine ("wrenchhelper-{0}:", make_escaped_name);
					writer.WriteLine ("\t$(Q) rm -f \".$@-failure.stamp\" \".$@-ios-simclassic-build-failure.stamp\" \".$@-ios-simunified-build-failure.stamp\" \".$@-tvos-sim-build-failure.stamp\" \".$@-watchos-sim-build-failure.stamp\"");
					writer.WriteLine ("\t$(Q) echo \"@MonkeyWrench: SetSummary:\"");
					// first build (serialized)
					writer.WriteLine ("\t$(Q) $(MAKE) \"build-ios-simclassic-{0}\" || echo \"@MonkeyWrench: AddSummary: <b>classic failed to build</b> <br/>\" >> \".$@-ios-simclassic-build-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) $(MAKE) \"build-ios-simunified-{0}\" || echo \"@MonkeyWrench: AddSummary: <b>unified failed to build</b> <br/>\" >> \".$@-ios-simunified-build-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) $(MAKE) \"build-tvos-sim-{0}\"       || echo \"@MonkeyWrench: AddSummary: <b>tvos failed to build</b> <br/>\"    >> \".$@-tvos-sim-build-failure.stamp\"", target.Name);
					if (harness.INCLUDE_WATCH)
						writer.WriteLine ("\t$(Q) $(MAKE) \"build-watchos-sim-{0}\"    || echo \"@MonkeyWrench: AddSummary: <b>watchos failed to build</b> <br/>\" >> \".$@-watchos-sim-build-failure.stamp\"", target.Name);
					// then run
					writer.WriteLine ("\t$(Q) (test -e \".$@-ios-simclassic-build-failure.stamp\" && cat \".$@-ios-simclassic-build-failure.stamp\" >> \".$@-failure.stamp\") || $(MAKE) \"exec-ios-simclassic-{0}\" || echo \"exec-sim-iosclassic-{0} failed\" >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) (test -e \".$@-ios-simunified-build-failure.stamp\" && cat \".$@-ios-simunified-build-failure.stamp\" >> \".$@-failure.stamp\") || $(MAKE) \"exec-ios-sim32-{0}\"      || echo \"exec-ios-sim32-{0} failed\"      >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q)  test -e \".$@-ios-simunified-build-failure.stamp\"                                                                             || $(MAKE) \"exec-ios-sim64-{0}\"      || echo \"exec-ios-sim64-{0} failed\"      >> \".$@-failure.stamp\"", target.Name);
					writer.WriteLine ("\t$(Q) (test -e \".$@-tvos-sim-build-failure.stamp\"       && cat \".$@-tvos-sim-build-failure.stamp\"       >> \".$@-failure.stamp\") || $(MAKE) \"exec-tvos-sim-{0}\"       || echo \"exec-tvos-sim-{0} failed\"       >> \".$@-failure.stamp\"", target.Name);
					if (harness.INCLUDE_WATCH) {
						if (harness.DisableWatchOSOnWrench) {
							writer.WriteLine ("\t$(Q) (test -e \".$@-watchos-sim-build-failure.stamp\"    && cat \".$@-watchos-sim-build-failure.stamp\"                            ) || $(MAKE) \"exec-watchos-sim-{0}\"    || echo \"exec-watchos-sim-{0} failed\"                            ", target.Name);
						} else {
							writer.WriteLine ("\t$(Q) (test -e \".$@-watchos-sim-build-failure.stamp\"    && cat \".$@-watchos-sim-build-failure.stamp\"    >> \".$@-failure.stamp\") || $(MAKE) \"exec-watchos-sim-{0}\"    || echo \"exec-watchos-sim-{0} failed\"    >> \".$@-failure.stamp\"", target.Name);
						}
					}
					writer.WriteLine ("\t$(Q) if test -e \".$@-failure.stamp\"; then cat \".$@-failure.stamp\"; rm \".$@-failure.stamp\"; exit 1; fi");
					writer.WriteLine ();

				}

				writer.WriteLine ();
			}
		}
	}
}

