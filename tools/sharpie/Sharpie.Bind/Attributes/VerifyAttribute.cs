// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Attributes;

public enum VerifyHint {
	None,
	InferredFromPreceedingTypedef,
	InferredFromMemberPrefix,
	ConstantsInterfaceAssociation,
	MethodToProperty,
	StronglyTypedNSArray,
	PlatformInvoke,
	GenericTypeParameter,
}

public static class VerifyHintExtensions {
	static string? GetHtmlDescription (this VerifyHint hint)
	{
		switch (hint) {
		case VerifyHint.InferredFromPreceedingTypedef:
			return "The name of this declaration was inferred by common convention from the " +
			"immediately preceeding <code>typedef</code> in the original native source code. " +
			"Verify that the inferred name is correct as this convention is ambiguous.";
		case VerifyHint.InferredFromMemberPrefix:
			return "The name of this originally anonymous declaration was taken from a " +
			"common prefix of its members.";
		case VerifyHint.ConstantsInterfaceAssociation:
			return "There's no foolproof way to determine with which Objective-C interface " +
			"an extern variable declaration may be associated. Instances of these are bound " +
			"as <code>[Field]</code> properties in a partial interface into a nearby concrete " +
			"interface to produce a more intuitive API, possibly eliminating the 'Constants' " +
			"interface altogether.";
		case VerifyHint.MethodToProperty:
			return "An Objective-C method was bound as a C# property due to convention such as " +
			"taking no parameters and returning a value (non-void return). Often methods like " +
			"these should be bound as properties to surface a nicer API, but sometimes false-positives " +
			"can occur and the binding should actually be a method.";
		case VerifyHint.StronglyTypedNSArray:
			return "A native <code>NSArray*</code> was bound as <code>NSObject[]</code>. It might " +
			"be possible to more strongly type the array in the binding based on expectations set " +
			"through API documentation (e.g. comments in the header file) or by examining the array " +
			"contents through testing. For example, an NSArray* containing only NSNumber* instances " +
			"can be bound as <code>NSNumber[]</code> instead of <code>NSObject[]</code>.";
		case VerifyHint.PlatformInvoke:
			return "In general P/Invoke bindings are not as correct or complete as Objective-C " +
			"bindings (at least currently). You may need to fix up the library name (it defaults " +
			"to '__Internal') and return/parameter types manually to conform to C calling conventions" +
			"for the target platform. You may find you don't even want to expose the C API in your " +
			"binding, but if you do, you'll probably also want to relocate the definition to a more " +
			"appropriate class and expose a stronger type-safe wrapper. For P/Invoke guidance, see " +
			"http://www.mono-project.com/docs/advanced/pinvoke/.";
		case VerifyHint.GenericTypeParameter:
			return "In Objective-C the type was a generic type parameter. It has been bound as " +
			"as a compatible concrete type, but in the future should become a C# generic type parameter.";
		case VerifyHint.None:
		default:
			return null;
		}
	}

	public static string? GetDescription (this VerifyHint hint, bool html = false)
	{
		var description = hint.GetHtmlDescription ();
		if (html)
			return description;

		return description?.Replace ("<code>", string.Empty)?.Replace ("</code>", string.Empty);
	}
}

public sealed class VerifyAttribute : ObjectiveSharpieHintAttribute {
	public VerifyHint Hint { get; private set; }

	public VerifyAttribute (VerifyHint hint, string? message = null)
	{
		Hint = hint;
		Type = AstType.Create ("VerifyAttribute");
		Arguments.Add (new IdentifierExpression (hint.ToString ()));
		if (message is not null)
			Arguments.Add (new PrimitiveExpression (message));
	}
}
