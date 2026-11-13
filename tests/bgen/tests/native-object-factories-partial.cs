// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ObjCRuntime;

namespace CoreFoundation {
	public partial class DispatchData : NativeObject, ICreatableNativeObject<DispatchData> {
		internal DispatchData (NativeHandle handle, bool owns) : base (handle, owns) { }
		static DispatchData ICreatableNativeObject<DispatchData>.Create (NativeHandle handle, bool owns)
			=> new DispatchData (handle, owns);
	}
}
