namespace Microsoft.Macios.Generator.Emitters;

/// <summary>
/// Static class that holds all the documentation strings. This allows to make the code generator
/// cleaner by removing the need to have the documentation strings in the code.
/// </summary>
public static class Documentation {

	/// <summary>
	/// Smart enum documentation.
	/// </summary>
	public static class SmartEnum {

		public static string ClassDocumentation (string name) =>
@$"/// <summary>
/// Extension methods for the <see cref=""{name}"" /> enumeration.
/// </summary>";

		public static string GetConstant () =>
@"/// <summary>
/// Retrieves the <see cref=""global::Foundation.NSString"" /> constant that describes <paramref name=""self"" />.
/// </summary>
/// <param name=""self"">The instance on which this method operates.</param>";

		public static string GetValueNSString (string name) =>
@$"/// <summary>
/// Retrieves the <see cref=""{name}"" /> value named by <paramref name=""constant"" />.
/// </summary>
/// <param name=""constant"">The name of the constant to retrieve.</param>";

		public static string GetValueHandle (string name) =>
@$"/// <summary>
/// Retrieves the <see cref=""{name}"" /> value represented by the backing field value in <paramref name=""handle"" />.
/// </summary>
/// <param name=""handle"">The native handle with the name of the constant to retrieve.</param>";

		public static string ToConstantArray (string name) =>
@$"/// <summary>
/// Converts an array of <see cref=""{name}"" /> enum values into an array of their corresponding constants.
/// </summary>
/// <param name=""values"">The array of enum values to convert.</param>";

		public static string ToEnumArray (string _) =>
@"/// <summary>
/// Converts an array of <see cref=""NSString"" /> values into an array of their corresponding enum values.
/// </summary>
/// <param name=""values"">The array if <see cref=""NSString"" /> values to convert.</param>";

	}
}
