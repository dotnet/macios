// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

using Sharpie.Bind.Attributes;
using Sharpie.Bind.Types;

namespace Sharpie.Bind;

public sealed class BindingGenerator : AstVisitor {
	readonly DocumentSyntaxTree root = new DocumentSyntaxTree ();
	readonly Stack<EntityDeclaration> entityStack = new Stack<EntityDeclaration> ();

	TypeDeclaration? dllImportsType;

	public BindingGenerator (BindingResult bindingResult)
		: base (bindingResult)
	{
	}

	public DocumentSyntaxTree SyntaxTree {
		get { return root; }
	}

	EntityDeclaration PushEntity (EntityDeclaration entity)
	{
		if (entityStack.Count == 0 || !(entityStack.Peek () is TypeDeclaration))
			root.Members.Add (entity);
		else
			((TypeDeclaration) entityStack.Peek ()).Members.Add (entity);

		var nativeNode = entity.Annotation<object> ();
		if (nativeNode is not null) {
			foreach (var attr in entity.Attributes.SelectMany (s => s.Attributes))
				attr.AddLinkAnnotation (nativeNode);
		}

		entityStack.Push (entity);
		return entity;
	}

	EntityDeclaration PopEntity ()
	{
		var entity = entityStack.Pop ();

		if (String.IsNullOrEmpty (entity.Name)) {
			var type = entity as TypeDeclaration;
			if (type is not null && type.Members.Count > 0)
				return entity;

			entity.Remove ();
		}

		return entity;
	}

	EntityDeclaration? CurrentEntity {
		get { return entityStack.Count > 0 ? entityStack.Peek () : null; }
	}

	TypeDeclaration CurrentType {
		get { return (TypeDeclaration) CurrentEntity!; }
	}

	public override void VisitObjCContainerDecl (ObjCContainerDecl decl)
	{
		if (decl.Location.IsInvalid)
			return;
		if (!decl.Handle.IsThisDeclarationADefinition && decl is not ObjCCategoryDecl)
			return;

		PushEntity (Bind (BindingResult, decl));
		base.VisitObjCContainerDecl (decl);
		PopEntity ();
	}

	public override void VisitObjCMethodDecl (ObjCMethodDecl decl)
	{
		if (decl.IsPropertyAccessor)
			return;

		CurrentType.Members.Add (Bind (BindingResult, decl));
	}

	public override void VisitObjCPropertyDecl (ObjCPropertyDecl decl)
	{
		CurrentType.Members.Add (Bind (BindingResult, decl));
	}

	public override void VisitTypedefDecl (TypedefDecl decl)
	{
		var underlyingDesugaredType = decl.UnderlyingType.UnqualifiedDesugaredType;
		switch (underlyingDesugaredType?.TypeClass) {
		case CX_TypeClass.CX_TypeClass_Enum:
			var boundEnum = ((EnumType) underlyingDesugaredType).Decl.Annotation<TypeDeclaration> ();
			if (boundEnum is not null)
				boundEnum.Name = decl.Name;
			break;
		case CX_TypeClass.CX_TypeClass_Record:
			var boundRecord = ((RecordType) underlyingDesugaredType).Decl.Annotation<TypeDeclaration> ();
			if (boundRecord is not null)
				boundRecord.Name = decl.Name;
			break;
		case CX_TypeClass.CX_TypeClass_BlockPointer:
			underlyingDesugaredType.AddAnnotation (decl);
			underlyingDesugaredType.Accept (this);
			break;
		}
	}

	/// <summary>
	/// Translate an anonymous delegate (a System.Action or System.Func) as
	/// bound by TypeBinder.VisitFunctionType to a strongly named delegate
	/// </summary>
	public override void VisitBlockPointerType (BlockPointerType type)
	{
		var typedef = type.GetAnnotations<TypedefDecl> ().LastOrDefault ();
		if (typedef is null)
			return;

		var anonDelegate = type.Bind (BindingResult) as DelegateType;
		if (anonDelegate is null)
			throw new InvalidOperationException ("expected DelegateType from " +
				"BlockPointerType.Bind; should never happen");

		var del = new DelegateDeclaration {
			Name = typedef.Name
		};

		del.AddLinkAnnotation (typedef);

		int i = 0;
		foreach (var typeArg in anonDelegate.TypeArguments) {
			typeArg.Remove ();
			del.Parameters.Add (new ParameterDeclaration (typeArg, String.Format ("arg{0}", i++)));
		}

		var functionProtoType = anonDelegate.Annotation<FunctionProtoType> ();
		if (functionProtoType is not null && functionProtoType.IsVariadic) {
			var vaParam = del.Parameters.LastOrNullObject ();
			if (vaParam is not null)
				vaParam.Name = "varArgs";
		}

		if (anonDelegate is FuncType) {
			var returnType = del.Parameters.LastOrNullObject ().Type;
			returnType.Parent.Remove ();
			returnType.Remove ();
			del.ReturnType = returnType;
		} else {
			del.ReturnType = new PrimitiveType ("void");
		}

		PushEntity (del);
	}

	public override void VisitEnumDecl (EnumDecl decl)
	{
		if (decl.Location.IsInvalid || !decl.IsThisDeclarationADefinition)
			return;

		PushEntity (Bind (BindingResult, decl));
		Visit (decl.Decls);
		PopEntity ();
	}

	public override void VisitEnumConstantDecl (EnumConstantDecl decl)
	{
		CurrentType?.Members.Add (Bind (BindingResult, decl));
	}

	public override void VisitFunctionDecl (FunctionDecl decl)
	{
		if (decl is CXXMethodDecl)
			return;

		if (dllImportsType is null)
			root.Members.Add (dllImportsType = new TypeDeclaration {
				ClassType = ClassType.Class,
				Modifiers = Modifiers.Static,
				Name = "CFunctions"
			});

		dllImportsType.Members.Add (Bind (BindingResult, decl));
	}

	public override void VisitRecordDecl (RecordDecl decl)
	{
		if (decl.Location.IsInvalid || !decl.IsThisDeclarationADefinition)
			return;

		switch (decl.CursorKind) {
		case CXCursorKind.CXCursor_StructDecl:
		case CXCursorKind.CXCursor_UnionDecl:
			break;
		default:
			return;
		}

		var cxxDecl = decl as CXXRecordDecl;
		if (cxxDecl is not null && !cxxDecl.IsPOD)
			return;

		PushEntity (Bind (BindingResult, decl));
		Visit (decl.Decls);
		PopEntity ();
	}

	public override void VisitFieldDecl (FieldDecl decl)
	{
		if (decl.IsBitField && decl.BitWidthValue == 0)
			return;

		if (CurrentType is not null && !(decl is ObjCIvarDecl))
			CurrentType.Members.Add (Bind (decl));
	}

	class ConstantsInterfaceTypeDeclaration : TypeDeclaration {
		public ConstantsInterfaceTypeDeclaration ()
		{
			Name = "Constants";
			Modifiers = Modifiers.Partial;
			ClassType = ClassType.Interface;

			this.AddAttribute (new VerifyAttribute (VerifyHint.ConstantsInterfaceAssociation));
			this.AddAttribute (new StaticAttribute ());
		}
	}

	public override void VisitVarDecl (VarDecl decl)
	{
		if (decl.Name is null)
			return;

		var type = root.Members.LastOrNullObject () as ConstantsInterfaceTypeDeclaration;
		if (type is null)
			root.Members.Add (type = new ConstantsInterfaceTypeDeclaration ());

		var property = new PropertyDeclaration {
			Name = decl.Name,
			ReturnType = decl.Type.Bind (BindingResult),
			Getter = new Accessor ()
		};

		if (string.IsNullOrEmpty (property.Name)) {
			BindingResult.ReportUnsupportedConstruct (decl.Location, "Variable with no name");
			return;
		} else if (property.Name.Contains (' ')) {
			BindingResult.ReportUnsupportedConstruct (decl.Location, $"Variable with space in its name: {property.Name}");
			return;
		}

		property.AddLinkAnnotation (decl);

		property.AddAttribute (new FieldAttribute (decl.Name, "__Internal"));

		type.Members.Add (property);
	}

	#region Static Binders

	static int anonymousTypeCounter;
	static TypeDeclaration Bind (BindingResult bindingResult, ObjCContainerDecl decl)
	{
		var typeName = decl.GetIsUnnamedOrAnonymous (bindingResult) ? "" : decl.Name;

		if (typeName.Contains (' ')) {
			bindingResult.ReportUnsupportedConstruct (decl.Location, $"Type with space in its name: {typeName}");
		}

		var type = new TypeDeclaration {
			Name = typeName,
			ClassType = ClassType.Interface
		};

		type.AddLinkAnnotation (decl);

		// We should check if we are binding a swift bridging header, if so we should use the mangled names.
		string? swiftName = null;
		if (decl.Attrs.TryGetObjCRuntimeName (out var name)) {
			if (IsSwiftName (name))
				swiftName = name;
		}

		if (decl is ObjCInterfaceDecl iface && iface.SuperClass is not null) {
			var bt = new BaseTypeAttribute (iface.SuperClass.Name);
			if (swiftName is not null)
				bt.Arguments.Add (new NamedExpression ("Name", new PrimitiveExpression (swiftName)));
			type.AddAttribute (bt);
		}

		if (decl is ObjCCategoryDecl category && category.ClassInterface is not null) {
			type.Name = category.ClassInterface.Name + "_" + type.Name;
			type.AddAttribute (new CategoryAttribute ());
			type.AddAttribute (new BaseTypeAttribute (category.ClassInterface.Name));
		}

		if (decl is ObjCProtocolDecl) {
			// Just add '[Model]' to Delegates or DataSources we do not need it anywhere else
			if (NeedsModel (decl.Name)) {
				type.AddAttribute (new ModelAttribute ());
			} else {
				var comment = new Comment (ModelAttribute.ModelRemovedComment, CommentType.MultiLine);
				type.AddChild (comment, Roles.Comment);
			}

			var pa = new ProtocolAttribute ();
			if (swiftName is not null)
				pa.Arguments.Add (new NamedExpression ("Name", new PrimitiveExpression (swiftName)));
			type.AddAttribute (pa);
		}

		foreach (var protocol in decl.Protocols) {
			AstType protoType;
			if (protocol.Name == "NSObject") {
				var bt = new BaseTypeAttribute (protoType = AstType.Create ("Foundation.NSObject"));
				if (swiftName is not null)
					bt.Arguments.Add (new NamedExpression ("Name", new PrimitiveExpression (swiftName)));
				type.AddAttribute (bt);
			} else
				type.BaseTypes.Add (protoType = AstType.Create ("I" + protocol.Name));
			protoType.AddAnnotation (protocol);
		}

		return type;
	}

	// Swift symbols begin with either '_T' or '_$'
	static bool IsSwiftName (string name) => name.StartsWith ("_T", StringComparison.Ordinal) || name.StartsWith ("_$", StringComparison.Ordinal);

	static bool NeedsModel (string name) => name.EndsWith ("Delegate", StringComparison.OrdinalIgnoreCase) || name.EndsWith ("DataSource", StringComparison.OrdinalIgnoreCase);

	public static MethodDeclaration Bind (BindingResult bindingResult, ObjCMethodDecl decl)
	{
		MethodDeclaration method;

		if (decl.MethodFamily == ObjCMethodFamily.Init || decl.IsDesignatedInitializer)
			method = new ObjCConstructorMethodDeclaration {
				Name = "Constructor",
				ReturnType = new NativeHandleType ()
			};
		else
			method = new MethodDeclaration {
				Name = decl.GetSelector ().ToCSharpName (),
				ReturnType = decl.ReturnType.Bind (bindingResult)
			};

		if (string.IsNullOrEmpty (method.Name))
			bindingResult.ReportUnsupportedConstruct (decl.Location, "Method with no name");

		method.AddLinkAnnotation (decl);

		BindParameters (bindingResult, method, decl);

		if (decl.IsClassMethod)
			method.AddAttribute (new StaticAttribute ());

		if (decl.DeclContext is ObjCProtocolDecl && !decl.Handle.IsObjCOptional)
			method.AddAttribute (new AbstractAttribute ());

		method.AddAttribute (new ExportAttribute (decl.Name, isVariadic: decl.Handle.IsVariadic));

		if (decl.IsDesignatedInitializer)
			method.AddAttribute (new DesignatedInitializerAttribute ());

		if (decl.Handle.IsVariadic)
			method.AddAttribute (new InternalAttribute ());

		if (decl.Attrs.IsObjCRequiresSuperAttr ())
			method.AddAttribute (new RequiresSuperAttribute ());

		return method;
	}

	public static ParameterDeclaration Bind (BindingResult bindingResult, ParmVarDecl decl, int index)
	{
		ParameterModifier modifier;
		var param = new ParameterDeclaration (decl.Type.Bind (bindingResult, out modifier), string.IsNullOrEmpty (decl.Name) ? "unnamedParameter" + index : decl.Name);
		param.ParameterModifier = modifier;
		param.AddLinkAnnotation (decl);
		return param;
	}

	public static PropertyDeclaration Bind (BindingResult bindingResult, ObjCPropertyDecl decl)
	{
		var property = new PropertyDeclaration {
			Name = decl.Name.UCFirst (),
			ReturnType = decl.Type.Bind (bindingResult)
		};

		if (string.IsNullOrEmpty (property.Name)) {
			bindingResult.ReportUnsupportedConstruct (decl.Location, "Property with no name");
		} else if (property.Name.Contains (' ')) {
			bindingResult.ReportUnsupportedConstruct (decl.Location, $"Property with space in its name: {property.Name}");
		}

		property.AddLinkAnnotation (decl);

		if (decl.DeclContext is ObjCProtocolDecl && (
			!decl.Handle.IsObjCOptional ||
			decl.GetterMethodDecl?.Handle.IsObjCOptional == false ||
			decl.SetterMethodDecl?.Handle.IsObjCOptional == false))
			property.AddAttribute (new AbstractAttribute ());

		if (decl.GetPropertyAttributes ().HasFlag (ObjCPropertyAttributeKind.Class))
			property.AddAttribute (new StaticAttribute ());

		if (decl.GetPropertyAttributes ().HasFlag (ObjCPropertyAttributeKind.NullResettable))
			property.AddAttribute (new NullAllowedAttribute ());

		property.AddAttribute (new ExportAttribute (decl.Name, decl.ArgumentSemantic));

		if (decl.GetterMethodDecl is not null) {
			property.Getter = new Accessor ();
			if (decl.GetPropertyAttributes ().HasFlag (ObjCPropertyAttributeKind.Getter))
				property.Getter.AddAttribute (new BindAttribute (decl.GetterMethodDecl.Name));
		}

		if (decl.SetterMethodDecl is not null) {
			property.Setter = new Accessor ();
			if (decl.GetPropertyAttributes ().HasFlag (ObjCPropertyAttributeKind.Setter))
				property.Setter.AddAttribute (new BindAttribute (decl.SetterMethodDecl.Name));
		}

		return property;
	}

	public static TypeDeclaration Bind (BindingResult bindingResult, EnumDecl decl)
	{
		var type = new TypeDeclaration {
			ClassType = ClassType.Enum,
			Modifiers = Modifiers.Public,
			Name = decl.GetIsUnnamedOrAnonymous (bindingResult) ? (decl.TypedefNameForAnonDecl?.Name ?? string.Empty) : decl.Name
		};

		type.AddLinkAnnotation (decl);

		var backingType = decl.IntegerType.Bind (bindingResult);
		if ((backingType as PrimitiveType)?.Keyword != "int")
			type.BaseTypes.Add (backingType);

		if (decl.Attrs.IsFlagEnumAttr ())
			type.AddAttribute (new Attributes.FlagsAttribute ());

		if (backingType is NIntType) {
			type.AddAttribute (new NativeAttribute ());
			backingType.ReplaceWith (new PrimitiveType ("long"));
		} else if (backingType is NUIntType) {
			type.AddAttribute (new NativeAttribute ());
			backingType.ReplaceWith (new PrimitiveType ("ulong"));
		}

		return type;
	}

	public static EnumMemberDeclaration Bind (BindingResult bindingResult, EnumConstantDecl decl)
	{
		try {
			var member = new EnumMemberDeclaration {
				Name = decl.Name,
				Initializer = ExpressionBinder.Bind (bindingResult, decl.InitExpr, n => {
					var builtinKind = CXTypeKind.CXType_Int;
					var type = decl.Type;

					// enum constants can take the type of either a builtin,
					// or a typedef in Objective-C (with 'objc_fixed_enum' support,
					// or an enum type in C++ (with 'cxx_strong_enums') support.
					// Resolve typedef types and enum types to builtins. This is
					// done by hand instead of using a TypeBinder since we _only_
					// want to allow builtin, typedef, or enum types in the chain.
					while (true) {
						var builtinType = type as BuiltinType;
						if (builtinType is not null) {
							builtinKind = builtinType.Kind;
							break;
						}

						var typedefType = type as TypedefType;
						if (typedefType is not null) {
							type = typedefType.Decl.UnderlyingType;
							continue;
						}

						var enumType = type as EnumType;
						if (enumType is not null) {
							type = enumType.Decl.IntegerType;
							continue;
						}
						var elabType = type as ElaboratedType;
						if (elabType is not null) {
							// get the most basic type from the QualType and if we are dealing with a long, set the type accordingly, this 
							// is because we might be dealing with a NSInteger, the issue happens when the enum is exposed to swift
							var namedType = elabType.Desugar.UnqualifiedDesugaredType as BuiltinType;
							if (namedType is not null) {
								// switch on the builtin type and set the type accordingly
								switch (namedType.Kind) {
								case CXTypeKind.CXType_UChar:
								case CXTypeKind.CXType_SChar:
								case CXTypeKind.CXType_Short:
								case CXTypeKind.CXType_UShort:
								case CXTypeKind.CXType_Int:
								case CXTypeKind.CXType_UInt:
								case CXTypeKind.CXType_Long:
								case CXTypeKind.CXType_ULong:
								case CXTypeKind.CXType_LongLong:
								case CXTypeKind.CXType_ULongLong:
									type = namedType;
									builtinKind = namedType.Kind;
									// yes! you can use a continue inside a switch
									continue;
								default:
									bindingResult.ReportUnsupportedConstruct (decl.Location, $"Enum constant with unsupported underlying type ({namedType.Kind})");
									// we do not know how to deal with this
									break;
								}
							}
						}

						throw new Exception ("Unsupported Clang.Ast.Type for EnumConstantDecl: "
							+ type.Kind + " at " + decl.PresumedLoc);
					}

					switch (builtinKind) {
					case CXTypeKind.CXType_Int:
						return (int) n;
					case CXTypeKind.CXType_UInt:
						return (uint) n;
					case CXTypeKind.CXType_Long:
					case CXTypeKind.CXType_LongLong:
						return (long) n;
					default:
						return n;
					}
				})
			};

			member.AddLinkAnnotation (decl);

			return member;
		} catch (Exception e) {
			string initExpr = "<unable to Expr::PrettyPrint(AstContext)>";
			if (decl.InitExpr is not null) {
				var expr = decl.InitExpr.ToString ();
				if (expr is not null)
					initExpr = expr;
			}
			throw new AggregateException ("Unable to bind expression `" + initExpr + "'", e);
		}
	}

	public static MethodDeclaration Bind (BindingResult bindingResult, FunctionDecl decl)
	{
		var method = new MethodDeclaration {
			Modifiers = Modifiers.Static | Modifiers.Extern,
			Name = decl.Name,
			ReturnType = decl.ReturnType.Bind (bindingResult)
		};

		if (string.IsNullOrEmpty (method.Name)) {
			bindingResult.ReportUnsupportedConstruct (decl.Location, "Method with no name");
		} else if (method.Name.Contains (' ')) {
			bindingResult.ReportUnsupportedConstruct (decl.Location, $"Method with space in its name: {method.Name}");
		}

		method.AddLinkAnnotation (decl);

		BindParameters (bindingResult, method, decl);

		method.AddAttribute (new Attributes.DllImportAttribute ("__Internal"));
		method.AddAttribute (new VerifyAttribute (VerifyHint.PlatformInvoke));

		return method;
	}

	public static TypeDeclaration Bind (BindingResult bindingResult, RecordDecl decl)
	{
		string? typeName;

		if (decl.GetIsUnnamedOrAnonymous (bindingResult)) {
			if (decl.TypedefNameForAnonDecl is not null) {
				typeName = decl.TypedefNameForAnonDecl?.Name;
			} else if (decl.NextDeclInContext is NamedDecl nextNamedDecl && !string.IsNullOrEmpty (nextNamedDecl.Name) && !nextNamedDecl.GetIsUnnamedOrAnonymous (bindingResult)) {
				typeName = nextNamedDecl.Name;
			} else {
				if (decl.CursorKind == CXCursorKind.CXCursor_UnionDecl) {
					typeName = $"AnonymousUnion{++anonymousTypeCounter}";
				} else {
					typeName = $"AnonymousStruct{++anonymousTypeCounter}";
				}
			}
		} else {
			typeName = decl.Name;
		}

		if (typeName?.Contains (' ') == true)
			bindingResult.ReportUnsupportedConstruct (decl.Location, $"Type with space in its name: {typeName}");

		var type = new TypeDeclaration {
			ClassType = ClassType.Struct,
			Modifiers = Modifiers.Public,
			Name = typeName,
		};

		type.AddLinkAnnotation (decl);

		if (decl.CursorKind != CXCursorKind.CXCursor_UnionDecl)
			type.AddAttribute (new Attributes.StructLayoutAttribute (LayoutKind.Sequential));

		return type;
	}

	public FieldDeclaration Bind (FieldDecl decl)
	{
		AstType? returnType;
		var returnDeclType = decl.Type.AsTagDecl;
		if (returnDeclType is not null && returnDeclType.GetIsUnnamedOrAnonymous (BindingResult)) {
			var lastType = CurrentType.Members.OfType<TypeDeclaration> ().LastOrDefault ();
			if (lastType is not null)
				returnType = AstType.Create (lastType.Name);
			else
				returnType = AstType.Create (returnDeclType.Name);
		} else if (decl.Type is ConstantArrayType constantArrayType && constantArrayType.ElementType.AsTagDecl?.GetIsUnnamedOrAnonymous (BindingResult) == true) {
			var lastType = CurrentType.Members.OfType<TypeDeclaration> ().LastOrDefault ();
			AstType elementType;
			if (lastType is not null)
				elementType = AstType.Create (lastType.Name);
			else
				elementType = AstType.Create (constantArrayType.ElementType.AsTagDecl.Name);
			returnType = elementType.MakeArrayType ();
		} else {
			returnType = decl.Type.Bind (BindingResult);
		}

		var returnTypeString = returnType?.ToString () ?? "";
		if (returnTypeString.Contains ("unnamed"))
			BindingResult.ReportUnsupportedConstruct (decl.Location, $"Field type with no name or invalid name ({returnTypeString})");

		var field = new FieldDeclaration {
			Modifiers = Modifiers.Public,
			ReturnType = returnType,
		};

		field.AddLinkAnnotation (decl);

		string? name = null;
		if (decl.GetIsUnnamedOrAnonymous (BindingResult)) {
			var fieldType = CurrentType.Members.LastOrDefault () as TypeDeclaration;
			switch (decl.Type.AsTagDecl?.CursorKind) {
			case CXCursorKind.CXCursor_UnionDecl:
				if (fieldType is not null) {
					field.ReturnType = AstType.Create (fieldType.Name);
					name = "AnonymousUnionField";
				}
				break;
			case CXCursorKind.CXCursor_StructDecl:
				if (fieldType is not null) {
					field.ReturnType = AstType.Create (fieldType.Name);
					name = "AnonymousStructField";
				}
				break;
			}
		}

		if (string.IsNullOrEmpty (name))
			name = decl.Name;

		field.Variables.Add (new VariableInitializer (name));

		if (string.IsNullOrEmpty (name))
			BindingResult.ReportUnsupportedConstruct (decl.Location, "Field with no name");
		else if (name.Contains (' '))
			BindingResult.ReportUnsupportedConstruct (decl.Location, $"Field with space in its name: {name}");

		return field;
	}

	static void BindParameters (BindingResult bindingResult, MethodDeclaration method, object decl)
	{
		var parameters = GetParameters (decl, out var isVariadic);

		for (var i = 0; i < parameters.Count; i++)
			method.Parameters.Add (Bind (bindingResult, parameters [i], i));

		if (isVariadic)
			method.Parameters.Add (new ParameterDeclaration (new IntPtrType (), "varArgs"));
	}

	static IReadOnlyList<ParmVarDecl> GetParameters (object decl, out bool isVariadic)
	{
		isVariadic = false;
		if (decl is FunctionDecl functionDecl) {
			isVariadic = functionDecl.IsVariadic;
			return functionDecl.Parameters;
		} else if (decl is ObjCMethodDecl methodDecl) {
			isVariadic = methodDecl.Handle.IsVariadic;
			return methodDecl.Parameters;
		} else {
			throw new NotImplementedException ($"GetParameters: {decl.GetType ().Name}");
		}
	}

	#endregion
}
