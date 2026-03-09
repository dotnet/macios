// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.ComponentModel;

#nullable enable

namespace ObjCRuntime {

	/// <summary>Base type for Objective-C trampoline blocks.</summary>
	// infrastructure code - not intended to be used directly by user code
	[EditorBrowsable (EditorBrowsableState.Never)]
	public abstract class TrampolineBlockBase {

		readonly IntPtr blockPtr;

		internal TrampolineBlockBase (IntPtr block, bool owns)
		{
			if (owns) {
				blockPtr = block;
			} else {
				blockPtr = BlockLiteral.Copy (block);
			}
		}

		/// <summary>Initializes a new instance from a block literal pointer.</summary>
		/// <param name="block">A pointer to a native <see cref="BlockLiteral"/>.</param>
		protected unsafe TrampolineBlockBase (BlockLiteral* block)
			: this ((IntPtr) block, false)
		{
		}

		~TrampolineBlockBase ()
		{
			Runtime.ReleaseBlockOnMainThread (blockPtr);
		}

		/// <summary>Gets the native block pointer for this instance.</summary>
		/// <value>The native block pointer.</value>
		protected IntPtr BlockPointer {
			get { return blockPtr; }
		}

		/// <summary>Gets the existing managed delegate for a block if it wraps managed code.</summary>
		/// <param name="block">The native block pointer.</param>
		/// <returns>The managed delegate if this is a managed block; otherwise, <see langword="null"/>.</returns>
		protected unsafe static object? GetExistingManagedDelegate (IntPtr block)
		{
			if (block == IntPtr.Zero)
				return null;
			if (!BlockLiteral.IsManagedBlock (block))
				return null;
			return ((BlockLiteral*) block)->Target;
		}
	}
}
