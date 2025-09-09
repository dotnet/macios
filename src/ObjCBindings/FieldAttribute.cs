using System;
using System.Diagnostics.CodeAnalysis;

#nullable enable

namespace ObjCBindings {

	/// <summary>
	/// Generic attribute that is used to mark code to be backed by a field in ObjC.
	/// </summary>
	[Experimental ("APL0003")]
	[AttributeUsage (AttributeTargets.Property | AttributeTargets.Field)]
	public class FieldAttribute<T> : Attribute where T : Enum {

		/// <summary>
		/// Extra configuration flags for the field.
		/// </summary>
		public T? Flags { get; set; } = default;

		/// <summary>
		/// Get/Set the native symbol represented by the attribute.
		/// </summary>
		public string SymbolName { get; set; }

		/// <summary>
		/// Get/Set the library that contains the symbol.
		/// </summary>
		public string? LibraryPath { get; set; } = default;

		/// <summary>
		/// Get/Set the notification type.
		/// </summary >
		public Type? Type { get; set; } = null;

		/// <summary>
		/// Get/Set the notification center.
		/// </summary >
		public string? NotificationCenter { get; set; } = null;

		/// <summary>
		/// Create a new FieldAttribute for the given symbol and using the namespace as its containing library.
		/// </summary>
		/// <param name="symbolName">The name of the symbol.</param>
		public FieldAttribute (string symbolName)
		{
			SymbolName = symbolName;
			Flags = default (T);
			LibraryPath = null;
		}

		/// <summary>
		/// Create a new FieldAttribute for the given symbol in the provided library.
		/// </summary>
		/// <param name="symbolName">The name of the symbol.</param>
		/// <param name="libraryName">The name of the library that contains the symbol.</param>
		public FieldAttribute (string symbolName, string libraryName)
		{
			SymbolName = symbolName;
			LibraryPath = libraryName;
			Flags = default (T);
		}

		/// <summary>
		/// Create a new FieldAttribute for the given symbol and customizing the flags.
		/// </summary>
		/// <param name="symbolName">The name of the symbol.</param>
		/// <param name="flags">The flags to customize the field.</param>
		public FieldAttribute (string symbolName, T? flags)
		{
			SymbolName = symbolName;
			Flags = flags;
			LibraryPath = null;
		}

		/// <summary>
		/// Create a new FieldAttribute for the given symbol in the provided library and customizing the flags.
		/// </summary>
		/// <param name="symbolName">The name of the symbol.</param>
		/// <param name="libraryName">The name of the library that contains the symbol.</param>
		/// <param name="flags">The flags to customize the field.</param>
		public FieldAttribute (string symbolName, string libraryName, T? flags)
		{
			SymbolName = symbolName;
			LibraryPath = libraryName;
			Flags = flags;
		}

	}
}
