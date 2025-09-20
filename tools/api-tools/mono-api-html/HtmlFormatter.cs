// 
// Authors
//    Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013 Xamarin Inc. http://www.xamarin.com
// Copyright 2018 Microsoft Inc.
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
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace Mono.ApiTools {

	class HtmlFormatter : Formatter {
		public string Header { get; set; } = "";

		public HtmlFormatter (State state)
			: base (state)
		{
		}

		public override string LesserThan => "&lt;";
		public override string GreaterThan => "&gt;";

		public override void BeginDocument (string title)
		{
			output.WriteLine ("<div>");
			if (State.Colorize) {
				output.WriteLine ("<style scoped>");
				output.WriteLine ("\t.obsolete { color: gray; }");
				output.WriteLine ("\t.added { color: green; }");
				output.WriteLine ("\t.added-inline { text-decoration: line-through; }");
				output.WriteLine ("\t.removed-inline { text-decoration: line-through; }");
				output.WriteLine ("\t.removed { color: red; }");
				output.WriteLine ("</style>");
			}
		}

		public override void BeginAssembly ()
		{
			output.WriteLine ($"<h1>{State.Assembly}.dll</h1>");
			if (!string.IsNullOrEmpty (Header)) {
				var lines = File.ReadAllLines (Header);
				var htmlEncoding = false;
				foreach (var line in lines) {
					if (line.Contains ("<!-- start html encode -->")) {
						htmlEncoding = true;
						continue;
					} else if (line.Contains ("<!-- end html encode -->")) {
						htmlEncoding = false;
						continue;
					}

					if (htmlEncoding) {
						output.WriteLine (System.Web.HttpUtility.HtmlEncode (line));
					} else {
						output.WriteLine (line);
					}
				}
			}
			output.WriteLine ("<div> <!-- start topmost div -->");
		}

		public override void EndAssembly ()
		{
			output.WriteLine ("</div> <!-- end topmost div -->");
			output.WriteLine ("</div>");
		}

		public override void BeginNamespace (string action)
		{
			output.WriteLine ($"<!-- start namespace {State.Namespace} --> <div>");
			output.WriteLine ($"<h2>{action}Namespace {State.Namespace}</h2>");
		}

		public override void EndNamespace ()
		{
			output.WriteLine ($"</div> <!-- end namespace {State.Namespace} -->");
		}

		public override void BeginAttributeModification ()
		{
			output.WriteLine ("<div>");
			output.WriteLine ("<p>Modified attributes:</p>");
			output.WriteLine ("<pre>");
			IndentLevel++;
		}

		public override void AddAttributeModification (string source, string target)
		{
			output.Write ("\t<span class='removed'>");
			AppendEncoded (source);
			output.WriteLine ("</span>");
			output.Write ("\t<span class='added'>");
			AppendEncoded (target);
			output.WriteLine ("</span>");
		}

		public override void EndAttributeModification ()
		{
			IndentLevel--;
			output.WriteLine ("</pre>");
			output.WriteLine ("</div>");
		}

		public override void BeginTypeAddition ()
		{
			output.WriteLine ($"<div> <!-- start type {State.Type} -->");
			output.WriteLine ($"<h3>New Type {State.Namespace}.{State.Type}</h3>");
			output.WriteLine ("<pre class='added'>");
		}

		public override void EndTypeAddition ()
		{
			output.WriteLine ("</pre>");
			output.WriteLine ($"</div> <!-- end type {State.Type} -->");
		}

		public override void BeginTypeModification ()
		{
			output.WriteLine ($"<!-- start type {State.Type} --> <div>");
			output.WriteLine ($"<h3>Type Changed: {State.Namespace}.{State.Type}</h3>");
		}

		public override void EndTypeModification ()
		{
			output.WriteLine ($"</div> <!-- end type {State.Type} -->");
		}

		public override void BeginTypeRemoval ()
		{
			output.Write ($"<h3>Removed Type <span class='removed'>{State.Namespace}.{State.Type}</span></h3>");
		}

		public override void BeginMemberAddition (IEnumerable<XElement> list, MemberComparer member)
		{
			output.WriteLine ("<div>");
			if (State.BaseType == "System.Enum") {
				output.WriteLine ("<p>Added value{0}:</p>", list.Count () > 1 ? "s" : String.Empty);
				output.WriteLine ("<pre class='added'>");
			} else {
				output.WriteLine ("<p>Added {0}:</p>", list.Count () > 1 ? member.GroupName : member.ElementName);
				output.WriteLine ("<pre>");
			}
			IndentLevel++;
		}

		public override void AddMember (MemberComparer member, string obsolete, string description)
		{
			output.Write ("<span class='added'>", member.ElementName);
			output.Write ($"{obsolete}{description}");
			output.WriteLine ("</span>");
		}

		public override void EndMemberAddition ()
		{
			IndentLevel--;
			output.WriteLine ("</pre>");
			output.WriteLine ("</div>");
		}

		public override void BeginMemberModification (string sectionName)
		{
			output.WriteLine ($"<p>{sectionName}:</p>");
			output.WriteLine ("<pre>");
		}

		public override void EndMemberModification ()
		{
			output.WriteLine ("</pre>");
		}

		public override void BeginMemberRemoval (IEnumerable<XElement> list, MemberComparer member)
		{
			if (State.BaseType == "System.Enum") {
				output.WriteLine ("<p>Removed value{0}:</p>", list.Count () > 1 ? "s" : String.Empty);
				output.WriteLine ($"<pre class='removed'>");
			} else {
				output.WriteLine ("<p>Removed {0}:</p>", list.Count () > 1 ? member.GroupName : member.ElementName);
				output.WriteLine ("<pre>");
			}
			IndentLevel++;
		}

		public override void RemoveMember (MemberComparer member, string obsolete, string description)
		{
			WriteIndentation ();
			output.Write ("<span class='removed'>");
			if (obsolete.Length > 0) {
				output.Write (obsolete);
				WriteIndentation ();
			}
			output.Write (description);
			output.WriteLine ("</span>");
		}

		public override void RenderObsoleteMessage (TextChunk chunk, MemberComparer member, string description, string optionalObsoleteMessage)
		{
			RenderAttribute (chunk, member, "Obsolete", description, optionalObsoleteMessage);
		}

		public override void RenderAttribute (TextChunk chunk, Comparer member, string attributeName, string description, params string [] attributeArguments)
		{
			var output = chunk.GetStringBuilder (this);
			output.Append ($"<span class='obsolete'>");
			output.Append ($"[{attributeName} (");
			foreach (var arg in attributeArguments) {
				output.Append ('"').Append (arg).Append ('"');
				if (arg != attributeArguments.Last ())
					output.Append (", ");
			}
			output.AppendLine (")]");
			output.Append (description);
			output.Append ("</span>");
		}

		public override void EndMemberRemoval ()
		{
			IndentLevel--;
			output.WriteLine ("</pre>"); ;
		}

		public override void DiffAddition (TextChunk chunk, string text)
		{
			var output = chunk.GetStringBuilder (this);
			output.Append ("<span class='added ");
			output.Append ("'>");
			output.Append (text);
			output.Append ("</span>");
		}

		public override void DiffModification (TextChunk chunk, string old, string @new)
		{
			var output = chunk.GetStringBuilder (this);
			if (old.Length > 0)
				DiffRemoval (chunk, old);
			// if (old.Length > 0 && @new.Length > 0)
			// 	output.Append (' ');
			if (@new.Length > 0)
				DiffAddition (chunk, @new);
		}

		public override void DiffRemoval (TextChunk chunk, string text)
		{
			var output = chunk.GetStringBuilder (this);
			output.Append ("<span class='removed'>");
			output.Append (text);
			output.Append ("</span>");
		}

		public override void Diff (ApiChange apichange)
		{
			output.Write ("<div>");
			foreach (var line in apichange.Member.GetStringBuilder (this).ToString ().Split (new [] { Environment.NewLine }, 0)) {
				output.Write ('\t');
				output.WriteLine (line);
			}
			output.Write ("</div>");
		}

		void AppendEncoded (string value)
		{
			System.Web.HttpUtility.HtmlEncode (value, output);
		}
	}
}
