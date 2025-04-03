//
// SCNNode.cs: extensions to SCNNode
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)   
//
// Copyright Xamarin Inc.
//

using System;
using System.Collections;
using System.Collections.Generic;

using CoreFoundation;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace SpriteKit {
	public partial class SKNode : IEnumerable, IEnumerable<SKNode> {
#if NET
		/// <typeparam name="T">To be added.</typeparam>
		///         <param name="file">
		///
		/// Filename containing the SpriteKit assets, without the extension.</param>
		///         <summary>Creates a new <see cref="T:SpriteKit.SKNode" /> by loading the assets from a file included in the application. </summary>
		///         <returns>
		///
		/// The new instance of the node.   The parameter type <typeparamref name="T" /> is used to determine which kind of class you want to get out of the file.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		public static T? FromFile<T> (string file) where T : SKNode
		{
			var fileHandle = CFString.CreateNative (file);
			try {
				var handle = ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (Class.GetHandle (typeof (T)), Selector.GetHandle ("nodeWithFileNamed:"), fileHandle);
				return Runtime.GetNSObject<T> (handle);
			} finally {
				CFString.ReleaseNative (fileHandle);
			}
		}

		/// <param name="node">To be added.</param>
		///         <summary>Adds <paramref name="node" /> to the end of the list of child nodes.</summary>
		///         <remarks>To be added.</remarks>
		public void Add (SKNode node)
		{
			AddChild (node);
		}

		/// <param name="nodes">To be added.</param>
		///         <summary>Adds <paramref name="nodes" /> to the end of the list of child nodes.</summary>
		///         <remarks>To be added.</remarks>
		public void AddNodes (params SKNode []? nodes)
		{
			if (nodes is null)
				return;
			foreach (var n in nodes)
				AddChild (n);
		}

		/// <summary>Returns an enumerator that iterates over the child nodes that belong to the current node.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public IEnumerator<SKNode> GetEnumerator ()
		{
			foreach (var node in Children)
				yield return node;
		}

		/// <summary>Internal.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

#if NET
		/// <param name="filename">To be added.</param>
		///         <param name="types">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
#endif
		public static SKNode? Create (string filename, Type [] types, out NSError error)
		{
			// Let's fail early.
			if (filename is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (filename));
			if (types is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (types));
			if (types.Length == 0)
				ObjCRuntime.ThrowHelper.ThrowArgumentException (nameof (types), "Length must be greater than zero.");

			using (var classes = new NSMutableSet<Class> ((nint) types.Length)) {
				foreach (var type in types)
					classes.Add (new Class (type));
				SKNode? result = Create (filename, classes.Handle, out error);
				GC.KeepAlive (classes);
				return result;
			}
		}

#if NET
		/// <param name="filename">To be added.</param>
		///         <param name="classes">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
#endif
		public static SKNode? Create (string filename, NSSet<Class> classes, out NSError error)
		{
			// `filename` will be checked by `Create` later
			if (classes is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (classes));
			if (classes.Count == 0)
				ObjCRuntime.ThrowHelper.ThrowArgumentException (nameof (classes), "Length must be greater than zero.");

			SKNode? result = Create (filename, classes.Handle, out error);
			GC.KeepAlive (classes);
			return result;
		}
	}
}
