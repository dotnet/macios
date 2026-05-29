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

			if (!hasNullabilityDiff) {
				// Check child elements recursively
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
			}

			return hasNullabilityDiff;
		}
	}
}
