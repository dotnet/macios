using Foundation;

namespace BackgroundAssets {
	public partial class BAAssetPackManifest {
		/// <summary>Create a new <see cref="BAAssetPackManifest" /> for the specified file on disk.</summary>
		/// <param name="url">The url of the file on disk. The file is expected to be formatted as json.</param>
		/// <param name="applicationGroupIdentifier">The identifier for the application group where the downloaded assets will be stored.</param>
		/// <param name="error">The error if an error occurred.</param>
		/// <returns>A new <see cref="BAAssetPackManifest" /> if the operation succeeded, <see langword="null" /> otherwise.</returns>
		public static BAAssetPackManifest? Create (NSUrl url, string applicationGroupIdentifier, out NSError? error)
		{
			var rv = new BAAssetPackManifest (NSObjectFlag.Empty);
			rv.InitializeHandle (rv._InitWithContentsOfUrl (url, applicationGroupIdentifier, out error), "initWithContentsOfURL:applicationGroupIdentifier:error:");
			if (rv.Handle == IntPtr.Zero) {
				rv.Dispose ();
				return null;
			}
			return rv;
		}

		/// <summary>Create a new <see cref="BAAssetPackManifest" /> for the specified json data in memory.</summary>
		/// <param name="data">The json data to use.</param>
		/// <param name="applicationGroupIdentifier">The identifier for the application group where the downloaded assets will be stored.</param>
		/// <param name="error">The error if an error occurred.</param>
		/// <returns>A new <see cref="BAAssetPackManifest" /> if the operation succeeded, <see langword="null" /> otherwise.</returns>
		public static BAAssetPackManifest? Create (NSData data, string applicationGroupIdentifier, out NSError? error)
		{
			var rv = new BAAssetPackManifest (NSObjectFlag.Empty);
			rv.InitializeHandle (rv._InitFromData (data, applicationGroupIdentifier, out error), "initFromData:applicationGroupIdentifier:error:");
			if (rv.Handle == IntPtr.Zero) {
				rv.Dispose ();
				return null;
			}
			return rv;
		}
	}
}
