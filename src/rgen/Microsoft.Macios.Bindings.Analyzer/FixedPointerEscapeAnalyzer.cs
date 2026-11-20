// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.CodeAnalysis.Operations;
using static Microsoft.Macios.Generator.RgenDiagnostics;

namespace Microsoft.Macios.Bindings.Analyzer;

/// <summary>Detects fixed pointers returned directly or through local aliases.</summary>
[DiagnosticAnalyzer (LanguageNames.CSharp)]
public class FixedPointerEscapeAnalyzer : DiagnosticAnalyzer {
	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create (RBI0043);

	public override void Initialize (AnalysisContext context)
	{
		context.ConfigureGeneratedCodeAnalysis (GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
		context.EnableConcurrentExecution ();
		context.RegisterOperationBlockAction (AnalyzeBlock);
	}

	static void AnalyzeBlock (OperationBlockAnalysisContext context)
	{
		foreach (var block in context.OperationBlocks.OfType<IBlockOperation> ()) {
			if (!block.Syntax.DescendantNodesAndSelf ().OfType<FixedStatementSyntax> ().Any ())
				continue;
			var graph = block.Parent switch {
				IMethodBodyOperation method => ControlFlowGraph.Create (method, context.CancellationToken),
				IConstructorBodyOperation constructor => ControlFlowGraph.Create (constructor, context.CancellationToken),
				null => ControlFlowGraph.Create (block, context.CancellationToken),
				_ => null,
			};
			if (graph is not null)
				AnalyzeGraph (graph, context);
		}
	}

	sealed class State {
		public readonly HashSet<ISymbol> Locals = new (SymbolEqualityComparer.Default);
		public readonly HashSet<CaptureId> Captures = new ();

		public void UnionWith (State other)
		{
			Locals.UnionWith (other.Locals);
			Captures.UnionWith (other.Captures);
		}

		public bool Equals (State other) => Locals.SetEquals (other.Locals) && Captures.SetEquals (other.Captures);

		public void Assign (IOperation target, bool pinned)
		{
			if (target is ILocalReferenceOperation local) {
				if (pinned)
					Locals.Add (local.Local);
				else
					Locals.Remove (local.Local);
			}
		}
	}

	static void AnalyzeGraph (ControlFlowGraph graph, OperationBlockAnalysisContext context)
	{
		var outputs = graph.Blocks.Select (_ => new State ()).ToArray ();
		bool changed;
		do {
			changed = false;
			foreach (var block in graph.Blocks) {
				context.CancellationToken.ThrowIfCancellationRequested ();
				var state = GetInput (block, outputs);
				ProcessBlock (block, state);
				if (!state.Equals (outputs [block.Ordinal])) {
					outputs [block.Ordinal] = state;
					changed = true;
				}
			}
		} while (changed);

		foreach (var block in graph.Blocks) {
			var state = GetInput (block, outputs);
			var pinned = ProcessBlock (block, state);
			if (pinned && block.FallThroughSuccessor?.Semantics == ControlFlowBranchSemantics.Return && block.BranchValue is { } value)
				context.ReportDiagnostic (Diagnostic.Create (RBI0043, value.Syntax.GetLocation ()));

			var operations = block.BranchValue is null ? block.Operations : block.Operations.Add (block.BranchValue);
			foreach (var operation in operations.SelectMany (o => o.DescendantsAndSelf ()).OfType<IFlowAnonymousFunctionOperation> ())
				AnalyzeGraph (graph.GetAnonymousFunctionControlFlowGraph (operation, context.CancellationToken), context);
		}
		foreach (var localFunction in graph.LocalFunctions)
			AnalyzeGraph (graph.GetLocalFunctionControlFlowGraph (localFunction, context.CancellationToken), context);
	}

	static State GetInput (BasicBlock block, State [] outputs)
	{
		var state = new State ();
		foreach (var predecessor in block.Predecessors)
			state.UnionWith (outputs [predecessor.Source.Ordinal]);
		return state;
	}

	static bool ProcessBlock (BasicBlock block, State state)
	{
		foreach (var operation in block.Operations)
			Evaluate (operation, state);
		return block.BranchValue is { } value && Evaluate (value, state);
	}

	static bool Evaluate (IOperation operation, State state)
	{
		switch (operation) {
		case ILocalReferenceOperation local:
			return local.Local.IsFixed || state.Locals.Contains (local.Local);
		case IFlowCaptureReferenceOperation capture:
			return state.Captures.Contains (capture.Id);
		case IFlowCaptureOperation capture:
			var captured = Evaluate (capture.Value, state);
			if (captured)
				state.Captures.Add (capture.Id);
			else
				state.Captures.Remove (capture.Id);
			return captured;
		case ISimpleAssignmentOperation assignment:
			var assigned = Evaluate (assignment.Value, state);
			state.Assign (assignment.Target, assigned);
			return assigned;
		case ICompoundAssignmentOperation assignment:
			var compound = Evaluate (assignment.Target, state) | Evaluate (assignment.Value, state);
			state.Assign (assignment.Target, compound);
			return compound;
		case IConversionOperation conversion:
			return Evaluate (conversion.Operand, state);
		case IBinaryOperation binary:
			var left = Evaluate (binary.LeftOperand, state);
			var right = Evaluate (binary.RightOperand, state);
			return (left || right) && (binary.OperatorKind is BinaryOperatorKind.Add or BinaryOperatorKind.Subtract)
				&& !(binary.LeftOperand.Type is IPointerTypeSymbol && binary.RightOperand.Type is IPointerTypeSymbol);
		case IAddressOfOperation address:
			// Dereferencing copies a value, but taking its address retains the pointer's origin.
			return IsPinnedAddress (address.Reference, state);
		case IObjectCreationOperation creation when IsNativeInteger (creation.Type):
			return creation.Arguments.Any (a => Evaluate (a.Value, state));
		case IInvocationOperation invocation when invocation.Instance is { } receiver && IsNativeInteger (receiver.Type)
			&& invocation.TargetMethod.Name is "ToPointer" or "ToInt32" or "ToInt64" or "ToUInt32" or "ToUInt64":
			return Evaluate (receiver, state);
		case IInvocationOperation invocation when IsNativeInteger (invocation.TargetMethod.ContainingType)
			&& invocation.TargetMethod.IsStatic && invocation.TargetMethod.Name is "Add" or "Subtract":
			return invocation.Arguments.Any (a => Evaluate (a.Value, state));
		default:
			foreach (var child in operation.ChildOperations)
				Evaluate (child, state);
			return false;
		}
	}

	static bool IsPinnedAddress (IOperation operation, State state)
	{
		if (operation is IFieldReferenceOperation field && field.Instance is { } instance)
			return IsPinnedAddress (instance, state);
		if (operation is IArrayElementReferenceOperation array)
			return Evaluate (array.ArrayReference, state);
		// Roslyn represents pointer indirection and pointer indexing with OperationKind.None.
		if (operation.Kind == OperationKind.None)
			return operation.ChildOperations.Any (child => Evaluate (child, state));
		return Evaluate (operation, state);
	}

	static bool IsNativeInteger (ITypeSymbol? type)
		=> type?.SpecialType is SpecialType.System_IntPtr or SpecialType.System_UIntPtr;
}
