using System;
using System.Threading.Tasks;
using CloudKit;
using ObjCRuntime;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace Foundation {
	public partial class NSItemProvider {
#if MONOMAC
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public virtual Task<CloudKitRegistrationPreparationHandler> RegisterCloudKitShareAsync ()
		{
			var tcs = new TaskCompletionSource<CloudKitRegistrationPreparationHandler> ();
			CloudKitRegistrationPreparationAction action = (handler) => {
				tcs.SetResult (handler);
			};
			RegisterCloudKitShare (action);
			return tcs.Task;
		}
#endif

		/// <typeparam name="T">To be added.</typeparam>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		public NSProgress LoadObject<T> (Action<T, NSError> completionHandler) where T : NSObject, INSItemProviderReading
		{
			return LoadObject (new Class (typeof (T)), (rv, err) => {
				var obj = rv as T;
				if (obj is null && rv is not null) {
					obj = Runtime.ConstructNSObject<T> (rv.Handle);
					GC.KeepAlive (rv);
				}
				completionHandler (obj, err);
			});
		}

		/// <typeparam name="T">To be added.</typeparam>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		public Task<T> LoadObjectAsync<T> () where T : NSObject, INSItemProviderReading
		{
			var rv = LoadObjectAsync (new Class (typeof (T)));
			return rv.ContinueWith ((v) => {
				var obj = v.Result as T;
				if (obj is null && v.Result is not null)
					obj = Runtime.ConstructNSObject<T> (v.Result.Handle);
				return obj;
			});
		}

		/// <typeparam name="T">To be added.</typeparam>
		///         <param name="result">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		public Task<T> LoadObjectAsync<T> (out NSProgress result) where T : NSObject, INSItemProviderReading
		{
			var rv = LoadObjectAsync (new Class (typeof (T)), out result);
			return rv.ContinueWith ((v) => {
				var obj = v.Result as T;
				if (obj is null && v.Result is not null)
					obj = Runtime.ConstructNSObject<T> (v.Result.Handle);
				return obj;
			});
		}
	}
}
