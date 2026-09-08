// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// This file is needed because we compile the current project under netstandard2.0, and the types here don't exist in netstandard2.0

#if !NET

public static class QueueExtensions {
	public static bool TryDequeue<T> (this Queue<T> queue, [MaybeNullWhen (false)] out T item)
	{
		if (queue.Count == 0) {
			item = default;
			return false;
		}
		item = queue.Dequeue ();
		return true;
	}

	public static bool TryAdd<T, V> (this Dictionary<T, V> dictionary, T key, V value)
	{
		if (dictionary.ContainsKey (key))
			return false;
		dictionary.Add (key, value);
		return true;
	}
}

public static class DictionaryExtensions {
	public static bool Remove<T, V> (this Dictionary<T, V> dictionary, T key, [MaybeNullWhen (false)] out V value)
	{
		if (dictionary.TryGetValue (key, out value)) {
			dictionary.Remove (key);
			return true;
		}
		return false;
	}
}

public static class EnumerableExtensions {
	public static IEnumerable<TSource> SkipLast<TSource> (this IEnumerable<TSource> source, int count)
	{
		// very naive implementation, but it's only for netstandard2.0, which will go away soon, so no need to optimize it
		var rv = source.ToList ();
		if (rv.Count <= count)
			return [];
		rv.RemoveRange (rv.Count - count, count);
		return rv;
	}
}

// From: https://github.com/dotnet/dotnet/blob/b0f34d51fccc69fd334253924abd8d6853fad7aa/src/runtime/src/libraries/System.Private.CoreLib/src/System/Runtime/CompilerServices/RequiredMemberAttribute.cs
namespace System.Runtime.CompilerServices {
	using System.ComponentModel;

	/// <summary>Specifies that a type has required members or that a member is required.</summary>
	[AttributeUsage (AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	[EditorBrowsable (EditorBrowsableState.Never)]
	internal sealed class RequiredMemberAttribute : Attribute { }
}

// From: https://github.com/dotnet/dotnet/blob/b0f34d51fccc69fd334253924abd8d6853fad7aa/src/runtime/src/libraries/System.Private.CoreLib/src/System/Runtime/CompilerServices/CompilerFeatureRequiredAttribute.cs
namespace System.Runtime.CompilerServices {
	using System.ComponentModel;

	/// <summary>
	/// Indicates that compiler support for a particular feature is required for the location where this attribute is applied.
	/// </summary>
	[AttributeUsage (AttributeTargets.All, AllowMultiple = true, Inherited = false)]
	internal sealed class CompilerFeatureRequiredAttribute : Attribute {
		public CompilerFeatureRequiredAttribute (string featureName)
		{
			FeatureName = featureName;
		}

		/// <summary>
		/// The name of the compiler feature.
		/// </summary>
		public string FeatureName { get; }

		/// <summary>
		/// If true, the compiler can choose to allow access to the location where this attribute is applied if it does not understand <see cref="FeatureName"/>.
		/// </summary>
		public bool IsOptional { get; init; }

		/// <summary>
		/// The <see cref="FeatureName"/> used for the ref structs C# feature.
		/// </summary>
		public const string RefStructs = nameof (RefStructs);

		/// <summary>
		/// The <see cref="FeatureName"/> used for the required members C# feature.
		/// </summary>
		public const string RequiredMembers = nameof (RequiredMembers);
	}
}

// From: https://github.com/dotnet/dotnet/blob/b0f34d51fccc69fd334253924abd8d6853fad7aa/src/runtime/src/libraries/System.Private.CoreLib/src/System/Runtime/CompilerServices/IsExternalInit.cs
namespace System.Runtime.CompilerServices {
	using System.ComponentModel;

	/// <summary>
	/// Reserved to be used by the compiler for tracking metadata.
	/// This class should not be used by developers in source code.
	/// </summary>
	[EditorBrowsable (EditorBrowsableState.Never)]
	internal static class IsExternalInit {
	}
}
#endif
