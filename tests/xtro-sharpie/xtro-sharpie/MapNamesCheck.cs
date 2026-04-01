//
// The rule iterates over all managed types and maps their native name to their managed name,
// if they have any attributes that tells us the native name is different than the managed name.
//

namespace Extrospection {

	public class MapNamesVisitor : BaseVisitor {
		public MapNamesVisitor (BindingResult bindingResult)
			: base (bindingResult)
		{
		}

		public override void VisitManagedType (TypeDefinition type)
		{
			var nativeName = type.GetName ();
			if (nativeName is not null && nativeName != type.Name)
				Helpers.MapNames (nativeName, type.Name);
		}
	}
}
