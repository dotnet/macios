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
			// This only applies to generic delegate types (Action<...>). For non-generic delegates,
			// the NullableAttribute on the outer parameter describes the delegate instance, not
			// the Invoke parameters.
			var outerParam = mi.GetParameters ().Last ();
			var genericArgs = lastType.GetGenericArguments ();
			var nullabilityBytes = genericArgs.Length > 0 ? generator.AttributeManager.GetNullabilityBytes (outerParam) : null;
			if (nullabilityBytes is not null && nullabilityBytes.Length == 1) {
				// Single-byte (uniform) form: the same byte applies to all positions
				IsNSErrorNullable = nullabilityBytes [0] == 2;
			} else if (nullabilityBytes is not null && nullabilityBytes.Length > 1) {
				// Multi-byte form: walk the type arguments depth-first to find the byte
				// index for the last param (NSError). byte[0] is for the Action<> itself.
				int byteIndex = 1;
				for (int i = 0; i < genericArgs.Length; i++) {
					if (i == genericArgs.Length - 1) {
						// This is the last generic argument (NSError), which is a reference type
						IsNSErrorNullable = byteIndex < nullabilityBytes.Length && nullabilityBytes [byteIndex] == 2;
						break;
					}
					// Advance the byte index past this argument's subtree
					byteIndex += CountNullabilityBytes (genericArgs [i]);
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

	// Counts how many nullability bytes a type subtree consumes in depth-first order.
	// Value types consume 0 bytes (unless they are generic and contain reference type args).
	// Reference types consume 1 byte for themselves, plus bytes for their generic args/element types.
	static int CountNullabilityBytes (Type type)
	{
		if (type.IsValueType) {
			// Value types don't consume a byte themselves, but their generic args might
			var vtargs = type.GetGenericArguments ();
			int count = 0;
			foreach (var arg in vtargs)
				count += CountNullabilityBytes (arg);
			return count;
		}

		// Reference types consume 1 byte for themselves
		int bytes = 1;

		if (type.IsArray) {
			var elementType = type.GetElementType ();
			if (elementType is not null)
				bytes += CountNullabilityBytes (elementType);
		} else {
			var targs = type.GetGenericArguments ();
			foreach (var arg in targs)
				bytes += CountNullabilityBytes (arg);
		}

		return bytes;
	}

}
