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

	abstract class MemberComparer : Comparer {

		// true if this is the first element being added or removed in the group being rendered
		protected bool first;

		public MemberComparer (State state)
			: base (state)
		{
		}

		public void Compare (XElement source, XElement target)
		{
			var s = source.Element (GroupName);
			var t = target.Element (GroupName);
			if (XNode.DeepEquals (s, t))
				return;

			if (s is null) {
				Add (t.Elements (ElementName));
			} else if (t is null) {
				Remove (s.Elements (ElementName));
			} else {
				Compare (s.Elements (ElementName), t.Elements (ElementName));
			}
		}

		public override void SetContext (XElement current)
		{
		}

		string GetContainingType (XElement el)
		{
			return el.Ancestors ("class").First ().Attribute ("type").Value;
		}

		bool IsInInterface (XElement el)
		{
			return GetContainingType (el) == "interface";
		}

		public XElement Source { get; set; }

		public virtual bool Find (XElement e)
		{
			return e.GetAttribute ("name") == Source.GetAttribute ("name");
		}

		XElement Find (IEnumerable<XElement> target)
		{
			return target.SingleOrDefault (Find);
		}

		public override void Compare (IEnumerable<XElement> source, IEnumerable<XElement> target)
		{
			removed.Clear ();
			modified.Clear ();

			foreach (var s in source) {
				SetContext (s);
				Source = s;
				var t = Find (target);
				if (t is null) {
					// not in target, it was removed
					removed.Add (s);
				} else {
					t.Remove ();
					// possibly modified
					if (Equals (s, t, modified))
						continue;

					Modified (s, t, modified);
				}
			}
			// delayed, that way we show "Modified", "Added" and then "Removed"
			Remove (removed);

			Modify (modified);

			// remaining == newly added in target
			Add (target);
		}

		void Add (IEnumerable<XElement> elements)
		{
			bool a = false;
			foreach (var item in elements) {
				SetContext (item);
				if (!a) {
					Formatter.BeginMemberAddition (elements, this);
					a = true;
				}
				Added (item, false);
			}
			if (a)
				Formatter.EndMemberAddition ();
		}

		void Modify (ApiChanges modified)
		{
			foreach (var changes in modified) {
				Formatter.BeginMemberModification (changes.Key);
				foreach (var element in changes.Value) {
					Formatter.Diff (element);
				}
				Formatter.EndMemberModification ();
			}
		}

		void Remove (IEnumerable<XElement> elements)
		{
			bool r = false;
			foreach (var item in elements) {
				SetContext (item);
				if (!r) {
					Formatter.BeginMemberRemoval (elements, this);
					first = true;
					r = true;
				}
				Removed (item);
			}
			if (r)
				Formatter.EndMemberRemoval ();
		}

		public abstract string GetDescription (XElement e);

		protected StringBuilder GetObsoleteMessage (XElement e)
		{
			var sb = new StringBuilder ();
			string o = e.GetObsoleteMessage ();
			if (o is not null) {
				sb.Append ("[Obsolete");
				if (o.Length > 0)
					sb.Append (" (\"").Append (o).Append ("\")");
				sb.AppendLine ("]");
			}
			return sb;
		}

		public override bool Equals (XElement source, XElement target, ApiChanges changes)
		{
			// Change 'protected internal' into 'protected'
			RemoveInternalFromProtectedInternal (source);
			RemoveInternalFromProtectedInternal (target);

			return base.Equals (source, target, changes);
		}

		public override void Added (XElement target, bool wasParentAdded)
		{
			var o = GetObsoleteMessage (target);
			if (!first && (o.Length > 0))
				Output.WriteLine ();
			Indent ();
			Formatter.AddMember (this, o.ToString (), GetDescription (target));
			first = false;
		}

		public override void Modified (XElement source, XElement target, ApiChanges change)
		{
		}

		public override void Removed (XElement source)
		{
			var o = GetObsoleteMessage (source);
			if (!first && (o.Length > 0))
				Output.WriteLine ();

			Formatter.RemoveMember (this, o.ToString (), GetDescription (source));
			first = false;
		}

		string RenderGenericParameter (XElement gp)
		{
			var sb = new StringBuilder ();
			sb.Append (gp.GetTypeName ("name", State));

			var constraints = gp.DescendantList ("generic-parameter-constraints", "generic-parameter-constraint");
			if (constraints is not null && constraints.Count > 0) {
				sb.Append (" : ");
				for (int i = 0; i < constraints.Count; i++) {
					if (i > 0)
						sb.Append (", ");
					sb.Append (constraints [i].GetTypeName ("name", State));
				}
			}
			return sb.ToString ();
		}

		protected void RenderGenericParameters (XElement source, XElement target, ApiChange change)
		{
			var src = source.DescendantList ("generic-parameters", "generic-parameter");
			var tgt = target.DescendantList ("generic-parameters", "generic-parameter");
			var srcCount = src is null ? 0 : src.Count;
			var tgtCount = tgt is null ? 0 : tgt.Count;

			if (srcCount == 0 && tgtCount == 0)
				return;

			change.Append (Formatter.LesserThan);
			for (int i = 0; i < System.Math.Max (srcCount, tgtCount); i++) {
				if (i > 0)
					change.Append (", ");
				if (i >= srcCount) {
					change.AppendAdded (RenderGenericParameter (tgt [i]));
				} else if (i >= tgtCount) {
					change.AppendRemoved (RenderGenericParameter (src [i]));
				} else {
					var srcName = RenderGenericParameter (src [i]);
					var tgtName = RenderGenericParameter (tgt [i]);

					if (srcName != tgtName) {
						change.AppendModified (srcName, tgtName);
					} else {
						change.Append (srcName);
					}
				}
			}
			change.Append (Formatter.GreaterThan);
		}

		protected string FormatValue (string type, string value)
		{
			if (value is null)
				return "null";

			if (type == "string")
				return "\"" + value + "\"";
			else if (type == "bool") {
				switch (value) {
				case "True":
					return "true";
				case "False":
					return "false";
				default:
					return value;
				}
			}

			return value;
		}

		protected void RenderParameters (XElement source, XElement target, ApiChange change)
		{
			var src = source.DescendantList ("parameters", "parameter");
			var tgt = target.DescendantList ("parameters", "parameter");
			var srcCount = src is null ? 0 : src.Count;
			var tgtCount = tgt is null ? 0 : tgt.Count;

			change.Append (" (");
			for (int i = 0; i < System.Math.Max (srcCount, tgtCount); i++) {
				if (i > 0)
					change.Append (", ");

				string mods_tgt = tgt [i].GetAttribute ("direction") ?? "";
				string mods_src = src [i].GetAttribute ("direction") ?? "";

				if (mods_tgt.Length > 0)
					mods_tgt = mods_tgt + " ";

				if (mods_src.Length > 0)
					mods_src = mods_src + " ";

				if (i >= srcCount) {
					change.AppendAdded (mods_tgt + tgt [i].GetTypeName ("type", State) + " " + tgt [i].GetAttribute ("name"));
				} else if (i >= tgtCount) {
					change.AppendRemoved (mods_src + src [i].GetTypeName ("type", State) + " " + src [i].GetAttribute ("name"));
				} else {
					var paramSourceType = src [i].GetTypeName ("type", State);
					var paramTargetType = tgt [i].GetTypeName ("type", State);

					var paramSourceName = src [i].GetAttribute ("name");
					var paramTargetName = tgt [i].GetAttribute ("name");

					if (mods_src != mods_tgt) {
						change.AppendModified (mods_src, mods_tgt);
					} else {
						change.Append (mods_src);
					}

					if (paramSourceType != paramTargetType) {
						change.AppendModified (paramSourceType, paramTargetType);
					} else {
						change.Append (paramSourceType);
					}
					change.Append (" ");
					if (paramSourceName != paramTargetName) {
						change.AppendModified (paramSourceName, paramTargetName);
					} else {
						change.Append (paramSourceName);
					}

					var optSource = src [i].Attribute ("optional");
					var optTarget = tgt [i].Attribute ("optional");
					var srcValue = FormatValue (paramSourceType, src [i].GetAttribute ("defaultValue"));
					var tgtValue = FormatValue (paramTargetType, tgt [i].GetAttribute ("defaultValue"));

					if (optSource is not null) {
						if (optTarget is not null) {
							change.Append (" = ");
							if (srcValue != tgtValue) {
								change.AppendModified (srcValue, tgtValue);
							} else {
								change.Append (tgtValue);
							}
						} else {
							change.AppendRemoved (" = " + srcValue);
						}
					} else {
						if (optTarget is not null)
							change.AppendAdded (" = " + tgtValue);
					}
				}
			}

			change.Append (")");
		}

		void RenderVTable (XElement sourceElement, XElement targetElement, MethodAttributes source, MethodAttributes target, ApiChange change)
		{
			var srcAbstract = (source & MethodAttributes.Abstract) == MethodAttributes.Abstract;
			var tgtAbstract = (target & MethodAttributes.Abstract) == MethodAttributes.Abstract;
			var srcFinal = (source & MethodAttributes.Final) == MethodAttributes.Final;
			var tgtFinal = (target & MethodAttributes.Final) == MethodAttributes.Final;
			var srcVirtual = (source & MethodAttributes.Virtual) == MethodAttributes.Virtual;
			var tgtVirtual = (target & MethodAttributes.Virtual) == MethodAttributes.Virtual;
			var srcOverride = (source & MethodAttributes.VtableLayoutMask) != MethodAttributes.NewSlot;
			var tgtOverride = (target & MethodAttributes.VtableLayoutMask) != MethodAttributes.NewSlot;

			var srcWord = srcVirtual ? (srcOverride ? "override" : "virtual") : string.Empty;
			var tgtWord = tgtVirtual ? (tgtOverride ? "override" : "virtual") : string.Empty;

			if (srcAbstract) {
				if (tgtAbstract) {
					change.Append ("abstract ");
				} else if (tgtVirtual) {
					change.AppendModified ("abstract", tgtWord).Append (" ");
				} else {
					change.AppendRemoved ("abstract").Append (" ");
				}
			} else {
				if (tgtAbstract) {
					change.AppendAdded ("abstract").Append (" ");
				} else if (srcWord != tgtWord) {
					if (!tgtFinal) {
						change.AppendModified (srcWord, tgtWord).Append (" ");
					}
				} else if (tgtWord.Length > 0) {
					change.Append (tgtWord).Append (" ");
				} else if (srcWord.Length > 0) {
					change.AppendRemoved (srcWord).Append (" ");
				}
			}

			if (srcFinal) {
				if (tgtFinal) {
					change.Append ("final ");
				} else {
					change.AppendRemoved ("final").Append (" ");
				}
			} else {
				if (tgtFinal && srcVirtual) {
					change.AppendModified ("virtual", "final").Append (" ");
				}
			}
		}

		protected string GetVisibility (MethodAttributes attr)
		{
			switch (attr) {
			case MethodAttributes.Private:
			case MethodAttributes.PrivateScope:
				return "private";
			case MethodAttributes.Assembly:
				return "internal";
			case MethodAttributes.FamANDAssem:
				return "private internal";
			case MethodAttributes.FamORAssem:
				return "protected"; // customers don't care about 'internal';
			case MethodAttributes.Family:
				return "protected";
			case MethodAttributes.Public:
				return "public";
			default:
				throw new NotImplementedException ();
			}
		}

		protected void RenderVisibility (MethodAttributes source, MethodAttributes target, ApiChange diff)
		{
			source = source & MethodAttributes.MemberAccessMask;
			target = target & MethodAttributes.MemberAccessMask;

			if (source == target) {
				diff.Append (GetVisibility (target));
			} else {
				diff.AppendModified (GetVisibility (source), GetVisibility (target));
			}
			diff.Append (" ");
		}

		protected void RenderStatic (MethodAttributes src, MethodAttributes tgt, ApiChange diff)
		{
			var srcStatic = (src & MethodAttributes.Static) == MethodAttributes.Static;
			var tgtStatic = (tgt & MethodAttributes.Static) == MethodAttributes.Static;

			if (srcStatic != tgtStatic) {
				if (srcStatic) {
					diff.AppendRemoved ("static").Append (" ");
				} else {
					diff.AppendAdded ("static").Append (" ");
				}
			}
		}

		protected void RenderMethodAttributes (XElement source, XElement target, MethodAttributes src, MethodAttributes tgt, ApiChange diff)
		{
			RenderStatic (src, tgt, diff);
			RenderVisibility (src & MethodAttributes.MemberAccessMask, tgt & MethodAttributes.MemberAccessMask, diff);
			RenderVTable (source, target, src, tgt, diff);
		}

		protected void RenderMethodAttributes (XElement source, XElement target, ApiChange diff)
		{
			RenderMethodAttributes (source, target, source.GetMethodAttributes (), target.GetMethodAttributes (), diff);
		}

		protected void RemoveInternalFromProtectedInternal (XElement element)
		{
			var attrib = element.GetMethodAttributes ();

			// Changing between 'protected' and 'protected internal' is not visible in the API, so remove the 'internal' part.
			if ((attrib & MethodAttributes.FamORAssem) == MethodAttributes.FamORAssem) {
				attrib = (attrib & ~MethodAttributes.MemberAccessMask) | MethodAttributes.Family;
				element.Attribute ("attrib").Value = ((int) attrib).ToString ();
			}
		}

		protected void RenderName (XElement source, XElement target, ApiChange change)
		{
			var name = target.GetAttribute ("name");
			// show the constructor as it would be defined in C#
			name = name.Replace (".ctor", State.Type);

			var p = name.IndexOf ('(');
			if (p >= 0)
				name = name.Substring (0, p);

			change.Append (name);
		}

	}
}
