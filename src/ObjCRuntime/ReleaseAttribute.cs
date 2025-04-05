//
// Release attribute
//
using System;

namespace ObjCRuntime {
	/// <summary>This attribute indicates that the return value of a function is retained (the caller obtains a reference to the object returned).</summary>
	///     <remarks>
	///       <para>
	///       The Xamarin.iOS runtime uses this attribute to determine the reference counting behavior on the boundary between Objective-C and managed code.
	///       </para>
	///       <example>
	///         <code lang="csharp lang-csharp"><![CDATA[
	///           class MyUrl : NSUrl { // NSUrl implements NSCopying
	///             public override NSObject Clone ()
	///             {
	///               return base.Clone ();
	///             }
	///           }
	///           ]]></code>
	///       </example>
	///       <para>
	///         In this example Xamarin.iOS will first call 'release' on the object returned by [NSUrl clone] (the call to base.Clone), then call 'retain' as the object is returned to Objective-C.
	///       </para>
	///     </remarks>
	[AttributeUsage (AttributeTargets.ReturnValue)]
	public sealed class ReleaseAttribute : Attribute {
	}
}

