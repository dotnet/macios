#if !XAMCORE_2_0
using MonoMac.Foundation;
#else
using Foundation;
#endif

namespace Test
{
	[BaseType (typeof (NSObject))]
	interface SimServiceConnectionManager {}

	// Both these should produce the same output,
	// both calling xamarin_IntPtr_objc_msgSend
	[BaseType (typeof (NSObject))]
	interface MarshalOnProperty
	{
		[Static]
		[Export ("sharedConnectionManager")]
		[MarshalNativeExceptions]
		SimServiceConnectionManager Shared { get; }
	}

	[BaseType (typeof (NSObject))]
	interface MarshalInProperty
	{
		[Static]
		[Export ("sharedConnectionManager")]
		SimServiceConnectionManager Shared { [MarshalNativeExceptions]get; }
	}
}