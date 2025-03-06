//
// SystemLog.cs: Bindings to the kqueue(3) API from Apple
//
// Authors:
//  Miguel de Icaza (miguel@xamarin.com)
//
// Copyright 2012, Xamarin Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#nullable enable

#if MONOMAC

using System;
using CoreFoundation;
using ObjCRuntime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections.Generic;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace Darwin {
	[StructLayout (LayoutKind.Sequential)]
	public struct KernelEvent {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public IntPtr /* uintptr_tr */ Ident;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public EventFilter /* int16_t */ Filter;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public EventFlags /* uint16_t */ Flags;
#if NET
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public FilterFlags /* uint32_t */ FilterFlags;
#else
		public uint /* uint32_t */ FilterFlags;
#endif
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public IntPtr /* uintptr_t */ Data;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public IntPtr /* void */ UserData;
	}

	[Flags]
	public enum EventFlags : ushort {
		/// <summary>To be added.</summary>
		Add = 0x0001,
		/// <summary>To be added.</summary>
		Delete = 0x0002,
		/// <summary>To be added.</summary>
		Enable = 0x0004,
		/// <summary>To be added.</summary>
		Disable = 0x0008,
		/// <summary>To be added.</summary>
		OneShot = 0x0010,
		/// <summary>To be added.</summary>
		Clear = 0x0020,
		/// <summary>To be added.</summary>
		Receipt = 0x0040,
		/// <summary>To be added.</summary>
		Dispatch = 0x0080,

		/// <summary>To be added.</summary>
		Flag0 = 0x1000,
		/// <summary>To be added.</summary>
		Flag1 = 0x2000,
		/// <summary>To be added.</summary>
		SystemFlags = unchecked(0xf000),

		// Return values.
		/// <summary>To be added.</summary>
		EOF = 0x8000,
		/// <summary>To be added.</summary>
		Error = 0x4000,
	}

	public enum EventFilter : short {
		/// <summary>To be added.</summary>
		Read = -1,
		/// <summary>To be added.</summary>
		Write = -2,
		/// <summary>To be added.</summary>
		Aio = -3,
		/// <summary>To be added.</summary>
		Vnode = -4,
		/// <summary>To be added.</summary>
		Proc = -5,
		/// <summary>To be added.</summary>
		Signal = -6,
		/// <summary>To be added.</summary>
		Timer = -7,
		/// <summary>To be added.</summary>
		MachPort = -8,
		/// <summary>To be added.</summary>
		FS = -9,
		/// <summary>To be added.</summary>
		User = -10,
		/// <summary>To be added.</summary>
		VM = -11,
	}

	[Flags]
	public enum FilterFlags : uint {
		/// <summary>To be added.</summary>
		ReadPoll = EventFlags.Flag0,
		/// <summary>To be added.</summary>
		ReadOutOfBand = EventFlags.Flag1,
		/// <summary>To be added.</summary>
		ReadLowWaterMark = 0x00000001,

		/// <summary>To be added.</summary>
		WriteLowWaterMark = ReadLowWaterMark,

		/// <summary>To be added.</summary>
		NoteTrigger = 0x01000000,
		/// <summary>To be added.</summary>
		NoteFFNop = 0x00000000,
		/// <summary>To be added.</summary>
		NoteFFAnd = 0x40000000,
		/// <summary>To be added.</summary>
		NoteFFOr = 0x80000000,
		/// <summary>To be added.</summary>
		NoteFFCopy = 0xc0000000,
		/// <summary>To be added.</summary>
		NoteFFCtrlMask = 0xc0000000,
		/// <summary>To be added.</summary>
		NoteFFlagsMask = 0x00ffffff,

		/// <summary>To be added.</summary>
		VNodeDelete = 0x00000001,
		/// <summary>To be added.</summary>
		VNodeWrite = 0x00000002,
		/// <summary>To be added.</summary>
		VNodeExtend = 0x00000004,
		/// <summary>To be added.</summary>
		VNodeAttrib = 0x00000008,
		/// <summary>To be added.</summary>
		VNodeLink = 0x00000010,
		/// <summary>To be added.</summary>
		VNodeRename = 0x00000020,
		/// <summary>To be added.</summary>
		VNodeRevoke = 0x00000040,
		/// <summary>To be added.</summary>
		VNodeNone = 0x00000080,

		/// <summary>To be added.</summary>
		ProcExit = 0x80000000,
		/// <summary>To be added.</summary>
		ProcFork = 0x40000000,
		/// <summary>To be added.</summary>
		ProcExec = 0x20000000,
		/// <summary>To be added.</summary>
		ProcReap = 0x10000000,
		/// <summary>To be added.</summary>
		ProcSignal = 0x08000000,
		/// <summary>To be added.</summary>
		ProcExitStatus = 0x04000000,
		/// <summary>To be added.</summary>
		ProcResourceEnd = 0x02000000,

		// iOS only
		/// <summary>To be added.</summary>
		ProcAppactive = 0x00800000,
		/// <summary>To be added.</summary>
		ProcAppBackground = 0x00400000,
		/// <summary>To be added.</summary>
		ProcAppNonUI = 0x00200000,
		/// <summary>To be added.</summary>
		ProcAppInactive = 0x00100000,
		/// <summary>To be added.</summary>
		ProcAppAllStates = 0x00f00000,

		// Masks
		/// <summary>To be added.</summary>
		ProcPDataMask = 0x000fffff,
		/// <summary>To be added.</summary>
		ProcControlMask = 0xfff00000,

		/// <summary>To be added.</summary>
		VMPressure = 0x80000000,
		/// <summary>To be added.</summary>
		VMPressureTerminate = 0x40000000,
		/// <summary>To be added.</summary>
		VMPressureSuddenTerminate = 0x20000000,
		/// <summary>To be added.</summary>
		VMError = 0x10000000,

		/// <summary>To be added.</summary>
		TimerSeconds = 0x00000001,
		/// <summary>To be added.</summary>
		TimerMicroSeconds = 0x00000002,
		/// <summary>To be added.</summary>
		TimerNanoSeconds = 0x00000004,
		/// <summary>To be added.</summary>
		TimerAbsolute = 0x00000008,
	}

	public class KernelQueue : IDisposable, INativeObject {
		int handle;

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NativeHandle Handle { get { return (NativeHandle) (IntPtr) handle; } }

		[DllImport (Constants.SystemLibrary)]
		extern static int /* int */ kqueue ();

		public KernelQueue ()
		{
			handle = kqueue ();
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		~KernelQueue ()
		{
			Dispose (false);
		}

		protected virtual void Dispose (bool disposing)
		{
			if (handle != -1) {
				DispatchSource.VnodeMonitor.close (handle);
				handle = -1;
			}
		}

		[DllImport (Constants.SystemLibrary)]
		unsafe extern static int /* int */ kevent (int kq, KernelEvent* changeList, int /* int */ nChanges, KernelEvent* eventList, int /* int */ nEvents, TimeSpec* timeout);

		public int KEvent (KernelEvent [] changeList, KernelEvent [] eventList, TimeSpan? timeout = null)
		{
			if (changeList is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (changeList));

			if (eventList is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (eventList));

			if (changeList.Length < 1)
				throw new ArgumentOutOfRangeException ("eventList must contain at least one element", nameof (eventList));

			if (eventList.Length < 1)
				throw new ArgumentOutOfRangeException ("changeList must contain at least one element", nameof (changeList));

			return KEvent (changeList, changeList.Length, eventList, eventList.Length, ToTimespec (timeout));
		}

		public unsafe int KEvent (KernelEvent [] changeList, int nChanges, KernelEvent [] eventList, int nEvents, TimeSpec? timeout = null)
		{
			if (changeList is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (changeList));

			if (eventList is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (eventList));

			if (changeList.Length < 1)
				throw new ArgumentOutOfRangeException ("eventList must contain at least one element", nameof (eventList));

			if (eventList.Length < 1)
				throw new ArgumentOutOfRangeException ("changeList must contain at least one element", nameof (changeList));

			if (changeList.Length < nChanges)
				throw new ArgumentOutOfRangeException ("nChanges is larger than the number of elements in changeList", nameof (nChanges));

			if (eventList.Length < nEvents)
				throw new ArgumentOutOfRangeException ("nEvents is larger than the number of elements in eventList", nameof (nEvents));

			unsafe {
				fixed (KernelEvent* cp = changeList)
				fixed (KernelEvent* ep = eventList) {
					if (timeout is null) {
						return kevent (handle, cp, nChanges, ep, nEvents, null);
					} else {
						TimeSpec ts = timeout.Value;
						return kevent (handle, cp, nChanges, ep, nEvents, &ts);
					}
				}
			}
		}

		static TimeSpec? ToTimespec (TimeSpan? ts)
		{
			if (ts is null)
				return null;

			var rv = new TimeSpec ();
			rv.Seconds = (nint) ts.Value.TotalSeconds;
			rv.NanoSeconds = (nint) (ts.Value.Milliseconds * 1000000L);
			return rv;
		}

		// Don't worry about nullability for !NET
#nullable disable
#if !NET
		[Obsolete ("Use any of the overloads that return an int to get how many events were returned from kevent.")]
		public bool KEvent (KernelEvent [] changeList, int nChanges, KernelEvent [] eventList, int nEvents, ref TimeSpec timeOut)
		{
			if (changeList is not null && changeList.Length < nChanges)
				throw new ArgumentException ("nChanges is larger than the number of elements in changeList");

			if (eventList is not null && eventList.Length < nEvents)
				throw new ArgumentException ("nChanges is larger than the number of elements in changeList");
			
			unsafe {
				fixed (KernelEvent *cp = changeList)
					fixed (KernelEvent *ep = eventList)
						return kevent (handle, cp, nChanges, ep, nEvents, (TimeSpec *) Unsafe.AsPointer<TimeSpec> (ref timeOut)) != -1;
			}
		}

		[Obsolete ("Use any of the overloads that return an int to get how many events were returned from kevent.")]
		public bool KEvent (KernelEvent [] changeList, int nChanges, KernelEvent [] eventList, int nEvents)
		{
			if (changeList is not null && changeList.Length < nChanges)
				throw new ArgumentException ("nChanges is larger than the number of elements in changeList");

			if (eventList is not null && eventList.Length < nEvents)
				throw new ArgumentException ("nChanges is larger than the number of elements in changeList");
			
			unsafe {
				fixed (KernelEvent *cp = changeList)
					fixed (KernelEvent *ep = eventList)
						return kevent (handle, cp, nChanges, ep, nEvents, null) != -1;
			}
		}

		[Obsolete ("Use any of the overloads that return an int to get how many events were returned from kevent.")]
		public bool KEvent (KernelEvent [] changeList, KernelEvent [] eventList, ref TimeSpec timeOut)
		{
			unsafe {
				fixed (KernelEvent *cp = changeList)
					fixed (KernelEvent *ep = eventList)
						return kevent (handle, cp, changeList?.Length ?? 0, ep, eventList?.Length ?? 0, (TimeSpec *) Unsafe.AsPointer<TimeSpec> (ref timeOut)) != -1;
			}
		}
#endif
#nullable enable

#if NET
		public int KEvent (KernelEvent [] changeList, KernelEvent [] eventList)
#else
		[Obsolete ("Use any of the overloads that return an int to get how many events were returned from kevent.")]
		public bool KEvent (KernelEvent [] changeList, KernelEvent [] eventList)
#endif
		{
			unsafe {
				fixed (KernelEvent* cp = changeList)
				fixed (KernelEvent* ep = eventList)
#if NET
					return kevent (handle, cp, changeList?.Length ?? 0, ep, eventList?.Length ?? 0, null);
#else
						return kevent (handle, cp, changeList?.Length ?? 0, ep, eventList?.Length ?? 0, null) != -1;
#endif
			}
		}
	}
}

#endif
