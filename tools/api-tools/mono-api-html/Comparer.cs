// 
// Authors
//    Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013 Xamarin Inc. http://www.xamarin.com
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
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Mono.ApiTools {

	abstract class Comparer {

		protected List<XElement> removed;
		protected ApiChanges modified;

		public abstract string GroupName { get; }
		public abstract string ElementName { get; }

		public Comparer (State state)
		{
			State = state;
			removed = new List<XElement> ();
			modified = new ApiChanges (state);
		}

		protected void WriteAttributes (XElement element)
		{
			foreach (var attribute in element.EnumerateAttributes ())
				Indent ().WriteLine (RenderAttribute (attribute));
		}

		protected string RenderAttribute (XElement attribute)
		{
			var sb = new StringBuilder ();
			sb.Append ("[");
			sb.Append (attribute.GetAttribute ("name"));
			sb.Append ("(");
			var args = attribute.Element ("arguments");
			if (args is not null) {
				var arguments = args.Elements ("argument").ToArray ();
				foreach (var arg in arguments) {
					var value = arg.GetAttribute ("value");
					var isString = arg.GetAttribute ("type") == "System.String";
					if (isString)
						sb.Append ('"');
					sb.Append (value);
					if (isString)
						sb.Append ('"');
					if (arg != arguments.Last ())
						sb.Append (", ");
				}
			}
			var props = attribute.Element ("properties");
			if (props is not null) {
				var properties = props.Elements ("property").ToArray ();
				foreach (var prop in properties) {
					sb.Append (prop.GetAttribute ("name"));
					sb.Append (" = ");
					sb.Append (prop.GetAttribute ("value"));
					if (prop != properties.Last ())
						sb.Append (", ");
				}
			}
			sb.Append (")]");
			return sb.ToString ();
		}

		protected void RenderAttributes (XElement source, XElement target, ApiChange diff)
		{
			var srcAttributes = source.EnumerateAttributes ().Select (RenderAttribute).OrderBy (v => v).ToArray ();
			var tgtAttributes = target.EnumerateAttributes ().Select (RenderAttribute).OrderBy (v => v).ToArray ();
			if (srcAttributes.SequenceEqual (tgtAttributes))
				return;

			var added = tgtAttributes.Except (srcAttributes).ToList ();
			var removed = srcAttributes.Except (tgtAttributes).ToList ();
			var modified = new List<(string Source, string Target)> ();

			for (var i = added.Count - 1; i >= 0; i--) {
				var addedType = added [i].Substring (0, added [i].IndexOf ('('));
				var removedOfSameTypeIndex = removed.FindIndex ((v) => v.StartsWith (addedType));
				if (removedOfSameTypeIndex == -1)
					continue;

				modified.Add ((removed [removedOfSameTypeIndex], added [i]));
				added.RemoveAt (i);
				removed.RemoveAt (removedOfSameTypeIndex);
			}

			if (added.Any ()) {
				foreach (var a in added) {
					var breaking = a.StartsWith ("[System.Diagnostics.CodeAnalysis.ExperimentalAttribute");
					diff.AppendAdded (a + "\n", breaking);
				}
			}
			if (modified.Any ()) {
				foreach (var a in modified) {
					diff.AppendModified (a.Source + "\n", a.Target + "\n", false);
				}
			}
			if (removed.Any ()) {
				foreach (var a in removed) {
					diff.AppendRemoved (a + "\n", false);
				}
			}
		}

		protected virtual bool IsBreakingRemoval (XElement e)
		{
			if (e.IsExperimental ())
				return false;
			return true;
		}

		public State State { get; }

		public Formatter Output {
			get { return Formatter; }
		}

		public Formatter Formatter {
			get { return State.Formatter; }
		}

		protected Formatter Indent ()
		{
			Output.WriteIndentation ();
			return Output;
		}

		public abstract void Added (XElement target, bool wasParentAdded);
		public abstract void Modified (XElement source, XElement target, ApiChanges changes);
		public abstract void Removed (XElement source);

		public virtual bool Equals (XElement source, XElement target, ApiChanges changes)
		{
			return XNode.DeepEquals (source, target);
		}

		public abstract void SetContext (XElement current);

		public virtual void Compare (IEnumerable<XElement> source, IEnumerable<XElement> target)
		{
			removed.Clear ();
			modified.Clear ();

			foreach (var s in source) {
				SetContext (s);
				string sn = s.GetAttribute ("name");
				var t = target is null ? null : target.SingleOrDefault (x => x.GetAttribute ("name") == sn);
				if (t is null) {
					// not in target, it was removed
					removed.Add (s);
				} else {
					t.Remove ();
					// possibly modified
					if (Equals (s, t, modified))
						continue;

					// still in target so will be part of Added
					Modified (s, t, modified);
				}
			}
			// delayed, that way we show "Modified", "Added" and then "Removed"
			foreach (var item in removed) {
				SetContext (item);
				Removed (item);
			}
			// remaining == newly added in target
			if (target is not null) {
				foreach (var item in target) {
					SetContext (item);
					Added (item, false);
				}
			}
		}
	}
}
