//
// SKVideoNode.cs: extensions to SKVideoNode
//
// Authors:
//   Alex Soto (alex.soto@xamarin.com)
//
// Copyright 2016 Xamarin Inc.
//

using System;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace SpriteKit {
	public partial class SKVideoNode : SKNode {

		static bool CheckSystemVersion ()
		{
#if MONOMAC
			return SystemVersion.CheckmacOS (10, 10);
#elif TVOS || IOS
			return SystemVersion.CheckiOS (8, 0);
#else
#error Unknown platform
#endif
		}

		// Apple deprecated videoNodeWithVideoFileNamed: in 10.10/8.0
		// and made available videoNodeWithFileNamed: so we invoke
		// the right one at runtime depending on which OS version we are running
		// https://bugzilla.xamarin.com/show_bug.cgi?id=37727
		/// <param name="videoFile">To be added.</param>
		///         <summary>Create a video node from the named video file.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static SKVideoNode FromFile (string videoFile)
		{
			if (CheckSystemVersion ())
				return VideoNodeWithFileNamed (videoFile);
			else
				return VideoNodeWithVideoFileNamed (videoFile);
		}

		// Apple deprecated videoNodeWithVideoURL: in 10.10/8.0
		// and made available videoNodeWithURL: so we invoke
		// the right one at runtime depending on which OS version we are running
		// https://bugzilla.xamarin.com/show_bug.cgi?id=37727
		/// <param name="videoUrl">To be added.</param>
		///         <summary>Creates a video node from the file at the specified URL.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static SKVideoNode FromUrl (NSUrl videoUrl)
		{
			if (CheckSystemVersion ())
				return VideoNodeWithURL (videoUrl);
			else
				return VideoNodeWithVideoURL (videoUrl);
		}

		// Apple deprecated initWithVideoFileNamed: in 10.10/8.0
		// and made available initWithFileNamed: so we invoke
		// the right one at runtime depending on which OS version we are running
		// https://bugzilla.xamarin.com/show_bug.cgi?id=37727
		/// <param name="videoFile">To be added.</param>
		///         <summary>Creates a new <see cref="T:SpriteKit.SKVideoNode" /> that plays video loaded from the file named <paramref name="videoFile" />.</summary>
		///         <remarks>To be added.</remarks>
		[DesignatedInitializer]
		public SKVideoNode (string videoFile)
		{
			if (CheckSystemVersion ())
				Handle = InitWithFileNamed (videoFile);
			else
				Handle = InitWithVideoFileNamed (videoFile);
		}

		// Apple deprecated initWithVideoURL: in 10.10/8.0
		// and made available initWithURL: so we invoke
		// the right one at runtime depending on which OS version we are running
		// https://bugzilla.xamarin.com/show_bug.cgi?id=37727
		/// <param name="url">To be added.</param>
		///         <summary>Creates a new <see cref="T:SpriteKit.SKVideoNode" /> to play the content located at <paramref name="url" />.</summary>
		///         <remarks>To be added.</remarks>
		[DesignatedInitializer]
		public SKVideoNode (NSUrl url)
		{
			if (CheckSystemVersion ())
				Handle = InitWithURL (url);
			else
				Handle = InitWithVideoURL (url);
		}
	}
}
