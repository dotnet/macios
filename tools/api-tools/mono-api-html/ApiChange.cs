using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Mono.ApiTools {

	class ApiChange {
		public string Header = "";
		public TextChunk Member = new TextChunk ();
		public bool AnyChange;
		public bool IsNullabilityChange;
		public string SourceDescription;
		public State State;

		public ApiChange (string sourceDescription, State state)
		{
			SourceDescription = sourceDescription;
			State = state;
		}

		public ApiChange Append (string text)
		{
			Member.Append (text);
			return this;
		}

		public ApiChange AppendAdded (string text)
		{
			State.Formatter.DiffAddition (Member, text);
			AnyChange = true;
			return this;
		}

		public ApiChange AppendRemoved (string text)
		{
			State.Formatter.DiffRemoval (Member, text);
			AnyChange = true;
			return this;
		}

		public ApiChange AppendModified (string old, string @new)
		{
			State.Formatter.DiffModification (Member, old, @new);
			AnyChange = true;
			return this;
		}

		// Renders a type change: uses inline nullability rendering if the only
		// difference is '?' annotations, otherwise falls back to full modification.
		public ApiChange AppendTypeModified (string old, string @new)
		{
			if (Helper.DiffersOnlyByNullability (old, @new))
				return AppendNullabilityModified (old, @new);
			return AppendModified (old, @new);
		}

		// Renders a type modification where only nullability annotations ('?') differ.
		// Only the added/removed '?' characters are highlighted; the rest is plain text.
		public ApiChange AppendNullabilityModified (string old, string @new)
		{
			int si = 0;
			int ti = 0;
			while (si < old.Length && ti < @new.Length) {
				if (old [si] == @new [ti]) {
					Member.Append (old [si]);
					si++;
					ti++;
				} else if (old [si] == '?' && IsNullabilitySuffix (old, si)) {
					State.Formatter.DiffRemoval (Member, "?");
					AnyChange = true;
					si++;
				} else if (@new [ti] == '?' && IsNullabilitySuffix (@new, ti)) {
					State.Formatter.DiffAddition (Member, "?");
					AnyChange = true;
					ti++;
				} else {
					// Shouldn't happen for nullability-only diffs, fall back to full modification
					State.Formatter.DiffModification (Member, old.Substring (si), @new.Substring (ti));
					AnyChange = true;
					return this;
				}
			}
			// Handle remaining characters
			while (si < old.Length) {
				if (old [si] == '?' && IsNullabilitySuffix (old, si)) {
					State.Formatter.DiffRemoval (Member, "?");
					AnyChange = true;
					si++;
				} else {
					Member.Append (old [si]);
					si++;
				}
			}
			while (ti < @new.Length) {
				if (@new [ti] == '?' && IsNullabilitySuffix (@new, ti)) {
					State.Formatter.DiffAddition (Member, "?");
					AnyChange = true;
					ti++;
				} else {
					Member.Append (@new [ti]);
					ti++;
				}
			}
			return this;
		}

		static bool IsNullabilitySuffix (string text, int index)
		{
			// A '?' is a nullability suffix if it's at the end, or before a type separator.
			// The input type names are already formatted (via GetTypeName + Formatter), so generic
			// brackets appear as HTML entities (&lt; / &gt;). A '?' before '&' catches the &gt; case.
			if (index + 1 >= text.Length)
				return true;
			char next = text [index + 1];
			return next == ']' || next == ',' || next == '>' || next == '&' || next == ' ';
		}
	}

	class ApiChanges : Dictionary<string, List<ApiChange>> {

		public State State;

		public ApiChanges (State state)
		{
			State = state;
		}

		public void Add (XElement source, XElement target, ApiChange change)
		{
			if (!change.AnyChange)
				return;

			// Detect if this change is nullability-only
			if (DiffersOnlyByNullability (source, target))
				change.IsNullabilityChange = true;

			if (!TryGetValue (change.Header, out List<ApiChange>? list)) {
				list = new List<ApiChange> ();
				base.Add (change.Header, list);
			}
			list.Add (change);
		}

		static bool DiffersOnlyByNullability (XElement source, XElement target)
		{
			// Compare all attributes, stripping nullability from type-related attributes
			var typeAttributes = new HashSet<string> { "returntype", "fieldtype", "ptype", "eventtype", "type" };

			if (source.Name != target.Name)
				return false;

			// Check that all non-type-related attributes are the same
			var srcAttrs = source.Attributes ().ToDictionary (a => a.Name.LocalName, a => a.Value);
			var tgtAttrs = target.Attributes ().ToDictionary (a => a.Name.LocalName, a => a.Value);

			if (srcAttrs.Count != tgtAttrs.Count)
				return false;

			bool hasNullabilityDiff = false;
			foreach (var kvp in srcAttrs) {
				if (!tgtAttrs.TryGetValue (kvp.Key, out var tgtValue))
					return false;

				if (kvp.Value == tgtValue)
					continue;

				if (typeAttributes.Contains (kvp.Key)) {
					if (Helper.DiffersOnlyByNullability (kvp.Value, tgtValue))
						hasNullabilityDiff = true;
					else
						return false;
				} else {
					return false;
				}
			}

			// Always check child elements — a non-nullability child diff means this
			// is not a nullability-only change, even if the parent attributes differ only by nullability.
			var srcChildren = source.Elements ().ToList ();
			var tgtChildren = target.Elements ().ToList ();
			if (srcChildren.Count != tgtChildren.Count)
				return false;

			for (int i = 0; i < srcChildren.Count; i++) {
				if (XNode.DeepEquals (srcChildren [i], tgtChildren [i]))
					continue;
				if (!DiffersOnlyByNullability (srcChildren [i], tgtChildren [i]))
					return false;
				hasNullabilityDiff = true;
			}

			return hasNullabilityDiff;
		}
	}
}
