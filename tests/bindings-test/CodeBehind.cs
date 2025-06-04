using System;
using System.Runtime.InteropServices;

using Foundation;
using ObjCRuntime;

using MatrixFloat2x2 = global::CoreGraphics.NMatrix2;
using MatrixFloat3x3 = global::CoreGraphics.NMatrix3;
using MatrixFloat4x3 = global::CoreGraphics.NMatrix4x3;
using MatrixFloat4x4 = global::CoreGraphics.NMatrix4;

namespace Bindings.Test {
	public unsafe partial class EvilDeallocator : NSObject {
		public void MarkMeDirty ()
		{
			MarkDirty ();
		}
	}
}
