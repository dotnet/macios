// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind;

public class ExpressionBinder : AstVisitor {
	public ExpressionBinder (BindingResult bindingResult)
		: base (bindingResult)
	{
	}

	public static Expression? Bind (BindingResult bindingResult, Expr? expr,
		Func<ulong, object>? numericLiteralResolver = null)
	{
		if (expr is null)
			return null;

		var binder = new ExpressionBinder (bindingResult, numericLiteralResolver);
		expr.Accept (binder);

		if (binder.stack.Count == 0)
			return null;

		if (binder.stack.Count > 1)
			bindingResult.ReportInternalError (expr.Location, "stack should have exactly one expression on it");

		return binder.stack.Pop ();
	}

	readonly Func<ulong, object>? numericLiteralResolver;
	readonly Stack<Expression> stack = new Stack<Expression> ();

	ExpressionBinder (BindingResult bindingResult, Func<ulong, object>? numericLiteralResolver)
		: base (bindingResult)
	{
		this.numericLiteralResolver = numericLiteralResolver;
	}

	public override void VisitStmt (Stmt stmt)
	{
		BindingResult.ReportUnsupportedConstruct (stmt.Location, $"Statement visitation not supported.");
	}

	void ResolveNumericLiteral (ulong literal)
	{
		object resolved = literal;
		if (numericLiteralResolver is not null)
			resolved = numericLiteralResolver (literal) ?? resolved;
		stack.Push (new PrimitiveExpression (resolved));
	}

	public override void VisitIntegerLiteral (IntegerLiteral stmt)
	{
		ResolveNumericLiteral (stmt.UnsignedValue);
	}

	public override void VisitCharacterLiteral (CharacterLiteral stmt)
	{
		ResolveNumericLiteral ((ulong) stmt.Value);
	}

	public override void VisitFloatingLiteral (FloatingLiteral stmt)
	{
		var semantics = stmt.RawSemantics;
		switch (semantics) {
		case CX_FloatingSemantics.CX_FLK_IEEEdouble:
			stack.Push (new PrimitiveExpression (stmt.ValueAsApproximateDouble));
			break;
		case CX_FloatingSemantics.CX_FLK_IEEEsingle:
			stack.Push (new PrimitiveExpression ((float) stmt.ValueAsApproximateDouble));
			break;
		default:
			BindingResult.ReportUnsupportedConstruct (stmt.Location, $"Unsupported floating literal semantics: {semantics}");
			break;
		}
	}

	public override void VisitBinaryOperator (BinaryOperator stmt)
	{
		stmt.RHS.Accept (this);
		stmt.LHS.Accept (this);
		stack.Push (new BinaryOperatorExpression (
			stack.Pop (),
			stmt.Opcode.ToBinaryOperatorType (),
			stack.Pop ()
		));
	}

	public override void VisitUnaryOperator (UnaryOperator stmt)
	{
		stmt.SubExpr.Accept (this);
		stack.Push (new UnaryOperatorExpression (
			stmt.Opcode.ToUnaryOperatorType (),
			stack.Pop ())
		);
	}

	public override void VisitParenExpr (ParenExpr stmt)
	{
		stmt.SubExpr.Accept (this);
		stack.Push (new ParenthesizedExpression (stack.Pop ()));
	}

	public override void VisitImplicitCastExpr (ImplicitCastExpr stmt)
	{
		stmt.SubExprAsWritten.Accept (this);
	}

	public override void VisitExplicitCastExpr (ExplicitCastExpr stmt)
	{
		stmt.SubExprAsWritten.Accept (this);
		stack.Push (new CastExpression (
			stmt.TypeAsWritten.Bind (BindingResult),
			stack.Pop ()
		));
	}

	public override void VisitDeclRefExpr (DeclRefExpr stmt)
	{
		stack.Push (new IdentifierExpression (stmt.Decl.Name));
	}

	public override void VisitUnaryExprOrTypeTraitExpr (UnaryExprOrTypeTraitExpr stmt)
	{
		if (stmt.Kind != CX_UnaryExprOrTypeTrait.CX_UETT_SizeOf)
			BindingResult.ReportUnsupportedConstruct (stmt.Location, $"Unsupported UnaryExprOrTypeTraitKind: {stmt.Kind}");

		var argType = stmt.ArgumentType.Bind (BindingResult);

		switch ((argType as PrimitiveType)?.Keyword) {
		case "bool":
		case "char":
		case "sbyte":
		case "byte":
		case "short":
		case "ushort":
		case "int":
		case "uint":
		case "long":
		case "ulong":
		case "float":
		case "double":
			break;
		default:
			var e = stmt.Handle.Evaluate;
			if (e.Kind == CXEvalResultKind.CXEval_Int) {
				ResolveNumericLiteral (e.AsUnsigned);
				return;
			}
			BindingResult.ReportUnsupportedConstruct (stmt.Location, $"Unsupported type for sizeof: {argType}");
			break;
		}

		stack.Push (new SizeOfExpression (argType));
	}

	public override void VisitConstantExpr (ConstantExpr stmt) => stmt.SubExpr?.Accept (this);
}
