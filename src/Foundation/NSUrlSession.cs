
using System.IO;

#nullable enable

namespace Foundation {
	/// <summary>Holds the return values from the asynchronous download task methods in <see cref="NSUrlSession" />.</summary>
	/// <remarks>
	/// <para>This class holds the return values from the asynchronous download task methods in <see cref="NSUrlSession" />.</para>
	/// </remarks>
	public partial class NSUrlSessionDownloadTaskRequest : IDisposable {
		string? tmpfile;

		partial void Initialize ()
		{
			// Location points to a temporary file on disk which is deleted
			// when returning from the delegate callback.
			//
			// iOS docs recommend to open the file for reading or moving
			// it, but we're left with only moving it, since iOS will
			// call 'unlink' to remove the file upon return, which means
			// that even if you've opened the file, you can only access it
			// using the file handle, and we've exposed the file name to
			// the user (which would still be unusable).
			//
			// So instead move the file so that iOS doesn't delete it,
			// expose the new filename, and keep it alive as long as this
			// object is alive.
			//
			// See bug #31427.

			var locationPath = Location.Path;
			if (!string.IsNullOrEmpty (locationPath)) {
				tmpfile = Path.GetTempFileName ();
				File.Delete (tmpfile);
				File.Move (locationPath, tmpfile);
				Location = NSUrl.FromFilename (tmpfile);
			}
		}

		/// <summary>Finalizer for the <see cref="NSUrlSessionDownloadTaskRequest" /> class.</summary>
		~NSUrlSessionDownloadTaskRequest ()
		{
			Dispose (false);
		}

		/// <summary>Releases the resources used by the <see cref="NSUrlSessionDownloadTaskRequest" /> object.</summary>
		/// <remarks>
		///   <para>The Dispose method releases the resources used by the <see cref="NSUrlSessionDownloadTaskRequest" /> class.</para>
		///   <para>Calling the Dispose method when the application is finished using the <see cref="NSUrlSessionDownloadTaskRequest" /> ensures that all external resources used by this managed object are released as soon as possible. Once developers have invoked the Dispose method, the object is no longer useful and developers should no longer make any calls to it. For more information on releasing resources see ``Cleaning up Unmananaged Resources'' at https://msdn.microsoft.com/en-us/library/498928w2.aspx</para>
		/// </remarks>
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		/// <summary>Releases the resources used by the <see cref="NSUrlSessionDownloadTaskRequest" /> object.</summary>
		/// <param name="disposing">
		///   <para>
		///     If set to <see langword="true" />, the method is invoked directly and will dispose managed
		///     and unmanaged resources; If set to <see langword="false" /> the method is being called by
		///     the garbage collector finalizer and should only release unmanaged resources.
		///   </para>
		/// </param>
		/// <remarks>
		///   <para>This Dispose method releases the resources used by the <see cref="NSUrlSessionDownloadTaskRequest" /> class.</para>
		///   <para>This method is called by both the Dispose() method and the object finalizer (Finalize). When invoked by the Dispose method, the parameter <paramref name="disposing" /> is set to <see langword="true" /> and any managed object references that this object holds are also disposed or released; when invoked by the object finalizer, on the finalizer thread the value is set to <see langword="false" />.</para>
		///   <para>Calling the Dispose method when the application is finished using the <see cref="NSUrlSessionDownloadTaskRequest" /> ensures that all external resources used by this managed object are released as soon as possible. Once developers have invoked the Dispose method, the object is no longer useful and developers should no longer make any calls to it.</para>
		///   <para>For more information on how to override this method and on the Dispose/IDisposable pattern, read the ``Implementing a Dispose Method'' document at https://msdn.microsoft.com/en-us/library/fs2xkftw.aspx</para>
		/// </remarks>
		protected void Dispose (bool disposing)
		{
			if (tmpfile is not null) {
				try {
					File.Delete (tmpfile);
				} catch {
					// We don't care if we can't delete the tmp file.
				}
				tmpfile = null;
			}
		}
	}
}
