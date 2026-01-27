#if !NET // the below attributes are no longer needed once we switch to .NET

// Ref: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Diagnostics/CodeAnalysis/NullableAttributes.cs

namespace System.Diagnostics.CodeAnalysis {
	[AttributeUsage (AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
	internal sealed class NotNullIfNotNullAttribute : Attribute {
		public string ParameterName { get; }

		public NotNullIfNotNullAttribute (string parameterName)
		{
			ParameterName = parameterName;
		}
	}

	/// <summary>Specifies that when a method returns <see cref="ReturnValue"/>, the parameter will not be null even if the corresponding type allows it.</summary>
	[AttributeUsage (AttributeTargets.Parameter, Inherited = false)]
	internal sealed class NotNullWhenAttribute : Attribute {
		/// <summary>Initializes the attribute with the specified return value condition.</summary>
		/// <param name="returnValue">
		/// The return value condition. If the method returns this value, the associated parameter will not be null.
		/// </param>
		public NotNullWhenAttribute (bool returnValue) => ReturnValue = returnValue;

		/// <summary>Gets the return value condition.</summary>
		public bool ReturnValue { get; }
	}

	/// <summary>Specifies that the method or property will ensure that the listed field and property members have not-null values.</summary>
	[AttributeUsage (AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	internal sealed class MemberNotNullAttribute : Attribute {
		/// <summary>Initializes the attribute with a field or property member.</summary>
		/// <param name="member">
		/// The field or property member that is promised to be not-null.
		/// </param>
		public MemberNotNullAttribute (string member) => Members = [member];

		/// <summary>Initializes the attribute with the list of field and property members.</summary>
		/// <param name="members">
		/// The list of field and property members that are promised to be not-null.
		/// </param>
		public MemberNotNullAttribute (params string [] members) => Members = members;

		/// <summary>Gets field or property member names.</summary>
		public string [] Members { get; }
	}
}

#endif // !NET
