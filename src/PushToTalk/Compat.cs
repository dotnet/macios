#nullable enable

#if !XAMCORE_5_0

namespace PushToTalk {
	public partial class PTChannelManagerDelegate {
		public override NativeHandle ClassHandle { get => base.ClassHandle; }
	}

	public partial class PTChannelRestorationDelegate {
		public override NativeHandle ClassHandle { get => base.ClassHandle; }
	}
}

#endif // !XAMCORE_5_0
