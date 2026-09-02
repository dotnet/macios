using System.Diagnostics.CodeAnalysis;
using System.Text;

#nullable enable

namespace Registrar {
	abstract partial class Registrar {
		[return: NotNullIfNotNull (nameof (getterSelector))]
		internal static string? CreateSetterSelector (string? getterSelector)
		{
#if NET
			if (string.IsNullOrEmpty (getterSelector))
#else
			if (string.IsNullOrEmpty (getterSelector) || getterSelector is null)
#endif
				return getterSelector;

			var first = (int) getterSelector [0];
			// Objective-C uses the native 'toupper' function, which only handles a-z and translates them to A-Z.
			if (first >= 'a' && first <= 'z')
				first = (char) (first - 32 /* 'a' - 'A' */);
			return "set" + ((char) first).ToString () + getterSelector.Substring (1) + ":";
		}

		[return: NotNullIfNotNull (nameof (name))]
		public static string? SanitizeObjectiveCName (string? name)
		{
#if NET
			if (string.IsNullOrEmpty (name))
#else
			if (string.IsNullOrEmpty (name) || name is null)
#endif
				return name;

			StringBuilder? sb = null;

			for (int i = 0; i < name.Length; i++) {
				var ch = name [i];
				switch (ch) {
				case '.':
				case '+':
				case '/':
				case '`':
				case '@':
				case '<':
				case '>':
				case '$':
				case '-':
					if (sb is null)
						sb = new StringBuilder (name, 0, i, name.Length);
					sb.Append ('_');
					break;
				default:
					if (sb is not null)
						sb.Append (ch);
					break;
				}
			}

			if (sb is not null)
				return sb.ToString ();

			return name;
		}
	}
}
