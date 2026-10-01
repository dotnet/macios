using System;
using System.Collections;

using Mono.Linker;
using Mono.Linker.Steps;

using Mono.Cecil;
using Xamarin.Tuner;

#nullable enable

namespace Xamarin.Linker.Steps {

#if ASSEMBLY_PREPARER
	public abstract class AttributeIteratorBaseStep : AssemblyModifierStep {

		protected DerivedLinkContext LinkContext {
			get {
				return Configuration.DerivedLinkContext;
			}
		}

		protected override bool IsActiveFor (AssemblyDefinition assembly)
		{
			return Annotations.GetAction (assembly) == AssemblyAction.Link;
		}

		protected override bool ModifyAssembly (AssemblyDefinition assembly)
		{
			modified = false;

			ProcessAssemblyAttributes (assembly);
			foreach (var type in assembly.MainModule.Types)
				ProcessTypeRecursively (type);

			return modified;
		}

		void ProcessTypeRecursively (TypeDefinition type)
		{
			ProcessType (type);

			if (type.HasFields) {
				foreach (var field in type.Fields)
					ProcessField (field);
			}

			if (type.HasMethods) {
				foreach (var method in type.Methods)
					ProcessMethod (method);
			}

			if (type.HasProperties) {
				foreach (var property in type.Properties)
					ProcessProperty (property);
			}

			if (type.HasEvents) {
				foreach (var @event in type.Events)
					ProcessEvent (@event);
			}

			if (type.HasNestedTypes) {
				foreach (var nested in type.NestedTypes)
					ProcessTypeRecursively (nested);
			}
		}

		// The linker's BaseSubStep declares these as overridable methods, the assembly-preparer's
		// AssemblyModifierStep doesn't, so declare them here to keep the rest of the code identical.
		bool modified;

		void ProcessAssemblyAttributes (AssemblyDefinition assembly)
		{
			ProcessAttributeProvider (assembly);
			ProcessAttributeProvider (assembly.MainModule);
		}

		protected override bool ProcessType (TypeDefinition type)
		{
			ProcessAttributeProvider (type);

			if (type.HasGenericParameters)
				ProcessAttributeProviderCollection (type.GenericParameters);

			return modified;
		}

		void ProcessField (FieldDefinition field)
		{
			ProcessAttributeProvider (field);
		}

		protected override bool ProcessMethod (MethodDefinition method)
		{
			ProcessMethodAttributeProvider (method);
			return modified;
		}

		void ProcessProperty (PropertyDefinition property)
		{
			ProcessAttributeProvider (property);
		}

		void ProcessEvent (EventDefinition @event)
		{
			ProcessAttributeProvider (@event);
		}
#else
	public abstract class AttributeIteratorBaseStep : BaseSubStep {

		protected DerivedLinkContext LinkContext {
			get {
				return LinkerConfiguration.GetInstance (Context).DerivedLinkContext;
			}
		}

		public override SubStepTargets Targets {
			get {
				return SubStepTargets.Assembly
					| SubStepTargets.Type
					| SubStepTargets.Field
					| SubStepTargets.Method
					| SubStepTargets.Property
					| SubStepTargets.Event;
			}
		}

		public override bool IsActiveFor (AssemblyDefinition assembly)
		{
			return Annotations.GetAction (assembly) == AssemblyAction.Link;
		}

		public override void ProcessAssembly (AssemblyDefinition assembly)
		{
			ProcessAttributeProvider (assembly);
			ProcessAttributeProvider (assembly.MainModule);
		}

		public override void ProcessType (TypeDefinition type)
		{
			ProcessAttributeProvider (type);

			if (type.HasGenericParameters)
				ProcessAttributeProviderCollection (type.GenericParameters);
		}

		public override void ProcessField (FieldDefinition field)
		{
			ProcessAttributeProvider (field);
		}

		public override void ProcessMethod (MethodDefinition method)
		{
			ProcessMethodAttributeProvider (method);
		}

		public override void ProcessProperty (PropertyDefinition property)
		{
			ProcessAttributeProvider (property);
		}

		public override void ProcessEvent (EventDefinition @event)
		{
			ProcessAttributeProvider (@event);
		}
#endif

		void ProcessAttributeProviderCollection (IList list)
		{
			for (int i = 0; i < list.Count; i++)
				ProcessAttributeProvider ((ICustomAttributeProvider) list [i]!);
		}

		void ProcessMethodAttributeProvider (MethodDefinition method)
		{
			ProcessAttributeProvider (method);
			ProcessAttributeProvider (method.MethodReturnType);

			if (method.HasParameters)
				ProcessAttributeProviderCollection (method.Parameters);

			if (method.HasGenericParameters)
				ProcessAttributeProviderCollection (method.GenericParameters);
		}

		void ProcessAttributeProvider (ICustomAttributeProvider provider)
		{
			if (!provider.HasCustomAttributes)
				return;

			for (int i = 0; i < provider.CustomAttributes.Count; i++) {
				var attrib = provider.CustomAttributes [i];
				ProcessAttribute (provider, attrib, out var remove);

				if (remove) {
					provider.CustomAttributes.RemoveAt (i--);
#if ASSEMBLY_PREPARER
					modified = true;
#endif
				}
			}
		}

		protected abstract void ProcessAttribute (ICustomAttributeProvider provider, CustomAttribute attribute, out bool remove);
	}
}
