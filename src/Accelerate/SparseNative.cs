// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System.Numerics;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Accelerate {
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	static unsafe class SparseNative {
		[StructLayout (LayoutKind.Sequential)]
		internal struct Matrix {
			internal int Rows, Columns;
			internal long* Starts;
			internal int* Indices;
			internal void* Data;
			internal int Kind, Triangle, BlockSize, Operation;
		}

		[StructLayout (LayoutKind.Sequential)]
		internal struct Options {
			internal int Order;
			internal int* Permutation;
			internal int* Ignored;
			internal int Scaling;
			internal void* Scale;
			internal double PivotTolerance, ZeroTolerance;
			internal IntPtr Allocate, Deallocate;
		}

		[StructLayout (LayoutKind.Sequential)]
		internal struct IterationOptions {
			internal int Method, Variant, Vectors, MaxIterations, Convergence;
			internal double AbsoluteTolerance, RelativeTolerance, Damping, RhsTolerance, ConditionLimit;
			internal IntPtr Report, User;
		}

		[StructLayout (LayoutKind.Sequential)]
		internal struct UserPreconditioner {
			internal IntPtr Apply, User;
		}

		internal static int Scalar<T> () where T : unmanaged
		{
			if (typeof (T) == typeof (double)) return 0;
			if (typeof (T) == typeof (float)) return 1;
			if (typeof (T) != typeof (Complex) && typeof (T) != typeof (SparseComplexFloat))
				throw new NotSupportedException ("Sparse solvers support double, float, System.Numerics.Complex, and SparseComplexFloat.");
			RequireComplex ();
			return typeof (T) == typeof (Complex) ? 2 : 3;
		}

		internal static void RequireComplex ()
		{
			if (!(OperatingSystem.IsMacOSVersionAtLeast (15, 5) || OperatingSystem.IsIOSVersionAtLeast (18, 5) || OperatingSystem.IsTvOSVersionAtLeast (18, 5) || OperatingSystem.IsMacCatalystVersionAtLeast (18, 5)))
				throw new PlatformNotSupportedException ("Complex sparse solvers require macOS 15.5, iOS 18.5, tvOS 18.5, or Mac Catalyst 18.5.");
		}

		internal static void RequireVersion26 ()
		{
			if (!(OperatingSystem.IsMacOSVersionAtLeast (26) || OperatingSystem.IsIOSVersionAtLeast (26) || OperatingSystem.IsTvOSVersionAtLeast (26) || OperatingSystem.IsMacCatalystVersionAtLeast (26)))
				throw new PlatformNotSupportedException ("This sparse solver operation requires OS version 26 or later.");
		}

		internal static void Check (int status = 0)
		{
			var message = Marshal.PtrToStringUTF8 (xamarin_sparse_error ());
			if (!string.IsNullOrEmpty (message)) {
				if (xamarin_sparse_allocation_failed () != 0)
					throw new OutOfMemoryException (message);
				throw new InvalidOperationException (message);
			}
			SparseValidation.CheckStatus (status);
		}

		internal static byte* Align (Span<byte> workspace, byte* pointer, nuint size)
		{
			var aligned = (byte*) (((nuint) pointer + 15) & ~(nuint) 15);
			if (workspace.IsEmpty || size > (nuint) workspace.Length || (nuint) (aligned - pointer) > (nuint) workspace.Length - size)
				throw new ArgumentException ("The workspace is too small, including the required alignment padding.", nameof (workspace));
			return aligned;
		}

		[DllImport ("__Internal")]
		static extern IntPtr xamarin_sparse_error ();
		[DllImport ("__Internal")]
		static extern byte xamarin_sparse_allocation_failed ();
		[DllImport ("__Internal")]
		internal static extern void xamarin_sparse_release (IntPtr handle);
		[DllImport ("__Internal")]
		internal static extern IntPtr xamarin_sparse_matrix_retain (IntPtr handle);
		[DllImport ("__Internal")]
		internal static extern IntPtr xamarin_sparse_storage_create (nuint size, void** data);
		[DllImport ("__Internal")]
		internal static extern IntPtr xamarin_sparse_matrix_create (int scalar, Matrix* matrix, long count);
		[DllImport ("__Internal")]
		internal static extern IntPtr xamarin_sparse_coordinate (int scalar, Matrix* matrix, long count, int* rows, int* columns, void* values);
		[DllImport ("__Internal")]
		internal static extern IntPtr xamarin_sparse_from_opaque (int scalar, IntPtr matrix, Matrix* descriptor);
		[DllImport ("__Internal")]
		internal static extern void xamarin_sparse_matrix_multiply (int scalar, Matrix* matrix, int count, void* input, void* output, void* alpha, byte accumulate);
		[DllImport ("__Internal")]
		internal static extern IntPtr xamarin_sparse_symbolic_create (int scalar, byte type, Matrix* matrix, Options* options, nuint* sizes);
		[DllImport ("__Internal")]
		internal static extern IntPtr xamarin_sparse_factor_create (int scalar, byte type, Matrix* matrix, Options* options, IntPtr symbolic, void* storage, void* workspace, int* status);
		[DllImport ("__Internal")]
		internal static extern nuint xamarin_sparse_solve_size (int scalar, IntPtr handle, int count, byte subfactor);
		[DllImport ("__Internal")]
		internal static extern void xamarin_sparse_direct_solve (int scalar, IntPtr handle, int rows, int columns, int count, void* rhs, void* solution, void* workspace, int operation);
		[DllImport ("__Internal")]
		internal static extern IntPtr xamarin_sparse_factor_view (int scalar, IntPtr handle, int operation, int* subfactor);
		[DllImport ("__Internal")]
		internal static extern void xamarin_sparse_refactor (int scalar, IntPtr handle, Matrix* matrix, Options* options, void* workspace, int* status);
		[DllImport ("__Internal")]
		internal static extern nuint xamarin_sparse_refactor_size (int scalar, IntPtr handle);
		[DllImport ("__Internal")]
		internal static extern int xamarin_sparse_inertia (int scalar, IntPtr handle, int* positive, int* zero, int* negative);
		[DllImport ("__Internal")]
		internal static extern void xamarin_sparse_update (int scalar, IntPtr handle, int count, int* indices, Matrix* matrix, int* status);
		[DllImport ("__Internal")]
		internal static extern IntPtr xamarin_sparse_preconditioner_create (int scalar, Matrix* matrix, int type, UserPreconditioner* user);
		[DllImport ("__Internal")]
		internal static extern int xamarin_sparse_iterative_solve (int scalar, IterationOptions* options, Matrix* matrix, IntPtr callback, IntPtr user, int rows, int columns, int count, void* rhs, void* solution, IntPtr preconditioner);
		[DllImport ("__Internal")]
		internal static extern nuint xamarin_sparse_state_size (int scalar, IterationOptions* options, byte preconditioner, int rows, int columns, int count);
		[DllImport ("__Internal")]
		internal static extern void xamarin_sparse_iterate (int scalar, IterationOptions* options, int iteration, byte* converged, void* state, IntPtr callback, IntPtr user, int rows, int columns, int count, void* rhs, void* residual, int residualStride, void* solution, IntPtr preconditioner);

		[UnmanagedCallersOnly]
		internal static void Apply (IntPtr context, byte accumulate, int operation, int inputRows, int inputColumns, int inputStride, void* input, int outputRows, int outputColumns, int outputStride, void* output)
		{
			if (GCHandle.FromIntPtr (context).Target is not SparseCallback callback)
				Environment.FailFast ("Invalid sparse operator callback context.");
			else {
				try {
					callback.Apply (accumulate != 0, (SparseOperation) operation, inputRows, inputColumns, inputStride, input, outputRows, outputColumns, outputStride, output);
				} catch (Exception exception) {
					callback.Error ??= ExceptionDispatchInfo.Capture (exception);
				}
			}
		}

		[UnmanagedCallersOnly]
		internal static void Report (IntPtr context, IntPtr message)
		{
			if (GCHandle.FromIntPtr (context).Target is not SparseCallback callback)
				Environment.FailFast ("Invalid sparse status callback context.");
			else {
				try {
					callback.Report?.Invoke (Marshal.PtrToStringUTF8 (message) ?? "");
				} catch (Exception exception) {
					callback.Error ??= ExceptionDispatchInfo.Capture (exception);
				}
			}
		}

		internal static IntPtr ApplyPointer => (IntPtr) (delegate* unmanaged<IntPtr, byte, int, int, int, int, void*, int, int, int, void*, void>) &Apply;
		internal static IntPtr ReportPointer => (IntPtr) (delegate* unmanaged<IntPtr, IntPtr, void>) &Report;
	}

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	sealed class SparseHandle : SafeHandleZeroOrMinusOneIsInvalid {
		readonly SparseHandle? dependency;
		readonly Action? cleanup;
		internal SparseHandle (IntPtr handle, SparseHandle? dependency = null, Action? cleanup = null) : base (true)
		{
			if (handle == IntPtr.Zero)
				throw new OutOfMemoryException ("Could not allocate sparse solver storage.");
			if (dependency is not null) {
				bool retained = false;
				dependency.DangerousAddRef (ref retained);
				this.dependency = dependency;
			}
			SetHandle (handle);
			this.cleanup = cleanup;
		}

		protected override bool ReleaseHandle ()
		{
			SparseNative.xamarin_sparse_release (handle);
			cleanup?.Invoke ();
			dependency?.DangerousRelease ();
			return true;
		}
	}

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	ref struct SparseLease {
		readonly SparseHandle? handle;
		internal IntPtr Value { get; }
		internal SparseLease (SparseHandle? handle)
		{
			this.handle = handle;
			Value = IntPtr.Zero;
			if (handle is not null) {
				bool retained = false;
				handle.DangerousAddRef (ref retained);
				Value = handle.DangerousGetHandle ();
			}
		}
		public void Dispose () => handle?.DangerousRelease ();
	}

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	abstract unsafe class SparseCallback : IDisposable {
		readonly GCHandle context;
		internal ExceptionDispatchInfo? Error;
		internal Action<string>? Report;
		internal IntPtr Context => GCHandle.ToIntPtr (context);
		protected SparseCallback () => context = GCHandle.Alloc (this);
		internal abstract void Apply (bool accumulate, SparseOperation operation, int inputRows, int inputColumns, int inputStride, void* input, int outputRows, int outputColumns, int outputStride, void* output);
		public void Dispose () => context.Free ();
	}
}
