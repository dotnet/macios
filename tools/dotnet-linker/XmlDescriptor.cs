// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using Mono.Cecil;

#nullable enable

namespace Xamarin.Linker.Steps {

	// Builds an ILLink root-descriptor XML (see
	// https://github.com/dotnet/runtime/blob/main/docs/tools/illink/data-formats.md#descriptor-format)
	// that preserves types, methods and fields. Collect the members to preserve (PreserveMethod,
	// PreserveField, PreserveTypeWithAllMembers, PreserveTypeFields), then either create the XML
	// document (CreateXml) or save it to a file (Save).
	public class XmlDescriptor {
		sealed class TypeEntry {
			public TypeEntry (TypeDefinition type)
			{
				Type = type;
			}

			public TypeDefinition Type { get; }
			public bool PreserveAllMembers { get; set; }
			public bool PreserveOnlyFields { get; set; }
			// Whether the type itself is required. A type that only holds conditionally-preserved
			// members isn't required on its own, and is emitted with required="false".
			public bool Required { get; set; }
			// Field name -> required.
			public Dictionary<string, bool> Fields { get; } = new (System.StringComparer.Ordinal);
			// Method signature -> (required, method).
			public Dictionary<string, (bool Required, MethodDefinition Method)> Methods { get; } = new (System.StringComparer.Ordinal);
		}

		// Assembly name -> type full name -> type entry.
		readonly Dictionary<string, Dictionary<string, TypeEntry>> entries = new (System.StringComparer.Ordinal);

		public bool IsEmpty => entries.Count == 0;

		TypeEntry GetOrCreateEntry (TypeDefinition type)
		{
			var assemblyName = type.Module.Assembly.Name.Name;
			if (!entries.TryGetValue (assemblyName, out var types)) {
				types = new Dictionary<string, TypeEntry> (System.StringComparer.Ordinal);
				entries.Add (assemblyName, types);
			}

			if (!types.TryGetValue (type.FullName, out var entry)) {
				entry = new TypeEntry (type);
				types.Add (type.FullName, entry);
			}

			return entry;
		}

		// Preserve a type and all of its members (preserve="all").
		public void PreserveTypeWithAllMembers (TypeDefinition type)
		{
			var entry = GetOrCreateEntry (type);
			entry.PreserveAllMembers = true;
			entry.Required = true;
		}

		// Preserve a type and its fields (preserve="fields"). Typically used for enums.
		public void PreserveTypeFields (TypeDefinition type)
		{
			var entry = GetOrCreateEntry (type);
			entry.PreserveOnlyFields = true;
			entry.Required = true;
		}

		// Preserve a type on its own (preserve="nothing", but the type is kept).
		public void PreserveType (TypeDefinition type)
		{
			var entry = GetOrCreateEntry (type);
			entry.Required = true;
		}

		// Preserve a single method. A required (unconditional) method also makes its declaring type required.
		public void PreserveMethod (MethodDefinition method, bool required = true)
		{
			var entry = GetOrCreateEntry (method.DeclaringType);
			entry.Methods [GetXmlSignature (method)] = (required, method);
			if (required)
				entry.Required = true;
		}

		// Preserve a single field. A required (unconditional) field also makes its declaring type required.
		public void PreserveField (FieldDefinition field, bool required = true)
		{
			var entry = GetOrCreateEntry (field.DeclaringType);
			entry.Fields [field.Name] = required;
			if (required)
				entry.Required = true;
		}

		public XDocument CreateXml ()
		{
			return new XDocument (
				new XElement ("linker",
					entries
						.OrderBy (v => v.Key, System.StringComparer.Ordinal)
						.Select (assembly => new XElement ("assembly",
							new XAttribute ("fullname", assembly.Key),
							assembly.Value
								.OrderBy (v => v.Key, System.StringComparer.Ordinal)
								.Select (v => CreateTypeElement (v.Value))))));
		}

		XElement CreateTypeElement (TypeEntry entry)
		{
			var type = new XElement ("type", new XAttribute ("fullname", entry.Type.FullName));

			if (entry.PreserveAllMembers) {
				type.SetAttributeValue ("preserve", "all");
				return type;
			}

			if (entry.PreserveOnlyFields && entry.Fields.Count == 0 && entry.Methods.Count == 0) {
				type.SetAttributeValue ("preserve", "fields");
				return type;
			}

			if (!entry.Required)
				type.SetAttributeValue ("required", "false");

			type.SetAttributeValue ("preserve", "nothing");

			foreach (var field in entry.Fields.OrderBy (v => v.Key, System.StringComparer.Ordinal))
				type.Add (new XElement ("field", new XAttribute ("name", field.Key), new XAttribute ("required", field.Value ? "true" : "false")));

			foreach (var method in entry.Methods.OrderBy (v => v.Key, System.StringComparer.Ordinal)) {
				var element = new XElement ("method");
				// ILC (NativeAOT compiler) can't resolve generic parameter names (like 'T') in XML descriptor
				// method signatures (see https://github.com/dotnet/runtime/issues/128121), so use the method
				// name instead of the full signature when the method has generic parameter types.
				if (HasGenericParameterInSignature (method.Value.Method))
					element.SetAttributeValue ("name", method.Value.Method.Name);
				else
					element.SetAttributeValue ("signature", method.Key);
				element.SetAttributeValue ("required", method.Value.Required ? "true" : "false");
				type.Add (element);
			}

			return type;
		}

		// Saves the descriptor XML to the specified path, creating the containing directory if needed.
		// The file is only (re)written if its contents would change, so an unchanged descriptor doesn't
		// needlessly invalidate incremental builds. Returns true if the file was written.
		public bool Save (string path)
		{
			byte [] bytes;
			using (var stream = new MemoryStream ()) {
				CreateXml ().Save (stream);
				bytes = stream.ToArray ();
			}

			if (File.Exists (path) && File.ReadAllBytes (path).AsSpan ().SequenceEqual (bytes))
				return false;

			var directory = Path.GetDirectoryName (path);
			if (!string.IsNullOrEmpty (directory))
				Directory.CreateDirectory (directory);

			File.WriteAllBytes (path, bytes);
			return true;
		}

		// The ILLink xml root-descriptor method signature is the method's full name without the declaring type prefix.
		static string GetXmlSignature (MethodDefinition method)
		{
			var marker = method.DeclaringType.FullName + "::";
			var index = method.FullName.IndexOf (marker, System.StringComparison.Ordinal);
			if (index < 0)
				return method.FullName;

			return method.FullName.Substring (0, index) + method.FullName.Substring (index + marker.Length);
		}

		// Check if a method has any generic parameters in its signature (return type or parameter types).
		// This includes generic parameters nested inside other types (e.g. Action<T>, T[], ref T, Nullable<T>).
		// The linker XML descriptor can't resolve generic parameter names like 'T' in method signatures.
		static bool HasGenericParameterInSignature (MethodDefinition method)
		{
			if (method.ReturnType.ContainsGenericParameter)
				return true;
			foreach (var param in method.Parameters) {
				if (param.ParameterType.ContainsGenericParameter)
					return true;
			}
			return false;
		}
	}
}
