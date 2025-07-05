// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using ObjCRuntime;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Generator.Attributes;

/// <summary>
/// Represents the data found in an ExportAttribute&lt;T&gt; 
/// </summary>
/// <typeparam name="T">The configuration flags used on the exported element.</typeparam>
readonly struct ExportData<T> : IEquatable<ExportData<T>> where T : Enum {

	/// <summary>
	/// The exported native selector.
	/// </summary>
	public string? Selector { get; }

	/// <summary>
	/// The configuration flags used on the exported member.
	/// </summary>
	public T? Flags { get; init; }

	/// <summary>
	/// Argument semantics to use with the selector.
	/// </summary>
	public ArgumentSemantic ArgumentSemantic { get; } = ArgumentSemantic.None;

	/// <summary>
	/// Get the native prefix to be used in the custom marshal directive.
	///
	/// Should only be present with the CustomeMarshalDirective flag.
	/// </summary >
	public string? NativePrefix { get; init; }

	/// <summary>
	/// Get the native sufix to be used in the custom marshal directive.
	///
	/// Should only be present with the CustomeMarshalDirective flag.
	/// </summary >
	public string? NativeSuffix { get; init; }

	/// <summary>
	/// Get the library to be used in the custom marshal directive.
	///
	/// Should only be present with the CustomeMarshalDirective flag.
	/// </summary >
	public string? Library { get; init; }

	// async related data. This data will only be present if the ExportAttribute is a Method AND has a 
	// Async flag set. Otherwise they will be ignored. All this methods have to be named parameters

	/// <summary>
	/// The type of the result for an async method.
	/// </summary>
	public TypeInfo? ResultType { get; init; }

	/// <summary>
	/// The name of the generated async method.
	/// </summary>
	public string? MethodName { get; init; }

	/// <summary>
	/// The name of the type of the result for an async method.
	/// </summary>
	public string? ResultTypeName { get; init; }

	/// <summary>
	/// A code snippet to be executed after the async method call.
	/// </summary>
	public string? PostNonResultSnippet { get; init; }

	/// <summary>
	/// The type of the strong delegate for a weak delegate property.
	/// </summary>
	public TypeInfo? StrongDelegateType { get; init; }

	/// <summary>
	/// The name of the strong delegate for a weak delegate property.
	/// </summary>
	public string? StrongDelegateName { get; init; }

	public ExportData () { }

	public ExportData (string? selector)
	{
		Selector = selector;
	}

	public ExportData (string? selector, ArgumentSemantic argumentSemantic)
	{
		Selector = selector;
		ArgumentSemantic = argumentSemantic;
	}

	public ExportData (string? selector, ArgumentSemantic argumentSemantic, T flags)
	{
		Selector = selector;
		ArgumentSemantic = argumentSemantic;
		Flags = flags;
	}

	/// <summary>
	/// Try to parse the attribute data to retrieve the information of an ExportAttribute&lt;T&gt;.
	/// </summary>
	/// <param name="attributeData">The attribute data to be parsed.</param>
	/// <param name="data">The parsed data. Null if we could not parse the attribute data.</param>
	/// <returns>True if the data was parsed.</returns>
	public static bool TryParse (AttributeData attributeData,
		[NotNullWhen (true)] out ExportData<T>? data)
	{
		data = null;
		var count = attributeData.ConstructorArguments.Length;
		string? selector = null;
		ArgumentSemantic argumentSemantic = ArgumentSemantic.None;
		T? flags = default;

		// custom marshal directive values
		string? nativePrefix = null;
		string? nativeSuffix = null;
		string? library = null;
		// async related data
		TypeInfo? resultType = null;
		string? methodName = null;
		string? resultTypeName = null;
		string? postNonResultSnippet = null;
		// weak delegate related data
		TypeInfo? strongDelegateType = null;
		string? strongDelegateName = null;

		switch (count) {
		case 1:
			selector = (string?) attributeData.ConstructorArguments [0].Value!;
			break;
		case 2:
			// there are two possible cases in this situation.
			// 1. The second argument is an ArgumentSemantic
			// 2. The second argument is a T
			if (attributeData.ConstructorArguments [1].Type?.Name == nameof (ObjCRuntime.ArgumentSemantic)) {
				selector = (string?) attributeData.ConstructorArguments [0].Value!;
				argumentSemantic = (ArgumentSemantic) attributeData.ConstructorArguments [1].Value!;
			} else {
				selector = (string?) attributeData.ConstructorArguments [0].Value!;
				argumentSemantic = ArgumentSemantic.None;
				flags = (T) attributeData.ConstructorArguments [1].Value!;
			}
			break;
		case 3:
			selector = (string?) attributeData.ConstructorArguments [0].Value!;
			argumentSemantic = (ArgumentSemantic) attributeData.ConstructorArguments [1].Value!;
			flags = (T) attributeData.ConstructorArguments [2].Value!;
			break;
		default:
			// 0 should not be an option..
			return false;
		}

		if (attributeData.NamedArguments.Length == 0) {
			data = flags is not null ?
				new (selector, argumentSemantic, flags) : new (selector, argumentSemantic);
			return true;
		}

		// convert the attrs names to a dictionary to avoid multiple lookups
		var attrsDict = attributeData.NamedArguments
			.ToDictionary (x => x.Key, x => x.Value.Value);
		if (attrsDict.TryGetValue ("Flags", out var flagsValue)) {
			// set the flags first, so we can check if the export is an async method
			flags = (T) flagsValue!;
		}

		// from this point we have to check the name of the argument AND if the export method is an Async method.
		var isAsync = typeof (T) == typeof (ObjCBindings.Method) && flags is not null && flags.HasFlag (ObjCBindings.Method.Async);
		var isWeakDelegate = typeof (T) == typeof (ObjCBindings.Property) && flags is not null && flags.HasFlag (ObjCBindings.Property.WeakDelegate);

		// loop over all the named arguments and set the data accordingly, ignore the Flags one since we already set it
		foreach (var (name, value) in attrsDict) {
			switch (name) {
			case "Selector":
				selector = (string?) value!;
				break;
			case "ArgumentSemantic":
				argumentSemantic = (ArgumentSemantic) value!;
				break;
			case "NativePrefix":
				nativePrefix = (string?) value!;
				break;
			case "NativeSuffix":
				nativeSuffix = (string?) value!;
				break;
			case "Library":
				library = (string?) value!;
				break;
			case "Flags":
				// we already set the flags, so we can ignore this one
				break;
			// async related data
			case "ResultType":
				if (isAsync) {
					resultType = new ((INamedTypeSymbol) value!);
				}
				break;
			case "MethodName":
				if (isAsync) {
					methodName = (string?) value!;
				}
				break;
			case "ResultTypeName":
				if (isAsync) {
					resultTypeName = (string?) value!;
				}
				break;
			case "PostNonResultSnippet":
				if (isAsync) {
					postNonResultSnippet = (string?) value;
				}
				break;
			// weak delegate related data
			case "StrongDelegateType":
				if (isWeakDelegate) {
					strongDelegateType = new ((INamedTypeSymbol) value!);
				}
				break;
			case "StrongDelegateName":
				if (isWeakDelegate) {
					strongDelegateName = (string?) value!;
				}
				break;
			default:
				data = null;
				return false;
			}
		}

		if (flags is not null) {
			data = new (selector, argumentSemantic, flags) {
				NativePrefix = nativePrefix,
				NativeSuffix = nativeSuffix,
				Library = library,
				// set the data for async methods only if the flags are set
				ResultType = isAsync ? resultType : null,
				MethodName = isAsync ? methodName : null,
				ResultTypeName = isAsync ? resultTypeName : null,
				PostNonResultSnippet = isAsync ? postNonResultSnippet : null,
				// we set the data for the weak delegate only if the flags are set
				StrongDelegateType = isWeakDelegate ? strongDelegateType : null,
				StrongDelegateName = isWeakDelegate ? strongDelegateName : null
			};
			return true;
		}

		data = new (selector, argumentSemantic) {
			NativePrefix = nativePrefix,
			NativeSuffix = nativeSuffix,
			Library = library
		};
		return true;
	}

	/// <inheritdoc />
	public bool Equals (ExportData<T> other)
	{
		if (Selector != other.Selector)
			return false;
		if (ArgumentSemantic != other.ArgumentSemantic)
			return false;
		if (NativePrefix != other.NativePrefix)
			return false;
		if (NativeSuffix != other.NativeSuffix)
			return false;
		if (Library != other.Library)
			return false;
		if (ResultType != other.ResultType)
			return false;
		if (MethodName != other.MethodName)
			return false;
		if (ResultTypeName != other.ResultTypeName)
			return false;
		if (PostNonResultSnippet != other.PostNonResultSnippet)
			return false;
		return (Flags, other.Flags) switch {
			(null, null) => true,
			(null, _) => false,
			(_, null) => false,
			(_, _) => Flags!.Equals (other.Flags)
		};
	}

	/// <inheritdoc />
	public override bool Equals (object? obj)
	{
		return obj is ExportData<T> other && Equals (other);
	}

	/// <inheritdoc />
	public override int GetHashCode ()
	{
		return HashCode.Combine (Selector, Flags);
	}

	public static bool operator == (ExportData<T> x, ExportData<T> y)
	{
		return x.Equals (y);
	}

	public static bool operator != (ExportData<T> x, ExportData<T> y)
	{
		return !(x == y);
	}

	/// <inheritdoc />
	public override string ToString ()
	{
		var sb = new StringBuilder ("{ Type: '");
		sb.Append (typeof (T).FullName);
		sb.Append ("', Selector: '");
		sb.Append (Selector ?? "null");
		sb.Append ("', ArgumentSemantic: '");
		sb.Append (ArgumentSemantic);
		sb.Append ("', Flags: '");
		sb.Append (Flags);
		sb.Append ("', NativePrefix: '");
		sb.Append (NativePrefix ?? "null");
		sb.Append ("', NativeSuffix: '");
		sb.Append (NativeSuffix ?? "null");
		sb.Append ("', Library: '");
		sb.Append (Library ?? "null");
		sb.Append ("', ResultType: '");
		sb.Append (ResultType?.FullyQualifiedName ?? "null");
		sb.Append ("', MethodName: '");
		sb.Append (MethodName ?? "null");
		sb.Append ("', ResultTypeName: '");
		sb.Append (ResultTypeName ?? "null");
		sb.Append ("', PostNonResultSnippet: '");
		sb.Append (PostNonResultSnippet ?? "null");
		sb.Append ("' }");
		return sb.ToString ();
	}
}
