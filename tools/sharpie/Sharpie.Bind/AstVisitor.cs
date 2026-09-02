// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using static ClangSharp.Interop.CX_DeclKind;

namespace Sharpie.Bind;

public abstract class AstVisitor {
	public BindingResult BindingResult { get; set; }

	public AstVisitor (BindingResult bindingResult)
	{
		this.BindingResult = bindingResult;
	}

	public Func<Decl, bool>? IsIncluded { get; set; }

	public virtual bool IsDeclIncluded (Decl? decl)
	{
		if (decl is null)
			return false;
		if (IsIncluded is null)
			return true;
		return IsIncluded (decl);
	}

	public virtual void VisitObjCTypeParamDecl (ObjCTypeParamDecl decl)
	{
		VisitTypedefNameDecl (decl);
	}

	public virtual void VisitTypedefNameDecl (TypedefNameDecl decl)
	{
		VisitTypeDecl (decl);
	}

	public virtual void VisitTypeDecl (TypeDecl decl)
	{
		VisitNamedDecl (decl);
	}

	public virtual void VisitNamedDecl (NamedDecl decl)
	{
		// VisitDecl (decl);
	}

	public virtual void VisitTagDecl (TagDecl decl)
	{
		VisitTypeDecl (decl);
	}

	public virtual void VisitConstantExpr (ConstantExpr stmt)
	{
		VisitFullExpr (stmt);
	}

	public virtual void VisitFullExpr (FullExpr stmt)
	{
		VisitExpr (stmt);
	}

	public virtual void VisitExpr (Expr stmt)
	{
		VisitValueStmt (stmt);
	}

	public virtual void VisitValueStmt (ValueStmt stmt)
	{
		VisitStmt (stmt);
	}

	public virtual void VisitUnaryExprOrTypeTraitExpr (UnaryExprOrTypeTraitExpr stmt)
	{
		VisitExpr (stmt);
	}

	public virtual void VisitDeclRefExpr (DeclRefExpr stmt)
	{
		VisitExpr (stmt);
	}

	public virtual void VisitCastExpr (CastExpr stmt)
	{
		VisitExpr (stmt);
	}

	public virtual void VisitExplicitCastExpr (ExplicitCastExpr stmt)
	{
		VisitCastExpr (stmt);
	}

	public virtual void VisitImplicitCastExpr (ImplicitCastExpr stmt)
	{
		VisitCastExpr (stmt);
	}

	public virtual void VisitParenExpr (ParenExpr stmt)
	{
		VisitExpr (stmt);
	}

	public virtual void VisitUnaryOperator (UnaryOperator stmt)
	{
		VisitExpr (stmt);
	}

	public virtual void VisitBinaryOperator (BinaryOperator stmt)
	{
		VisitExpr (stmt);
	}

	public virtual void VisitFloatingLiteral (FloatingLiteral stmt)
	{
		VisitExpr (stmt);
	}

	public virtual void VisitCharacterLiteral (CharacterLiteral stmt)
	{
		VisitExpr (stmt);
	}

	public virtual void VisitIntegerLiteral (IntegerLiteral stmt)
	{
		VisitExpr (stmt);
	}

	public virtual void VisitStmt (Stmt stmt)
	{
		VisitNode (stmt);
	}

	public virtual void VisitNode (object node)
	{
		// empty!
	}

	public virtual void VisitReferenceType (ReferenceType type)
	{
		VisitType (type);
	}

	public virtual void VisitLValueReferenceType (LValueReferenceType type)
	{
		VisitReferenceType (type);
	}

	public virtual void VisitFunctionNoProtoType (FunctionNoProtoType type)
	{
		VisitFunctionType (type);
	}

	public virtual void VisitAtomicType (AtomicType type)
	{
		VisitType (type);
	}

	public virtual void VisitComplexType (ComplexType type)
	{
		VisitType (type);
	}

	public virtual void VisitAdjustedType (AdjustedType type)
	{
		VisitType (type);

	}

	public virtual void VisitDecayedType (DecayedType type)
	{
		VisitAdjustedType (type);
	}

	public virtual void VisitArrayType (ArrayType type)
	{
		VisitType (type);
	}

	public virtual void VisitMacroQualifiedType (MacroQualifiedType type)
	{
		VisitType (type);
	}

	public virtual void VisitFunctionType (FunctionType type)
	{
		VisitType (type);
	}

	public virtual void VisitTypeWithKeyword (TypeWithKeyword type)
	{
		VisitType (type);
	}

	public virtual void VisitElaboratedType (ElaboratedType type)
	{
		VisitTypeWithKeyword (type);
	}

	public virtual void VisitTagType (TagType type)
	{
		VisitType (type);
	}

	public virtual void VisitRecordType (RecordType type)
	{
		VisitTagType (type);
	}

	public virtual void VisitEnumType (EnumType type)
	{
		VisitTagType (type);
	}

	public virtual void VisitObjCTypeParamType (ObjCTypeParamType type)
	{
		VisitType (type);
	}

	public virtual void VisitObjCObjectType (ObjCObjectType type)
	{
		VisitType (type);
	}

	public virtual void VisitObjCObjectPointerType (ObjCObjectPointerType type)
	{
		VisitType (type);
	}

	public virtual void VisitPointerType (PointerType type)
	{
		VisitType (type);
	}

	public virtual void VisitAttributedType (AttributedType type)
	{
		VisitType (type);
	}

	public virtual void VisitParenType (ParenType type)
	{
		VisitType (type);
	}

	public virtual void VisitType (ClangSharp.Type type)
	{
		VisitNode (type);
	}

	public virtual void VisitBuiltinType (BuiltinType type)
	{
		VisitType (type);
	}

	public virtual void VisitTypedefType (TypedefType type)
	{
		VisitType (type);
	}

	public virtual void VisitValueDecl (ValueDecl decl)
	{
		VisitNamedDecl (decl);
	}

	public virtual void VisitDeclaratorDecl (DeclaratorDecl decl)
	{
		VisitValueDecl (decl);
	}

	public virtual void VisitVarDecl (VarDecl decl)
	{
		VisitDeclaratorDecl (decl);
	}
	public virtual void VisitFieldDecl (FieldDecl decl)
	{
		VisitDeclaratorDecl (decl);
	}

	public virtual void VisitRecordDecl (RecordDecl decl)
	{
		VisitTagDecl (decl);
	}

	public virtual void VisitFunctionDecl (FunctionDecl decl)
	{
		VisitDeclaratorDecl (decl);
	}

	public virtual void VisitFunctionProtoType (FunctionProtoType type)
	{
		VisitFunctionType (type);
	}

	public virtual void VisitObjCContainerDecl (ObjCContainerDecl decl)
	{
		VisitNamedDecl (decl);
		Visit (decl.Decls);
	}

	public virtual void VisitObjCCategoryDecl (ObjCCategoryDecl decl)
	{
		VisitObjCContainerDecl (decl);
	}

	public virtual void VisitObjCMethodDecl (ObjCMethodDecl decl)
	{
		VisitNamedDecl (decl);
	}

	public virtual void VisitObjCPropertyDecl (ObjCPropertyDecl decl)
	{
		VisitNamedDecl (decl);
	}

	public virtual void VisitTypedefDecl (TypedefDecl decl)
	{
		VisitTypedefNameDecl (decl);
	}

	public virtual void VisitBlockPointerType (BlockPointerType type)
	{
		VisitType (type);
	}

	public virtual void VisitEnumConstantDecl (EnumConstantDecl decl)
	{
		VisitValueDecl (decl);
	}

	public virtual void VisitIndirectFieldDecl (IndirectFieldDecl decl)
	{
		// turns into an infinite loop
		// VisitValueDecl (decl);
	}
	public virtual void VisitObjCIvarDecl (ObjCIvarDecl decl)
	{
		VisitFieldDecl (decl);
	}

	public virtual void VisitConstantArrayType (ConstantArrayType type)
	{
		VisitArrayType (type);
	}

	public virtual void VisitIncompleteArrayType (IncompleteArrayType type)
	{
		VisitArrayType (type);
	}

	public virtual void VisitLinkageSpecDecl (LinkageSpecDecl decl)
	{
		// VisitDecl (decl);
	}

	public virtual void VisitVectorType (VectorType type)
	{
		VisitType (type);
	}

	public virtual void VisitExtVectorType (ExtVectorType type)
	{
		// VisitVectorType (type);
		VisitType (type);
	}

	public virtual void VisitAttr (Attr attr)
	{
	}

	public virtual void VisitRef (Ref @ref)
	{
	}

	public virtual void VisitEnumDecl (EnumDecl decl)
	{
		VisitTagDecl (decl);
	}

	public virtual void VisitObjCInterfaceDecl (ObjCInterfaceDecl decl)
	{
		VisitObjCContainerDecl (decl);
	}

	public virtual void VisitObjCProtocolDecl (ObjCProtocolDecl decl)
	{
		VisitObjCContainerDecl (decl);
	}

	public virtual void VisitNamespaceDecl (NamespaceDecl decl)
	{
		// VisitNamedDecl (decl);
	}

	public virtual void VisitTranslationUnitDecl (TranslationUnitDecl translationUnitDecl)
	{
		Visit (translationUnitDecl.Decls);
		Visit (translationUnitDecl.CursorChildren.Except (translationUnitDecl.Decls));
	}

	public void Visit (Cursor cursor)
	{
		if (cursor is Attr attr) {
			VisitAttr (attr);
		} else if (cursor is Decl decl) {
			if (IsDeclIncluded (decl))
				VisitDecl (decl);
		} else if (cursor is Ref @ref) {
			VisitRef (@ref);
		} else if (cursor is Stmt stmt) {
			BindingResult.ReportUnsupportedConstruct (cursor.Location, $"Statement visitation. CursorKind: '{cursor.CursorKindSpelling}'");
		} else if (cursor is MacroDefinitionRecord macroDefinitionRecord) {
			BindingResult.ReportUnsupportedConstruct (cursor.Location, $"Macro definition visitation. CursorKind: '{cursor.CursorKindSpelling}'");
		} else if (cursor is MacroExpansion macroExpansion) {
			BindingResult.ReportUnsupportedConstruct (cursor.Location, $"Macro expansion visitation. CursorKind: '{cursor.CursorKindSpelling}'");
		} else if (cursor is InclusionDirective inclusionDirective) {
			BindingResult.ReportUnsupportedConstruct (cursor.Location, $"Inclusion directive visitation. CursorKind: '{cursor.CursorKindSpelling}'");
		} else {
			BindingResult.ReportUnsupportedConstruct (cursor.Location, $"Visiting cursor of kind: '{cursor.CursorKindSpelling}'");
		}
	}

	protected void Visit (IEnumerable<Cursor> cursors)
	{
		foreach (var cursor in cursors) {
			Visit (cursor);
		}
	}

	public virtual void VisitDecl (Decl decl)
	{
		switch (decl.Kind) {
		case CX_DeclKind_AccessSpec:
			// Access specifications are also exposed as a queryable property
			// on the declarations they impact, so we don't need to do anything
			break;
		case CX_DeclKind_Empty:
			// Nothing to generate for empty declarations
			break;
		case CX_DeclKind_Friend:
			// Nothing to generate for friend declarations
			break;
		case CX_DeclKind_LinkageSpec:
			var lsd = (LinkageSpecDecl) decl;
			Visit (lsd.Decls);
			break;
		case CX_DeclKind_Namespace:
			VisitNamespaceDecl ((NamespaceDecl) decl);
			break;
		case CX_DeclKind_Enum:
			VisitEnumDecl ((EnumDecl) decl);
			break;
		case CX_DeclKind_Record:
		case CX_DeclKind_CXXRecord:
			VisitRecordDecl ((RecordDecl) decl);
			break;
		case CX_DeclKind_TypeAlias:
			// Nothing to generate for type alias declarations
			break;
		case CX_DeclKind_Typedef:
			VisitTypedefDecl ((TypedefDecl) decl);
			break;
		case CX_DeclKind_Using:
			// Using declarations only introduce existing members into
			// the current scope. There isn't an easy way to translate
			// this to C#, so we will ignore them for now.
			break;
		case CX_DeclKind_Field:
			VisitFieldDecl ((FieldDecl) decl);
			break;
		case CX_DeclKind_Function:
		case CX_DeclKind_CXXMethod:
		case CX_DeclKind_CXXConstructor:
		case CX_DeclKind_CXXDestructor:
		case CX_DeclKind_CXXConversion:
			VisitFunctionDecl ((FunctionDecl) decl);
			break;
		case CX_DeclKind_Var:
			VisitVarDecl ((VarDecl) decl);
			break;
		case CX_DeclKind_EnumConstant:
			VisitEnumConstantDecl ((EnumConstantDecl) decl);
			break;
		case CX_DeclKind_IndirectField:
			VisitIndirectFieldDecl ((IndirectFieldDecl) decl);
			break;
		case CX_DeclKind_PragmaComment:
			// Pragma comments can't be easily modeled in C#
			// We'll ignore them for now.
			break;
		case CX_DeclKind_StaticAssert:
			// Static asserts can't be easily modeled in C#
			// We'll ignore them for now.
			break;
		case CX_DeclKind_TranslationUnit:
			VisitTranslationUnitDecl ((TranslationUnitDecl) decl);
			break;
		case CX_DeclKind_ObjCInterface:
			VisitObjCInterfaceDecl ((ObjCInterfaceDecl) decl);
			break;
		case CX_DeclKind_ObjCProtocol:
			VisitObjCProtocolDecl ((ObjCProtocolDecl) decl);
			break;
		case CX_DeclKind_ObjCMethod:
			VisitObjCMethodDecl ((ObjCMethodDecl) decl);
			break;
		case CX_DeclKind_ObjCProperty:
			VisitObjCPropertyDecl ((ObjCPropertyDecl) decl);
			break;
		case CX_DeclKind_ObjCCategory:
			VisitObjCCategoryDecl ((ObjCCategoryDecl) decl);
			break;
		case CX_DeclKind_ObjCIvar:
			VisitObjCIvarDecl ((ObjCIvarDecl) decl);
			break;
		case CX_DeclKind_Import:
			// Imports are handled elsewhere; nothing to do here.
			break;
		case CX_DeclKind_BuiltinTemplate:
			break;
		case CX_DeclKind_ObjCCompatibleAlias:
			// Example:
			//     @compatibility_alias GCDeviceElement GCControllerElement;
			// Not sure if we can do something here, but for now we ignore it.
			break;
		default:
			BindingResult.ReportUnsupportedConstruct (decl.Location, $"Unsupported declaration: '{decl.DeclKindName}'.");
			break;
		}
	}

	public virtual void EndVisit ()
	{
	}
}
