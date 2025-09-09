//
// ExportAttribute.cs: The Export attribute
//
// Authors:
//   Geoff Norton
//
// Copyright 2009, Novell, Inc.
// Copyright 2010, Novell, Inc.
// Copyright 2011, 2012 Xamarin Inc
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
//
using System;
using System.Globalization;
using System.Reflection;
using ObjCRuntime;
using Registrar;

#nullable enable

namespace Foundation {

	/// <summary>Exports a method or property to the Objective-C world.</summary>
	///     <remarks>
	///       <para>
	/// This attribute is applied to properties and methods in classes that derive from <see cref="Foundation.NSObject" /> to export the value to the Objective-C world.    This can be used either to respond to messages or to override an Objective-C method.
	/// </para>
	///       <example>
	///         <code lang="csharp lang-csharp"><![CDATA[
	/// public class Test : SomeBaseClass {
	///     [Export ("setText:withFont:")]
	///     public void SetText (string text, string font)
	///     {
	///     }
	/// }
	///   ]]></code>
	///       </example>
	///     </remarks>
	[AttributeUsage (AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Property)]
	public class ExportAttribute : Attribute {
		string? selector;
		ArgumentSemantic semantic;

		/// <summary>Use this method to expose a C# method, property or constructor as a method that can be invoked from Objective-C.</summary>
		///         <remarks>Use this method to expose a C# method, property or constructor as a method that can be invoked from Objective-C.   The name is derived from the actual method or property.</remarks>
		protected ExportAttribute () { }

		/// <param name="selector">The selector name.</param>
		///         <summary>Exports the given method or property to Objective-C land with the specified method name.</summary>
		///         <remarks>Use this method to expose a C# method, property or constructor as a method that can be invoked from Objective-C.</remarks>
		public ExportAttribute (string? selector)
		{
			this.selector = selector;
			this.semantic = ArgumentSemantic.None;
		}

		/// <param name="selector">The selector name.</param>
		///         <param name="semantic">The semantics of the setter (Assign, Copy or Retain).</param>
		///         <summary>Use this method to expose a C# method, property or constructor as a method that can be invoked from Objective-C.</summary>
		///         <remarks>Use this method to expose a C# method, property or constructor as a method that can be invoked from Objective-C.</remarks>
		public ExportAttribute (string? selector, ArgumentSemantic semantic)
		{
			this.selector = selector;
			this.semantic = semantic;
		}

		/// <summary>The name of the C# selector if specified, or null if it is derived from the property name or method.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public string? Selector {
			get { return this.selector; }
			set { this.selector = value; }
		}

		/// <summary>The semantics for object ownership on setter properties or methods.</summary>
		///         <value>The assignment ownership semantics for setting the value.</value>
		///         <remarks>To be added.</remarks>
		public ArgumentSemantic ArgumentSemantic {
			get { return this.semantic; }
			set { this.semantic = value; }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsVariadic {
			get;
			set;
		}

		/// <param name="prop">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public ExportAttribute ToGetter (PropertyInfo prop)
		{
			if (string.IsNullOrEmpty (Selector))
				Selector = prop.Name;
			return new ExportAttribute (selector, semantic);
		}

		/// <param name="prop">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public ExportAttribute ToSetter (PropertyInfo prop)
		{
			if (string.IsNullOrEmpty (Selector))
				Selector = prop.Name;
			return new ExportAttribute (Registrar.Registrar.CreateSetterSelector (selector), semantic);
		}
	}

	/// <summary>Exposes the given property as an outlet to the Objective-C world.</summary>
	///     <remarks>
	///       <para> 
	/// 	This property is used to flag properties that need to be
	/// 	exposed as outlets to the Objective-C world.  This is used
	/// 	both by the designer support as well connecting the managed
	/// 	object with the unmanaged Objective-C outlet.
	///
	///       </para>
	///       <example>
	///         <code lang="csharp lang-csharp"><![CDATA[
	/// 	    [Outlet ("mainWindow")]
	/// 	    UIWindows mainWindow  { get; set; }
	/// 	  ]]></code>
	///       </example>
	///     </remarks>
	[AttributeUsage (AttributeTargets.Property)]
	public sealed class OutletAttribute : ExportAttribute {
		/// <summary>Default constructor</summary>
		///         <remarks>
		///         </remarks>
		public OutletAttribute () : base (null) { }
		/// <param name="name">Outlet name.</param>
		///         <summary>Links the managed property with the specified Objective-C outlet.</summary>
		///         <remarks>
		///         </remarks>
		public OutletAttribute (string name) : base (name) { }
	}

	/// <summary>Flags a method to respond to an Objective-C action</summary>
	///     <remarks>
	///       <para>
	/// 	You can apply this attribute to a method, turning it into an action that can be invoked by the Objective-C world. 
	///       </para>
	///       <example>
	///         <code lang="csharp lang-csharp"><![CDATA[
	/// 	    [Action ("clicked:")]
	/// 	    void Submit (NSObject sender)
	/// 	    {
	/// 		    // User has clicked on the Submit button, respond to this action
	/// 	    }
	/// 	  ]]></code>
	///       </example>
	///     </remarks>
	[AttributeUsage (AttributeTargets.Method)]
	public sealed class ActionAttribute : ExportAttribute {
		/// <summary>Initializes a new instance of an Action attribute</summary>
		///         <remarks>
		///         </remarks>
		public ActionAttribute () : base (null) { }
		/// <param name="selector">Name for the selector to expose</param>
		///         <summary>Creates a new instance of the action attribtue with the given selector.</summary>
		///         <remarks>
		///         </remarks>
		public ActionAttribute (string selector) : base (selector) { }
	}
}
