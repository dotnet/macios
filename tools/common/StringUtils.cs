using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

#nullable enable

namespace Xamarin.Utils {
	class StringUtils {
		static StringUtils ()
		{
			PlatformID pid = Environment.OSVersion.Platform;
			if (((int) pid != 128 && pid != PlatformID.Unix && pid != PlatformID.MacOSX))
				shellQuoteChar = '"'; // Windows
			else
				shellQuoteChar = '\''; // !Windows
		}

		static char shellQuoteChar;
		static char [] mustQuoteCharacters = new char [] { ' ', '\'', ',', '$', '\\' };
		static char [] mustQuoteCharactersProcess = { ' ', '\\', '"', '\'' };

		[return: NotNullIfNotNull (nameof (array))]
		public static string []? Quote (params string [] array)
		{
			if (array is null || array.Length == 0)
				return array;

			var rv = new string [array.Length];
			for (var i = 0; i < array.Length; i++)
				rv [i] = Quote (array [i]);
			return rv;
		}

		public static string Quote (string f)
		{
			if (String.IsNullOrEmpty (f))
				return f ?? String.Empty;

			if (f.IndexOfAny (mustQuoteCharacters) == -1)
				return f;

			var s = new StringBuilder ();

			s.Append (shellQuoteChar);
			foreach (var c in f) {
				if (c == '\'' || c == '"' || c == '\\')
					s.Append ('\\');

				s.Append (c);
			}
			s.Append (shellQuoteChar);

			return s.ToString ();
		}

		public static string []? QuoteForProcess (IList<string> arguments)
		{
			if (arguments is null)
				return Array.Empty<string> ();
			return QuoteForProcess (arguments.ToArray ());
		}

		[return: NotNullIfNotNull ("array")]
		public static string []? QuoteForProcess (params string [] array)
		{
			if (array is null || array.Length == 0)
				return array;

			var rv = new string [array.Length];
			for (var i = 0; i < array.Length; i++)
				rv [i] = QuoteForProcess (array [i]);
			return rv;
		}

		// Quote input according to how System.Diagnostics.Process needs it quoted.
		public static string QuoteForProcess (string f)
		{
			if (String.IsNullOrEmpty (f))
				return f ?? String.Empty;

			if (f.IndexOfAny (mustQuoteCharactersProcess) == -1)
				return f;

			var s = new StringBuilder ();

			s.Append ('"');
			foreach (var c in f) {
				if (c == '"') {
					s.Append ('\\');
				} else if (c == '\\') {
					s.Append (c);
				}
				s.Append (c);
			}
			s.Append ('"');

			return s.ToString ();
		}

		public static string FormatArguments (params string [] arguments)
		{
			return FormatArguments ((IList<string>) arguments);
		}

		public static string FormatArguments (IList<string> arguments)
		{
			return string.Join (" ", QuoteForProcess (arguments)!);
		}

		public static string? Unquote (string input)
		{
			if (input is null || input.Length == 0 || input [0] != shellQuoteChar)
				return input;

			var builder = new StringBuilder ();
			for (int i = 1; i < input.Length - 1; i++) {
				char c = input [i];
				if (c == '\\') {
					builder.Append (input [i + 1]);
					i++;
					continue;
				}
				builder.Append (input [i]);
			}
			return builder.ToString ();
		}

		public static string [] ParseArguments (string quotedArguments)
		{
			if (!TryParseArguments (quotedArguments, out var argv, out var ex))
				throw ex;

			return argv;
		}

		public static bool TryParseArguments (string quotedArguments, [NotNullWhen (true)] out string []? argv, [NotNullWhen (false)] out Exception? ex)
		{
			var builder = new StringBuilder ();
			var args = new List<string> ();
			int i = 0, j;
			char c;

			while (i < quotedArguments.Length) {
				c = quotedArguments [i];
				if (c != ' ' && c != '\t') {
					if (TryGetArgument (builder, quotedArguments, i, out var argument, out j, out ex)) {
						args.Add (argument);
						i = j;
					} else {
						argv = null;
						return false;
					}
				}

				i++;
			}

			argv = args.ToArray ();
			ex = null;

			return true;
		}

		static bool TryGetArgument (StringBuilder builder, string buf, int startIndex, [NotNullWhen (true)] out string? argument, out int endIndex, [NotNullWhen (false)] out Exception? ex)
		{
			bool escaped = false;
			char qchar, c = '\0';
			int i = startIndex;

			builder.Clear ();
			switch (buf [startIndex]) {
			case '\'': qchar = '\''; i++; break;
			case '"': qchar = '"'; i++; break;
			default: qchar = '\0'; break;
			}

			while (i < buf.Length) {
				c = buf [i];

				if (c == qchar && !escaped) {
					// unescaped qchar means we've reached the end of the argument
					i++;
					break;
				}

				if (c == '\\') {
					escaped = true;
				} else if (escaped) {
					builder.Append (c);
					escaped = false;
				} else if (qchar == '\0' && (c == ' ' || c == '\t')) {
					break;
				} else if (qchar == '\0' && (c == '\'' || c == '"')) {
					string sofar = builder.ToString ();

					if (TryGetArgument (builder, buf, i, out var embedded, out endIndex, out ex)) {
						i = endIndex;
						builder.Clear ();
						builder.Append (sofar);
						builder.Append (embedded);
						continue;
					}

					argument = null;
					return false;

				} else {
					builder.Append (c);
				}

				i++;
			}

			if (escaped || (qchar != '\0' && c != qchar)) {
				ex = new FormatException (escaped ? "Incomplete escape sequence." : "No matching quote found.");
				endIndex = -1;
				argument = null;
				return false;
			}

			endIndex = i;
			ex = null;

			argument = builder.ToString ();
			return true;
		}

		// Version.Parse requires, minimally, both major and minor parts.
		// However we want to accept `11` as `11.0`
		public static Version ParseVersion (string v)
		{
			int major;
			if (int.TryParse (v, out major))
				return new Version (major, 0);
			return Version.Parse (v);
		}

		/// <summary>
		/// Format a message according to MSBuild diagnostic format.
		/// </summary>
		/// <param name="fileName">The file name in the formatted message (null if no file name is present).</param>
		/// <param name="lineNumber">The line number in the formatted message (null if no line number is present).</param>
		/// <param name="isError">True if the message is an error, false if it is a warning.</param>
		/// <param name="prefix">The prefix of the message.</param>
		/// <param name="code">The code of the message.</param>
		/// <param name="message">The message text.</param>
		/// <returns></returns>
		/// <see cref="https://learn.microsoft.com/visualstudio/msbuild/msbuild-diagnostic-format-for-tasks"/>
		public static string FormatMessage (string? fileName, long? lineNumber, bool isError, string prefix, int code, string message)
		{
			var sb = new StringBuilder ();
			if (!string.IsNullOrEmpty (fileName)) {
				sb.Append (fileName);
				if (lineNumber is not null && lineNumber.Value > 0)
					sb.Append ('(').Append (lineNumber.Value).Append (")");
				sb.Append (": ");
			}
			sb.Append (isError ? "error" : "warning").Append (' ');
			sb.Append (prefix).Append (code.ToString ("0000: "));
			sb.Append (message);
			return sb.ToString ();
		}

		/// <summary>
		/// Parse a formatted message created with <see cref="FormatMessage" />.
		/// </summary>
		/// <param name="line">The formatted message line to parse.</param>
		/// <param name="fileName">The file name in the formatted message (null if no file name is present).</param>
		/// <param name="lineNumber">The line number in the formatted message (null if no line number is present).</param>
		/// <param name="isError">True if the message is an error, false if it is a warning, null if neither.</param>
		/// <param name="prefix">The prefix of the message.</param>
		/// <param name="code">The code of the message.</param>
		/// <param name="message">The message text.</param>
		/// <returns></returns>
		public static bool TryParseFormattedMessage (string? line, out string? fileName, out int? lineNumber, out bool isError, [NotNullWhen (true)] out string? prefix, out int code, [NotNullWhen (true)] out string? message)
		{
			fileName = null;
			lineNumber = null;
			isError = false;
			prefix = null;
			code = 0;
			message = null;

#if NET
			if (string.IsNullOrEmpty (line))
#else
			if (string.IsNullOrEmpty (line) || line is null)
#endif
				return false;

			var origin = string.Empty;

			if (IndexOfAny (line, out var idxError, out var endError, ": error ", ":  error ")) {
				isError = true;
				origin = line.Substring (0, idxError);
				line = line.Substring (endError);
				line = RemovePathAtEnd (line);
			} else if (IndexOfAny (line, out var idxWarning, out var endWarning, ": warning ", ":  warning ")) {
				isError = false;
				origin = line.Substring (0, idxWarning);
				line = line.Substring (endWarning);
				line = RemovePathAtEnd (line);
			} else if (line.StartsWith ("error ", StringComparison.Ordinal)) {
				isError = true;
				line = line.Substring (6);
			} else if (line.StartsWith ("warning ", StringComparison.Ordinal)) {
				isError = false;
				line = line.Substring (8);
			} else {
				// something else
				return false;
			}

			if (line.Length < 7)
				return false; // something else

			var firstNumber = line.IndexOfAny ("0123456789".ToCharArray ());
			if (firstNumber == -1)
				return false; // something else)
			prefix = line.Substring (0, firstNumber);
			if (!int.TryParse (line.Substring (firstNumber, 4), out var codeValue))
				return false; // something else
			code = codeValue;
			line = line.Substring (firstNumber + 4);

			if (line.StartsWith (": "))
				line = line.Substring (2);

			message = line;

			if (!string.IsNullOrEmpty (origin)) {
				var idx = origin.IndexOf ('(');
				if (idx > 0) {
					var closing = origin.IndexOf (')');
					if (!int.TryParse (origin.Substring (idx + 1, closing - idx - 1), out var number))
						return false;
					lineNumber = number;
					fileName = origin.Substring (0, idx);
				} else {
					fileName = origin;
				}
			}

			return true;
		}

		static bool IndexOfAny (string line, out int start, out int end, params string [] values)
		{
			foreach (var value in values) {
				start = line.IndexOf (value, StringComparison.Ordinal);
				if (start >= 0) {
					end = start + value.Length;
					return true;
				}
			}
			start = -1;
			end = -1;
			return false;
		}

		static string RemovePathAtEnd (string line)
		{
#if NET
			if (line.TrimEnd ().EndsWith (']')) {
#else
			if (line.TrimEnd ().EndsWith ("]")) {
#endif
				var start = line.LastIndexOf ('[');
				if (start >= 0) {
					// we want to get the space before `[` too.
					if (start > 0 && line [start - 1] == ' ')
						start--;

					line = line.Substring (0, start);
					return line;
				}
			}

			return line;
		}
	}

	static class StringExtensions {
		internal static string [] SplitLines (this string s) => s.Split (new [] { Environment.NewLine }, StringSplitOptions.None);

		// Adds an element to an array and returns a new array with the added element.
		// The original array is not modified.
		// If the original array is null, a new array is also created, with just the new value.
		internal static T [] CopyAndAdd<T> (this T [] array, T value)
		{
			if (array is null || array.Length == 0)
				return new T [] { value };
			var tmpArray = array;
			Array.Resize (ref array, array.Length + 1);
			tmpArray [tmpArray.Length - 1] = value;
			return tmpArray;
		}
	}
}
