#if !XAMCORE_5_0 && __MACOS__

using System.ComponentModel;

using AudioToolbox;

#nullable enable

namespace AVFoundation {
	public partial class AVSampleCursor {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Obsolete ("Use 'CurrentSampleDependencyInfo2' instead. The property type of this property is wrong.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public virtual AVSampleCursorSyncInfo CurrentSampleDependencyInfo {
			get => throw new NotSupportedException ("Use 'CurrentSampleDependencyInfo2' instead. The property type of this property is wrong.");
		}
	}
}

#endif // !XAMCORE_5_0 && __MACOS__
