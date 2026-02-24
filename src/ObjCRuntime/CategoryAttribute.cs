//
// CategoryAttribute.cs:
//
// Authors:
//   Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2015 Xamarin Inc.

#nullable enable

namespace ObjCRuntime {
	/// <summary>Attribute used to flag a class as a category that extends another type.</summary>
	/// <remarks>
	///   <para>This attribute is applied to static user-code classes and surfaces all exported methods and properties (those with the <see cref="Foundation.ExportAttribute"/>) to the provided system type.</para>
	///   <para>This allows new methods to be introduced or implemented for all types in the class. For example, this can be used to provide a global implementation of some method across all your types surfaced to Objective-C.</para>
	///   <para>All managed extension methods must be static, but it is possible to create Objective-C instance methods using the standard syntax for extension methods in C#:</para>
	///   <example>
	///     <code lang="csharp"><![CDATA[
	/// // Make "shouldAutoRotate" return true for all UIViewControllers in the application
	///
	/// [Category (typeof (UIViewController))]
	/// static class MyViewControllerMethods {
	///     [Export ("shouldAutorotate")]
	///     static bool ShouldAutoRotate (this UIViewController self)
	///     {
	///         return true;
	///     }
	///
	///     [Export ("supportedInterfaceOrientations")]
	///     static UIInterfaceOrientationMask SupportedRotations (this UIViewController self)
	///     {
	///         return UIInterfaceOrientationMask.All;
	///     }
	/// }
	/// ]]></code>
	///   </example>
	///   <example>
	///     <code lang="csharp"><![CDATA[
	/// // This example adds a native toUpper instance method to the NSString class,
	/// // which can be invoked from Objective-C.
	///
	/// [Category (typeof (NSString))]
	/// public static class MyStringCategory
	/// {
	///     [Export ("toUpper")]
	///     static string ToUpper (this NSString self)
	///     {
	///         return self.ToString ().ToUpper ();
	///     }
	/// }
	/// ]]></code>
	///   </example>
	///   <para>If the managed class is not referenced by other managed code (and is only called from Objective-C), the managed linker may remove it. This can be avoided by either adding a <see cref="Foundation.PreserveAttribute"/> attribute to the class, or by creating a custom linker definition file.</para>
	///   <para>See <see href="https://docs.microsoft.com/xamarin/cross-platform/deploy-test/linker">Custom Linker Configuration</see> for more information.</para>
	/// </remarks>
	[AttributeUsage (AttributeTargets.Class)]
	public class CategoryAttribute : Attribute {
		/// <summary>Initializes a new instance of the <see cref="CategoryAttribute"/> class.</summary>
		/// <param name="type">The Objective-C type to extend. This must be a subclass of <see cref="Foundation.NSObject"/> (or <see cref="Foundation.NSObject"/> itself).</param>
		public CategoryAttribute (Type type)
		{
			Type = type;
		}

		/// <summary>Gets or sets the type that this category extends.</summary>
		/// <value>The type this category extends.</value>
		public Type Type { get; set; }

		/// <summary>Gets or sets the name of the category.</summary>
		/// <value>The category name, or <see langword="null"/> to use the managed type name.</value>
		/// <remarks>
		/// <para>This must be a valid Objective-C type name, but is otherwise unused.</para>
		/// </remarks>
		public string? Name { get; set; }
	}
}
