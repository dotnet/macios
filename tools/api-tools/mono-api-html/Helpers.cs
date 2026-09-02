// 
// Authors
//    Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013-2014 Xamarin Inc. http://www.xamarin.com
// 
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace Mono.ApiTools {

	static class Helper {
		public static bool IsTrue (this XElement self, string name)
		{
			return (self.GetAttribute (name) == "true");
		}

		public static string? GetAttribute (this XElement self, string name)
		{
			var n = self.Attribute (name);
			if (n is null)
				return null;
			return n.Value;
		}

		public static IEnumerable<XElement> EnumerateAttributes (this XElement self, string? attributeName = null)
		{
			if (self is null)
				yield break;

			var attribs = self.Element ("attributes");
			if (attribs is null)
				yield break;

			foreach (var attrib in attribs.Elements ("attribute")) {
				if (!string.IsNullOrEmpty (attributeName) && attrib.GetAttribute ("name") != attributeName)
					continue;
				yield return attrib;
			}
		}

		static bool TryGetAttributeProperty (this XElement? self, string attributeName, bool recursive, out string? firstArgument)
		{
			firstArgument = null;

			if (self is null)
				return false;

			foreach (var ca in self.EnumerateAttributes (attributeName)) {
				var args = ca.Element ("arguments");
				if (args is not null) {
					var firstCtorArgument = args.Elements ("argument")?.FirstOrDefault ();
					if (firstCtorArgument is not null && firstCtorArgument.GetAttribute ("type") == "System.String") {
						firstArgument = firstCtorArgument.GetAttribute ("value");
					}
				}

				return true;
			}

			if (recursive)
				return TryGetAttributeProperty (self.Parent, attributeName, recursive, out firstArgument);

			return false;
		}

		// null == no obsolete, String.Empty == no description
		public static string? GetObsoleteMessage (this XElement self)
		{
			if (TryGetAttributeProperty (self, "System.ObsoleteAttribute", false, out string? message))
				return message ?? String.Empty;
			return null;
		}

		public static IEnumerable<XElement>? Descendants (this XElement self, params string [] names)
		{
			XElement? el = self;
			if (el is null)
				return null;

			for (int i = 0; i < names.Length - 1; i++) {
				el = el.Element (names [i]);
				if (el is null)
					return null;
			}
			return el.Elements (names [names.Length - 1]);
		}

		public static List<XElement>? DescendantList (this XElement self, params string [] names)
		{
			var descendants = self.Descendants (names);
			if (descendants is null)
				return null;
			return descendants.ToList ();
		}

		// make it beautiful (.NET -> C#)
		public static string? GetTypeName (this XElement self, string name, State state)
		{
			string? type = self.GetAttribute (name);
			if (type is null)
				return null;

			StringBuilder sb = null!;
			bool is_nullable = false;
			bool is_nullable_ref = false;
			if (type.StartsWith ("System.Nullable`1[", StringComparison.Ordinal)) {
				is_nullable = true;
				sb = new StringBuilder (type, 18, type.Length - 19, 1024);
			} else {
				sb = new StringBuilder (type);
			}

			// Handle nullable reference type annotation (trailing '?' added by mono-api-info)
			if (!is_nullable && sb.Length > 0 && sb [sb.Length - 1] == '?') {
				is_nullable_ref = true;
				sb.Remove (sb.Length - 1, 1);
			}

			bool is_ref = (sb [sb.Length - 1] == '&');
			if (is_ref)
				sb.Remove (sb.Length - 1, 1);

			int array = 0;
			while ((sb [sb.Length - 1] == ']') && (sb [sb.Length - 2] == '[')) {
				sb.Remove (sb.Length - 2, 2);
				array++;
			}

			bool is_pointer = (sb [sb.Length - 1] == '*');
			if (is_pointer)
				sb.Remove (sb.Length - 1, 1);

			type = GetElementTypeName (self, sb.Replace ('+', '.').ToString (), state);
			sb.Length = 0;
			if (is_ref)
				sb.Append (self.GetAttribute ("direction")).Append (' ');

			sb.Append (type);

			while (array-- > 0)
				sb.Append ("[]");
			if (is_nullable)
				sb.Append ('?');
			if (is_nullable_ref)
				sb.Append ('?');
			if (is_pointer)
				sb.Append ('*');
			return sb.ToString ();
		}

		static string GetElementTypeName (XElement element, string type, State state)
		{
			int pos = type.IndexOf ('`');
			if (pos >= 0) {
				int end = type.LastIndexOf (']');
				string subtype = type.Substring (pos + 3, end - pos - 3);
				return type.Substring (0, pos) + state.Formatter.LesserThan + GetElementTypeName (element, subtype, state) + state.Formatter.GreaterThan;
			}

			switch (type) {
			case "System.String":
				return "string";
			case "System.Int32":
				return "int";
			case "System.UInt32":
				return "uint";
			case "System.Int64":
				return "long";
			case "System.UInt64":
				return "ulong";
			case "System.Void":
				return "void";
			case "System.Boolean":
				return "bool";
			case "System.Object":
				return "object";
			case "System.Single":
				return "float";
			case "System.Double":
				return "double";
			case "System.Byte":
				return "byte";
			case "System.SByte":
				return "sbyte";
			case "System.Int16":
				return "short";
			case "System.UInt16":
				return "ushort";
			case "System.Char":
				return "char";
			case "System.nint":
				return "nint";
			case "System.nuint":
				return "nuint";
			case "System.nfloat":
				return "nfloat";
			case "System.IntPtr":
				return IsNativeInteger (element) ? "nint" : "IntPtr";
			case "System.UIntPtr":
				return IsNativeInteger (element) ? "nuint" : "UIntPtr";
			default:
				if (type.StartsWith (state.Namespace, StringComparison.Ordinal))
					type = type.Substring (state.Namespace.Length + 1);
				return type;
			}
		}

		static bool IsNativeInteger (XElement element)
		{
			var attribs = element.Descendants ("attributes", "attribute");
			if (attribs is null)
				return false;
			foreach (var attrib in attribs) {
				if (attrib.GetAttribute ("name") == "System.Runtime.CompilerServices.NativeIntegerAttribute")
					return true;
			}

			return false;
		}

		public static MethodAttributes GetMethodAttributes (this XElement element)
		{
			var srcAttribs = element.Attribute ("attrib");
			return (MethodAttributes) (srcAttribs is not null ? Int32.Parse (srcAttribs.Value) : 0);
		}

		public static FieldAttributes GetFieldAttributes (this XElement element)
		{
			var srcAttribs = element.Attribute ("attrib");
			return (FieldAttributes) (srcAttribs is not null ? Int32.Parse (srcAttribs.Value) : 0);
		}

		// Strips trailing '?' nullability annotations from a type name for comparison purposes.
		// Handles both top-level (System.String?) and nested generics (List`1[System.String?]).
		public static string? StripNullability (string? type)
		{
			if (type is null)
				return null;
			// Remove all '?' that appear before ']', at end of string, before ',',
			// before '>' or '&' (HTML entities like &gt;), before ' ' (before param name),
			// or before '%' (placeholder boundaries like %GREATERTHANREPLACEMENT%)
			var sb = new StringBuilder (type.Length);
			for (int i = 0; i < type.Length; i++) {
				if (type [i] == '?') {
					if (i + 1 >= type.Length || type [i + 1] == ']' || type [i + 1] == ',' || type [i + 1] == '>' || type [i + 1] == '&' || type [i + 1] == ' ' || type [i + 1] == '%')
						continue;
				}
				sb.Append (type [i]);
			}
			return sb.ToString ();
		}

		// Returns true if two type names differ only in nullability annotations.
		public static bool DiffersOnlyByNullability (string? source, string? target)
		{
			if (source == target)
				return false;
			return StripNullability (source) == StripNullability (target);
		}
	}
}
