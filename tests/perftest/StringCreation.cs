using System;
using System.Runtime.InteropServices;
using System.Text;

using Foundation;
using ObjCRuntime;

using BenchmarkDotNet.Attributes;

using Bindings.Test;

namespace PerfTest {
	public class StringCreation {
		string input;

        [Params (1, 10, 100, 1000, 10000)]
        public int StringLength { get; set; }

		[GlobalSetup]
		public void GlobalSetup ()
		{
			var sb = new StringBuilder (StringLength);
			for (var i = 0; i < StringLength; i++)
				sb.Append ((char) ((byte) 'a' + (i % 20)));
			input = sb.ToString ();
		}

		/*
		 * old style
		 */
		NativeHandle old_style;

		[Benchmark]
		public NativeHandle OldStyle ()
		{
			return old_style = NSStringOldStyle.CreateNative (input);
		}

		[IterationCleanup (Target = nameof (OldStyle))]
		public void OldStyle_Cleanup ()
		{
			NSString.ReleaseNative (old_style);
			old_style = NativeHandle.Zero;
		}

		/*
		 * new style
		 */
		NativeHandle new_style;

		[Benchmark]
		public NativeHandle NewStyle ()
		{
			return new_style = NSStringNewStyle.CreateNative (input);
		}

		[IterationCleanup (Target = nameof (NewStyle))]
		public void NewStyle_Cleanup ()
		{
			NSString.ReleaseNative (new_style);
			new_style = NativeHandle.Zero;
		}

		/*
		 * CFString style
		 */
		NativeHandle cfstring;

		[Benchmark]
		public NativeHandle CFString ()
		{
			return cfstring = CoreFoundation.CFString.CreateNative (input);
		}

		[IterationCleanup (Target = nameof (CFString))]
		public void CFString_Cleanup ()
		{
			CoreFoundation.CFString.ReleaseNative (cfstring);
			cfstring = NativeHandle.Zero;
		}
	}

	class NSStringOldStyle {
		static IntPtr class_ptr = Class.GetHandle ("NSString");
		const string selInitWithCharactersLength = "initWithCharacters:length:";

		static IntPtr selInitWithCharactersLengthHandle = Selector.GetHandle (selInitWithCharactersLength);
		static NativeHandle CreateWithCharacters (NativeHandle handle, string str, int offset, int length, bool autorelease = false)
		{
			unsafe {
				fixed (char* ptrFirstChar = str) {
					var ptrStart = (IntPtr) (ptrFirstChar + offset);
#if MONOMAC
					handle = Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr (handle, selInitWithCharactersLengthHandle, ptrStart, (IntPtr) length);
#else
					handle = Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr (handle, Selector.GetHandle (selInitWithCharactersLength), ptrStart, (IntPtr) length);
#endif

					if (autorelease)
						Messaging.void_objc_msgSend (handle, Selector.GetHandle ("autorelease"));

					return handle;
				}
			}
		}

		public static NativeHandle CreateNative (string str)
		{
			return CreateNative (str, false);
		}

		public static NativeHandle CreateNative (string str, bool autorelease)
		{
			if (str is null)
				return NativeHandle.Zero;

			return CreateNative (str, 0, str.Length, autorelease);
		}

		public static NativeHandle CreateNative (string value, int start, int length)
		{
			return CreateNative (value, start, length, false);
		}

		public static NativeHandle CreateNative (string value, int start, int length, bool autorelease)
		{
			if (value is null)
				return NativeHandle.Zero;

			if (start < 0 || start > value.Length)
				throw new ArgumentOutOfRangeException (nameof (start));

			if (length < 0 || start > value.Length - length)
				throw new ArgumentOutOfRangeException (nameof (length));

#if MONOMAC
			var handle = Messaging.IntPtr_objc_msgSend (class_ptr, Selector.AllocHandle);
#else
			var handle = Messaging.IntPtr_objc_msgSend (class_ptr, Selector.GetHandle ("alloc"));
#endif

			return CreateWithCharacters (handle, value, start, length, autorelease);
		}
	}

	class NSStringNewStyle {
		static IntPtr class_ptr = Class.GetHandle ("NSString");
		static NativeHandle CreateWithCharacters (string str, int start, int length, bool autorelease = false, bool allowNull = false)
		{
			if (str is null) {
				if (allowNull)
					return NativeHandle.Zero;
				throw new ArgumentNullException (nameof (str));
			}

			if (start < 0 || start > str.Length)
				throw new ArgumentOutOfRangeException (nameof (start));

			if (length < 0 || start > str.Length - length)
				throw new ArgumentOutOfRangeException (nameof (length));

			unsafe {
				fixed (char* ptrFirstChar = str) {
					var ptrStart = (IntPtr) (ptrFirstChar + start);
					var handle = Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr (class_ptr, Selector.GetHandle ("stringWithCharacters:length:"), ptrStart, (IntPtr) length);

					if (!autorelease)
						Messaging.void_objc_msgSend (handle, Selector.GetHandle ("retain"));

					return handle;
				}
			}
		}

		public static NativeHandle CreateNative (string str)
		{
			return CreateNative (str, false);
		}

		public static NativeHandle CreateNative (string str, bool autorelease)
		{
			if (str is null)
				return NativeHandle.Zero;

			return CreateNative (str, 0, str.Length, autorelease);
		}

		public static NativeHandle CreateNative (string value, int start, int length)
		{
			return CreateNative (value, start, length, false);
		}

		public static NativeHandle CreateNative (string value, int start, int length, bool autorelease)
		{
			return CreateWithCharacters (value, start, length, autorelease, allowNull: true);
		}
	}
}
