using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Linker;

using Mono.Tuner;

#if !MMP
using Xamarin.Tuner;
#endif

namespace MonoTouch.Tuner {

	public static class Extensions {

#if !MMP
		public static bool? GetIsDirectBindingConstant (this TypeDefinition type, DerivedLinkContext link_context)
		{
			if (link_context?.IsDirectBindingValue is null)
				return null;

			bool? value;
			if (link_context.IsDirectBindingValue.TryGetValue (type, out value))
				return value;

			return null;
		}
#endif

		// Extension method to avoid conditional code for files shared between
		// .NET linker and Legacy (where LinkContext doesn't implement IMetadataResolver).
		// This doesn't actually use the LinkContext.
		public static TypeDefinition Resolve (this LinkContext context, TypeReference type)
		{
			return type.Resolve ();
		}
	}
}
