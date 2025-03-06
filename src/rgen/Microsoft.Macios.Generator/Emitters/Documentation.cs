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

		public static string GetDomain (string name) =>
@$"/// <summary>Returns the error domain associated with the {name} value</summary>
/// <param name=""self"">The enumeration value</param>
/// <remarks>
///   <para>See the <see cref=""global::Foundation.NSError"" /> for information on how to use the error domains when reporting errors.</para>
/// </remarks>";
	}

	/// <summary>
	/// Doc string for the generated classes.
	/// </summary>
	public static class Class {
		public static string ClassHandle (string _) =>
@"/// <summary>The Objective-C class handle for this class.</summary>
/// <value>The pointer to the Objective-C class.</value>
/// <remarks>
///     Each managed class mirrors an unmanaged Objective-C class.
///     This value contains the pointer to the Objective-C class.
///     It is similar to calling the managed <see cref=\""ObjCRuntime.Class.GetHandle(string)\"" /> or the native <see href=\""https://developer.apple.com/documentation/objectivec/1418952-objc_getclass\"">objc_getClass</see> method with the type name.
/// </remarks>";

		public static string DefaultInit (string name) =>
@$"/// <summary>Creates a new <see cref=""{name.Replace ('<', '{').Replace ('>', '}')}"" /> with default values.</summary>";

		public static string DefaultInitWithCode (string _) =>
@"/// <summary>A constructor that initializes the object from the data stored in the unarchiver object.</summary>
/// <param name=\""coder\"">The unarchiver object.</param>
/// <remarks>
///   <para>This constructor is provided to allow the class to be initialized from an unarchiver (for example, during NIB deserialization). This is part of the <see cref=\""Foundation.NSCoding\"" /> protocol.</para>
///   <para>If developers want to create a subclass of this object and continue to support deserialization from an archive, they should implement a constructor with an identical signature: taking a single parameter of type <see cref=\""Foundation.NSCoder\"" /> and decorate it with the <c>[Export(\""initWithCoder:\""]</c> attribute.</para>
///   <para>The state of this object can also be serialized by using the <see cref=\""Foundation.INSCoding.EncodeTo\"" /> companion method.</para>
/// </remarks>";

		public static string DefaultInitWithFlag (string _) =>
@"/// <summary>Constructor to call on derived classes to skip initialization and merely allocate the object.</summary>
/// <param name=""t"">Unused sentinel value, pass NSObjectFlag.Empty.</param>
/// <remarks>
///     <para>
///         This constructor should be called by derived classes when they completely construct the object in managed code and merely want the runtime to allocate and initialize the <see cref=""Foundation.NSObject"" />.
///         This is required to implement the two-step initialization process that Objective-C uses, the first step is to perform the object allocation, the second step is to initialize the object.
///         When developers invoke this constructor, they take advantage of a direct path that goes all the way up to <see cref=""Foundation.NSObject"" /> to merely allocate the object's memory and bind the Objective-C and C# objects together.
///         The actual initialization of the object is up to the developer.
///     </para>
///     <para>
///         This constructor is typically used by the binding generator to allocate the object, but prevent the actual initialization to take place.
///         Once the allocation has taken place, the constructor has to initialize the object.
///         With constructors generated by the binding generator this means that it manually invokes one of the ""init"" methods to initialize the object.
///     </para>
///     <para>It is the developer's responsibility to completely initialize the object if they chain up using this constructor chain.</para>
///     <para>
///         In general, if the developer's constructor invokes the corresponding base implementation, then it should also call an Objective-C init method.
///         If this is not the case, developers should instead chain to the proper constructor in their class.
///     </para>
///     <para>
///         The argument value is ignored and merely ensures that the only code that is executed is the construction phase is the basic <see cref=""Foundation.NSObject"" /> allocation and runtime type registration.
///         Typically the chaining would look like this:
///     </para>
///     <example>
///             <code lang=""csharp lang-csharp""><![CDATA[
/// //
/// // The NSObjectFlag constructor merely allocates the object and registers the C# class with the Objective-C runtime if necessary.
/// // No actual initXxx method is invoked, that is done later in the constructor
/// //
/// // This is taken from the iOS SDK's source code for the UIView class:
/// //
/// [Export (""initWithFrame:"")]
/// public UIView (System.Drawing.RectangleF frame) : base (NSObjectFlag.Empty)
/// {
///     // Invoke the init method now.
///     var initWithFrame = new Selector (""initWithFrame:"").Handle;
///     if (IsDirectBinding) {
///         Handle = ObjCRuntime.Messaging.IntPtr_objc_msgSend_CGRect (this.Handle, initWithFrame, frame);
///     } else {
///         Handle = ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_CGRect (this.SuperHandle, initWithFrame, frame);
///     }
/// }
/// ]]></code>
///     </example>
/// </remarks>";

		public static string DefaultInitWithHandle (string name) =>
@"/// <summary>A constructor used when creating managed representations of unmanaged objects. Called by the runtime.</summary>
/// <param name=""handle"">Pointer (handle) to the unmanaged object.</param>
/// <remarks>
///     <para>
///         This constructor is invoked by the runtime infrastructure (<see cref=""ObjCRuntime.Runtime.GetNSObject(System.IntPtr)"" />) to create a new managed representation for a pointer to an unmanaged Objective-C object.
///         Developers should not invoke this method directly, instead they should call <see cref=""ObjCRuntime.Runtime.GetNSObject(System.IntPtr)"" /> as it will prevent two instances of a managed object pointing to the same native object.
///     </para>
/// </remarks>";
	}
}
