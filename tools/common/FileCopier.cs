using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

#nullable enable

namespace Xamarin.Bundler {
	public static class FileCopier {
		enum CopyFileFlags : uint {
			ACL = 1 << 0,
			Stat = 1 << 1,
			Xattr = 1 << 2,
			Data = 1 << 3,
			Security = Stat | ACL,
			Metadata = Security | Xattr,
			All = Metadata | Data,

			Recursive = 1 << 15,
			NoFollow_Src = 1 << 18,
			NoFollow_Dst = 1 << 19,
			Unlink = 1 << 21,
			Nofollow = NoFollow_Src | NoFollow_Dst,
			Clone = 1 << 24,
		}

		enum CopyFileState : uint {
			StatusCB = 6,
			StatusCtx = 7,
		}

		enum CopyFileStep {
			Start = 1,
			Finish = 2,
			Err = 3,
			Progress = 4,
		}

		enum CopyFileResult {
			Continue = 0,
			Skip = 1,
			Quit = 2,
		}

		enum CopyFileWhat {
			Error = 0,
			File = 1,
			Dir = 2,
			DirCleanup = 3,
			CopyData = 4,
			CopyXattr = 5,
		}

		[DllImport ("/usr/lib/libSystem.dylib")]
		static extern IntPtr copyfile_state_alloc ();

		[DllImport ("/usr/lib/libSystem.dylib")]
		static extern int copyfile_state_free (IntPtr state);

#if NET
		[DllImport ("/usr/lib/libSystem.dylib")]
		static extern unsafe int copyfile_state_set (IntPtr state, CopyFileState flag, delegate* unmanaged<CopyFileWhat, CopyFileStep, IntPtr, IntPtr, IntPtr, IntPtr, CopyFileResult> value);
#else
		delegate CopyFileResult CopyFileCallbackDelegate (CopyFileWhat what, CopyFileStep stage, IntPtr state, string src, string dst, IntPtr ctx);
#endif

		[DllImport ("/usr/lib/libSystem.dylib")]
		static extern unsafe int copyfile_state_set (IntPtr state, CopyFileState flag, IntPtr value);

		[DllImport ("/usr/lib/libSystem.dylib", SetLastError = true)]
		static extern int copyfile (string @from, string @to, IntPtr state, CopyFileFlags flags);

		static void ReportError (IToolLog log, int code, string format, params object? [] arguments)
		{
#if LEGACY_TOOLS || BUNDLER
			log.LogError (ErrorHelper.CreateError (code, format, arguments));
#else
			log.LogError (new Exception ($"{code} {string.Format (format, arguments)}"));
#endif
		}

		public static void UpdateDirectory (IToolLog log, string source, string target)
		{
			// first chance, try to update existing content inside `target`
			if (TryUpdateDirectory (log, source, target, out var err))
				return;

			// 2nd chance, remove `target` then copy everything
			log.Log (1, "Could not update `{0}` content (error #{1} : {2}), trying to overwrite everything...", target, err, strerror (err));
			Directory.Delete (target, true);
			if (!TryUpdateDirectory (log, source, target, out err))
				ReportError (log, 1022, Errors.MT1022, source, target, err, strerror (err));
		}

		static bool? use_managed_copying;
		static bool UseManagedCopying {
			get {
				if (!use_managed_copying.HasValue) {
					if (!string.IsNullOrEmpty (Environment.GetEnvironmentVariable ("XAMARIN_USE_MANAGED_UPDATE_DIRECTORY"))) {
						use_managed_copying = true;
					} else if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
						use_managed_copying = true;
					} else {
						use_managed_copying = false;
					}
				}
				return use_managed_copying.Value;
			}
		}

		static bool TryUpdateDirectory (IToolLog log, string source, string target, out int errno)
		{
			if (UseManagedCopying)
				return TryUpdateDirectoryWindows (log, source, target, out errno);
			return TryUpdateDirectoryMacOS (log, source, target, out errno);
		}

		static bool TryUpdateDirectoryWindows (IToolLog log, string source, string target, out int errno)
		{
			log.Log (1, $"Copying {source} to {target} recursively");
			errno = 0;
			Directory.CreateDirectory (target);

			var rv = true;
			var attr = File.GetAttributes (source);
			if (attr.HasFlag (FileAttributes.Directory)) {
				var dir = new DirectoryInfo (source);
				foreach (var sourceFile in dir.GetFiles ()) {
					var sourcePath = sourceFile.FullName;
					var targetPath = Path.Combine (target, Path.GetFileName (source), sourceFile.Name);
					CopyIfNeeded (log, sourcePath, targetPath);
				}
				foreach (var subdir in dir.GetDirectories ()) {
					rv &= TryUpdateDirectoryWindows (log, Path.Combine (source, subdir.Name), Path.Combine (target, Path.GetFileName (source)), out errno);
				}
			} else {
				var targetPath = Path.Combine (target, Path.GetFileName (source));
				CopyIfNeeded (log, source, targetPath);
			}
			return rv;
		}

		static void CopyIfNeeded (IToolLog log, string source, string target)
		{
			if (IsUptodate (log, source, target)) {
				log.Log (3, "Target '{0}' is up-to-date", target);
			} else {
				Directory.CreateDirectory (Path.GetDirectoryName (target)!);
				File.Copy (source, target, true);
				log.Log (1, "Copied {0} to {1}", source, target);
			}
		}

		static bool TryUpdateDirectoryMacOS (IToolLog log, string source, string target, out int errno)
		{
			Directory.CreateDirectory (target);

			// Mono's File.Copy can't handle symlinks (the symlinks are followed instead of copied),
			// so we need to use native functions directly. Luckily OSX provides exactly what we need.
			IntPtr state = copyfile_state_alloc ();
			var logHandle = GCHandle.Alloc (log);
			try {
#if NET
				unsafe {
					copyfile_state_set (state, CopyFileState.StatusCB, &CopyFileCallback);
				}
#else
				CopyFileCallbackDelegate del = CopyFileCallback;
				copyfile_state_set (state, CopyFileState.StatusCB, Marshal.GetFunctionPointerForDelegate (del));
#endif
				copyfile_state_set (state, CopyFileState.StatusCtx, (IntPtr) logHandle);
				int rv = copyfile (source, target, state, CopyFileFlags.Data | CopyFileFlags.Recursive | CopyFileFlags.Nofollow | CopyFileFlags.Clone);
				if (rv == 0) {
					errno = 0; // satisfy compiler and make sure not to pick up some older error code
					return true;
				} else {
					errno = Marshal.GetLastWin32Error (); // might not be very useful since the callback signaled an error (CopyFileResult.Quit)
					return false;
				}
			} finally {
				logHandle.Free ();
				copyfile_state_free (state);
			}
		}

		// do not call `Marshal.GetLastWin32Error` inside this method since it's called while the p/invoke is executing and will return `260`
#if NET
		[UnmanagedCallersOnly]
		static CopyFileResult CopyFileCallback (CopyFileWhat what, CopyFileStep stage, IntPtr state, IntPtr sourcePtr, IntPtr targetPtr, IntPtr ctx)
		{
			var source = Marshal.PtrToStringUTF8 (sourcePtr)!;
			var target = Marshal.PtrToStringUTF8 (targetPtr)!;
#else
		static CopyFileResult CopyFileCallback (CopyFileWhat what, CopyFileStep stage, IntPtr state, string source, string target, IntPtr ctx)
		{
#endif
			var log = (IToolLog) GCHandle.FromIntPtr (ctx).Target!;

			// log.Log ("CopyFileCallback ({0}, {1}, 0x{2}, {3}, {4}, 0x{5})", what, stage, state.ToString ("x"), source, target, ctx.ToString ("x"));
			switch (what) {
			case CopyFileWhat.File:
				if (!IsUptodate (log, source, target)) {
					if (stage == CopyFileStep.Finish)
						log.Log (1, "Copied {0} to {1}", source, target);
					else if (stage == CopyFileStep.Err) {
						log.Log (1, "Could not copy the file '{0}' to '{1}'", source, target);
						return CopyFileResult.Quit;
					} else if (stage == CopyFileStep.Start) {
						if (File.Exists (target) || Directory.Exists (target)) {
							log.Log (1, "Deleted target {0}, it's not up-to-date", target);
							// This callback won't be called for directories, but we can get here for symlinks to directories.
							// This means that File.Delete should always work (no need to check for a directory to call Directory.Delete)
							File.Delete (target);
						}
					}
					return CopyFileResult.Continue;
				} else {
					log.Log (3, "Target '{0}' is up-to-date", target);
					return CopyFileResult.Skip;
				}
			case CopyFileWhat.Dir:
			case CopyFileWhat.DirCleanup:
			case CopyFileWhat.CopyData:
			case CopyFileWhat.CopyXattr:
				return CopyFileResult.Continue;
			case CopyFileWhat.Error:
				log.Log (1, "Could not copy the file '{0}' to '{1}'", source, target);
				return CopyFileResult.Quit;
			default:
				return CopyFileResult.Continue;
			}
		}

		// Checks if the source file has a time stamp later than the target file.
		//
		// Optionally check if the contents of the files are different after checking the timestamp.
		//
		// If check_stamp is true, the function will use the timestamp of a "target".stamp file
		// if it's later than the timestamp of the "target" file itself.
		public static bool IsUptodate (IToolLog log, string source, string target, bool check_contents = false, bool check_stamp = true)
		{
#if LEGACY_TOOLS || BUNDLER   // msbuild does not have force                                  
			if (Driver.Force)
				return false;
#endif

			var tfi = new FileInfo (target);

			if (!tfi.Exists) {
				log.Log (3, "Target '{0}' does not exist.", target);
				return false;
			}

			if (check_stamp) {
				var tfi_stamp = new FileInfo (target + ".stamp");
				if (tfi_stamp.Exists && tfi_stamp.LastWriteTimeUtc > tfi.LastWriteTimeUtc) {
					log.Log (3, "Target '{0}' has a stamp file with newer timestamp ({1} > {2}), using the stamp file's timestamp", target, tfi_stamp.LastWriteTimeUtc, tfi.LastWriteTimeUtc);
					tfi = tfi_stamp;
				}
			}

			var sfi = new FileInfo (source);

			if (sfi.LastWriteTimeUtc <= tfi.LastWriteTimeUtc) {
				log.Log (3, "Prerequisite '{0}' is older than the target '{1}'.", source, target);
				return true;
			}

#if LEGACY_TOOLS || BUNDLER   // msbuild usages do not require CompareFiles optimization                                                              
			if (check_contents && Cache.CompareFiles (log, source, target)) {
				log.Log (3, "Prerequisite '{0}' is newer than the target '{1}', but the contents are identical.", source, target);
				return true;
			}
#else
			if (check_contents)
				throw new NotImplementedException ("Checking file contents is not supported");
#endif

			log.Log (3, "Prerequisite '{0}' is newer than the target '{1}'.", source, target);
			return false;
		}

		// Checks if any of the source files have a time stamp later than any of the target files.
		//
		// If check_stamp is true, the function will use the timestamp of a "target".stamp file
		// if it's later than the timestamp of the "target" file itself.
		public static bool IsUptodate (IToolLog log, IEnumerable<string> sources, IEnumerable<string> targets, bool check_stamp = true)
		{
#if LEGACY_TOOLS || BUNDLER  // msbuild does not have force
			if (Driver.Force)
				return false;
#endif

			DateTime max_source = DateTime.MinValue;
			string? max_s = null;

			if (sources.Count () == 0 || targets.Count () == 0)
				throw ErrorHelper.CreateError (1013, Errors.MT1013);

			foreach (var s in sources) {
				var sfi = new FileInfo (s);
				if (!sfi.Exists) {
					log.Log (3, "Prerequisite '{0}' does not exist.", s);
					return false;
				}

				var st = sfi.LastWriteTimeUtc;
				if (st > max_source) {
					max_source = st;
					max_s = s;
				}
			}


			foreach (var t in targets) {
				var tfi = new FileInfo (t);
				if (!tfi.Exists) {
					log.Log (3, "Target '{0}' does not exist.", t);
					return false;
				}

				if (check_stamp) {
					var tfi_stamp = new FileInfo (t + ".stamp");
					if (tfi_stamp.Exists && tfi_stamp.LastWriteTimeUtc > tfi.LastWriteTimeUtc) {
						log.Log (3, "Target '{0}' has a stamp file with newer timestamp ({1} > {2}), using the stamp file's timestamp", t, tfi_stamp.LastWriteTimeUtc, tfi.LastWriteTimeUtc);
						tfi = tfi_stamp;
					}
				}

				var lwt = tfi.LastWriteTimeUtc;
				if (max_source > lwt) {
					log.Log (3, "Prerequisite '{0}' is newer than target '{1}' ({2} vs {3}).", max_s, t, max_source, lwt);
					return false;
				}
			}

			log.Log (3, "Prerequisite(s) '{0}' are all older than the target(s) '{1}'.", string.Join ("', '", sources.ToArray ()), string.Join ("', '", targets.ToArray ()));

			return true;
		}

		[DllImport ("libc", SetLastError = true, EntryPoint = "strerror")]
		static extern IntPtr _strerror (int errno);

		internal static string strerror (int errno)
		{
			return Marshal.PtrToStringAuto (_strerror (errno))!;
		}
	}
}
