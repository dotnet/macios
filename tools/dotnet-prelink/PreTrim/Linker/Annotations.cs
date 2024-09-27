// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

//
// Annotations.cs
//
// Author:
//   Jb Evain (jbevain@novell.com)
//
// (C) 2007 Novell, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Mono.Linker {
	partial class AnnotationStore {
		protected readonly LinkContext context;

		protected readonly Dictionary<AssemblyDefinition, AssemblyAction> assembly_actions = new Dictionary<AssemblyDefinition, AssemblyAction> ();
		protected readonly HashSet<IMetadataTokenProvider> marked = new HashSet<IMetadataTokenProvider> ();
		protected readonly Dictionary<TypeDefinition, TypePreserve> preserved_types = new Dictionary<TypeDefinition, TypePreserve> ();
		protected readonly Dictionary<IMemberDefinition, List<MethodDefinition>> preserved_methods = new Dictionary<IMemberDefinition, List<MethodDefinition>> ();
		protected readonly Dictionary<AssemblyDefinition, ISymbolReader> symbol_readers = new Dictionary<AssemblyDefinition, ISymbolReader> ();
		readonly Dictionary<object, Dictionary<IMetadataTokenProvider, object>> custom_annotations = new Dictionary<object, Dictionary<IMetadataTokenProvider, object>> ();
		protected readonly HashSet<CustomAttribute> marked_attributes = new HashSet<CustomAttribute> ();

		internal AnnotationStore (LinkContext context)
		{
			this.context = context;
			TypeMapInfo = new TypeMapInfo (context);
			SubstitutionInfo = new ();
		}

		internal bool ProcessSatelliteAssemblies { get; set; } = true;

		TypeMapInfo TypeMapInfo { get; }

		SubstitutionInfo SubstitutionInfo { get; }

		public partial AssemblyAction GetAction (AssemblyDefinition assembly)
		{
			if (assembly_actions.TryGetValue (assembly, out AssemblyAction action))
				return action;

			throw new InvalidOperationException ($"No action for the assembly {assembly.Name} defined");
		}

		public partial void SetAction (AssemblyDefinition assembly, AssemblyAction action)
		{
			assembly_actions [assembly] = action;
		}

		public partial bool HasAction (AssemblyDefinition assembly)
		{
			return assembly_actions.ContainsKey (assembly);
		}

		public partial void SetAction (MethodDefinition method, MethodAction action)
		{
			SubstitutionInfo.SetMethodAction (method, action);
		}

		public partial void SetStubValue (MethodDefinition method, object value)
		{
			SubstitutionInfo.SetMethodStubValue (method, value);
		}

		public partial void Mark (IMetadataTokenProvider provider)
		{
			throw new NotImplementedException ();
		}

		public partial void Mark (CustomAttribute attribute)
		{
			marked_attributes.Add (attribute);
		}

		public partial bool IsMarked (IMetadataTokenProvider provider)
		{
			return marked.Contains (provider);
		}

		public partial bool IsMarked (CustomAttribute attribute)
		{
			return marked_attributes.Contains (attribute);
		}

		public partial void SetPreserve (TypeDefinition type, TypePreserve preserve)
		{
			Debug.Assert (preserve != TypePreserve.Nothing);
			if (!preserved_types.TryGetValue (type, out TypePreserve existing)) {
				preserved_types.Add (type, preserve);
				return;
			}
			Debug.Assert (existing != TypePreserve.Nothing);
			var newPreserve = ChoosePreserveActionWhichPreservesTheMost (existing, preserve);
			if (newPreserve != existing) {
				preserved_types [type] = newPreserve;
			}
		}

		static TypePreserve ChoosePreserveActionWhichPreservesTheMost (TypePreserve leftPreserveAction, TypePreserve rightPreserveAction)
		{
			if (leftPreserveAction == rightPreserveAction)
				return leftPreserveAction;

			if (leftPreserveAction == TypePreserve.All || rightPreserveAction == TypePreserve.All)
				return TypePreserve.All;

			if (leftPreserveAction == TypePreserve.Nothing)
				return rightPreserveAction;

			if (rightPreserveAction == TypePreserve.Nothing)
				return leftPreserveAction;

			if ((leftPreserveAction == TypePreserve.Methods && rightPreserveAction == TypePreserve.Fields) ||
				(leftPreserveAction == TypePreserve.Fields && rightPreserveAction == TypePreserve.Methods))
				return TypePreserve.All;

			return rightPreserveAction;
		}

		/// <summary>
		/// Returns a list of all known methods that override <paramref name="method"/>.
		/// The list may be incomplete if other overrides exist in assemblies that haven't been processed by TypeMapInfo yet
		/// </summary>
		public partial IEnumerable<OverrideInformation>? GetOverrides (MethodDefinition method)
		{
			return TypeMapInfo.GetOverrides (method);
		}

		public partial void AddPreservedMethod (TypeDefinition type, MethodDefinition method)
		{
			AddPreservedMethod (type as IMemberDefinition, method);
		}

		partial void AddPreservedMethod (MethodDefinition key, MethodDefinition method)
		{
			// AddPreservedMethod (key as IMemberDefinition, method);
		}

		List<MethodDefinition>? GetPreservedMethods (IMemberDefinition definition)
		{
			if (preserved_methods.TryGetValue (definition, out List<MethodDefinition>? preserved))
				return preserved;

			return null;
		}

		void AddPreservedMethod (IMemberDefinition definition, MethodDefinition method)
		{
			var methods = GetPreservedMethods (definition);
			if (methods == null) {
				methods = new List<MethodDefinition> ();
				preserved_methods [definition] = methods;
			}

			methods.Add (method);
		}

		internal void AddSymbolReader (AssemblyDefinition assembly, ISymbolReader symbolReader)
		{
			symbol_readers [assembly] = symbolReader;
		}
		public void CloseSymbolReader (AssemblyDefinition assembly)
		{
			if (!symbol_readers.TryGetValue (assembly, out ISymbolReader? symbolReader))
				return;

			symbol_readers.Remove (assembly);
			symbolReader.Dispose ();
		}

		public partial object? GetCustomAnnotation (object key, IMetadataTokenProvider item)
		{
			if (!custom_annotations.TryGetValue (key, out Dictionary<IMetadataTokenProvider, object>? slots))
				return null;

			if (!slots.TryGetValue (item, out object? value))
				return null;

			return value;
		}

		public partial void SetCustomAnnotation (object key, IMetadataTokenProvider item, object value)
		{
			if (!custom_annotations.TryGetValue (key, out Dictionary<IMetadataTokenProvider, object>? slots)) {
				slots = new Dictionary<IMetadataTokenProvider, object> ();
				custom_annotations.Add (key, slots);
			}

			slots [item] = value;
		}
	}
}
