// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Globalization;
using System.IO;

using Clang;

using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.CSharp;

using Sharpie.Bind.Attributes;

namespace Sharpie.Bind;

public class BindingTokenWriter : TokenWriter {
	readonly Stack<AstNode> nodeStack = new Stack<AstNode> ();

	bool inAttributeSection;
	bool inPropertyDeclaration;
	bool disableNewLine;
	int inShorthandAvailabilityAttrState;

	readonly TokenWriter writer;
	readonly ObjectiveCBinder Binder;

	public BindingTokenWriter (ObjectiveCBinder binder, TokenWriter writer)
	{
		if (writer is null)
			throw new ArgumentNullException (nameof (writer));

		this.Binder = binder;
		this.writer = writer;
	}

	public BindingTokenWriter (ObjectiveCBinder binder, TextWriter writer) : this (binder, new TextWriterTokenWriter (writer))
	{
	}

	public override void WritePrimitiveType (string type)
	{
		writer.WritePrimitiveType (type);
	}

	public override void Indent ()
	{
		writer.Indent ();
	}

	public override void Unindent ()
	{
		writer.Unindent ();
	}

	public override void WriteComment (CommentType commentType, string content)
	{
		writer.WriteComment (commentType, content);
	}

	public override void WritePreProcessorDirective (PreProcessorDirectiveType type, string argument)
	{
		writer.WritePreProcessorDirective (type, argument);
	}

	public override void StartNode (AstNode node)
	{
		nodeStack.Push (node);

		inPropertyDeclaration |= node is PropertyDeclaration;
		inAttributeSection |= node is AttributeSection;
		inShorthandAvailabilityAttrState = node is AvailabilityBaseAttribute &&
			((AvailabilityBaseAttribute) node).IsShorthand ? 1 : inShorthandAvailabilityAttrState;

		if (inPropertyDeclaration && disableNewLine && node is AttributeSection)
			WriteToken (Roles.Whitespace, " ");

		if (node is TypeDeclaration || node is DelegateDeclaration ||
			node is PropertyDeclaration || node is MethodDeclaration) {
			foreach (var nativeNode in node.Annotations.OfType<object> ()) {
				var comment = GenerateNativeCodeComment (Binder, nativeNode);
				if (!String.IsNullOrEmpty (comment))
					WriteComment (CommentType.SingleLine, " " + comment);
			}
		}

		writer.StartNode (node);
	}

	public override void EndNode (AstNode node)
	{
		if (node != nodeStack.Pop ())
			throw new Exception ("node stack is not balanced");

		if (node is PropertyDeclaration) {
			disableNewLine = false;
			inPropertyDeclaration = false;
			NewLine ();
		} else if ((node is TypeDeclaration || node is DelegateDeclaration) && node.NextSibling is not null) {
			NewLine ();
		} else if (node is AttributeSection) {
			inAttributeSection = false;
		} else if (node is AvailabilityBaseAttribute) {
			inShorthandAvailabilityAttrState = 0;
		}

		writer.EndNode (node);
	}

	public override void WriteIdentifier (Identifier identifier)
	{
		// write [return: foo] instead of [@return: foo] as NRefactory
		// always escapes keyword identifiers with '@', which is not ideal
		// in an attribute section context
		if (inAttributeSection && identifier.Name == "return") {
			WriteKeyword (Roles.Identifier, "return");
			return;
		}

		// we track this state to compress shorthand availability attributes
		// (e.g. [Mac (10,13,4)] instead of [Mac (10, 13, 4)]), however if we
		// have walked into named arguments inside the shorthand attribute,
		// stop compressing them so we end up with [Mac (10,13,4, onlyOn64: true)]
		// instead of [Mac (10,13,4,onlyOn64:true)].
		if (inShorthandAvailabilityAttrState == 2) {
			inShorthandAvailabilityAttrState = 0;
			writer.Space ();
		}

		writer.WriteIdentifier (identifier);
	}

	public override void WriteToken (Role role, string token)
	{
		if (inPropertyDeclaration) {
			if (role == Roles.LBrace)
				disableNewLine = true;
			else if (role == Roles.RBrace)
				WriteKeyword (Roles.Whitespace, " ");
		} else if (inShorthandAvailabilityAttrState == 1 && role == Roles.LPar)
			inShorthandAvailabilityAttrState = 2;

		writer.WriteToken (role, token);
	}

	public override void Space ()
	{
		if (inShorthandAvailabilityAttrState < 2)
			writer.Space ();
	}

	public override void WritePrimitiveValue (object value, string? literalValue = null)
	{
		if (literalValue is not null) {
			writer.WritePrimitiveValue (value, literalValue);
			return;
		}

		var isShiftRhs = false;

		var primitiveExpr = nodeStack.Pop ();
		try {
			if (nodeStack.Peek () is BinaryOperatorExpression binopExpr) {
				// https://github.com/xamarin/ObjectiveSharpie/issues/106
				// Always write out type suffix for literals on the LHS
				// of a shift operation. RHS will end up always coercing
				// into 'int' below, to ensure valid C#.
				switch (binopExpr.Operator) {
				case BinaryOperatorType.ShiftLeft:
				case BinaryOperatorType.ShiftRight:
					if (binopExpr.Left == primitiveExpr) {
						writer.WritePrimitiveValue (value);
						return;
					} else if (binopExpr.Right == primitiveExpr) {
						isShiftRhs = true;
					}
					break;
				}
			}
		} finally {
			nodeStack.Push (primitiveExpr);
		}

		var hexFormat = false;

		if (!isShiftRhs) {
			var enumDecl = nodeStack
				.OfType<TypeDeclaration> ()
				.FirstOrDefault (t => t.ClassType == ClassType.Enum);

			hexFormat = enumDecl is not null && enumDecl.HasAttribute<Attributes.FlagsAttribute> ();
		}

		// The base version will always write the 'u' and 'L'
		// constant suffixes even if they can fit safely into
		// an int and would be implicitly convertible. Convert
		// them to int if possible.
		// https://bugzilla.xamarin.com/show_bug.cgi?id=28037
		switch (value) {
		case ulong ulongValue when ulongValue <= long.MaxValue:
			if (ulongValue <= int.MaxValue)
				value = (int) ulongValue;
			else
				value = (long) ulongValue;
			break;
		case long longValue when longValue <= int.MaxValue:
			value = (int) longValue;
			break;
		case uint uintValue when uintValue <= int.MaxValue:
			value = (int) uintValue;
			break;
		}

		switch (value) {
		case sbyte _:
		case byte _:
		case short _:
		case ushort _:
		case int _:
		case uint _:
		case long _:
		case ulong _:
			string? format = null;
			if (hexFormat) {
				format = "x";
				literalValue = "0x";
			}

			literalValue += ((IFormattable) value).ToString (format, NumberFormatInfo.InvariantInfo);

			if (value is uint || value is ulong)
				literalValue += "u";

			if (value is long || value is ulong)
				literalValue += "L";

			break;
		}

		writer.WritePrimitiveValue (value, literalValue);
	}

	public override void WriteKeyword (Role role, string keyword)
	{
		if (role == PropertyDeclaration.GetKeywordRole || role == PropertyDeclaration.SetKeywordRole)
			WriteToken (Roles.Whitespace, " ");

		writer.WriteKeyword (role, keyword);
	}

	public override void NewLine ()
	{
		if (!disableNewLine)
			writer.NewLine ();
	}

	static string? GenerateNativeCodeComment (ObjectiveCBinder binder, object nativeNode)
	{
		var writer = new StringWriter ();
		var generator = new NativeCodeGenerator (binder, writer);

		// note that we do not want to call nativeDecl.Accept (generator) since
		// that will cause the generator to visit DeclContext nodes (e.g. members
		// of an interface). Calling Visit* methods directly will not traverse
		// children so we will end up with just the toplevel definition
		if (nativeNode is ObjCContainerDecl)
			generator.VisitObjCContainerDecl ((ObjCContainerDecl) nativeNode);
		else if (nativeNode is ObjCMethodDecl)
			generator.VisitObjCMethodDecl ((ObjCMethodDecl) nativeNode);
		else if (nativeNode is ObjCPropertyDecl)
			generator.VisitObjCPropertyDecl ((ObjCPropertyDecl) nativeNode);
		else if (nativeNode is FunctionDecl)
			generator.VisitFunctionDecl ((FunctionDecl) nativeNode);
		else if (nativeNode is TypedefDecl)
			generator.VisitTypedefDecl ((TypedefDecl) nativeNode);
		else if (nativeNode is BlockPointerType)
			generator.VisitBlockPointerType ((BlockPointerType) nativeNode);
		else if (nativeNode is VarDecl)
			generator.VisitVarDecl ((VarDecl) nativeNode);
		else
			return null;

		return writer.ToString ().Trim ();
	}
}
