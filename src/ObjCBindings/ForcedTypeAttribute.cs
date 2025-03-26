using System;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;
using ObjCRuntime;

#nullable enable

namespace ObjCBindings {

	/// <summary>
	/// ForcedTypeAttribute
	///
	/// The ForcedTypeAttribute is used to enforce the creation of a managed type even
	/// if the returned unmanaged object does not match the type described in the binding definition.
	///
	/// This is useful when the type described in a header does not match the returned type
	/// of the native method for example take the following Objective-C definition from NSURLSession:
	///
	///	- (NSURLSessionDownloadTask *)downloadTaskWithRequest:(NSURLRequest *)request
	///
	/// It clearly states that it will return an NSURLSessionDownloadTask instance, but yet
	/// it returns a NSURLSessionTask, which is a superclass and thus not convertible to 
	/// NSURLSessionDownloadTask. Since we are in a type-safe context an InvalidCastException will happen.
	///
	/// In order to comply with the header description and avoid the InvalidCastException, 
	/// the ForcedTypeAttribute is used.
	///
	/// <code>
	///	[BindingType&lt;Class&gt;]
	///	public partial class NSUrlSession {
	///		[Export&lt;Method&gt; ("downloadTaskWithRequest:")]
	///		[return: ForcedType]
	///		public virtual partial NSUrlSessionDownloadTask CreateDownloadTask (NSUrlRequest request);
	///	}
	///	</code>
	///
	/// The `ForcedTypeAttribute` also accepts a boolean value named `Owns` that is `false`
	/// by default `[ForcedType (owns: true)]`. The owns parameter could be used to follow
	/// the Ownership Policy[1] for Core Foundation objects.
	///
	/// [1]: https://developer.apple.com/library/content/documentation/CoreFoundation/Conceptual/CFMemoryMgmt/Concepts/Ownership.html
	/// </summary>
	[Experimental ("APL0003")]
	[AttributeUsage (AttributeTargets.ReturnValue | AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false)]
	public class ForcedTypeAttribute : Attribute {
		/// <summary>
		/// Indicates if the object is owned by the caller or not.
		/// </summary>
		public bool Owns { get; set; } = false;

		/// <summary>
		/// Creates a new instance of the <see cref="ForcedTypeAttribute"/> class.
		/// </summary>
		public ForcedTypeAttribute (bool owns = false)
		{
			Owns = owns;
		}

	}
}


