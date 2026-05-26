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
	public string Location {
		get {
			if (cache_dir is null) {
				do {
					cache_dir = Path.Combine (Path.GetTempPath (), NAME + ".cache", Path.GetRandomFileName ());
					if (File.Exists (cache_dir) || Directory.Exists (cache_dir))
						continue;
					Directory.CreateDirectory (cache_dir);
					break;
				} while (true);

				cache_dir = Application.GetRealPath (cache_dir);

				temporary_cache = true;
				if (!Directory.Exists (cache_dir))
					Directory.CreateDirectory (cache_dir);
#if DEBUG
				Console.WriteLine ("Cache defaults to {0}", cache_dir);
#endif
			}
			return cache_dir;
		}
		set {
			cache_dir = value;
			if (!Directory.Exists (cache_dir))
				Directory.CreateDirectory (cache_dir);
			cache_dir = Application.GetRealPath (Path.GetFullPath (cache_dir));
		}
	}

	public void Clean ()
	{
#if DEBUG
		Console.WriteLine ("Cache.Clean: {0}", Location);
#endif
		Directory.Delete (Location, true);
		Directory.CreateDirectory (Location);
	}

	public static bool CompareFiles (string a, string b, bool ignore_cache = false)
	{
		if (Driver.Force && !ignore_cache) {
			Driver.Log (6, "Files {0} and {1} are considered different because -f was passed to " + NAME + ".", a, b);
			return false;
		}

		if (!File.Exists (b)) {
			Driver.Log (6, "Files {0} and {1} are considered different because the latter doesn't exist.", a, b);
			return false;
		}

		using (var astream = new FileStream (a, FileMode.Open, FileAccess.Read, FileShare.Read)) {
			using (var bstream = new FileStream (b, FileMode.Open, FileAccess.Read, FileShare.Read)) {
				bool rv;
				Driver.Log (6, "Comparing files {0} and {1}...", a, b);
				rv = CompareStreams (astream, bstream, ignore_cache);
				Driver.Log (6, " > {0}", rv ? "Identical" : "Different");
				return rv;
			}
		}
	}

	public unsafe static bool CompareStreams (Stream astream, Stream bstream, bool ignore_cache = false)
	{
		if (Driver.Force && !ignore_cache) {
			Driver.Log (6, " > streams are considered different because -f was passed to " + NAME + ".");
			return false;
		}

		if (astream.Length != bstream.Length) {
			Driver.Log (6, " > streams are considered different because their lengths do not match.");
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
		var pcache = Path.Combine (Location, name);

		if (!File.Exists (pcache)) {
			Driver.Log (3, "A full rebuild will be performed because the cache is either incomplete or entirely missing.");
			return false;
		} else if (GetArgumentsForCacheData (app) != File.ReadAllText (pcache)) {
			Driver.Log (3, "A full rebuild will be performed because the arguments to " + NAME + " has changed with regards to the cached data.");
			return false;
		}

		// Check if mtouch/mmp has been modified.
		var executable = System.Reflection.Assembly.GetExecutingAssembly ().Location;
		if (!Application.IsUptodate (executable, pcache)) {
			Driver.Log (3, "A full rebuild will be performed because " + NAME + " has been modified.");
			return false;
		}

		return true;
	}

	public bool VerifyCache (Application app)
	{
		if (!IsCacheValid (app)) {
			Clean ();
			return false;
		}

		return true;
	}

	public void ValidateCache (Application app)
	{
		var name = "arguments";
		var pcache = Path.Combine (Location, name);
		File.WriteAllText (pcache, GetArgumentsForCacheData (app));
	}

#if false
	static public void ComputeDependencies (IEnumerable<string> assemblies, MonoTouchResolver resolver)
	{
		// note: Parallel.ForEach (with lock to add on 'digests') turns out (much) slower
		// (linksdk.app with 20 assemblies)
		// likely because it's faster (using commoncrypto) than it seems
		foreach (string a in assemblies) {
			string key = Path.GetFileNameWithoutExtension (a);
			using (Stream fs = File.OpenRead (a)) {
				string digest = ComputeDigest (fs, 140);
				digests.Add (key, digest);
			}
		}
		
		Dictionary<string, HashSet<string>> dependencies = new Dictionary<string, HashSet<string>> ();
		foreach (string a in assemblies) {
			HashSet<string> references;
			AssemblyDefinition ad = resolver.Load (a);
			foreach (AssemblyNameReference ar in ad.MainModule.AssemblyReferences) {
				if (!dependencies.TryGetValue (ar.Name, out references)) {
					references = new HashSet<string> ();
					dependencies.Add (ar.Name, references);
				}
				references.Add (ad.Name.Name);
			}
		}
#if DEBUG
		foreach (var kvp in dependencies) {
			Console.WriteLine ("The following assemblies depends on {0}", kvp.Key);
			foreach (var s in kvp.Value)
				Console.WriteLine ("\t{0}", s);
		}
#endif
		// if a dependency has changed everything that depends on it must be cleaned
		foreach (var kvp in dependencies) {
			string cname = kvp.Key + ".*.cache." + GetDigestForAssembly (kvp.Key) + ".o";
			var files = Directory.GetFiles (Location, cname);
			if (files.Length != 0)
				continue;

			Clean (kvp.Key + "*");
			foreach (var deps in kvp.Value)
				Clean (deps + "*");
		}
	}
#endif
}
