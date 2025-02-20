//
// UIActionSheet.cs: Extensions to UIActionSheet
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
// Copyright 2015 Xamarin Inc.
//

#if IOS

using System;
using System.Collections;
using ObjCRuntime;
using Foundation;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace UIKit {
	public partial class UIActionSheet : IEnumerable {

#if !NET
		[Obsolete ("Use overload with a IUIActionSheetDelegate parameter")]
		public UIActionSheet (string title, UIActionSheetDelegate del, string cancelTitle, string destroy, params string [] other)
			: this (title, del as IUIActionSheetDelegate, cancelTitle, destroy, other)
		{
		}
#endif

		/// <include file="../../docs/api/UIKit/UIActionSheet.xml" path="/Documentation/Docs[@DocId='M:UIKit.UIActionSheet.#ctor(System.String,UIKit.IUIActionSheetDelegate,System.String,System.String,System.String[])']/*" />
	public UIActionSheet (string title, IUIActionSheetDelegate del, string cancelTitle, string destroy, params string [] other)
			: this (title, del, null, null, (string) null)
		{
			if (destroy is not null)
				DestructiveButtonIndex = AddButton (destroy);

			if (other is null) {
				if (cancelTitle is not null)
					CancelButtonIndex = AddButton (cancelTitle);
				return;
			}

			foreach (string b in other) {
				if (b is not null)
					AddButton (b);
			}

			if (cancelTitle is not null)
				CancelButtonIndex = AddButton (cancelTitle);
		}

#if !NET
		[Obsolete ("Use overload with a IUIActionSheetDelegate parameter")]
		public UIActionSheet (string title, UIActionSheetDelegate del)
			: this (title, del as IUIActionSheetDelegate, null, null, (string) null)
		{
		}
#endif

		/// <param name="title">To be added.</param>
		///         <param name="del">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public UIActionSheet (string title, IUIActionSheetDelegate del)
		: this (title, del, null, null, (string) null) { }

		/// <param name="title">A title to be displayed in the title area of the action sheet.</param>
		///         <summary>Initialize an <see cref="T:UIKit.UIActionSheet" /> with a title.</summary>
		///         <remarks>Pass <see langword="null" /> to <paramref name="title" /> if there is no text to display in the title area.</remarks>
		public UIActionSheet (string title)
		: this (title, null, null, null, (string) null) { }

		/// <param name="name">Text for the button.</param>
		///         <summary>Adds a button with the specified text.</summary>
		///         <remarks>This method exists to allow the class to be initialized with C# 3.0 object initializers.   This is equivalent to calling AddButton (name).</remarks>
		public void Add (string name)
		{
			AddButton (name);
		}

		/// <summary>Obtains an enumerator that returns the button titles.</summary>
		///         <returns>An IEnumerator.</returns>
		///         <remarks>To be added.</remarks>
		public IEnumerator GetEnumerator ()
		{
			for (int i = 0; i < ButtonCount; i++)
				yield return ButtonTitle (i);
		}
	}

}

#endif // IOS
