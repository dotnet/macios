using System;

#if LEGACY_TOOLS || BUNDLER
using Xamarin.Bundler;
using Xamarin.Utils;
#endif

namespace Xamarin {
	public sealed class ProductConstants {
		public string Version;
		public string Revision;
		public const string Hash = "@PRODUCT_HASH@";

		internal const string XcodeVersion = "@XCODE_VERSION@";
		internal const string XcodeBumpCommitDistance = "@XCODE_BUMP_COMMIT_DISTANCE@";
		public const string SharpieVersion = XcodeVersion + ".0." + XcodeBumpCommitDistance;

		ProductConstants (string version, string revision)
		{
			Version = version;
			Revision = revision;
		}
		public readonly static ProductConstants iOS = new ProductConstants ("@IOS_NUGET_VERSION@", "@IOS_NUGET_REVISION@");
		public readonly static ProductConstants tvOS = new ProductConstants ("@TVOS_NUGET_VERSION@", "@TVOS_NUGET_REVISION@");
		public readonly static ProductConstants macOS = new ProductConstants ("@MACOS_NUGET_VERSION@", "@MACOS_NUGET_REVISION@");
		public readonly static ProductConstants MacCatalyst = new ProductConstants ("@MACCATALYST_NUGET_VERSION@", "@MACCATALYST_NUGET_VERSION@");
	}
}
