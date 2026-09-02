#nullable enable

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace GameController {

#if !XAMCORE_5_0
	// The GCMouse doesn't conform to NSCoding/NSSecureCoding, but it probably did in an early beta, which is why we declared it as such.
	public partial class GCMouse : INSCoding, INSSecureCoding {
		[BindingImpl (BindingImplOptions.Optimizable)]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public GCMouse (NSCoder coder) : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				InitializeHandle (global::ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.GetHandle ("initWithCoder:"), coder.Handle), "initWithCoder:");
				GC.KeepAlive (coder);
			} else {
				unsafe {
					var __objc_super__ = new global::ObjCRuntime.ObjCSuper (this);
					InitializeHandle (global::ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (&__objc_super__, Selector.GetHandle ("initWithCoder:"), coder.Handle), "initWithCoder:");
					GC.KeepAlive (coder);
					GC.KeepAlive (this);
				}
			}
		}

		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos14.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public virtual void EncodeTo (NSCoder encoder)
		{
			var encoder__handle__ = encoder!.GetNonNullHandle (nameof (encoder));
			if (IsDirectBinding) {
				global::ObjCRuntime.Messaging.void_objc_msgSend_NativeHandle (this.Handle, Selector.GetHandle ("encodeWithCoder:"), encoder__handle__);
			} else {
				unsafe {
					var __objc_super__ = new global::ObjCRuntime.ObjCSuper (this);
					global::ObjCRuntime.Messaging.void_objc_msgSendSuper_NativeHandle (&__objc_super__, Selector.GetHandle ("encodeWithCoder:"), encoder__handle__);
					GC.KeepAlive (this);
				}
			}
			GC.KeepAlive (encoder);
		}
	}
#endif // !XAMCORE_5_0
}
