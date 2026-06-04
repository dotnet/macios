#if !__MACCATALYST__

#nullable enable

namespace AppKit {

	public partial class NSCollectionViewLayout {
		/// <param name="itemClass">The class to register for the decoration view.</param>
		/// <param name="elementKind">The element kind identifier for the decoration view.</param>
		/// <summary>Registers a class for use in creating decoration views.</summary>
		public void RegisterClassForDecorationView (Type itemClass, NSString elementKind)
		{
			_RegisterClassForDecorationView (Class.GetHandle (itemClass), elementKind);
		}
	}
}
#endif // !__MACCATALYST__
