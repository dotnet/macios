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

using System.Collections;

#nullable enable

namespace UIKit {
	/// <summary>A <see cref="UIView" /> that displays an action sheet with one or more buttons.</summary>
	/// <remarks>
	///   <para>Use <see cref="UIAlertController" /> rather than this class. Extensions may not use this class at all.</para>
	///   <para>
	///     The <see cref="UIActionSheet" /> control is a convenient way to allow the application user to choose among alternative actions.
	///     The following code and diagram are taken from the "Action Sheets" section of the "iOS Standard Controls" sample.
	///   </para>
	///   <example>
	///     <code lang="csharp lang-csharp"><![CDATA[
	/// protected void HandleBtnActionSheetWithOtherButtonsTouchUpInside (object sender, EventArgs e)
	/// {
	/// 	actionSheet = new UIActionSheet ("action sheet with other buttons");
	/// 	actionSheet.AddButton ("delete");
	/// 	actionSheet.AddButton ("cancel");
	/// 	actionSheet.AddButton ("a different option!");
	/// 	actionSheet.AddButton ("another option");
	/// 	actionSheet.DestructiveButtonIndex = 0;
	/// 	actionSheet.CancelButtonIndex = 1;
	/// 	//actionSheet.FirstOtherButtonIndex = 2;
	/// 	actionSheet.Clicked += delegate(object a, UIButtonEventArgs b) {
	/// 		Console.WriteLine ("Button " + b.ButtonIndex.ToString () + " clicked");
	/// 	};
	/// 	actionSheet.ShowInView (View);
	/// }
	/// ]]></code>
	///   </example>
	///   <para>
	///     <img href="~/xml/UIKit/_images/UIKit.UIActionSheet.png" alt="Screenshot showing the UIActionSheet" />
	///   </para>
	/// </remarks>
	public partial class UIActionSheet : IEnumerable {

		/// <summary>Initializes a <see cref="UIActionSheet" /> instance.</summary>
		/// <param name="title">A title to be displayed in the title area of the action sheet.</param>
		/// <param name="del">A delegate that will respond to taps in the action sheet.</param>
		/// <param name="cancelTitle">The title of the Cancel button. This will be displayed in a black button.</param>
		/// <param name="destroy">The title of the destructive button. This will be displayed in a red button.</param>
		/// <param name="other">An array of <see cref="string" /> to use for other buttons in the <see cref="UIActionSheet" />.</param>
		/// <remarks>
		///   <para>Pass <see langword="null" /> to <paramref name="title" /> if there is no text to display in the title area.</para>
		///   <para>If the action sheet is being presented on an iPad, then the <paramref name="cancelTitle" /> should be set to <see langword="null" />.</para>
		/// </remarks>
		public UIActionSheet (string? title, IUIActionSheetDelegate? del, string? cancelTitle, string? destroy, params string? []? other)
			: this (title, del, null, null, (string?) null)
		{
			if (destroy is not null)
				DestructiveButtonIndex = AddButton (destroy);

			if (other is null) {
				if (cancelTitle is not null)
					CancelButtonIndex = AddButton (cancelTitle);
				return;
			}

			foreach (string? b in other) {
				if (b is not null)
					AddButton (b);
			}

			if (cancelTitle is not null)
				CancelButtonIndex = AddButton (cancelTitle);
		}

		/// <summary>Initialize a <see cref="UIActionSheet" /> with a title and a delegate that will handle taps.</summary>
		/// <param name="title">A title to be displayed in the title area of the action sheet.</param>
		/// <param name="del">A delegate that will respond to taps in the action sheet.</param>
		/// <remarks>Pass <see langword="null" /> to <paramref name="title" /> if there is no text to display in the title area.</remarks>
		public UIActionSheet (string? title, IUIActionSheetDelegate? del)
		: this (title, del, null, null, (string?) null) { }

		/// <summary>Initialize a <see cref="UIActionSheet" /> with a title.</summary>
		/// <param name="title">A title to be displayed in the title area of the action sheet.</param>
		/// <remarks>Pass <see langword="null" /> to <paramref name="title" /> if there is no text to display in the title area.</remarks>
		public UIActionSheet (string? title)
		: this (title, null, null, null, (string?) null) { }

		/// <summary>Adds a button with the specified text.</summary>
		/// <param name="name">Text for the button.</param>
		/// <remarks>This method exists to allow the class to be initialized with C# 3.0 object initializers. This is equivalent to calling <see cref="AddButton" />.</remarks>
		public void Add (string name)
		{
			AddButton (name);
		}

		/// <summary>Obtains an enumerator that returns the button titles.</summary>
		/// <returns>An <see cref="IEnumerator" />.</returns>
		public IEnumerator GetEnumerator ()
		{
			for (int i = 0; i < ButtonCount; i++)
				yield return ButtonTitle (i);
		}
	}

}

#endif // IOS
