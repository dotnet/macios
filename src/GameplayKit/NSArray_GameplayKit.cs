//
// NSArray_GameplayKit.cs: Generic extensions to NSArray
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

#nullable enable

using System;
using Foundation;
using ObjCRuntime;

namespace GameplayKit {

#if NET
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
#endif
	public static class NSArray_GameplayKit {

		/// <typeparam name="T">To be added.</typeparam>
		///         <param name="This">The instance on which this method operates.</param>
		///         <param name="randomSource">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("shuffledArrayWithRandomSource:")]
		public static T [] GetShuffledArray<T> (this NSArray This, GKRandomSource randomSource) where T : class, INativeObject
		{
			if (randomSource is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (randomSource));
			T [] result = NSArray.ArrayFromHandle<T> (Messaging.IntPtr_objc_msgSend_IntPtr (This.Handle, Selector.GetHandle ("shuffledArrayWithRandomSource:"), randomSource.Handle));
			GC.KeepAlive (This);
			GC.KeepAlive (randomSource);
			return result;
		}

		/// <typeparam name="T">To be added.</typeparam>
		///         <param name="This">The instance on which this method operates.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("shuffledArray")]
		public static T [] GetShuffledArray<T> (this NSArray This) where T : class, INativeObject
		{
			T [] result = NSArray.ArrayFromHandle<T> (Messaging.IntPtr_objc_msgSend (This.Handle, Selector.GetHandle ("shuffledArray")));
			GC.KeepAlive (This);
			return result;
		}
	}
}
