// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using CoreFoundation;
using ObjCRuntime;

namespace NativeFactories {
	public class ForeignWrapper : NativeObject, ICreatableNativeObject<ForeignWrapper> {
		protected ForeignWrapper (NativeHandle handle, bool owns) : base (handle, owns) { }
		static ForeignWrapper ICreatableNativeObject<ForeignWrapper>.Create (NativeHandle handle, bool owns)
			=> new ForeignWrapper (handle, owns);
	}

	public class InheritedForeignWrapper : ForeignWrapper {
		internal InheritedForeignWrapper (NativeHandle handle, bool owns) : base (handle, owns) { }
	}
}
