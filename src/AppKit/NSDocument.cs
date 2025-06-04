#if !__MACCATALYST__
using System;
using System.Collections.Generic;

using Foundation;
using ObjCRuntime;

#nullable enable

namespace AppKit {

	public partial class NSDocument {
		/// <param name="document">To be added.</param>
		///     <param name="didDuplicate">To be added.</param>
		///     <summary>To be added.</summary>
		///     <remarks>To be added.</remarks>
		public delegate void DuplicateCallback (NSDocument document, bool didDuplicate);

		[Register ("__NSDocumentDuplicateCallback")]
		internal class Callback : NSObject {
			DuplicateCallback callback;

			public Callback (DuplicateCallback callback)
			{
				this.callback = callback;
				IsDirectBinding = false;
				DangerousRetain ();
			}

			[Export ("document:didDuplicate:contextInfo:")]
			void SelectorCallback (NSDocument source, bool didDuplicate, IntPtr contextInfo)
			{
				try {
					callback (source, didDuplicate);
				} finally {
					DangerousRelease ();
				}
			}
		}

		/// <param name="callback">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void DuplicateDocument (DuplicateCallback? callback)
		{
			if (callback is null) {
				_DuplicateDocument (null, null, IntPtr.Zero);
			} else {
				_DuplicateDocument (new Callback (callback), new Selector ("document:didDuplicate:contextInfo:"), IntPtr.Zero);
			}
		}
	}
}
#endif // !__MACCATALYST__
