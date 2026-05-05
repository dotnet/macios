// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// #define LOG_TRIMMABLE_TYPEMAP

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading;

using Xamarin.Bundler;

namespace ObjCRuntime;

// The trimmable static registrar makes this type public when needed.
abstract class NSObjectProxyAttribute : Attribute {
	protected NSObjectProxyAttribute () { }

	public abstract NSObject? CreateObject (IntPtr handle);
	public abstract IntPtr GetClassHandle (out bool is_custom_type);
	public abstract IntPtr LookupUnmanagedFunction (string? name);
}

// The trimmable static registrar makes this type public when needed.
abstract class ProtocolProxyAttribute : Attribute {
	public abstract INativeObject? CreateObject (IntPtr handle, bool owns);
	public abstract string? GetProtocolName ();
}

// The trimmable static registrar makes this type public when needed.
abstract class INativeObjectProxyAttribute : Attribute {
	public abstract INativeObject? CreateObject (IntPtr handle, bool owns);
}

// The trimmable static registrar makes this type public when needed.
sealed class SkippedObjectiveCTypeUniverse {
	SkippedObjectiveCTypeUniverse () { }
}

static class TypeMaps {
#if LOG_TRIMMABLE_TYPEMAP
	static void PreDump ()
	{
		Console.WriteLine ($"TypeMaps.Initialize ()");
		AppDomain.CurrentDomain.AssemblyLoad += (sender, args) => {
			Console.WriteLine ($"AssemblyLoad (): {args.LoadedAssembly} => {args.LoadedAssembly.Location}");
		};
		AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => {
			Console.WriteLine ($"AssemblyResolve (): {args.Name} failed to load (by {args.RequestingAssembly})");
			return null;
		};
		AppDomain.CurrentDomain.FirstChanceException += (sender, args) => {
			Console.WriteLine ($"FirstChanceException ({args.Exception}):\n{args.Exception.StackTrace})");
		};
	}

	static void PostDump ()
	{
		Console.WriteLine ($"System.Runtime.InteropServices.TypeMappingEntryAssembly: {AppContext.GetData ("System.Runtime.InteropServices.TypeMappingEntryAssembly")}");
		Dump ("NSObjectTypes", NSObjectTypes);
		Dump ("SkippedProxyTypes", SkippedProxyTypes);
		Dump ("NSObjectProxyTypes", NSObjectProxyTypes);
		Dump ("INativeObjectProxyTypes", INativeObjectProxyTypes);
		Dump ("ProtocolProxyTypes", ProtocolProxyTypes);
		Dump ("ProtocolWrapperTypes", ProtocolWrapperTypes);
		foreach (var asm in AppDomain.CurrentDomain.GetAssemblies ()) {
			Console.WriteLine ($"Loaded assembly: {asm}");
		}
		Console.WriteLine ($"TypeMaps.Initialize () DONE");
	}

	static void Dump (string name, IReadOnlyDictionary<string, Type> dict)
	{
		var precachedModules = (System.Collections.IList?) dict.GetType ().GetField ("_preCachedModules", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue (dict);
		var lazyData = (System.Collections.IDictionary?) dict.GetType ().GetField ("_lazyData", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue (dict);
		Console.WriteLine ($"Dictionary '{name}':");
		if (precachedModules is not null) {
			if (precachedModules.Count > 0) {
				Console.WriteLine ($"    {precachedModules.Count} precached modules:");
				foreach (Module mod in precachedModules)
					Console.WriteLine ($"    {mod.Name}");
			} else {
				Console.WriteLine ($"    No precached modules.");
			}
		}
		if (lazyData is not null) {
			Console.WriteLine ($"    {lazyData.Keys.Count} lazy data entries");
		} else {
			Console.WriteLine ($"    No lazy data entries.");
			var fields = dict.GetType ().GetFields (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			Console.WriteLine ($"    Got dictionary of type '{dict.GetType ()}' with {fields.Length} fields: {dict}");
			foreach (var field in fields) {
				var value = field.GetValue (dict);
				Console.WriteLine ($"        Field '{field.Name}': {field}");
			}
		}
	}

	static void Dump (string name, IReadOnlyDictionary<Type, Type> dict)
	{
		var precachedModules = (System.Collections.IList?) dict.GetType ().GetField ("_preCachedModules", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue (dict);
		var lazyData = (System.Collections.IDictionary?) dict.GetType ().GetField ("_lazyData", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue (dict);
		Console.WriteLine ($"Dictionary '{name}':");
		if (precachedModules is not null) {
			if (precachedModules.Count > 0) {
				Console.WriteLine ($"    {precachedModules.Count} precached modules:");
				foreach (Module mod in precachedModules)
					Console.WriteLine ($"    {mod.Name}");
			} else {
				Console.WriteLine ($"    No precached modules.");
			}
		}
		if (lazyData is not null) {
			Console.WriteLine ($"    {lazyData.Keys.Count} lazy data entries");
		} else {
			Console.WriteLine ($"    No lazy data entries.");
			var fields = dict.GetType ().GetFields (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			Console.WriteLine ($"    Got dictionary of type '{dict.GetType ()}' with {fields.Length} fields: {dict}");
			foreach (var field in fields) {
				var value = field.GetValue (dict);
				Console.WriteLine ($"        Field '{field.Name}': {field}");
			}
		}
	}
#endif // LOG_TRIMMABLE_TYPEMAP

#if NET11_0_OR_GREATER
#pragma warning disable 8618 // "Non-nullable field '...' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.": we make sure through other means that these will never be null
	internal static IReadOnlyDictionary<string, Type> NSObjectTypes;
	internal static IReadOnlyDictionary<Type, Type> SkippedProxyTypes;
	internal static IReadOnlyDictionary<Type, Type> NSObjectProxyTypes;
	internal static IReadOnlyDictionary<Type, Type> INativeObjectProxyTypes;
	internal static IReadOnlyDictionary<Type, Type> ProtocolProxyTypes;
	internal static IReadOnlyDictionary<Type, Type> ProtocolWrapperTypes;
#pragma warning restore 8618

	internal static void Initialize ()
	{
#if LOG_TRIMMABLE_TYPEMAP
		PreDump ();
#endif

		NSObjectTypes = TypeMapping.GetOrCreateExternalTypeMapping<NSObject> ();
		SkippedProxyTypes = TypeMapping.GetOrCreateProxyTypeMapping<SkippedObjectiveCTypeUniverse> ();
		NSObjectProxyTypes = TypeMapping.GetOrCreateProxyTypeMapping<NSObject> ();
		INativeObjectProxyTypes = TypeMapping.GetOrCreateProxyTypeMapping<INativeObject> ();
		ProtocolProxyTypes = TypeMapping.GetOrCreateProxyTypeMapping<ProtocolProxyAttribute> ();
		ProtocolWrapperTypes = TypeMapping.GetOrCreateProxyTypeMapping<ProtocolAttribute> ();

#if LOG_TRIMMABLE_TYPEMAP
		PostDump ();
#endif
	}
#else
	static IReadOnlyDictionary<string, Type>? nsobject_types;
	internal static IReadOnlyDictionary<string, Type> NSObjectTypes {
		get {
			if (nsobject_types is null)
				Initialize ();
			return nsobject_types;
		}
	}

	static IReadOnlyDictionary<Type, Type>? skipped_proxy_types;
	internal static IReadOnlyDictionary<Type, Type> SkippedProxyTypes {
		get {
			if (skipped_proxy_types is null)
				Initialize ();
			return skipped_proxy_types;
		}
	}

	static IReadOnlyDictionary<Type, Type>? nsobject_proxy_types;
	internal static IReadOnlyDictionary<Type, Type> NSObjectProxyTypes {
		get {
			if (nsobject_proxy_types is null)
				Initialize ();
			return nsobject_proxy_types;
		}
	}

	static IReadOnlyDictionary<Type, Type>? inativeobject_proxy_types;
	internal static IReadOnlyDictionary<Type, Type> INativeObjectProxyTypes {
		get {
			if (inativeobject_proxy_types is null)
				Initialize ();
			return inativeobject_proxy_types;
		}
	}

	static IReadOnlyDictionary<Type, Type>? protocol_proxy_types;
	internal static IReadOnlyDictionary<Type, Type> ProtocolProxyTypes {
		get {
			if (protocol_proxy_types is null)
				Initialize ();
			return protocol_proxy_types;
		}
	}

	static IReadOnlyDictionary<Type, Type>? protocol_wrapper_types;
	internal static IReadOnlyDictionary<Type, Type> ProtocolWrapperTypes {
		get {
			if (protocol_wrapper_types is null)
				Initialize ();
			return protocol_wrapper_types;
		}
	}

	static readonly Lock lock_obj = new Lock ();

	[MemberNotNull (nameof (nsobject_types))]
	[MemberNotNull (nameof (skipped_proxy_types))]
	[MemberNotNull (nameof (nsobject_proxy_types))]
	[MemberNotNull (nameof (inativeobject_proxy_types))]
	[MemberNotNull (nameof (protocol_proxy_types))]
	[MemberNotNull (nameof (protocol_wrapper_types))]
	internal static void Initialize ()
	{
		// In .NET 10 we can only create the type maps from the entry assembly, which can only be done after calling the
		// main assembly's Main method - so we need to create the type maps on demand, instead of from Runtime.Initialize.
		// For reference, this is what happens:
		//     System.InvalidOperationException: Entry assembly is required but was not found.
		//         at System.Runtime.InteropServices.TypeMapLazyDictionary.CreateMaps(RuntimeType groupType,  newExternalTypeEntry,  newProxyTypeEntry)
		//         at System.Runtime.InteropServices.TypeMapLazyDictionary.CreateExternalTypeMap(RuntimeType groupType)
		lock (lock_obj) {
			if (nsobject_types is null) {
#if LOG_TRIMMABLE_TYPEMAP
				PreDump ();
#endif
				nsobject_types = TypeMapping.GetOrCreateExternalTypeMapping<NSObject> ();
			}

			if (skipped_proxy_types is null)
				skipped_proxy_types = TypeMapping.GetOrCreateProxyTypeMapping<SkippedObjectiveCTypeUniverse> ();

			if (nsobject_proxy_types is null)
				nsobject_proxy_types = TypeMapping.GetOrCreateProxyTypeMapping<NSObject> ();

			if (inativeobject_proxy_types is null)
				inativeobject_proxy_types = TypeMapping.GetOrCreateProxyTypeMapping<INativeObject> ();

			if (protocol_proxy_types is null)
				protocol_proxy_types = TypeMapping.GetOrCreateProxyTypeMapping<ProtocolProxyAttribute> ();

			if (protocol_wrapper_types is null) {
				protocol_wrapper_types = TypeMapping.GetOrCreateProxyTypeMapping<ProtocolAttribute> ();
#if LOG_TRIMMABLE_TYPEMAP
				PostDump ();
#endif
			}
		}
	}
#endif // NET11_0_OR_GREATER

	internal static bool TryGetProtocolProxyAttribute (Type protocol, [NotNullWhen (true)] out ProtocolProxyAttribute? proxyAttribute)
	{
		proxyAttribute = null;

		if (ProtocolProxyTypes.TryGetValue (protocol, out var protocolProxyType)) {
#if LOG_TRIMMABLE_TYPEMAP
			Runtime.NSLog ($"TryGetProtocolProxyAttribute ({protocol}) found proxy type {protocolProxyType} in protocol proxy map");
#endif
			proxyAttribute = protocolProxyType.GetCustomAttribute<ProtocolProxyAttribute> ();
			if (proxyAttribute is null)
				throw ErrorHelper.CreateError (8062, Errors.MX8062 /* Type '{0}' is expected to have a ProtocolProxyAttribute. Please file a bug report with a test case (https://github.com/dotnet/macios/issues/new). */, protocolProxyType.FullName);
			return proxyAttribute is not null;
		}

		// workaround for https://github.com/dotnet/runtime/issues/127004
		proxyAttribute = protocol.GetCustomAttribute<ProtocolProxyAttribute> (false);
		if (proxyAttribute is not null) {
#if LOG_TRIMMABLE_TYPEMAP
			Runtime.NSLog ($"TryGetProtocolProxyAttribute ({protocol}) found proxy attribute on the protocol type itself");
#endif
			return true;
		}

#if LOG_TRIMMABLE_TYPEMAP
		Runtime.NSLog ($"TryGetProtocolProxyAttribute ({protocol}) did not find proxy attribute anywhere");
#endif
		// end workaround for https://github.com/dotnet/runtime/issues/127004

		return false;
	}

	internal static bool IsSkippedType (Type type, [NotNullWhen (true)] out Type? actualType)
	{
		var potentiallySkippedType = type;
		if (potentiallySkippedType.IsGenericType)
			potentiallySkippedType = potentiallySkippedType.GetGenericTypeDefinition ();

		var rv = SkippedProxyTypes.TryGetValue (potentiallySkippedType, out actualType);

#if LOG_TRIMMABLE_TYPEMAP
		Runtime.NSLog ($"IsSkippedType ({type}, {actualType}) looked for '{potentiallySkippedType}' => {rv}");
#endif

		return rv;
	}

	internal static bool TryCreateInstanceUsingProxyTypeAttribute<T> (Type type, IntPtr ptr, bool owns, [NotNullWhen (true)] out T? instance) where T : INativeObject
	{
		instance = default;

		if (!TryGetNSObjectProxyAttribute (type, out var proxyAttribute)) {
#if LOG_TRIMMABLE_TYPEMAP
			Runtime.NSLog ($"TryCreateInstanceUsingProxyTypeAttribute<{typeof (T).FullName}> ({type}, 0x{@ptr:X}, {owns}) did not find proxy attribute type '{type.FullName}'");
#endif
			return false;
		}

		var obj = proxyAttribute.CreateObject (ptr);
		if (obj is null) {
#if LOG_TRIMMABLE_TYPEMAP
			Runtime.NSLog ($"TryCreateInstanceUsingProxyTypeAttribute<{typeof (T).FullName}> ({type}, 0x{@ptr:X}, {owns}) found proxy attribute of type {proxyAttribute.GetType ()}, but its CreateObject method returned null.");
#endif
			return false;
		}

		if (owns)
			Runtime.TryReleaseINativeObject (obj);

		if (obj is not T objT) {
#if LOG_TRIMMABLE_TYPEMAP
			Runtime.NSLog ($"TryCreateInstanceUsingProxyTypeAttribute<{typeof (T).FullName}> ({type}, 0x{@ptr:X}, {owns}) found proxy attribute of type {proxyAttribute.GetType ()}, and an object was created of type {obj.GetType ()}, but that's not compatible with the target type {typeof (T)}.");
#endif
			return false;
		}

		instance = objT;

		return true;
	}

	internal static bool TryGetNSObjectProxyAttribute (Type managedType, [NotNullWhen (true)] out NSObjectProxyAttribute? proxyAttribute)
	{
		proxyAttribute = null;

		// workaround for https://github.com/dotnet/runtime/issues/127004
		proxyAttribute = managedType.GetCustomAttribute<NSObjectProxyAttribute> (false);
		if (proxyAttribute is not null) {
#if LOG_TRIMMABLE_TYPEMAP
			Runtime.NSLog ($"TryGetNSObjectProxyAttribute ({managedType}): found proxy attribute on the type itself");
#endif
			return true;
		}
#if LOG_TRIMMABLE_TYPEMAP
		Runtime.NSLog ($"TryGetNSObjectProxyAttribute ({managedType}): did not find proxy attribute on the type itself");
#endif
		// end workaround for https://github.com/dotnet/runtime/issues/127004

		if (!NSObjectProxyTypes.TryGetValue (managedType, out var proxyType)) {
#if LOG_TRIMMABLE_TYPEMAP
			Runtime.NSLog ($"TryGetNSObjectProxyAttribute ({managedType}) found in NSObjectTypes type map, but proxy type in NSObjectProxyTypes not found");
#endif
			return false;
		}

		proxyAttribute = proxyType.GetCustomAttribute<NSObjectProxyAttribute> ();
		if (proxyAttribute is null) {
#if LOG_TRIMMABLE_TYPEMAP
			Runtime.NSLog ($"TryGetNSObjectProxyAttribute ({managedType}) found in proxy type map, but could not create proxy attribute for it");
#endif
			return false;
		}

		return proxyAttribute is not null;
	}

	internal static bool TryGetNSObjectProxyAttribute (string? className, [NotNullWhen (true)] out NSObjectProxyAttribute? proxyAttribute, [NotNullWhen (true)] out Type? managedType)
	{
		proxyAttribute = null;
		managedType = null;

		if (string.IsNullOrEmpty (className)) {
#if LOG_TRIMMABLE_TYPEMAP
			Runtime.NSLog ($"GetTrimmableProxyTypeAttribute ({className}) = no class name provided");
#endif
			return false;
		}

		if (!TypeMaps.NSObjectTypes.TryGetValue (className, out managedType)) {
#if LOG_TRIMMABLE_TYPEMAP
			Runtime.NSLog ($"GetTrimmableProxyTypeAttribute ({className}) Objective-C class \"{className}\" not found in NSObjectTypes type map");
#endif
			return false;
		}

		return TryGetNSObjectProxyAttribute (managedType, out proxyAttribute);
	}
}

