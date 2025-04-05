using System;
using System.Reflection;
using System.Collections;
using System.Runtime.InteropServices;

using ObjCRuntime;

#if !MONOMAC

namespace Foundation {
	public partial class NSBundleResourceRequest : NSObject {
		static NSSet<NSString> MakeSetFromTags (string [] tags)
		{
			var x = new NSString [tags.Length];
			for (int i = 0; i < tags.Length; i++)
				x [i] = new NSString (tags [i]);
			return new NSSet<NSString> (x);
		}

		static NSSet<NSString> MakeSetFromTags (NSString [] tags)
		{
			return new NSSet<NSString> (tags);
		}

		/// <param name="tags">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSBundleResourceRequest (params string [] tags) : this (MakeSetFromTags (tags)) { }
		/// <param name="bundle">To be added.</param>
		///         <param name="tags">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSBundleResourceRequest (NSBundle bundle, params string [] tags) : this (MakeSetFromTags (tags), bundle) { }

		/// <param name="tags">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSBundleResourceRequest (params NSString [] tags) : this (MakeSetFromTags (tags)) { }
		/// <param name="bundle">To be added.</param>
		///         <param name="tags">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSBundleResourceRequest (NSBundle bundle, params NSString [] tags) : this (MakeSetFromTags (tags), bundle) { }

	}
}

#endif
