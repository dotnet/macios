// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Sharpie.Bind;

// A modulemap parser.
// Docs: https://clang.llvm.org/docs/Modules.html#module-map-language
// This isn't a complete parser, it takes numerous shortcuts (in particular it assumes all input is valid), and it hasn't been verified against the grammar either.
// It's purpose is to find all headers referenced in modulemaps.
public sealed class Map {
	ObjectiveCBinder binder;
	public string FilePath { get; private set; }
	public Module RootModule { get; private set; }

	Map (ObjectiveCBinder binder, string path, Tokens tokens)
	{
		this.binder = binder;
		FilePath = path;
		RootModule = ParseModule (tokens);
		if (tokens.Count > 0) {
			Trace ($"Done with {tokens.Count} tokens left, {tokens.Peek ()} is the first one.");
		} else {
			Trace ($"Done with no tokens left!");
		}

		var rootPath = Path.GetDirectoryName (Path.GetDirectoryName (FilePath))!;
		var headers = RootModule.GetHeaders (rootPath, false, false);
		foreach (var fp in headers) {
			Log ($"    Got header {fp} exists: {File.Exists (fp)}");
		}
	}

	void Log (string message)
	{
		binder.BindingResult.Log (2, message);
	}

	void Trace (string message)
	{
		binder.BindingResult.Log (3, message);
	}

	public static Map? Parse (ObjectiveCBinder binder, string path)
	{
		binder.BindingResult.Log (1, $"Parsing {path}");
		using (var reader = new StreamReader (path)) {
			var tokens = new List<string> ();
			foreach (var token in Tokenize (reader)) {
				binder.BindingResult.Log (3, token);
				tokens.Add (token);
			}

			if (tokens.Count == 0)
				return null;

			return new Map (binder, path, new Tokens (tokens));
		}
	}

	Module ParseModule (Tokens tokens)
	{
		if (tokens.TryAcceptKeyword ("extern")) {
			tokens.AcceptKeyword ("module");
			var moduleId = tokens.AcceptIdentifier ();
			var stringLiteral = tokens.AcceptIdentifier ();
			Trace ($"Found top-level extern module {moduleId} {stringLiteral}");
			return new Module (moduleId);
		}

		var isFramework = tokens.TryAcceptKeyword ("framework");
		var isExplicit = tokens.TryAcceptKeyword ("explicit");
		tokens.AcceptKeyword ("module");
		string name;
		if (tokens.TryAccept ("*")) {
			name = "*";
		} else {
			name = tokens.AcceptIdentifier ();
		}

		var rv = new Module (name) {
			IsFramework = isFramework,
			IsExplicit = isExplicit,
		};

		ParseAttributes (tokens);

		tokens.AcceptOperator ("{");

		Trace ($"Found module {name} (isFramework: {isFramework})");

		while (true) {
			if (tokens.TryAcceptKeyword ("requires")) {
				do {
					var notEqual = tokens.TryAccept ("!");
					var req = tokens.AcceptIdentifier ();
					rv.Requirements.Add ((notEqual ? "!" : "") + req);
				} while (tokens.TryAccept (","));
				Trace ($"Found requirements: {string.Join (";", rv.Requirements)}");
				continue;
			}

			if (tokens.TryAccept ("umbrella")) {
				if (tokens.TryAcceptKeyword ("header")) {
					var path = tokens.AcceptIdentifier ();
					Trace ($"Found umbrella header path: {path}");
					rv.Headers.Add (new Header (path) { IsUmbrella = true });
				} else {
					var identifier = tokens.AcceptIdentifier ();
					rv.UmbrellaDirectories.Add (identifier);
				}
				continue;
			}

			if (tokens.TryAccept ("textual")) {
				tokens.AcceptKeyword ("header");
				var path = tokens.AcceptIdentifier ();
				Trace ($"Found textual header path: {path}");
				rv.Headers.Add (new Header (path) { IsTextual = true });
				continue;
			}

			if (tokens.TryAccept ("exclude")) {
				tokens.AcceptKeyword ("header");
				var path = tokens.AcceptIdentifier ();
				Trace ($"Found exclude header path: {path}");
				rv.Headers.Add (new Header (path) { IsExclude = true });
				continue;
			}

			if (tokens.TryAccept ("header")) {
				var path = tokens.AcceptIdentifier ();
				Trace ($"Found header path: {path}");
				rv.Headers.Add (new Header (path));
				continue;
			}

			if (tokens.TryAccept ("export")) {
				if (tokens.TryAccept ("*")) {
					rv.Exports.Add ("*");
				} else {
					var export = tokens.AcceptIdentifier ();
					rv.Exports.Add (export);
				}
				Trace ($"Found export {rv.Exports.Last ()}");
				continue;
			}

			if (tokens.Peek () == "module" || tokens.Peek () == "framework" || tokens.Peek () == "explicit") {
				rv.Modules.Add (ParseModule (tokens));
				continue;
			}

			if (tokens.TryAccept ("config_macros")) {
				rv.ConfigMacros.AddRange (tokens.AcceptIdentifierList ());
				Trace ($"Found config macros: {string.Join (", ", rv.ConfigMacros)}");
				continue;
			}

			if (tokens.TryAcceptKeyword ("link")) {
				var linkFramework = tokens.TryAcceptKeyword ("framework");
				var link = tokens.AcceptIdentifier ();
				rv.Link.Add (link);
				Trace ($"Found link: {link} linkFramework: {linkFramework}");
				continue;
			}

			if (tokens.TryAcceptKeyword ("export_as")) {
				rv.ReExport = tokens.AcceptIdentifier ();
				Trace ($"Found re-export: {rv.ReExport}");
				continue;
			}

			if (tokens.TryAcceptKeyword ("extern")) {
				tokens.AcceptKeyword ("module");
				var moduleId = tokens.AcceptIdentifier ();
				var stringLiteral = tokens.AcceptIdentifier ();
				Trace ($"Found extern module {moduleId} {stringLiteral}");
				continue;
			}

			if (tokens.TryAccept ("}"))
				return rv;

			throw new NotImplementedException ($"ParseFramework (2): unexpected token {tokens.Peek ()}");
		}
	}

	void ParseAttributes (Tokens tokens)
	{
		var attribs = new List<string> ();
		while (tokens.TryAccept ("[")) {
			attribs.Add (tokens.AcceptIdentifier ());
			tokens.AcceptOperator ("]");
		}
		if (attribs.Count > 0)
			Trace ($"Got attributes: {string.Join (", ", attribs)}");
	}

	static IEnumerable<string> Tokenize (StreamReader reader)
	{
		while (!reader.EndOfStream) {
			var chi = reader.Read ();
			if (chi == -1)
				break;
			var ch = (char) chi;
			if (Char.IsLetter (ch) || ch == '_') {
				var str = new StringBuilder ();
				str.Append (ch);
				ch = (char) reader.Peek ();
				while (Char.IsLetterOrDigit (ch) || ch == '_' || ch == '.') {
					str.Append ((char) reader.Read ());
					ch = (char) reader.Peek ();
				}
				yield return str.ToString ();
				continue;
			}
			switch (ch) {
			case '{':
			case '}':
			case '*':
			case '[':
			case ']':
			case ',':
			case '!':
				yield return ch.ToString ();
				break;
			case '"':
				var str = new StringBuilder ();
				while (reader.Peek () != '"') {
					str.Append ((char) reader.Read ());
				}
				reader.Read ();
				yield return str.ToString ();
				continue;
			case '\n':
			case ' ':
			case '\r':
			case '\t':
				continue; // whitespace
			case '/':
				var next = (char) reader.Peek ();
				if (next == '/') {
					while (reader.Peek () != '\n' && reader.Peek () != '\r') {
						reader.Read ();
					}
				} else if (next == '*') {
					reader.Read ();
					bool eol;
					do {
						eol = reader.Read () == '*' && reader.Read () == '/';
					} while (!eol && reader.Peek () != -1);
				} else {
					throw new NotImplementedException ($"/ followed by {next}");
				}
				break;
			default:
				throw new NotImplementedException ($"char: {ch} at position {reader.BaseStream.Position}");
			}
		}
	}
}

class Tokens : Queue<string> {
	public Tokens (IEnumerable<string> tokens)
	: base (tokens)
	{
	}

	public bool TryAcceptKeyword (string keyword)
	{
		if (Peek () == keyword) {
			Dequeue ();
			return true;
		}
		return false;
	}

	public bool TryAccept (string token)
	{
		if (Peek () == token) {
			Dequeue ();
			return true;
		}
		return false;
	}

	public void AcceptKeyword (string keyword)
	{
		var rv = Dequeue ();
		if (rv != keyword)
			throw new NotImplementedException ($"Expected keyword '{keyword}', got '{rv}'");
	}

	public void AcceptOperator (string op)
	{
		var rv = Dequeue ();
		if (rv != op)
			throw new NotImplementedException ($"Expected operator '{op}', got '{rv}'");
	}

	public string AcceptIdentifier ()
	{
		{
			var rv = Dequeue ();
			if (IsKeyword (rv))
				throw new NotImplementedException ($"Expected identifier, got keyword: {rv}");
			if (IsOperator (rv))
				throw new NotImplementedException ($"Expected identifier, got operator: {rv}");
			return rv;
		}
	}

	public IEnumerable<string> AcceptIdentifierList ()
	{
		do {
			yield return AcceptIdentifier ();
		} while (TryAccept (","));
	}

	static bool IsKeyword (string token)
	{
		return keywords.Contains (token);
	}

	static bool IsOperator (string token)
	{
		return operators.Contains (token);
	}

	static HashSet<string> operators = new HashSet<string> {
			"*", "[", "]", ",", "!", "{", "}"
		};

	static HashSet<string> keywords = new HashSet<string> {
		"config_macros",
		"conflict",
		"exclude",
		"explicit",
		"export",
		"export_as",
		"extern",
		"framework",
		"header",
		"link",
		"module",
		"private",
		"requires",
		"textual",
		"umbrella",
		"use",
	};
}

public sealed class Module {
	public bool IsFramework { get; set; }
	public bool IsExplicit { get; set; }
	public string Name;
	public List<Header> Headers = new List<Header> ();
	public List<Module> Modules = new List<Module> ();
	public List<string> Requirements = new List<string> ();
	public List<string> Exports = new List<string> ();
	public List<string> ConfigMacros = new List<string> ();
	public List<string> Link = new List<string> ();
	public string? ReExport { get; set; }
	public List<string> UmbrellaDirectories = new List<string> ();

	public Module (string name)
	{
		Name = name;
	}

	public IEnumerable<string> GetHeaders (string rootDirectory, bool includeUmbrella, bool includeTextual)
	{
		foreach (var h in Headers) {
			if (!includeUmbrella && h.IsUmbrella)
				continue;
			if (!includeTextual && h.IsTextual)
				continue;
			if (h.IsExclude)
				continue;
			yield return Path.Combine (rootDirectory, "Headers", h.Path);
		}

		foreach (var m in Modules) {
			var subRootDirectory = m.IsFramework ? Path.Combine (rootDirectory, "Frameworks", m.Name + ".framework") : rootDirectory;
			foreach (var h in m.GetHeaders (subRootDirectory, includeUmbrella, includeTextual)) {
				yield return Path.Combine (rootDirectory, h);
			}
			foreach (var u in m.UmbrellaDirectories) {
				var umbrellaDirectory = Path.Combine (rootDirectory, u);
				var files = Directory.GetFiles (umbrellaDirectory, "*.h");
				foreach (var f in files)
					yield return f;
			}
		}
	}
}

public sealed class Header {
	public bool IsUmbrella { get; set; }
	public bool IsTextual { get; set; }
	public bool IsExclude { get; set; }
	public string Path { get; private set; }

	public Header (string path)
	{
		Path = path;
	}
}
