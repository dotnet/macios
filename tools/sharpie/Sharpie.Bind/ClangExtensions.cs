// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Diagnostics;
using ICSharpCode.NRefactory.CSharp;
using Mono.CSharp;

namespace Sharpie.Bind;

public static class ClangExtensions {
	public static bool IsObjCRequiresSuperAttr (this IEnumerable<Attr> attrs)
	{
		return IsAttribute (attrs, CX_AttrKind.CX_AttrKind_ObjCRequiresSuper);
	}

	public static bool IsDesignatedInitializerAttr (this IEnumerable<Attr> attrs)
	{
		return IsAttribute (attrs, CX_AttrKind.CX_AttrKind_ObjCDesignatedInitializer);
	}

	public static bool IsFlagEnumAttr (this IEnumerable<Attr> attrs)
	{
		return IsAttribute (attrs, CX_AttrKind.CX_AttrKind_FlagEnum);
	}

	public static bool IsUnavailableAttr (this IEnumerable<Attr> attrs)
	{
		return IsAttribute (attrs, CX_AttrKind.CX_AttrKind_Unavailable);
	}

	public static bool IsAnyAvailabilityAttributeUnavailable (this IEnumerable<Attr> attrs)
	{
		return GetAvailabilityAttributes (attrs).Any (v => v.AvailabilityAttributeUnavailable);
	}

	public static IEnumerable<Attr> GetAvailabilityAttributes (this IEnumerable<Attr> attrs)
	{
		foreach (var attr in attrs) {
			if (attr.Kind == CX_AttrKind.CX_AttrKind_Availability)
				yield return attr;
		}
	}

	public static bool TryGetObjCRuntimeName (this IEnumerable<Attr> attrs, [NotNullWhen (true)] out string? runtimeName)
	{
		var attr = attrs.FirstOrDefault (v => v.Kind == CX_AttrKind.CX_AttrKind_ObjCRuntimeName);
		if (attr is null) {
			runtimeName = null;
			return false;
		}
		runtimeName = attr.ObjCRuntimeNameMetadataName;
		return true;
	}

	public static bool IsAttribute (this IEnumerable<Attr> attrs, CX_AttrKind kind)
	{
		foreach (var a in attrs) {
			if (a.Kind == kind)
				return true;
		}
		return false;
	}

	public static IEnumerable<Attr> GetAttrs (this IEnumerable<Attr> attrs, CX_AttrKind kind)
	{
		foreach (var a in attrs) {
			if (a.Kind == kind)
				yield return a;
		}
	}

	public static AstType? Bind (this ClangSharp.Type type, BindingResult bindingResult)
	{
		ParameterModifier parameterModifier;
		return type.Bind (bindingResult, false, out parameterModifier);
	}

	public static AstType? Bind (this ClangSharp.Type type,
	BindingResult bindingResult,
	out ParameterModifier parameterModifier)
	{
		return type.Bind (bindingResult, true, out parameterModifier);
	}

	static AstType? Bind (this ClangSharp.Type type,
		BindingResult bindingResult,
		bool parameterContext, out ParameterModifier parameterModifier)
	{
		if (type is null) {
			parameterModifier = ParameterModifier.None;
			return null;
		}

		var binder = new TypeBinder (bindingResult);
		type.Accept (binder);
		binder.Resolve ();
		parameterModifier = binder.ParameterModifier;
		return parameterContext ? binder.ParameterType : binder.Type;
	}

	public static string ToCSharpName (this Selector selector)
	{
		return selector.GetNameForSlot (0).UCFirst ();
	}

	public static ArgumentSemantic ToArgumentSemantic (this ObjCPropertyAttributeKind attr)
	{
		if ((attr & ObjCPropertyAttributeKind.Retain) != 0)
			return ArgumentSemantic.Retain;
		else if ((attr & ObjCPropertyAttributeKind.Copy) != 0)
			return ArgumentSemantic.Copy;
		else if ((attr & ObjCPropertyAttributeKind.Assign) != 0)
			return ArgumentSemantic.Assign;
		else if ((attr & ObjCPropertyAttributeKind.Weak) != 0)
			return ArgumentSemantic.Weak;
		else if ((attr & ObjCPropertyAttributeKind.Strong) != 0)
			return ArgumentSemantic.Strong;
		else if ((attr & ObjCPropertyAttributeKind.UnsafeUnretained) != 0)
			return ArgumentSemantic.UnsafeUnretained;
		else
			return ArgumentSemantic.None;
	}

	public static BinaryOperatorType ToBinaryOperatorType (this CXBinaryOperatorKind kind)
	{
		switch (kind) {
		case CXBinaryOperatorKind.CXBinaryOperator_Mul:
			return BinaryOperatorType.Multiply;
		case CXBinaryOperatorKind.CXBinaryOperator_Div:
			return BinaryOperatorType.Divide;
		case CXBinaryOperatorKind.CXBinaryOperator_Rem:
			return BinaryOperatorType.Modulus;
		case CXBinaryOperatorKind.CXBinaryOperator_Add:
			return BinaryOperatorType.Add;
		case CXBinaryOperatorKind.CXBinaryOperator_Sub:
			return BinaryOperatorType.Subtract;
		case CXBinaryOperatorKind.CXBinaryOperator_Shl:
			return BinaryOperatorType.ShiftLeft;
		case CXBinaryOperatorKind.CXBinaryOperator_Shr:
			return BinaryOperatorType.ShiftRight;
		case CXBinaryOperatorKind.CXBinaryOperator_LT:
			return BinaryOperatorType.LessThan;
		case CXBinaryOperatorKind.CXBinaryOperator_GT:
			return BinaryOperatorType.GreaterThan;
		case CXBinaryOperatorKind.CXBinaryOperator_LE:
			return BinaryOperatorType.LessThanOrEqual;
		case CXBinaryOperatorKind.CXBinaryOperator_GE:
			return BinaryOperatorType.GreaterThanOrEqual;
		case CXBinaryOperatorKind.CXBinaryOperator_EQ:
			return BinaryOperatorType.Equality;
		case CXBinaryOperatorKind.CXBinaryOperator_NE:
			return BinaryOperatorType.InEquality;
		case CXBinaryOperatorKind.CXBinaryOperator_And:
			return BinaryOperatorType.BitwiseAnd;
		case CXBinaryOperatorKind.CXBinaryOperator_Xor:
			return BinaryOperatorType.ExclusiveOr;
		case CXBinaryOperatorKind.CXBinaryOperator_Or:
			return BinaryOperatorType.BitwiseOr;
		case CXBinaryOperatorKind.CXBinaryOperator_LAnd:
			return BinaryOperatorType.ConditionalAnd;
		case CXBinaryOperatorKind.CXBinaryOperator_LOr:
			return BinaryOperatorType.ConditionalOr;
		case CXBinaryOperatorKind.CXBinaryOperator_PtrMemD:
		case CXBinaryOperatorKind.CXBinaryOperator_PtrMemI:
		case CXBinaryOperatorKind.CXBinaryOperator_Assign:
		case CXBinaryOperatorKind.CXBinaryOperator_MulAssign:
		case CXBinaryOperatorKind.CXBinaryOperator_DivAssign:
		case CXBinaryOperatorKind.CXBinaryOperator_RemAssign:
		case CXBinaryOperatorKind.CXBinaryOperator_AddAssign:
		case CXBinaryOperatorKind.CXBinaryOperator_SubAssign:
		case CXBinaryOperatorKind.CXBinaryOperator_ShlAssign:
		case CXBinaryOperatorKind.CXBinaryOperator_ShrAssign:
		case CXBinaryOperatorKind.CXBinaryOperator_AndAssign:
		case CXBinaryOperatorKind.CXBinaryOperator_XorAssign:
		case CXBinaryOperatorKind.CXBinaryOperator_OrAssign:
		case CXBinaryOperatorKind.CXBinaryOperator_Comma:
		default:
			throw new Exception ("Unsupported BinaryOperatorKind: " + kind);
		}
	}

	public static UnaryOperatorType ToUnaryOperatorType (this CXUnaryOperatorKind kind)
	{
		switch (kind) {
		case CXUnaryOperatorKind.CXUnaryOperator_PostInc:
			return UnaryOperatorType.PostIncrement;
		case CXUnaryOperatorKind.CXUnaryOperator_PostDec:
			return UnaryOperatorType.PostDecrement;
		case CXUnaryOperatorKind.CXUnaryOperator_PreInc:
			return UnaryOperatorType.Increment;
		case CXUnaryOperatorKind.CXUnaryOperator_PreDec:
			return UnaryOperatorType.Decrement;
		case CXUnaryOperatorKind.CXUnaryOperator_AddrOf:
			return UnaryOperatorType.AddressOf;
		case CXUnaryOperatorKind.CXUnaryOperator_Deref:
			return UnaryOperatorType.Dereference;
		case CXUnaryOperatorKind.CXUnaryOperator_Plus:
			return UnaryOperatorType.Plus;
		case CXUnaryOperatorKind.CXUnaryOperator_Minus:
			return UnaryOperatorType.Minus;
		case CXUnaryOperatorKind.CXUnaryOperator_Not:
			return UnaryOperatorType.BitNot;
		case CXUnaryOperatorKind.CXUnaryOperator_LNot:
			return UnaryOperatorType.Not;
		case CXUnaryOperatorKind.CXUnaryOperator_Real:
		case CXUnaryOperatorKind.CXUnaryOperator_Imag:
		case CXUnaryOperatorKind.CXUnaryOperator_Extension:
		default:
			throw new ArgumentOutOfRangeException ();
		}
	}

	extension(ClangSharp.Type self) {
		public void Accept (AstVisitor visitor)
		{
			if (self is BlockPointerType blockPointerType)
				visitor.VisitBlockPointerType (blockPointerType);
			else if (self is FunctionProtoType functionProtoType)
				visitor.VisitFunctionProtoType (functionProtoType);
			else if (self is BuiltinType builtinType)
				visitor.VisitBuiltinType (builtinType);
			else if (self is AttributedType attributedType)
				visitor.VisitAttributedType (attributedType);
			else if (self is ElaboratedType elaboratedType)
				visitor.VisitElaboratedType (elaboratedType);
			else if (self is PointerType pointerType)
				visitor.VisitPointerType (pointerType);
			else if (self is ObjCObjectPointerType objCObjectPointerType)
				visitor.VisitObjCObjectPointerType (objCObjectPointerType);
			else if (self is ObjCObjectType objCObjectType)
				visitor.VisitObjCObjectType (objCObjectType);
			else if (self is MacroQualifiedType macroQualifiedType)
				visitor.VisitMacroQualifiedType (macroQualifiedType);
			else if (self is ObjCTypeParamType objCTypeParamType)
				visitor.VisitObjCTypeParamType (objCTypeParamType);
			else if (self is TagType tagType)
				visitor.VisitTagType (tagType);
			else if (self is TypedefType typedefType)
				visitor.VisitTypedefType (typedefType);
			else if (self is ConstantArrayType constantArrayType)
				visitor.VisitConstantArrayType (constantArrayType);
			else if (self is IncompleteArrayType incompleteArrayType)
				visitor.VisitIncompleteArrayType (incompleteArrayType);
			else if (self is VectorType vectorType)
				visitor.VisitVectorType (vectorType);
			else if (self is ExtVectorType extVectorType)
				visitor.VisitExtVectorType (extVectorType);
			else if (self is ComplexType complexType)
				visitor.VisitComplexType (complexType);
			else if (self is LValueReferenceType lValueReferenceType)
				visitor.VisitLValueReferenceType (lValueReferenceType);
			else if (self is FunctionNoProtoType functionNoProtoType)
				visitor.VisitFunctionNoProtoType (functionNoProtoType);
			else if (self is AtomicType atomicType) {
				visitor.VisitAtomicType (atomicType);
			} else
				visitor.BindingResult.ReportUnsupportedConstruct (self.AsTagDecl?.Location, $"Type kind not supported: {self.GetType ().Name}");
		}
	}

	extension(Decl self) {
		public bool GetIsLastInContext (AstVisitor visitor)
		{
			throw visitor.BindingResult.CreateInternalError ("Need a test case for this scenario: is last in context.");
		}

		public void Accept (AstVisitor visitor)
		{
			visitor.BindingResult.ReportUnsupportedConstruct (self.Location, $"Accepting decl of type {self.GetType ().Name}");
		}

		public bool TryGetPresumedLoc ([NotNullWhen (true)] out Clang.PresumedLoc? presumedLoc)
		{
			return self.Location.TryGetPresumedLocation (out presumedLoc);
		}

		public bool TryGetAttr (CX_AttrKind kind, [NotNullWhen (true)] out Attr? attr)
		{
			attr = self.GetAttrs (kind).SingleOrDefault ();
			return attr is not null;
		}

		public IEnumerable<Attr> GetAttrs (CX_AttrKind kind)
		{
			foreach (var a in self.Attrs) {
				if (a.Kind == kind)
					yield return a;
			}
		}
	}

	extension(CXSourceLocation self) {
		public bool IsInvalid {
			get {
				return self.int_data == 0;
			}
		}

		public bool TryGetPresumedLocation ([NotNullWhen (true)] out Clang.PresumedLoc? presumedLoc)
		{
			self.GetPresumedLocation (out var fileName, out var line, out var column);
			presumedLoc = new Clang.PresumedLoc (fileName, line, column);
			return true;
		}

		public Clang.PresumedLoc? PresumedLoc {
			get {
				self.TryGetPresumedLocation (out var loc);
				return loc;
			}
		}
	}

	extension(ObjCContainerDecl self) {
		public IEnumerable<ObjCProtocolDecl> Protocols {
			get {
				if (self is ObjCInterfaceDecl interfaceDecl)
					return interfaceDecl.Protocols;
				if (self is ObjCProtocolDecl protocolDecl)
					return protocolDecl.Protocols;
				if (self is ObjCCategoryDecl categoryDecl)
					return categoryDecl.Protocols;

				throw new NotImplementedException ($"Unknown ObjC container type: {self.GetType ()}");
			}
		}
	}

	extension(ObjCMethodDecl self) {
		public bool IsDesignatedInitializer {
			get {
				if (self.MethodFamily != ObjCMethodFamily.Init)
					return false;

				return self.Attrs.IsDesignatedInitializerAttr ();
			}
		}

		public Selector GetSelector ()
		{
			return new Selector (self.Selector);
		}
	}

	extension(ObjCPropertyDecl self) {
		public ArgumentSemantic ArgumentSemantic {
			get {
				return self.GetPropertyAttributes ().ToArgumentSemantic ();
			}
		}
	}

	extension(Expr self) {
		public void Accept (AstVisitor visitor)
		{
			if (self is IntegerLiteral integerLiteral)
				visitor.VisitIntegerLiteral (integerLiteral);
			else if (self is FloatingLiteral floatingLiteral)
				visitor.VisitFloatingLiteral (floatingLiteral);
			else if (self is BinaryOperator binaryOperator)
				visitor.VisitBinaryOperator (binaryOperator);
			else if (self is UnaryOperator unaryOperator)
				visitor.VisitUnaryOperator (unaryOperator);
			else if (self is ParenExpr parenExpr)
				visitor.VisitParenExpr (parenExpr);
			else if (self is ImplicitCastExpr implicitCastExpr)
				visitor.VisitImplicitCastExpr (implicitCastExpr);
			else if (self is ExplicitCastExpr explicitCastExpr)
				visitor.VisitExplicitCastExpr (explicitCastExpr);
			else if (self is CastExpr castExpr)
				visitor.VisitCastExpr (castExpr);
			else if (self is UnaryExprOrTypeTraitExpr unaryExprOrTypeTraitExpr)
				visitor.VisitUnaryExprOrTypeTraitExpr (unaryExprOrTypeTraitExpr);
			else if (self is FullExpr fullExpr)
				visitor.VisitFullExpr (fullExpr);
			else if (self is DeclRefExpr declRefExpr)
				visitor.VisitDeclRefExpr (declRefExpr);
			else if (self is CharacterLiteral characterLiteralExpr)
				visitor.VisitCharacterLiteral (characterLiteralExpr);
			else
				visitor.BindingResult.ReportUnsupportedConstruct (self.Location, $"Expression kind not supported: {self.GetType ().Name}");
		}

		public bool EvaluateAsInt (out long signedValue, out ulong unsignedValue)
		{
			var e = self.Handle.Evaluate;
			unsignedValue = e.AsUnsigned;
			signedValue = e.AsLongLong;
			return true;
		}
	}

	extension(Cursor self) {
		public bool TryGetPresumedLoc (out Clang.PresumedLoc? presumedLoc)
		{
			return self.Location.TryGetPresumedLocation (out presumedLoc);
		}

		public Clang.PresumedLoc? PresumedLoc {
			get {
				return self.Location.PresumedLoc;
			}
		}
	}

	extension(NamedDecl self) {
		public bool GetIsUnnamedOrAnonymous (BindingResult bindingResult)
		{
			var name = self.Name;

			if (string.IsNullOrEmpty (name))
				return true;

			if (name.IndexOf ("(unnamed at ", StringComparison.Ordinal) != -1 ||
				name.IndexOf ("(unnamed struct at ", StringComparison.Ordinal) != -1 ||
				name.IndexOf ("(unnamed enum at ", StringComparison.Ordinal) != -1 ||
				name.IndexOf ("(unnamed union at ", StringComparison.Ordinal) != -1 ||
				name.IndexOf ("(anonymous at ", StringComparison.Ordinal) != -1)
				return true;

			if (name.Contains ("unnamed ") || name.Contains ("anonymous "))
				bindingResult.ReportUnsupportedConstruct (self.Location, $"found 'unnamed' or 'anonymous' in name '{name}'");

			return false;
		}

		public bool IsUIAppearanceSelector {
			get {
				foreach (var attr in self.Attrs) {
					if (attr.Kind != CX_AttrKind.CX_AttrKind_Annotate)
						continue;

					return string.Equals (attr.Spelling, "ui_appearance_selector", StringComparison.OrdinalIgnoreCase);
				}

				return false;
			}
		}
	}

	extension(Attr self) {
		public VersionTuple Introduced => self.AvailabilityAttributeIntroduced ?? default;
		public VersionTuple Deprecated => self.AvailabilityAttributeDeprecated ?? default;
		public VersionTuple Obsoleted => self.AvailabilityAttributeObsoleted ?? default;
	}
}

