// Copyright 2012 Xamarin Inc. All rights reserved.
//#define DEBUG_COMPARE
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Utils;
using Xamarin.Bundler;

#nullable enable

public class Cache {
#if LEGACY_TOOLS
	const string NAME = "mtouch";
#elif BUNDLER
	const string NAME = "dotnet-linker";
#elif ASSEMBLY_PREPARER
	const string NAME = "assembly-preparer";
#else
#error Wrong defines
#endif

	string? cache_dir;
	bool temporary_cache;
	string [] arguments;

	public Cache (string [] arguments)
	{
		this.arguments = arguments;
	}

	public bool IsCacheTemporary {
		get { return temporary_cache; }
	}

	// see --cache=DIR
	public string GetLocation (IToolLog log)
	{
		if (cache_dir is null) {
			do {
				cache_dir = Path.Combine (Path.GetTempPath (), NAME + ".cache", Path.GetRandomFileName ());
				if (File.Exists (cache_dir) || Directory.Exists (cache_dir))
					continue;
				Directory.CreateDirectory (cache_dir);
				break;
			} while (true);

			cache_dir = Application.GetRealPath (log, cache_dir);

			temporary_cache = true;
			if (!Directory.Exists (cache_dir))
				Directory.CreateDirectory (cache_dir);
#if DEBUG
			log.Log ("Cache defaults to {0}", cache_dir);
#endif
		}
		return cache_dir;
	}

	public void SetLocation (IToolLog log, string value)
	{
		cache_dir = value;
		if (!Directory.Exists (cache_dir))
			Directory.CreateDirectory (cache_dir);
		cache_dir = Application.GetRealPath (log, Path.GetFullPath (cache_dir));
	}

	public void Clean (IToolLog log)
	{
		var location = GetLocation (log);
#if DEBUG
		log.Log ("Cache.Clean: {0}", location);
#endif
		Directory.Delete (location, true);
		Directory.CreateDirectory (location);
	}

	public static bool CompareFiles (IToolLog log, string a, string b)
	{
		if (!File.Exists (b)) {
			log.Log (6, "Files {0} and {1} are considered different because the latter doesn't exist.", a, b);
			return false;
		}

		using (var astream = new FileStream (a, FileMode.Open, FileAccess.Read, FileShare.Read)) {
			using (var bstream = new FileStream (b, FileMode.Open, FileAccess.Read, FileShare.Read)) {
				bool rv;
				log.Log (6, "Comparing files {0} and {1}...", a, b);
				rv = CompareStreams (log, astream, bstream);
				log.Log (6, " > {0}", rv ? "Identical" : "Different");
				return rv;
			}
		}
	}

	public unsafe static bool CompareStreams (IToolLog log, Stream astream, Stream bstream)
	{
		if (astream.Length != bstream.Length) {
			log.Log (6, " > streams are considered different because their lengths do not match.");
			return false;
		}

		return FileUtils.CompareStreams (astream, bstream);
	}

	string GetArgumentsForCacheData (Application app)
	{
		var sb = new StringBuilder ();
		var args = new List<string> (arguments);

		sb.Append ("# Version: ").Append (app.ProductConstants.Version).Append ('.').Append (app.ProductConstants.Revision).AppendLine ();
		sb.Append (Driver.GetFullPath ()).AppendLine (" \\");
		CollectArgumentsForCache (args, 0, sb);
		return sb.ToString ();
	}

	void CollectArgumentsForCache (IList<string> args, int firstArgument, StringBuilder sb)
	{
		for (int i = firstArgument; i < args.Count; i++) {
			var arg = args [i];
			switch (arg) {
			// Remove arguments that don't affect the cache status.
			case "":
			case "/v":
			case "-v":
			case "--v":
			case "/f":
			case "-f":
			case "--f":
			case "/time":
			case "-time":
			case "--time":
				break;
			default:
				if (arg [0] == '@')
					CollectArgumentsForCache (File.ReadAllLines (arg.Substring (1)), 0, sb);

				sb.Append ('\t').Append (StringUtils.Quote (arg)).AppendLine (" \\");
				break;
			}
		}
	}

	public bool IsCacheValid (Application app)
	{
		var name = "arguments";
		var pcache = Path.Combine (GetLocation (app), name);

		if (!File.Exists (pcache)) {
			app.Log (3, "A full rebuild will be performed because the cache is either incomplete or entirely missing.");
			return false;
		} else if (GetArgumentsForCacheData (app) != File.ReadAllText (pcache)) {
			app.Log (3, "A full rebuild will be performed because the arguments to " + NAME + " has changed with regards to the cached data.");
			return false;
		}

		// Check if mtouch/mmp has been modified.
		var executable = System.Reflection.Assembly.GetExecutingAssembly ().Location;
		if (!Application.IsUptodate (app, executable, pcache)) {
			app.Log (3, "A full rebuild will be performed because " + NAME + " has been modified.");
			return false;
		}

		return true;
	}

	public bool VerifyCache (Application app)
	{
		if (!IsCacheValid (app)) {
			Clean (app);
			return false;
		}

		return true;
	}

	public void ValidateCache (Application app)
	{
		var name = "arguments";
		var pcache = Path.Combine (GetLocation (app), name);
		File.WriteAllText (pcache, GetArgumentsForCacheData (app));
	}
}
