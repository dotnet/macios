using System.Threading.Tasks;
using CloudKit;

#nullable enable

namespace Foundation {
	public partial class NSItemProvider {
#if MONOMAC
		/// <summary>Asynchronously registers a CloudKit share.</summary>
		/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="CloudKitRegistrationPreparationHandler" /> that can be used to prepare the share.</returns>
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

		/// <summary>Loads an object of the specified type.</summary>
		/// <typeparam name="T">The type of object to load. Must conform to <see cref="INSItemProviderReading" />.</typeparam>
		/// <param name="completionHandler">A completion handler that is called when the object is loaded. The first parameter contains the loaded object (or <see langword="null" /> if loading failed), and the second parameter contains an error if one occurred.</param>
		/// <returns>An <see cref="NSProgress" /> object that can be used to track the loading progress.</returns>
		public NSProgress LoadObject<T> (Action<T?, NSError?> completionHandler) where T : NSObject, INSItemProviderReading
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

		/// <summary>Asynchronously loads an object of the specified type.</summary>
		/// <typeparam name="T">The type of object to load. Must conform to <see cref="INSItemProviderReading" />.</typeparam>
		/// <returns>A task that represents the asynchronous operation. The task result contains the loaded object, or <see langword="null" /> if loading failed.</returns>
		public Task<T?> LoadObjectAsync<T> () where T : NSObject, INSItemProviderReading
		{
			var rv = LoadObjectAsync (new Class (typeof (T)));
			return rv.ContinueWith ((v) => {
				var result = v.Result;
				var obj = result as T;
				if (obj is null && result is not null) {
					obj = Runtime.ConstructNSObject<T> (result.Handle);
					GC.KeepAlive (result);
				}
				return obj;
			});
		}

		/// <summary>Asynchronously loads an object of the specified type.</summary>
		/// <typeparam name="T">The type of object to load. Must conform to <see cref="INSItemProviderReading" />.</typeparam>
		/// <param name="result">When this method returns, contains an <see cref="NSProgress" /> object that can be used to track the loading progress.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the loaded object, or <see langword="null" /> if loading failed.</returns>
		public Task<T?> LoadObjectAsync<T> (out NSProgress result) where T : NSObject, INSItemProviderReading
		{
			var rv = LoadObjectAsync (new Class (typeof (T)), out result);
			return rv.ContinueWith ((v) => {
				var result = v.Result;
				var obj = result as T;
				if (obj is null && result is not null) {
					obj = Runtime.ConstructNSObject<T> (result.Handle);
					GC.KeepAlive (result);
				}
				return obj;
			});
		}
	}
}
