using System;
using System.Linq;
using System.Reflection;

#nullable enable

class AsyncMethodInfo : MemberInformation {
	public ParameterInfo [] AsyncInitialParams { get; }
	public ParameterInfo [] AsyncCompletionParams { get; }
	public bool HasNSError { get; }
	public bool IsNSErrorNullable { get; }
	public bool IsVoidAsync { get; }
	public bool IsSingleArgAsync { get; }
	public MethodInfo MethodInfo { get; }

	public AsyncMethodInfo (Generator generator, IMemberGatherer gather, Type type, MethodInfo mi, Type? categoryExtensionType, bool isExtensionMethod)
		: base (generator, gather, mi, type, categoryExtensionType, false, isExtensionMethod)
	{
		this.MethodInfo = mi;
		this.AsyncInitialParams = mi.GetParameters ().DropLast ();

		var lastType = mi.GetParameters ().Last ().ParameterType;
		if (!lastType.IsSubclassOf (generator.TypeCache.System_Delegate))
			throw new BindingException (1036, true, mi.DeclaringType?.FullName, mi.Name, lastType.FullName);
		var cbParams = lastType.GetMethod ("Invoke")?.GetParameters () ?? Array.Empty<ParameterInfo> ();
		AsyncCompletionParams = cbParams;

		var lastParam = cbParams.LastOrDefault ();
		if (lastParam is not null && lastParam.ParameterType.Name == "NSError") {
			HasNSError = true;
			// The nullability info for generic type arguments is encoded in the NullableAttribute
			// on the outer method parameter (the one with the Action<...> type), not on the
			// delegate's Invoke method parameters. Check the nullability bytes to determine if
			// the NSError type argument is nullable.
			var outerParam = mi.GetParameters ().Last ();
			var nullabilityBytes = generator.AttributeManager.GetNullabilityBytes (outerParam);
			if (nullabilityBytes is not null && nullabilityBytes.Length > 1) {
				// Walk the type arguments depth-first to find the byte index for the last param.
				// Value types don't consume bytes, reference types do.
				// byte[0] is for the Action<> itself, then each reference type arg consumes one byte.
				int byteIndex = 1; // start after the outer type byte
				var genericArgs = lastType.GetGenericArguments ();
				for (int i = 0; i < genericArgs.Length; i++) {
					if (!genericArgs [i].IsValueType) {
						if (i == genericArgs.Length - 1) {
							// This is the last generic argument (NSError)
							IsNSErrorNullable = byteIndex < nullabilityBytes.Length && nullabilityBytes [byteIndex] == 2;
						}
						byteIndex++;
					}
				}
			} else {
				IsNSErrorNullable = generator.AttributeManager.IsNullable (lastParam);
			}
			cbParams = cbParams.DropLast ();
		}

		IsVoidAsync = cbParams.Length == 0;
		IsSingleArgAsync = cbParams.Length == 1;
	}

	public string GetUniqueParamName (string suggestion)
	{
		while (true) {
			bool next = false;

			foreach (var pi in AsyncCompletionParams) {
				if (pi.Name == suggestion) {
					next = true;
					break;
				}
			}

			if (!next)
				return suggestion;

			suggestion = "_" + suggestion;
		}
	}

}
