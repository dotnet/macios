// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Sharpie.Bind;
using Sharpie.Writing;

namespace Clang;

public class NativeCodeGenerator : AstVisitor {
	readonly Stack<ClangSharp.Type> pointerStack = new Stack<ClangSharp.Type> ();
	readonly Stack<string> contextNames = new Stack<string> ();
	readonly CodeWriter writer;
	int indentLevel;

	public NativeCodeGenerator (ObjectiveCBinder binder, TextWriter writer)
		: base (binder.BindingResult)
	{
		if (writer is null)
			throw new ArgumentNullException ("writer");

		this.writer = new CodeWriter (writer);
	}

	public NativeCodeGenerator (ObjectiveCBinder binder, CodeWriter writer)
		: base (binder.BindingResult)
	{
		if (writer is null)
			throw new ArgumentNullException ("writer");

		this.writer = writer;
	}

	public override void VisitParenType (ParenType type)
	{
		type.InnerType.Accept (this);
	}

	public override void VisitPointerType (PointerType type)
	{
		pointerStack.Push (type);
		type.PointeeType.Accept (this);
		if (pointerStack.PeekOrDefault () == type)
			pointerStack.Pop ();
	}

	public override void VisitBlockPointerType (BlockPointerType type)
	{
		pointerStack.Push (type);
		type.PointeeType.Accept (this);
		if (pointerStack.PeekOrDefault () == type)
			pointerStack.Pop ();
	}

	public override void VisitFunctionType (FunctionType type)
	{
		var pointerType = pointerStack.PeekOrDefault ();
		if (contextNames.Count == 0 || pointerType is null)
			return;

		writer.Write (Term.TypeReferenceName, type.ReturnType.ToString ());
		writer.Write (" (");
		if (pointerType is PointerType)
			writer.Write ("* ");
		else if (pointerType is BlockPointerType)
			writer.Write ("^");
		writer.Write (Term.MethodName, contextNames.Pop ());
		writer.Write (")(");

		var protoType = type as FunctionProtoType;
		if (protoType is null) {
			writer.Write (")");
			return;
		}

		var i = 0;
		foreach (var paramType in protoType.ParamTypes) {
			if (i++ > 0)
				writer.Write (", ");
			writer.Write (Term.TypeReferenceName, paramType.ToString ());
		}

		if (protoType.IsVariadic)
			writer.Write (", ...");

		writer.Write (")");
	}

	public override void VisitEnumConstantDecl (EnumConstantDecl decl)
	{
		writer.WriteIndent (indentLevel);
		writer.Write (decl.Name);

		if (decl.InitExpr is not null) {
			var initExpr = decl.InitExpr.ToString ();
			if (initExpr is not null) {
				writer.Write (" ");
				writer.Write ("=");
				writer.Write (" ");
				writer.Write (Term.NumberLiteral, initExpr);
			}
		}

		if (!decl.GetIsLastInContext (this))
			writer.Write (",");

		writer.WriteLine ();
	}

	public override void VisitVarDecl (VarDecl decl)
	{
		if (decl.IsExternC || decl.LinkageInternal == CXLinkageKind.CXLinkage_External) {
			writer.Write (Term.Keyword, "extern");
			writer.Write (" ");
		}

		writer.Write (Term.TypeName, decl.Type.ToString ());
		writer.Write (" ");
		writer.Write (decl.Name);

		VisitAttrs (decl.Attrs);

		writer.Write (";");
		writer.WriteLine ();
	}

	public override void VisitFieldDecl (FieldDecl decl)
	{
		writer.WriteIndent (indentLevel);
		writer.Write (Term.TypeReferenceName, decl.Type.ToString ());
		writer.Write (" ");
		writer.Write (decl.Name);
		writer.Write (";");
		writer.WriteLine ();
	}

	public override void VisitTypedefDecl (TypedefDecl decl)
	{
		contextNames.Push (decl.Name);

		writer.WriteIndent (indentLevel);
		writer.Write (Term.Keyword, "typedef");
		writer.Write (" ");

		decl.UnderlyingType.Accept (this);

		// we didn't visit a block or function pointer (which would have
		// popped this typedef decl), so write the typedef as normal
		if (contextNames.PeekOrDefault () == decl.Name) {
			writer.Write (Term.TypeReferenceName, decl.UnderlyingType.ToString ());
			writer.Write (" ");
			writer.Write (decl.Name);
			contextNames.Pop ();
		}

		writer.Write (";");
		writer.WriteLine ();
	}

	public static string FunctionDeclSignatureToString (ObjectiveCBinder binder, FunctionDecl decl,
		bool withTypes = true,
		bool forceNames = false,
		bool withExtern = false,
		bool parametersOnly = false)
	{
		var writer = new StringWriter ();
		var gen = new NativeCodeGenerator (binder, writer);
		gen.WriteFunctionDeclSignature (decl, withTypes, forceNames, withExtern, parametersOnly);
		return writer.ToString ();
	}

	void WriteFunctionDeclSignature (FunctionDecl decl,
		bool withTypes = true,
		bool forceNames = false,
		bool withExtern = true,
		bool parametersOnly = false)
	{
		if (!parametersOnly) {
			if (decl.IsExternC && withExtern) {
				writer.Write (Term.Keyword, "extern");
				writer.Write (" ");
			}

			writer.Write (Term.TypeReferenceName, decl.ReturnType.ToString ());
			writer.Write (" ");
			writer.Write (Term.MethodName, decl.Name);
			writer.Write (" ");
			writer.Write ("(");
		}

		int i = 0;
		foreach (var param in decl.Parameters) {
			if (i++ > 0) {
				writer.Write (",");
				writer.Write (" ");
			}

			var name = param.Name is null && forceNames
				? String.Format ("_{0}", i)
				: param.Name;

			if (!withTypes) {
				writer.Write (name);
				continue;
			}

			var typeSpelling = param.Type.ToString ();

			if (name is null) {
				writer.Write (Term.TypeReferenceName, typeSpelling);
				continue;
			}

			contextNames.Push (name);

			param.Type.Accept (this);

			if (contextNames.PeekOrDefault () == name) {
				writer.Write (Term.TypeReferenceName, typeSpelling);
				if (typeSpelling [typeSpelling.Length - 1] != '*')
					writer.Write (" ");
				writer.Write (Term.PropertyName, name);
				contextNames.Pop ();
			}
		}

		if (decl.IsVariadic) {
			writer.Write (",");
			writer.Write (" ");
			writer.Write ("...");
		}

		if (!parametersOnly)
			writer.Write (")");
	}

	public override void VisitFunctionDecl (FunctionDecl decl)
	{
		WriteFunctionDeclSignature (decl);

		VisitAttrs (decl.Attrs);

		writer.Write (";");
		writer.WriteLine ();
	}

	public override void VisitObjCTypeParamDecl (ObjCTypeParamDecl decl)
	{
		switch (decl.Variance) {
		case ObjCTypeParamVariance.Covariant:
			writer.Write (Term.ObjCKeyword, "__covariant");
			writer.Write (" ");
			break;
		case ObjCTypeParamVariance.Contravariant:
			writer.Write (Term.ObjCKeyword, "__contravariant");
			writer.Write (" ");
			break;
		}

		writer.Write (Term.TypeName, decl.Name);

		if (decl.HasExplicitBound) {
			writer.Write (" : ");
			writer.Write (Term.TypeName, decl.UnderlyingType.ToString ());
		}
	}

	public override void VisitObjCContainerDecl (ObjCContainerDecl decl)
	{
		var proto = decl as ObjCProtocolDecl;
		var iface = decl as ObjCInterfaceDecl;
		var category = decl as ObjCCategoryDecl;
		var isForwardDeclared = false;
		string keyword;

		if (proto is not null && proto.IsThisDeclarationADefinition)
			keyword = "@protocol";
		else if ((iface is not null && iface.IsThisDeclarationADefinition) || category is not null)
			keyword = "@interface";
		else {
			isForwardDeclared = true;
			keyword = "@class";
		}

		var typeParams = iface?.TypeParamList?.ToArray ();

		if (typeParams is not null && typeParams.Length > 0)
			writer.Write ("audit-objc-generics: ");

		writer.Write (Term.ObjCKeyword, keyword);
		writer.Write (" ");
		writer.Write (Term.TypeDefinitionName, decl.Name);

		for (int i = 0; typeParams is not null && i < typeParams.Length; i++) {
			if (i == 0)
				writer.Write ("<");
			else if (i > 0)
				writer.Write (", ");

			VisitObjCTypeParamDecl (typeParams [i]);
			// typeParams[i].Accept(this);

			if (i == typeParams.Length - 1)
				writer.Write (">");
		}

		if (isForwardDeclared) {
			writer.Write (";");
			writer.WriteLine ();
			if (!decl.GetIsLastInContext (this))
				writer.WriteLine ();
			return;
		}

		if (iface is not null && iface.SuperClass is not null) {
			writer.Write (" ");
			writer.Write (":");
			writer.Write (" ");
			writer.Write (Term.TypeReferenceName, iface.SuperClass.Name);
		}

		if (category is not null && category.ClassInterface is not null) {
			writer.Write (" ");
			writer.Write ("(");
			writer.Write (Term.TypeReferenceName, category.ClassInterface.Name);
			writer.Write (")");
		}

		var protoCount = 0;

		foreach (var conformingToProto in decl.Protocols) {
			if (protoCount++ == 0) {
				writer.Write (" ");
				writer.Write ("<");
			} else {
				writer.Write (",");
				writer.Write (" ");
			}

			writer.Write (Term.TypeReferenceName, conformingToProto.Name);
		}

		if (protoCount > 0)
			writer.Write (">");

		writer.WriteLine ();
		indentLevel++;
	}

	void WriteObjCImplementationControl (Decl decl)
	{
		if (decl.DeclContext is ObjCProtocolDecl protocolDecl) {
			if (decl.Handle.IsObjCOptional)
				writer.Write (Term.ObjCKeyword, "@optional");
			else
				writer.Write (Term.ObjCKeyword, "@required");
			writer.Write (" ");
		}
	}

	public override void VisitObjCMethodDecl (ObjCMethodDecl decl)
	{
		if (decl.IsPropertyAccessor)
			return;

		writer.WriteIndent (indentLevel);

		WriteObjCImplementationControl (decl);

		writer.Write (decl.IsClassMethod ? "+" : "-");

		writer.Write ("(");
		writer.Write (Term.TypeReferenceName, decl.ReturnType.ToString ());
		writer.Write (")");

		int i = 0;
		foreach (var param in decl.Parameters) {
			if (i > 0)
				writer.Write (" ");
			var selSlot = decl.GetSelector ().GetNameForSlot ((uint) i);
			writer.Write (Term.MethodName, selSlot + ":");
			writer.Write ("(");
			writer.Write (Term.TypeReferenceName, param.Type.ToString ());
			writer.Write (")");
			writer.Write (Term.ArgumentName, param.Name);
			i++;
		}

		if (i == 0)
			writer.Write (Term.MethodName, decl.Name);

		if (decl.Handle.IsVariadic) {
			writer.Write (",");
			writer.Write (" ");
			writer.Write ("...");
		}

		VisitAttrs (decl.Attrs);

		writer.Write (";");
		writer.WriteLine ();
	}

	public override void VisitObjCPropertyDecl (ObjCPropertyDecl decl)
	{
		writer.WriteIndent (indentLevel);

		WriteObjCImplementationControl (decl);

		writer.Write (Term.ObjCKeyword, "@property");
		writer.Write (" ");

		var attrs = decl.PropertyAttributes;
		var attrWritten = false;

		foreach (var attr in Enum.GetValues<CXObjCPropertyAttrKind> ()) {
			if (attr == CXObjCPropertyAttrKind.CXObjCPropertyAttr_noattr || !attrs.HasFlag (attr))
				continue;

			if (attrWritten) {
				writer.Write (",");
				writer.Write (" ");
			} else {
				writer.Write ("(");
			}

			writer.Write (Term.Keyword, attr.ToString ().Replace ("CXObjCPropertyAttr_", "").ToLowerInvariant ());

			switch (attr) {
			case CXObjCPropertyAttrKind.CXObjCPropertyAttr_getter:
			case CXObjCPropertyAttrKind.CXObjCPropertyAttr_setter:
				writer.Write (" ");
				writer.Write ("=");
				writer.Write (" ");
				writer.Write (Term.MethodName, attr == CXObjCPropertyAttrKind.CXObjCPropertyAttr_getter
					? decl.GetterMethodDecl.Name
					: decl.SetterMethodDecl.Name);
				break;
			}

			attrWritten = true;
		}

		if (decl.GetPropertyAttributes ().HasFlag (ObjCPropertyAttributeKind.NullResettable)) {
			if (attrWritten) {
				writer.Write (",");
				writer.Write (" ");
			} else {
				writer.Write ("(");
			}
			writer.Write (Term.Keyword, "null_resettable");
			attrWritten = true;
		}


		if (attrWritten) {
			writer.Write (")");
			writer.Write (" ");
		}

		// set in case the property type is a block
		contextNames.Push (decl.Name);

		decl.Type.Accept (this);

		// we didn't visit a block (which would have nulled
		// out blockName), so write the property as normal
		if (contextNames.PeekOrDefault () == decl.Name) {
			writer.Write (Term.TypeReferenceName, decl.Type.ToString ());
			writer.Write (" ");
			writer.Write (Term.PropertyName, decl.Name);
			contextNames.Pop ();
		}

		VisitAttrs (decl.Attrs);

		writer.Write (";");
		writer.WriteLine ();
	}

	void VisitAttrs (IEnumerable<Attr> attrs)
	{
		foreach (var attr in attrs) {
			writer.Write (" ");
			VisitAttr (attr);
		}
	}

	public override void VisitAttr (Attr attr)
	{
		if (attr.Kind == CX_AttrKind.CX_AttrKind_CFAuditedTransfer)
			return; // pretty-printing this attribute makes clang assert.

		var str = attr.PrettyPrint ()?.Trim ();
		if (!string.IsNullOrEmpty (str))
			writer.Write (str);
	}
}
