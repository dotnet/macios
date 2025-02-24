
using System;
using System.IO;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace Foundation {
	public partial class NSUrlSessionDownloadTaskRequest : IDisposable {
		string tmpfile;

		partial void Initialize ()
		{
			// Location points to a temporary file on disk which is deleted
			// which is deleted when returning from the delegate callback.
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

			tmpfile = Path.GetTempFileName ();
			File.Delete (tmpfile);
			File.Move (Location.Path, tmpfile);
			Location = NSUrl.FromFilename (tmpfile);
		}

		~NSUrlSessionDownloadTaskRequest ()
		{
			Dispose (false);
		}

		/// <summary>Releases the resources used by the NSUrlSessionDownloadTaskRequest object.</summary>
		///         <remarks>
		///           <para>The Dispose method releases the resources used by the NSUrlSessionDownloadTaskRequest class.</para>
		///           <para>Calling the Dispose method when the application is finished using the NSUrlSessionDownloadTaskRequest ensures that all external resources used by this managed object are released as soon as possible.  Once developers have invoked the Dispose method, the object is no longer useful and developers should no longer make any calls to it.  For more information on releasing resources see ``Cleaning up Unmananaged Resources'' at https://msdn.microsoft.com/en-us/library/498928w2.aspx</para>
		///         </remarks>
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		/// <include file="../../docs/api/Foundation/NSUrlSessionDownloadTaskRequest.xml" path="/Documentation/Docs[@DocId='M:Foundation.NSUrlSessionDownloadTaskRequest.Dispose(System.Boolean)']/*" />
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
