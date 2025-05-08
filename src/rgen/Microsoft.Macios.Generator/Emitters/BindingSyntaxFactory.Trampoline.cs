// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
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

			//  Runtime.RetainAndAutoreleaseNSObject (NSArray.FromNSObjects(auxVariable))
			{ IsArray: true, ArrayElementTypeIsWrapped: true }
				=> RetainAndAutoreleaseNSObject ([Argument(NSArrayFromNSObjects ([Argument (IdentifierName (auxVariableName))]))]),
			
			// Runtime.RetainAndAutoreleaseNativeObject (auxVariable)
			{ IsArray: false, IsINativeObject: true, IsNSObject: false, IsInterface: false}
				=> RetainAndAutoreleaseNativeObject ([Argument(IdentifierName (auxVariableName))]),
			
			// Runtime.RetainAndAutoreleaseNSObject (auxVariable)
			{ IsArray: false, IsWrapped: true }
				=> RetainAndAutoreleaseNSObject ([Argument (IdentifierName(auxVariableName))]),
			
			//  NSString.CreateNative (auxVariable, true);
			{ SpecialType: SpecialType.System_String }
				=> NStringCreateNative ([Argument (IdentifierName(auxVariableName)), BoolArgument (true)]),
			
			// (UIntPtr) (ulong) myParam 
			{ IsNativeEnum: true }
				=> CastToNative (auxVariableName, typeInfo.Delegate.ReturnType),
			
			// auxVariable ? (byte) 1 : (byte) 0; 
			{ SpecialType: SpecialType.System_Boolean } 
				=> CastToByte (auxVariableName, typeInfo.Delegate.ReturnType),
			
			// default case, return the value as is
			_ => IdentifierName (auxVariableName),

		};
#pragma warning restore format
	}

	/// <summary>
	/// Returns the expression for the creation of the NativeInvocationClass for a given trampoline.
	/// </summary>
	/// <param name="trampolineName">The name of the trampoline whose class we want to create.</param>
	/// <param name="arguments">The arguments for pass to the create method.</param>
	/// <returns>The expression needed to create the native invocation class.</returns>
	internal static ExpressionSyntax CreateTrampolineNativeInvocationClass (string trampolineName, ImmutableArray<ArgumentSyntax> arguments)
	{
		var className = Nomenclator.GetTrampolineClassName (trampolineName, Nomenclator.TrampolineClassType.NativeInvocationClass);
		var staticClassName = IdentifierName (className);
		return StaticInvocationExpression (staticClassName, "Create", arguments, suppressNullableWarning: true);
	}

	/// <summary>
	/// Returns the argument syntax of a parameter to be used for the trampoliner to invoke a delegate.
	/// </summary>
	/// <param name="trampolineName">The name of the trampoline whose parameter we are generating.</param>
	/// <param name="parameter">The parameter whose argument syntax has to be calculated.</param>
	/// <returns>The argument syntax for the given parameter.</returns>
	internal static ArgumentSyntax GetTrampolineInvokeArgument (string trampolineName, in DelegateParameter parameter)
	{
		// build the needed expression based on the information of the parameter.
		var parameterIdentifier = IdentifierName (parameter.Name);
#pragma warning disable format
		var expression = parameter switch {
			// pointer parameter 
			{ Type.IsPointer: true } => parameterIdentifier,
			
			// parameters that are passed by reference, the nomenclator will return the name of the
			// temporary variable to use for the trampoline, there is no need for us to do anything
			{ IsByRef: true } => IdentifierName (Nomenclator.GetNameForTempTrampolineVariable (parameter) ?? parameter.Name),
			
			// delegate parameter, c callback
			// System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<ParameterType> (ParameterName)
			{ Type.IsDelegate: true, IsCCallback: true } => 
				GetDelegateForFunctionPointer (parameter.Type.FullyQualifiedName, [Argument (parameterIdentifier)]),
			
			// delegate parameter, block callback
			// TrampolineNativeInvocationClass.Create (ParameterName)!
			{ Type.IsDelegate: true, IsBlockCallback: true } 
				=> CreateTrampolineNativeInvocationClass (trampolineName, [Argument (parameterIdentifier)]),
			
			// native enum, return the conversion expression to the native type
			{ Type.IsNativeEnum: true} 
				=> CastToNative (parameter)!,
			
			// boolean, convert it to byte
			{ Type.SpecialType: SpecialType.System_Boolean } 
				=> CastToByte (parameter.Name, parameter.Type)!,
			
			// array types
			
			// CFArray.ArrayFromHandle<{0}> ({1})!
			{ Type.IsArray: true, Type.ArrayElementTypeIsWrapped: true } 
				=> GetCFArrayFromHandle (parameter.Type.FullyQualifiedName, [
					Argument (parameterIdentifier)
				], suppressNullableWarning: true), 
			
			// NSArray.ArrayFromHandle<{0}> ({1})!
			{ Type.IsArray: true, Type.ArrayElementIsINativeObject: true } 
				=> GetNSArrayFromHandle (parameter.Type.FullyQualifiedName, [
					Argument (parameterIdentifier)
				], suppressNullableWarning: true),
			
			// string[]
			// CFArray.StringArrayFromHandle (ParameterName)!
			{ Type.IsArray: true, Type.ArrayElementType: SpecialType.System_String }
				=> SuppressNullableWarning (StringArrayFromHandle ([Argument (parameterIdentifier)])),
			
			// string
			// CFString.FromHandle (ParameterName)!
			{ Type.SpecialType: SpecialType.System_String }
				=> SuppressNullableWarning (StringFromHandle ([Argument (parameterIdentifier)])),
			
			// Runtime.GetINativeObject<ParameterType> (ParameterName, false)!
			{ Type.IsProtocol: true } => 
				GetINativeObject (parameter.Type.FullyQualifiedName, [
						Argument (parameterIdentifier), 
						BoolArgument (false)
					], suppressNullableWarning: true),
			// Runtime.GetINativeObject<ParameterType> (ParameterName, true, Forced.Owns)!
			{ ForcedType: not null } => GetINativeObject (parameter.Type.FullyQualifiedName, 
				[
					Argument (parameterIdentifier),
					BoolArgument (true),
					BoolArgument (parameter.ForcedType.Value.Owns)
				], suppressNullableWarning: true),
			
			// special types
			
			// CoreMedia.CMSampleBuffer
			// {0} == IntPtr.Zero ? null! : new global::CoreMedia.CMSampleBuffer ({0}, false)
			{ Type.FullyQualifiedName: "CoreMedia.CMSampleBuffer" } =>
				IntPtrZeroCheck (parameter.Name, 
					expressionSyntax: New (parameter.Type, [Argument (parameterIdentifier), BoolArgument (false)], true), 
					suppressNullableWarning: true),
			
			// AudioToolbox.AudioBuffers
			// new global::AudioToolbox.AudioBuffers ({0})
			{ Type.FullyQualifiedName: "AudioToolbox.AudioBuffers" } =>
				New (parameter.Type, [Argument (parameterIdentifier)]),
			
			// general NSObject/INativeObject, has to be after the special types otherwise the special types will
			// fall into the NSObject/INativeObject case
			
			// Runtime.GetNSObject<ParameterType> (ParameterName)! 
			{ Type.IsNSObject: true } =>
				GetNSObject (parameter.Type.FullyQualifiedName, [
					Argument (parameterIdentifier)
				], suppressNullableWarning: true),
			
			// Runtime.GetINativeObject<ParameterType> (ParameterName, false)!
			{ Type.IsINativeObject: true } =>
				GetINativeObject (parameter.Type.FullyQualifiedName, [
					Argument (parameterIdentifier), 
					BoolArgument (false)
				], suppressNullableWarning: true),
			
			// by default, we will use the parameter name as is
			_ => parameterIdentifier
		};
#pragma warning restore format
		
		// this are arguments no parameters, therefore we do not need to add the ref modifiers
		return Argument (expression);
	}

	/// <summary>
	/// Return a immutable array of arguments to be used for the trampoline invoke method. The arguments are all
	/// the different expressions needed to pass the parameters to the trampoline.
	/// </summary>
	/// <param name="trampolineName">The trampoline whose parameters we are generating.</param>
	/// <param name="delegateInfo">The delegate info of the trampoline we are generating.</param>
	/// <returns>An immutable array with the argument expressions needed to invoke the trampoline delegate.</returns>
	internal static ImmutableArray<ArgumentSyntax> GetTrampolineInvokeArguments (string trampolineName, in DelegateInfo delegateInfo)
	{
		// create the builder for the arguments, we already know the size of the array
		var bucket = ImmutableArray.CreateBuilder<ArgumentSyntax> (delegateInfo.Parameters.Length);
		foreach (var parameter in delegateInfo.Parameters) {
			bucket.Add (GetTrampolineInvokeArgument (trampolineName, parameter));
		}
		return bucket.ToImmutable ();
	}
}
