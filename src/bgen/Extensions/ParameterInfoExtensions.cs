// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Reflection;
using ObjCRuntime;

public static class ParameterInfoExtensions {
	/// <summary>
	/// Returns the type of the parameter taking into account several special cases:
	/// 1. The parameter is decorated with [BindAs].
	/// 2. The parameter is generic. 
	/// </summary>
	/// <param name="parameterInfo"></param>
	/// <param name="methodInfo"></param>
	/// <param name="bindAsAttribute"></param>
	/// <returns></returns>
	public static Type GetBindingType (this ParameterInfo parameterInfo, MethodInfo methodInfo, BindAsAttribute? bindAsAttribute = null)
	{
		if (parameterInfo.ParameterType.IsGenericParameter) {
			// a little more complicated, we need to find the source of the generic and use the constraint
			var classType = methodInfo.DeclaringType!;
			var genericType = classType.GetGenericArguments ();
			foreach (var t in genericType) {
				if (t != parameterInfo.ParameterType)
					continue;

				var constraints = t.GetGenericParameterConstraints ();
				if (constraints.Length == 0)
					return parameterInfo.ParameterType;

				// we have a constraint, we need to find the first one that is not generic
				foreach (var constraint in constraints) {
					if (!constraint.IsGenericType)
						return constraint;
				}
				// all constraints are generic, return the first one
				return constraints [0];
			}
		}
		// simplest case, we are not generic
		return bindAsAttribute is null ? parameterInfo.ParameterType : bindAsAttribute.Type;
	}
}
