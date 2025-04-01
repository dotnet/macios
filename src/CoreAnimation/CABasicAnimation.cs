using System;

using Foundation;
using ObjCRuntime;
#if MONOMAC
using AppKit;
#endif
using CoreGraphics;

#nullable enable

namespace CoreAnimation {
	public partial class CABasicAnimation {
		/// <typeparam name="T">To be added.</typeparam>
		///         <summary>Returns the initial value for the property to animate, returned as an object of the specified type.</summary>
		///         <returns>
		///           <para>
		///           </para>
		///         </returns>
		///         <remarks>
		///           <para>
		///           </para>
		///         </remarks>
		public T GetFromAs<T> () where T : class, INativeObject
		{
			return Runtime.GetINativeObject<T> (_From, false)!;
		}

		/// <param name="value">
		///           <para>Initial value that the property will have.</para>
		///           <para>If you want to set the value to null, use the From property.</para>
		///           <para>
		///           </para>
		///         </param>
		///         <summary>Sets the value for the initial value of the property to animate, by using a non-NSObject type.</summary>
		///         <remarks>
		///           <para>
		///           </para>
		///         </remarks>
		public void SetFrom (INativeObject value)
		{
			_From = value.Handle;
			GC.KeepAlive (value);
		}

		/// <typeparam name="T">To be added.</typeparam>
		///         <summary>Returns the destination value for the property to animate, returned as an object of the specified type.</summary>
		///         <returns>
		///           <para>
		///           </para>
		///         </returns>
		///         <remarks>
		///           <para>
		///           </para>
		///         </remarks>
		public T GetToAs<T> () where T : class, INativeObject
		{
			return Runtime.GetINativeObject<T> (_To, false)!;
		}

		/// <param name="value">
		///           <para>Final value that the property will have.</para>
		///           <para tool="nullallowed">If you want to set the value to null, use the property To.</para>
		///           <para>
		///           </para>
		///         </param>
		///         <summary>Destination value for the property to animate (using INativeObject).</summary>
		///         <remarks>
		///         </remarks>
		public void SetTo (INativeObject value)
		{
			_To = value.Handle;
			GC.KeepAlive (value);
		}

		/// <typeparam name="T">To be added.</typeparam>
		///         <summary>Returns the value to increment by, returned as an object of the specified type.</summary>
		///         <returns>
		///           <para />
		///         </returns>
		///         <remarks>
		///           <para />
		///         </remarks>
		public T GetByAs<T> () where T : class, INativeObject
		{
			return Runtime.GetINativeObject<T> (_By, false)!;
		}

		/// <param name="value">
		///           <para />
		///         </param>
		///         <summary>Sets the value to increment by, by using a non-NSObject type.</summary>
		///         <remarks>
		///           <para />
		///         </remarks>
		public void SetBy (INativeObject value)
		{
			_By = value.Handle;
			GC.KeepAlive (value);
		}
	}
}
