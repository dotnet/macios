namespace Extrospection {

	public class Runner {

		public Runner ()
		{
		}

		public void Execute (IEnumerable<string> assemblyNames, string outputDirectory, IEnumerable<string> searchDirectories, string responseFile)
		{
			string [] arguments = new string [] {
				$"@{responseFile}",
			};

			var rv = Tools.Visit (arguments, (bindingResult) => {
				var visitor = new SharpieVisitor (bindingResult) {
					new MapNamesVisitor (bindingResult), // must come first to map managed and native names.
					new ReleaseAttributeCheck (bindingResult),
					new DesignatedInitializerCheck (bindingResult),
					new DllImportCheck (bindingResult),
					new EnumCheck (bindingResult),
					new FieldCheck (bindingResult),
					new ObjCInterfaceCheck (bindingResult),
					new ObjCProtocolCheck (bindingResult),
					new SelectorCheck (bindingResult),
					new SimdCheck (bindingResult),
					new RequiresSuperCheck (bindingResult),
					new DeprecatedCheck (bindingResult),
					new NullabilityCheck (bindingResult),
					new UIAppearanceCheck (bindingResult),
	//				new ListNative (bindingResult), // for debug
				};
				foreach (var assemblyName in assemblyNames) {
					var name = Path.GetFileNameWithoutExtension (assemblyName);
					if (name.EndsWith (".iOS", StringComparison.Ordinal))
						Helpers.Platform = Platforms.iOS;
					else if (name.EndsWith (".macOS", StringComparison.Ordinal))
						Helpers.Platform = Platforms.macOS;
					else if (name.EndsWith (".tvOS", StringComparison.Ordinal))
						Helpers.Platform = Platforms.tvOS;
					else if (name.EndsWith (".MacCatalyst", StringComparison.Ordinal))
						Helpers.Platform = Platforms.MacCatalyst;
					else
						throw new NotImplementedException ($"Unknown platform for assembly {assemblyName}");
					visitor.Load (assemblyName, searchDirectories);
				}
				visitor.ProcessManaged ();
				return visitor;
			}, out var bindingResult);

			if (!rv) {
				bindingResult?.PrintMessages ();
				throw new InvalidOperationException ("No visitor was created.");
			}

			Log.Save (outputDirectory);
		}
	}

	class AssemblyResolver : IAssemblyResolver, IDisposable {
		Dictionary<string, AssemblyDefinition> cache = new Dictionary<string, AssemblyDefinition> (StringComparer.Ordinal);
		HashSet<string> directories = new HashSet<string> ();

		public AssemblyDefinition Resolve (AssemblyNameReference name)
		{
			return Resolve (name, new ReaderParameters ());
		}

		AssemblyDefinition SearchDirectories (AssemblyNameReference name, ReaderParameters parameters)
		{
			var extensions = new string [] { ".dll", ".exe" };
			var paths = directories
				.SelectMany (dir => extensions.Select (ext => Path.Combine (dir, name.Name + ext)))
				.Where (File.Exists)
				.ToArray ();

			if (paths.Length == 0)
				throw new Exception ($"Unable to resolve the assembly {name.FullName} because it wasn't found in any of the search directories:\n\t{string.Join ("\n\t", directories)}");

			if (paths.Length > 1)
				throw new Exception ($"Unable to resolve the assembly {name.FullName} because multiple candidates were found:\n\t{string.Join ("\n\t", paths)}\nIn the search directories:\n\t{string.Join ("\n\t", directories)}");

			var path = paths [0];
			if (parameters.AssemblyResolver is null)
				parameters.AssemblyResolver = this;
			return AssemblyDefinition.ReadAssembly (path, parameters);
		}

		public AssemblyDefinition Load (string path)
		{
			var parameters = new ReaderParameters () {
				AssemblyResolver = this,
			};
			var rv = AssemblyDefinition.ReadAssembly (path, parameters);
			cache [rv.Name.FullName] = rv;
			return rv;
		}

		public AssemblyDefinition Resolve (AssemblyNameReference name, ReaderParameters parameters)
		{
			var key = name.FullName;
			if (cache.TryGetValue (key, out var assembly))
				return assembly;

			assembly = SearchDirectories (name, parameters);
			cache [key] = assembly;
			return assembly;
		}

		public void AddSearchDirectory (params string [] values)
		{
			foreach (var value in values)
				directories.Add (value.TrimEnd ('/'));
		}

		public void Dispose ()
		{
			// Nothing to do.
		}
	}

	sealed class SharpieVisitor : BaseVisitor, IEnumerable<BaseVisitor> {

		HashSet<AssemblyDefinition> assemblies = new HashSet<AssemblyDefinition> ();
		AssemblyResolver resolver = new AssemblyResolver ();

		public SharpieVisitor (BindingResult bindingResult)
			: base (bindingResult)
		{
		}

		public void Load (string filename, IEnumerable<string> searchDirectories)
		{
			resolver.AddSearchDirectory (searchDirectories.ToArray ());
			resolver.AddSearchDirectory (Path.GetDirectoryName (filename));
			assemblies.Add (resolver.Load (filename));
		}

		public void ProcessManaged ()
		{
			foreach (var ad in assemblies) {
				foreach (var v in Visitors) {
					v.VisitManagedAssembly (ad);
					foreach (var module in ad.Modules) {
						v.VisitManagedModule (module);
						if (!module.HasTypes)
							continue;
						foreach (var td in module.Types)
							ProcessType (v, td);
					}
				}
			}
		}

		void ProcessType (BaseVisitor v, TypeDefinition type)
		{
			v.VisitManagedType (type);
			if (type.HasMethods) {
				foreach (var md in type.Methods)
					v.VisitManagedMethod (md);
			}

			if (type.HasNestedTypes) {
				foreach (var nested in type.NestedTypes)
					ProcessType (v, nested);
			}
		}

		List<BaseVisitor> Visitors { get; } = new List<BaseVisitor> ();

		public void Add (BaseVisitor visitor)
		{
			Visitors.Add (visitor);
		}

		public override void EndVisit ()
		{
			base.EndVisit ();
			foreach (var v in Visitors)
				v.EndVisit ();
		}

		public IEnumerator<BaseVisitor> GetEnumerator ()
		{
			return Visitors.GetEnumerator ();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return Visitors.GetEnumerator ();
		}

		public override void VisitAdjustedType (AdjustedType type)
		{
			base.VisitAdjustedType (type);
			foreach (var v in Visitors)
				v.VisitAdjustedType (type);
		}

		public override void VisitArrayType (ClangSharp.ArrayType type)
		{
			base.VisitArrayType (type);
			foreach (var v in Visitors)
				v.VisitArrayType (type);
		}

		public override void VisitPointerType (ClangSharp.PointerType type)
		{
			base.VisitPointerType (type);
			foreach (var v in Visitors)
				v.VisitPointerType (type);
		}

		public override void VisitTypedefType (ClangSharp.TypedefType type)
		{
			base.VisitTypedefType (type);
			foreach (var v in Visitors)
				v.VisitTypedefType (type);
		}

		public override void VisitFunctionType (ClangSharp.FunctionType type)
		{
			base.VisitFunctionType (type);
			foreach (var v in Visitors)
				v.VisitFunctionType (type);
		}

		public override void VisitAttributedType (ClangSharp.AttributedType type)
		{
			base.VisitAttributedType (type);
			foreach (var v in Visitors)
				v.VisitAttributedType (type);
		}

		public override void VisitBuiltinType (ClangSharp.BuiltinType type)
		{
			base.VisitBuiltinType (type);
			foreach (var v in Visitors)
				v.VisitBuiltinType (type);
		}

		public override void VisitEnumType (ClangSharp.EnumType type)
		{
			base.VisitEnumType (type);
			foreach (var v in Visitors)
				v.VisitEnumType (type);
		}

		public override void VisitRecordType (ClangSharp.RecordType type)
		{
			base.VisitRecordType (type);
			foreach (var v in Visitors)
				v.VisitRecordType (type);
		}

		public override void VisitObjCObjectType (ClangSharp.ObjCObjectType type)
		{
			base.VisitObjCObjectType (type);
			foreach (var v in Visitors)
				v.VisitObjCObjectType (type);
		}

		public override void VisitUnaryExprOrTypeTraitExpr (UnaryExprOrTypeTraitExpr stmt)
		{
			base.VisitUnaryExprOrTypeTraitExpr (stmt);
			foreach (var v in Visitors)
				v.VisitUnaryExprOrTypeTraitExpr (stmt);
		}

		public override void VisitAtomicType (AtomicType type)
		{
			base.VisitAtomicType (type);
			foreach (var v in Visitors)
				v.VisitAtomicType (type);
		}

		public override void VisitAttr (Attr attr)
		{
			base.VisitAttr (attr);
			foreach (var v in Visitors)
				v.VisitAttr (attr);
		}

		public override void VisitBinaryOperator (BinaryOperator stmt)
		{
			base.VisitBinaryOperator (stmt);
			foreach (var v in Visitors)
				v.VisitBinaryOperator (stmt);
		}

		public override void VisitBlockPointerType (BlockPointerType type)
		{
			base.VisitBlockPointerType (type);
			foreach (var v in Visitors)
				v.VisitBlockPointerType (type);
		}

		public override void VisitCastExpr (CastExpr stmt)
		{
			base.VisitCastExpr (stmt);
			foreach (var v in Visitors)
				v.VisitCastExpr (stmt);
		}

		public override void VisitCharacterLiteral (CharacterLiteral stmt)
		{
			base.VisitCharacterLiteral (stmt);
			foreach (var v in Visitors)
				v.VisitCharacterLiteral (stmt);
		}

		public override void VisitComplexType (ComplexType type)
		{
			base.VisitComplexType (type);
			foreach (var v in Visitors)
				v.VisitComplexType (type);
		}

		public override void VisitConstantArrayType (ConstantArrayType type)
		{
			base.VisitConstantArrayType (type);
			foreach (var v in Visitors)
				v.VisitConstantArrayType (type);
		}

		public override void VisitConstantExpr (ConstantExpr stmt)
		{
			base.VisitConstantExpr (stmt);
			foreach (var v in Visitors)
				v.VisitConstantExpr (stmt);
		}

		public override void VisitDecayedType (DecayedType type)
		{
			base.VisitDecayedType (type);
			foreach (var v in Visitors)
				v.VisitDecayedType (type);
		}

		public sealed override void VisitDecl (Decl decl)
		{
			base.VisitDecl (decl);
			// If we iterate over all the visitors here, we end up visiting the same nodes multiple times.
		}

		public override void VisitDeclaratorDecl (DeclaratorDecl decl)
		{
			base.VisitDeclaratorDecl (decl);
			foreach (var v in Visitors)
				v.VisitDeclaratorDecl (decl);
		}

		public override void VisitDeclRefExpr (DeclRefExpr stmt)
		{
			base.VisitDeclRefExpr (stmt);
			foreach (var v in Visitors)
				v.VisitDeclRefExpr (stmt);
		}

		public override void VisitElaboratedType (ElaboratedType type)
		{
			base.VisitElaboratedType (type);
			foreach (var v in Visitors)
				v.VisitElaboratedType (type);
		}

		public override void VisitEnumConstantDecl (EnumConstantDecl decl)
		{
			base.VisitEnumConstantDecl (decl);
			foreach (var v in Visitors)
				v.VisitEnumConstantDecl (decl);
		}

		public override void VisitEnumDecl (EnumDecl decl)
		{
			base.VisitEnumDecl (decl);
			foreach (var v in Visitors)
				v.VisitEnumDecl (decl);
		}

		public override void VisitExplicitCastExpr (ExplicitCastExpr stmt)
		{
			base.VisitExplicitCastExpr (stmt);
			foreach (var v in Visitors)
				v.VisitExplicitCastExpr (stmt);
		}

		public override void VisitExpr (Expr stmt)
		{
			base.VisitExpr (stmt);
			foreach (var v in Visitors)
				v.VisitExpr (stmt);
		}

		public override void VisitExtVectorType (ExtVectorType type)
		{
			base.VisitExtVectorType (type);
			foreach (var v in Visitors)
				v.VisitExtVectorType (type);
		}

		public override void VisitFieldDecl (FieldDecl decl)
		{
			base.VisitFieldDecl (decl);
			foreach (var v in Visitors)
				v.VisitFieldDecl (decl);
		}

		public override void VisitFloatingLiteral (FloatingLiteral stmt)
		{
			base.VisitFloatingLiteral (stmt);
			foreach (var v in Visitors)
				v.VisitFloatingLiteral (stmt);
		}

		public override void VisitFullExpr (FullExpr stmt)
		{
			base.VisitFullExpr (stmt);
			foreach (var v in Visitors)
				v.VisitFullExpr (stmt);
		}

		public override void VisitFunctionDecl (FunctionDecl decl)
		{
			base.VisitFunctionDecl (decl);
			foreach (var v in Visitors)
				v.VisitFunctionDecl (decl);
		}

		public override void VisitFunctionNoProtoType (FunctionNoProtoType type)
		{
			base.VisitFunctionNoProtoType (type);
			foreach (var v in Visitors)
				v.VisitFunctionNoProtoType (type);
		}

		public override void VisitFunctionProtoType (FunctionProtoType type)
		{
			base.VisitFunctionProtoType (type);
			foreach (var v in Visitors)
				v.VisitFunctionProtoType (type);
		}

		public override void VisitImplicitCastExpr (ImplicitCastExpr stmt)
		{
			base.VisitImplicitCastExpr (stmt);
			foreach (var v in Visitors)
				v.VisitImplicitCastExpr (stmt);
		}

		public override void VisitIncompleteArrayType (IncompleteArrayType type)
		{
			base.VisitIncompleteArrayType (type);
			foreach (var v in Visitors)
				v.VisitIncompleteArrayType (type);
		}

		public override void VisitIndirectFieldDecl (IndirectFieldDecl decl)
		{
			base.VisitIndirectFieldDecl (decl);
			foreach (var v in Visitors)
				v.VisitIndirectFieldDecl (decl);
		}

		public override void VisitIntegerLiteral (IntegerLiteral stmt)
		{
			base.VisitIntegerLiteral (stmt);
			foreach (var v in Visitors)
				v.VisitIntegerLiteral (stmt);
		}

		public override void VisitLinkageSpecDecl (LinkageSpecDecl decl)
		{
			base.VisitLinkageSpecDecl (decl);
			foreach (var v in Visitors)
				v.VisitLinkageSpecDecl (decl);
		}

		public override void VisitLValueReferenceType (LValueReferenceType type)
		{
			base.VisitLValueReferenceType (type);
			foreach (var v in Visitors)
				v.VisitLValueReferenceType (type);
		}

		public override void VisitMacroQualifiedType (MacroQualifiedType type)
		{
			base.VisitMacroQualifiedType (type);
			foreach (var v in Visitors)
				v.VisitMacroQualifiedType (type);
		}

		public override void VisitManagedAssembly (AssemblyDefinition assembly)
		{
			base.VisitManagedAssembly (assembly);
			foreach (var v in Visitors)
				v.VisitManagedAssembly (assembly);
		}

		public override void VisitManagedMethod (MethodDefinition method)
		{
			base.VisitManagedMethod (method);
			foreach (var v in Visitors)
				v.VisitManagedMethod (method);
		}

		public override void VisitManagedModule (ModuleDefinition module)
		{
			base.VisitManagedModule (module);
			foreach (var v in Visitors)
				v.VisitManagedModule (module);
		}

		public override void VisitManagedType (TypeDefinition type)
		{
			base.VisitManagedType (type);
			foreach (var v in Visitors)
				v.VisitManagedType (type);
		}

		public override void VisitNamedDecl (NamedDecl decl)
		{
			base.VisitNamedDecl (decl);
			foreach (var v in Visitors)
				v.VisitNamedDecl (decl);
		}

		public override void VisitNamespaceDecl (NamespaceDecl decl)
		{
			base.VisitNamespaceDecl (decl);
			foreach (var v in Visitors)
				v.VisitNamespaceDecl (decl);
		}

		public override void VisitNode (object node)
		{
			base.VisitNode (node);
			foreach (var v in Visitors)
				v.VisitNode (node);
		}

		public override void VisitObjCCategoryDecl (ObjCCategoryDecl decl)
		{
			base.VisitObjCCategoryDecl (decl);
			foreach (var v in Visitors)
				v.VisitObjCCategoryDecl (decl);
		}

		public override void VisitObjCContainerDecl (ObjCContainerDecl decl)
		{
			base.VisitObjCContainerDecl (decl);
			foreach (var v in Visitors)
				v.VisitObjCContainerDecl (decl);
		}

		public override void VisitObjCInterfaceDecl (ObjCInterfaceDecl decl)
		{
			base.VisitObjCInterfaceDecl (decl);
			foreach (var v in Visitors)
				v.VisitObjCInterfaceDecl (decl);
		}

		public override void VisitObjCIvarDecl (ObjCIvarDecl decl)
		{
			base.VisitObjCIvarDecl (decl);
			foreach (var v in Visitors)
				v.VisitObjCIvarDecl (decl);
		}

		public override void VisitObjCMethodDecl (ObjCMethodDecl decl)
		{
			base.VisitObjCMethodDecl (decl);
			foreach (var v in Visitors)
				v.VisitObjCMethodDecl (decl);
		}

		public override void VisitObjCObjectPointerType (ObjCObjectPointerType type)
		{
			base.VisitObjCObjectPointerType (type);
			foreach (var v in Visitors)
				v.VisitObjCObjectPointerType (type);
		}

		public override void VisitObjCPropertyDecl (ObjCPropertyDecl decl)
		{
			base.VisitObjCPropertyDecl (decl);
			foreach (var v in Visitors)
				v.VisitObjCPropertyDecl (decl);
		}

		public override void VisitObjCProtocolDecl (ObjCProtocolDecl decl)
		{
			base.VisitObjCProtocolDecl (decl);
			foreach (var v in Visitors)
				v.VisitObjCProtocolDecl (decl);
		}

		public override void VisitObjCTypeParamDecl (ObjCTypeParamDecl decl)
		{
			base.VisitObjCTypeParamDecl (decl);
			foreach (var v in Visitors)
				v.VisitObjCTypeParamDecl (decl);
		}

		public override void VisitObjCTypeParamType (ObjCTypeParamType type)
		{
			base.VisitObjCTypeParamType (type);
			foreach (var v in Visitors)
				v.VisitObjCTypeParamType (type);
		}

		public override void VisitParenExpr (ParenExpr stmt)
		{
			base.VisitParenExpr (stmt);
			foreach (var v in Visitors)
				v.VisitParenExpr (stmt);
		}

		public override void VisitParenType (ParenType type)
		{
			base.VisitParenType (type);
			foreach (var v in Visitors)
				v.VisitParenType (type);
		}

		public override void VisitRecordDecl (RecordDecl decl)
		{
			base.VisitRecordDecl (decl);
			foreach (var v in Visitors)
				v.VisitRecordDecl (decl);
		}

		public override void VisitRef (Ref @ref)
		{
			base.VisitRef (@ref);
			foreach (var v in Visitors)
				v.VisitRef (@ref);
		}

		public override void VisitReferenceType (ReferenceType type)
		{
			base.VisitReferenceType (type);
			foreach (var v in Visitors)
				v.VisitReferenceType (type);
		}

		public override void VisitStmt (Stmt stmt)
		{
			base.VisitStmt (stmt);
			foreach (var v in Visitors)
				v.VisitStmt (stmt);
		}

		public override void VisitTagDecl (TagDecl decl)
		{
			base.VisitTagDecl (decl);
			foreach (var v in Visitors)
				v.VisitTagDecl (decl);
		}

		public override void VisitTagType (TagType type)
		{
			base.VisitTagType (type);
			foreach (var v in Visitors)
				v.VisitTagType (type);
		}

		public sealed override void VisitTranslationUnitDecl (TranslationUnitDecl translationUnitDecl)
		{
			base.VisitTranslationUnitDecl (translationUnitDecl);
			// If we iterate over all the visitors here, we end up visiting the same nodes multiple times.
		}

		public override void VisitType (ClangSharp.Type type)
		{
			base.VisitType (type);
			foreach (var v in Visitors)
				v.VisitType (type);
		}

		public override void VisitTypeDecl (TypeDecl decl)
		{
			base.VisitTypeDecl (decl);
			foreach (var v in Visitors)
				v.VisitTypeDecl (decl);
		}

		public override void VisitTypedefDecl (TypedefDecl decl)
		{
			base.VisitTypedefDecl (decl);
			foreach (var v in Visitors)
				v.VisitTypedefDecl (decl);
		}

		public override void VisitTypedefNameDecl (TypedefNameDecl decl)
		{
			base.VisitTypedefNameDecl (decl);
			foreach (var v in Visitors)
				v.VisitTypedefNameDecl (decl);
		}

		public override void VisitTypeWithKeyword (TypeWithKeyword type)
		{
			base.VisitTypeWithKeyword (type);
			foreach (var v in Visitors)
				v.VisitTypeWithKeyword (type);
		}

		public override void VisitUnaryOperator (UnaryOperator stmt)
		{
			base.VisitUnaryOperator (stmt);
			foreach (var v in Visitors)
				v.VisitUnaryOperator (stmt);
		}

		public override void VisitValueDecl (ValueDecl decl)
		{
			base.VisitValueDecl (decl);
			foreach (var v in Visitors)
				v.VisitValueDecl (decl);
		}

		public override void VisitValueStmt (ValueStmt stmt)
		{
			base.VisitValueStmt (stmt);
			foreach (var v in Visitors)
				v.VisitValueStmt (stmt);
		}

		public override void VisitVarDecl (VarDecl decl)
		{
			base.VisitVarDecl (decl);
			foreach (var v in Visitors)
				v.VisitVarDecl (decl);
		}

		public override void VisitVectorType (VectorType type)
		{
			base.VisitVectorType (type);
			foreach (var v in Visitors)
				v.VisitVectorType (type);
		}
	}

	public class BaseVisitor : AstVisitor {
		public BaseVisitor (BindingResult bindingResult)
			: base (bindingResult)
		{
		}

		public virtual void VisitManagedAssembly (AssemblyDefinition assembly)
		{
		}

		public virtual void VisitManagedModule (ModuleDefinition module)
		{
		}

		public virtual void VisitManagedType (TypeDefinition type)
		{
		}

		public virtual void VisitManagedMethod (MethodDefinition method)
		{
		}
	}

	// debug
	class ListNative : BaseVisitor {
		public ListNative (BindingResult bindingResult)
			: base (bindingResult)
		{
		}

		public override void VisitDecl (Decl decl)
		{
			if (decl is FunctionDecl) {
				;
			} else if (decl is VarDecl) {
				;
			} else if (decl is ObjCProtocolDecl) {
				;
			} else if (decl is ObjCInterfaceDecl) {
				;
			} else if (decl is EnumDecl) {
				;
			} else {
				Console.WriteLine ("{0}\t{1}", decl, decl.GetType ().Name);
			}
		}
	}
}
