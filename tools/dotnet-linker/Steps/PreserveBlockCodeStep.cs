using System;
using System.Linq;

using Mono.Cecil;

using Mono.Linker;
using Mono.Linker.Steps;
using Mono.Tuner;

using Xamarin.Bundler;

#nullable enable

namespace Xamarin.Linker.Steps {

	public class PreserveBlockCodeStep : AssemblyModifierStep {

		protected override string Name { get; } = "Preserve Block Code";
		protected override int ErrorCode { get; } = 2240;

		protected override bool IsActiveFor (AssemblyDefinition assembly)
		{
			// We only care about assemblies that are being linked.
			if (Annotations.GetAction (assembly) != AssemblyAction.Link)
				return false;

			// Unless an assembly is or references our platform assembly, then it won't have anything we need to preserve
			if (!Configuration.Profile.IsOrReferencesProductAssembly (assembly))
				return false;

			return true;
		}

		protected override bool ProcessType (TypeDefinition type)
		{
			var modified = false;

			if (GetMembersToPreserve (type, out var field, out var method)) {
				modified |= abr.AddDynamicDependencyAttributeToStaticConstructor (type, field);
				modified |= abr.AddDynamicDependencyAttributeToStaticConstructor (type, method);
			}

			if (GetNewStyleMethodToPreserve (type, out var invokeMethod))
				modified |= abr.AddDynamicDependencyAttributeToStaticConstructor (type, invokeMethod);

			return modified;
		}

		public static bool GetMembersToPreserve (TypeDefinition type, [NotNullWhen (true)] out FieldDefinition? field, [NotNullWhen (true)] out MethodDefinition? method)
		{
			field = null;
			method = null;

			/* For the following class:

			static internal class SDInnerBlock {
				// this field is not preserved by other means, but it must not be linked away
				static internal readonly DInnerBlock Handler = Invoke;

				[MonoPInvokeCallback (typeof (DInnerBlock))]
				static internal void Invoke (IntPtr block, int magic_number)
				{
				}
			}

			* We need to make sure the linker doesn't remove the Handler field
			* and the Invoke method.
			*/

			// First make sure we got the right class
			// The type for the field we're looking for is abstract, sealed and nested and contains exactly 1 field.
			if (!type.HasFields || !type.IsAbstract || !type.IsSealed || !type.IsNested)
				return false;
			if (type.Fields.Count != 1)
				return false;

			// The type is also nested inside ObjCRuntime.Trampolines class)
			var nestingType = type.DeclaringType;
			if (!nestingType.Is ("ObjCRuntime", "Trampolines"))
				return false;

			// The class has a readonly field named 'Handler'
			field = type.Fields [0];
			if (!field.IsInitOnly)
				return false;
			if (field.Name != "Handler")
				return false;

			// The class has a parameterless 'Invoke' method with a 'MonoPInvokeCallback' attribute
			if (!type.HasMethods)
				return false;
			method = type.Methods.SingleOrDefault (v => {
				if (v.Name != "Invoke")
					return false;
				if (!v.HasParameters)
					return false;
				if (!v.HasCustomAttributes)
					return false;
				if (!v.CustomAttributes.Any (v => v.AttributeType.Name == "MonoPInvokeCallbackAttribute"))
					return false;
				return true;
			});

			if (method is null)
				return false;

			// The type was used, so preserve the method and field
			return true;
		}

		// New-style block proxy types use [UnmanagedCallersOnly] on the Invoke method
		// and don't have a Handler field. They are generated when the bgen tool emits
		// function pointer-based block trampolines. We need to preserve the Invoke method
		// because the runtime looks it up via reflection in Blocks.GetBlockForDelegate.
		public static bool GetNewStyleMethodToPreserve (TypeDefinition type, [NotNullWhen (true)] out MethodDefinition? method)
		{
			method = null;

			/* For the following class:

			static internal class SDRegistrarTestBlock {
				[UnmanagedCallersOnly]
				[UserDelegateType (typeof (RegistrarTestBlock))]
				internal static unsafe uint Invoke (IntPtr block, uint magic)
				{
				}
				internal static unsafe BlockLiteral CreateBlock (RegistrarTestBlock callback)
				{
					delegate* unmanaged<IntPtr, uint, uint> trampoline = &Invoke;
					return new BlockLiteral (trampoline, callback, typeof (SDRegistrarTestBlock), nameof (Invoke));
				}
			}

			* We need to make sure the linker doesn't remove the Invoke method.
			*/

			// The type must be abstract, sealed (static class) and nested
			if (!type.IsAbstract || !type.IsSealed || !type.IsNested)
				return false;

			// The type must not have fields (old-style types have a Handler field and are handled by GetMembersToPreserve)
			if (type.HasFields)
				return false;

			// The type is nested inside ObjCRuntime.Trampolines class
			var nestingType = type.DeclaringType;
			if (!nestingType.Is ("ObjCRuntime", "Trampolines"))
				return false;

			if (!type.HasMethods)
				return false;

			// The class has an 'Invoke' method with [UnmanagedCallersOnly] and [UserDelegateType] attributes
			method = type.Methods.SingleOrDefault (v => {
				if (v.Name != "Invoke")
					return false;
				if (!v.HasParameters)
					return false;
				if (!v.HasCustomAttributes)
					return false;
				var hasUnmanagedCallersOnly = false;
				var hasUserDelegateType = false;
				foreach (var attr in v.CustomAttributes) {
					if (attr.AttributeType.Name == "UnmanagedCallersOnlyAttribute")
						hasUnmanagedCallersOnly = true;
					else if (attr.AttributeType.Name == "UserDelegateTypeAttribute")
						hasUserDelegateType = true;
					if (hasUnmanagedCallersOnly && hasUserDelegateType)
						break;
				}
				return hasUnmanagedCallersOnly && hasUserDelegateType;
			});

			return method is not null;
		}
	}
}
