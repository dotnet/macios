﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml;

using Xamarin.Tests;

namespace MTouchTests
{
	public enum MTouchAction
	{
		None,
		BuildDev,
		BuildSim,
	}

	enum MTouchLinker
	{
		Unspecified,
		LinkAll,
		LinkSdk,
		DontLink,
	}

	enum MTouchRegistrar
	{
		Unspecified,
		Dynamic,
		Static,
	}

	[Flags]
	enum I18N
	{
		None = 0,

		CJK = 1,
		MidEast = 2,
		Other = 4,
		Rare = 8,
		West = 16,

		All = CJK | MidEast | Other | Rare | West,
		Base
	}

	class MTouchTool : Tool, IDisposable
	{
		public const string None = "None";

#pragma warning disable 649
		// These map directly to mtouch options
		public int Verbosity;
		public string SdkRoot;
		public bool? NoSign;
		public bool? Debug;
		public bool? FastDev;
		public bool? Dlsym;
		public string Sdk;
		public string [] References;
		public string Executable;
		public string TargetFramework;
		public string Abi;
		public string AppPath;
		public string Cache;
		public MTouchLinker Linker;
		public bool? NoFastSim;
		public MTouchRegistrar Registrar;
		public I18N I18N;
#pragma warning restore 649

		// These are a bit smarter
		public MTouch.Profile Profile = MTouch.Profile.Classic;

		static XmlDocument device_list_cache;

		List<string> directories_to_delete;

		public class DeviceInfo
		{
			public string UDID;
			public string Name;
			public string CompanionIdentifier;
			public string DeviceClass;

			public DeviceInfo Companion;
		}

		string GetVerbosity ()
		{
			if (Verbosity == 0) {
				return string.Empty;
			} else if (Verbosity > 0) {
				return new string ('-', Verbosity).Replace ("-", "-v ");
			} else {
				return new string ('-', -Verbosity).Replace ("-", "-q ");
			}
		}

		public int LaunchOnDevice (DeviceInfo device, string appPath, bool waitForUnlock, bool waitForExit)
		{
			return Execute ("--devname \"{0}\" --launchdev \"{1}\" --sdkroot \"{2}\" --wait-for-unlock:{3} --wait-for-exit:{4} {5}", device.Name, appPath, Configuration.xcode_root, waitForUnlock ? "yes" : "no", waitForExit ? "yes" : "no", GetVerbosity ());
		}

		public int InstallOnDevice (DeviceInfo device, string appPath, string devicetype = null)
		{
			return Execute ("--devname \"{0}\" --installdev \"{1}\" --sdkroot \"{2}\" {3} {4}", device.Name, appPath, Configuration.xcode_root, GetVerbosity (), devicetype == null ? string.Empty : "--device " + devicetype);
		}

		public int Execute (MTouchAction action)
		{
			return Execute (BuildArguments (action));
		}

		string BuildArguments (MTouchAction action)
		{
			var sb = new StringBuilder ();
			var isDevice = false;

			switch (action) {
			case MTouchAction.None:
				break;
			case MTouchAction.BuildDev:
				if (AppPath == null)
					throw new Exception ("No AppPath specified.");
				isDevice = true;
				sb.Append (" --dev ").Append (MTouch.Quote (AppPath));
				break;
			case MTouchAction.BuildSim:
				isDevice = false;
				if (AppPath == null)
					throw new Exception ("No AppPath specified.");
				sb.Append (" --sim ").Append (MTouch.Quote (AppPath));
				break;
			default:
				throw new NotImplementedException ();
			}

			if (SdkRoot == None) {
				// do nothing
			} else if (!string.IsNullOrEmpty (SdkRoot)) {
				sb.Append (" --sdkroot ").Append (MTouch.Quote (SdkRoot));
			} else {
				sb.Append (" --sdkroot ").Append (MTouch.Quote (Configuration.xcode_root));
			}

			sb.Append (" ").Append (GetVerbosity ());

			if (!NoSign.HasValue && Profile == MTouch.Profile.Classic) {
				sb.Append (" --nosign");
			} else if (NoSign.HasValue && NoSign.Value && Profile == MTouch.Profile.Classic)
				sb.Append (" --nosign");

			if (Sdk == None) {
				// do nothing	
			} else if (!string.IsNullOrEmpty (Sdk)) {
				sb.Append (" --sdk ").Append (Sdk);
			} else {
				sb.Append (" --sdk ").Append (MTouch.GetSdkVersion (Profile));
			}

			if (Debug.HasValue && Debug.Value)
				sb.Append (" --debug");

			if (FastDev.HasValue && FastDev.Value)
				sb.Append (" --fastdev");

			if (Dlsym.HasValue)
				sb.Append (" --dlsym:").Append (Dlsym.Value ? "true" : "false");

			if (References != null) {
				foreach (var r in References)
					sb.Append (" -r:").Append (MTouch.Quote (r));
			}

			if (!string.IsNullOrEmpty (Executable))
				sb.Append (" ").Append (MTouch.Quote (Executable));

			if (TargetFramework == None) {
				// do nothing
			} else if (!string.IsNullOrEmpty (TargetFramework)) {
				sb.Append (" --target-framework ").Append (TargetFramework);
			} else {
				// make the implicit default the way tests have been running until now, and at the same time the very minimum to make apps build.
				switch (Profile) {
				case MTouch.Profile.Classic:
					break;
				case MTouch.Profile.Unified:
					sb.Append (" -r:").Append (MTouch.Quote (Configuration.XamarinIOSDll));
					break;
				case MTouch.Profile.TVOS:
				case MTouch.Profile.WatchOS:
					sb.Append (" --target-framework ").Append (MTouch.GetTargetFramework (Profile));
					sb.Append (" -r:").Append (MTouch.Quote (MTouch.GetBaseLibrary (Profile)));
					break;
				default:
					throw new NotImplementedException ();
				}
			}

			if (!string.IsNullOrEmpty (Abi)) {
				sb.Append (" --abi ").Append (Abi);
			} else {
				switch (Profile) {
				case MTouch.Profile.Classic:
				case MTouch.Profile.Unified:
					break; // not required
				case MTouch.Profile.TVOS:
					sb.Append (isDevice ? " --abi arm64" : " --abi x86_64");
					break;
				case MTouch.Profile.WatchOS:
					sb.Append (isDevice ? " --abi armv7k" : " --abi i386");
					break;
				default:
					throw new NotImplementedException ();
				}
			}

			switch (Linker) {
			case MTouchLinker.LinkAll:
			case MTouchLinker.Unspecified:
				break;
			case MTouchLinker.DontLink:
				sb.Append (" --nolink");
				break;
			case MTouchLinker.LinkSdk:
				sb.Append (" --linksdkonly");
				break;
			default:
				throw new NotImplementedException ();
			}

			if (NoFastSim.HasValue && NoFastSim.Value)
				sb.Append (" --nofastsim");

			switch (Registrar) {
			case MTouchRegistrar.Unspecified:
				break;
			case MTouchRegistrar.Dynamic:
				sb.Append (" --registrar:static");
				break;
			case MTouchRegistrar.Static:
				sb.Append (" --registrar:static");
				break;
			default:
				throw new NotImplementedException ();
			}

			if (I18N != I18N.None) {
				sb.Append (" --i18n ");
				int count = 0;
				if ((I18N & I18N.CJK) == I18N.CJK)
					sb.Append (count++ == 0 ? string.Empty : ",").Append ("cjk");
				if ((I18N & I18N.MidEast) == I18N.MidEast)
					sb.Append (count++ == 0 ? string.Empty : ",").Append ("mideast");
				if ((I18N & I18N.Other) == I18N.Other)
					sb.Append (count++ == 0 ? string.Empty : ",").Append ("other");
				if ((I18N & I18N.Rare) == I18N.Rare)
					sb.Append (count++ == 0 ? string.Empty : ",").Append ("rare");
				if ((I18N & I18N.West) == I18N.West)
					sb.Append (count++ == 0 ? string.Empty : ",").Append ("west");
			}

			if (!string.IsNullOrEmpty (Cache))
				sb.Append (" --cache ").Append (MTouch.Quote (Cache));

			return sb.ToString ();
		}

		XmlDocument FetchDeviceList (bool allowCache = true)
		{
			if (device_list_cache == null || !allowCache) {
				var output_file = Path.GetTempFileName ();
				try {
					if (Execute ("--listdev:{1} --sdkroot {0} --output-format xml", Configuration.xcode_root, output_file) != 0)
						throw new Exception ("Failed to list devices.");
					device_list_cache = new XmlDocument ();
					device_list_cache.Load (output_file);
				} finally {
					File.Delete (output_file);
				}
			}
			return device_list_cache;
		}

		public List<DeviceInfo> ListDevices ()
		{
			var rv = new List<DeviceInfo> ();

			foreach (XmlNode node in FetchDeviceList ().SelectNodes ("//MTouch/Device")) {
				rv.Add (new DeviceInfo () {
					UDID = node.SelectSingleNode ("UDID")?.InnerText,
					Name = node.SelectSingleNode ("Name")?.InnerText,
					CompanionIdentifier = node.SelectSingleNode ("CompanionIdentifier")?.InnerText,
					DeviceClass = node.SelectSingleNode ("DeviceClass")?.InnerText,
				});
			}

			foreach (var device in rv) {
				if (string.IsNullOrEmpty (device.CompanionIdentifier))
					continue;

				device.Companion = rv.FirstOrDefault ((d) => d.UDID == device.CompanionIdentifier);
			}

			return rv;
		}

		public IEnumerable<DeviceInfo> FindAvailableDevices (string [] deviceClasses)
		{
			return ListDevices ().Where ((info) => deviceClasses.Contains (info.DeviceClass));
		}

		public string NativeExecutablePath {
			get {
				return Path.Combine (AppPath, Path.GetFileNameWithoutExtension (Executable));
			}
		}

		public void CreateTemporaryApp (string code = null)
		{
			var testDir = CreateTemporaryDirectory ();
			var app = Path.Combine (testDir, "testApp.app");
			Directory.CreateDirectory (app);

			AppPath = app;
			Executable = MTouch.CompileTestAppExecutable (testDir, code: code, profile: Profile);
		}

		public string CreateTemporaryDirectory ()
		{
			var tmpDir = MTouch.GetTempDirectory ();
			if (directories_to_delete == null)
				directories_to_delete = new List<string> ();
			directories_to_delete.Add (tmpDir);
			return tmpDir;
		}

		public void CreateTemporaryApp_LinkWith ()
		{
			AppPath = CreateTemporaryAppDirectory ();
			Executable = MTouch.CompileTestAppExecutableLinkWith (Path.GetDirectoryName (AppPath), profile: Profile);
		}

		public string CreateTemporaryAppDirectory ()
		{
			if (AppPath != null)
				throw new Exception ("There already is an App directory");
			
			AppPath = Path.Combine (CreateTemporaryDirectory (), "testApp.app");
			Directory.CreateDirectory (AppPath);

			return AppPath;
		}

		void IDisposable.Dispose ()
		{
			if (directories_to_delete != null) {
				foreach (var dir in directories_to_delete)
					Directory.Delete (dir, true);
			}
		}
	}
}
