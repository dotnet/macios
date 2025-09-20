using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Mono.ApiTools {

	class ApiChange {
		public string Header;
		public TextChunk Member = new TextChunk ();
		public bool AnyChange;
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

			List<ApiChange> list;
			if (!TryGetValue (change.Header, out list)) {
				list = new List<ApiChange> ();
				base.Add (change.Header, list);
			}
			list.Add (change);
		}
	}
}

