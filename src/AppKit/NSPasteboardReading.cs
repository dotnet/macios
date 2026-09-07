#if !__MACCATALYST__

#nullable enable

namespace AppKit {
	/// <summary>Provides methods for reading data from the pasteboard.</summary>
	public partial interface INSPasteboardReading {
		[BindingImpl (BindingImplOptions.Optimizable)]
		public unsafe static T? CreateInstance<T> (NSObject propertyList, NSPasteboardType type) where T : NSObject, INSPasteboardReading
		{
			return CreateInstance<T> (propertyList, type.GetConstant ()!);
		}
	}
}
#endif // !__MACCATALYST__
