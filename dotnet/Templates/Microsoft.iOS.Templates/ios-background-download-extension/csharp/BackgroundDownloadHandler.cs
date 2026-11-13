// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using BackgroundAssets;
using Foundation;
using ObjCRuntime;

namespace iOSBackgroundDownloadExtension1;

[Register ("BackgroundDownloadHandler")]
public class BackgroundDownloadHandler : NSObject, IBAManagedDownloaderExtension {
	protected BackgroundDownloadHandler (NativeHandle handle) : base (handle)
	{
	}

	public bool ShouldDownload (BAAssetPack assetPack)
	{
		// Filter asset packs here, or return true to use their configured download policies.
		return true;
	}
}
