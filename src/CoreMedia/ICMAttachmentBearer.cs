#nullable enable

using ObjCRuntime;

namespace CoreMedia {

	// empty interface used as a marker to state which CM objects DO support the API
	/// <summary>Marker interface for type that can bear attachments.</summary>
	///     <remarks>
	///       <para>Application developers can use the static and extension methods of the <see cref="CoreMedia.CMAttachmentBearer" /> class to operate on the dictionaries of attachments that come with system-defined objects that implement this interface.</para>
	///     </remarks>
	public interface ICMAttachmentBearer : INativeObject { }

}
