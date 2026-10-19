// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DiagnosticDocumentation;

public static class Program {
	public static int Main (string [] args)
	{
		try {
			if (args.Length < 2 || args [0] is not ("check" or "update" or "check-incoming"))
				throw new InvalidDataException ("Usage: diagnostic-docs check|update <macios-root> | check-incoming <macios-root> <docs-mobile-ios-root>");
			var root = Path.GetFullPath (args [1]);
			var incoming = args [0] == "check-incoming";
			if (args.Length != (incoming ? 3 : 2))
				throw new InvalidDataException ("Incorrect number of arguments.");
			var source = incoming ? Path.GetFullPath (args [2]) : Path.Combine (root, "docs");
			var documentation = new Documentation (root, source);
			documentation.Validate ();
			documentation.Generate (args [0] == "update", incoming);
			Console.WriteLine ("Diagnostic documentation is up to date.");
			return 0;
		} catch (Exception ex) when (ex is IOException or InvalidDataException or JsonException or System.Xml.XmlException or ArgumentException) {
			Console.Error.WriteLine (ex.Message);
			return 1;
		}
	}
}

public sealed record Page (string Code, string Message, string Meaning, string Status, string Category, string Path);
public sealed record LegacyEntry (string Code, string [] Headings);
public sealed record LegacyCategory (string Heading, LegacyEntry [] Entries);
public sealed record LegacyIndex (string File, string FrontMatter, string Title, LegacyCategory [] Categories, Dictionary<string, string>? Aliases = null);
public sealed record Allocation (string Resource, string [] Codes);
public sealed record Renumbering (string OldCode, string NewCode, string Reason);

public sealed class Documentation {
	const string publicRoot = "https://learn.microsoft.com/dotnet/ios/messages/";
	static readonly Regex codePattern = new ("^(MT|MM|MX|BI)[0-9]{4}$", RegexOptions.CultureInvariant);
	static readonly Regex includePattern = new (@"\[!INCLUDE\s+[^\]]*\]\(([^)]+)\)", RegexOptions.CultureInvariant);
	static readonly Regex linkPattern = new (@"(?<!!)\[[^\]\n]*\]\(([^)\s]+)(?:\s+""[^""]*"")?\)", RegexOptions.CultureInvariant);
	readonly string root;
	readonly string docs;
	readonly Dictionary<string, Page> pages = new (StringComparer.Ordinal);
	readonly JsonSerializerOptions jsonOptions = new () {
		PropertyNameCaseInsensitive = true,
		RespectNullableAnnotations = true,
		RespectRequiredConstructorParameters = true,
	};

	public Documentation (string root, string docs)
	{
		this.root = root;
		this.docs = docs;
	}

	T ReadJson<T> (string file)
	{
		return JsonSerializer.Deserialize<T> (File.ReadAllText (Path.Combine (root, "docs", "messages", file)), jsonOptions)
			?? throw new InvalidDataException ($"Empty diagnostic manifest: {file}");
	}

	public void Validate ()
	{
		var messages = Path.Combine (docs, "messages");
		if (!Directory.Exists (messages))
			throw new InvalidDataException ($"Diagnostic directory does not exist: {messages}");
		foreach (var path in Directory.GetFiles (messages, "*.md").Order (StringComparer.Ordinal)) {
			var name = Path.GetFileNameWithoutExtension (path);
			if (name is "index" or "README")
				continue;
			var code = name.ToUpperInvariant ();
			if (!codePattern.IsMatch (code) || name != code.ToLowerInvariant ())
				throw new InvalidDataException ($"Invalid diagnostic filename: {path}");
			var text = File.ReadAllText (path);
			var metadata = Metadata (text, path);
			var message = StringValue (metadata, "diagnostic_message", path);
			var meaning = StringValue (metadata, "diagnostic_id", path);
			var status = StringValue (metadata, "diagnostic_status", path);
			var category = StringValue (metadata, "diagnostic_category", path);
			foreach (var field in new [] { "title", "description", "ms.date" })
				StringValue (metadata, field, path);
			if (!metadata.TryGetValue ("f1_keywords", out var keywords) ||
				!(JsonSerializer.Deserialize<string []> (keywords) ?? []).SequenceEqual ([code]))
				throw new InvalidDataException ($"{path}: f1_keywords must contain exactly {code}.");
			if (status is not ("current" or "historical"))
				throw new InvalidDataException ($"{path}: invalid diagnostic_status.");
			var expanded = Expand (path, new HashSet<string> (StringComparer.Ordinal));
			if (!expanded.Contains ("\n## Issue\n", StringComparison.Ordinal) ||
				!expanded.Contains ("\n## Solution\n", StringComparison.Ordinal))
				throw new InvalidDataException ($"{path}: Issue and Solution sections are required (directly or through an include).");
			pages.Add (code, new Page (code, message, meaning, status, category, path));
		}
		if (pages.Count == 0)
			throw new InvalidDataException ("No diagnostic pages found.");

		var allocations = ReadJson<Allocation []> ("allocations.json");
		var expectedResources = ResourceNames ("tools/mtouch/Errors.resx").Concat (ResourceNames ("src/Resources.resx")).ToHashSet (StringComparer.Ordinal);
		var allocatedResources = new HashSet<string> (StringComparer.Ordinal);
		foreach (var allocation in allocations) {
			if (!allocatedResources.Add (allocation.Resource) || !expectedResources.Contains (allocation.Resource))
				throw new InvalidDataException ($"Duplicate or unknown resource allocation: {allocation.Resource}");
			if (allocation.Codes.Length == 0)
				throw new InvalidDataException ($"Resource {allocation.Resource} has no documented public codes.");
			foreach (var code in allocation.Codes) {
				if (!codePattern.IsMatch (code) || code [2..] != allocation.Resource [2..] ||
					(code.StartsWith ("BI", StringComparison.Ordinal) != allocation.Resource.StartsWith ("BI", StringComparison.Ordinal)))
					throw new InvalidDataException ($"Invalid public-code mapping: {allocation.Resource} -> {code}");
				if (!pages.ContainsKey (code))
					throw new InvalidDataException ($"Resource {allocation.Resource} has no documentation for {code}.");
			}
		}
		var missing = expectedResources.Except (allocatedResources).Order (StringComparer.Ordinal).ToArray ();
		if (missing.Length > 0)
			throw new InvalidDataException ($"Undocumented diagnostic resource allocations: {string.Join (", ", missing)}. Add pages and an explicit public-prefix mapping.");

		foreach (var group in pages.Values.Where (v => v.Status == "current").GroupBy (v => v.Code [2..])) {
			if (group.Select (v => v.Meaning).Distinct (StringComparer.Ordinal).Count () > 1)
				throw new InvalidDataException ($"Conflicting current diagnostic number: {string.Join (", ", group.Select (v => v.Code))}.");
		}
		foreach (var renumbering in ReadJson<Renumbering []> ("renumberings.json")) {
			if (!pages.TryGetValue (renumbering.OldCode, out var oldPage) || oldPage.Status != "historical" ||
				!pages.ContainsKey (renumbering.NewCode) || string.IsNullOrWhiteSpace (renumbering.Reason))
				throw new InvalidDataException ($"Invalid historical diagnostic reservation: {renumbering.OldCode} -> {renumbering.NewCode}");
			if (!File.ReadAllText (oldPage.Path).Contains (renumbering.NewCode, StringComparison.Ordinal))
				throw new InvalidDataException ($"{renumbering.OldCode} must explain its replacement {renumbering.NewCode}.");
		}
		foreach (var index in ReadJson<LegacyIndex []> ("legacy-indexes.json")) {
			foreach (var entry in index.Categories.SelectMany (v => v.Entries)) {
				if (!pages.ContainsKey (entry.Code))
					throw new InvalidDataException ($"Historical diagnostic {entry.Code} from {index.File} has been lost.");
			}
		}
		ValidateEmitters ();
	}

	void ValidateEmitters ()
	{
		var call = new Regex (@"(?:ErrorHelper\.(?:CreateError|CreateWarning|Warning|Error)|new BindingException|new ProductException|ReportError|(?<msbuild>\.Log(?:Error|Warning|Message)))\s*\(\s*(?:[A-Za-z_.]+\s*,\s*)?(?<number>\d{1,4})\s*[,)]", RegexOptions.CultureInvariant);
		foreach (var directory in new [] { "src/bgen", "tools/common", "tools/mtouch", "tools/dotnet-linker", "tools/assembly-preparer", "msbuild/Xamarin.MacDev.Tasks" }) {
			var source = Path.Combine (root, directory);
			if (!Directory.Exists (source))
				continue;
			foreach (var path in Directory.EnumerateFiles (source, "*.cs", SearchOption.AllDirectories)) {
				var relative = Path.GetRelativePath (source, path);
				if (relative.Split (Path.DirectorySeparatorChar).Any (v => v is "obj" or "bin") ||
					path.EndsWith (".designer.cs", StringComparison.OrdinalIgnoreCase))
					continue;
				foreach (Match match in call.Matches (File.ReadAllText (path))) {
					var number = int.Parse (match.Groups ["number"].Value, System.Globalization.CultureInfo.InvariantCulture).ToString ("0000", System.Globalization.CultureInfo.InvariantCulture);
					var msbuild = match.Groups ["msbuild"].Success;
					var candidates = directory == "src/bgen" ? new [] { "BI" + number } : msbuild ? new [] { "MT" + number } : new [] { "MT" + number, "MM" + number, "MX" + number };
					if (!candidates.Any (pages.ContainsKey))
						throw new InvalidDataException ($"{path}: undocumented emitted diagnostic {string.Join ("/", candidates)}.");
					if (directory == "src/bgen" && pages.TryGetValue (candidates [0], out var page) && page.Status != "current")
						throw new InvalidDataException ($"{path}: emits historical binding diagnostic {page.Code}; use its current allocation.");
					if (msbuild && pages [candidates [0]].Status != "current")
						throw new InvalidDataException ($"{path}: emitted MSBuild diagnostic {candidates [0]} must have current documentation.");
				}
			}
		}
	}

	IEnumerable<string> ResourceNames (string file)
	{
		var document = XDocument.Load (Path.Combine (root, file));
		return document.Descendants ("data").Select (v => (string?) v.Attribute ("name"))
			.OfType<string> ().Where (v => codePattern.IsMatch (v));
	}

	static Dictionary<string, string> Metadata (string text, string path)
	{
		var lines = text.Replace ("\r\n", "\n").Split ('\n');
		if (lines [0] != "---")
			throw new InvalidDataException ($"{path}: missing YAML front matter.");
		var result = new Dictionary<string, string> (StringComparer.Ordinal);
		for (var i = 1; i < lines.Length; i++) {
			if (lines [i] == "---")
				return result;
			if (lines [i].StartsWith ('#') || string.IsNullOrWhiteSpace (lines [i]))
				continue;
			var colon = lines [i].IndexOf (':');
			if (colon <= 0 || !result.TryAdd (lines [i] [..colon], lines [i] [(colon + 1)..].Trim ()))
				throw new InvalidDataException ($"{path}: invalid or duplicate metadata field: {lines [i]}");
		}
		throw new InvalidDataException ($"{path}: unterminated YAML front matter.");
	}

	static string StringValue (Dictionary<string, string> metadata, string key, string path)
	{
		if (!metadata.TryGetValue (key, out var raw))
			throw new InvalidDataException ($"{path}: missing {key}.");
		var value = JsonSerializer.Deserialize<string> (raw);
		if (string.IsNullOrWhiteSpace (value))
			throw new InvalidDataException ($"{path}: {key} must be a nonempty double-quoted YAML string.");
		return value;
	}

	string Expand (string path, HashSet<string> stack)
	{
		path = Path.GetFullPath (path);
		if (!stack.Add (path))
			throw new InvalidDataException ($"Cyclic diagnostic include: {path}");
		var text = File.ReadAllText (path);
		foreach (Match link in linkPattern.Matches (text))
			CheckLocalLink (path, link.Groups [1].Value);
		var expanded = includePattern.Replace (text, match => {
			var target = Resolve (path, match.Groups [1].Value);
			if (!target.StartsWith (Path.Combine (docs, "includes") + Path.DirectorySeparatorChar, StringComparison.Ordinal))
				throw new InvalidDataException ($"Diagnostic includes must be under docs/includes: {path}: {target}");
			return Expand (target, stack);
		});
		stack.Remove (path);
		return expanded;
	}

	static string Resolve (string path, string link)
	{
		return Path.GetFullPath (Path.Combine (Path.GetDirectoryName (path) ?? ".", Uri.UnescapeDataString (link.Split ('#') [0])));
	}

	static void CheckLocalLink (string path, string link)
	{
		if (link.StartsWith ('#') || Uri.TryCreate (link, UriKind.Absolute, out _))
			return;
		if (link.StartsWith ("~/", StringComparison.Ordinal))
			throw new InvalidDataException ($"{path}: replace legacy site-root link {link} with a canonical URL.");
		var target = Resolve (path, link);
		if (!File.Exists (target))
			throw new InvalidDataException ($"{path}: missing link target: {link}");
	}

	public void Generate (bool update, bool incoming)
	{
		var index = new StringBuilder ();
		index.Append ("---\ntitle: \".NET for Apple platforms errors and warnings reference\"\ndescription: \"Build and runtime diagnostic codes for .NET for iOS, tvOS, macOS and Mac Catalyst.\"\nms.topic: troubleshooting\n---\n\n");
		index.Append ("<!-- Copyright (c) Microsoft Corporation. Licensed under the MIT License. -->\n<!-- Generated by scripts/diagnostic-docs. Edit the individual diagnostic pages, not this index. -->\n\n");
		index.Append ("# .NET for Apple platforms errors and warnings reference\n\n");
		index.Append ("Each code links to its explanation and recovery guidance. Historical diagnostics are retained for older projects.\n\n");
		foreach (var group in pages.Values.OrderBy (v => v.Code, StringComparer.Ordinal).GroupBy (v => v.Category)) {
			index.Append ("## ").Append (group.Key).Append ("\n\n");
			foreach (var page in group)
				index.Append ("- [").Append (page.Code).Append ("](").Append (page.Code.ToLowerInvariant ()).Append (".md): ").Append (OneLine (page.Message)).Append (page.Status == "historical" ? " (historical)" : "").Append ('\n');
			index.Append ('\n');
		}
		Write (Path.Combine (docs, "messages", "index.md"), index.ToString (), update);
		foreach (var legacy in ReadJson<LegacyIndex []> ("legacy-indexes.json")) {
			var output = new StringBuilder (legacy.FrontMatter);
			output.Append ("\n\n<!-- Generated by scripts/diagnostic-docs. Edit docs/messages instead. -->\n\n").Append (legacy.Title).Append ("\n\n");
			output.Append ("Errors and warnings are now documented on individual pages. See the [complete diagnostic reference](").Append (publicRoot).Append (").\n\n");
			var anchors = new HashSet<string> (StringComparer.Ordinal);
			foreach (var alias in legacy.Aliases ?? []) {
				if (!pages.ContainsKey (alias.Value))
					throw new InvalidDataException ($"Historical anchor {alias.Key} has no destination {alias.Value}.");
				if (!Regex.IsMatch (alias.Key, "^[A-Za-z0-9_-]+$", RegexOptions.CultureInvariant))
					throw new InvalidDataException ($"Invalid historical anchor: {alias.Key}");
				if (anchors.Add (alias.Key))
					output.Append ("<a name=\"").Append (alias.Key).Append ("\"></a>\n\n- [").Append (alias.Key).Append ("](").Append (publicRoot).Append (alias.Value.ToLowerInvariant ()).Append (")\n\n");
			}
			var slugCounts = new Dictionary<string, int> (StringComparer.Ordinal);
			foreach (var category in legacy.Categories) {
				output.Append (category.Heading).Append ("\n\n");
				foreach (var entry in category.Entries) {
					var page = pages [entry.Code];
					if (anchors.Add (entry.Code))
						output.Append ("<a name=\"").Append (entry.Code).Append ("\"></a>\n\n");
					foreach (var heading in entry.Headings) {
						// Preserve heading-derived fragments as well as the original explicit code anchors.
						var slug = Slug (heading);
						slugCounts.TryGetValue (slug, out var count);
						slugCounts [slug] = count + 1;
						if (count > 0)
							slug += "-" + count.ToString (System.Globalization.CultureInfo.InvariantCulture);
						if (anchors.Add (slug))
							output.Append ("<a id=\"").Append (slug).Append ("\"></a>\n\n");
					}
					output.Append ("- [").Append (entry.Code).Append ("](").Append (publicRoot).Append (entry.Code.ToLowerInvariant ()).Append ("): ").Append (OneLine (page.Message)).Append ('\n');
				}
				output.Append ('\n');
			}
			if (!incoming)
				Write (Path.Combine (root, "docs", "website", legacy.File), output.ToString (), update);
		}
	}

	static string Slug (string heading)
	{
		var title = Regex.Replace (heading, @"^#+\s*|<[^>]+>", "");
		title = Regex.Replace (title.ToLowerInvariant (), @"[^\p{L}\p{N}\s_-]", "");
		return Regex.Replace (title.Trim (), @"\s", "-");
	}

	static string OneLine (string text)
	{
		return text.Replace ("\r", "").Replace ("\n", " ");
	}

	static void Write (string path, string content, bool update)
	{
		if (update) {
			File.WriteAllText (path, content);
		} else if (!File.Exists (path) || File.ReadAllText (path) != content) {
			throw new InvalidDataException ($"Generated diagnostic index differs: {path}. Run docs/update-diagnostic-docs.sh, or move index-only edits into the individual diagnostic pages.");
		}
	}
}
