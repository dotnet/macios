// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Generator.Emitters;

static partial class BindingSyntaxFactory {
	/// <summary>
	/// Returns the statement with the return type of the invoke method for the given type info delegate.
	/// </summary>
	/// <param name="typeInfo"></param>
	/// <param name="auxVariableName"></param>
	/// <returns></returns>
	internal static ExpressionSyntax? GetTrampolineInvokeReturnType (TypeInfo typeInfo, string auxVariableName)
	{
		// ignore those types that are not delegates or that are a delegate with a void return type
		if (!typeInfo.IsDelegate || typeInfo.Delegate.ReturnType.IsVoid)
			return null;

#pragma warning disable format
		// based on the return type of the delegate we build a statement that will return the expected value
		return typeInfo.Delegate.ReturnType switch {

			//  NSArray.FromNSObjects(auxVariable).GetHandle ()
			{ IsArray: true, ArrayElementTypeIsWrapped: true }
				=> GetHandle (NSArrayFromNSObjects ([Argument (IdentifierName (auxVariableName))])),

			// auxVariable.GetHandle ()
			{ IsArray: false, IsWrapped: true }
				=> GetHandle (IdentifierName(auxVariableName)),
			
			//  NSString.CreateNative (auxVariable, true);
			{ SpecialType: SpecialType.System_String }
				=> NStringCreateNative ([Argument (IdentifierName(auxVariableName)), BoolArgument (true)]),
			
			// (UIntPtr) (ulong) myParam 
			{ IsNativeEnum: true }
				=> CastToNative (auxVariableName, typeInfo.Delegate.ReturnType),
			
			// auxVariable.GetHandle ()
			{ IsINativeObject: true }
				=> GetHandle (IdentifierName (auxVariableName)),
			
			// auxVariable ? (byte) 1 : (byte) 0; 
			{ SpecialType: SpecialType.System_Boolean } 
				=> CastToByte (auxVariableName, typeInfo.Delegate.ReturnType),
			
			_ => null

		};
#pragma warning restore format
	}
}
